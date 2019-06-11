using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public class ListaEventosEvento : IBLL<Modelo.ListaEventosEvento>
    {
        DAL.IListaEventosEvento dalListaEventosEvento;
        private string ConnectionString;

        public ListaEventosEvento() : this(null)
        {
            
        }

        public ListaEventosEvento(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ListaEventosEvento(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalListaEventosEvento = new DAL.SQL.ListaEventosEvento(new DataBase(ConnectionString));

            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    dalListaEventosEvento = new DAL.SQL.ListaEventosEvento(new DataBase(ConnectionString));
                    break;
            }
            dalListaEventosEvento.UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalListaEventosEvento.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalListaEventosEvento.GetAll();
        }

        public Modelo.ListaEventosEvento LoadObject(int id)
        {
            return dalListaEventosEvento.LoadObject(id);
        }

        public List<Modelo.ListaEventosEvento> GetAllList()
        {
            return dalListaEventosEvento.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ListaEventosEvento objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ListaEventosEvento objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalListaEventosEvento.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalListaEventosEvento.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalListaEventosEvento.Excluir(objeto);
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
            return dalListaEventosEvento.getId(pValor, pCampo, pValor2);
        }

        public void IncluirLoteIdsFuncionario(SqlTransaction trans, int idListaEventos, List<int> idsFuncs)
        {
            dalListaEventosEvento.IncluirLoteIdsEvento(trans, idListaEventos, idsFuncs);
        }

        public void ExcluirLoteIdsFuncionario(SqlTransaction trans, int idListaEventos, List<int> idsFuncs)
        {
            dalListaEventosEvento.ExcluirLoteIdsEvento(trans, idListaEventos, idsFuncs);
        }

        public List<Modelo.ListaEventosEvento> GetAllPorListaEventos(Int32 idListaEventos)
        {
            return dalListaEventosEvento.GetAllPorListaEventos(idListaEventos);
        }
    }
}
