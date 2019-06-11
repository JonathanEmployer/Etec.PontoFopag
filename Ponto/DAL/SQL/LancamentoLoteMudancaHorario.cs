using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class LancamentoLoteMudancaHorario : DAL.SQL.DALBase, DAL.ILancamentoLoteMudancaHorario
    {

        public LancamentoLoteMudancaHorario(DataBase database)
        {
            db = database;
            TABELA = "LancamentoLoteMudancaHorario";

            SELECTPID = @"   SELECT * FROM LancamentoLoteMudancaHorario WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM LancamentoLoteMudancaHorario";

            INSERT = @"  INSERT INTO LancamentoLoteMudancaHorario
							(codigo, incdata, inchora, incusuario, idLancamentoLote, tipohorario, idhorario, IdHorarioDinamico, CicloSequenciaIndice)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idLancamentoLote, @tipohorario, @idhorario, @IdHorarioDinamico, @CicloSequenciaIndice)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE LancamentoLoteMudancaHorario SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idLancamentoLote = @idLancamentoLote, 
                                tipohorario = @tipohorario, 
                                idhorario = @idhorario,
                                CicloSequenciaIndice = @CicloSequenciaIndice,
                                IdHorarioDinamico = @IdHorarioDinamico
						WHERE id = @id";

            DELETE = @"  DELETE FROM LancamentoLoteMudancaHorario WHERE id = @id ";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LancamentoLoteMudancaHorario";

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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.LancamentoLoteMudancaHorario obj)
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
                dr.Dispose();
            }
        }

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.LancamentoLoteMudancaHorario> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.LancamentoLoteMudancaHorario>>();
                    obj = Mapper.Map<List<Modelo.LancamentoLoteMudancaHorario>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.LancamentoLoteMudancaHorario>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.LancamentoLoteMudancaHorario>();
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
				new SqlParameter ("@tipohorario", SqlDbType.Int),
                new SqlParameter ("@idhorario", SqlDbType.Int),
                new SqlParameter ("@IdHorarioDinamico", SqlDbType.Int),
                new SqlParameter ("@CicloSequenciaIndice", SqlDbType.Int)
                
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
            parms[1].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Codigo;
            parms[2].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Incdata;
            parms[3].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Inchora;
            parms[4].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Incusuario;
            parms[5].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Altdata;
            parms[6].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Althora;
            parms[7].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Altusuario;
            parms[8].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).IdLancamentoLote;
            parms[9].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Tipohorario;
            parms[10].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).Idhorario;
            parms[11].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).IdHorarioDinamico;
            parms[12].Value = ((Modelo.LancamentoLoteMudancaHorario)obj).CicloSequenciaIndice;
        }

        public Modelo.LancamentoLoteMudancaHorario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LancamentoLoteMudancaHorario obj = new Modelo.LancamentoLoteMudancaHorario();
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

        public List<Modelo.LancamentoLoteMudancaHorario> GetAllList()
        {
            List<Modelo.LancamentoLoteMudancaHorario> lista = new List<Modelo.LancamentoLoteMudancaHorario>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            SetaListaObjeto(dr,ref lista);
            return lista;
        }

        /// <summary>
        /// Retorna o LancamentoLoteMudancaHorario de acordo com o id de um LancamentoLote
        /// </summary>
        /// <param name="idLote">Id do LançamentoLote</param>
        /// <returns>Retorno objeto LancamentoLoteMudancaHorario</returns>
        public Modelo.LancamentoLoteMudancaHorario LoadByIdLote(int idLote)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idLote", SqlDbType.Int)
            };

            parms[0].Value = idLote;

            string sql = @" select llmh.*,
		                            case when llmh.tipohorario = 1 then
				                            Convert(varchar,h.codigo) + ' | ' + h.descricao
			                             else '' end HorarioNormal,
		                            case when llmh.tipohorario = 2 then
				                            Convert(varchar,h.codigo) + ' | ' + h.descricao
		                            else '' end HorarioFlexivel
									, convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico 
                               from lancamentolotemudancahorario llmh
                              inner join horario h on llmh.idhorario = h.id
							  left join horariodinamico hdn on hdn.id = llmh.idhorariodinamico
                              where idlancamentolote = @idLote";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LancamentoLoteMudancaHorario>();
                    Modelo.LancamentoLoteMudancaHorario ret = AutoMapper.Mapper.Map<Modelo.LancamentoLoteMudancaHorario>(dr);
                    ret.Tipohorario = ret.IdHorarioDinamico.GetValueOrDefault() > 0 ? 3 : ret.Tipohorario;
                    return ret;
                }
                else
                {
                    return new Modelo.LancamentoLoteMudancaHorario();
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

        public void ExcluirMudancaHorarioLote(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" update funcionario
                               set tipohorario = MudancaAnt.tipohorario,
		                            idhorario = MudancaAnt.idhorario
                              from funcionario
                            inner join (
		                            select *
		                              from (
			                            select ROW_NUMBER() over(partition by mudAnt.idfuncionario order by mudAnt.data desc, mudAnt.id) ordem,
				                               isnull(mudAnt.idhorario, mudancahorario.tipohorario_ant) tipohorario,
				                               isnull(mudAnt.idhorario, mudancahorario.idhorario_ant) idhorario,
				                               mudancahorario.idfuncionario
			                              from mudancahorario 
			                             left join mudancahorario mudAnt on mudancahorario.idfuncionario = mudAnt.idfuncionario and mudAnt.data <= mudancahorario.data and mudAnt.id != mudancahorario.id
			                             where mudancahorario.idLancamentoLoteFuncionario in (select * from f_clausulain (@idlancamentolotefuncionario))
			                               ) t where t.ordem = 1
	                                   ) MudancaAnt on funcionario.id = MudancaAnt.idfuncionario;

                             UPDATE dbo.marcacao SET tipohoraextrafalta = p.tipohoraextrafalta
                              FROM dbo.marcacao m
                             INNER JOIN dbo.LancamentoLoteFuncionario llf ON m.idfuncionario = llf.idFuncionario AND llf.id in (select * from f_clausulain (@idlancamentolotefuncionario))
                             INNER JOIN dbo.mudancahorario mh ON mh.idLancamentoLoteFuncionario = llf.id AND m.data >= mh.data
                             INNER JOIN dbo.horario h ON mh.idhorario_ant = h.id
                             INNER JOIN dbo.parametros p ON h.idparametro = p.id;

                            delete from mudancahorario where idLancamentoLoteFuncionario in (select * from f_clausulain (@idlancamentolotefuncionario)) ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void IncluirMudancaHorarioLote(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" insert into mudancahorario
                            select isnull(codUltMud.codigo,0) + row_number() over(order by llf.id) Codigo,
	                               llf.idfuncionario,
	                               llmh.tipohorario,
	                               llmh.idhorario,
	                               ll.datalancamento data,
	                               isnull(isnull(mudAnt.tipohorario, mudPos.tipohorario_ant), f.tipohorario) tipohorario_ant,
	                               isnull(isnull(mudAnt.idhorario, mudpos.idhorario_ant), f.idhorario) idhorario_ant,
	                               llf.incdata,
	                               llf.inchora,
	                               llf.incusuario,
	                               llf.altdata,
	                               llf.althora,
	                               llf.altusuario,
	                               llf.id idLancamentoLoteFuncionario,
                                   llmh.idHorarioDinamico,
                                   llmh.CicloSequenciaIndice
                              from LancamentoLote ll
                             inner join lancamentolotemudancahorario llmh on ll.id = llmh.idlancamentolote
                             inner join LancamentoLoteFuncionario llf on ll.id = llf.idlancamentolote and llf.id in ( select * from f_clausulain(@idlancamentolotefuncionario))
                             inner join funcionario f on llf.idfuncionario = f.id
                             cross join (select max(codigo) codigo from mudancahorario) codUltMud
                              left join ( select *
                                            from (
					                            select *, row_number() over(partition by idfuncionario order by data desc) ordem 
						                            from mudancahorario 
						                            where data < (select top(1) datalancamento from LancamentoLote lla inner join LancamentoLoteFuncionario llfa on lla.id = llfa.idLancamentoLote where llfa.id in ( select * from f_clausulain(@idlancamentolotefuncionario)))) t where t.ordem = 1 ) mudAnt on llf.idfuncionario = mudAnt.idFuncionario
                            left join ( select *
                                            from (
					                            select *, row_number() over(partition by idfuncionario order by data) ordem 
						                            from mudancahorario 
						                            where data > (select top(1) datalancamento from LancamentoLote lla inner join LancamentoLoteFuncionario llfa on lla.id = llfa.idLancamentoLote where llfa.id in ( select * from f_clausulain(@idlancamentolotefuncionario)))) t where t.ordem = 1 ) mudPos on llf.idfuncionario = mudPos.idFuncionario
                             where not exists (select * from mudancahorario where idLancamentoLoteFuncionario = llf.id) and llf.efetivado = 1;

                         UPDATE dbo.marcacao SET tipohoraextrafalta = p.tipohoraextrafalta
                          FROM dbo.marcacao m
                         INNER JOIN dbo.LancamentoLoteFuncionario llf ON m.idfuncionario = llf.idFuncionario AND llf.id in (select * from f_clausulain (@idlancamentolotefuncionario))
                         INNER JOIN dbo.mudancahorario mh ON mh.idLancamentoLoteFuncionario = llf.id AND m.data >= mh.data
                         INNER JOIN dbo.horario h ON mh.idhorario = h.id
                         INNER JOIN dbo.parametros p ON h.idparametro = p.id
                        ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void UpdateMudancaHorarioLote(SqlTransaction trans, List<int> idlancamentolotefuncionario)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idlancamentolotefuncionario", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idlancamentolotefuncionario);

            string aux = @" update mudancahorario
                               set tipohorario = llmh.tipohorario,
	                               idhorario = llmh.idhorario,
	                               data = ll.datalancamento,
	                               tipohorario_ant = isnull(isnull(mudAnt.tipohorario, mudPos.tipohorario_ant), f.tipohorario),
	                               idhorario_ant = isnull(isnull(mudAnt.idhorario, mudpos.idhorario_ant), f.idhorario),
	                               altdata = llf.altdata,
	                               althora = llf.althora,
	                               altusuario = llf.altusuario
                              from mudancahorario
                             inner join LancamentoLoteFuncionario llf on mudancahorario.idlancamentolotefuncionario = llf.id and llf.id in ( select * from f_clausulain(@idlancamentolotefuncionario)) and efetivado = 1
                             inner join LancamentoLote ll on ll.id = llf.idlancamentolote
                             inner join funcionario f on llf.idfuncionario = f.id
                             inner join lancamentolotemudancahorario llmh on ll.id = llmh.idlancamentolote
                              left join ( select *
                                            from (
					                            select *, row_number() over(partition by idfuncionario order by data desc) ordem 
						                            from mudancahorario 
						                            where data < (select top(1) datalancamento from LancamentoLote lla inner join LancamentoLoteFuncionario llfa on lla.id = llfa.idLancamentoLote where llfa.id in ( select * from f_clausulain(@idlancamentolotefuncionario)))) t where t.ordem = 1 ) mudAnt on llf.idfuncionario = mudAnt.idFuncionario
							  left join ( select *
                                            from (
					                            select *, row_number() over(partition by idfuncionario order by data) ordem 
						                            from mudancahorario 
						                            where data > (select top(1) datalancamento from LancamentoLote lla inner join LancamentoLoteFuncionario llfa on lla.id = llfa.idLancamentoLote where llfa.id in ( select * from f_clausulain(@idlancamentolotefuncionario)))) t where t.ordem = 1 ) mudPos on llf.idfuncionario = mudPos.idFuncionario ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }
        #endregion
    }
}
