using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class AlertasFuncionario : DAL.SQL.DALBase, DAL.IAlertasFuncionario
    {

        public AlertasFuncionario(DataBase database)
        {
            db = database;
            TABELA = "AlertasFuncionario";

            SELECTPID = @"   SELECT * FROM AlertasFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   AlertasFuncionario.*
                             FROM AlertasFuncionario";

            INSERT = @"  INSERT INTO AlertasFuncionario
							(codigo, incdata, inchora, incusuario, IDAlertas,IDFuncionario)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IDAlertas,@IDFuncionario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE AlertasFuncionario SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IDAlertas = @IDAlertas
                           ,IDFuncionario = @IDFuncionario

						WHERE id = @id";

            DELETE = @"  DELETE FROM AlertasFuncionario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM AlertasFuncionario";

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
                obj = new Modelo.AlertasFuncionario();
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
            ((Modelo.AlertasFuncionario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.AlertasFuncionario)obj).IDAlertas = Convert.ToInt32(dr["IDAlertas"]);
             ((Modelo.AlertasFuncionario)obj).IDFuncionario = Convert.ToInt32(dr["IDFuncionario"]);

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
                ,new SqlParameter ("@IDAlertas", SqlDbType.Int)
                ,new SqlParameter ("@IDFuncionario", SqlDbType.Int)

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
            parms[1].Value = ((Modelo.AlertasFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.AlertasFuncionario)obj).Incdata;
            parms[3].Value = ((Modelo.AlertasFuncionario)obj).Inchora;
            parms[4].Value = ((Modelo.AlertasFuncionario)obj).Incusuario;
            parms[5].Value = ((Modelo.AlertasFuncionario)obj).Altdata;
            parms[6].Value = ((Modelo.AlertasFuncionario)obj).Althora;
            parms[7].Value = ((Modelo.AlertasFuncionario)obj).Altusuario;
           parms[8].Value = ((Modelo.AlertasFuncionario)obj).IDAlertas;
           parms[9].Value = ((Modelo.AlertasFuncionario)obj).IDFuncionario;

        }

        public Modelo.AlertasFuncionario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.AlertasFuncionario obj = new Modelo.AlertasFuncionario();
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

        public List<Modelo.AlertasFuncionario> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.AlertasFuncionario> lista = new List<Modelo.AlertasFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AlertasFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AlertasFuncionario>>(dr);
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

        public void IncluirLoteIdsFuncionario(SqlTransaction trans, int idAlerta, List<int> idsFuncs)
        {
            if (idAlerta > 0 && idsFuncs.Count() > 0)
            {
                SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@idsFuncs", SqlDbType.VarChar),
                    new SqlParameter("@idAlerta", SqlDbType.Int)
                };

                parms[0].Value = String.Join(",", idsFuncs);
                parms[1].Value = idAlerta;

                string sql = @"INSERT INTO dbo.AlertasFuncionario ( IDAlertas, IDFuncionario )
                           SELECT @idAlerta, Id FROM dbo.funcionario WHERE id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs))";
                try
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, sql, true, parms);
                }
                catch (SqlException ex)
                {
                    List<string> erros = new List<string>();
                    foreach (SqlError error in ex.Errors)
                    {
                        erros.Add(error.Message);
                    }
                    throw new Exception(String.Join("; ", erros));
                }
            }
        }

        public void ExcluirLoteIdsFuncionario(SqlTransaction trans, int idAlerta, List<int> idsFuncs)
        {
            if (idAlerta > 0 && idsFuncs.Count() > 0)
            {
                SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@idsFuncs", SqlDbType.VarChar),
                    new SqlParameter("@idAlertas", SqlDbType.Int)
                };

                parms[0].Value = String.Join(",", idsFuncs);
                parms[1].Value = idAlerta;

                try
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"DELETE FROM AlertasFuncionario WHERE idFuncionario in (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) and idAlertas = @idAlertas", true, parms);
                }
                catch (SqlException ex)
                {
                    List<string> erros = new List<string>();
                    foreach (SqlError error in ex.Errors)
                    {
                        erros.Add(error.Message);
                    }
                    throw new Exception(String.Join("; ", erros));
                }
            }
            
        }

        public void ExcluirLoteIdAlerta(SqlTransaction trans, int idAlerta)
        {
            if (idAlerta > 0)
            {
                SqlParameter[] parms = new SqlParameter[1]
                {
                    new SqlParameter("@idAlertas", SqlDbType.Int)
                };

                parms[0].Value = idAlerta;

                try
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"DELETE FROM AlertasFuncionario WHERE idAlertas = @idAlertas", true, parms);
                }
                catch (SqlException ex)
                {
                    List<string> erros = new List<string>();
                    foreach (SqlError error in ex.Errors)
                    {
                        erros.Add(error.Message);
                    }
                    throw new Exception(String.Join("; ", erros));
                }
            }
        }

        public List<Modelo.AlertasFuncionario> GetAllPorAlerta(Int32 idAlerta)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idAlerta", SqlDbType.Int)
            };

            parms[0].Value = idAlerta;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM AlertasFuncionario WHERE IDAlertas = @idAlerta", parms);

            List<Modelo.AlertasFuncionario> lista = new List<Modelo.AlertasFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AlertasFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AlertasFuncionario>>(dr);
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
    }
}
