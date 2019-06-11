using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MonitorJobs.DAL
{
    public class TabLogErro: DalBase
    {
        public TabLogErro(string conn)
        {
            conexao = conn;
        }

        public List<Models.TabLogErro> GetErros(DateTime data, string appId)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@dta_cadastro", SqlDbType.DateTime),
                new SqlParameter ("@appId", SqlDbType.VarChar)
            };
            parms[0].Value = data.Date;
            parms[1].Value = appId;
            List<Models.TabLogErro> bases = new List<Models.TabLogErro>();
            try
            {
                string sql = @"	SELECT Idf_Erro_Log
                                        , Dta_Cadastro
                                        , Aplicacao
                                        , Usuario
                                        , Centro_Servico
                                        , Empresa
                                        , Sessao
                                        , URL
                                        , Request
                                        , Response
                                        , Inner_Exception
                                        , Stack_Trace
                                        , Message
                                        , Source
                                        , MachineName
                                        , Custon_Message
                                        , URL_Referrer
                                        , WsRequest
                                        FROM log_erro.TAB_Erro_Log 
                                        WHERE Dta_Cadastro BETWEEN @dta_cadastro and dateadd(day,1,@dta_cadastro)
                                          AND Aplicacao = @appId
                                        ORDER BY Dta_Cadastro DESC
                                ";

                DataTable dt = ExecuteReaderToDataTabela(conexao, sql, parms);

                bases = DataTableMapToList<Models.TabLogErro>(dt);
                return bases;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}