using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;

namespace DAL.SQL
{
    public class Inconsistencia : DAL.SQL.DALBase, DAL.IInconsistencia
    {

        public Inconsistencia(DataBase database)
        {
            db = database;
            TABELA = "inconsistencia";

            SELECTPID = @"   SELECT * FROM inconsistencia WHERE id = @id";

            SELECTALL = @"   SELECT * FROM inconsistencia";

            INSERT = @"  INSERT INTO inconsistencia
							(codigo, legenda, descricao, incdata, inchora, incusuario)
							VALUES
							(@codigo, @legenda, @descricao, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE inconsistencia SET
							  codigo = @codigo
                            , legenda = @legenda
							, descricao = @descricao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM inconsistencia WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM inconsistencia";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AuxSetInstance(dr, obj);                  
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Inconsistencia();
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Inconsistencia)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Inconsistencia)obj).Legenda = Convert.ToString(dr["legenda"]); 
            ((Modelo.Inconsistencia)obj).Descricao = Convert.ToString(dr["descricao"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@legenda", SqlDbType.VarChar),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.Inconsistencia)obj).Codigo;
            parms[2].Value = ((Modelo.Inconsistencia)obj).Legenda;
            parms[3].Value = ((Modelo.Inconsistencia)obj).Descricao;
            parms[4].Value = ((Modelo.Inconsistencia)obj).Incdata;
            parms[5].Value = ((Modelo.Inconsistencia)obj).Inchora;
            parms[6].Value = ((Modelo.Inconsistencia)obj).Incusuario;
            parms[7].Value = ((Modelo.Inconsistencia)obj).Altdata;
            parms[8].Value = ((Modelo.Inconsistencia)obj).Althora;
            parms[9].Value = ((Modelo.Inconsistencia)obj).Altusuario;
        }

        public Modelo.Inconsistencia LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Inconsistencia objInconsistencia = new Modelo.Inconsistencia();
            try
            {

                SetInstance(dr, objInconsistencia);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objInconsistencia;
        }

        public Modelo.Inconsistencia LoadObjectByCodigo(int pCodigo)
        {

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@codigo", SqlDbType.Int)
            };
            parms[0].Value = pCodigo;

            string sql = " SELECT * " +
                            " FROM inconsistencia" +
                            " WHERE codigo = @codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.Inconsistencia objInconsistencia = new Modelo.Inconsistencia();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Inconsistencia>();
                objInconsistencia = AutoMapper.Mapper.Map<List<Modelo.Inconsistencia>>(dr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objInconsistencia;
        }

        public Hashtable GetHashIdDescricao()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM inconsistencia", parms);

            Hashtable lista = new Hashtable();
            try
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]), Convert.ToString(dr["descricao"]));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Inconsistencia> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM inconsistencia", parms);

            List<Modelo.Inconsistencia> lista = new List<Modelo.Inconsistencia>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Inconsistencia objInconsistencia = new Modelo.Inconsistencia();
                    AuxSetInstance(dr, objInconsistencia);
                    lista.Add(objInconsistencia);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        #endregion

        /// <summary>
        /// Retorna os dados para Geração de Relatório de Inconsistências
        /// </summary>
        /// <param name="cpfs">Lista com CPFs dos funcionários que serão exibidos no relatório</param>
        /// <param name="datainicial">Data início do relatório</param>
        /// <param name="datafinal">Data fim do relatório</param>
        /// <returns>Retorna lista para geração de relatório</returns>
        public List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias> RelatorioInconsistencias(List<int> idsFuncs, string datainicial, string datafinal, List<bool> paramInconsistencia)
        {
            SqlParameter[] parms = new SqlParameter[]
            {

                new SqlParameter("@idsFuncs", SqlDbType.VarChar),

                new SqlParameter("@datainicial", SqlDbType.DateTime),
                new SqlParameter("@datafinal", SqlDbType.DateTime),

                new SqlParameter("@Intrajornada", SqlDbType.Bit),
                new SqlParameter("@Interjornada", SqlDbType.Bit),
                new SqlParameter("@SetimoDiaConsecutivo", SqlDbType.Bit),
                new SqlParameter("@LimiteHorasTrabDia", SqlDbType.Bit),
                new SqlParameter("@TerceiroDomingoTrab", SqlDbType.Bit),
                new SqlParameter("@TempoSemIntervaloMinino", SqlDbType.Bit),
            };

            if (idsFuncs != null && idsFuncs.Count > 0)
                parms[0].Value = string.Join(",", idsFuncs.ToArray());
            else
                parms[0].Value = DBNull.Value;

            parms[1].Value = datainicial;
            parms[2].Value = datafinal;

            parms[3].Value = paramInconsistencia[0];    // Intrajornada
            parms[4].Value = paramInconsistencia[1];    // Interjornada
            parms[5].Value = paramInconsistencia[2];    // Sétimo dia trabalhado
            parms[6].Value = paramInconsistencia[3];    // Limite de hora do dia
            parms[7].Value = paramInconsistencia[4];    // Terceiro domingo trabalhado
            parms[8].Value = paramInconsistencia[5];    // Seis horas sem intervalo

            //string sql = GerarSqlRelatorioInconsistencias(string.Format("f.id IN ( {0} ) ", String.Join(",", idsFuncs)));
            string sql = GerarSqlRelatorioInconsistencias();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias> lista = new List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias obj = new Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias();
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).CPF = Convert.ToString(dr["Cpf"]);
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).Matricula = Convert.ToString(dr["matricula"]);
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).Data = Convert.ToDateTime(dr["Data"]);
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).Competencia = Convert.ToString(dr["Competencia"]);
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).Dia = Convert.ToString(dr["Dia"]);
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).Batidas = Convert.ToString(dr["Batidas"]);
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).HorasOcorrencia = Convert.ToString(dr["HorasOcorrencia"]);
                    ((Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias)obj).TipoOcorrencia = Convert.ToString(dr["TipoOcorrencia"]);
                    lista.Add(obj);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        //private string GerarSqlRelatorioInconsistencias(string condicional)
        private string GerarSqlRelatorioInconsistencias()
        {

            return @"   DECLARE @T1 TABLE (
                        idfuncionario  INT, 
                        Cpf VARCHAR(100), 
                        Data DATETIME, 
                        Dia VARCHAR(5), 
                        Matricula VARCHAR(50), 
                        Batidas VARCHAR(100), 
                        Competencia VARCHAR(15), 
                        SetimoDiaConsecutivo bit, 
                        DiasTrabalhadosConsec int, 
                        Interjornada bit, 
                        ValorInterjornada VARCHAR(10), 
                        Intrajornada bit, 
                        ValorIntrajornada VARCHAR(10), 
                        TempoSemIntervaloMinimo bit, 
                        ValorTempoSemIntervalo VARCHAR(10),
                        LimiteHorasTrabDia bit, 
                        ValorTotalHorasTrabalhadas  VARCHAR(10),
                        TerceiroDomingoTrab bit, 
                        SeqDomingoTrabalhados int
                        )

                        INSERT @T1
                               SELECT
                               d.idfuncionario,
	                           D.CPF,
                               D.data as Data,
                               D.dia as Dia,
                               D.matricula as Matricula,
							   REPLACE(RTRIM(CONCAT(d.entrada_1,' ',d.saida_1,' ',d.entrada_2,' ',d.saida_2,' ',d.entrada_3,' ',d.saida_3,' ',d.entrada_4,' ',d.saida_4)),' ',' - ') AS Batidas,
                               CASE mesComp
                                        WHEN 1 THEN 'Jan'
                                        WHEN 2 THEN 'Fev'
                                        WHEN 3 THEN 'Mar'
                                        WHEN 4 THEN 'Abr'
                                        WHEN 5 THEN 'Mai'
                                        WHEN 6 THEN 'Jun'
                                        WHEN 7 THEN 'Jul'
                                        WHEN 8 THEN 'Ago'
                                        WHEN 9 THEN 'Set'
                                        WHEN 10 THEN 'Out'
                                        WHEN 11 THEN 'Nov'
                                        WHEN 12 THEN 'Dez'
                                        END + '/'+D.anoComp AS Competencia,
                               IIF(DiasTrabalhados >= 7, 1, 0) SetimoDiaConsecutivo,
                               DiasTrabalhados DiasTrabalhadosConsec,
                               IIF((dbo.FN_CONVHORA(d.Interjornada) > 0 AND  dbo.FN_CONVHORA(d.Interjornada) < dbo.FN_CONVHORA(ISNULL(d.LimiteInterjornada, CONVERT(TIME, '11:00',108))) AND D.Interjornada != '--:--' AND d.Interjornada IS NOT NULL),1,0) Interjornada,
                               ISNULL(d.Interjornada,'--:--') ValorInterjornada,
                               IIF((d.tempoRealizadoAlmocoMin < dbo.FN_CONVHORA(ISNULL(d.LimiteMinimoHorasAlmoco, CONVERT(TIME, '01:00',108))) AND LEN(d.Batidas) > 0),1,0) Intrajornada,
                               dbo.FN_CONVMIN(d.tempoRealizadoAlmocoMin) ValorIntrajornada,
                               IIF ((D.periodo1 > 360 OR D.periodo2 > 360 OR D.periodo3 > 360 OR D.periodo4 > 360 OR D.periodo5 > 360 OR D.periodo6 > 360),1,0) TempoSemIntervaloMinimo,
                               case when D.periodo1 > 360 then dbo.FN_CONVMIN(D.periodo1)
                                    when D.periodo2 > 360 then dbo.FN_CONVMIN(D.periodo2)
                                    when D.periodo3 > 360 then dbo.FN_CONVMIN(D.periodo3)
                                    when D.periodo4 > 360 then dbo.FN_CONVMIN(D.periodo4)
									when D.periodo5 > 360 then dbo.FN_CONVMIN(D.periodo5)
									when D.periodo6 > 360 then dbo.FN_CONVMIN(D.periodo6)
                                    else '--:--'
                                end ValorTempoSemIntervalo,
                               IIF ((dbo.FN_CONVHORA(d.totalHorasTrabalhadas) > dbo.FN_CONVHORA(isnull(d.LimiteHorasTrabalhadasDia, '10:00'))),1,0) LimiteHorasTrabDia,
                               d.totalHorasTrabalhadas ValorTotalHorasTrabalhadas,
                               IIF ((D.SeqDomingoTrabalhados >= 3),1,0) TerceiroDomingoTrab,
                               SeqDomingoTrabalhados
                          FROM (
                            SELECT  t.*,
                                    REPLACE(RTRIM(CONCAT(t.entrada_1,' ',t.saida_1,' ',t.entrada_2,' ',t.saida_2,' ',t.entrada_3,' ',t.saida_3,' ',t.entrada_4,' ',t.saida_4)),' ',' - ') AS Batidas,
                                    SUBSTRING(comp.mesComp,1,CHARINDEX('/',comp.mesComp,0)-1) mesComp,
                                    SUBSTRING(comp.mesComp,CHARINDEX('/',comp.mesComp,0)+1,LEN(comp.mesComp)) anoComp,
                                    IIF(t.trabalhou = 0, 0, RANK() OVER ( PARTITION BY t.Ordenador,
                                                                        t.idfuncionario ORDER BY t.data )) DiasTrabalhados,
									CASE WHEN t.cafeManha = 1 AND t.diaTemCafe = 1 THEN 
											ISNULL( CASE WHEN entrada_3Min < t.saida_2Min THEN
															(1440 - t.saida_2Min ) +  t.entrada_3Min
														 ELSE
															entrada_3Min - t.saida_2Min
													END,0)
                                        ELSE ISNULL( CASE WHEN entrada_2Min < t.saida_1Min THEN
															(1440 - t.saida_1Min ) +  t.entrada_2Min
														 ELSE
															entrada_2Min - t.saida_1Min
													END,0)
                                END tempoRealizadoAlmocoMin,
                                    IIF(T.dia = 'Dom.',[dbo].[FN_SEQUENCIA_DOMINGOS_TRABALHADOS] (t.idfuncionario, t.data),0) SeqDomingoTrabalhados,
								    CASE WHEN entrada_1Min > t.saida_1Min THEN
											(1440 - entrada_1Min) +  t.saida_1Min
										 ELSE
											t.saida_1Min - entrada_1Min
									END periodo1,
									CASE WHEN entrada_2Min > t.saida_2Min THEN
											(1440 - entrada_2Min) +  t.saida_2Min
										 ELSE
											t.saida_2Min - entrada_2Min
									END periodo2,
									CASE WHEN entrada_3Min > t.saida_3Min THEN
											(1440 - entrada_3Min) +  t.saida_3Min
										 ELSE
											t.saida_3Min - entrada_3Min
									END periodo3,
									CASE WHEN entrada_4Min > t.saida_4Min THEN
											(1440 - entrada_4Min) +  t.saida_4Min
										 ELSE
											t.saida_4Min - entrada_4Min
									END periodo4,
									CASE WHEN entrada_5Min > t.saida_5Min THEN
											(1440 - entrada_5Min) +  t.saida_5Min
										 ELSE
											t.saida_5Min - entrada_5Min
									END periodo5,
									CASE WHEN entrada_6Min > t.saida_6Min THEN
											(1440 - entrada_6Min) +  t.saida_6Min
										 ELSE
											t.saida_6Min - entrada_6Min
									END periodo6
                            FROM    (
										SELECT i.*,
											   IIF(i.entrada_1 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_1,1)) entrada_1Min,
											   IIF(i.entrada_2 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_2,1)) entrada_2Min,
											   IIF(i.entrada_3 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_3,1)) entrada_3Min,
											   IIF(i.entrada_4 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_4,1)) entrada_4Min,
											   IIF(i.entrada_5 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_5,1)) entrada_5Min,
											   IIF(i.entrada_6 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_6,1)) entrada_6Min,
											   IIF(i.entrada_7 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_7,1)) entrada_7Min,
											   IIF(i.entrada_8 is null,null,dbo.FN_ConvHoraNulavel(i.entrada_8,1)) entrada_8Min,
											   IIF(i.saida_1 is null,null,dbo.FN_ConvHoraNulavel(i.saida_1,1)) saida_1Min,
											   IIF(i.saida_2 is null,null,dbo.FN_ConvHoraNulavel(i.saida_2,1)) saida_2Min,
											   IIF(i.saida_3 is null,null,dbo.FN_ConvHoraNulavel(i.saida_3,1)) saida_3Min,
											   IIF(i.saida_4 is null,null,dbo.FN_ConvHoraNulavel(i.saida_4,1)) saida_4Min,
											   IIF(i.saida_5 is null,null,dbo.FN_ConvHoraNulavel(i.saida_5,1)) saida_5Min,
											   IIF(i.saida_6 is null,null,dbo.FN_ConvHoraNulavel(i.saida_6,1)) saida_6Min,
											   IIF(i.saida_7 is null,null,dbo.FN_ConvHoraNulavel(i.saida_7,1)) saida_7Min,
											   IIF(i.saida_8 is null,null,dbo.FN_ConvHoraNulavel(i.saida_8,1)) saida_8Min
										  FROM (
												SELECT flgtrabalhou AS trabalhou,
													   IIF(flgtrabalhou = 1 AND DATEDIFF(DAY,LAG(m.data) OVER ( ORDER BY m.idfuncionario, m.data ), m.data) = 1, 
																[dbo].[FN_QTD_DIAS_NAO_TRABALHADOS_ANTERIOR](m.idfuncionario,m.data), 
																ROW_NUMBER() OVER ( ORDER BY m.idfuncionario, m.data )) Ordenador,
														m.idfuncionario,
														m.dscodigo,
														m.data,
														m.dia,
														m.Interjornada,
														m.totalHorasTrabalhadas,
														bi.E1 entrada_1,
														bi.E2 entrada_2,
														bi.E3 entrada_3,
														bi.E4 entrada_4,
														bi.E5 entrada_5,
														bi.E6 entrada_6,
														bi.E7 entrada_7,
														bi.E8 entrada_8,
														bi.S1 saida_1,
														bi.S2 saida_2,
														bi.S3 saida_3,
														bi.S4 saida_4,
														bi.S5 saida_5,
														bi.S6 saida_6,
														bi.S7 saida_7,
														bi.S8 saida_8,
														f.matricula,
														f.CPF,
														h.LimiteInterjornada,
														h.LimiteMinimoHorasAlmoco,
														h.LimiteHorasTrabalhadasDia,
														h.habilitaperiodo01 cafeManha,
														CASE WHEN ( ( m.dia = 'Seg.' AND dias_cafe_1 = 1) OR
																	( m.dia = 'Ter.' AND dias_cafe_2 = 1) OR
																	( m.dia = 'Qua.' AND dias_cafe_3 = 1) OR
																	( m.dia = 'Qui.' AND dias_cafe_4 = 1) OR
																	( m.dia = 'Sex.' AND dias_cafe_5 = 1) OR
																	( m.dia = 'Sáb.' AND dias_cafe_6 = 1) OR
																	( m.dia = 'Dom.' AND dias_cafe_7 = 1)
																  ) THEN 1
															 ELSE 0
														END diaTemCafe,
														 dbo.FN_CONVHORA(m.horastrabalhadas)
																	+ dbo.FN_CONVHORA(m.horastrabalhadasnoturnas)
																	+ dbo.FN_CONVHORA(m.horasextrasdiurna)
																	+ dbo.FN_CONVHORA(m.horasextranoturna)
																	+ dbo.FN_CONVHORA(m.bancohorascre) totalHorasTrabalhadasMin

											  FROM      marcacao_view m
														INNER JOIN funcionario f ON m.idfuncionario = f.id
														INNER JOIN funcao fc ON f.idfuncao = fc.id
														INNER JOIN departamento dp ON f.iddepartamento = dp.id
														INNER JOIN dbo.horario h ON h.id = m.idhorario
														LEFT JOIN ( SELECT *
																	  FROM (
																		SELECT b.IdFuncionario,
																			   b.mar_data,
																			   b.mar_hora,
																			   b.ent_sai+CONVERT(VARCHAR(2),b.posicao) posicao
																		  FROM dbo.bilhetesimp b
																		 WHERE b.IdFuncionario IN (SELECT valor FROM [dbo].[F_ClausulaIn](@idsFuncs))
																		   AND b.mar_Data BETWEEN DATEADD(DAY,-10, @DataInicial) AND @DataFinal
																		   ) bi
																	   PIVOT ( MAX(bi.mar_hora) FOR posicao IN ([E1], [E2], [E3], [E4], [E5], [E6], [E7], [E8], [S1], [S2], [S3], [S4], [S5], [S6], [S7], [S8])) AS pt
																   ) bi ON bi.IdFuncionario = m.idfuncionario AND bi.mar_data = m.data
											  WHERE     f.id IN (SELECT valor FROM [dbo].[F_ClausulaIn](@idsFuncs))
														AND m.data BETWEEN DATEADD(DAY,-10, @DataInicial) AND @DataFinal
											   ) i
									) t
                                CROSS APPLY (SELECT * FROM [dbo].[FN_CompetenciaPeriodoFuncionario](@idsFuncs, @DataInicial, @DataFinal)) comp
                                WHERE t.idfuncionario = comp.IdFuncionario AND CONVERT(DATE, t.data) = CONVERT(DATE, comp.data)
		                          AND t.data BETWEEN @DataInicial AND @DataFinal
                                ) D 

                        select idfuncionario, Cpf, Data, Dia, Matricula, Batidas, Competencia, ValorInterjornada HorasOcorrencia, 'Intervalo interjornada' TipoOcorrencia
                          from @T1 as Inter
                         where Inter.interjornada = 1 AND @Interjornada = 1
                         UNION ALL
                         select idfuncionario, Cpf, Data, Dia, Matricula, Batidas, Competencia, ValorIntrajornada HorasOcorrencia, 'Intervalo intrajornada' TipoOcorrencia
                          from @T1 as Intra
                         where Intra.intrajornada = 1 AND @Intrajornada = 1
                          UNION ALL
                         select idfuncionario, Cpf, Data, Dia, Matricula, Batidas, Competencia, Convert(varchar(3), DiasTrabalhadosConsec) HorasOcorrencia, 'Sétimo dia consecutivo trabalhado' TipoOcorrencia
                          from @T1 as Setimo
                         where Setimo.SetimoDiaConsecutivo = 1 AND @SetimoDiaConsecutivo = 1
                           UNION ALL
                         select idfuncionario, Cpf, Data, Dia, Matricula, Batidas, Competencia, ValorTotalHorasTrabalhadas HorasOcorrencia, 'Limite de horas trabalhadas no dia' TipoOcorrencia
                          from @T1 as LimTrab
                         where LimTrab.LimiteHorasTrabDia = 1 AND @LimiteHorasTrabDia = 1
                            UNION ALL
                         select idfuncionario, Cpf, Data, Dia, Matricula, Batidas, Competencia, Convert(varchar(3), SeqDomingoTrabalhados) HorasOcorrencia, 'Terceiro domingo consecutivo trabalhado' TipoOcorrencia
                          from @T1 as Dom
                         where Dom.TerceiroDomingoTrab = 1 AND @TerceiroDomingoTrab = 1
                            UNION ALL
                         select idfuncionario, Cpf, Data, Dia, Matricula, Batidas, Competencia, ValorTempoSemIntervalo HorasOcorrencia, 'Seis horas sem intervalo' TipoOcorrencia
                          from @T1 as SemInter
                         where SemInter.TempoSemIntervaloMinimo = 1 AND @TempoSemIntervaloMinino = 1";
        }
    }
}
