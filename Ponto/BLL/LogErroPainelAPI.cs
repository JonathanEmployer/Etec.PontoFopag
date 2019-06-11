using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class LogErroPainelAPI : IBLL<Modelo.LogErroPainelAPI>
    {
        DAL.ILogErroPainelAPI dalLogErroPainelAPI;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public LogErroPainelAPI()
            : this(null)
        {

        }

        public LogErroPainelAPI(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LogErroPainelAPI(string connString, Modelo.Cw_Usuario usuarioLogado)
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
                    dalLogErroPainelAPI = new DAL.SQL.LogErroPainelAPI(new DataBase(ConnectionString));
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalLogErroPainelAPI.UsuarioLogado = usuarioLogado;
        }

        public DataTable GetAll()
        {
            return dalLogErroPainelAPI.GetAll();
        }

        public List<Modelo.LogErroPainelAPI> GetGrid()
        {
            return dalLogErroPainelAPI.GetGrid();
        }

        public List<Modelo.LogErroPainelAPI> GetAllList()
        {
            return dalLogErroPainelAPI.GetAllList();
        }

        public Modelo.LogErroPainelAPI LoadObject(int id)
        {
            return dalLogErroPainelAPI.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LogErroPainelAPI objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (String.IsNullOrEmpty(objeto.logErro))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LogErroPainelAPI objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalLogErroPainelAPI.Incluir(objeto);
                        return ret;
                    case Modelo.Acao.Alterar:
                        dalLogErroPainelAPI.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalLogErroPainelAPI.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalLogErroPainelAPI.getId(pValor, pCampo, pValor2);
        }
        

    }
}
