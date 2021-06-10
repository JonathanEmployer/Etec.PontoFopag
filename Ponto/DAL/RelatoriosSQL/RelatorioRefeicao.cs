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
	public class RelatorioRefeicao
	{
		private DataBase db;
		public RelatorioRefeicao(DataBase database)
		{
			db = database;
		}

		/// <summary>
		/// Select com os dados para o relatório de homem hora
		/// </summary>
		/// <param name="idsFuncionarios">String com ids separados por vingula. Ex: '1,2,3,25,36'</param>
		/// <param name="pDataInicial">Data inicial para o filtro do relatório</param>
		/// <param name="pDataFinal">Data Final para o filtro do relatório</param>
		/// <returns>DataTable</returns>
		public DataTable GetRelatorioRefeicao(List<int> idsFunc, DateTime pDataInicial, DateTime pDataFinal, int percJornadaMinima, decimal valorRefeicao, bool considerarDoisRegistros, bool considerarDiasSemjornada)
		{
			SqlParameter[] parms = new SqlParameter[7]
			{
					new SqlParameter("@identificadores", SqlDbType.Structured),
					new SqlParameter("@dataIni", SqlDbType.DateTime),
					new SqlParameter("@dataFin", SqlDbType.DateTime),
					new SqlParameter("@jornadaMin", SqlDbType.Int),
					new SqlParameter("@valorRefeicao", SqlDbType.Decimal),
					new SqlParameter("@considerarDoisRegistros", SqlDbType.Bit),
					new SqlParameter("@considerarDiasSemjornada", SqlDbType.Bit)
			};
			IEnumerable<long> ids = idsFunc.Select(s => (long)s);
			parms[0].Value = DAL.SQL.DALBase.CreateDataTableIdentificadores(ids);
			parms[0].TypeName = "Identificadores";
			parms[1].Value = pDataInicial;
			parms[2].Value = pDataFinal;
			parms[3].Value = percJornadaMinima;
			parms[4].Value = valorRefeicao;
			parms[5].Value = considerarDoisRegistros;
			parms[6].Value = considerarDiasSemjornada;

			string aux = @" SELECT	D.FuncionarioNome,
		D.FuncionarioCPF,
		D.FuncionarioPis,
		D.CodigoFilial,
		D.FuncionarioMatricula,
		COUNT(*) Quantidade,
		COUNT(*) * @valorRefeicao Valor,
		YEAR(@dataFin) Ano,
		MONTH(@dataFin) Mes
						  FROM (
							SELECT I.*,
								     iif((I.trabalharMin = 0 and @considerarDiasSemjornada = 1) and (
										(@considerarDoisRegistros = 1 and I.QtdRegistroJornada >= 2) OR
										(@considerarDoisRegistros = 0 and I.QtdRegistroJornada >= 4)
										),100, iif(
										((@considerarDoisRegistros = 1 and I.QtdRegistroJornada >= 2) OR
										(@considerarDoisRegistros = 0 and I.QtdRegistroJornada >= 4)
										),(I.trabalhadasMin * 100) / trabalharMin, 0)
									) percTrab
							  FROM (
								SELECT E.*,
										IIF(ISNULL(E.entrada_1,'--:--') = '--:--',0,1) +
										IIF(ISNULL(E.saida_1, '--:--') = '--:--',0,1) +
										IIF(ISNULL(E.entrada_2, '--:--') = '--:--',0,1) +
										IIF(ISNULL(E.saida_2, '--:--') = '--:--',0,1) +
										IIF(ISNULL(E.entrada_3, '--:--') = '--:--',0,1) +
										IIF(ISNULL(E.saida_3, '--:--') = '--:--',0,1) + 
										IIF(ISNULL(E.entrada_4, '--:--') = '--:--',0,1) +
										IIF(ISNULL(E.saida_4, '--:--') = '--:--',0,1) AS QtdRegistroJornada,
										dbo.FN_CONVHORA(dbo.fnTotalHorasTrabalhadas(data, j.entrada_1, j.entrada_2, j.entrada_3, j.entrada_4,'--:--','--:--', '--:--', '--:--',
																		  j.saida_1, j.saida_2, j.saida_3, j.saida_4,'--:--','--:--', '--:--', '--:--')) AS trabalharMin
								  FROM (
										SELECT m.id IdMarcacao,
										f.CPF FuncionarioCPF,
										f.pis FuncionarioPis,
										e.codigo CodigoFilial,
										f.matricula FuncionarioMatricula,
											   m.data,
											   m.totalHorasTrabalhadas,
											   dbo.FN_CONVHORA(m.totalHorasTrabalhadas) trabalhadasMin,
											   f.id IdFuncionario,
											   e.nome EmpresaNome,
											   e.cnpj EmpresaCNPJ,
											   f.codigo FuncionarioCodigo,
											   f.nome FuncionarioNome,
											   ISNULL(ja.idjornada, hd.idjornada) idJornada,
											   m.entrada_1,
											   m.saida_1,
											   m.entrada_2,
											   m.saida_2,
											   m.entrada_3,
											   m.saida_3,
											   m.entrada_4,
											   m.saida_4
										  FROM marcacao_view as m with (nolock)
										 INNER JOIN funcionario as f ON f.id = m.idfuncionario
										 INNER JOIN @identificadores i on i.Identificador = f.id
										 INNER JOIN empresa e on f.idempresa = e.id
										  LEFT JOIN horario ON horario.id = m.idhorario 
										  LEFT JOIN horariodetalhe as hd ON hd.idhorario = m.idhorario AND 
											   ((horario.tipohorario = 2 AND hd.data = m.data) OR 
											   (hd.idhorario = m.idhorario 
											   AND horario.tipohorario = 1
											   AND hd.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, m.data) AS INT)-1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, m.data) AS INT)-1) END)))                        
										  LEFT JOIN jornadaalternativa_view jav ON 
											   ((jav.tipo = 0 AND jav.identificacao = f.idempresa) 
											   OR (jav.tipo = 1 AND jav.identificacao = f.iddepartamento) 
											   OR (jav.tipo = 2 AND jav.identificacao = f.id) 
											   OR (jav.tipo = 3 AND jav.identificacao = f.idfuncao)) 
											   AND ( jav.datacompensada = m.data OR
													(jav.datacompensada IS NULL AND m.data >= jav.datainicial AND m.data <= jav.datafinal))
										  LEFT JOIN jornadaalternativa ja on ja.id = jav.id
										 WHERE m.data BETWEEN @dataIni AND @dataFin
									   ) E
								   LEFT JOIN jornada AS j ON E.idJornada = j.id
								   ) I
						   WHERE ((@considerarDoisRegistros = 0 and I.QtdRegistroJornada >= 4) or
						   (@considerarDoisRegistros = 1 and I.QtdRegistroJornada >= 2) and
						   ((@considerarDiasSemjornada = 1 ) OR (@considerarDiasSemjornada = 0 AND I.idJornada is not NULL))
						   )
							) D
						WHERE D.percTrab >= @jornadaMin
						GROUP BY D.FuncionarioNome,
								 D.FuncionarioCPF,
								 D.FuncionarioPis,
								 D.CodigoFilial,
								 D.FuncionarioMatricula
						ORDER BY FuncionarioNome";
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