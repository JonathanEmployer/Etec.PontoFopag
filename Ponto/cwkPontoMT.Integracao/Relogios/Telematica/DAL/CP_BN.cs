using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public class CP_BN : DALBase
    {
        public CP_BN(string conexao)
            : base(conexao)
        {
            #region comandos
            INSERT = @"INSERT INTO TELESSVR.CP_BN
                                    ( END_IP ,
                                      BLUEB ,
                                      AVISO ,
                                      ST ,
                                      TERM,
                                      EPN3
                                    )
                            VALUES  ( @END_IP ,
                                      @BLUEB ,
                                      @AVISO ,
                                      @ST ,
                                      @TERM,
                                      @EPN3
                                    )";

            UPDATE = @"UPDATE TELESSVR.CP_BN SET END_IP = @END_IP, BLUEB = @BLUEB, AVISO = @AVISO , ST = @ST , TERM = @TERM, EPN3 = @EPN3";

            DELETE = @"DELETE TELESSVR.CP_BN WHERE END_IP = @END_IP";
            #endregion

        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@END_IP" , SqlDbType.VarChar),
				new SqlParameter ("@BLUEB"  , SqlDbType.VarChar),
				new SqlParameter ("@AVISO"  , SqlDbType.VarChar),
				new SqlParameter ("@ST"     , SqlDbType.VarChar),
                new SqlParameter ("@TERM"   , SqlDbType.VarChar),
                new SqlParameter ("@EPN3"   , SqlDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters<T>(SqlParameter[] parms, T obj)
        {
            Modelo.CP_BN objConv = (Modelo.CP_BN)Convert.ChangeType(obj, typeof(Modelo.CP_BN));
            parms[0].Value = objConv.END_IP;
            parms[1].Value = objConv.BLUEB;
            parms[2].Value = objConv.AVISO;
            parms[3].Value = objConv.ST;
            parms[3].Value = objConv.TERM;
            parms[3].Value = objConv.EPN3;
        }

        public Modelo.CP_BN LoadObject(string ip, string comando)
        {
            SqlParameter[] parms = new SqlParameter[]
			{
                new SqlParameter ("@END_IP", SqlDbType.VarChar),
                new SqlParameter ("@CC", SqlDbType.VarChar)
            };

            parms[0].Value = Telematica.BLL.Util.ConvertIP15Digitos(ip);
            parms[1].Value = comando;

            string sql = "SELECT * FROM TELESSVR.CP_BN WHERE END_IP = @END_IP and AVISO = @CC";

            DataTable dr = ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.CP_BN> retorno = new List<Modelo.CP_BN>();
            retorno = DataReaderMapToList<Modelo.CP_BN>(dr);
            return retorno.FirstOrDefault();
        }
    }
}
