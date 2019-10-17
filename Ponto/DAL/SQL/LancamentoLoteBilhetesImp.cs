using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class LancamentoLoteBilhetesImp : DAL.SQL.DALBase, DAL.ILancamentoLoteBilhetesImp
    {
        #region Sql utizado para alterar os valores da marcação de acordo com o bilhetes (Alterar/Excluir), de acordo com idsLancamentoFuncionarios.
        string UPDATEBILHETEMARCACAO = @" UPDATE marcacao
	                                        SET	  campo01 = case	when @acao = 3 and biE1.mar_hora is not null then null 
								                                    when biE1.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE1.mar_hora ))
								                                    else campo01 end
			                                    , campo02 = case	when @acao = 3 and biE2.mar_hora is not null then null 
								                                    when biE2.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE2.mar_hora ))
								                                    else campo02 end
			                                    , campo03 = case	when @acao = 3 and biE3.mar_hora is not null then null 
								                                    when biE3.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE3.mar_hora ))
								                                    else campo03 end
			                                    , campo04 = case	when @acao = 3 and biE4.mar_hora is not null then null 
								                                    when biE4.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE4.mar_hora ))
								                                    else campo04 end
			                                    , campo05 = case	when @acao = 3 and biE5.mar_hora is not null then null 
								                                    when biE5.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE5.mar_hora ))
								                                    else campo05 end
			                                    , campo06 = case	when @acao = 3 and biE6.mar_hora is not null then null 
								                                    when biE6.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE6.mar_hora ))
								                                    else campo06 end
			                                    , campo07 = case	when @acao = 3 and biE7.mar_hora is not null then null 
								                                    when biE7.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE7.mar_hora ))
								                                    else campo07 end
			                                    , campo08 = case	when @acao = 3 and biE8.mar_hora is not null then null 
								                                    when biE8.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biE8.mar_hora ))
								                                    else campo08 end

			                                    , campo09 = case	when @acao = 3 and biS1.mar_hora is not null then null 
								                                    when biS1.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS1.mar_hora ))
								                                    else campo09 end
			                                    , campo10 = case	when @acao = 3 and biS2.mar_hora is not null then null 
								                                    when biS2.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS2.mar_hora ))
								                                    else campo10 end
			                                    , campo11 = case	when @acao = 3 and biS3.mar_hora is not null then null 
								                                    when biS3.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS3.mar_hora ))
								                                    else campo11 end
			                                    , campo12 = case	when @acao = 3 and biS4.mar_hora is not null then null 
								                                    when biS4.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS4.mar_hora ))
								                                    else campo12 end
			                                    , campo13 = case	when @acao = 3 and biS5.mar_hora is not null then null 
								                                    when biS5.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS5.mar_hora ))
								                                    else campo13 end
			                                    , campo14 = case	when @acao = 3 and biS6.mar_hora is not null then null 
								                                    when biS6.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS6.mar_hora ))
								                                    else campo14 end
			                                    , campo15 = case	when @acao = 3 and biS7.mar_hora is not null then null 
								                                    when biS7.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS7.mar_hora ))
								                                    else campo15 end
			                                    , campo16 = case	when @acao = 3 and biS8.mar_hora is not null then null 
								                                    when biS8.mar_hora is not null then encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  biS8.mar_hora ))
								                                    else campo16 end
			                                    , ent_num_relogio_1 = case	when @acao = 3 and biE1.mar_hora is not null then '' 
										                                    when biE1.mar_hora is not null then biE1.mar_relogio
										                                    else ent_num_relogio_1 end
			                                    , ent_num_relogio_2 = case	when @acao = 3 and biE2.mar_hora is not null then '' 
										                                    when biE2.mar_hora is not null then biE2.mar_relogio
										                                    else ent_num_relogio_2 end
			                                    , ent_num_relogio_3 = case	when @acao = 3 and biE3.mar_hora is not null then '' 
										                                    when biE3.mar_hora is not null then biE3.mar_relogio
										                                    else ent_num_relogio_3 end
			                                    , ent_num_relogio_4 = case	when @acao = 3 and biE4.mar_hora is not null then '' 
										                                    when biE4.mar_hora is not null then biE4.mar_relogio
										                                    else ent_num_relogio_4 end
			                                    , ent_num_relogio_5 = case	when @acao = 3 and biE5.mar_hora is not null then '' 
										                                    when biE5.mar_hora is not null then biE5.mar_relogio
										                                    else ent_num_relogio_5 end
			                                    , ent_num_relogio_6 = case	when @acao = 3 and biE6.mar_hora is not null then '' 
										                                    when biE6.mar_hora is not null then biE6.mar_relogio
										                                    else ent_num_relogio_6 end
			                                    , ent_num_relogio_7 = case	when @acao = 3 and biE7.mar_hora is not null then '' 
										                                    when biE7.mar_hora is not null then biE7.mar_relogio
										                                    else ent_num_relogio_7 end
			                                    , ent_num_relogio_8 = case	when @acao = 3 and biE8.mar_hora is not null then '' 
										                                    when biE8.mar_hora is not null then biE8.mar_relogio
										                                    else ent_num_relogio_8 end

			                                    , sai_num_relogio_1 = case	when @acao = 3 and biS1.mar_hora is not null then '' 
										                                    when biS1.mar_hora is not null then biS1.mar_relogio
										                                    else sai_num_relogio_1 end
			                                    , sai_num_relogio_2 = case	when @acao = 3 and biS2.mar_hora is not null then '' 
										                                    when biS2.mar_hora is not null then biS2.mar_relogio
										                                    else sai_num_relogio_2 end
			                                    , sai_num_relogio_3 = case	when @acao = 3 and biS3.mar_hora is not null then '' 
										                                    when biS3.mar_hora is not null then biS3.mar_relogio
										                                    else sai_num_relogio_3 end
			                                    , sai_num_relogio_4 = case	when @acao = 3 and biS4.mar_hora is not null then '' 
										                                    when biS4.mar_hora is not null then biS4.mar_relogio
										                                    else sai_num_relogio_4 end
			                                    , sai_num_relogio_5 = case	when @acao = 3 and biS5.mar_hora is not null then '' 
										                                    when biS5.mar_hora is not null then biS5.mar_relogio
										                                    else sai_num_relogio_5 end
			                                    , sai_num_relogio_6 = case	when @acao = 3 and biS6.mar_hora is not null then '' 
										                                    when biS6.mar_hora is not null then biS6.mar_relogio
										                                    else sai_num_relogio_6 end
			                                    , sai_num_relogio_7 = case	when @acao = 3 and biS7.mar_hora is not null then '' 
										                                    when biS7.mar_hora is not null then biS7.mar_relogio
										                                    else sai_num_relogio_7 end
			                                    , sai_num_relogio_8 = case	when @acao = 3 and biS8.mar_hora is not null then '' 
										                                    when biS8.mar_hora is not null then biS8.mar_relogio
										                                    else sai_num_relogio_8 end
                                        from marcacao
                                        left join bilhetesimp as biE1 on biE1.mar_data = marcacao.data and marcacao.dscodigo = biE1.dscodigo and biE1.ent_sai = 'E' and biE1.posicao = 1 and biE1.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biE2 on biE2.mar_data = marcacao.data and marcacao.dscodigo = biE2.dscodigo and biE2.ent_sai = 'E' and biE2.posicao = 2 and biE2.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biE3 on biE3.mar_data = marcacao.data and marcacao.dscodigo = biE3.dscodigo and biE3.ent_sai = 'E' and biE3.posicao = 3 and biE3.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biE4 on biE4.mar_data = marcacao.data and marcacao.dscodigo = biE4.dscodigo and biE4.ent_sai = 'E' and biE4.posicao = 4 and biE4.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biE5 on biE5.mar_data = marcacao.data and marcacao.dscodigo = biE5.dscodigo and biE5.ent_sai = 'E' and biE5.posicao = 5 and biE5.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biE6 on biE6.mar_data = marcacao.data and marcacao.dscodigo = biE6.dscodigo and biE6.ent_sai = 'E' and biE6.posicao = 6 and biE6.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biE7 on biE7.mar_data = marcacao.data and marcacao.dscodigo = biE7.dscodigo and biE7.ent_sai = 'E' and biE7.posicao = 7 and biE7.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biE8 on biE8.mar_data = marcacao.data and marcacao.dscodigo = biE8.dscodigo and biE8.ent_sai = 'E' and biE8.posicao = 8 and biE8.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS1 on biS1.mar_data = marcacao.data and marcacao.dscodigo = biS1.dscodigo and biS1.ent_sai = 'S' and biS1.posicao = 1 and biS1.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS2 on biS2.mar_data = marcacao.data and marcacao.dscodigo = biS2.dscodigo and biS2.ent_sai = 'S' and biS2.posicao = 2 and biS2.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS3 on biS3.mar_data = marcacao.data and marcacao.dscodigo = biS3.dscodigo and biS3.ent_sai = 'S' and biS3.posicao = 3 and biS3.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS4 on biS4.mar_data = marcacao.data and marcacao.dscodigo = biS4.dscodigo and biS4.ent_sai = 'S' and biS4.posicao = 4 and biS4.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS5 on biS5.mar_data = marcacao.data and marcacao.dscodigo = biS5.dscodigo and biS5.ent_sai = 'S' and biS5.posicao = 5 and biS5.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS6 on biS6.mar_data = marcacao.data and marcacao.dscodigo = biS6.dscodigo and biS6.ent_sai = 'S' and biS6.posicao = 6 and biS6.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS7 on biS7.mar_data = marcacao.data and marcacao.dscodigo = biS7.dscodigo and biS7.ent_sai = 'S' and biS7.posicao = 7 and biS7.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        left join bilhetesimp as biS8 on biS8.mar_data = marcacao.data and marcacao.dscodigo = biS8.dscodigo and biS8.ent_sai = 'S' and biS8.posicao = 8 and biS8.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))
                                        where marcacao.id in (
	                                    select mw.id
	                                        from marcacao_view mw
	                                        inner join bilhetesimp bi on bi.mar_data = mw.data and mw.dscodigo = bi.dscodigo
	                                        where bi.idlancamentolotefuncionario in (select * from F_clausulain(@idsLancamentoLoteFuncionario))) ";
        #endregion
        public LancamentoLoteBilhetesImp(DataBase database)
        {
            db = database;
            TABELA = "LancamentoLoteBilhetesImp";

            SELECTPID = @"   SELECT * FROM LancamentoLoteBilhetesImp WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM LancamentoLoteBilhetesImp";

            INSERT = @"  INSERT INTO LancamentoLoteBilhetesImp
							(codigo, incdata, inchora, incusuario, idLancamentoLote, hora, ocorrencia, motivo, idjustificativa, relogio)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idLancamentoLote, @hora, @ocorrencia, @motivo, @idjustificativa, @relogio)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LancamentoLoteBilhetesImp SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idLancamentoLote = @idLancamentoLote,
                                hora = @hora, 
                                ocorrencia = @ocorrencia, 
                                motivo = @motivo, 
                                idjustificativa = @idjustificativa,
                                relogio = @relogio                          
						WHERE id = @id";

            DELETE = @"  DELETE FROM LancamentoLoteBilhetesImp WHERE id = @id ";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LancamentoLoteBilhetesImp";

        }

        #region Metodos
        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.ExcluirAux(trans, obj);
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteMudancaHorario>();
                    obj = Mapper.Map<Modelo.LancamentoLoteMudancaHorario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLoteMudancaHorario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLoteMudancaHorario();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.LancamentoLoteBilhetesImp obj)
        {
            try
            {

                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteBilhetesImp>();
                    obj = Mapper.Map<Modelo.LancamentoLoteBilhetesImp>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLoteBilhetesImp();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLoteBilhetesImp();
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
        }

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.LancamentoLoteBilhetesImp> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.LancamentoLoteBilhetesImp>>();
                    obj = Mapper.Map<List<Modelo.LancamentoLoteBilhetesImp>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.LancamentoLoteBilhetesImp>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.LancamentoLoteBilhetesImp>();
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
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idLancamentoLote", SqlDbType.Int),
				new SqlParameter ("@hora", SqlDbType.VarChar),
                new SqlParameter ("@ocorrencia", SqlDbType.VarChar),
                new SqlParameter ("@motivo", SqlDbType.VarChar),
                new SqlParameter ("@idjustificativa", SqlDbType.Int),
                new SqlParameter ("@relogio", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Codigo;
            parms[2].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Incdata;
            parms[3].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Inchora;
            parms[4].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Incusuario;
            parms[5].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Altdata;
            parms[6].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Althora;
            parms[7].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Altusuario;
            parms[8].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).IdLancamentoLote;
            parms[9].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Hora;
            parms[10].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Ocorrencia;
            parms[11].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Motivo;
            parms[12].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Idjustificativa;
            parms[13].Value = ((Modelo.LancamentoLoteBilhetesImp)obj).Relogio;
        }

        public Modelo.LancamentoLoteBilhetesImp LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LancamentoLoteBilhetesImp obj = new Modelo.LancamentoLoteBilhetesImp();
            try
            {
                SetaObjeto(dr, ref obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }

        public List<Modelo.LancamentoLoteBilhetesImp> GetAllList()
        {
            List<Modelo.LancamentoLoteBilhetesImp> lista = new List<Modelo.LancamentoLoteBilhetesImp>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            SetaListaObjeto(dr,ref lista);
            return lista;
        }

        /// <summary>
        /// Retorna o LancamentoLoteBilhetesImp de acordo com o id de um LancamentoLote
        /// </summary>
        /// <param name="idLote">Id do LançamentoLote</param>
        /// <returns>Retorno objeto LancamentoLoteBilhetesImp</returns>
        public Modelo.LancamentoLoteBilhetesImp LoadByIdLote(int idLote)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idLote", SqlDbType.Int)
            };

            parms[0].Value = idLote;

            string sql = @" select llIB.*
                               from LancamentoLoteBilhetesImp llIB
                              where idlancamentolote = @idLote";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteBilhetesImp>();
                    return AutoMapper.Mapper.Map<Modelo.LancamentoLoteBilhetesImp>(dr);
                }
                else
                {
                    return new Modelo.LancamentoLoteBilhetesImp();
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
        }

        public void ExcluirLancamentoLoteBilhetesImp(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[2]
            { 
                    new SqlParameter("@idsLancamentoLoteFuncionario", SqlDbType.VarChar),
                    new SqlParameter("@acao", SqlDbType.Int)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);
            parms[1].Value = 3;

            string aux = UPDATEBILHETEMARCACAO;

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();

            aux = @" delete from BilhetesImp where idLancamentoLoteFuncionario in (select * from f_clausulain (@idsLancamentoLoteFuncionario)) ";

            SqlCommand cmd2 = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd2.Parameters.Clear();
        }

        public void IncluirLancamentoLoteBilhetesImp(SqlTransaction trans, Modelo.LancamentoLote lancLote)
        {
            IList<Modelo.LancamentoLoteFuncionario> lFuncAdd = lancLote.LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Incluir && x.UltimaAcao == (int)Modelo.Acao.Incluir && x.Efetivado == true).ToList();
            Funcionario dalFunc = new Funcionario(db);
            List<Modelo.Funcionario> funcs = dalFunc.GetAllListByIds(String.Join(",", lFuncAdd.Select(s => s.IdFuncionario).Distinct()));
            BilhetesImp dalBilhetes = new BilhetesImp(db);
            dalBilhetes.UsuarioLogado = UsuarioLogado;
            if (lFuncAdd.Count() > 0)
            {
                List<Modelo.BilhetesImp> lBilhetes = new List<Modelo.BilhetesImp>();
                foreach (Modelo.LancamentoLoteFuncionario lancFuncionario in lFuncAdd)
                {
                    Modelo.Funcionario func = funcs.Where(w => w.Id == lancFuncionario.IdFuncionario).FirstOrDefault();
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    objBilhete.Ordem = "000";
                    objBilhete.Data = Convert.ToDateTime(lancLote.DataLancamento).Date;
                    objBilhete.Hora = lancLote.LancamentoLoteBilhetesImp.Hora;
                    objBilhete.Func = func.Dscodigo.ToString();
                    objBilhete.Relogio = lancLote.LancamentoLoteBilhetesImp.Relogio;
                    objBilhete.Importado = 0;
                    objBilhete.Codigo = 0;
                    objBilhete.Mar_data = objBilhete.Data;
                    objBilhete.Mar_hora = objBilhete.Hora;
                    objBilhete.Mar_relogio = objBilhete.Relogio;
                    objBilhete.DsCodigo = objBilhete.Func;
                    objBilhete.Incdata = DateTime.Now;
                    objBilhete.Inchora = DateTime.Now;
                    objBilhete.IdLancamentoLoteFuncionario = lancFuncionario.Id;
                    objBilhete.Idjustificativa = lancLote.LancamentoLoteBilhetesImp.Idjustificativa;
                    objBilhete.Motivo = lancLote.LancamentoLoteBilhetesImp.Motivo;
                    objBilhete.Ocorrencia = lancLote.LancamentoLoteBilhetesImp.Ocorrencia[0];
                    objBilhete.IdFuncionario = func.Id;
                    objBilhete.PIS = func.Pis;
                    lBilhetes.Add(objBilhete);
                }

                int ret = dalBilhetes.IncluirbilhetesEmLote(trans, lBilhetes);
            }
        }

        public void UpdateLancamentoLoteBilhetesImp(SqlTransaction trans, Modelo.LancamentoLote lancLote)
        {
            BilhetesImp dalBilhetes = new BilhetesImp(db);
            dalBilhetes.UsuarioLogado = UsuarioLogado;
            List<int> lFuncAlt = new List<int>();
            if (lancLote.LancamentoLoteFuncionarios.Where(x => x.Acao != Modelo.Acao.Excluir && x.UltimaAcao == (int)Modelo.Acao.Alterar && x.Efetivado == true).Count() > 0 &&
                (lancLote.DataLancamento != lancLote.DataLancamentoAnt ||
                 lancLote.LancamentoLoteBilhetesImp.Hora != lancLote.LancamentoLoteBilhetesImp.Hora_Ant))
            {
                lFuncAlt = lancLote.LancamentoLoteFuncionarios.Where(x => x.Acao == Modelo.Acao.Alterar).Select(s => s.Id).ToList();
            }
            if (lFuncAlt.Count() > 0)
            {
                List<Modelo.BilhetesImp> lBilhetes = GetBilhetesLoteFuncionario(lFuncAlt, trans);
                foreach (Modelo.BilhetesImp bil in lBilhetes)
                {
                    SetDadosAlt(bil);
                    bil.Data = lancLote.DataLancamento;
                    bil.Hora = lancLote.LancamentoLoteBilhetesImp.Hora;
                    bil.Relogio = lancLote.LancamentoLoteBilhetesImp.Relogio;
                    bil.Mar_data = bil.Data;
                    bil.Mar_hora = bil.Hora;
                    bil.Mar_relogio = bil.Relogio;
                    bil.Ocorrencia = lancLote.LancamentoLoteBilhetesImp.Ocorrencia[0];
                    bil.Idjustificativa = lancLote.LancamentoLoteBilhetesImp.Idjustificativa;
                    bil.Motivo = lancLote.LancamentoLoteBilhetesImp.Motivo;
                }

                dalBilhetes.AtualizarBilhetesEmLote(lBilhetes, trans);

                SqlParameter[] parms = new SqlParameter[2]
                { 
                        new SqlParameter("@idsLancamentoLoteFuncionario", SqlDbType.VarChar),
                        new SqlParameter("@acao", SqlDbType.Int)
                };
                parms[0].Value = string.Join(",", lFuncAlt);
                parms[1].Value = 2;

                string aux = UPDATEBILHETEMARCACAO;

                SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
                cmd.Parameters.Clear();
            }
        }

        public List<Modelo.BilhetesImp> GetBilhetesLoteFuncionario(List<int> idsLancamentoLoteFuncionario, SqlTransaction trans)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsLancamentoLoteFuncionario);

            string sql = @"select *
                              from bilhetesimp bi
                             where bi.idlancamentolotefuncionario in (select * from F_ClausulaIn(@ids))";

            SqlDataReader dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, sql, parms);

            List<Modelo.BilhetesImp> lista = new List<Modelo.BilhetesImp>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.BilhetesImp>();
                lista = AutoMapper.Mapper.Map<List<Modelo.BilhetesImp>>(dr);
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
    }
}
