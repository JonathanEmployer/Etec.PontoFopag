using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class Horario : IBLL<Modelo.Horario>
    {
        DAL.IHorario dalHorario;
        private string ConnectionString;
        private DAL.IHorarioDetalhe dalHDetalhe;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Horario()
            : this(null)
        {

        }

        public Horario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Horario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    DataBase db = new DataBase(ConnectionString);
                    dalHorario = new DAL.SQL.Horario(db);
                    dalHDetalhe = new DAL.SQL.HorarioDetalhe(db);
                    break;
                case 2:
                    dalHorario = DAL.FB.Horario.GetInstancia;
                    dalHDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
                    break;
            }
            dalHorario.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalHorario.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalHorario.GetAll();
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipo, string pIds)
        {
            return dalHorario.GetAllList(carregaHorarioDetalhe, carregaPercentuais, tipo, pIds);
        }

        public List<int> GetIds()
        {
            return dalHorario.GetIds();
        }

        /// <summary>
        /// Retorna uma tabela hash onde o código é a chave e o id é o valor
        /// </summary>
        /// <returns>Tabela Hash(Código, Id)</returns>
        public Hashtable GetHashCodigoId()
        {
            return dalHorario.GetHashCodigoId();
        }

        public Hashtable GetHashCodigoIdNormal()
        {
            return dalHorario.GetHashCodigoIdNormal();
        }

        public Hashtable GetHashCodigoIdFlexivel()
        {
            return dalHorario.GetHashCodigoIdFlexivel();
        }

        public DataTable GetPorDescricao(string pHorarios)
        {
            return dalHorario.GetPorDescricao(pHorarios);
        }

        public int? GetIdPorCodigo(int Cod, bool validaPermissaoUser)
        {
            return dalHorario.GetIdPorCodigo(Cod, validaPermissaoUser);
        }

        public DataTable GetHorarioNormal()
        {
            return dalHorario.GetHorarioNormal();
        }

        public DataTable GetHorarioMovel()
        {
            return dalHorario.GetHorarioMovel();
        }

        public List<Modelo.Horario> GetHorarioNormalMovelList(int tipoHorario, bool validaPermissaoUser)
        {
            return dalHorario.GetHorarioNormalMovelList(tipoHorario, validaPermissaoUser);
        }

        public List<Modelo.Horario> getPorParametro(int pIdParametro)
        {
            return dalHorario.getPorParametro(pIdParametro);
        }

        public Modelo.Horario LoadObject(int id)
        {
            return dalHorario.LoadObject(id);
        }

        public List<Modelo.Proxy.PxyGridHorarioFlexivel> GetAllGrid(int tipoHorario)
        {
            return dalHorario.GetAllGrid(tipoHorario);
        }

        public List<Modelo.Horario> getTodosHorariosDaEmpresa(int pIdEmpresa)
        {
            return dalHorario.getTodosHorariosDaEmpresa(pIdEmpresa);
        }

        public List<Modelo.Horario> GetParaIncluirMarcacao(Hashtable ids, bool carregaHorarioDetalhe)
        {
            return dalHorario.GetParaIncluirMarcacao(ids, carregaHorarioDetalhe);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Horario objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            if (objeto.Idparametro == 0)
            {
                ret.Add("cbIdparametro", "Campo obrigatório.");
            }

            if (objeto.Limitemin == "--:--" || String.IsNullOrEmpty(objeto.Limitemin))
            {
                ret.Add("txtLimitemin", "Campo obrigatório.");
            }

            if (objeto.Limitemax == "--:--" || String.IsNullOrEmpty(objeto.Limitemax))
            {
                ret.Add("txtLimitemax", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.QtdHEPreClassificadas) && objeto.IdClassificacao.GetValueOrDefault() > 0)
            {
                ret.Add("QtdHEPreClassificadas", "Quando informado a Classificação, deve ser preenchido a Quantidade de Horas!");
            }
            else
            {
                if (objeto.IdClassificacao.GetValueOrDefault() == 0 && !String.IsNullOrEmpty(objeto.QtdHEPreClassificadas))
                {
                    ret.Add("DescClassificacao", "Quando informado a Quantidade de Horas, deve ser preenchido a Classificação!");
                }
            }

            if (objeto.TipoHorario == 1)
            {
                bool possuiHorario = false;
                foreach (Modelo.HorarioDetalhe hd in objeto.HorariosDetalhe)
                {
                    if (hd.Entrada_1 != "--:--" && hd.Saida_1 != "--:--")
                    {
                        possuiHorario = true;
                        break;
                    }
                }

                if (!possuiHorario)
                {
                    ret.Add("txtEntrada_1", "Não é possível cadastrar um horário sem definir os horários das marcações.");
                }
            }
            else
            {
                //Verifica se foi gerado horários (Turno)
                //CRNC - 16/01/2010
                if (objeto.HorariosFlexiveis.Where(h => h.Acao != Modelo.Acao.Excluir).Count() == 0)
                {
                    ret.Add("txtEntrada_1", "Não é possível cadastrar um horário sem gerar os horários das marcações.");
                }
            }

            if (Convert.ToBoolean(objeto.HorariosPHExtra[6].Marcapercentualextra))
            {
                if (objeto.HorariosPHExtra[6].TipoAcumulo < 1 || objeto.HorariosPHExtra[6].TipoAcumulo > 3)
                {
                    ret.Add("cbTipoAcumuloSab", "Campo Obrigatório.");
                }
            }
            if (Convert.ToBoolean(objeto.HorariosPHExtra[7].Marcapercentualextra))
            {
                if (objeto.HorariosPHExtra[7].TipoAcumulo < 1 || objeto.HorariosPHExtra[7].TipoAcumulo > 3)
                {
                    ret.Add("cbTipoAcumuloDom", "Campo Obrigatório.");
                }
            }
            if (Convert.ToBoolean(objeto.HorariosPHExtra[8].Marcapercentualextra))
            {
                if (objeto.HorariosPHExtra[8].TipoAcumulo < 1 || objeto.HorariosPHExtra[8].TipoAcumulo > 3)
                {
                    ret.Add("cbTipoAcumuloFer", "Campo Obrigatório.");
                }
            }
            if (Convert.ToBoolean(objeto.HorariosPHExtra[9].Marcapercentualextra))
            {
                if (objeto.HorariosPHExtra[9].TipoAcumulo < 1 || objeto.HorariosPHExtra[9].TipoAcumulo > 3)
                {
                    ret.Add("cbTipoAcumuloFol", "Campo Obrigatório.");
                }
            }

            if (objeto.MarcaSegundaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.SegundaPercBanco))
            {
                ret.Add("txtSegundaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaTercaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.TercaPercBanco))
            {
                ret.Add("txtTercaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaQuartaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.QuartaPercBanco))
            {
                ret.Add("txtQuartaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaQuintaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.QuintaPercBanco))
            {
                ret.Add("txtQuintaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaSextaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.SextaPercBanco))
            {
                ret.Add("txtSextaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaSabadoPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.SabadoPercBanco))
            {
                ret.Add("txtSabadoPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaDomingoPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.DomingoPercBanco))
            {
                ret.Add("txtDomingoPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaFeriadoPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.FeriadoPercBanco))
            {
                ret.Add("txtFeriadoPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.MarcaFolgaPercBanco == 1 &&
                string.IsNullOrEmpty(objeto.FolgaPercBanco))
            {
                ret.Add("txtFolgaPercBanco", "Percentual de banco deve ser informado");
            }
            if (objeto.Id > 0 && objeto.LHorariosDetalhe != null && objeto.LHorariosDetalhe.Count() > 0)
            {
                if (objeto.TipoHorario == 1)
                {
                    if (objeto.LHorariosDetalhe != null && objeto.LHorariosDetalhe.Count > 0)
                    {
                        ValidaHorarioConflitanteRegistrosEmpregado(objeto.LHorariosDetalhe, ref ret);
                    }
                }
                else
                {
                    if (objeto.HorariosFlexiveis != null && objeto.HorariosFlexiveis.Count > 0)
                    {
                        ValidaHorarioConflitanteRegistrosEmpregado(objeto.HorariosFlexiveis, ref ret);
                    }
                }

            }

            return ret;
        }

        public void ValidaHorarioConflitanteRegistrosEmpregado(IList<Modelo.HorarioDetalhe> horariosDetalhes, ref Dictionary<string, string> ret)
        {
            List<Modelo.pxyHorarioDetalheFuncionario> hDetalhe2Registro = dalHDetalhe.HorarioDetalheSegundoRegistroPorIdHorarioDoPrimeiroRegistro(horariosDetalhes.FirstOrDefault().Idhorario);
            List<Modelo.pxyHorarioDetalheFuncionario> hdConflitantes = new List<Modelo.pxyHorarioDetalheFuncionario>();

            if (hDetalhe2Registro.Count > 0)
            {
                Modelo.pxyHorarioDetalheFuncionario hrDetalhe;
                foreach (Modelo.HorarioDetalhe hd in horariosDetalhes)
                {
                    hrDetalhe = new Modelo.pxyHorarioDetalheFuncionario();
                    hrDetalhe = hDetalhe2Registro.Where(w => w.data == hd.Data && w.dia == hd.Dia).FirstOrDefault();

                    if (hrDetalhe != null)
                    {
                        Dictionary<int, string> Saidas = new Dictionary<int, string>();
                        Saidas.Add(1, hd.Saida_1); Saidas.Add(2, hd.Saida_2); Saidas.Add(3, hd.Saida_3); Saidas.Add(4, hd.Saida_4);
                        string ultimaSaida1 = Saidas.Where(s => s.Value != "--:--").OrderBy(o => o.Key).LastOrDefault().Value;

                        Saidas = new Dictionary<int, string>();
                        Saidas.Add(1, hrDetalhe.saida_1); Saidas.Add(2, hrDetalhe.saida_2); Saidas.Add(3, hrDetalhe.saida_3); Saidas.Add(4, hrDetalhe.saida_4);
                        string ultimaSaida2 = Saidas.Where(s => s.Value != "--:--").OrderBy(o => o.Key).LastOrDefault().Value;

                        if ((!String.IsNullOrEmpty(ultimaSaida1)) && (!String.IsNullOrEmpty(ultimaSaida2)))
                        {
                            TimeSpan primeiraEntradaReg1Time;
                            TimeSpan ultimaSaidaRe1Time;
                            TimeSpan primeiraEntradaReg2Time;
                            TimeSpan ultimaSaidaRe2Time;
                            DAL.SQL.MudancaHorario.ValidarHorarioEmConflito(TimeSpan.Parse(hd.Entrada_1), TimeSpan.Parse(hrDetalhe.entrada_1), ultimaSaida1, ultimaSaida2, out primeiraEntradaReg1Time, out ultimaSaidaRe1Time, out primeiraEntradaReg2Time, out ultimaSaidaRe2Time);

                            if (DAL.SQL.MudancaHorario.RetornaInterferenciaDeHorario(ref primeiraEntradaReg1Time, ref ultimaSaidaRe1Time, ref primeiraEntradaReg2Time, ref ultimaSaidaRe2Time))
                                hdConflitantes.Add(hrDetalhe);
                        }
                    }
                }
                if (hdConflitantes.Count > 0)
                    ret.Add(" ", "Alteração de horário conflitando com as jornadas informadas nos funcionários: " + String.Join("; ", hdConflitantes.Select(s => s.dscodigo + " | " + s.nome).Distinct()));
            }
        }
        
        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Horario objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalHorario.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalHorario.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalHorario.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public DataTable GetHorarioTipo(int pTipoHorario)
        {
            return dalHorario.GetHorarioTipo(pTipoHorario);
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
            return dalHorario.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais)
        {
            return dalHorario.GetAllList(carregaHorarioDetalhe, carregaPercentuais);
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipohorario, bool validaPermissaoUser)
        {
            return dalHorario.GetAllList(carregaHorarioDetalhe, carregaPercentuais, tipohorario, validaPermissaoUser);
        }


        public List<Modelo.Horario> GetAllListOrigem(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipohorario)
        {
            return dalHorario.GetAllListOrigem(carregaHorarioDetalhe, carregaPercentuais, tipohorario);
        }

        public List<Modelo.Horario> GetAllListOrigem(bool carregaHorarioDetalhe, bool carregaPercentuais)
        {
            return dalHorario.GetAllListOrigem(carregaHorarioDetalhe, carregaPercentuais);
        }

        /// <summary>
        /// Atribui os horários detalhe do horário
        /// </summary>
        /// <param name="objHorario">Horário</param>
        public void AtribuiHDHorario(Modelo.Horario objHorario)
        {
            if (objHorario.TipoHorario == 1)
            {
                objHorario.HorariosDetalhe = dalHDetalhe.LoadPorHorario(objHorario.Id).ToArray();
            }
            else
            {
                objHorario.HorariosFlexiveis = dalHDetalhe.LoadPorHorario(objHorario.Id);
            }
        }

        /// <summary>
        /// Atribui os horários detalhe do horário buscando em uma lista
        /// </summary>
        /// <param name="objHorario">Horário</param>
        /// <param name="listaHorariosDetalhe">Lista de Horários Detalhe</param>
        public void AtribuiHDHorario(Modelo.Horario objHorario, List<Modelo.HorarioDetalhe> listaHorariosDetalhe)
        {
            if (objHorario.TipoHorario == 1)
            {
                objHorario.HorariosDetalhe = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToArray();
            }
            else
            {
                objHorario.HorariosFlexiveis = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToList();
            }
        }

        public static void CalculaCafe(int[] pEntrada, int[] pSaida, short pPeriodo1, short pPeriodo2, ref int totalD, ref int totalN)
        {
            int cafe1 = 0, cafe2 = 0;

            if (pPeriodo1 == 1 && pPeriodo2 == 1)
            {
                cafe1 = Math.Abs(pEntrada[1] - pSaida[0]);
                cafe2 = Math.Abs(pEntrada[3] - pSaida[2]);
            }
            else if (pPeriodo1 == 1)
            {
                cafe1 = Math.Abs(pEntrada[1] - pSaida[0]);
            }
            else if (pPeriodo2 == 1 && pSaida[2] != -1 && pEntrada[3] != -1)
            {
                cafe2 = Math.Abs(pEntrada[3] - pSaida[2]);
            }
            else if (pPeriodo2 == 1)
            {
                cafe2 = Math.Abs(pEntrada[2] - pSaida[1]);
            }

            if (totalD > 0)
            {
                totalD = totalD + cafe1 + cafe2;
            }
            else if (totalN > 0)
            {
                totalN = totalN + cafe1 + cafe2;
            }
        }

        public static void CalculaCafe(string[] pEntrada, string[] pSaida, bool pPeriodo1, bool pPeriodo2, ref string totalD, ref string totalN)
        {
            string cafe1 = "--:--", cafe2 = "--:--";

            if (pPeriodo1 && pPeriodo2)
            {
                cafe1 = BLL.CalculoHoras.OperacaoHoras('-', pEntrada[1], pSaida[0]);
                cafe2 = BLL.CalculoHoras.OperacaoHoras('-', pEntrada[3], pSaida[2]);
            }
            else if (pPeriodo1)
            {
                cafe1 = BLL.CalculoHoras.OperacaoHoras('-', pEntrada[1], pSaida[0]);
            }
            else if (pPeriodo2 && pSaida[2] != "--:--" && pEntrada[3] != "--:--")
            {
                cafe2 = BLL.CalculoHoras.OperacaoHoras('-', pEntrada[3], pSaida[2]);
            }
            else if (pPeriodo2)
            {
                cafe2 = BLL.CalculoHoras.OperacaoHoras('-', pEntrada[2], pSaida[1]);
            }

            if (totalD != "--:--")
            {
                totalD = BLL.CalculoHoras.OperacaoHoras('+', totalD, cafe1);

                totalD = BLL.CalculoHoras.OperacaoHoras('+', totalD, cafe2);
            }
            else
            {
                totalN = BLL.CalculoHoras.OperacaoHoras('+', totalN, cafe1);

                totalN = BLL.CalculoHoras.OperacaoHoras('+', totalN, cafe2);
            }
        }

        public void InicializaHorario(ref Modelo.Horario objHorario)
        {
            objHorario = new Modelo.Horario();
            objHorario.Codigo = MaxCodigo();
            objHorario.Limitemin = null;
            objHorario.Limitemax = null;
            objHorario.Refeicao_01 = null;
            objHorario.Refeicao_02 = null;
            objHorario.Qtdhorasdsr = null;
            objHorario.Limiteperdadsr = null;
            objHorario.Tipoacumulo = 0;
            objHorario.Diasemanadsr = -1;
            objHorario.Horasnormais = 1;
            objHorario.TipoHorario = 1;
            objHorario.Descricao = "";

            objHorario.Horariodescricao_1 = "--:--";
            objHorario.Horariodescricao_2 = "--:--";
            objHorario.Horariodescricao_3 = "--:--";
            objHorario.Horariodescricao_4 = "--:--";
            objHorario.Horariodescricaosai_1 = "--:--";
            objHorario.Horariodescricaosai_2 = "--:--";
            objHorario.Horariodescricaosai_3 = "--:--";
            objHorario.Horariodescricaosai_4 = "--:--";

            InicializaHorarioDetalhe(ref objHorario);

            InicializaHorariosPHExtra(ref objHorario);
        }

        public void InicializaHorarioMovel(ref Modelo.Horario objHorario)
        {
            objHorario = new Modelo.Horario();
            objHorario.Codigo = MaxCodigo();
            objHorario.Limitemin = "--:--";
            objHorario.Limitemax = "--:--";
            objHorario.Refeicao_01 = null;
            objHorario.Refeicao_02 = null;
            objHorario.Qtdhorasdsr = null;
            objHorario.Limiteperdadsr = null;
            objHorario.Tipoacumulo = -1;
            objHorario.Diasemanadsr = -1;
            objHorario.Horasnormais = 1;
            objHorario.TipoHorario = 2;
            objHorario.Descricao = "";

            objHorario.Horariodescricao_1 = "--:--";
            objHorario.Horariodescricao_2 = "--:--";
            objHorario.Horariodescricao_3 = "--:--";
            objHorario.Horariodescricao_4 = "--:--";
            objHorario.Horariodescricaosai_1 = "--:--";
            objHorario.Horariodescricaosai_2 = "--:--";
            objHorario.Horariodescricaosai_3 = "--:--";
            objHorario.Horariodescricaosai_4 = "--:--";

            InicializaHorariosPHExtra(ref objHorario);
        }

        private void InicializaHorariosPHExtra(ref Modelo.Horario objHorario)
        {
            objHorario.HorariosPHExtra = new Modelo.HorarioPHExtra[10];

            for (int i = 0; i < objHorario.HorariosPHExtra.Length; i++)
            {
                objHorario.HorariosPHExtra[i] = new Modelo.HorarioPHExtra();
                objHorario.HorariosPHExtra[i].Codigo = i;
                objHorario.HorariosPHExtra[i].Quantidadeextra = "---:--";
            }
        }

        private void InicializaHorarioDetalhe(ref Modelo.Horario objHorario)
        {
            objHorario.HorariosDetalhe = new Modelo.HorarioDetalhe[7];
            for (int i = 0; i < objHorario.HorariosDetalhe.Length; i++)
            {
                objHorario.HorariosDetalhe[i] = new Modelo.HorarioDetalhe();
                objHorario.HorariosDetalhe[i].Acao = Modelo.Acao.Incluir;
                objHorario.HorariosDetalhe[i].Codigo = i;
                objHorario.HorariosDetalhe[i].Dia = i + 1;
                objHorario.HorariosDetalhe[i].bCarregar = 0;
                objHorario.HorariosDetalhe[i].Flagfolga = 0;
                objHorario.HorariosDetalhe[i].Data = null;
                objHorario.HorariosDetalhe[i].Totaltrabalhadadiurna = "--:--";
                objHorario.HorariosDetalhe[i].Totaltrabalhadanoturna = "--:--";
                objHorario.HorariosDetalhe[i].Cargahorariamista = "--:--";
                objHorario.HorariosDetalhe[i].Entrada_1 = "--:--";
                objHorario.HorariosDetalhe[i].Entrada_2 = "--:--";
                objHorario.HorariosDetalhe[i].Entrada_3 = "--:--";
                objHorario.HorariosDetalhe[i].Entrada_4 = "--:--";
                objHorario.HorariosDetalhe[i].Saida_1 = "--:--";
                objHorario.HorariosDetalhe[i].Saida_2 = "--:--";
                objHorario.HorariosDetalhe[i].Saida_3 = "--:--";
                objHorario.HorariosDetalhe[i].Saida_4 = "--:--";
            }
        }

        public int MinIdHorarioNormal()
        {
            return dalHorario.MinIdHorarioNormal();
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids)
        {
            return dalHorario.FechamentoPontoHorario(ids);
        }

        public List<Modelo.Proxy.pxyHistoricoAlteracaoHorario> GetHistoricoAlteracaoHorario(int id)
        {
            return dalHorario.GetHistoricoAlteracaoHorario(id);
        }

        public Modelo.Horario GetHorEntradaSaidaFunc(int idhorario)
        {
            return dalHorario.GetHorEntradaSaidaFunc(idhorario);
        }

        public void CarregaLimiteDDsr(ref List<Modelo.Horario> horarios)
        {
            if (horarios.Where(w => w.bUtilizaDDSRProporcional).Count() > 0)
            {
                BLL.LimiteDDsr bllLimiteDDsr = new BLL.LimiteDDsr(ConnectionString, UsuarioLogado);
                List<Modelo.LimiteDDsr> dsrProp = bllLimiteDDsr.GetAllListPorHorarios(horarios.Where(w => w.bUtilizaDDSRProporcional).Select(s => s.Id).ToList());
                foreach (Modelo.Horario horario in horarios)
                {
                    horario.LimitesDDsrProporcionais = dsrProp.Where(w => w.IdHorario == horario.Id).ToList();
                }
            }
        }
    }
}
