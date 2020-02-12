using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class LoteCalculo : ILoteCalculo
    {
        protected DataBase db { get; set; }

        private static string INSERT = @"INSERT INTO LoteCalculo(DataInicio, DataFim, AltUsuario) OUTPUT Inserted.Id VALUES (@DataInicio, @DataFim, @UsuarioLogado)";
        public Modelo.Cw_Usuario UsuarioLogado { get; set; }

        public LoteCalculo(DataBase dataBase)
        {
            db = dataBase;
        }

        public Guid Adicionar(DateTime dataInicio, DateTime dataFim, string usuarioLogado)
        {
            Guid id = Guid.Empty;

            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlParameter[] parms = new SqlParameter[]
                    {
                        new SqlParameter("@DataInicio", SqlDbType.Date),
                        new SqlParameter("@DataFim", SqlDbType.Date),
                        new SqlParameter("@UsuarioLogado", SqlDbType.VarChar)
                    };
                    parms[0].Value = dataInicio;
                    parms[1].Value = dataFim;
                    parms[2].Value = usuarioLogado;

                    SqlDataReader dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, INSERT, parms);
                    if (dr.Read())
                        id = (Guid)dr["Id"];

                    dr.Close();
                    trans.Commit();
                }
                conn.Close();
            }
            return id;
        }
    }
}