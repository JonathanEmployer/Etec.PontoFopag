using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class FechamentoPontoFuncionario : DAL.SQL.DALBase, DAL.IFechamentoPontoFuncionario
    {

        public FechamentoPontoFuncionario(DataBase database)
        {
            db = database;
            TABELA = "FechamentoPontoFuncionario";

            SELECTPID = @"   SELECT * FROM FechamentoPontoFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM FechamentoPontoFuncionario";

            INSERT = @"  INSERT INTO FechamentoPontoFuncionario
							(codigo, incdata, inchora, incusuario, idFechamentoPonto, idFuncionario)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idFechamentoPonto, @idFuncionario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE FechamentoPontoFuncionario SET
							    codigo = @codigo, 
                                altdata = @altdata,
                                althora = @althora,
                                altusuario = @altusuario,
                                idFechamentoPonto = @idFechamentoPonto, 
                                idFuncionario = @idFuncionario                            
						WHERE id = @id";

            DELETE = @"  DELETE FROM FechamentoPontoFuncionario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM FechamentoPontoFuncionario";

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
                    var map = Mapper.CreateMap<IDataReader, Modelo.FechamentoPontoFuncionario>();
                    obj = Mapper.Map<Modelo.FechamentoPontoFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.FechamentoPontoFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.FechamentoPontoFuncionario();
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

        protected bool SetaObjeto(SqlDataReader dr, ref Modelo.FechamentoPontoFuncionario obj)
        {
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = Mapper.CreateMap<IDataReader, Modelo.FechamentoPontoFuncionario>();
                    obj = Mapper.Map<Modelo.FechamentoPontoFuncionario>(dr);
                    return true;
                }
                else
                {
                    obj = new Modelo.FechamentoPontoFuncionario();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.FechamentoPontoFuncionario();
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

        protected bool SetaListaObjeto(SqlDataReader dr, ref List<Modelo.FechamentoPontoFuncionario> obj)
        {
            try
            {
                if (dr.HasRows)
                {
                    var map = Mapper.CreateMap<IDataReader, List<Modelo.FechamentoPontoFuncionario>>();
                    obj = Mapper.Map<List<Modelo.FechamentoPontoFuncionario>>(dr);
                    return true;
                }
                else
                {
                    obj = new List<Modelo.FechamentoPontoFuncionario>();
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new List<Modelo.FechamentoPontoFuncionario>();
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
                new SqlParameter ("@IdFechamentoPonto", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.FechamentoPontoFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.FechamentoPontoFuncionario)obj).Incdata;
            parms[3].Value = ((Modelo.FechamentoPontoFuncionario)obj).Inchora;
            parms[4].Value = ((Modelo.FechamentoPontoFuncionario)obj).Incusuario;
            parms[5].Value = ((Modelo.FechamentoPontoFuncionario)obj).Altdata;
            parms[6].Value = ((Modelo.FechamentoPontoFuncionario)obj).Althora;
            parms[7].Value = ((Modelo.FechamentoPontoFuncionario)obj).Altusuario;
            parms[8].Value = ((Modelo.FechamentoPontoFuncionario)obj).IdFechamentoPonto;
            parms[9].Value = ((Modelo.FechamentoPontoFuncionario)obj).IdFuncionario;
        }

        public Modelo.FechamentoPontoFuncionario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.FechamentoPontoFuncionario obj = new Modelo.FechamentoPontoFuncionario();
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

        public List<Modelo.FechamentoPontoFuncionario> GetAllList()
        {
            return GetListWhere("");
        }

        public List<Modelo.FechamentoPontoFuncionario> GetListWhere(string condicao)
        {
            List<Modelo.FechamentoPontoFuncionario> lista = new List<Modelo.FechamentoPontoFuncionario>();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM FechamentoPontoFuncionario where 1 = 1 "+condicao, parms);
            SetaListaObjeto(dr, ref lista);
            return lista;
        }
        #endregion

        public void ExcluirLoteIds(SqlTransaction trans, List<int> ids)
        {
            SqlParameter[] parms = null;
            string strIDS = String.Join(",", ids);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"  DELETE FROM FechamentoPontoFuncionario WHERE id in (" + strIDS + ")", true, parms);
        }

        /// <summary>
        ///  Retorna uma lista com todos os funcionários e os seus fechamentos dentro de um período de acordo com um determinado tipo(empresa, departamento, funcionário, função)
        /// </summary>
        /// <param name="tipo">0 = Empresa, 1 = Departamento, 2 = Funcionário, 3 = Função, 4 = Todos </param>
        /// <param name="idsRegistros">Ids do registro a ser pesquisado (id de acordo com o tipo)</param>
        /// <param name="data"> Data </param>
        /// <returns></returns>


        public List<Modelo.Proxy.pxyFechamentoPontoFuncionario> ListaFechamentoPontoFuncionario(int tipo, List<int> idsRegistros, DateTime data, SqlTransaction trans)
        {
            List<Modelo.Proxy.pxyFechamentoPontoFuncionario> lista = new List<Modelo.Proxy.pxyFechamentoPontoFuncionario>();
            Modelo.Proxy.pxyFechamentoPontoFuncionario obj = new Modelo.Proxy.pxyFechamentoPontoFuncionario();
            SqlParameter[] parms;
            string sql;
            ListaFechamentoPontoFuncionario(tipo, idsRegistros, data, out parms, out sql);
             SqlDataReader dr = TransactDbOps.ExecuteReader(trans, CommandType.Text, sql, parms);
            try
            {
                //AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFechamentoPontoFuncionario>();
                //lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyFechamentoPontoFuncionario>>(dr);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        obj = new Modelo.Proxy.pxyFechamentoPontoFuncionario();
                        SetInstancePxyFechamentoPontoFuncionario(dr, obj);
                        lista.Add(obj);
                    }
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


        private void SetInstancePxyFechamentoPontoFuncionario(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Proxy.pxyFechamentoPontoFuncionario)obj).IdFechamentoPonto = Convert.ToInt32(dr["IdFechamentoPonto"]);
            ((Modelo.Proxy.pxyFechamentoPontoFuncionario)obj).IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]);
            ((Modelo.Proxy.pxyFechamentoPontoFuncionario)obj).CodigoFechamento = Convert.ToInt32(dr["CodigoFechamento"]);
            ((Modelo.Proxy.pxyFechamentoPontoFuncionario)obj).DataFechamento = Convert.ToDateTime(dr["DataFechamento"]);
            ((Modelo.Proxy.pxyFechamentoPontoFuncionario)obj).DescricaoFechamento = (dr["DescricaoFechamento"]).ToString();
            ((Modelo.Proxy.pxyFechamentoPontoFuncionario)obj).DSCodigo = (dr["DSCodigo"]).ToString();
            ((Modelo.Proxy.pxyFechamentoPontoFuncionario)obj).NomeFuncionario = (dr["NomeFuncionario"]).ToString();
        }

        public List<Modelo.Proxy.pxyFechamentoPontoFuncionario> ListaFechamentoPontoFuncionario(int tipo, List<int> idsRegistros, DateTime data)
        {
            SqlParameter[] parms;
            string sql;
            ListaFechamentoPontoFuncionario(tipo, idsRegistros, data, out parms, out sql);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.Proxy.pxyFechamentoPontoFuncionario> lista = new List<Modelo.Proxy.pxyFechamentoPontoFuncionario>();
            Modelo.Proxy.pxyFechamentoPontoFuncionario obj = new Modelo.Proxy.pxyFechamentoPontoFuncionario();
            try
            {
                //AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFechamentoPontoFuncionario>();
                //lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyFechamentoPontoFuncionario>>(dr);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        obj = new Modelo.Proxy.pxyFechamentoPontoFuncionario();
                        SetInstancePxyFechamentoPontoFuncionario(dr, obj);
                        lista.Add(obj);
                    }
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

        private static void ListaFechamentoPontoFuncionario(int tipo, List<int> idsRegistros, DateTime data, out SqlParameter[] parms, out string sql)
        {
            parms = new SqlParameter[] 
            {
                new SqlParameter("@tipo", SqlDbType.Int),
                new SqlParameter("@idsRegistros", SqlDbType.VarChar),
                new SqlParameter("@data", SqlDbType.DateTime)
            };
            parms[0].Value = tipo;
            parms[1].Value = String.Join(",", idsRegistros);
            parms[2].Value = data;

            sql = @"select *
                              from (
                            select fpf.*, 
	                               fp.codigo CodigoFechamento, 
	                               fp.datafechamento, 
	                               fp.descricao DescricaoFechamento, 
	                               f.dscodigo, 
	                               f.nome NomeFuncionario,
	                               row_number() over(partition by f.id order by datafechamento desc) ordem,
	                               Count(*) over() qtdReg
                              from fechamentopontofuncionario fpf
                             inner join fechamentoponto fp on fp.id = fpf.idfechamentoponto
                              left join funcionario f on f.id = fpf.idfuncionario
                              left join empresa e on e.id = f.idempresa
                              left join departamento d on d.id = f.iddepartamento
                              left join funcao fu on fu.id = f.idfuncao
                             where fp.dataFechamento >= @data
                               and ( 
			                            (@tipo = 0 and e.id in (select * from f_clausulain(@idsRegistros))) or
			                            (@tipo = 1 and d.id in (select * from f_clausulain(@idsRegistros))) or
			                            (@tipo = 2 and f.id in (select * from f_clausulain(@idsRegistros))) or
			                            (@tipo = 3 and fu.id in (select * from f_clausulain(@idsRegistros))) or
                                        (@tipo = 4)
		                            )
	                            ) t order by nomefuncionario, ordem desc";
        }

    }
}
