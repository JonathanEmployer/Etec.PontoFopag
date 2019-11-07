using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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


        public DataTable GetRelatorioAFDPortaria373(Dictionary<int, string> lIdEmpAndNumRep, DateTime pDataInicial, DateTime pDataFinal)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                new SqlParameter("@dataInicial", SqlDbType.DateTime),
                new SqlParameter("@dataFinal", SqlDbType.DateTime)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string filtroEmpresaRelogio = string.Join(" or ", lIdEmpAndNumRep.Select(s => string.Format(" (f.idempresa = {0} and b.relogio = '{1}') ", s.Key, s.Value)));

            string aux = string.Format(@"
                      SELECT ISNULL(CASE WHEN b.nsr = 0 THEN b.id ELSE b.nsr END, 0) nsr
		                    ,'3' TipoReg
		                    ,b.data Data
		                    ,b.hora Hora
		                    ,f.pis PIS
		                    ,b.relogio
		                    ,f.idempresa
                       FROM dbo.bilhetesimp b with (nolock)
                      INNER JOIN dbo.funcionario f ON b.dscodigo = f.dscodigo
                      WHERE 1 = 1
                        AND b.data BETWEEN @dataInicial AND @dataFinal
                        AND {0}
                      ORDER BY idempresa, b.relogio, b.nsr, b.id ", filtroEmpresaRelogio);
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