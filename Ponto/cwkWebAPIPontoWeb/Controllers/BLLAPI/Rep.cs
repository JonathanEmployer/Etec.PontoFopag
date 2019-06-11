using cwkWebAPIPontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace cwkWebAPIPontoWeb.Controllers.BLLAPI
{
    public class Rep
    {
        /// <summary>
        /// Método que retorna uma lista de todos os reps
        /// </summary>
        /// <param name="connectionStr">Conexão da base de onde será carregado os Rep's</param>
        /// <returns></returns>
        public static IList<Modelo.REP> GetReps(string connectionStr)
        {
            IList<Modelo.REP> retorno = new List<Modelo.REP>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = @"
                            select 
                                r.*
                                , eh.nomeFabricante as empresaNome
                                , eh.nomeModelo as modeloNome 
                            from rep r 
                            left join equipamentohomologado eh on r.idequipamentohomologado = eh.id";
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<Modelo.REP> reps = new List<Modelo.REP>();
                                reps = MetodosAuxiliares.DataReaderMapToList<Modelo.REP>(reader);
                                retorno = reps;
                            }
                        }
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}