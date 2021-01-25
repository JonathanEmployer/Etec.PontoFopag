using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace DAL.SQL
{
    public class JornadaAlternativaFuncionario : DAL.SQL.DALBase, DAL.IJornadaAlternativaFuncionario
    {

        public JornadaAlternativaFuncionario(DataBase database)
        {
            db = database;
            TABELA = "JornadaAlternativaFuncionario";

            SELECTPID = @"   SELECT * FROM JornadaAlternativaFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM JornadaAlternativaFuncionario";

            INSERT = @"  INSERT INTO JornadaAlternativaFuncionario
							(codigo, incdata, inchora, incusuario, idJornadaAlternativa, idFuncionario)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idJornadaAlternativa, @idFuncionario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE JornadaAlternativaFuncionario SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idFuncionario = @idFuncionario,
                                idJornadaAlternativa = @idJornadaAlternativa          
						WHERE id = @id";

            DELETE = @"  DELETE FROM JornadaAlternativaFuncionario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM JornadaAlternativaFuncionario";

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
                    var map = Mapper.CreateMap<IDataReader, Modelo.JornadaAlternativaFuncionario>();
                    obj = Mapper.Map<Modelo.JornadaAlternativaFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.JornadaAlternativaFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.JornadaAlternativaFuncionario();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.JornadaAlternativaFuncionario obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.JornadaAlternativaFuncionario>();
                    obj = Mapper.Map<Modelo.JornadaAlternativaFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.JornadaAlternativaFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.JornadaAlternativaFuncionario();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.JornadaAlternativaFuncionario> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.JornadaAlternativaFuncionario>>();
                    obj = Mapper.Map<List<Modelo.JornadaAlternativaFuncionario>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.JornadaAlternativaFuncionario>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.JornadaAlternativaFuncionario>();
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
                new SqlParameter ("@IdJornadaAlternativa", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.JornadaAlternativaFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.JornadaAlternativaFuncionario)obj).Incdata;
            parms[3].Value = ((Modelo.JornadaAlternativaFuncionario)obj).Inchora;
            parms[4].Value = ((Modelo.JornadaAlternativaFuncionario)obj).Incusuario;
            parms[5].Value = ((Modelo.JornadaAlternativaFuncionario)obj).Altdata;
            parms[6].Value = ((Modelo.JornadaAlternativaFuncionario)obj).Althora;
            parms[7].Value = ((Modelo.JornadaAlternativaFuncionario)obj).Altusuario;
            parms[8].Value = ((Modelo.JornadaAlternativaFuncionario)obj).IdJornadaAlternativa;
            parms[9].Value = ((Modelo.JornadaAlternativaFuncionario)obj).IdFuncionario;
        }

        public Modelo.JornadaAlternativaFuncionario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.JornadaAlternativaFuncionario obj = new Modelo.JornadaAlternativaFuncionario();
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

        public List<Modelo.JornadaAlternativaFuncionario> GetAllList()
        {
            return GetListWhere("");
        }

        public List<Modelo.JornadaAlternativaFuncionario> GetListWhere(string condicao)
        {
            List<Modelo.JornadaAlternativaFuncionario> lista = new List<Modelo.JornadaAlternativaFuncionario>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM JornadaAlternativaFuncionario where 1 = 1 " + condicao, parms);
            SetaListaObjeto(dr, ref lista);
            return lista;
        }
        #endregion






        /// <summary>
        ///  Retorna lista de JornadaAlternativa de um Funcionário
        /// </summary>
        /// <param name="idFuncionario"> Id do Funionário </param>
        /// <returns>Lista de JornadaAlternativa</returns>
        public List<Modelo.JornadaAlternativa> ListaJornadaAlternativaFuncionario(int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idFuncionario", SqlDbType.Int)
            };
            parms[0].Value = idFuncionario;

            string sql = @" select * 
                              from JornadaAlternativaFuncionario jaf
                             inner join jornadaalternativa ja on ja.id = jaf.idJornadaAlternativa
                             where jaf.idFuncionario in (select * from f_clausulain(@idFuncionario))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.JornadaAlternativa> lista = new List<Modelo.JornadaAlternativa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.JornadaAlternativa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.JornadaAlternativa>>(dr);
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
        ///  Retorna lista de Funcionários ligados a uma JornadaAlternativa
        /// </summary>
        /// <param name="idjornadaAlternativa"> Id da JornadaAlternativa </param>
        /// <returns>Lista de Funcionários</returns>
        public List<Modelo.Funcionario> ListaFuncionariosJornadaAlternativa(int idjornadaAlternativa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idjornadaAlternativa", SqlDbType.Int)
            };
            parms[0].Value = idjornadaAlternativa;

            string sql = @" select fun.* 
                              from JornadaAlternativaFuncionario jaf
                             inner join jornadaalternativa ja on ja.id = jaf.idjornadaAlternativa
                             inner join funcionario fun on jaf.idFuncionario = fun.id
                             where ja.id = @idjornadaAlternativa";

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

        public void IncluirJornadaAlternativaFuncionarioLote(SqlTransaction trans, int idjornadaAlternativa, string idsFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                new SqlParameter("@idjornadaAlternativa", SqlDbType.Int),
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = idjornadaAlternativa;
            parms[1].Value = idsFuncionarios;

            string aux = @" insert into JornadaAlternativaFuncionario(codigo, incdata, inchora, incusuario, idjornadaAlternativa, idFuncionario)
                            select isnull((select max(codigo) from JornadaAlternativaFuncionario),0) + 1,
	                               ja.incdata,
	                               ja.inchora,
	                               ja.incusuario,
	                               ja.id,
	                               func.id
                              from funcionario func
                              left join jornadaalternativa ja on ja.id =  @idjornadaAlternativa
                              left join JornadaAlternativaFuncionario jaf on func.id = jaf.idFuncionario and jaf.idjornadaAlternativa = ja.id
                             where func.id in (select * from F_ClausulaIn(@ids))
                               and jaf.idFuncionario is null ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void ExcluirJornadaAlternativaFuncionarioLote(SqlTransaction trans, int idjornadaAlternativa)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idjornadaAlternativa", SqlDbType.Int)
            };
            parms[0].Value = idjornadaAlternativa;

            string aux = @" delete from JornadaAlternativaFuncionario where idjornadaAlternativa = @idjornadaAlternativa ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

    }
}
