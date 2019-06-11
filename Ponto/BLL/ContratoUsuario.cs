using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class ContratoUsuario : IBLL<Modelo.ContratoUsuario>
    {
        #region Construtores
        DAL.IContratoUsuario dal;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public ContratoUsuario()
            : this(null)
        {

        }

        public ContratoUsuario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ContratoUsuario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
            dal = new DAL.SQL.ContratoUsuario(new DataBase(ConnectionString));
            dal.UsuarioLogado = usuarioLogado;
        } 
        #endregion

        public int MaxCodigo()
        {
            return dal.MaxCodigo();
        }
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        public Modelo.ContratoUsuario LoadObject(int id)
        {
            return dal.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ContratoUsuario objeto)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            return res;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ContratoUsuario objeto)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            if (pAcao == Modelo.Acao.Alterar || pAcao == Modelo.Acao.Incluir)
            {
                res = ValidaObjeto(objeto);
            }
            if (res.Count > 0)
            {
                return res;
            }
            try
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
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return res;
        }

        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dal.getId(pValor, pCampo, pValor2);
        }

        public List<Modelo.ContratoUsuario> GetAllList()
        {
            return dal.GetAllList();
        }

        public List<Modelo.ContratoUsuario> GetAllListPorContrato(int idContrato)
        {
            return dal.GetAllListPorContrato(idContrato);
        }

        public Modelo.ContratoUsuario LoadPorCodigo(int codigo)
        {
            return dal.LoadPorCodigo(codigo);
        }
        public pxyContratoCwUsuario GetListaUsuariosLiberadosBloqueadosPorContrato(int idContrato)
        {
            return dal.GetListaUsuariosLiberadosBloqueadosPorContrato(idContrato);
        }
    }
}
