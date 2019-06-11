using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class Departamento : IBLL<Modelo.Departamento>
    {
        DAL.IDepartamento dalDepartamento;
        private string ConnectionString;

        public Departamento() : this(null)
        {
            
        }

        public Departamento(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }
        public Departamento(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalDepartamento = new DAL.SQL.Departamento(new DataBase(ConnectionString));
                    break;
                case 2:
                    dalDepartamento = DAL.FB.Departamento.GetInstancia;
                    break;
            }
            if (usuarioLogado != null)
            {
                dalDepartamento.UsuarioLogado = usuarioLogado;   
            }
        }

        public int MaxCodigo()
        {
            return dalDepartamento.MaxCodigo();
        }

        public List<Modelo.Departamento> GetAllList()
        {
            return dalDepartamento.GetAllList();
        }

        public List<Modelo.Departamento> GetAllListByIds(List<int> ids)
        {
            return dalDepartamento.GetAllListByIds(ids);
        }

        public List<Modelo.Departamento> GetAllListComOpcaoTodos()
        {
            List<Modelo.Departamento> lDep = dalDepartamento.GetAllList();
            Modelo.Departamento TDep = new Modelo.Departamento { Id = 0, Codigo = 0, Descricao = "TODOS OS DEPARTAMENTOS" };
            lDep.Add(TDep);
            return lDep;
        }

        public List<Modelo.Departamento> GetAllListLike(string desc)
        {
            return dalDepartamento.GetAllListLike(desc);
        }

        public DataTable GetAll()
        {
            return dalDepartamento.GetAll();
        }

        public DataTable GetAllComOpcaoTodos()
        {

            DataTable dt = dalDepartamento.GetAll();
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { 0, "Todos os Departamentos", 0, "" };
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        public DataTable GetPorEmpresaComOpcaoTodos(int IdEmpresa)
        {

            DataTable dt = dalDepartamento.GetPorEmpresa(IdEmpresa);
            DataRow dr = dt.NewRow();
            dr.ItemArray = new object[] { 0, 0, "Todos os Departamentos", 0, null, null, "", null, null, "" };
            dt.Rows.InsertAt(dr, 0);
            return dt;            
        }

        public DataTable GetPorEmpresa(int IdEmpresa)
        {
            return dalDepartamento.GetPorEmpresa(IdEmpresa);
        }

        public DataTable GetPorEmpresa(string empresas)
        {
            return dalDepartamento.GetPorEmpresa(empresas);
        }

        public Modelo.Departamento LoadObject(int id)
        {
            return dalDepartamento.LoadObject(id);
        }

        public Modelo.Departamento LoadObjectByCodigo(int codigo)
        {
            return dalDepartamento.LoadObjectByCodigo(codigo);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Departamento objeto)
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

            if (objeto.IdEmpresa == 0)
            {
                ret.Add("cbIdEmpresa", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Departamento objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalDepartamento.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalDepartamento.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalDepartamento.Excluir(objeto);
                        break;
                }
            }
            return erros;
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
            return dalDepartamento.getId(pValor, pCampo, pValor2);
        }

        public int? GetIdPorDesc(string Descricao) 
        {
            return dalDepartamento.GetIdPorDesc(Descricao);
        }

        public int? GetIdPorCodigo(int Cod) 
        {
           return dalDepartamento.GetIdPorCodigo(Cod);
        }

        public int? GetIdPoridIntegracao(int idIntegracao)
        {
            return dalDepartamento.GetIdPorIdIntegracao(idIntegracao);
        }

        public bool PossuiFuncionarios(int id)
        {
            return dalDepartamento.PossuiFuncionarios(id);
        }
        public Modelo.Departamento GetDepartamentoPadrao(string cnpj)
        {
            return dalDepartamento.GetDepartamentoPadrao(cnpj);
        }

        public List<Modelo.Departamento> LoadPEmpresa(int IdEmpresa)
        {
            return dalDepartamento.LoadPEmpresa(IdEmpresa);
        }

        public List<int> GetIdsPorCodigos(List<int> codigos)
        {
            return dalDepartamento.GetIdsPorCodigos(codigos);
        }
    }
}
