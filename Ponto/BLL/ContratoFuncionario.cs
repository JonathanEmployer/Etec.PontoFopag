using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class ContratoFuncionario : IBLL<Modelo.ContratoFuncionario>
    {
        #region Construtores
        DAL.IContratoFuncionario dal;
        private string ConnectionString;

        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public ContratoFuncionario()
            : this(null)
        {

        }

        public ContratoFuncionario(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ContratoFuncionario(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dal = new DAL.SQL.ContratoFuncionario(new DataBase(ConnectionString));
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

        public Modelo.ContratoFuncionario LoadObject(int id)
        {
            return dal.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.ContratoFuncionario objeto)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            return res;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.ContratoFuncionario objeto)
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

        public List<Modelo.ContratoFuncionario> GetAllList()
        {
            return dal.GetAllList();
        }

        public List<Modelo.ContratoFuncionario> GetAllListPorContrato(int idContrato)
        {
            return dal.GetAllListPorContrato(idContrato);
        }

        public Modelo.ContratoFuncionario LoadPorCodigo(int codigo)
        {
            return dal.LoadPorCodigo(codigo);
        }

        public pxyContratoFuncionario GetListaFuncionariosLiberadosBloqueadosPorContrato(int idContrato)
        {
            return dal.GetListaFuncionariosLiberadosBloqueadosPorContrato(idContrato);
        }

        public int? GetIdPorIdContratoeIdFuncionario(int idcontrato, int idfuncionario)
        {
            return dal.GetIdPorIdContratoeIdFuncionario(idcontrato, idfuncionario);
        }
        public int getContratoId(int idfuncionario)
        {
            return dal.getContratoId(idfuncionario);
        }
        public int getContratoCodigo(int idcontrato, int idfuncionario)
        {
            return dal.getContratoCodigo(idcontrato, idfuncionario);
        }
    }
}
