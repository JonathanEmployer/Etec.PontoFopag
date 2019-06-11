using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public class SITCOLETOR : DALBase
    {
        public SITCOLETOR(string conexao) : base(conexao)
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

        public int VerificaStatusRep (string IP)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@IP", SqlDbType.VarChar)
            };

            parms[0].Value = IP;

            string SQL = @"SELECT  IIF(DATEDIFF(MINUTE, DT_ATUALI, GETDATE()) > 5, 0, 1) RepOn
                           FROM    TELESSVR.SITCOLETOR
                           WHERE   ENDIP = @IP";
            DataTable dr = ExecuteReader(CommandType.Text, SQL, parms);
            int retorno = int.Parse(dr.Rows[0][0].ToString());

            return retorno;
        }
    }
}
