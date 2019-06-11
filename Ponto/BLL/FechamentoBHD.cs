using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class FechamentoBHD : IBLL<Modelo.FechamentoBHD>
    {
        DAL.IFechamentoBHD dalFechamentoBHD;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public FechamentoBHD() : this(null)
        {
            
        }

        public FechamentoBHD(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public FechamentoBHD(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalFechamentoBHD = new DAL.SQL.FechamentoBHD(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalFechamentoBHD = new DAL.SQL.FechamentoBHD(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalFechamentoBHD = DAL.FB.FechamentoBHD.GetInstancia;
                    break;
            }
            dalFechamentoBHD.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFechamentoBHD.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalFechamentoBHD.GetAll();
        }

        public Modelo.FechamentoBHD LoadObject(int id)
        {
            return dalFechamentoBHD.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FechamentoBHD objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FechamentoBHD objeto, bool pRecalculaMarcacao)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFechamentoBHD.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFechamentoBHD.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFechamentoBHD.Excluir(objeto);
                        break;
                }
                if (pRecalculaMarcacao)
                {
                    bllMarcacao.RecalculaMarcacao(2, objeto.Identificacao, objeto.DataFechamento.Value, objeto.DataFechamento.Value, objProgressBar);   
                }
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FechamentoBHD objeto)
        {
            return Salvar(pAcao, objeto, true);
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
            return dalFechamentoBHD.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.FechamentoBHD> getPorEmpresa(int pIdEmpresa)
        {
            return dalFechamentoBHD.getPorEmpresa(pIdEmpresa);
        }

        public List<Modelo.FechamentoBHD> getPorDepartamento(int pIdDepartamento)
        {
            return dalFechamentoBHD.getPorDepartamento(pIdDepartamento);
        }

        public List<Modelo.FechamentoBHD> getPorFuncionario(int pIdFuncionario)
        {
            return dalFechamentoBHD.getPorFuncionario(pIdFuncionario);
        }

        public List<Modelo.FechamentoBHD> getPorListaFuncionario(List<int> pIdsFuncionarios)
        {
            return dalFechamentoBHD.getPorListaFuncionario(pIdsFuncionarios);
        }

        public List<Modelo.FechamentoBHD> getPorFuncao(int pIdFuncao)
        {
            return dalFechamentoBHD.getPorFuncao(pIdFuncao);
        }

        /// <summary>
        /// Pega os fechamentos de um determinado período
        /// </summary>
        /// <param name="pDataInicial">Data Inicial</param>
        /// <param name="pDataFinal">Data Final</param>
        /// <returns>Lista de Fechamentos BHD</returns>
        public List<Modelo.FechamentoBHD> getPorPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes)
        {
            return dalFechamentoBHD.getPorPeriodo(pDataInicial, pDataFinal, pTipo, pIdentificacoes);
        }

        public void Incluir(List<Modelo.FechamentoBHD> lista)
        {
            dalFechamentoBHD.Incluir(lista);
        }

        public string MontaLista(Modelo.FechamentoBHD pObjFechamentoBHD, Modelo.Acao acao)
        {            
            switch (acao)
            {
                case Modelo.Acao.Incluir: return dalFechamentoBHD.MontaStringInsert(pObjFechamentoBHD);                    
                case Modelo.Acao.Alterar: return dalFechamentoBHD.MontaStringUpdate(pObjFechamentoBHD);
                default: return "";
            }
        }

        public void SalvarLista(List<string> pLstStrFechamentoBHD)
        {
            dalFechamentoBHD.SalvaLista(pLstStrFechamentoBHD);
        }


        public DataTable GetFuncionariosFechamento(int pIdFechamentoBH)
        {
            return dalFechamentoBHD.GetFuncionariosFechamento(pIdFechamentoBH);
        }

        public List<Modelo.FechamentoBHD> GetAllList()
        {
            return dalFechamentoBHD.GetAllList();
        }

        public List<Modelo.Proxy.PxyFechamentoBHD> GetAllGrid(int idFechamentoBH)
        {
            return dalFechamentoBHD.GetAllGrid(idFechamentoBH);
        }

        public IList<Modelo.FechamentoBHD> GetFechamentoBHDPorIdFechamentoBH(int idFechamentoBH)
        {
            return dalFechamentoBHD.GetFechamentoBHDPorIdFechamentoBH(idFechamentoBH);
        }

    }
}
