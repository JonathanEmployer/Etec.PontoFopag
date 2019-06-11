using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class HorarioDinamicoLimiteDdsr : DAL.SQL.DALBase, DAL.IHorarioDinamicoLimiteDdsr
    {

        public HorarioDinamicoLimiteDdsr(DataBase database)
        {
            db = database;
            TABELA = "HorarioDinamicoLimiteDdsr";

            SELECTPID = @"   SELECT * FROM HorarioDinamicoLimiteDdsr WHERE id = @id";

            SELECTALL = @"   SELECT   HorarioDinamicoLimiteDdsr.*
                             FROM HorarioDinamicoLimiteDdsr";

            INSERT = @"  INSERT INTO HorarioDinamicoLimiteDdsr
							(codigo, incdata, inchora, incusuario, LimitePerdaDsr,QtdHorasDsr,IdHorarioDinamico)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @LimitePerdaDsr,@QtdHorasDsr,@IdHorarioDinamico)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE HorarioDinamicoLimiteDdsr SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,LimitePerdaDsr = @LimitePerdaDsr
                           ,QtdHorasDsr = @QtdHorasDsr
                           ,IdHorarioDinamico = @IdHorarioDinamico

						WHERE id = @id";

            DELETE = @"  DELETE FROM HorarioDinamicoLimiteDdsr WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM HorarioDinamicoLimiteDdsr";

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
                obj = new Modelo.HorarioDinamicoLimiteDdsr();
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
            ((Modelo.HorarioDinamicoLimiteDdsr)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.HorarioDinamicoLimiteDdsr)obj).LimitePerdaDsr = Convert.ToString(dr["LimitePerdaDsr"]);
             ((Modelo.HorarioDinamicoLimiteDdsr)obj).QtdHorasDsr = Convert.ToString(dr["QtdHorasDsr"]);
             ((Modelo.HorarioDinamicoLimiteDdsr)obj).IdHorarioDinamico = Convert.ToInt32(dr["IdHorarioDinamico"]);

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
                ,new SqlParameter ("@LimitePerdaDsr", SqlDbType.VarChar)
                ,new SqlParameter ("@QtdHorasDsr", SqlDbType.VarChar)
                ,new SqlParameter ("@IdHorarioDinamico", SqlDbType.Int)

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
            parms[1].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).Incdata;
            parms[3].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).Inchora;
            parms[4].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).Incusuario;
            parms[5].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).Altdata;
            parms[6].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).Althora;
            parms[7].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).Altusuario;
           parms[8].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).LimitePerdaDsr;
           parms[9].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).QtdHorasDsr;
           parms[10].Value = ((Modelo.HorarioDinamicoLimiteDdsr)obj).IdHorarioDinamico;

        }

        public Modelo.HorarioDinamicoLimiteDdsr LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioDinamicoLimiteDdsr obj = new Modelo.HorarioDinamicoLimiteDdsr();
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

        public List<Modelo.HorarioDinamicoLimiteDdsr> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.HorarioDinamicoLimiteDdsr> lista = new List<Modelo.HorarioDinamicoLimiteDdsr>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioDinamicoLimiteDdsr>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioDinamicoLimiteDdsr>>(dr);
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

        public List<Modelo.HorarioDinamicoLimiteDdsr> LoadObjectByHorarioDinamico(int idHorarioDinamico)
        {
            return LoadObjectByHorarioDinamico(new List<int>() { idHorarioDinamico });
        }

        public List<Modelo.HorarioDinamicoLimiteDdsr> LoadObjectByHorarioDinamico(List<int> idsHorarioDinamico)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idHorarioDinamico", SqlDbType.VarChar)
                };
            parms[0].Value = String.Join(",", idsHorarioDinamico);

            string sql = @"select * from HorarioDinamicoLimiteDdsr where idhorariodinamico in (SELECT * FROM dbo.f_clausulaIn(@idHorarioDinamico))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.HorarioDinamicoLimiteDdsr> lista = new List<Modelo.HorarioDinamicoLimiteDdsr>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioDinamicoLimiteDdsr>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioDinamicoLimiteDdsr>>(dr);
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
