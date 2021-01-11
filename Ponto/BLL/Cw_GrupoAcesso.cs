using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Cw_GrupoAcesso : IBLL<Modelo.Cw_GrupoAcesso>
    {
        DAL.SQL.Cw_GrupoAcesso dal;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        public System.Data.DataTable GetAll()
        {
            return dal.GetAll();
        }

        public Cw_GrupoAcesso()
        {
            
        }

        public Cw_GrupoAcesso(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dal = new DAL.SQL.Cw_GrupoAcesso(new DataBase(ConnectionString));
            UsuarioLogado = usuarioLogado;
            dal.UsuarioLogado = usuarioLogado;
        }

        public Modelo.Cw_GrupoAcesso LoadObject(int id)
        {
            return dal.LoadObject(id);
        }

        public pxyCwGrupoAcesso GetPxyListPorGrupo(int idGrupo)
        {
            pxyCwGrupoAcesso model = new pxyCwGrupoAcesso();
            model.NomeGrupo = new BLL.Cw_Grupo(ConnectionString, UsuarioLogado).LoadObject(idGrupo).Nome;
            model.Permissoes = dal.GetAllListPorGrupo(idGrupo);
            return model;
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Cw_GrupoAcesso objeto)
        {
            return new Dictionary<string, string>();
        }

        public List<Modelo.Proxy.PxyGridGrupodeUsuario> GetAllGrid()
        {
            return dal.GetAllGrid();
        }

        public List<Modelo.Proxy.PxyGridUsuario> GetAllGridU()
        {
            return dal.GetAllGridU();
        }

        public List<Modelo.Proxy.pxyUsuarioControleAcessoCopiar> GetAllGridUCompact()
        {
            return dal.GetAllGridUCompact();
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Cw_GrupoAcesso objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);

            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dal.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dal.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dal.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dal.getId(pValor, pCampo, pValor2);
        }
    }
}
