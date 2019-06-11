using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorJobs.DAL
{
    public class Bases: DalBase
    {
        public Bases(string conn)
        {
            conexao = conn;
        }

        public List<Models.Bases> GetAllDBPontofopag()
        {
            List<Models.Bases> bases = new List<Models.Bases>();
            try
            {
                string sql = @"	SELECT name Nome, database_id IdDatabase, create_date DataCriacao
	                            FROM sys.databases 
                                where (name LIKE 'Pontofopag%' or name = 'CWORKPONTOWEBCRIACAO') AND name NOT LIKE '%BloqueioEsta%'";

                DataTable dt = ExecuteReaderToDataTabela(conexao, sql, null);

                bases = DataTableMapToList<Models.Bases>(dt);
                return bases;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
