using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace MonitorJobs.Negocio
{
    public class Bases
    {
        private SqlConnectionStringBuilder conexao = new SqlConnectionStringBuilder();
        public Bases(SqlConnectionStringBuilder conexao)
        {
            this.conexao = conexao;
        }

        public List<Models.Bases> GetAllDBPontofopag()
        {
            DAL.Bases dalBases = new DAL.Bases(conexao.ConnectionString);
            return dalBases.GetAllDBPontofopag();
        }

        public static IList<Models.Bases> GetBasesPontofopagAtivas()
        {
            Negocio.Bases bllBases = new Negocio.Bases(BLL.cwkFuncoes.ConstroiConexao("master"));
            IList<Models.Bases> lbases = bllBases.GetAllDBPontofopag().ToList();

            string basesNMonitorar = ConfigurationManager.AppSettings["BasesDesconsiderarJobs"];
            if (!String.IsNullOrEmpty(basesNMonitorar))
            {
                List<string> basesNaoMonitorar = basesNMonitorar.Split(',').ToList();
                if (basesNaoMonitorar.Count > 0)
                {
                    lbases = lbases.Where(w => !basesNaoMonitorar.Contains(w.Nome)).ToList();
                }
            }
            return lbases;
        }
    }
}
