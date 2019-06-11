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
    public class RelatorioHoraExtra
    {
        private DataBase db;
        public RelatorioHoraExtra(DataBase database)
        {
            db = database;
        }

        public DataTable GetHorasExtrasMetasDepartamentos(DateTime dataIni, DateTime dataFin, int tipo, Int32[] ids)
        {

            SqlParameter[] parms = new SqlParameter[]
            { 
                    new SqlParameter("@dataIni", SqlDbType.Date),
                    new SqlParameter("@dataFin", SqlDbType.Date),
                    new SqlParameter("@tipo", SqlDbType.Int),
                    new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = dataIni;
            parms[1].Value = dataFin;
            parms[2].Value = tipo;
            parms[3].Value = string.Join(",", ids);

            string sql = @"select idEmpresa,
								codEmpresa,
								nomeEmpresa,
								idDepto,
                                codDepto,
	                            descDepto,
	                            mesInt,
	                            mesNome,
	                            anoInt,
	                            PercMaxHE,
	   	                            case when sum(hed) + sum(hen) > 0 then
			                        convert(decimal(10,2),(convert(decimal(10,2),(sum(hed) + sum(hen))) * 100.00) / convert(decimal(10,2),sum(totalHoras))) 
			                        else convert(decimal(10,2),0) end percHE
                            from (
                            select idEmpresa,
								   codEmpresa,
								   nomeEmpresa,
								   idDepto,
                                   codDepto,
	                               descDepto,
	                               mesInt,
								   data,
	                               DATENAME(month, '01/'+convert(varchar(2),mesInt)+'/'+convert(varchar(4),anoInt)) mesNome, 
	                               anoInt,
	                               PercMaxHE,
	                               hed,
	                               hen,
	                               htd + htn + bhc + hen + hed totalHoras
	                              from (
		                            select e.id idEmpresa,
										   e.codigo codEmpresa,
										   e.nome nomeEmpresa,
										   d.id idDepto,
                                           d.codigo codDepto,
			                               d.descricao descDepto,
			                               d.PercentualMaximoHorasExtras PercMaxHE,
										   m.data,
										   p.DiaFechamentoInicial,
			                               case when DAY(m.data) >= isnull(p.DiaFechamentoInicial,0) then 
													case when MONTH(m.data)+1 > 12 then
															1 
													else MONTH(m.data)+1 end
												else MONTH(m.data) end mesInt, 
										   case when DAY(m.data) >= isnull(p.DiaFechamentoInicial,0) and MONTH(m.data)+1 > 12 then 
															YEAR(m.data) + 1
												else YEAR(m.data) end anoInt, 
			                               dbo.FN_CONVHORA(m.horastrabalhadas) htd, 
			                               dbo.FN_CONVHORA(m.horastrabalhadasnoturnas) htn, 
			                               dbo.FN_CONVHORA(m.bancohorascre) bhc,
			                               dbo.FN_CONVHORA(m.horasextranoturna) hen,
			                               dbo.FN_CONVHORA(m.horasextrasdiurna) hed
		                              from marcacao_view m
		                             inner join funcionario f on m.dscodigo = f.dscodigo
		                             inner join departamento d on d.id = f.iddepartamento
									 inner join empresa e on e.id = d.idempresa
									 cross join (select top(1) * from parametros) p
		                             where convert(date,m.data) between @dataIni AND @dataFin
		                               and d.PercentualMaximoHorasExtras is not null
		                               and ((@Tipo = 0 and (d.idEmpresa in (select * from F_RetornaTabelaLista(@ids,',')))) or
		                                     @tipo = 1 and (d.id in (select * from F_RetornaTabelaLista(@ids,','))))
		                               ) x
	                               ) t
                            group by
								   idEmpresa,
								   codEmpresa,
								   nomeEmpresa, 
								   idDepto, 
								   codDepto,
	                               descDepto,
	                               mesInt,
	                               mesNome,
	                               anoInt,
	                               PercMaxHE";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReaderPT(CommandType.Text, sql, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }
    }
}
