using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ModelPonto = RegistradorPontoWeb.Models.Ponto;

namespace RegistradorPontoWeb.Controllers.DAL.Ponto
{
    public class FechamentoPonto : DALBase
    {
        public FechamentoPonto(string conexao)
            : base(conexao)
        {
            #region comandos
            INSERT = @"";

            UPDATE = @"";

            DELETE = @"";
            #endregion

        }

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters<T>(System.Data.SqlClient.SqlParameter[] parms, T obj)
        {
            throw new NotImplementedException();
        }

        public ModelPonto.FechamentoPonto GetUltimoFechamentoPonto(Int64 idFunc)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idFunc", idFunc)
            };

            parms[0].Value = idFunc;

            string sql = @"SELECT fp.DataUltimoFechamento,
	                               fb.ultimoFechamentoBH DataUltimoFechamentoBH
                              FROM funcionario func 
                             OUTER APPLY (SELECT top(1) fbh.data ultimoFechamentoBH
 			                            FROM dbo.fechamentobh fbh
 			                            INNER JOIN dbo.fechamentobhd fbhd ON fbh.id = fbhd.idfechamentobh
 			                            WHERE fbhd.identificacao = func.id
 			                            ORDER BY fbh.data DESC) fb
                             OUTER APPLY (SELECT top(1) datafechamento DataUltimoFechamento
 			                            FROM fechamentoponto fp
 			                            INNER JOIN fechamentopontofuncionario fpf ON fp.id = fpf.idfechamentoponto 
 			                            WHERE idfuncionario = func.id
 			                            ORDER BY datafechamento DESC) fp
                             WHERE func.id = @idFunc";

            DataTable dt = ExecuteReader(CommandType.Text, sql, parms);
            List<ModelPonto.FechamentoPonto> retorno = new List<ModelPonto.FechamentoPonto>();
            retorno = DataReaderMapToList<ModelPonto.FechamentoPonto>(dt);
            return retorno.FirstOrDefault();

        }
    }
}