using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class RepLog : DAL.SQL.DALBase, DAL.IRepLog
    {

        public RepLog(DataBase database)
        {
            db = database;
            TABELA = "RepLog";

            SELECTPID = @"   SELECT * FROM RepLog WHERE id = @id";

            SELECTALL = @"   SELECT   RepLog.*
                             FROM RepLog";

            INSERT = @"  INSERT INTO RepLog
							(codigo, incdata, inchora, incusuario, IdRep,Comando,DescricaoExecucao,Executor,Complemento,Status)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdRep,@Comando,@DescricaoExecucao,@Executor,@Complemento,@Status)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE RepLog SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdRep = @IdRep
                           ,Comando = @Comando
                           ,DescricaoExecucao = @DescricaoExecucao
                           ,Executor = @Executor
                           ,Complemento = @Complemento
                           ,Status = @Status

						WHERE id = @id";

            DELETE = @"  DELETE FROM RepLog WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM RepLog";

        }

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
                obj = new Modelo.RepLog();
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
            ((Modelo.RepLog)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.RepLog)obj).IdRep = Convert.ToInt32(dr["IdRep"]);
             ((Modelo.RepLog)obj).Comando = Convert.ToString(dr["Comando"]);
             ((Modelo.RepLog)obj).DescricaoExecucao = Convert.ToString(dr["DescricaoExecucao"]);
             ((Modelo.RepLog)obj).Executor = Convert.ToString(dr["Executor"]);
             ((Modelo.RepLog)obj).Complemento = Convert.ToString(dr["Complemento"]);
             ((Modelo.RepLog)obj).Status = Convert.ToInt32(dr["Status"]);

        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				 new SqlParameter ("@id", SqlDbType.Int)
				,new SqlParameter ("@codigo", SqlDbType.Int)
				,new SqlParameter ("@incdata", SqlDbType.DateTime)
				,new SqlParameter ("@inchora", SqlDbType.DateTime)
				,new SqlParameter ("@incusuario", SqlDbType.VarChar)
				,new SqlParameter ("@altdata", SqlDbType.DateTime)
				,new SqlParameter ("@althora", SqlDbType.DateTime)
				,new SqlParameter ("@altusuario", SqlDbType.VarChar)
                ,new SqlParameter ("@IdRep", SqlDbType.Int)
                ,new SqlParameter ("@Comando", SqlDbType.VarChar)
                ,new SqlParameter ("@DescricaoExecucao", SqlDbType.VarChar)
                ,new SqlParameter ("@Executor", SqlDbType.VarChar)
                ,new SqlParameter ("@Complemento", SqlDbType.VarChar)
                ,new SqlParameter ("@Status", SqlDbType.Int)

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
            parms[1].Value = ((Modelo.RepLog)obj).Codigo;
            parms[2].Value = ((Modelo.RepLog)obj).Incdata;
            parms[3].Value = ((Modelo.RepLog)obj).Inchora;
            parms[4].Value = ((Modelo.RepLog)obj).Incusuario;
            parms[5].Value = ((Modelo.RepLog)obj).Altdata;
            parms[6].Value = ((Modelo.RepLog)obj).Althora;
            parms[7].Value = ((Modelo.RepLog)obj).Altusuario;
           parms[8].Value = ((Modelo.RepLog)obj).IdRep;
           parms[9].Value = ((Modelo.RepLog)obj).Comando;
           parms[10].Value = ((Modelo.RepLog)obj).DescricaoExecucao;
           parms[11].Value = ((Modelo.RepLog)obj).Executor;
           parms[12].Value = ((Modelo.RepLog)obj).Complemento;
           parms[13].Value = ((Modelo.RepLog)obj).Status;

        }

        public Modelo.RepLog LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.RepLog obj = new Modelo.RepLog();
            try
            {

                SetInstance(dr, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }

        public List<Modelo.RepLog> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.RepLog> lista = new List<Modelo.RepLog>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RepLog>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RepLog>>(dr);
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

        public List<Modelo.RepLog> GetAllListByRep(int idRep)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@idRep", SqlDbType.Int)
            };
            parms[0].Value = idRep;

            string sql = SELECTALL + @" Where idRep = @idRep ORDER BY Inchora DESC";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.RepLog> lista = new List<Modelo.RepLog>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RepLog>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RepLog>>(dr);
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

        public void DeletaLogAntigo()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string aux = @" DELETE FROM replog WHERE Incdata <= DATEADD(DAY,-5, GETDATE()) ";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public List<Modelo.Proxy.PxyRepLogAFD> GetRepLogAFD(string lote)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@lote", SqlDbType.VarChar)
            };
            parms[0].Value = lote;

            string sql = @"
                            SELECT CONVERT(VARCHAR(10), a.Inchora, 103) + ' '  + convert(VARCHAR(8), a.Inchora, 14) DataHoraInclusaoAFD, 
	                               a.LinhaAFD, 
	                               a.Observacao Situacao, 
	                               a.relogio Relogio, 
	                               a.datahora DataHoraRegistro,
	                               a.Nsr,
	                               CONVERT(VARCHAR(10), r.IncHora, 103) + ' '  + convert(VARCHAR(8), r.IncHora, 14) DataHoraInclusaoFila,  
	                               CASE	
	                               WHEN r.Situacao IS NULL THEN 
			                            'Registro Descartado'
	                               WHEN r.Situacao = 'I' THEN 
			                            'Aguardando Processamento'
	                               WHEN r.Situacao = 'R' THEN 
			                            'Aguardando Reprocessamento'
	                               WHEN r.Situacao = 'P' THEN 
			                            'Processando'
	                               WHEN r.Situacao = 'C' THEN 
			                            'Processado'
	                               WHEN r.Situacao = 'E' THEN 
			                            'Erro Processamento'
	                               ELSE 'Situação Desconhecida' END SituacaoFila,
	                               b.data + ' '+ b.hora HoraDoRegistroPonto,
	                               b.mar_data DataAlocacaoMarcacao,
	                               f.nome NomeFuncionario,
	                               f.pis Pis,
	                               f.CPF CPF,
	                               f.matricula Matricula,
	                               e.nome Empresa,
	                               e.cnpj CNPJEmpresa
                              FROM dbo.AFD a
                             LEFT JOIN RegistroPonto r ON a.identificador = r.chave AND a.lote = r.lote
                             LEFT JOIN dbo.bilhetesimp b ON r.id = b.IdRegistroPonto
                             LEFT JOIN dbo.funcionario f ON r.IdFuncionario = f.id
                             LEFT JOIN empresa e ON e.id = f.idempresa
                            WHERE a.Lote = @lote
                            ORDER BY a.Inchora
            ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyRepLogAFD> lista = new List<Modelo.Proxy.PxyRepLogAFD>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyRepLogAFD>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyRepLogAFD>>(dr);
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

        public DataTable GetRepLogAFDResumo(string relogio)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@relogio", SqlDbType.VarChar)
            };
            parms[0].Value = relogio;

            string sql = @"
                            SELECT Inchora,
	                               IncUsuario,
                                   r.Lote,
                                   r.Relogio,
                                   r.QtdRegistrosProcessados,
                                   r.PrimeiroNSR,
                                   r.UltimoNSR,
                                   r.QtdAddFila,
                                   r.QtdAddBilhetes,
                                   r.Visualizar 
                              FROM (
	                            SELECT  MAX(a.Inchora) Inchora,
	                                    MAX(a.Incusuario) IncUsuario,
			                            a.Lote,
			                            a.Relogio,
			                            COUNT(a.LinhaAFD) QtdRegistrosProcessados,  
			                            ISNULL(MIN(IIF(a.Nsr = 0,NULL,a.Nsr)),0) PrimeiroNSR,
			                            ISNULL(MAX(a.Nsr),0) UltimoNSR,
			                            COUNT(r.id) QtdAddFila,  
			                            COUNT(b.id) QtdAddBilhetes,
			                            '<a href=""/RepLog/ResultadoImportacao?chave=' + a.Lote + '""  target=""_blank"" class=""btn btn-default""><span class=""glyphicon glyphicon-save-file""></span>  Detalhe</a>' Visualizar
	                            FROM dbo.AFD a
	                            LEFT JOIN RegistroPonto r ON a.identificador = r.chave AND a.lote = r.lote
	                            LEFT JOIN dbo.bilhetesimp b ON r.id = b.IdRegistroPonto
	                            LEFT JOIN dbo.funcionario f ON r.IdFuncionario = f.id
	                            LEFT JOIN empresa e ON e.id = f.idempresa
	                            WHERE a.Relogio = @relogio
	                            GROUP BY a.Lote,
			                             a.relogio
		                             ) r
                              ORDER BY r.Inchora
                        ";

            DataTable dt = new DataTable();
            using (SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms))
            {
                dt.Load(dr);
            }

            return dt;
        }
    }
}
