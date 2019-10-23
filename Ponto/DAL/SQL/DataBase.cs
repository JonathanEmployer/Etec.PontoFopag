using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
namespace DAL.SQL
{
    public class DataBase : IDataBase
    {
        private string _ConnectionString;
        internal string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
            }
        }
        public DataBase(string connString)
        {
            SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(connString);
            conn.MaxPoolSize = 5000;
            ConnectionString = conn.ConnectionString;
            if (string.IsNullOrEmpty(connString))
            {
                connString = Modelo.cwkGlobal.CONN_STRING;
            }
        }
        internal SqlConnection GetConnection
        {
            get
            {
                //Foi feito esse tratamento no erro pois existe um problema no ponto web ao tentar usar a rotina de exportação para o ministerio do trabalho
                //para exportar arquivos do tipo ACJEF a conexão esta sendo perdida, sem motivo aparente. Nenhuma outra tentativa de resolução deste problema funcionou.
                try
                {
                    return TentaAtribuirConexao();
                }
                catch (Exception)
                {
                    //throw ex;
                    try
                    {
                        return TentaAtribuirConexao();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        private SqlConnection TentaAtribuirConexao()
        {
            SqlConnection conn = null;
            if (String.IsNullOrEmpty(this.ConnectionString))
            {
                this.ConnectionString = geraConnectionString();
            }
            conn = new SqlConnection(this.ConnectionString);

            conn.Open();
            return conn;
        }

        private static string geraConnectionString()
        {
            try
            {
                throw new Exception("Gerando conexão indevidamente");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal static bool ConnStringIgual(string str1, string str2)
        {
            try
            {
                string[] v1 = str1.Split(';');
                string[] v2 = str2.Split(';');
                if (v1[0] == v2[0] && v1[1] == v2[1])
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static string SetLanguage(string cmdText)
        {
            return "SET LANGUAGE 'Brazilian'; " + cmdText;
        }

        #region ExecuteNonQuery
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = GetConnection)
            {
                try
                {
                    PrepareParameters(cmdParms, true);
                    cmd.CommandTimeout = 600000;
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    return val;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw (ex);
                }
            }
        }
        public int ExecuteNonQueryNoKey(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = GetConnection)
            {
                try
                {
                    PrepareParameters(cmdParms, true);
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw (ex);
                }
            }
        }

        #endregion

        #region ExecuteNonQueryCmd
        public SqlCommand ExecNonQueryCmd(CommandType cmdType, string cmdText, bool removeQuote, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 300;
            SqlConnection conn = GetConnection;
            try
            {
                PrepareParameters(cmdParms, removeQuote);
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }
            return cmd;
        }
        #endregion

        #region ExecuteReader
        public SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlDataReader dr = null;
            SqlConnection conn = GetConnection;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 1000;
                PrepareParameters(cmdParms, true);
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }
            return dr;
        }

        public DataTable ExecuteReaderToDataTable(string cmdText, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = GetConnection;
            DataTable ret = new DataTable();
            PrepareParameters(cmdParms, true);
            using (SqlDataAdapter comm = new SqlDataAdapter(cmdText, conn))
            {
                foreach (SqlParameter param in cmdParms)
                {
                    if (param.Value != null)
                    {
                        comm.SelectCommand.Parameters.Add(param);
                    }
                }
                // adaptor fill table function
                comm.Fill(ret);
            }
            conn.Close();
            return ret;
        }

        public SqlDataReader ExecuteReaderPT(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteReader(cmdType, SetLanguage(cmdText), cmdParms);
        }
        #endregion

        #region ExecuteReaderDs
        public DataSet ExecuteReaderDs(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            SqlConnection conn = GetConnection;
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareParameters(cmdParms, true);
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }
            return ds;
        }
        #endregion

        #region ExecuteScalar
        public object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            object obj = new object();
            SqlConnection conn = GetConnection;
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareParameters(cmdParms, true);
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return obj;
        }
        #endregion

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
        public void PrepareParameters(SqlParameter[] cmdParms, bool removeQuote)
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

        public bool ExecutarComandos(List<string> comandos, int limite)
        {
            string comandosSql = null;
            int Controle = 1;
            SqlCommand comandoSQL;
            if (comandos != null)
            {
                if (comandos.Count > 0)
                {
                    SqlConnection conn = GetConnection;
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i <= (comandos.Count - 1); i++)
                            {
                                if (i == (Controle * limite))
                                {
                                    comandosSql = comandosSql + comandos[i] + ";\n";
                                    comandoSQL = new SqlCommand(comandosSql);
                                    comandoSQL.Connection = conn;
                                    comandoSQL.Transaction = trans;
                                    comandoSQL.ExecuteNonQuery();

                                    Controle++;
                                    comandosSql = null;
                                }
                                else
                                {
                                    comandosSql = comandosSql + comandos[i] + ";";
                                }
                            }

                            if (comandosSql != null && comandosSql != "")
                            {
                                comandoSQL = new SqlCommand(comandosSql.ToString());
                                comandoSQL.Connection = conn;
                                comandoSQL.Transaction = trans;
                                comandoSQL.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw new Exception("Ocorreu um erro ao executar os comandos: " + ex.Message, ex);
                        }
                        finally
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public SqlParameter[] AddArrayParameters<T>(SqlCommand cmd, IEnumerable<T> values, string paramNameRoot, int start = 1, string separator = ", ")
        {
            /* An array cannot be simply added as a parameter to a SqlCommand so we need to loop through things and add it manually. 
             * Each item in the array will end up being it's own SqlParameter so the return value for this must be used as part of the
             * IN statement in the CommandText.
             */
            var parameters = new List<SqlParameter>();
            var parameterNames = new List<string>();
            var paramNbr = start;
            foreach (var value in values)
            {
                var paramName = string.Format("@{0}{1}", paramNameRoot, paramNbr++);
                parameterNames.Add(paramName);
                parameters.Add(cmd.Parameters.AddWithValue(paramName, value));
            }

            cmd.CommandText = cmd.CommandText.Replace("{" + paramNameRoot + "}", string.Join(separator, parameterNames));

            return parameters.ToArray();
        }
    }
}