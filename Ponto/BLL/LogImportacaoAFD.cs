using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.SQL;
using Modelo;
using Modelo.Proxy;

namespace BLL
{
    public class LogImportacaoAFD : IBLL<Modelo.LogImportacaoAFD>
    {
        DAL.ILogImportacaoAFD dalLogImportacaoAFD;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;
        private int tentativasNovoCodigo = 0;

        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public LogImportacaoAFD() : this(null)
        {

        }

        public LogImportacaoAFD(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public LogImportacaoAFD(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalLogImportacaoAFD = new DAL.SQL.LogImportacaoAFD(new DataBase(ConnectionString));
            dalLogImportacaoAFD.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public Modelo.LogImportacaoAFD LoadObject(int id)
        {
            return dalLogImportacaoAFD.LoadObject(id);
        }

        public List<Modelo.LogImportacaoAFD> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            return dalLogImportacaoAFD.GetPeriodo(pDataInicial, pDataFinal);
        }

        DataTable IBLL<Modelo.LogImportacaoAFD>.GetAll()
        {
            throw new NotImplementedException();
        }

        Modelo.LogImportacaoAFD IBLL<Modelo.LogImportacaoAFD>.LoadObject(int id)
        {
            return dalLogImportacaoAFD.LoadObject(id);
        }

        Dictionary<string, string> IBLL<Modelo.LogImportacaoAFD>.ValidaObjeto(Modelo.LogImportacaoAFD objeto)
        {
            throw new NotImplementedException();
        }

        Dictionary<string, string> IBLL<Modelo.LogImportacaoAFD>.Salvar(Acao pAcao, Modelo.LogImportacaoAFD objeto)
        {
            throw new NotImplementedException();
        }

        int IBLL<Modelo.LogImportacaoAFD>.getId(int pValor, string pCampo, int? pValor2)
        {
            throw new NotImplementedException();
        }
    }
}
