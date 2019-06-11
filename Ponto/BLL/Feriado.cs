using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class Feriado : IBLL<Modelo.Feriado>
    {
        DAL.IFeriado dalFeriado;

        private Modelo.ProgressBar objProgressBar;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public Feriado() : this(null)
        {
            
        }

        public Feriado(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Feriado(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalFeriado = new DAL.SQL.Feriado(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalFeriado = DAL.FB.Feriado.GetInstancia;
                    break;
            }
            dalFeriado.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFeriado.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFeriado.GetAll();
        }

        public List<Modelo.Feriado> GetAllList()
        {
            return dalFeriado.GetAllList();
        }

        public List<Modelo.Feriado> getFeriado(DateTime pDataI, DateTime pDataF)
        {
            return dalFeriado.getFeriado(pDataI, pDataF);
        }

        public List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(int idFuncionario, DateTime inicio, DateTime fim)
        {
            return dalFeriado.GetFeriadosFuncionarioPeriodo(idFuncionario, inicio, fim);
        }

        public List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(List<int> idsFuncionarios, DateTime inicio, DateTime fim)
        {
            return dalFeriado.GetFeriadosFuncionarioPeriodo(idsFuncionarios, inicio, fim);
        }

        public Modelo.Feriado LoadObject(int id)
        {
            return dalFeriado.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Feriado objeto)
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
           
            if (objeto.Data == null)
            {
                ret.Add("txtData", "Campo obrigatório.");
            }

            if (objeto.TipoFeriado == -1)
            {
                ret.Add("rgTipoFeriado", "Campo obrigatório.");
            }
            else
            {
                //Empresa
                if (objeto.TipoFeriado == 1 && objeto.IdEmpresa == 0)
                {
                    ret.Add("cbIdEmpresa", "Campo obrigatório.");
                }
                //Departamento
                else if (objeto.TipoFeriado == 2)
                {
                    if (objeto.IdEmpresa == 0)
                    {
                        ret.Add("cbIdEmpresa", "Campo obrigatório.");
                    }

                    if (objeto.IdDepartamento == 0)
                    {
                        ret.Add("cbIdDepartamento", "Campo obrigatório.");
                    }
                }
            }

            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            int tipoFechamento = 0;
            List<int> idTipos = new List<int>();
            switch (objeto.TipoFeriado)
            {
                case 1: // Empresa
                    tipoFechamento = 0;
                    idTipos.Add(objeto.IdEmpresa);
                    break;
                case 2: // Departamento
                    tipoFechamento = 1;
                    idTipos.Add(objeto.IdDepartamento);
                    break;
                case 3: // Funcionario
                    tipoFechamento = 2;
                    idTipos = objeto.IdsFeriadosFuncionariosSelecionados.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();
                    break;
                default: // Todos
                    tipoFechamento = 4;
                    break;
            }
            string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(tipoFechamento, idTipos, objeto.Data.GetValueOrDefault());
            if (!String.IsNullOrEmpty(mensagemFechamento))
            {
                ret.Add("Fechamento Ponto", mensagemFechamento);
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Feriado objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFeriado.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFeriado.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFeriado.Excluir(objeto);
                        break;
                }
                if (!objeto.NaoRecalcular)
                {
                    this.AtualizaMarcacao(pAcao, objeto);
                }
            }
            return erros;
        }

        /// <summary>
        /// Método responsável para encontrar se uma data é feriado ou não
        /// </summary>
        /// <param name="pData">Data da marcação</param>
        /// <param name="pEmpresa">Empresa do Funcionário</param>
        /// <param name="pDepartamento">Departamento do Funcionário</param>
        /// <returns>Retorna verdadeiro ou falso</returns>
        public bool PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento)
        {
            List<Modelo.Feriado> feriados = dalFeriado.getFeriado(pData);

            return feriados.Exists(f => (f.Data == pData) && ((f.TipoFeriado == 0) || (f.TipoFeriado == 1 && f.IdEmpresa == pEmpresa) || (f.TipoFeriado == 2 && f.IdDepartamento == pDepartamento)));
        }

        /// <summary>
        /// Método responsável para encontrar se uma data é feriado ou não baseado em uma lista de entrada
        /// </summary>
        /// <param name="pData">Data</param>
        /// <param name="pEmpresa">Id da empresa</param>
        /// <param name="pDepartamento">Id do departamento</param>
        /// <param name="pFeriadosList">Lista de feriados</param>
        /// <returns>true - tem feriado naquela data, false - não tem</returns>
        public bool PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, List<Modelo.Feriado> pFeriadosList)
        {
            return pFeriadosList.Exists(f => (f.Data == pData) && ((f.TipoFeriado == 0)|| (f.TipoFeriado == 1 && f.IdEmpresa == pEmpresa) ||(f.TipoFeriado == 2 && f.IdDepartamento == pDepartamento)));
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
            return dalFeriado.getId(pValor, pCampo, pValor2);
        }

        public void AtualizaMarcacao(Modelo.Acao pAcao, Modelo.Feriado pFeriado)
        {
            BLL.CalculaMarcacao bllCalculaMarcacao;
            
            int? tipoRecalculo = 0;
            int idRecalculo = 0;
            bool bAlterado = (pFeriado.TipoFeriado_Ant != pFeriado.TipoFeriado) || (pFeriado.Data_Ant != pFeriado.Data)
                    || (pFeriado.IdDepartamento_Ant != pFeriado.IdDepartamento) || (pFeriado.IdEmpresa_Ant != pFeriado.IdEmpresa);
            #region AtualizaDadosAnterior
            if (pAcao == Modelo.Acao.Alterar)
            {
                //Caso algum dado tenha sido alterado
                if (bAlterado)
                {
                    if (pFeriado.TipoFeriado_Ant == 0)//Geral
                    {
                        tipoRecalculo = null;
                    }
                    else if (pFeriado.TipoFeriado_Ant == 1) //Empresa
                    {
                        tipoRecalculo = 0;
                        idRecalculo = pFeriado.IdEmpresa_Ant;
                    }
                    else if (pFeriado.TipoFeriado_Ant == 2) //Departamento
                    {
                        tipoRecalculo = 1;
                        idRecalculo = pFeriado.IdDepartamento_Ant;
                    }

                    bllCalculaMarcacao = new CalculaMarcacao(tipoRecalculo, idRecalculo, ((DateTime)pFeriado.Data_Ant).AddDays(-1), ((DateTime)pFeriado.Data_Ant).AddDays(+1), objProgressBar, false, ConnectionString, UsuarioLogado, false);
                    bllCalculaMarcacao.CalculaMarcacoes();
                }
            }
            #endregion

            #region AtualizaDadosAlterados
            if (bAlterado || pAcao == Modelo.Acao.Excluir || pAcao == Modelo.Acao.Incluir)
            {
                if (pFeriado.TipoFeriado == 0)//Geral
                {
                    tipoRecalculo = null;
                }
                else if (pFeriado.TipoFeriado == 1) //Empresa
                {
                    tipoRecalculo = 0;
                    idRecalculo = pFeriado.IdEmpresa;
                }
                else if (pFeriado.TipoFeriado == 2) //Departamento
                {
                    tipoRecalculo = 1;
                    idRecalculo = pFeriado.IdDepartamento;
                }

                bllCalculaMarcacao = new CalculaMarcacao(tipoRecalculo, idRecalculo, ((DateTime)pFeriado.Data).AddDays(-1), ((DateTime)pFeriado.Data).AddDays(+1), objProgressBar, false, ConnectionString, UsuarioLogado, false);
                bllCalculaMarcacao.CalculaMarcacoes();
            }
            #endregion
        }

        public List<Modelo.Feriado> GetIdPorIdIntegracao(int idIntegracao)
        {
            return dalFeriado.GetIdPorIdIntegracao(idIntegracao);
        }
    }
}
