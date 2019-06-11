using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DAL.SQL;

namespace MonitorJobs.DAL
{
    public abstract class DalBase
    {
        protected string conexao;

        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(conexao))
            {
                    conn.Open();
                    PrepareParameters(cmdParms, true);
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
            }
        }

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

        #region PrepareCommand
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
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

        #region RemoveQuote
        private static string RemoveQuote(string text)
        {
            text = text.Replace("'", String.Empty);
            text = text.Replace("\"", String.Empty);
            text = text.Replace("´", String.Empty);
            return text;
        }
        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataTable ExecuteReaderToDataTabela(string strConn, string sql, SqlParameter[] parms)
        {

            using (SqlConnection conexao = new SqlConnection(strConn))
            {
                SqlCommand command = new SqlCommand(sql, conexao);
                conexao.Open();

                if (parms != null)
                {
                    foreach (SqlParameter parm in parms)
                    {
                        if (command.Parameters.Contains(parm))
                            command.Parameters[parm.ParameterName] = parm;
                        else
                            command.Parameters.Add(parm);
                    }
                }

                //Tempo de espera de 5 minutos (300000 mileseconds)
                command.CommandTimeout = 300000;
                SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
        }


        #region Mapeamento
        public List<T> DataTableMapToList<T>(DataTable dt)
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
                            var valor =  DALBase.ChangeType(row[prop.Name], prop.PropertyType);
                            prop.SetValue(obj, valor, null);
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        #endregion
    }
}
