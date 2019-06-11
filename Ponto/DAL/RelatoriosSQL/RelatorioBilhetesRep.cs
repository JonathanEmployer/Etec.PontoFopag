using AutoMapper;
using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.RelatoriosSQL
{
    public class RelatorioBilhetesRep
    {
        private DataBase db;
        public RelatorioBilhetesRep(DataBase database)
        {
            db = database;
        }

        /// <summary>
        /// Select com os dados para o relatório de Bilhetes por rep
        /// </summary>
        /// <param name="idRep">Id Rep</param>
        /// <param name="pDataInicial">Data inicial para o filtro do relatório</param>
        /// <param name="pDataFinal">Data Final para o filtro do relatório</param>
        /// <returns>DataTable</returns>
        public DataTable GetRelatorioBilhetesRep(int idRep, DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[3]
            { 
                    new SqlParameter("@IdRep", SqlDbType.Int),
                    new SqlParameter("@dataIni", SqlDbType.DateTime),
                    new SqlParameter("@dataFin", SqlDbType.DateTime)
            };
            parms[0].Value = idRep;
            parms[1].Value = pDataInicial;
            parms[2].Value = pDataFinal;

            string aux = @"select r.codigo,
	                               r.numrelogio,
	                               r.numserie,
	                               eh.nomefabricante,
	                               eh.nomemodelo,
	                               isnull(rh.local, r.local) localRep,
		                            b.data, 
		                            b.hora,
		                            b.nsr,
		                            b.pis,
		                            f.nome,
	                               case when b.importado = 1 then 'Importado' else 'Pendente' end Situacao,
	                               b.inchora,
	                               u.login,
	                               u.nome nomeUsuario
                              from rep r
                              left join equipamentohomologado eh on r.idequipamentohomologado = eh.id
                             inner join bilhetesimp b on r.numrelogio = b.relogio
                              left join cw_usuario u on b.incusuario = u.login
                              left join funcionario f on b.idfuncionario = f.id
                             outer apply (select top(1) local from rephistoricolocal rh where rh.idrep = r.id and rh.data >= b.data order by rh.data) rh
                             where r.id = @IdRep
                               and b.data between @dataIni and @dataFin
                              order by b.data, convert(time, b.hora), nsr;";
            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
    }
}