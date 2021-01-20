using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class ParametrosInternos : DAL.SQL.DALBase, DAL.IParametrosInternos
    {

        public ParametrosInternos(DataBase database)
        {
            db = database;
            TABELA = "ParametrosInternos";

            SELECTPID = @"   SELECT * FROM ParametrosInternos WHERE id = @id";

            SELECTALL = @"   SELECT   *
                             FROM ParametrosInternos";

            INSERT = @"  INSERT INTO ParametrosInternos
							(codigo, incdata, inchora, incusuario, ServicoCalculo)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @ServicoCalculo) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE funcao SET
							  codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , ServicoCalculo = @ServicoCalculo
						WHERE id = @id";

            DELETE = @"  DELETE FROM ServicoCalculo WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM ServicoCalculo";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.ParametrosInternos)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.ParametrosInternos)obj).ServicoCalculo = Convert.ToInt32(dr["ServicoCalculo"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.ParametrosInternos();
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
                new SqlParameter ("@ParametrosInternos", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.ParametrosInternos)obj).Codigo;
            parms[2].Value = ((Modelo.ParametrosInternos)obj).Incdata;
            parms[3].Value = ((Modelo.ParametrosInternos)obj).Inchora;
            parms[4].Value = ((Modelo.ParametrosInternos)obj).Incusuario;
            parms[5].Value = ((Modelo.ParametrosInternos)obj).Altdata;
            parms[6].Value = ((Modelo.ParametrosInternos)obj).Althora;
            parms[7].Value = ((Modelo.ParametrosInternos)obj).Altusuario;
            parms[8].Value = ((Modelo.ParametrosInternos)obj).ServicoCalculo;
        }

        public Modelo.ParametrosInternos LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ParametrosInternos objParametrosInternos = new Modelo.ParametrosInternos();
            try
            {
                SetInstance(dr, objParametrosInternos);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametrosInternos;
        }

        public List<Modelo.ParametrosInternos> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string cmd = " SELECT * " +
                            " FROM ParametrosInternos";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.ParametrosInternos> lista = new List<Modelo.ParametrosInternos>();
            try
            {
                while (dr.Read())
                {
                    Modelo.ParametrosInternos objParametrosInternos = new Modelo.ParametrosInternos();
                    SetInstance(dr, objParametrosInternos);
                    lista.Add(objParametrosInternos);
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

        public Modelo.ParametrosInternos LoadFirtObject()
        {
            SqlParameter[] parms = new SqlParameter[] {};
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "select top 1 * from ParametrosInternos ", parms);

            Modelo.ParametrosInternos objParametrosInternos = new Modelo.ParametrosInternos();
            try
            {
                SetInstance(dr, objParametrosInternos);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametrosInternos;
        }

        #endregion
    }
}
