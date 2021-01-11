using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class JornadaSubstituirFuncionario : DAL.SQL.DALBase, DAL.IJornadaSubstituirFuncionario
    {

        public JornadaSubstituirFuncionario(DataBase database)
        {
            db = database;
            TABELA = "JornadaSubstituirFuncionario";

            SELECTPID = @"   SELECT * FROM JornadaSubstituirFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   JornadaSubstituirFuncionario.*
                               FROM JornadaSubstituirFuncionario
                              WHERE 1 = 1 ";

            INSERT = @"  INSERT INTO JornadaSubstituirFuncionario
							(codigo, incdata, inchora, incusuario, IdJornadaSubstituir,IdFuncionario)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdJornadaSubstituir,@IdFuncionario)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE JornadaSubstituirFuncionario SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdJornadaSubstituir = @IdJornadaSubstituir
                           ,IdFuncionario = @IdFuncionario

						WHERE id = @id";

            DELETE = @"  DELETE FROM JornadaSubstituirFuncionario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM JornadaSubstituirFuncionario";

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
                obj = new Modelo.JornadaSubstituirFuncionario();
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
            ((Modelo.JornadaSubstituirFuncionario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.JornadaSubstituirFuncionario)obj).IdJornadaSubstituir = Convert.ToInt32(dr["IdJornadaSubstituir"]);
             ((Modelo.JornadaSubstituirFuncionario)obj).IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]);

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
                ,new SqlParameter ("@IdJornadaSubstituir", SqlDbType.Int)
                ,new SqlParameter ("@IdFuncionario", SqlDbType.Int)

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
            parms[1].Value = ((Modelo.JornadaSubstituirFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.JornadaSubstituirFuncionario)obj).Incdata;
            parms[3].Value = ((Modelo.JornadaSubstituirFuncionario)obj).Inchora;
            parms[4].Value = ((Modelo.JornadaSubstituirFuncionario)obj).Incusuario;
            parms[5].Value = ((Modelo.JornadaSubstituirFuncionario)obj).Altdata;
            parms[6].Value = ((Modelo.JornadaSubstituirFuncionario)obj).Althora;
            parms[7].Value = ((Modelo.JornadaSubstituirFuncionario)obj).Altusuario;
           parms[8].Value = ((Modelo.JornadaSubstituirFuncionario)obj).IdJornadaSubstituir;
           parms[9].Value = ((Modelo.JornadaSubstituirFuncionario)obj).IdFuncionario;

        }

        public Modelo.JornadaSubstituirFuncionario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.JornadaSubstituirFuncionario obj = new Modelo.JornadaSubstituirFuncionario();
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

        public List<Modelo.JornadaSubstituirFuncionario> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.JornadaSubstituirFuncionario> lista = new List<Modelo.JornadaSubstituirFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.JornadaSubstituirFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.JornadaSubstituirFuncionario>>(dr);
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

        public List<Modelo.JornadaSubstituirFuncionario> GetByIdJornadaSubstituir(int idJornadaSubstituir)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@IdJornadaSubstituir", idJornadaSubstituir)
            };

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL + " AND idJornadaSubstituir = @idJornadaSubstituir ", parms);

            List<Modelo.JornadaSubstituirFuncionario> lista = new List<Modelo.JornadaSubstituirFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.JornadaSubstituirFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.JornadaSubstituirFuncionario>>(dr);
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
