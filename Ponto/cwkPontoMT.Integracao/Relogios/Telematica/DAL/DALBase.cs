using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.DAL
{
    public abstract class DALBase
    {
        protected DALBase(string conn)
        {
            CONEXAO = conn;
        }

        protected virtual string CONEXAO { get; set; }

        protected virtual string INSERT { get; set; }

        protected virtual string UPDATE { get; set; }

        protected virtual string DELETE { get; set; }

        //protected abstract bool SetInstance<T>(SqlDataReader dr, T obj);

        protected abstract SqlParameter[] GetParameters();

        protected abstract void SetParameters<T>(SqlParameter[] parms, T obj);

        public virtual int Incluir<T>(T obj)
        {
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            return ExecuteNonQuery(CommandType.Text, INSERT, parms);
        }

        public virtual int Alterar<T>(T obj)
        {
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            return ExecuteNonQuery(CommandType.Text, UPDATE, parms);
        }

        public virtual int Deletar<T>(T obj)
        {
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            return ExecuteNonQuery(CommandType.Text, DELETE, parms);
        }


        #region ExecuteNonQuery
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            using (SqlConnection conn = new SqlConnection(CONEXAO))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    conn.Open();
                    try
                    {
                        PrepareParameters(cmdParms, true);
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        int ret = cmd.ExecuteNonQuery();
                        conn.Close();
                        return ret;

                    }
                    catch (Exception e)
                    {
                        if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                        throw e;
                    }
                }
            }
            
        }
        #endregion

        #region ExecuteReader
        public DataTable ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(CONEXAO))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                    {
                        conn.Open();
                        try
                        {
                            PrepareParameters(cmdParms, true);
                            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                            int ret = cmd.ExecuteNonQuery();
                            SqlDataReader dr = cmd.ExecuteReader();
                            dataTable.Load(dr);
                            dr.Close();
                            conn.Close();
                            return dataTable;
                        }
                        catch (Exception e)
                        {
                            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
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

        #region PrepareCommand
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 300;

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

        #region Mapeamento
        public List<T> DataReaderMapToList<T>(DataTable dt)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            List<string> colunas = dt.Columns
                                     .Cast<DataColumn>()
                                     .Select(x => x.ColumnName)
                                     .ToList();
            foreach (DataRow row in dt.Rows)
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (colunas.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        if (!object.Equals(row[prop.Name], DBNull.Value))
                        {
                            try
                            {
                                var valor = ChangeType(row[prop.Name], prop.PropertyType);
                                prop.SetValue(obj, valor, null);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
        #endregion
    }
}
