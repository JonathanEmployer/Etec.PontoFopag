using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class AlertasRepAcompanhamento : DAL.SQL.DALBase, DAL.IAlertasRepAcompanhamento
    {

        public AlertasRepAcompanhamento(DataBase database)
        {
            db = database;
            TABELA = "AlertasRepAcompanhamento";

            SELECTPID = @"   SELECT * FROM AlertasRepAcompanhamento WHERE id = @id";

            SELECTALL = @"   SELECT   AlertasRepAcompanhamento.*
                             FROM AlertasRepAcompanhamento ";

            INSERT = @"  INSERT INTO AlertasRepAcompanhamento
							(codigo, incdata, inchora, incusuario, IDAlertas,IDRep)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IDAlertas,@IDRep)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE AlertasRepAcompanhamento SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IDAlertas = @IDAlertas
                           ,IDRep = @IDRep

						WHERE id = @id";

            DELETE = @"  DELETE FROM AlertasRepAcompanhamento WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM AlertasRepAcompanhamento";

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
                obj = new Modelo.AlertasRepAcompanhamento();
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
            ((Modelo.AlertasRepAcompanhamento)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.AlertasRepAcompanhamento)obj).IDAlertas = Convert.ToInt32(dr["IDAlertas"]);
             ((Modelo.AlertasRepAcompanhamento)obj).IdRep = Convert.ToInt32(dr["IdRep"]);

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
                ,new SqlParameter ("@IDRep", SqlDbType.Int)

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
            parms[1].Value = ((Modelo.AlertasRepAcompanhamento)obj).Codigo;
            parms[2].Value = ((Modelo.AlertasRepAcompanhamento)obj).Incdata;
            parms[3].Value = ((Modelo.AlertasRepAcompanhamento)obj).Inchora;
            parms[4].Value = ((Modelo.AlertasRepAcompanhamento)obj).Incusuario;
            parms[5].Value = ((Modelo.AlertasRepAcompanhamento)obj).Altdata;
            parms[6].Value = ((Modelo.AlertasRepAcompanhamento)obj).Althora;
            parms[7].Value = ((Modelo.AlertasRepAcompanhamento)obj).Altusuario;
            parms[8].Value = ((Modelo.AlertasRepAcompanhamento)obj).IDAlertas;
            parms[9].Value = ((Modelo.AlertasRepAcompanhamento)obj).IdRep;
        }

        public Modelo.AlertasRepAcompanhamento LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.AlertasRepAcompanhamento obj = new Modelo.AlertasRepAcompanhamento();
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

        public List<Modelo.AlertasRepAcompanhamento> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.AlertasRepAcompanhamento> lista = new List<Modelo.AlertasRepAcompanhamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AlertasRepAcompanhamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AlertasRepAcompanhamento>>(dr);
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

        public void IncluirLoteIdsRep(SqlTransaction trans, int idAlerta, List<int> idsReps)
        {
            if (idAlerta > 0 && idsReps.Count() > 0)
            {
                SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@idsReps", SqlDbType.VarChar),
                    new SqlParameter("@idAlerta", SqlDbType.Int)
                };

                parms[0].Value = String.Join(",", idsReps);
                parms[1].Value = idAlerta;

                string sql = @"INSERT INTO dbo.AlertasRepAcompanhamento ( IDAlertas, IDRep )
                           SELECT @idAlerta, Id FROM dbo.rep WHERE id IN (SELECT * FROM dbo.F_ClausulaIn(@idsReps))";
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

        public void ExcluirLoteIdsRep(SqlTransaction trans, int idAlerta, List<int> idsReps)
        {
            if (idAlerta > 0 && idsReps.Count() > 0)
            {
                SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@idsReps", SqlDbType.VarChar),
                    new SqlParameter("@idAlertas", SqlDbType.Int)
                };

                parms[0].Value = String.Join(",", idsReps);
                parms[1].Value = idAlerta;

                try
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"DELETE FROM AlertasRepAcompanhamento WHERE idrep in (SELECT * FROM dbo.F_ClausulaIn(@idsReps)) and idAlertas = @idAlertas", true, parms);
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
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"DELETE FROM AlertasRepAcompanhamento WHERE idAlertas = @idAlertas", true, parms);
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

        public List<Modelo.AlertasRepAcompanhamento> GetAllPorAlerta(Int32 idAlerta)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idAlerta", SqlDbType.Int)
            };

            parms[0].Value = idAlerta;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM AlertasRepAcompanhamento WHERE IDAlertas = @idAlerta", parms);

            List<Modelo.AlertasRepAcompanhamento> lista = new List<Modelo.AlertasRepAcompanhamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AlertasRepAcompanhamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AlertasRepAcompanhamento>>(dr);
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
