using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class LoteCalculoFuncionario
    {
        private static string INSERT = @"INSERT INTO LoteCalculo(DataInicio, DataFim) VALUES (@DataInicio, @DataFim)";
        public Modelo.Cw_Usuario UsuarioLogado { get; set; }

        public Guid Adicionar(DateTime dataInicio, DateTime dataFim, List<int> idsFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@DataInicio", SqlDbType.Date),
                new SqlParameter("@DataFim", SqlDbType.Date)
            };
            parms[0].Value = dataInicio;
            parms[1].Value = dataFim;

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(null, CommandType.Text, INSERT, true, parms);
            return Guid.Parse(cmd.Parameters["@id"].Value.ToString());
        }
    }
}