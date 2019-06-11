using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class LancamentoLoteAfastamento : DAL.SQL.DALBase, DAL.ILancamentoLoteAfastamento
    {
        public LancamentoLoteAfastamento(DataBase database)
        {
            db = database;
            TABELA = "LancamentoLoteAfastamento";

            SELECTPID = @"   SELECT * FROM LancamentoLoteAfastamento WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM LancamentoLoteAfastamento";

            INSERT = @"  INSERT INTO LancamentoLoteAfastamento
							(codigo, incdata, inchora, incusuario, idLancamentoLote, idOcorrencia, abonado, dataI, dataF, abonoDiurno, abonoNoturno, parcial, semCalculo, bSuspensao)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idLancamentoLote, @idOcorrencia, @abonado, @dataI, @dataF, @abonoDiurno, @abonoNoturno, @parcial, @semCalculo, @bSuspensao)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LancamentoLoteAfastamento SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idLancamentoLote = @idLancamentoLote,
                                idOcorrencia = @idOcorrencia, 
                                abonado = @abonado, 
                                dataI = @dataI, 
                                dataF = @dataF, 
                                abonoDiurno = @abonoDiurno, 
                                abonoNoturno = @abonoNoturno, 
                                parcial = @parcial, 
                                semCalculo = @semCalculo, 
                                bSuspensao = @bSuspensao
						WHERE id = @id";

            DELETE = @"  DELETE FROM LancamentoLoteAfastamento WHERE id = @id ";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LancamentoLoteAfastamento";

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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.LancamentoLoteAfastamento obj)
        {
            try
            {

                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteAfastamento>();
                    obj = Mapper.Map<Modelo.LancamentoLoteAfastamento>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.LancamentoLoteAfastamento();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.LancamentoLoteAfastamento();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.LancamentoLoteAfastamento> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.LancamentoLoteAfastamento>>();
                    obj = Mapper.Map<List<Modelo.LancamentoLoteAfastamento>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.LancamentoLoteAfastamento>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.LancamentoLoteAfastamento>();
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
				new SqlParameter ("@idOcorrencia", SqlDbType.Int),
                new SqlParameter ("@abonado", SqlDbType.TinyInt),
                new SqlParameter ("@dataI", SqlDbType.DateTime),
                new SqlParameter ("@dataF", SqlDbType.DateTime),
                new SqlParameter ("@abonoDiurno", SqlDbType.VarChar),
                new SqlParameter ("@abonoNoturno", SqlDbType.VarChar),
                new SqlParameter ("@parcial", SqlDbType.TinyInt),
                new SqlParameter ("@semCalculo", SqlDbType.TinyInt),
                new SqlParameter ("@bSuspensao", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.LancamentoLoteAfastamento)obj).Codigo;
            parms[2].Value = ((Modelo.LancamentoLoteAfastamento)obj).Incdata;
            parms[3].Value = ((Modelo.LancamentoLoteAfastamento)obj).Inchora;
            parms[4].Value = ((Modelo.LancamentoLoteAfastamento)obj).Incusuario;
            parms[5].Value = ((Modelo.LancamentoLoteAfastamento)obj).Altdata;
            parms[6].Value = ((Modelo.LancamentoLoteAfastamento)obj).Althora;
            parms[7].Value = ((Modelo.LancamentoLoteAfastamento)obj).Altusuario;
            parms[8].Value = ((Modelo.LancamentoLoteAfastamento)obj).IdLancamentoLote;
            parms[9].Value = ((Modelo.LancamentoLoteAfastamento)obj).IdOcorrencia;
            parms[10].Value = ((Modelo.LancamentoLoteAfastamento)obj).Abonado;
            parms[11].Value = ((Modelo.LancamentoLoteAfastamento)obj).DataI;
            parms[12].Value = ((Modelo.LancamentoLoteAfastamento)obj).DataF;
            parms[13].Value = ((Modelo.LancamentoLoteAfastamento)obj).AbonoDiurno;
            parms[14].Value = ((Modelo.LancamentoLoteAfastamento)obj).AbonoNoturno;
            parms[15].Value = ((Modelo.LancamentoLoteAfastamento)obj).Parcial;
            parms[16].Value = ((Modelo.LancamentoLoteAfastamento)obj).SemCalculo;
            parms[17].Value = ((Modelo.LancamentoLoteAfastamento)obj).BSuspensao;
        }

        public Modelo.LancamentoLoteAfastamento LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LancamentoLoteAfastamento obj = new Modelo.LancamentoLoteAfastamento();
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

        public List<Modelo.LancamentoLoteAfastamento> GetAllList()
        {
            List<Modelo.LancamentoLoteAfastamento> lista = new List<Modelo.LancamentoLoteAfastamento>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            SetaListaObjeto(dr,ref lista);
            return lista;
        }

        /// <summary>
        /// Retorna o LancamentoLoteAfastamento de acordo com o id de um LancamentoLote
        /// </summary>
        /// <param name="idLote">Id do LançamentoLote</param>
        /// <returns>Retorno objeto LancamentoLoteAfastamento</returns>
        public Modelo.LancamentoLoteAfastamento LoadByIdLote(int idLote)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idLote", SqlDbType.Int)
            };

            parms[0].Value = idLote;

            string sql = @" select llA.*, Convert(varchar,o.codigo) + ' | ' + o.descricao ocorrencia
                               from LancamentoLoteAfastamento llA
							  inner join ocorrencia o on llA.idocorrencia = o.id
                              where idlancamentolote = @idLote";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteAfastamento>();
                    return AutoMapper.Mapper.Map<Modelo.LancamentoLoteAfastamento>(dr);
                }
                else
                {
                    return new Modelo.LancamentoLoteAfastamento();
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

        /// <summary>
        /// Retorna os afastamentos relacionados aos LancamentoLoteFuncionario
        /// </summary>
        /// <param name="idsLancamentoLoteFuncionario">Ids dos LancamentoLoteFuncionario que deseja recuparar os afastamentos</param>
        /// <param name="trans">Transação</param>
        /// <returns></returns>
        public List<Modelo.Afastamento> GetAfastamentosLoteFuncionario(List<int> idsLancamentoLoteFuncionario, SqlTransaction trans)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsLancamentoLoteFuncionario);

            string sql = @"select *
                              from afastamento a
                             where a.idlancamentolotefuncionario in (select * from F_ClausulaIn(@ids))";

            SqlDataReader dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, sql, parms);

            List<Modelo.Afastamento> lista = new List<Modelo.Afastamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Afastamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Afastamento>>(dr);
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

        public void IncluirAfastamentoLote(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" insert into afastamento (codigo, idocorrencia, tipo, abonado, datai, dataf, idfuncionario, horai, horaf, parcial, semcalculo, incdata, inchora, incusuario, altdata, althora, altusuario, bsuspensao, idLancamentoLoteFuncionario)
                            select isnull(codUlt.codigo,0) + row_number() over(order by llf.id) Codigo,
	                               lla.idocorrencia,
								   0 tipo,
								   lla.abonado,
								   lla.dataI,
								   lla.DataF,
								   llf.idfuncionario,
								   lla.abonoDiurno,
								   lla.abonoNoturno,
								   lla.parcial,
								   lla.semCalculo,
	                               llf.incdata,
	                               llf.inchora,
	                               llf.incusuario,
	                               llf.altdata,
	                               llf.althora,
	                               llf.altusuario,
								   lla.bsuspensao,
	                               llf.id idLancamentoLoteFuncionario
                              from LancamentoLote ll
                             inner join LancamentoLoteAfastamento lla on ll.id = lla.idlancamentolote
                             inner join LancamentoLoteFuncionario llf on ll.id = llf.idlancamentolote and llf.id in ( select * from f_clausulain(@idlancamentolotefuncionario))
                             inner join funcionario f on llf.idfuncionario = f.id
							 cross join (select max(codigo) codigo from afastamento) codUlt
                             where not exists (select * from afastamento where idLancamentoLoteFuncionario = llf.id) and llf.efetivado = 1 ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }


        public void UpdateAfastamentoLote(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" update afastamento
                               set idocorrencia = lla.idocorrencia,
								   abonado = lla.abonado,	
								   datai = lla.dataI,
								   dataf = lla.DataF,
	                               horai = lla.abonoDiurno,
								   horaf = lla.abonoNoturno,
								   parcial = lla.parcial,
								   semcalculo = lla.semCalculo,
								   bsuspensao = lla.bsuspensao,
	                               altdata = llf.altdata,
	                               althora = llf.althora,
	                               altusuario = llf.altusuario
                              from afastamento
                             inner join LancamentoLoteFuncionario llf on afastamento.idlancamentolotefuncionario = llf.id and llf.id in ( select * from f_clausulain(@idlancamentolotefuncionario)) and efetivado = 1
                             inner join LancamentoLote ll on ll.id = llf.idlancamentolote
                             inner join funcionario f on llf.idfuncionario = f.id
                             inner join LancamentoLoteAfastamento lla on ll.id = lla.idlancamentolote ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void ExcluirAfastamentoLote(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" delete from afastamento where idLancamentoLoteFuncionario in (select * from f_clausulain (@idlancamentolotefuncionario)) ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }
        #endregion
    }
}
