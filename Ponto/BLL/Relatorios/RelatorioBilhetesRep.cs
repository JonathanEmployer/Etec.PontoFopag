using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Relatorios
{
    public class RelatorioBilhetesRep
    {
        private DAL.RelatoriosSQL.RelatorioBilhetesRep dalRelatorioBilhetesRep;
        public RelatorioBilhetesRep() : this(null)
        {
        }

        public RelatorioBilhetesRep(string conn)
        {
            if (String.IsNullOrEmpty(conn))
	        {
                conn = Modelo.cwkGlobal.CONN_STRING;
	        }
            DataBase db = new DataBase(conn);
            dalRelatorioBilhetesRep = new DAL.RelatoriosSQL.RelatorioBilhetesRep(db);
        }

        public DataTable GetDadosRelatorioBilhetesRep(int idRep, DateTime dataIni, DateTime dataFin)
        {
            return dalRelatorioBilhetesRep.GetRelatorioBilhetesRep(idRep, dataIni, dataFin);
        }
    }
}
