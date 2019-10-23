using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public static class TransactDbOps
    {
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareParameters(cmdParms, true);
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static SqlCommand ExecNonQueryCmd(SqlTransaction trans, CommandType cmdType, string cmdText, bool removeQuote, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 120;

            PrepareParameters(cmdParms, removeQuote);
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);

            int val = cmd.ExecuteNonQuery();
            return cmd;
        }

        public static SqlDataReader ExecuteReader(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlDataReader dr = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 120;
                PrepareParameters(cmdParms, true);
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                dr.Dispose();
                throw (ex);
            }
            return dr;
        }

        public static DataSet ExecuteReaderDs(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareParameters(cmdParms, true);
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ds;
        }

        public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareParameters(cmdParms, true);
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return obj;
        }

        public static bool ExecutarComandos(List<string> comandos, int limite, SqlTransaction trans)
        {
            SqlCommand comandoSQL;
            if (comandos != null && comandos.Count() > 0)
            {
                try
                {
                    IEnumerable<List<string>> comandosParts = splitList<string>(comandos, 1000);
                    foreach (List<string> comands in comandosParts)
                    {
                        comandoSQL = new SqlCommand(String.Join("; ", comands), trans.Connection, trans);
                        comandoSQL.CommandTimeout = 600;
                        comandoSQL.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return false;
        }

        public static IEnumerable<List<T>> splitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }

        public static void ValidaDependencia(SqlTransaction trans, Modelo.ModeloBase obj, string TABELA)
        {
            if (TABELA != null)
            {
                SqlParameter[] parms1 = new SqlParameter[] { new SqlParameter("@tabela", SqlDbType.VarChar, 60) };
                parms1[0].Value = TABELA;

                string sql = @" SELECT   f.name as tabela 
		                                , al.alias as alias 
		                                , c.name as campo 
                                FROM sys.foreign_keys a 
                                INNER JOIN sys.objects f ON f.object_id = a.parent_object_id 
                                INNER JOIN sys.objects p ON p.object_id = a.referenced_object_id 
                                INNER JOIN alias_tabela al ON al.tabela = f.name
                                INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = a.object_id 
                                INNER JOIN sys.columns c ON c.object_id = fkc.parent_object_id and c.column_id = fkc.parent_column_id 
                                WHERE p.name = @tabela  
                                AND a.delete_referential_action = 0";

                DataSet ds = TransactDbOps.ExecuteReaderDs(trans, CommandType.Text, sql, parms1);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (CountCampo(trans, dr["tabela"].ToString(), dr["campo"].ToString(), obj.Id, 0) > 0)
                    {
                        string alias = dr["alias"].ToString();
                        throw new Exception("O registro excluído está sendo utilizado no cadastro de " + alias + ".\n\nVerifique.");
                    }
                }

                ds.Dispose();
            }
        }

        public static int CountCampo(SqlTransaction trans, string tabela, string campo, double valor, int id)
        {
            StringBuilder str = new StringBuilder("SELECT ISNULL(COUNT(");
            str.Append(campo);
            str.Append("), 0) AS qt FROM ");
            str.Append(tabela);
            str.Append(" WHERE " + tabela + "." + campo);
            str.Append(" = @parametro");

            if (id > 0)
                str.Append(" AND " + tabela + ".id <> @id");

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@parametro", SqlDbType.Float, 4),
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = valor;
            parms[1].Value = id;

            return (int)TransactDbOps.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms);
        }

        public static int CountCampo(SqlTransaction trans, string tabela, string campo, int valor, int id)
        {
            StringBuilder str = new StringBuilder("SELECT ISNULL(COUNT(");
            str.Append(campo);
            str.Append("), 0) AS qt FROM ");
            str.Append(tabela);
            str.Append(" WHERE " + tabela + "." + campo);
            str.Append(" = @parametro");

            if (id > 0)
                str.Append(" AND " + tabela + ".id <> @id");

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@parametro", SqlDbType.Int, 4),
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = valor;
            parms[1].Value = id;

            return (int)TransactDbOps.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms);
        }

        public static int MaxCodigo(SqlTransaction trans, string MAXCOD)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[0];
                var cod = TransactDbOps.ExecuteScalar(trans, CommandType.Text, MAXCOD, parms);
                return Convert.ToInt32(cod) + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        #region Metodos Auxiliares
        #region PrepareCommand
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 600000;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if (cmd.Parameters.Contains(parm))
                        cmd.Parameters[parm.ParameterName] = parm;
                    else
                        cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

        #region PrepareParameters
        public static void PrepareParameters(SqlParameter[] cmdParms, bool removeQuote)
        {
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                    {
                        parm.Value = DBNull.Value;
                    }
                    else if ((parm.DbType.Equals(DbType.AnsiString) || parm.DbType.Equals(DbType.AnsiStringFixedLength)) && parm.Value != DBNull.Value)
                    {
                        if (removeQuote)
                        {
                            parm.Value = RemoveQuote(parm.Value.ToString());
                            if (parm.Value.ToString() == String.Empty)
                                parm.Value = DBNull.Value;
                        }
                    }
                }
            }
        }
        public static string GetParmsFucker(SqlParameter[] cmdParms)
        {
            string retorno = "";
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    retorno += string.Format("declare {0} as {1} = {2};{3}", parm.ParameterName, parm.SqlDbType, parm.Value, System.Environment.NewLine);
                }
            }

            return retorno;
        }
        #endregion

        #region RemoveQuote
        private static string RemoveQuote(string text)
        {
            text = text.Replace("'", String.Empty);
            text = text.Replace("\"", String.Empty);
            text = text.Replace("´", String.Empty);
            return text;
        }
        #endregion
        #endregion
    }
}
