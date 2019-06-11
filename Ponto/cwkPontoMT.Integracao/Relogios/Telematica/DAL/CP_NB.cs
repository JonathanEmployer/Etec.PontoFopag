using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public class CP_NB : DALBase
    {
        public CP_NB(string conexao) : base (conexao)
        {
            #region comandos
            INSERT = @"INSERT INTO TELESSVR.CP_NB
                                    ( END_IP ,
                                      BLUEB ,
                                      CODIN ,
                                      ST ,
                                      CC ,
                                      TIPO_CC ,
                                      ACAO
                                    )
                            VALUES  ( @END_IP,
                                      '00' , -- BLUEB - char(2) - SEMPRE FIXO
                                      '00' , -- CODIN - char(2) - SEMPRE FIXO
                                      @ST,
                                      @CC,
                                      '0' , 
                                      @ACAO
                                    )";
            UPDATE = @"UPDATE TELESSVR.CP_NB SET ST = @ST, CC = @CC, ACAO = @ACAO WHERE END_IP = @END_IP";

            DELETE = @"DELETE TELESSVR.CP_NB WHERE END_IP = @END_IP";
            #endregion

        }

//        public override int Incluir(string st, string cc, string acao, string ip)
//        {
//            int ret = 0;
//            SqlParameter[] parms = new SqlParameter[]
//            {
//                new SqlParameter ("@ST", SqlDbType.VarChar),
//                new SqlParameter ("@CC", SqlDbType.VarChar),
//                new SqlParameter ("@Acao", SqlDbType.VarChar),
//                new SqlParameter ("@IP", SqlDbType.VarChar),
//            };
//            parms[0].Value = st;
//            parms[1].Value = cc;
//            parms[2].Value = acao;
//            parms[3].Value = ip;

//            string aux = @"UPDATE TELESSVR.CP_NB 
//                            SET ST = @ST, CC = @CC, ACAO = @Acao 
//                            WHERE END_IP = '@IP'";
//            ret = ExecuteNonQuery(CommandType.Text, aux, parms);

//            return ret;
//        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@END_IP", SqlDbType.VarChar),
				new SqlParameter ("@ST", SqlDbType.VarChar),
				new SqlParameter ("@CC", SqlDbType.VarChar),
				new SqlParameter ("@ACAO", SqlDbType.VarChar)
			};
            return parms;
        }

        //protected override bool SetInstance<T>(SqlDataReader dr, T obj)
        //{
        //    DALBase.DataReaderMapToList();
        //}

        protected override void SetParameters<T>(SqlParameter[] parms, T obj)
        {
            Modelo.CP_NB objConv = (Modelo.CP_NB)Convert.ChangeType(obj, typeof(Modelo.CP_NB));
            parms[0].Value = objConv.END_IP;
            parms[1].Value = objConv.ST;
            parms[2].Value = objConv.CC;
            parms[3].Value = objConv.ACAO;
        }

        public Modelo.CP_NB LoadObject(string ip)
        {
            SqlParameter[] parms = new SqlParameter[]
			{
                new SqlParameter ("@END_IP", SqlDbType.VarChar)
            };

            parms[0].Value = Telematica.BLL.Util.ConvertIP15Digitos(ip);

            string sql = "SELECT * FROM TELESSVR.CP_NB WHERE END_IP = @END_IP";

            DataTable dr = ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.CP_NB> retorno = new List<Modelo.CP_NB>();
            retorno = DataReaderMapToList<Modelo.CP_NB>(dr);
            return retorno.FirstOrDefault();
        }
    }
}
