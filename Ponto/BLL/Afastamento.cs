using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Proxy.Relatorios;

namespace BLL
{
    public class Afastamento : IBLL<Modelo.Afastamento>
    {
        DAL.IAfastamento dalAfastamento;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private int tentativasNovoCodigo = 0;

        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public Afastamento() : this(null)
        {

        }

        public Afastamento(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Afastamento(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalAfastamento = new DAL.SQL.Afastamento(new DataBase(ConnectionString));
            dalAfastamento.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalAfastamento.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalAfastamento.GetAll();
        }

        public DataTable GetPorAfastamentoRel(DateTime pDataInicial, DateTime pDataFinal, string pEmpresas, string pDepartamentos, string pFuncionarios, int Tipo)
        {
            return dalAfastamento.GetPorAfastamentoRel(pDataInicial, pDataFinal, pEmpresas, pDepartamentos, pFuncionarios, Tipo);
        }

        public DataTable GetAfastamentoPorOcorrenciaRel(string pEmpresas, string pDepartamentos, string pFuncionarios, int Tipo, int idOcorrencia)
        {
            return dalAfastamento.GetAfastamentoPorOcorrenciaRel(pEmpresas, pDepartamentos, pFuncionarios, Tipo, idOcorrencia);
        }

        public Modelo.Afastamento LoadObject(int id)
        {
            return dalAfastamento.LoadObject(id);
        }

        public List<Modelo.Afastamento> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            return dalAfastamento.GetPeriodo(pDataInicial, pDataFinal);
        }

        public List<Modelo.Afastamento> GetParaExportacaoFolha(DateTime dataI, DateTime dataF, string idsOcorrencias, bool considerarAbsenteismo, List<int> IdsFuncs)
        {
            return dalAfastamento.GetParaExportacaoFolha(dataI, dataF, idsOcorrencias, considerarAbsenteismo, IdsFuncs);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Afastamento objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.BParcial == true)
            {
                if (!string.IsNullOrEmpty(objeto.Horai) && !objeto.Horai.Contains(':'))
                {
                    ret.Add("txtHorai", "Formato do horario inicial incorreto");
                }
                if (!string.IsNullOrEmpty(objeto.Horaf) && !objeto.Horaf.Contains(':'))
                {
                    ret.Add("txtHoraf", "Formato do horario final incorreto");
                }
            }
            int identificacao = 0;

            if (objeto.IdOcorrencia == 0)
            {
                ret.Add("cbIdOcorrencia", "Campo obrigat�rio.");
            }

            if (objeto.Datai == null)
            {
                ret.Add("txtDatai", "Campo obrigat�rio.");
            }

            int tipoIdentificador = 0;
            if (objeto.Tipo == -1)
            {
                ret.Add("rgTipo", "Campo obrigat�rio.");
            }
            else
            {
                if (objeto.Tipo == 0)
                {
                    if (objeto.IdFuncionario == 0)
                    {
                        ret.Add("cbIdFuncionario", "Campo obrigat�rio.");
                    }
                    else
                    {
                        identificacao = objeto.IdFuncionario;
                    }
                    tipoIdentificador = 2;
                }
                else if (objeto.Tipo == 1)
                {
                    if (objeto.IdEmpresa == 0)
                    {
                        ret.Add("cbIdEmpresa", "Campo obrigat�rio.");
                    }
                    if (objeto.IdDepartamento == 0)
                    {
                        ret.Add("cbIdDepartamento", "Campo obrigat�rio.");
                    }
                    else
                    {
                        identificacao = objeto.IdDepartamento;
                    }
                    tipoIdentificador = 1;
                }
                else if (objeto.Tipo == 2)
                {
                    if (objeto.IdEmpresa == 0)
                    {
                        ret.Add("cbIdEmpresa", "Campo obrigat�rio.");
                    }
                    else
                    {
                        identificacao = objeto.IdEmpresa;
                    }
                    tipoIdentificador = 0;
                }

                if (objeto.Datai != null)
                {
                    if (objeto.Dataf != null && objeto.Dataf.Value < objeto.Datai.Value)
                    {
                        ret.Add("txtDataf", "A data final deve ser maior ou igual a data inicial.");
                    }
                    else if (objeto.Acao != Modelo.Acao.Excluir && VerificaExiste(objeto.Id, objeto.Datai.Value, (objeto.Dataf == null ? DateTime.MaxValue : objeto.Dataf.Value), objeto.Tipo, identificacao))
                    {
                        ret.Add("rgTipo", "Existe afastamento cadastrado para o per�odo informado. Gentileza verificar.");
                    }
                }

                List<PxyUltimoFechamentoPonto> pxyUltimoFechamentos;
                DateTime? maiorFechamento;
                GetFechamentos(identificacao, tipoIdentificador, out pxyUltimoFechamentos, out maiorFechamento);
                Modelo.Afastamento afastamentoAnt = LoadObject(objeto.Id);
                DateTime? dataInicioAfastamento = objeto.Datai;
                DateTime? dataFimAfastamento = objeto.Dataf ?? DateTime.MaxValue;
                DateTime? dataInicioAfastamentoAnt = null;
                DateTime? dataFimAfastamentoAnt = null;

                //Se esta sendo excluido, a data inicial ou a final n�o pode violar
                if ((objeto.Acao == Modelo.Acao.Excluir || objeto.Acao == Modelo.Acao.Incluir) && (objeto.Datai <= maiorFechamento || objeto.Dataf <= maiorFechamento))
                {
                    AddErroFechamento(ret, pxyUltimoFechamentos);
                }
                //Se a data Inicial e final forem menor que a data do fechamento n�o permite alterar nada
                else
                {
                    if (afastamentoAnt.Id > 0)
                    {
                        dataInicioAfastamentoAnt = afastamentoAnt.Datai;
                        dataFimAfastamentoAnt = afastamentoAnt.Dataf ?? DateTime.MaxValue;
                    }

                    if (((dataInicioAfastamentoAnt != null && dataInicioAfastamentoAnt <= maiorFechamento) || (dataInicioAfastamento < maiorFechamento)) &&
                      ((dataFimAfastamentoAnt != null && dataFimAfastamentoAnt <= maiorFechamento) || (dataFimAfastamento < maiorFechamento)))
                    {
                        AddErroFechamento(ret, pxyUltimoFechamentos);
                    }
                    //Se a data final � maior que o Fechamento e a inicial � menor, apenas altera a data fim do afastamento
                    else if (((dataInicioAfastamentoAnt != null && dataInicioAfastamentoAnt <= maiorFechamento) || (dataInicioAfastamento < maiorFechamento)) &&
                             ((dataFimAfastamento == null || dataFimAfastamento > maiorFechamento) && (dataFimAfastamentoAnt == null || dataFimAfastamentoAnt > maiorFechamento)))
                    {
                        afastamentoAnt.Dataf = objeto.Dataf;
                        objeto = afastamentoAnt;
                    }
                }
            }
            return ret;
        }

        public void GetFechamentos(int identificacao, int tipoIdentificador, out List<PxyUltimoFechamentoPonto> pxyUltimoFechamentos, out DateTime? maiorFechamento)
        {
            BLL.Funcionario bllFunc = new Funcionario(ConnectionString, UsuarioLogado);
            List<int> idsFuncs = bllFunc.GetIDsByTipo(tipoIdentificador, new List<int>() { identificacao }, false, false);
            pxyUltimoFechamentos = bllFunc.GetUltimoFechamentoPontoFuncionarios(idsFuncs);
            maiorFechamento = pxyUltimoFechamentos.Max(m => m.UltimoFechamento);
        }

        private static void AddErroFechamento(Dictionary<string, string> ret, List<PxyUltimoFechamentoPonto> pxyUltimoFechamentos)
        {
            if (pxyUltimoFechamentos.Where(w => w.UltimoFechamento != null).Any())
            {
                ret.Add("FechamentoPonto", string.Join("</br>", pxyUltimoFechamentos.Where(w => w.UltimoFechamento != null).Select(s => $"{s.Codigo} | {s.Nome} - Fechamento Ponto: {s.UltimoFechamento.GetValueOrDefault().ToShortDateString()}").Take(100)));
            }
            if (pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Any())
            {
                ret.Add("FechamentoBanco", string.Join("</br>", pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Select(s => $"{s.Codigo} | {s.Nome} - Fechamento Banco: {s.UltimoFechamento.GetValueOrDefault().ToShortDateString()}").Take(100)));
            }
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Afastamento pObjAfastamento)
        {
            pObjAfastamento.Acao = pAcao;
            Dictionary<string, string> erros = ValidaObjeto(pObjAfastamento);

            try
            {
                if (erros.Count == 0)
                {
                    switch (pAcao)
                    {
                        case Modelo.Acao.Incluir:
                            try
                            {
                                dalAfastamento.Incluir(pObjAfastamento);
                            }
                            catch (Exception ex)
                            {
                                TrataCodigoUso(pObjAfastamento, erros, ex);
                                if (erros.Count > 0)
                                {
                                    return erros;
                                }
                            }
                            break;
                        case Modelo.Acao.Alterar:
                            dalAfastamento.Alterar(pObjAfastamento);
                            break;
                        case Modelo.Acao.Excluir:
                            dalAfastamento.Excluir(pObjAfastamento);
                            if (pObjAfastamento.IdLancamentoLoteFuncionario > 0)
                            {
                                BLL.LancamentoLoteFuncionario bllLLF = new BLL.LancamentoLoteFuncionario(ConnectionString, UsuarioLogado);
                                Modelo.LancamentoLoteFuncionario llf = new Modelo.LancamentoLoteFuncionario();
                                llf = bllLLF.LoadObject(pObjAfastamento.IdLancamentoLoteFuncionario.GetValueOrDefault());
                                bllLLF.Salvar(Modelo.Acao.Excluir, llf);
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("AK_Afastamento"))
                {
                    erros.Add("Datai", "J� existe um registro para o funcion�rio no mesmo per�odo");
                }
                else
                {
                    throw e;
                }
            }
            return erros;
        }

        /// <summary>
        /// Verifica se existe aquele registro no banco de dados
        /// </summary>
        /// <param name="pData">Data</param>
        /// <param name="pEmpresa">Id da empresa</param>
        /// <param name="pDepartamento">Id do departamento</param>
        /// <param name="pFuncionario">Id do funcionario</param>
        /// <returns>true: registro existe ; false: registro n�o existe</returns>
        public bool PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario)
        {
            return dalAfastamento.PossuiRegistro(pData, pEmpresa, pDepartamento, pFuncionario);
        }

        /// <summary>
        /// Verifica se uma lista de afastamentos possui um afastamento para aquela data
        /// </summary>
        /// <param name="pData">Data</param>
        /// <param name="pEmpresa">Id da empresa</param>
        /// <param name="pDepartamento">Id do departamento</param>
        /// <param name="pFuncionario">Id do funcionario</param>
        /// <param name="pAfastamentoList">Lista de afastamentos</param>
        /// <returns>true: existe um afastamento; false: n�o existe um afastamento</returns>
        public bool PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, List<Modelo.Afastamento> pAfastamentoList)
        {
            return pAfastamentoList.Exists(a => (a.Datai <= pData && (a.Dataf == null ? DateTime.MaxValue : a.Dataf) >= pData) && ((a.Tipo == 0 && a.IdFuncionario == pFuncionario) || (a.Tipo == 1 && a.IdDepartamento == pDepartamento) || (a.Tipo == 2 && a.IdEmpresa == pEmpresa)));
        }

        public bool VerificaExiste(int id, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao)
        {
            return (dalAfastamento.VerificaExiste(id, dataInicial, dataFinal, tipo, identificacao) > 0);
        }

        /// <summary>
        /// M�todo respons�vel em retornar o id da tabela. O campo padr�o para busca � o campo c�digo, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso n�o desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo C�digo</param>
        /// <param name="pCampo">Nome do segundo campo que ser� utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalAfastamento.getId(pValor, pCampo, pValor2);
        }

        public void Incluir(List<Modelo.Afastamento> afastamentos)
        {
            dalAfastamento.Incluir(afastamentos);
        }

        public List<Modelo.Afastamento> GetAllList()
        {
            var retorno = dalAfastamento.GetAllList();
            return retorno;
        }


        public DataTable GetParaRelatorioAbono(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento, string pIdsOcorrenciasSelecionados)
        {
            return dalAfastamento.GetParaRelatorioAbono(pTipo, pIdentificacao, pDataI, pDataF, pModoOrdenacao, pAgrupaDepartamento, pIdsOcorrenciasSelecionados);
        }

        public int? GetIdPorIdIntegracao(string idIntegracao)
        {
            return dalAfastamento.GetIdPorIdIntegracao(idIntegracao);
        }

        public int? GetIdAfastamentoPorIdMarcacao(int IdMarcacacao)
        {
            return dalAfastamento.GetIdAfastamentoPorIdMarcacao(IdMarcacacao);
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoAfastamento(int idAfastamento)
        {
            return dalAfastamento.FechamentoPontoAfastamento(idAfastamento);
        }
        public IList<Modelo.Proxy.pxyAbonosPorMarcacao> GetAbonosPorMarcacoes(IList<int> idFuncionarios, DateTime dataIni, DateTime dataFin)
        {
            return dalAfastamento.GetAbonosPorMarcacoes(idFuncionarios, dataIni, dataFin);
        }

        public List<Modelo.Afastamento> GetFeriasFuncionarioPeriodo(int idFuncionario, DateTime pDataInicial, DateTime pDataFinal)
        {
            return dalAfastamento.GetAfastamentoFuncionarioPeriodo(idFuncionario, pDataInicial, pDataFinal, true);
        }

        public List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(int idFuncionario, DateTime pDataInicial, DateTime? pDataFinal)
        {
            return dalAfastamento.GetAfastamentoFuncionarioPeriodo(idFuncionario, pDataInicial, pDataFinal, false);
        }
        public List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(List<int> idsFuncionarios, DateTime pDataInicial, DateTime? pDataFinal)
        {
            return dalAfastamento.GetAfastamentoFuncionarioPeriodo(idsFuncionarios, pDataInicial, pDataFinal, false);
        }

        public List<PxyRelAfastamento> GetRelatorioAfastamentoFolha(List<int> idsFuncs, DateTime pDataI, DateTime pDataF, Int16 absenteismo, bool considerarAbonado, bool considerarParcial, bool considerarSemCalculo, bool considerarSuspensao, bool considerarSemAbono)
        {
            return dalAfastamento.GetRelatorioAfastamentoFolha(idsFuncs, pDataI, pDataF, absenteismo, considerarAbonado, considerarParcial, considerarSemCalculo, considerarSuspensao, considerarSemAbono);
        }
        private void TrataCodigoUso(Modelo.Afastamento objeto, Dictionary<string, string> erros, Exception e)
        {
            if (e.Message.Contains("UX_Codigo"))
            {
                while (true)
                {
                    int ultimoCodigo = MaxCodigo();
                    objeto.Codigo = ultimoCodigo;
                    try
                    {
                        dalAfastamento.Incluir(objeto);
                        erros = new Dictionary<string, string>();
                        break;
                    }
                    catch (Exception ex)
                    {
                        tentativasNovoCodigo++;
                        if ((tentativasNovoCodigo > 3) || !(e.Message.Contains("UX_Codigo")))
                        {
                            if (erros.Count() > 0)
                            {
                                break;
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            else
            {
                throw e;
            }
        }
    }
}
