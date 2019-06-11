using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class FeriadoFuncionario : DAL.SQL.DALBase, DAL.IFeriadoFuncionario
    {

        public FeriadoFuncionario(DataBase database)
        {
            db = database;
            TABELA = "FeriadoFuncionario";

            SELECTPID = @"   SELECT * FROM FeriadoFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM FeriadoFuncionario";

            INSERT = @"  INSERT INTO FeriadoFuncionario
							(codigo, incdata, inchora, incusuario, idFeriado, idFuncionario, idFuncionario)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idFeriado, @idFuncionario, @idFuncionario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE FeriadoFuncionario SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idFechamentoPonto = @idFechamentoPonto, 
                                idFuncionario = @idFuncionario,
                                idFeriado = @idFeriado, 
                                idFuncionario = @idFuncionario                   
						WHERE id = @id";

            DELETE = @"  DELETE FROM FeriadoFuncionario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM FeriadoFuncionario";

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

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.FeriadoFuncionario>();
                    obj = Mapper.Map<Modelo.FeriadoFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.FeriadoFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.FeriadoFuncionario();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.FeriadoFuncionario obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.FeriadoFuncionario>();
                    obj = Mapper.Map<Modelo.FeriadoFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.FeriadoFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.FeriadoFuncionario();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.FeriadoFuncionario> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.FeriadoFuncionario>>();
                    obj = Mapper.Map<List<Modelo.FeriadoFuncionario>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.FeriadoFuncionario>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.FeriadoFuncionario>();
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
                new SqlParameter ("@IdFeriado", SqlDbType.Int),
				new SqlParameter ("@IdFuncionario", SqlDbType.Int)
                
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
            parms[1].Value = ((Modelo.FeriadoFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.FeriadoFuncionario)obj).Incdata;
            parms[3].Value = ((Modelo.FeriadoFuncionario)obj).Inchora;
            parms[4].Value = ((Modelo.FeriadoFuncionario)obj).Incusuario;
            parms[5].Value = ((Modelo.FeriadoFuncionario)obj).Altdata;
            parms[6].Value = ((Modelo.FeriadoFuncionario)obj).Althora;
            parms[7].Value = ((Modelo.FeriadoFuncionario)obj).Altusuario;
            parms[8].Value = ((Modelo.FeriadoFuncionario)obj).IdFeriado;
            parms[9].Value = ((Modelo.FeriadoFuncionario)obj).IdFuncionario;
        }

        public Modelo.FeriadoFuncionario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FeriadoFuncionario obj = new Modelo.FeriadoFuncionario();
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

        public List<Modelo.FeriadoFuncionario> GetAllList()
        {
            return GetListWhere("");
        }

        public List<Modelo.FeriadoFuncionario> GetListWhere(string condicao)
        {
            List<Modelo.FeriadoFuncionario> lista = new List<Modelo.FeriadoFuncionario>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM FeriadoFuncionario where 1 = 1 " + condicao, parms);
            SetaListaObjeto(dr, ref lista);
            return lista;
        }
        #endregion

        /// <summary>
        ///  Retorna lista de Feriados de um Funcionário
        /// </summary>
        /// <param name="idFuncionario"> Id do Funionário </param>
        /// <returns>Lista de Feriados</returns>
        public List<Modelo.Feriado> ListaFeriadosFuncionario(int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idFuncionario", SqlDbType.Int)
            };
            parms[0].Value = idFuncionario;

            string sql = @" select * 
                              from FeriadoFuncionario ff
                             inner join feriado f on f.id = ff.idferiado
                             where ff.idfuncionario in (select * from f_clausulain(@idFuncionario))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Feriado>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Feriado>>(dr);
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


        /// <summary>
        ///  Retorna lista de Funcionários ligados a um Feriado
        /// </summary>
        /// <param name="idFeriado"> Id do Feriado </param>
        /// <returns>Lista de Funcionários</returns>
        public List<Modelo.Funcionario> ListaFuncionariosFeriado(int idFeriado)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idFeriado", SqlDbType.Int)
            };
            parms[0].Value = idFeriado;

            string sql = @" select fun.* 
                              from FeriadoFuncionario ff
                             inner join feriado f on f.id = ff.idferiado
                             inner join funcionario fun on ff.idfuncionario = fun.id
                             where f.id = @idFeriado";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
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

        public void IncluirFeriadoFuncionarioLote(SqlTransaction trans, int idFeriado, string idsFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[2]
            { 
                new SqlParameter("@idFeriado", SqlDbType.Int),
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = idFeriado;
            parms[1].Value = idsFuncionarios;

            string aux = @" insert into feriadofuncionario(codigo, incdata, inchora, incusuario, idferiado, idfuncionario)
                            select isnull((select max(codigo) from feriadofuncionario),0) + 1,
	                               f.incdata,
	                               f.inchora,
	                               f.incusuario,
	                               f.id,
	                               func.id
                              from funcionario func
                              left join feriado f on f.id =  @idFeriado
                              left join feriadofuncionario ff on func.id = ff.idFuncionario and ff.idFeriado = f.id
                             where func.id in (select * from F_ClausulaIn(@ids))
                               and ff.idFuncionario is null ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void ExcluirFeriadoFuncionarioLote(SqlTransaction trans, int idFeriado)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                new SqlParameter("@idFeriado", SqlDbType.Int)
            };
            parms[0].Value = idFeriado;

            string aux = @" delete from feriadofuncionario where idferiado = @idFeriado ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

    }
}
