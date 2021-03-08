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
    public class RelatorioAbsenteismoV2
    {
        private DataBase db;
        public RelatorioAbsenteismoV2(DataBase database)
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
        public DataTable GetRelatorioAbsenteismoV2(List<int> idsFuncionarios, DateTime pDataInicial, DateTime pDataFinal)
        {
            string strIds = String.Join(",", idsFuncionarios);
            SqlParameter[] parms = new SqlParameter[3]
            { 
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar, strIds.Length),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = strIds;
            parms[1].Value = pDataInicial;
            parms[2].Value = pDataFinal;

            string aux = @"SET DATEFIRST 1; -- Indica que a segunda-feria começa o dia da semana, utilizado no horário detalhe

                                SELECT  CONVERT(VARCHAR(10), @datainicial,101) PeriodoInicial,--A
		                                CONVERT(VARCHAR(10), @datafinal,101) PeriodoFinal,--B
		                                NomeFuncionario,--C
		                                Matricula,--D
		                                Departamento,--E
		                                Funcao,--F
		                                Admissao,--G
		                                ISNULL(QtdeHorasPrevistas,'00:00') QtdeHorasPrevistas,--H
										Supervisor,
		                                ISNULL(HorasTrabalhadas,'00:00') HorasTrabalhadas,--I
		                                NdeDiasTrab,--J
		                                '=IFERROR(I**/H**,0)' PercTrabalhado,--K
		                                '=IFERROR(IF(H**> 0, 1-K**,0),0)' PercAbsent, --L
		                                ISNULL(HorasExtras,'00:00') HorasExtras, --M
		                                QtdHorasExtras, --N
		                                '=IFERROR(M**/H**,0)' PercHorasExtras, --O
		                                ISNULL(AbonoLegal,'00:00') AbonoLegal, --P
		                                qtdAbonoLegal, --Q
		                                '=IFERROR(P**/H**,0)' PercAbonoLegal, --R
		                                ISNULL(AbonoNaoLegalOutros,'00:00') AbonoNaoLegalOutros, --S
		                                qtdAbonoNaoLegalOutros, --T
		                                '=IFERROR(S**/H**,0)' PercAbonoNaoLegalOutros, --U
		                                ISNULL(falta,'00:00') falta, --V
		                                qtdfalta, --W
		                                '=IFERROR(V**/H**,0)' Percfalta, -- X
		                                ISNULL(Atrasos,'00:00') Atrasos, --Y
		                                qtdAtrasos, --Z
		                                '=IFERROR(Y**/H**,0)' PercAtrasos, --AA
									    ISNULL(CreBH,'00:00') CreBH, --AB
		                                QtdCreBH, --AC
		                                '=IFERROR(AB**/H**,0)' PercCreBH, --AD
		                                ISNULL(DebBH,'00:00') DebBH, --AE
		                                qtdDebBH, --AF
		                                '=IFERROR(AE**/H**,0)' PercDebBH -- AG
                                    FROM (
		                                SELECT S.NomeFuncionario,
			                                    S.Matricula,
			                                    CONVERT(VARCHAR(200), S.codigoDepartamento) + ' - ' + S.Departamento Departamento,
			                                    CONVERT(VARCHAR(200), S.CodigoFuncao) + ' - ' +S.Funcao Funcao,
			                                    CONVERT(VARCHAR(10), S.Admissao,101) Admissao,
			                                    dbo.FN_CONVMINNULAVEL(SUM(S.horasTrabalhadasPrevistasMin),1) QtdeHorasPrevistas,
												Supervisor,
                                                dbo.FN_CONVMINNULAVEL(SUM(S.trabalhadaMin - TotalAbonoMin + HoraTrabalhada),1) HorasTrabalhadas,
			                                    SUM(S.trabalhou) NdeDiasTrab,
			                                    dbo.FN_CONVMINNULAVEL(SUM(HorasExtrasMin),1) HorasExtras,
			                                    SUM(PossuiHorasExtras) QtdHorasExtras,
			                                    dbo.FN_CONVMINNULAVEL(SUM(S.AbonoLegal),1) AbonoLegal,
			                                    SUM(SIGN(S.AbonoLegal)) qtdAbonoLegal,
			                                    dbo.FN_CONVMINNULAVEL(SUM(S.AbonoNaoLegalOutros),1) AbonoNaoLegalOutros,
			                                    SUM(SIGN(S.AbonoNaoLegalOutros)) qtdAbonoNaoLegalOutros,
			                                    dbo.FN_CONVMINNULAVEL(SUM(S.falta),1) falta,
			                                    SUM(SIGN(S.falta)) qtdfalta,
			                                    dbo.FN_CONVMINNULAVEL(SUM(S.Atrasos),1) Atrasos,
			                                    SUM(SIGN(S.Atrasos)) qtdAtrasos,
												dbo.FN_CONVMINNULAVEL(SUM(S.TotalCreBHMin),1) CreBH,
			                                    SUM(SIGN(S.TotalCreBHMin)) qtdCreBH,
												dbo.FN_CONVMINNULAVEL(SUM(S.TotalDebBHMin),1) DebBH,
			                                    SUM(SIGN(S.TotalDebBHMin)) qtdDebBH
		                                    FROM (
				                                SELECT *,
					                                    CASE WHEN X.TipoAbono = 0 THEN X.TotalAbonoMin
							                                ELSE 0 END AbonoLegal,
					                                    CASE WHEN X.TipoAbono IN (1,2) THEN X.TotalAbonoMin
							                                ELSE 0 END AbonoNaoLegalOutros,
                                                        CASE WHEN X.TipoAbono = 3 THEN X.TotalAbonoMin
							                                ELSE 0 END HoraTrabalhada
				                                    FROM (
					                                SELECT  t.data,
							                                t.NomeFuncionario,
							                                t.Matricula,
							                                t.codigoDepartamento,
							                                t.Departamento,
							                                t.CodigoFuncao,
							                                t.Funcao,
							                                t.Admissao,
															t.Supervisor,
							                                t.horasTrabalhadasPrevistasMin,
							                                t.trabalhadaMin,
							                                SIGN(t.totalTrabalhadaMin) trabalhou,
							                                HorasExtrasMin,
							                                SIGN(HorasExtrasMin) PossuiHorasExtras,
							                                t.TipoAbono,
							                                CASE WHEN t.bAbonoParcial = 1 THEN
									                                t.qtdAbonoParcial
								                                    WHEN t.bAbonado = 1 THEN
									                                IIF(t.trabalhadaMin > t.totalTrabalhadaMin,t.trabalhadaMin - t.totalTrabalhadaMin,t.totalTrabalhadaMin)
								                                ELSE
									                                0
							                                END TotalAbonoMin,
							                                TotalHorasFaltasMin,
							                                CASE WHEN t.totalTrabalhadaMin = 0 THEN t.TotalHorasFaltasMin
								                                    ELSE 0 END falta,
							                                CASE WHEN t.totalTrabalhadaMin > 0 THEN t.TotalHorasFaltasMin
								                                    ELSE 0 END Atrasos,
															TotalCreBHMin,
															TotalDebBHMin,
															FeriadoParcial,
															FeriadoParcialInicio,
															FeriadoParcialFim,
															FeriadoID
					                                    FROM (
							                                SELECT  m.data,
									                                f.nome NomeFuncionario,
									                                f.matricula Matricula,
									                                f.dataadmissao Admissao,
																	super.RazaoSocial Supervisor,
									                                f.id idFuncionario,
									                                f.iddepartamento,
									                                f.idempresa,
									                                D.codigo codigoDepartamento,
									                                D.descricao Departamento,
									                                fo.codigo CodigoFuncao,
									                                fo.descricao Funcao,
																	CASE WHEN ISNULL(abst.absenteismo, 1) = 1  and (feriado.id is null OR feriado.Parcial = 1) THEN
																			IIF(ja.id IS NULL, 
																				IIF(h.marcacargahorariamista = 0, dbo.FN_CONVHORA(hd.totaltrabalhadadiurna) + dbo.FN_CONVHORA(hd.totaltrabalhadanoturna), dbo.FN_CONVHORA(hd.cargahorariamista)),
																				IIF(ja.cargamista = 0, dbo.FN_CONVHORA(ja.totaltrabalhadadiurna) + dbo.FN_CONVHORA(ja.totaltrabalhadanoturna), dbo.FN_CONVHORA(ja.totalmista))
																				) - (dbo.FN_CONVHORA(m.horasPrevistasDentroFeriadoDiurna) + dbo.FN_CONVHORA(m.horasPrevistasDentroFeriadoNoturna)) 
																		ELSE 0 end horasTrabalhadasPrevistasMin,
									                                dbo.FN_CONVHORA(m.horastrabalhadas) + dbo.FN_CONVHORA(m.horastrabalhadasnoturnas) trabalhadaMin,
									                                dbo.FN_CONVHORA(m.totalHorasTrabalhadas) totalTrabalhadaMin,
									                                dbo.FN_CONVHORA(m.horasextrasdiurna) + dbo.FN_CONVHORA(m.horasextranoturna) HorasExtrasMin,
									                                oc.TipoAbono,
									                                af.abonado bAbonado,
									                                af.parcial bAbonoParcial,
									                                dbo.FN_CONVHORA(af.horai) + dbo.FN_CONVHORA(af.horaf) qtdAbonoParcial,
									                                dbo.FN_CONVHORA(m.horasfaltas) + dbo.FN_CONVHORA(m.horasfaltanoturna) TotalHorasFaltasMin,
																	dbo.FN_CONVHORA(m.bancohorascre) TotalCreBHMin,
																	dbo.FN_CONVHORA(m.bancohorasdeb) TotalDebBHMin,
																	feriado.Parcial AS FeriadoParcial,
																	feriado.HoraInicio AS FeriadoParcialInicio,
																	feriado.HoraFim AS FeriadoParcialFim,
																	feriado.id as FeriadoID
							                                    FROM dbo.funcionario AS f 
																LEFT JOIN dbo.pessoa as super ON super.id = f.IdPessoaSupervisor
							                                    INNER JOIN dbo.funcao AS fo ON fo.id = f.idfuncao
							                                    INNER JOIN dbo.departamento AS D ON f.iddepartamento = D.id
							                                    INNER JOIN dbo.marcacao_view m WITH ( NOLOCK ) ON m.idfuncionario = f.id AND m.data BETWEEN @datainicial AND @datafinal and m.data >= f.dataadmissao and (f.datademissao is null or m.data <= f.datademissao)
							                                    INNER JOIN dbo.horario H ON m.idhorario = H.id
							                                    LEFT JOIN dbo.horariodetalhe hd ON hd.idhorario = m.idhorario 
												                                AND ((h.tipohorario = 1 AND hd.dia = DATEPART(WEEKDAY, m.data)) OR
														                                (h.tipohorario = 2 AND hd.data = m.data)
													                                )
							                                    LEFT JOIN dbo.jornadaalternativa ja ON (m.data BETWEEN ja.datainicial AND ja.datafinal OR exists (SELECT dja.idjornadaalternativa FROM dbo.diasjornadaalternativa dja WHERE ja.id = Dja.idjornadaalternativa AND Dja.datacompensada BETWEEN @datainicial and @datafinal))
																										                                AND ( ( ja.tipo = 0
																											                                AND ja.identificacao = f.idempresa
																											                                )
																											                                OR ( ja.tipo = 1
																											                                AND ja.identificacao = f.iddepartamento
																											                                )
																											                                OR ( ja.tipo = 2
																											                                AND ja.identificacao = f.id
																											                                )
																											                                )
							                                    OUTER APPLY ( SELECT TOP(1) * 
											                                    FROM dbo.afastamento af
											                                WHERE af.abonado = 1
											                                    AND m.data BETWEEN af.datai AND isnull(af.dataf, '9999-12-31')
											                                    AND ((af.idfuncionario = f.id) OR
												                                    (
														                                af.iddepartamento = f.iddepartamento
														                                AND af.idempresa = f.idempresa
												                                    ) OR 
												                                    (
													                                    af.idempresa = f.idempresa
												                                    ) OR 
												                                    (af.idcontrato IN (
																		                                SELECT idcontrato
																		                                FROM dbo.contratofuncionario
																		                                WHERE idfuncionario = f.id
																	                                )
												                                    )
											                                ) ORDER BY af.inchora DESC) af
							                                    LEFT JOIN ocorrencia oc ON oc.id = af.idocorrencia  and oc.absenteismo = 1
																OUTER APPLY ( SELECT TOP(1) oc.absenteismo
											                                    FROM dbo.afastamento af
																				LEFT JOIN ocorrencia oc ON oc.id = af.idocorrencia
											                                WHERE m.data BETWEEN af.datai AND isnull(af.dataf , '9999-12-31')
											                                    AND ((af.idfuncionario = f.id) OR
												                                    (
														                                af.iddepartamento = f.iddepartamento
														                                AND af.idempresa = f.idempresa
												                                    ) OR 
												                                    (
													                                    af.idempresa = f.idempresa
												                                    ) OR 
												                                    (af.idcontrato IN (
																		                                SELECT idcontrato
																		                                FROM dbo.contratofuncionario
																		                                WHERE idfuncionario = f.id
																	                                )
												                                    )
											                                ) ORDER BY af.inchora DESC) abst
					                                    --Busca Feriado
                                                     OUTER APPLY (SELECT TOP(1) * FROM feriado where h.desconsiderarferiado = 0
                                                             AND feriado.data = m.data 
                                                             AND ( feriado.tipoferiado = 0 
                                                                 OR ( feriado.tipoferiado = 1 
                                                                     AND feriado.idempresa = f.idempresa 
                                                                     ) 
                                                                 OR ( feriado.tipoferiado = 2 
                                                                     AND feriado.iddepartamento = f.iddepartamento 
                                                                     ) 
                                                                 OR ( feriado.tipoferiado = 3 
                                                                     AND EXISTS ( SELECT * 
                                                                                     FROM   FeriadoFuncionario FFUNC 
                                                                                     WHERE  feriado.id = FFUNC.idFeriado 
                                                                                         AND FFUNC.idFuncionario = f.id ) 
                                                                     ) 
                                                                 )) feriado 

							                                    WHERE f.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncionarios))
						                                    ) t
					                                    ) x
			                                    ) S
		                                    GROUP BY 
			                                    S.NomeFuncionario,
			                                    S.Matricula,
			                                    S.codigoDepartamento,
			                                    S.Departamento,
			                                    S.CodigoFuncao,
			                                    S.Funcao,
			                                    S.Admissao,
												S.Supervisor
	                                    ) F ORDER BY f.Departamento, f.NomeFuncionario";
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