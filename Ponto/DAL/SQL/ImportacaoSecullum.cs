using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace DAL.SQL
{
    public class ImportacaoSecullum : IImportacaoSecullum
    {
        private DataBase db;
        public ImportacaoSecullum(DataBase database)
        {
            db = database;
        }

        public Hashtable GetHashDepartamento()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, codigo FROM departamento";
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return lista;
        }

        public Hashtable GetHashFuncao()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, codigo FROM funcao";
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (!lista.ContainsKey(Convert.ToInt32(dr["codigo"])))
                            lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return lista;
        }

        public Hashtable GetHashFuncionario()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, dscodigo FROM funcionario";
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToString(dr["dscodigo"]), Convert.ToInt32(dr["id"]));
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return lista;
        }
    }
}
