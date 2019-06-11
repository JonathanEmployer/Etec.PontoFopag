using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Relatorios
{
    public class RelatorioImpBilhetes
    {
        private DAL.RelatoriosSQL.RelatorioBilhetesImp dalRelatorioImpBilhetes;
        public RelatorioImpBilhetes()
            : this(null)
        {
        }

        public RelatorioImpBilhetes(string conn)
        {
            if (String.IsNullOrEmpty(conn))
            {
                conn = Modelo.cwkGlobal.CONN_STRING;
            }
            DataBase db = new DataBase(conn);
            dalRelatorioImpBilhetes = new DAL.RelatoriosSQL.RelatorioBilhetesImp(db);
        }

        public DataTable GetRelatorioImpBilhetes(String idsFuncionarios, DateTime dataIni, DateTime dataFin)
        {
            return dalRelatorioImpBilhetes.GetRelatorioImpBilhetes(idsFuncionarios, dataIni, dataFin);
        }
    }
}
