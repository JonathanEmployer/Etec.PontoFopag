using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;

namespace DAL.SQL
{
    public class LoteCalculo : ILoteCalculo
    {
        private static string INSERT = @"INSERT INTO LoteCalculo(DataInicio, DataFim) VALUES (@DataInicio, @DataFim)";
        public Modelo.Cw_Usuario UsuarioLogado { get; set; }

        public Modelo.LoteCalculo Adicionar(Modelo.LoteCalculo obj)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@DataInicio", SqlDbType.Date),
                new SqlParameter("@DataFim", SqlDbType.Date)
            };
            parms[0].Value = ((Modelo.LoteCalculo)obj).DataInicio;
            parms[1].Value = ((Modelo.LoteCalculo)obj).DataFim;

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(null, CommandType.Text, INSERT, true, parms);
            obj.Id = Guid.Parse(cmd.Parameters["@id"].Value.ToString());
            return obj;
        }
    }
}