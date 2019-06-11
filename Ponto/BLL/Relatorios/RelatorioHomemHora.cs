using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.Relatorios
{
    public class RelatorioHomemHora
    {
        private DAL.RelatoriosSQL.RelatorioHomemHora dalRelatorioHomemHora;
        public RelatorioHomemHora() : this(null)
        {
        }

        public RelatorioHomemHora(string conn)
        {
            if (String.IsNullOrEmpty(conn))
	        {
                conn = Modelo.cwkGlobal.CONN_STRING;
	        }
            DataBase db = new DataBase(conn);
            dalRelatorioHomemHora = new DAL.RelatoriosSQL.RelatorioHomemHora(db);
        }

        public DataTable GetRelatorioHomemHora(String idsFuncionarios, DateTime dataIni, DateTime dataFin, string idsOcorrencias)
        {
            return dalRelatorioHomemHora.GetRelatorioHomemHora(idsFuncionarios, dataIni, dataFin, idsOcorrencias);
        }

        public List<Modelo.Proxy.Relatorios.RelHomemHora> GetRelatorioHomemHora(List<int> idsFuncionarios, DateTime dataIni, DateTime dataFin, string idsOcorrencias)
        {
            DataTable dt = dalRelatorioHomemHora.GetRelatorioHomemHora(String.Join(",", idsFuncionarios), dataIni, dataFin, idsOcorrencias);
            List<Modelo.Proxy.Relatorios.RelHomemHora> lista = dt.DataTableMapToList<Modelo.Proxy.Relatorios.RelHomemHora>();
            return lista;
        }
    }
}
