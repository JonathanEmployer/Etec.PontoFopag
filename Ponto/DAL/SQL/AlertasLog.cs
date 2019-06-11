using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class AlertasLog : DAL.SQL.DALBase, DAL.IAlertasLog
    {

        public AlertasLog(DataBase database)
        {
            db = database;
            TABELA = "AlertasLog";

            SELECTPID = @"   SELECT * FROM AlertasLog WHERE id = @id";

            SELECTALL = @"   SELECT   AlertasLog.*
                             FROM AlertasLog";

            INSERT = @"  INSERT INTO AlertasLog
							(codigo, incdata, inchora, incusuario, IdAlerta,Comando,Log,Complemento,Status)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdAlerta,@Comando,@Log,@Complemento,@Status)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE AlertasLog SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdAlerta = @IdAlerta
                           ,Comando = @Comando
                           ,Log = @Log
                           ,Complemento = @Complemento
                           ,Status = @Status

						WHERE id = @id";

            DELETE = @"  DELETE FROM AlertasLog WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM AlertasLog";

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
                obj = new Modelo.AlertasLog();
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
            ((Modelo.AlertasLog)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.AlertasLog)obj).IdAlerta = Convert.ToInt32(dr["IdAlerta"]);
            ((Modelo.AlertasLog)obj).Comando = Convert.ToString(dr["Comando"]);
            ((Modelo.AlertasLog)obj).Log = Convert.ToString(dr["Log"]);
            ((Modelo.AlertasLog)obj).Complemento = Convert.ToString(dr["Complemento"]);
            ((Modelo.AlertasLog)obj).Status = Convert.ToInt32(dr["Status"]);

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
                ,new SqlParameter ("@IdAlerta", SqlDbType.Int)
                ,new SqlParameter ("@Comando", SqlDbType.VarChar)
                ,new SqlParameter ("@Log", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.AlertasLog)obj).Codigo;
            parms[2].Value = ((Modelo.AlertasLog)obj).Incdata;
            parms[3].Value = ((Modelo.AlertasLog)obj).Inchora;
            parms[4].Value = ((Modelo.AlertasLog)obj).Incusuario;
            parms[5].Value = ((Modelo.AlertasLog)obj).Altdata;
            parms[6].Value = ((Modelo.AlertasLog)obj).Althora;
            parms[7].Value = ((Modelo.AlertasLog)obj).Altusuario;
            parms[8].Value = ((Modelo.AlertasLog)obj).IdAlerta;
            parms[9].Value = ((Modelo.AlertasLog)obj).Comando;
            parms[10].Value = ((Modelo.AlertasLog)obj).Log;
            parms[11].Value = ((Modelo.AlertasLog)obj).Complemento;
            parms[12].Value = ((Modelo.AlertasLog)obj).Status;

        }

        public Modelo.AlertasLog LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.AlertasLog obj = new Modelo.AlertasLog();
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

        public List<Modelo.AlertasLog> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.AlertasLog> lista = new List<Modelo.AlertasLog>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AlertasLog>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AlertasLog>>(dr);
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

        public List<Modelo.AlertasLog> GetAllListByAlerta(Int32 idAlerta)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idAlerta", SqlDbType.Int)
            };

            parms[0].Value = idAlerta;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL + " where AlertasLog.IdAlerta = @idAlerta", parms);

            List<Modelo.AlertasLog> lista = new List<Modelo.AlertasLog>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AlertasLog>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AlertasLog>>(dr);
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

        public void ExcluirLogPorAlerta(SqlTransaction trans, int idAlerta)
        {
            SqlParameter[] parms = new SqlParameter[1]
            { 
                    new SqlParameter("@idAlerta", SqlDbType.Int)
            };
            parms[0].Value = idAlerta;

            string aux = @" DELETE FROM dbo.AlertasLog WHERE IdAlerta = @idAlerta ";

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }
    }
}
