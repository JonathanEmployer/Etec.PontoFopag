using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public class REPAFD001 : DALBase
    {
        private string SqlGetALL = @" SELECT *,
		                                    TRY_PARSE(FORMAT(CAST(SUBSTRING(INFORM, 11, 12) AS BIGINT),'##/##/#### ##:##') AS DATETIME USING 'pt-BR') DATAHORA
                                        FROM TELESSVR.REPAFD001 
                                       WHERE 1 = 1 ";

        public REPAFD001(string conn)
            : base(conn)
        {

        }

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters<T>(System.Data.SqlClient.SqlParameter[] parms, T obj)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.REPAFD001> GetAFDPeriodo(string END_IP, DateTime DataIni, DateTime DataFin)
        {
            SqlParameter[] parms = new SqlParameter[]
			{
                new SqlParameter ("@END_IP", SqlDbType.VarChar),
                new SqlParameter ("@DataIni", SqlDbType.DateTime),
                new SqlParameter ("@DataFin", SqlDbType.DateTime)

            };

            parms[0].Value = Telematica.BLL.Util.ConvertIP15Digitos(END_IP);
            parms[1].Value = DataIni;
            parms[2].Value = DataFin;

            string sql = @"SELECT * FROM ( " 
                                           + SqlGetALL + 
                                            " AND END_IP = @END_IP AND CONVERT(BIGINT, NSR) > 0 /*Não pegar o NSR 0 pois ele não segue o padão geral e da problema da conversão de data dos registros*/"+
                                         ") T WHERE T.DATAHORA BETWEEN @DataIni AND @DataFin ";

            DataTable dr = ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.REPAFD001> retorno = new List<Modelo.REPAFD001>();
            retorno = DataReaderMapToList<Modelo.REPAFD001>(dr);
            return retorno.OrderBy(o => o.NSR).ToList();
        }

        public List<Modelo.REPAFD001> GetAFDNSR(string numSerieRep, int nsrIni, int nsrFin)
        {
            SqlParameter[] parms = new SqlParameter[]
			{
                new SqlParameter ("@numSerieRep", SqlDbType.VarChar),
                new SqlParameter ("@nsrIni", SqlDbType.BigInt),
                new SqlParameter ("@nsrFin", SqlDbType.BigInt)

            };

            parms[0].Value = numSerieRep;
            parms[1].Value = nsrIni;
            parms[2].Value = nsrFin;

            string sql = SqlGetALL + " AND CONVERT(BIGINT, NSR) BETWEEN @nsrIni AND @nsrFin AND REP = @numSerieRep";

            DataTable dr = ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.REPAFD001> retorno = new List<Modelo.REPAFD001>();
            retorno = DataReaderMapToList<Modelo.REPAFD001>(dr);
            return retorno.OrderBy(o => o.NSR).ToList();
        }

        public int GetLastNsr(string numSerieRep)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@numSerie", SqlDbType.VarChar)
                };
                parms[0].Value = numSerieRep;

                string sql = @"SELECT MAX(NSR) 
                               FROM TELESSVR.REPAFD001
                               WHERE REP = @numSerie";

                DataTable dr = ExecuteReader(CommandType.Text, sql, parms);
                int retorno = int.Parse(dr.Rows[0][0].ToString());

                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível retornar o último NSR, mensagem: " + e.Message);
            }
        }
    }
}
