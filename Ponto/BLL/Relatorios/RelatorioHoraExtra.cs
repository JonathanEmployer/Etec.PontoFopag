using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Relatorios
{
    public class RelatorioHoraExtra
    {
        private DAL.RelatoriosSQL.RelatorioHoraExtra dalRelatorioHoraExtra;
        public RelatorioHoraExtra() : this(null)
        {
        }

        public RelatorioHoraExtra(string conn)
        {
            if (String.IsNullOrEmpty(conn))
	        {
                conn = Modelo.cwkGlobal.CONN_STRING;
	        }
            DataBase db = new DataBase(conn);
            dalRelatorioHoraExtra = new DAL.RelatoriosSQL.RelatorioHoraExtra(db);
        }

        public DataTable GetHorasExtrasMetasDepartamentos(DateTime dataIni, DateTime dataFin, int tipo, Int32[] ids)
        {
            return dalRelatorioHoraExtra.GetHorasExtrasMetasDepartamentos(dataIni, dataFin, tipo, ids);
        }
    }
}
