using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public class REPEMPR001 : DALBase
    {

        public REPEMPR001(string conexao) : base(conexao)
        {
            #region Comandos

            INSERT = @"INSERT INTO TELESSVR.REPEMPR001
                       ( 
	                    IDENT ,
                        TIPO_ID ,
                        CEI ,
                        RSOCIAL ,
                        LOCAL_SERV
                       )
                       VALUES  
                       ( 
                        @IDENT,
                        @TIPO_ID,
                        @CEI,
                        @RSOCIAL,
                        @LOCAL_SERV
                       )";

            UPDATE = @"UPDATE TELESSVR.REPEMPR001
                       SET IDENT = @IDENT, TIPO_ID = @TIPO_ID, CEI = @CEI, RSOCIAL = @RSOCIAL, LOCAL_SERV = @LOCAL_SERV
                       WHERE IDENT = @IDENT";

            DELETE = @"DELETE FROM TELESSVR.REPEMPR001
                       WHERE IDENT = @IDENT";

            #endregion
        }

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@IDENT", SqlDbType.VarChar),
                new SqlParameter("@TIPO_ID", SqlDbType.VarChar),
                new SqlParameter("@CEI", SqlDbType.VarChar),
                new SqlParameter("@RSOCIAL", SqlDbType.VarChar),
                new SqlParameter("@LOCAL_SERV", SqlDbType.VarChar)
            };
            return parms;
        }

        protected override void SetParameters<T>(System.Data.SqlClient.SqlParameter[] parms, T obj)
        {
            Modelo.REPEMPR001 objConv = (Modelo.REPEMPR001)Convert.ChangeType(obj, typeof(Modelo.REPEMPR001));
            parms[0].Value = objConv.IDENT;
            parms[1].Value = objConv.TIPO_ID;
            parms[2].Value = objConv.CEI;
            parms[3].Value = objConv.RSOCIAL;
            parms[4].Value = objConv.LOCAL_SERV;
        }
    }
}
