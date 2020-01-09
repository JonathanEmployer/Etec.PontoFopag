using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections;
using DAL.SQL;
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
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalAfastamento = new DAL.SQL.Afastamento(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalAfastamento = DAL.FB.Afastamento.GetInstancia;
                    break;
            }
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

            int identificacao = 0;

            if (objeto.IdOcorrencia == 0)
            {
                ret.Add("cbIdOcorrencia", "Campo obrigatório.");
            }

            if (objeto.Datai == null)
            {
                ret.Add("txtDatai", "Campo obrigatório.");
            }

            if (objeto.Tipo == -1)
            {
                ret.Add("rgTipo", "Campo obrigatório.");
            }
            else
            {
                if (objeto.Tipo == 0)
                {
                    if (objeto.IdFuncionario == 0)
                    {
                        ret.Add("cbIdFuncionario", "Campo obrigatório.");
                    }
                    else
                    {
                        identificacao = objeto.IdFuncionario;
                    }
                }
                else if (objeto.Tipo == 1)
                {
                    if (objeto.IdEmpresa == 0)
                    {
                        ret.Add("cbIdEmpresa", "Campo obrigatório.");
                    }
                    if (objeto.IdDepartamento == 0)
                    {
                        ret.Add("cbIdDepartamento", "Campo obrigatório.");
                    }
                    else
                    {
                        identificacao = objeto.IdDepartamento;
                    }
                }
                else if (objeto.Tipo == 2)
                {
                    if (objeto.IdEmpresa == 0)
                    {
                        ret.Add("cbIdEmpresa", "Campo obrigatório.");
                    }
                    else
                    {
                        identificacao = objeto.IdEmpresa;
                    }
                }

                if (objeto.Datai != null)
                {
                    if (objeto.Dataf != null && objeto.Dataf.Value < objeto.Datai.Value)
                    {
                        ret.Add("txtDataf", "A data final deve ser maior ou igual a data inicial.");
                    }
                    else if (objeto.Acao != Modelo.Acao.Excluir && VerificaExiste(objeto.Id, objeto.Datai.Value, (objeto.Dataf == null ? DateTime.MaxValue : objeto.Dataf.Value), objeto.Tipo, identificacao))
                    {
                        ret.Add("rgTipo", "Existe afastamento cadastrado para o período informado. Gentileza verificar.");
                    }
                }

            }
            return ret;
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
                            catch(Exception ex)
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
                    erros.Add("Datai", "Já existe um registro para o funcionário no mesmo período");
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
        /// <returns>true: registro existe ; false: registro não existe</returns>
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
        /// <returns>true: existe um afastamento; false: não existe um afastamento</returns>
        public bool PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, List<Modelo.Afastamento> pAfastamentoList)
        {
            return pAfastamentoList.Exists(a => (a.Datai <= pData && (a.Dataf == null ? DateTime.MaxValue : a.Dataf) >= pData) && ((a.Tipo == 0 && a.IdFuncionario == pFuncionario) || (a.Tipo == 1 && a.IdDepartamento == pDepartamento) || (a.Tipo == 2 && a.IdEmpresa == pEmpresa)));
        }

        public bool VerificaExiste(int id, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao)
        {
            return (dalAfastamento.VerificaExiste(id, dataInicial, dataFinal, tipo, identificacao) > 0);
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
