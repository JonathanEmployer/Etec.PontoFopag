using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;

namespace DAL.FB
{
    public abstract class DataBase : IDataBase
    {
        #region ConnectionString

        //public static string CONN_STRING;

        //static DataBase()
        //{
            //CONN_STRING = Modelo.cwkGlobal.CONN_STRING;

            //try
            //{
            //    XmlDocument doc = new XmlDocument();
            //    doc.Load("DAL.xml");
            //    XmlNode no = doc.SelectSingleNode("DAL").SelectSingleNode("cwkConnectionString").SelectSingleNode("Fb");
            //    CONN_STRING = no.Attributes["connectionString"].Value.ToString();
            //}
            //catch (Exception)
            //{
                
            //}
        //}    

        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
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
        public static int ExecuteNonQuery(FbTransaction trans, CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            FbCommand cmd = new FbCommand();
            PrepareParameters(cmdParms, true);
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }
        #endregion

        #region ExecuteNonQueryCmd
        public static FbCommand ExecNonQueryCmd(CommandType cmdType, string cmdText, bool removeQuote, params FbParameter[] cmdParms)
        {
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
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
            }
            return cmd;
        }
        public static FbCommand ExecNonQueryCmd(FbTransaction trans, CommandType cmdType, string cmdText, bool removeQuote, params FbParameter[] cmdParms)
        {
            FbCommand cmd = new FbCommand();
            PrepareParameters(cmdParms, removeQuote);
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            return cmd;
        }
        #endregion

        #region ExecuteReader
        public static FbDataReader ExecuteReader(CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            FbDataReader dr = null;
            FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING);
            try
            {
                FbCommand cmd = new FbCommand();
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
        public static FbDataReader ExecuteReader(FbTransaction trans, CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            FbDataReader dr = null;
            try
            {
                FbCommand cmd = new FbCommand();
                PrepareParameters(cmdParms, true);
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dr;
        }
        #endregion

        #region ExecuteReaderDs
        public static DataSet ExecuteReaderDs(CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                try
                {
                    FbCommand cmd = new FbCommand();
                    PrepareParameters(cmdParms, true);
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    FbDataAdapter da = new FbDataAdapter(cmd);
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw (ex);
                }
            }
            return ds;
        }
        public static DataSet ExecuteReaderDs(FbTransaction trans, CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            try
            {
                FbCommand cmd = new FbCommand();
                PrepareParameters(cmdParms, true);
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
                FbDataAdapter da = new FbDataAdapter(cmd);
                da.Fill(ds);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ds;
        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            object obj = new object();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                try
                {
                    FbCommand cmd = new FbCommand();
                    PrepareParameters(cmdParms, true);
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    conn.Dispose();
                    throw (ex);
                }
            }
            return obj;
        }
        public static object ExecuteScalar(FbTransaction trans, CommandType cmdType, string cmdText, params FbParameter[] cmdParms)
        {
            FbCommand cmd = new FbCommand();
            PrepareParameters(cmdParms, true);
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            object obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return obj;
        }
        #endregion

        #region PrepareCommand
        private static void PrepareCommand(FbCommand cmd, FbConnection conn, FbTransaction trans, CommandType cmdType, string cmdText, FbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (FbParameter parm in cmdParms)
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
        public static void PrepareParameters(FbParameter[] cmdParms, bool removeQuote)
        {
            if (cmdParms != null)
            {
                foreach (FbParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                    {
                        parm.Value = DBNull.Value;
                    }
                    else if ((parm.DbType.Equals(DbType.String) || parm.DbType.Equals(DbType.AnsiString) || parm.DbType.Equals(DbType.AnsiStringFixedLength)) && parm.Value != DBNull.Value)
                    {
                        if (removeQuote)
                        {
                            parm.Value = RemoveQuote(parm.Value.ToString());
                        }
                        if (parm.Value.ToString() == String.Empty)
                            parm.Value = DBNull.Value;
                    }
                }
            }
        }
        #endregion

        #region RemoveQuote
        protected internal static string RemoveQuote(string text)
        {
            text = text.Replace("'", String.Empty);
            text = text.Replace("\"", String.Empty);
            text = text.Replace("´", String.Empty);
            return text;
        }
        #endregion

        /// <summary>
        /// Executa uma lista de comandos SQL
        /// </summary>
        /// <param name="comandos">Lista de comandos</param>
        /// <returns>retorna verdadeiro caso exista comandos na lista, e false caso contrário.</returns>
        public static bool ExecutarComandos(List<string> comandos)
        {
            if (comandos != null)
            {
                if (comandos.Count > 0)
                {
                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        try
                        {
                            conn.Open();
                            FbBatchExecution batch = new FbBatchExecution(conn);
                            batch.SqlStatements.AddRange(comandos);
                            batch.Execute();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Ocorreu um erro ao executar os comandos: " + ex.Message, ex);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        
    }
}