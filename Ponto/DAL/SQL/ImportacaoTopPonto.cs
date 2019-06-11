using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace DAL.SQL
{
    public class ImportacaoTopPonto : IImportacaoTopPonto
    {
        private DataBase db;
        public ImportacaoTopPonto(DataBase database)
        {
            db = database;
        }

        public Hashtable GetHashEmpresa()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, codigo FROM empresa";
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

        public Hashtable GetHashFuncaoDescricao()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, descricao FROM funcao";
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (!lista.ContainsKey(dr["descricao"].ToString()))
                            lista.Add(dr["descricao"].ToString(), Convert.ToInt32(dr["id"]));
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return lista;
        }

        public Hashtable GetHashHorario()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, codigo FROM horario";
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

        public Hashtable GetHashFuncCodigoDscodigo()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT codigo, dscodigo FROM funcionario";
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToString(dr["dscodigo"]));
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return lista;
        }

        public Hashtable GetHashOcorrenciaDescId()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, descricao FROM ocorrencia";
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lista.Add(Convert.ToString(dr["descricao"]), Convert.ToInt32(dr["id"]));
                    }
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return lista;
        }

        public Hashtable GetHashOcorrenciaCodigoId()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT id, codigo FROM ocorrencia";
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

        public Hashtable GetHashFuncCodigoId()
        {
            Hashtable lista = new Hashtable();

            SqlParameter[] parms = new SqlParameter[] { };
            string cmd = "SELECT codigo, id FROM funcionario";
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
    }
}
