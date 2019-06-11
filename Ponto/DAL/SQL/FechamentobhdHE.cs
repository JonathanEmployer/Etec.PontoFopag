using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Modelo.Proxy;
using Modelo;

namespace DAL.SQL
{
    public class FechamentobhdHE : DAL.SQL.DALBase, DAL.IFechamentobhdHE
    {
        public string SELECTPFECBH { get; set; }
        public FechamentobhdHE(DataBase database)
        {
            db = database;
            TABELA = "fechamentobhdhe";

            SELECTPID = @"  SELECT fecha_bhhe.*
                            FROM fechamentobhdhe fecha_bhhe
                            INNER JOIN fechamentobh fecha_bh ON fecha_bh.id = fecha_bhhe.idfechamentobh
                            INNER JOIN marcacao ma on ma.id = fecha_bhhe.IdMarcacao
                            WHERE fecha_bhhe.id = @id";

            SELECTALL = @"  SELECT fecha_bhhe.*
                            FROM fechamentobhdhe fecha_bhhe
                            INNER JOIN fechamentobh fecha_bh ON fecha_bh.id = fecha_bhhe.idfechamentobh
                            INNER JOIN marcacao ma on ma.id = fecha_bhhe.IdMarcacao
                            WHERE 1 = 1 ";

            INSERT = @"  INSERT INTO fechamentobhdhe
							(Codigo, IdMarcacao, IdFechamentoBH, QuantHorasPerc1, QuantHorasPerc2, PercQuantHorasPerc1, PercQuantHorasPerc2, incdata, inchora, incusuario)
							VALUES
							(@Codigo, @IdMarcacao, @IdFechamentoBH, @QuantHorasPerc1, @QuantHorasPerc2, @PercQuantHorasPerc1, @PercQuantHorasPerc2, @incdata, @inchora, @incusuario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE fechamentobhdhe 
                         SET Codigo = @Codigo
                            , IdMarcacao = @IdMarcacao
							, IdFechamentoBH = @IdFechamentoBH
							, QuantHorasPerc1 = @QuantHorasPerc1
							, QuantHorasPerc2 = @QuantHorasPerc2
                            , PercQuantHorasPerc1 = @PercQuantHorasPerc1,
                            , PercQuantHorasPerc2 = @PercQuantHorasPerc2,
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM fechamentobhdhe WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM fechamentobhdhe";

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
                obj = new Modelo.FechamentobhdHE();
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
            ((Modelo.FechamentobhdHE)obj).IdFechamentoBH = Convert.ToInt32(dr["IdFechamentoBH"]);
            ((Modelo.FechamentobhdHE)obj).IdMarcacao = Convert.ToInt32(dr["IdMarcacao"]);
            ((Modelo.FechamentobhdHE)obj).QuantHorasPerc1 = Convert.ToString(dr["QuantHorasPerc1"]);
            ((Modelo.FechamentobhdHE)obj).QuantHorasPerc2 = Convert.ToString(dr["QuantHorasPerc2"]);
            ((Modelo.FechamentobhdHE)obj).PercQuantHorasPerc1 = dr["PercQuantHorasPerc1"] is DBNull ? 0 : Convert.ToInt32(dr["PercQuantHorasPerc1"]);
            ((Modelo.FechamentobhdHE)obj).PercQuantHorasPerc2 = dr["PercQuantHorasPerc2"] is DBNull ? 0 : Convert.ToInt32(dr["PercQuantHorasPerc2"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@Id", SqlDbType.Int),
                new SqlParameter ("@Codigo", SqlDbType.Int),
                new SqlParameter ("@IdMarcacao ", SqlDbType.Int),
                new SqlParameter ("@IdFechamentoBH ", SqlDbType.Int),
                new SqlParameter ("@QuantHorasPerc1 ", SqlDbType.VarChar),
                new SqlParameter ("@QuantHorasPerc2 ", SqlDbType.VarChar),
                new SqlParameter ("@PercQuantHorasPerc1 ", SqlDbType.Int),
                new SqlParameter ("@PercQuantHorasPerc2 ", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),//incluido
				new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),//alterado
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

            parms[1].Value = ((Modelo.FechamentobhdHE)obj).Codigo;
            parms[2].Value = ((Modelo.FechamentobhdHE)obj).IdMarcacao;
            parms[3].Value = ((Modelo.FechamentobhdHE)obj).IdFechamentoBH;
            parms[4].Value = ((Modelo.FechamentobhdHE)obj).QuantHorasPerc1;
            parms[5].Value = ((Modelo.FechamentobhdHE)obj).QuantHorasPerc2;
            parms[6].Value = ((Modelo.FechamentobhdHE)obj).PercQuantHorasPerc1;
            parms[7].Value = ((Modelo.FechamentobhdHE)obj).PercQuantHorasPerc2;
            parms[8].Value = ((Modelo.FechamentobhdHE)obj).Incdata;
            parms[9].Value = ((Modelo.FechamentobhdHE)obj).Inchora;
            parms[10].Value = ((Modelo.FechamentobhdHE)obj).Incusuario;
            parms[11].Value = ((Modelo.FechamentobhdHE)obj).Altdata;
            parms[12].Value = ((Modelo.FechamentobhdHE)obj).Althora;
            parms[13].Value = ((Modelo.FechamentobhdHE)obj).Altusuario;
        }

        public Modelo.FechamentobhdHE LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FechamentobhdHE objFechamentoBHDHE = new Modelo.FechamentobhdHE();
            try
            {
                SetInstance(dr, objFechamentoBHDHE);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFechamentoBHDHE;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        public void Incluir(List<Modelo.FechamentobhdHE> listaObjeto)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (Modelo.FechamentobhdHE obj in listaObjeto)
                        {
                            IncluirAux(trans, obj);
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        public List<Modelo.Proxy.pxyPessoaMarcacaoParaRateio> GetPessoaMarcacaoParaRateio(int pTipo, string pIdTipo, DateTime dataInicial, DateTime dataFinal)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@dataInicial", SqlDbType.DateTime),
                new SqlParameter("@dataFinal", SqlDbType.DateTime),
                new SqlParameter("@identificacao", SqlDbType.VarChar)
            };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;


            string comando = @" SELECT CONVERT(VARCHAR(5), @datainicial,103) + ' a ' + CONVERT(VARCHAR(5),@datafinal,103) as Periodo,
                                PessoaMarcacao.IdentificadorMarcacao, PessoaMarcacao.IdentificadorFuncionario, PessoaMarcacao.Horario,
                                PessoaMarcacao.MatriculaFuncionario, PessoaMarcacao.DSCodigoFuncionario, PessoaMarcacao.NomeFuncionario, PessoaMarcacao.Supervisor,
                                PessoaMarcacao.Alocacao, PessoaMarcacao.Departamento, PessoaMarcacao.Funcao, PessoaMarcacao.Jornada, PessoaMarcacao.DataBatida,
                                PessoaMarcacao.Ent1, PessoaMarcacao.Sai1, PessoaMarcacao.Ent2, PessoaMarcacao.Sai2, PessoaMarcacao.Ent3, PessoaMarcacao.Sai3, 
	                        	PessoaMarcacao.Ent4, PessoaMarcacao.Sai4, PessoaMarcacao.CredBH, PessoaMarcacao.DebBH,  
                                (PessoaMarcacao.CreditoBancoHorasDia - PessoaMarcacao.DebitoBancoHorasDia) SaldoBancoHorasDia, 
                                ([dbo].CONVERTHORAMINUTO(PessoaMarcacao.SaldoBHDia)) SaldoBancoHorasTotalDia,
                                ([dbo].CONVERTHORAMINUTO(PessoaMarcacao.SaldoBHPeriodo)) SaldoBancoHorasTotal,
                                PessoaMarcacao.Ocorrencia
	                        	
                             FROM ( ";
            comando +=
                  @" SELECT marcacao.ID IdentificadorMarcacao, 
	                 [dbo].[FN_CONVMIN]((Select * from [dbo].[F_SaldoBHFuncionario](marcacao.Data, funcionario.id))) as 'SaldoBHDia',
                     [dbo].[FN_CONVMIN]((Select * from [dbo].[F_SaldoBHFuncionario](@datafinal, funcionario.id))) as 'SaldoBHPeriodo',
                     funcionario.id IdentificadorFuncionario, funcionario.dscodigo DSCodigoFuncionario, funcionario.nome AS NomeFuncionario, 
                     marcacao.data DataBatida, [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), marcacao.bancohorascre), '--:--')) CreditoBancoHorasDia,
                     [dbo].CONVERTHORAMINUTO(ISNULL(CONVERT(varchar(6), marcacao.bancohorasdeb), '--:--')) DebitoBancoHorasDia, 

                     funcionario.matricula as MatriculaFuncionario,
		             ISNULL(CONVERT(varchar(20), a.codigo), '')as Alocacao,
                     h.Descricao as Horario, 
                     d.codigo as Departamento,
	                 fun.descricao as Funcao,
		             ISNULL(j.JornadaCompleta,'') as Jornada,  
		             CASE
				         WHEN marcacao.entrada_1  =	'--:--'	THEN '' 
				         ELSE marcacao.entrada_1
		             END Ent1,
			         CASE
			            WHEN marcacao.saida_1  =	'--:--'	THEN '' 
				        ELSE marcacao.saida_1
		             END Sai1,
		             CASE
			            WHEN marcacao.entrada_2  =	'--:--'	THEN '' 
				        ELSE marcacao.entrada_2
		             END Ent2,
	                 CASE
			            WHEN marcacao.saida_2  =	'--:--'	THEN '' 
				        ELSE marcacao.saida_2
		             END Sai2,
		             CASE
			            WHEN marcacao.entrada_3  = '--:--' THEN '' 
				        ELSE marcacao.entrada_3
		             END Ent3,
	                 CASE
			            WHEN marcacao.saida_3  = '--:--'	THEN '' 
				        ELSE marcacao.saida_3
		             END Sai3,
	                 CASE
		                WHEN marcacao.entrada_4  = '--:--' THEN '' 
			            ELSE marcacao.entrada_4
		             END Ent4,
	                 CASE
		                WHEN marcacao.saida_4  =	'--:--'	THEN '' 
			            ELSE marcacao.saida_4
		             END Sai4,
		             CASE
		                WHEN marcacao.bancohorascre != '---:--' AND marcacao.bancohorascre != '00:00' THEN marcacao.bancohorascre
			            ELSE ''
		             END as 'CredBH',
		             CASE
			            WHEN marcacao.bancohorasdeb !=	'---:--' AND marcacao.bancohorasdeb != '00:00' THEN marcacao.bancohorasdeb
			            ELSE ''
		             END as 'DebBH',
						                            
	                 ISNULL(p.RazaoSocial, '') as Supervisor,
	    
		             CASE
		                WHEN marcacao.bancohorascre > marcacao.bancohorasdeb THEN 'CreditoBH'
			            WHEN marcacao.bancohorasdeb > marcacao.bancohorascre THEN 'DebitoBH'
			            ELSE marcacao.ocorrencia
		             END as Ocorrencia

                     FROM marcacao_view marcacao
                     JOIN Funcionario funcionario on marcacao.idfuncionario = funcionario.id
                     LEFT JOIN Alocacao a on funcionario.idAlocacao = a.id
                     JOIN Departamento d on funcionario.iddepartamento = d.id
                     JOIN Funcao fun on funcionario.idfuncao = fun.id
                     JOIN Horario h on marcacao.idhorario = h.id
                     JOIN HorarioDetalhe hd on h.id = hd.idhorario
                     LEFT JOIN Jornada_View j on hd.idjornada = j.id
                     LEFT JOIN Pessoa p on funcionario.IdPessoaSupervisor = p.id
                     WHERE (marcacao.data between @dataInicial and @dataFinal)
                     AND (ISNULL(funcionario.excluido,0) = 0 AND ISNULL(funcionarioativo,0) = 1) 
                     AND (ISNULL(marcacao.idfechamentobh,0) = 0 AND ISNULL(marcacao.naoentrarbanco,0) = 0) 
		             AND
                     CASE 
					 WHEN marcacao.dia = 'Seg.' THEN 1
					 WHEN marcacao.dia = 'Ter.' THEN 2
					 WHEN marcacao.dia = 'Qua.' THEN 3
					 WHEN marcacao.dia = 'Qui.' THEN 4
					 WHEN marcacao.dia = 'Sex.' THEN 5
					 WHEN marcacao.dia = 'Sáb.' THEN 6
					 WHEN marcacao.dia = 'Dom.' THEN 7		
					 END = hd.dia ";

            if (pTipo >= 0 && pTipo != 5)
            {
                parms[2].Value = pIdTipo;
                switch (pTipo)
                {
                    case 0://Empresa
                        comando += " AND funcionario.idempresa IN (select * from [dbo].[F_RetornaTabelaLista] (@identificacao,',')) ";
                        break;
                    case 1://Departamento
                        comando += " AND funcionario.iddepartamento IN (select * from [dbo].[F_RetornaTabelaLista] (@identificacao,',')) ";
                        break;
                    case 2://Funcionário
                        comando += " AND marcacao.idfuncionario IN (select * from [dbo].[F_RetornaTabelaLista] (@identificacao,',')) ";
                        break;
                    case 3://Função
                        comando += " AND funcionario.idfuncao IN (select * from [dbo].[F_RetornaTabelaLista] (@identificacao,',')) ";
                        break;
                    case 4://Horário
                        comando += " AND marcacao.idhorario IN (select * from [dbo].[F_RetornaTabelaLista] (@identificacao,',')) ";
                        break;
                    default:
                        break;
                }
            }

            comando += @" ) AS PessoaMarcacao ";
            comando += " order by PessoaMarcacao.IdentificadorFuncionario, PessoaMarcacao.DataBatida";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);

            List<Modelo.Proxy.pxyPessoaMarcacaoParaRateio> lista = new List<Modelo.Proxy.pxyPessoaMarcacaoParaRateio>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, List<Modelo.Proxy.pxyPessoaMarcacaoParaRateio>>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyPessoaMarcacaoParaRateio>>(dr);
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

        public IList<Modelo.FechamentobhdHE> GetFechamentobhdHEPorIdFechamentoBH(int idFechamentoBH, int identificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfechamentobh", SqlDbType.Int, 4),
                new SqlParameter("@idfuncionario", SqlDbType.Int, 4)
            };
            parms[0].Value = idFechamentoBH;
            parms[1].Value = identificacao;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text,
                @"  SELECT fecha_bhhe.*
                    FROM fechamentobhdhe fecha_bhhe
                    INNER JOIN fechamentobh fecha_bh ON fecha_bh.id = fecha_bhhe.idfechamentobh
                    INNER JOIN marcacao ma on ma.id = fecha_bhhe.IdMarcacao
                    INNER JOIN funcionario f on f.id = ma.idFuncionario
                    WHERE fecha_bh.ID = @idfechamentobh
                    AND f.ID = @idfuncionario", parms);

            IList<Modelo.FechamentobhdHE> listaObjFechamentobhdHE = new List<Modelo.FechamentobhdHE>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentobhdHE objFechamentoBHD = new Modelo.FechamentobhdHE();
                    AuxSetInstance(dr, objFechamentoBHD);
                    listaObjFechamentobhdHE.Add(objFechamentoBHD);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            return listaObjFechamentobhdHE;
        }

        public IList<Modelo.FechamentobhdHE> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            IList<Modelo.FechamentobhdHE> listaObjFechamentobhdHE = new List<Modelo.FechamentobhdHE>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentobhdHE objFechamentoBHD = new Modelo.FechamentobhdHE();
                    AuxSetInstance(dr, objFechamentoBHD);
                    listaObjFechamentobhdHE.Add(objFechamentoBHD);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            return listaObjFechamentobhdHE;
        }

        public DataTable GetRelatorioFechamentoPercentualHESintetico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = idSelecionados;
            parms[1].Value = inicioPeriodo;
            parms[2].Value = fimPeriodo;

            string aux = @"       SELECT 						
								  CONVERT(VARCHAR(5), @datainicial,103) + ' a ' + CONVERT(VARCHAR(5),@datafinal,103) as Período,
								  f.nome as Nome,
	                              f.matricula as Matrícula,
								  ISNULL(a.codigo, '') as Alocação,
	                              d.codigo as Departamento,
	                              fun.descricao as Função,
	                              h.Descricao as Horário, 

								  ISNULL([dbo].[FN_CONVMIN](SUM(CASE
			                            WHEN m.bancohorascre != '---:--' AND m.bancohorascre != '00:00' THEN [dbo].[FN_CONVHORA](m.bancohorascre)

		                          END)), '') as 'Créd. BH',
								  ISNULL([dbo].[FN_CONVMIN](SUM(CASE
										WHEN m.bancohorasdeb !=	'---:--' AND m.bancohorasdeb != '00:00' THEN [dbo].[FN_CONVHORA](m.bancohorasdeb)
		                          END)), '') as 'Déb. BH',

								  REPLACE((SELECT [dbo].[FN_CONVMIN]((SELECT * FROM [dbo].[F_SaldoBHFuncionario](@datafinal, m.idfuncionario)))), '--:--', '') as 'Saldo BH',
								  
								  ISNULL([dbo].[FN_CONVMIN](SUM(CASE
			                            WHEN fhe.QuantHorasPerc1 != '--:--' THEN [dbo].[FN_CONVHORA](fhe.QuantHorasPerc1)
		                          END)), '') as 'Perc. 1',

								  ISNULL([dbo].[FN_CONVMIN](SUM(CASE
			                            WHEN fhe.QuantHorasPerc2 != '--:--' THEN [dbo].[FN_CONVHORA](fhe.QuantHorasPerc2)
		                          END)), '') as 'Perc. 2',			  
	                              fe.codigo as 'Código Fechamento',
                                  fe.data as 'Data Fechamento'
		
								  from fechamentobh fe

						JOIN marcacao_view m on m.idfechamentobh = fe.id
						LEFT JOIN fechamentobhdhe fhe on m.id = fhe.idmarcacao
						JOIN Funcionario f on m.idfuncionario = f.id
						LEFT JOIN Alocacao a on f.idAlocacao = a.id
						JOIN Departamento d on f.iddepartamento = d.id
						JOIN Funcao fun on f.idfuncao = fun.id
						JOIN Horario h on m.idhorario = h.id
						JOIN HorarioDetalhe hd on h.id = hd.idhorario 
						LEFT JOIN Pessoa p on f.IdPessoaSupervisor = p.id

                        where f.id IN (select * from [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
                        AND m.data between @datainicial and @datafinal
                        AND CASE 
								WHEN m.dia = 'Seg.' THEN 1
								WHEN m.dia = 'Ter.' THEN 2
								WHEN m.dia = 'Qua.' THEN 3
								WHEN m.dia = 'Qui.' THEN 4
								WHEN m.dia = 'Sex.' THEN 5
								WHEN m.dia = 'Sáb.' THEN 6
								WHEN m.dia = 'Dom.' THEN 7		
							END = hd.dia

                        GROUP BY f.nome, f.matricula, a.codigo, d.codigo, fun.descricao, 
						         m.idfuncionario, fe.codigo, h.Descricao, fe.data
                        ORDER BY  f.matricula ";

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;

        }

        public DataTable GetRelatorioFechamentoPercentualHEAnalitico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar),
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = idSelecionados;
            parms[1].Value = inicioPeriodo;
            parms[2].Value = fimPeriodo;

            string aux = @"SELECT 
	 	                        f.matricula as Matrícula,
		                        ISNULL(CONVERT(varchar(20), a.codigo), '') as Alocação,
	                            d.codigo as Departamento,
	                            fun.descricao as Função,
		                        j.JornadaCompleta as Jornada,  
                                m.Data as DataBatida,
		                        CASE
				                        WHEN m.entrada_1  =	'--:--'	THEN '' 
				                        ELSE m.entrada_1
		                        END as 'Ent. 1',
			                        CASE
			                            WHEN m.saida_1  =	'--:--'	THEN '' 
				                        ELSE m.saida_1
		                            END as 'Sai. 1',
		                        CASE
			                            WHEN m.entrada_2  =	'--:--'	THEN '' 
				                        ELSE m.entrada_2
		                            END as 'Ent. 2',
	                            CASE
			                            WHEN m.saida_2  =	'--:--'	THEN '' 
				                        ELSE m.saida_2
		                        END as 'Sai. 2',
		                        CASE
			                            WHEN m.entrada_3  = '--:--' THEN '' 
				                        ELSE m.entrada_3
		                        END as 'Ent. 3',
	                            CASE
			                            WHEN m.saida_3  = '--:--'	THEN '' 
				                        ELSE m.saida_3
		                        END as 'Sai. 3',
	                            CASE
		                            WHEN m.entrada_4  = '--:--' THEN '' 
			                        ELSE m.entrada_4
		                        END as 'Ent. 4',
	                            CASE
		                            WHEN m.saida_4  =	'--:--'	THEN '' 
			                        ELSE m.saida_4
		                        END as 'Sai. 4',
		                        CASE
		                            WHEN m.bancohorascre != '---:--' AND m.bancohorascre != '00:00' THEN m.bancohorascre
			                        ELSE ''
		                        END as 'Créd. BH',
		                        CASE
			                        WHEN m.bancohorasdeb !=	'---:--' AND m.bancohorasdeb != '00:00' THEN m.bancohorasdeb
			                        ELSE ''
		                        END as 'Déb. BH',

	                            REPLACE((SELECT [dbo].[FN_CONVMIN]((Select * from [dbo].[F_SaldoBHFuncionario](m.data, m.idfuncionario)))), '--:--', '00:00') as 'Saldo BH',
		                        CASE
		                             WHEN fhe.QuantHorasPerc1 != '--:--' AND fhe.QuantHorasPerc1 IS NOT NULL THEN fhe.QuantHorasPerc1
			                         ELSE ''
		                        END as 'Perc. 1',
		                        CASE
		                             WHEN fhe.QuantHorasPerc2 != '--:--' AND fhe.QuantHorasPerc2 IS NOT NULL THEN fhe.QuantHorasPerc2
			                         ELSE ''
		                        END as 'Perc. 2',
	                            ISNULL(p.RazaoSocial, '') as Supervisor,
	    
		                        CASE
		                             WHEN m.bancohorascre > m.bancohorasdeb THEN 'Crédito BH'
			                         WHEN m.bancohorasdeb > m.bancohorascre THEN 'Débito BH'
			                         ELSE m.ocorrencia
		                        END as Ocorrência,

	                            fe.codigo as 'Código Fechamento'
		
	                            from fechamentobh fe

						JOIN marcacao_view m on m.idfechamentobh = fe.id
                        LEFT JOIN fechamentobhdhe fhe on m.id = fhe.idmarcacao
                        JOIN Funcionario f on m.idfuncionario = f.id
                        LEFT JOIN Alocacao a on f.idAlocacao = a.id
                        JOIN Departamento d on f.iddepartamento = d.id
                        JOIN Funcao fun on f.idfuncao = fun.id
                        JOIN Horario h on m.idhorario = h.id
                        JOIN HorarioDetalhe hd on h.id = hd.idhorario
                        LEFT JOIN Jornada_View j on hd.idjornada = j.id
                        LEFT JOIN Pessoa p on f.IdPessoaSupervisor = p.id

                        WHERE f.id IN (select * from [dbo].[F_RetornaTabelaLista] (@idsFuncionarios,','))
                        AND m.data between @datainicial and @datafinal
                        AND 
                        CASE 
							WHEN m.dia = 'Seg.' THEN 1
							WHEN m.dia = 'Ter.' THEN 2
							WHEN m.dia = 'Qua.' THEN 3
							WHEN m.dia = 'Qui.' THEN 4
							WHEN m.dia = 'Sex.' THEN 5
							WHEN m.dia = 'Sáb.' THEN 6
							WHEN m.dia = 'Dom.' THEN 7		
						END = hd.dia
                        ORDER BY  f.matricula";

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        #endregion
    }
}
