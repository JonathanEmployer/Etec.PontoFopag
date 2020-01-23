using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class FechamentobhdHE : IBLL<Modelo.FechamentobhdHE>
    {
        DAL.IFechamentobhdHE dalFechamentoBHDHE;
        DAL.IFechamentoBH dalFechamentoBH;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public FechamentobhdHE()
            : this(null)
        {

        }

        public FechamentobhdHE(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public FechamentobhdHE(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalFechamentoBHDHE = new DAL.SQL.FechamentobhdHE(new DataBase(ConnectionString));
            dalFechamentoBH = new DAL.SQL.FechamentoBH(new DataBase(ConnectionString));
            dalFechamentoBHDHE.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }


        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalFechamentoBHDHE.getId(pValor, pCampo, pValor2);
        }

        public int MaxCodigo()
        {
            return dalFechamentoBHDHE.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFechamentoBHDHE.GetAll();
        }

        public Modelo.FechamentobhdHE LoadObject(int id)
        {
            return dalFechamentoBHDHE.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FechamentobhdHE objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FechamentobhdHE objeto)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFechamentoBHDHE.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFechamentoBHDHE.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFechamentoBHDHE.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public List<Modelo.FechamentobhdHE> CalculaRateioSaldoBhdHE(Modelo.BancoHoras objBancoHoras, Modelo.FechamentoBH pObjFechamentoBH)
        {
            List<Modelo.FechamentobhdHE> lstFechamentobhdHE = new List<Modelo.FechamentobhdHE>();
            try
            {
                if (!String.IsNullOrEmpty(objBancoHoras.FechamentoPercentualHELimite1) && !objBancoHoras.FechamentoPercentualHELimite1.Contains("--:--"))
                {
                    List<pxyPessoaMarcacaoParaRateio> lstPxySaldoBancoHorasPDia = dalFechamentoBHDHE.GetPessoaMarcacaoParaRateio(pObjFechamentoBH.Tipo,
                                                                                                                                     Convert.ToString(pObjFechamentoBH.Identificacao),
                                                                                                                                     objBancoHoras.DataInicial.Value,
                                                                                                                                     pObjFechamentoBH.Data.Value);

                    Modelo.FechamentobhdHE objFechamentobhdHE;
                    Decimal limitePerc1, limitePerc2;
                    Decimal horasRateio = 0;

                    if ((lstPxySaldoBancoHorasPDia != null) && (lstPxySaldoBancoHorasPDia.Count > 0))
                    {
                        IEnumerable<IGrouping<int, Modelo.Proxy.pxyPessoaMarcacaoParaRateio>> lstGrupoPorFuncionario =
                        lstPxySaldoBancoHorasPDia.GroupBy(s => s.IdentificadorFuncionario);

                        limitePerc1 = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.FechamentoPercentualHELimite1);
                        limitePerc2 = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.FechamentoPercentualHELimite2);
                        foreach (var grupo in lstGrupoPorFuncionario)
                        {
                            try
                            {
                                int chaveGrupo = grupo.Key;
                                List<pxyPessoaMarcacaoParaRateio> lstPxyLocal = grupo.Select(s => s).ToList();

                                pxyPessoaMarcacaoParaRateio primeiroItemGrupo = lstPxyLocal.LastOrDefault();
                                horasRateio = primeiroItemGrupo.SaldoBancoHorasTotalDia;
                                foreach (var item in lstPxyLocal)
                                {
                                    if (horasRateio <= 0)
                                        break;
                                    else
                                    {
                                        if (item.SaldoBancoHorasDia > 0)
                                        {
                                            Decimal QuantHorasPerc1, QuantHorasPerc2;
                                            BuscaPrimeiroSegundoLimite(ref horasRateio, limitePerc1, limitePerc2, item, out QuantHorasPerc1, out QuantHorasPerc2);

                                            objFechamentobhdHE = new Modelo.FechamentobhdHE();
                                            objFechamentobhdHE.IdMarcacao = item.IdentificadorMarcacao;
                                            objFechamentobhdHE.IdFechamentoBH = pObjFechamentoBH.Id;
                                            objFechamentobhdHE.Codigo = dalFechamentoBHDHE.MaxCodigo();
                                            objFechamentobhdHE.QuantHorasPerc1 = Modelo.cwkFuncoes.ConvertMinutosHora(QuantHorasPerc1);
                                            objFechamentobhdHE.QuantHorasPerc2 = Modelo.cwkFuncoes.ConvertMinutosHora(QuantHorasPerc2);
                                            objFechamentobhdHE.PercQuantHorasPerc1 = String.IsNullOrEmpty(objBancoHoras.FechamentoPercentualHEPercentual1) ? 0 : Convert.ToInt32(objBancoHoras.FechamentoPercentualHEPercentual1);
                                            objFechamentobhdHE.PercQuantHorasPerc2 = String.IsNullOrEmpty(objBancoHoras.FechamentoPercentualHEPercentual2) ? 0 : Convert.ToInt32(objBancoHoras.FechamentoPercentualHEPercentual2);

                                            lstFechamentobhdHE.Add(objFechamentobhdHE);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                                throw ex;
                            }
                        }

                    } 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            };

            return lstFechamentobhdHE;
        }

        public IList<Modelo.FechamentobhdHE> GetFechamentobhdHEPorIdFechamentoBH(int idfechamentobh, int identificacao)
        {
            return dalFechamentoBHDHE.GetFechamentobhdHEPorIdFechamentoBH(idfechamentobh, identificacao);
        }

        private void BuscaPrimeiroSegundoLimite(ref Decimal horasRateio, decimal limitePerc1, decimal limitePerc2, pxyPessoaMarcacaoParaRateio item,
                                                                  out  Decimal QuantHorasPerc1, out Decimal QuantHorasPerc2)
        {
            QuantHorasPerc1 = 0;
            QuantHorasPerc2 = 0;

            Modelo.FechamentobhdHE objFechamentobhdHE = new Modelo.FechamentobhdHE();

            if ((item.SaldoBancoHorasDia + (item.SaldoBancoHorasTotal - horasRateio)) >= limitePerc1) //Existem horas trabalhadas que estão no segundo limite
            {
                Decimal horasTrabPrimeiroLimite = limitePerc1 - (item.SaldoBancoHorasTotal - horasRateio);
                horasTrabPrimeiroLimite = horasTrabPrimeiroLimite < 0 ? 0 : horasTrabPrimeiroLimite;
                Decimal horasTrabSegundoLimite = (item.SaldoBancoHorasDia - horasTrabPrimeiroLimite);

                horasTrabSegundoLimite = horasTrabSegundoLimite > limitePerc2 ? limitePerc2 : horasTrabSegundoLimite; //Verifica se as horas trabalhadas no segundo limite são maiores ou não que o limite 2

                if (item.SaldoBancoHorasDia > horasRateio)
                {
                    QuantHorasPerc2 = horasTrabSegundoLimite > horasRateio ? horasRateio : horasTrabSegundoLimite;
                    horasRateio -= QuantHorasPerc2;

                    if (horasRateio > 0)
                    {
                        QuantHorasPerc1 = horasTrabPrimeiroLimite > horasRateio ? horasRateio : horasTrabPrimeiroLimite;
                        horasRateio -= QuantHorasPerc1;
                    }
                }
                else
                {
                    QuantHorasPerc1 = horasTrabPrimeiroLimite;
                    QuantHorasPerc2 = horasTrabSegundoLimite;

                    horasRateio -= (QuantHorasPerc1 + QuantHorasPerc2);
                }
            }
            else
            {
                QuantHorasPerc1 = item.SaldoBancoHorasDia > horasRateio ? horasRateio : item.SaldoBancoHorasDia;

                horasRateio -= (QuantHorasPerc1 + QuantHorasPerc2);
            }

        }

        public DataTable GetRelatorioFechamentoPercentualHESintetico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            return dalFechamentoBHDHE.GetRelatorioFechamentoPercentualHESintetico(idSelecionados, inicioPeriodo, fimPeriodo);
        }

        public DataTable GetRelatorioFechamentoPercentualHEAnalitico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            return dalFechamentoBHDHE.GetRelatorioFechamentoPercentualHEAnalitico(idSelecionados, inicioPeriodo, fimPeriodo);
        }

        public List<pxyFechamentobhdHEAnalitico> BuscaFechamentobhdHEAnaliticoPrevia(Modelo.BancoHoras objBancoHoras, int tipoFechamento, String identificdoresFechamento,
                                                                  DateTime DataInicial, DateTime DataFinal)
        {
            List<pxyFechamentobhdHEAnalitico> lstPxyFechamentobhdHEAnalitico = new List<pxyFechamentobhdHEAnalitico>();
            try
            {
                List<pxyPessoaMarcacaoParaRateio> lstPxySaldoBancoHorasPDia = dalFechamentoBHDHE.GetPessoaMarcacaoParaRateio(tipoFechamento,
                                                                                                                         identificdoresFechamento,
                                                                                                                         DataInicial,
                                                                                                                         DataFinal);

                pxyFechamentobhdHEAnalitico objPxyFechamentobhdHEAnalitico;
                Decimal limitePerc1, limitePerc2;
                Decimal horasRateio = 0;

                if ((lstPxySaldoBancoHorasPDia != null) && (lstPxySaldoBancoHorasPDia.Count > 0))
                {
                    IEnumerable<IGrouping<int, Modelo.Proxy.pxyPessoaMarcacaoParaRateio>> lstGrupoPorFuncionario =
                    lstPxySaldoBancoHorasPDia.GroupBy(s => s.IdentificadorFuncionario);

                    limitePerc1 = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.FechamentoPercentualHELimite1);
                    limitePerc2 = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.FechamentoPercentualHELimite2);
                    foreach (var grupo in lstGrupoPorFuncionario)
                    {
                        int chaveGrupo = grupo.Key;

                        List<pxyPessoaMarcacaoParaRateio> lstPxyLocal = grupo.Select(s => s).Reverse().ToList();

                        pxyPessoaMarcacaoParaRateio primeiroItemGrupo = lstPxyLocal.FirstOrDefault();
                        horasRateio = primeiroItemGrupo.SaldoBancoHorasTotal;

                        foreach (var item in lstPxyLocal)
                        {
                            objPxyFechamentobhdHEAnalitico = new pxyFechamentobhdHEAnalitico();

                            objPxyFechamentobhdHEAnalitico.Matricula = item.MatriculaFuncionario;
                            objPxyFechamentobhdHEAnalitico.Alocacao = item.Alocacao;
                            objPxyFechamentobhdHEAnalitico.Departamento = item.Departamento;
                            objPxyFechamentobhdHEAnalitico.Funcao = item.Funcao;
                            objPxyFechamentobhdHEAnalitico.Jornada = item.Jornada;
                            objPxyFechamentobhdHEAnalitico.DataBatida = item.DataBatida;

                            objPxyFechamentobhdHEAnalitico.Ent1 = item.Ent1;
                            objPxyFechamentobhdHEAnalitico.Sai1 = item.Sai1;
                            objPxyFechamentobhdHEAnalitico.Ent2 = item.Ent2;
                            objPxyFechamentobhdHEAnalitico.Sai2 = item.Sai2;
                            objPxyFechamentobhdHEAnalitico.Ent3 = item.Ent3;
                            objPxyFechamentobhdHEAnalitico.Sai3 = item.Sai3;
                            objPxyFechamentobhdHEAnalitico.Ent4 = item.Ent4;
                            objPxyFechamentobhdHEAnalitico.Sai4 = item.Sai4;
                            objPxyFechamentobhdHEAnalitico.CredBH = item.CredBH;
                            objPxyFechamentobhdHEAnalitico.DebBH = item.DebBH;
                            objPxyFechamentobhdHEAnalitico.SaldoBH = Modelo.cwkFuncoes.ConvertMinutosHora(item.SaldoBancoHorasTotalDia);
                            objPxyFechamentobhdHEAnalitico.Supervisor = item.Supervisor;
                            objPxyFechamentobhdHEAnalitico.Ocorrencia = item.Ocorrencia;
                            objPxyFechamentobhdHEAnalitico.CodigoFechamento = String.Empty;

                            if (horasRateio > 0 && item.SaldoBancoHorasDia > 0)
                            {
                                Decimal QuantHorasPerc1, QuantHorasPerc2;
                                BuscaPrimeiroSegundoLimite(ref horasRateio, limitePerc1, limitePerc2, item, out QuantHorasPerc1, out QuantHorasPerc2);

                                objPxyFechamentobhdHEAnalitico.Perc1 = Modelo.cwkFuncoes.ConvertMinutosHora(QuantHorasPerc1);
                                objPxyFechamentobhdHEAnalitico.Perc2 = Modelo.cwkFuncoes.ConvertMinutosHora(QuantHorasPerc2);
                            }
                            else
                            {
                                objPxyFechamentobhdHEAnalitico.Perc1 = String.Empty;
                                objPxyFechamentobhdHEAnalitico.Perc2 = String.Empty;
                            }

                            lstPxyFechamentobhdHEAnalitico.Add(objPxyFechamentobhdHEAnalitico);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            };

            return lstPxyFechamentobhdHEAnalitico;
        }

        public List<pxyFechamentobhdHESintetico> BuscaFechamentobhdHESinteticoPrevia(ref List<pxyFechamentobhdHESintetico> lstPxyFechamentobhdHESintetico,
                                                                                     Modelo.BancoHoras objBancoHoras, int tipoFechamento,
                                                                                     String identificdoresFechamento, DateTime DataInicial, DateTime DataFinal)
        {
            List<pxyFechamentobhdHESintetico> lstPxyFechamentobhdHESinteticoPrevia = new List<pxyFechamentobhdHESintetico>();
            try
            {
                List<pxyPessoaMarcacaoParaRateio> lstPxySaldoBancoHorasPDia = dalFechamentoBHDHE.GetPessoaMarcacaoParaRateio(tipoFechamento,
                                                                                                                            identificdoresFechamento,
                                                                                                                            DataInicial,
                                                                                                                            DataFinal);

                pxyFechamentobhdHESintetico objPxyFechamentobhdHESintetico;
                Decimal limitePerc1, limitePerc2;
                Decimal accPerc1, accPerc2; // Acumuladores para se chegar aos totais dos percentuais por usuário no período

                Decimal horasRateio = 0;

                if ((lstPxySaldoBancoHorasPDia != null) && (lstPxySaldoBancoHorasPDia.Count > 0))
                {
                    IEnumerable<IGrouping<int, Modelo.Proxy.pxyPessoaMarcacaoParaRateio>> lstGrupoPorFuncionario =
                    lstPxySaldoBancoHorasPDia.GroupBy(s => s.IdentificadorFuncionario);

                    limitePerc1 = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.FechamentoPercentualHELimite1);
                    limitePerc2 = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.FechamentoPercentualHELimite2);

                    foreach (var grupo in lstGrupoPorFuncionario)
                    {
                        accPerc1 = 0;
                        accPerc2 = 0;

                        objPxyFechamentobhdHESintetico = new pxyFechamentobhdHESintetico();

                        int chaveGrupo = grupo.Key; // Pega o id do funcionário
                        List<pxyPessoaMarcacaoParaRateio> lstPxyLocal = grupo.Select(s => s).Reverse().ToList(); // Busca a lista de marcações

                        pxyPessoaMarcacaoParaRateio primeiroItemGrupo = lstPxyLocal.FirstOrDefault();
                        horasRateio = primeiroItemGrupo.SaldoBancoHorasTotal;

                        foreach (var item in lstPxyLocal)
                        {
                            if (horasRateio <= 0)
                                break;
                            else
                            {
                                if (item.SaldoBancoHorasDia > 0)
                                {
                                    Decimal QuantHorasPerc1, QuantHorasPerc2;
                                    BuscaPrimeiroSegundoLimite(ref horasRateio, limitePerc1, limitePerc2, item, out QuantHorasPerc1, out QuantHorasPerc2);

                                    accPerc1 += QuantHorasPerc1;
                                    accPerc2 += QuantHorasPerc2;
                                }
                            }
                        }

                        objPxyFechamentobhdHESintetico.Periodo = primeiroItemGrupo.Periodo;
                        objPxyFechamentobhdHESintetico.Nome = primeiroItemGrupo.NomeFuncionario;
                        objPxyFechamentobhdHESintetico.Matricula = primeiroItemGrupo.MatriculaFuncionario;
                        objPxyFechamentobhdHESintetico.Alocacao = primeiroItemGrupo.Alocacao;
                        objPxyFechamentobhdHESintetico.Departamento = primeiroItemGrupo.Departamento;
                        objPxyFechamentobhdHESintetico.Funcao = primeiroItemGrupo.Funcao;

                        objPxyFechamentobhdHESintetico.Horario = primeiroItemGrupo.Horario;

                        decimal totalCred = lstPxyLocal.Sum(s => Modelo.cwkFuncoes.ConvertHorasMinuto(s.CredBH));
                        decimal totalDeb = lstPxyLocal.Sum(s => Modelo.cwkFuncoes.ConvertHorasMinuto(s.DebBH));

                        objPxyFechamentobhdHESintetico.SaldoBH = Modelo.cwkFuncoes.ConvertMinutosHora(primeiroItemGrupo.SaldoBancoHorasTotal);

                        TentaPreencherCamposFechamento(accPerc1, accPerc2, totalCred, totalDeb, ref lstPxyFechamentobhdHESintetico, ref objPxyFechamentobhdHESintetico);

                        lstPxyFechamentobhdHESinteticoPrevia.Add(objPxyFechamentobhdHESintetico);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            };

            return lstPxyFechamentobhdHESinteticoPrevia;
        }

        private void TentaPreencherCamposFechamento(Decimal accPerc1, Decimal accPerc2, Decimal totalCred, Decimal totalDeb,
                                                    ref List<pxyFechamentobhdHESintetico> lstPxyFechamentobhdHESintetico, 
                                                    ref pxyFechamentobhdHESintetico objPxyFechamentobhdHESintetico)
        {
            String matricula = objPxyFechamentobhdHESintetico.Matricula;
            String codigoFechamento = "";
            String dataFechamento =String.Empty;

            pxyFechamentobhdHESintetico objPxyFechado = lstPxyFechamentobhdHESintetico.Where(s => s.Matricula == matricula).FirstOrDefault();

            if (objPxyFechado != null) // Existe fechamento para esta matricula no período informado no relatório
            {
		        accPerc1 += Modelo.cwkFuncoes.ConvertHorasMinuto(objPxyFechado.Perc1);
                accPerc2 += Modelo.cwkFuncoes.ConvertHorasMinuto(objPxyFechado.Perc2);

                totalCred += Modelo.cwkFuncoes.ConvertHorasMinuto(objPxyFechado.CredBH);
                totalDeb += Modelo.cwkFuncoes.ConvertHorasMinuto(objPxyFechado.DebBH);

                codigoFechamento = objPxyFechado.CodigoFechamento;
                dataFechamento = objPxyFechado.DataFechamento;

                lstPxyFechamentobhdHESintetico.Remove(objPxyFechado);
            }

            objPxyFechamentobhdHESintetico.Perc1 = Modelo.cwkFuncoes.ConvertMinutosHora2(3, accPerc1);
            objPxyFechamentobhdHESintetico.Perc2 = Modelo.cwkFuncoes.ConvertMinutosHora2(3, accPerc2);
            
            objPxyFechamentobhdHESintetico.CredBH = Modelo.cwkFuncoes.ConvertMinutosHora2(3, totalCred);
            objPxyFechamentobhdHESintetico.DebBH = Modelo.cwkFuncoes.ConvertMinutosHora2(3, totalDeb);
            
            objPxyFechamentobhdHESintetico.CodigoFechamento = codigoFechamento;
            objPxyFechamentobhdHESintetico.DataFechamento = dataFechamento;
        }
    }
}
