using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class LimiteDDsr : IBLL<Modelo.LimiteDDsr>
    {
        DAL.ILimiteDDsr dal;
        private string ConnectionString;

        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public LimiteDDsr()
            : this(null)
        {

        }

        public LimiteDDsr(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LimiteDDsr(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dal = new DAL.SQL.LimiteDDsr(new DataBase(ConnectionString));
            dal.UsuarioLogado = usuarioLogado;
        }
        public System.Data.DataTable GetAll()
        {
            return dal.GetAll();
        }

        public Modelo.LimiteDDsr LoadObject(int id)
        {
            return dal.LoadObject(id);
        }

        public Modelo.LimiteDDsr LoadPorCodigo(int codigo)
        {
            return dal.LoadPorCodigo(codigo);
        }

        public List<Modelo.LimiteDDsr> GetAllList()
        {
            return dal.GetAllList();
        }

        public List<Modelo.LimiteDDsr> GetAllListPorHorario(int idHorario)
        {
            return dal.GetAllListPorHorario(idHorario);
        }

        public List<Modelo.LimiteDDsr> GetAllListPorHorarios(List<int> idsHorario)
        {
            return dal.GetAllListPorHorarios(idsHorario);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.LimiteDDsr objeto)
        {
            return new Dictionary<string, string>();
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.LimiteDDsr objeto)
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
                        try
                        {
                            dal.Excluir(objeto);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
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

        public int MaxCodigo()
        {
            return dal.MaxCodigo();
        }
    }
}
