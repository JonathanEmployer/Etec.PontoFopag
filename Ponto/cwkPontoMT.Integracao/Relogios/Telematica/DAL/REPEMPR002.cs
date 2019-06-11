using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public class REPEMPR002 : DALBase
    {
        public REPEMPR002(string conexao) : base(conexao)
        {
            #region Comandos

            INSERT = @"INSERT INTO TELESSVR.REPEMPR002
                         ( IFUNC, PIS, NOME )
                       VALUES  
                       ( 
                        @IFUNC,
                        @PIS,
                        @NOME
                       )";

            UPDATE = @"UPDATE TELESSVR.REPEMPR002
                       SET IFUNC = @IFUNC, PIS = @PIS, NOME = @NOME
                       WHERE PIS = @PIS";

            DELETE = @"DELETE FROM TELESSVR.REPEMPR002
                       WHERE IFUNC = @IFUNC";

            #endregion
        }

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@IFUNC", SqlDbType.VarChar),
                new SqlParameter("@PIS", SqlDbType.VarChar),
                new SqlParameter("@NOME", SqlDbType.VarChar)
            };
            return parms;
        }

        protected override void SetParameters<T>(System.Data.SqlClient.SqlParameter[] parms, T obj)
        {
            Modelo.REPEMPR002 objConv = (Modelo.REPEMPR002)Convert.ChangeType(obj, typeof(Modelo.REPEMPR002));
            parms[0].Value = objConv.IFUNC;
            parms[1].Value = objConv.PIS;
            parms[2].Value = objConv.NOME;
        }
    }
}
