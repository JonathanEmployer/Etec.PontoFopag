using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public class DAT07 : DALBase
    {
        public DAT07(string conexao) : base (conexao)
        {
            #region Comandos

            INSERT = @"INSERT INTO TELESSVR.DAT07
                            ( 
                              END_IP ,
                              BLUEB ,
                              CODIN ,
                              DESC_END ,
                              LACES ,
                              TIP_TERM ,
                              FUSO ,
                              PLANTA ,
                              BIO_TIPO ,
                              TIP_LEIT 
                            )
                       VALUES  
                            ( 
                              @END_IP,
                              '00' , -- BLUEB - char(2) -- SEMPRE FIXO
                              '00' , -- CODIN - char(2) -- SEMPRE FIXO
                              @DESC_END,
                              @LACES,
                              '3' , -- TIP_TERM - char(1)  -- SEMPRE FIXO
                              @FUSO,
                              '001' , -- PLANTA - char(3) -- SEMPRE FIXO
                              @BIO_TIPO,
                              @TIP_LEIT 
                            )";

            UPDATE = @"UPDATE TELESSVR.DAT07 SET END_IP = @END_IP, DESC_END = @DESC_END, FUSO = @FUSO, BIO_TIPO = @BIO_TIPO WHERE LACES = @LACES";

            DELETE = @"DELETE FROM TELESSVR.DAT07 WHERE LACES = @LACES";

            #endregion
        }

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@END_IP", SqlDbType.VarChar),
				new SqlParameter ("@DESC_END", SqlDbType.VarChar),
				new SqlParameter ("@LACES", SqlDbType.VarChar),
				new SqlParameter ("@FUSO", SqlDbType.VarChar),
                new SqlParameter ("@BIO_TIPO", SqlDbType.VarChar),
                new SqlParameter ("@TIP_LEIT", SqlDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters<T>(SqlParameter[] parms, T obj)
        {
            Modelo.DAT07 objConv = (Modelo.DAT07)Convert.ChangeType(obj, typeof(Modelo.DAT07));
            parms[0].Value = objConv.END_IP;
            parms[1].Value = objConv.DESC_END;
            parms[2].Value = objConv.LACES;
            parms[3].Value = objConv.FUSO;
            parms[4].Value = objConv.BIO_TIPO;
            parms[5].Value = objConv.TIP_LEIT;
        }
    }
}
