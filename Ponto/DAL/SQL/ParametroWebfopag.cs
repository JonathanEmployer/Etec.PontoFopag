using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class ParametroWebfopag : DAL.SQL.DALBase, DAL.IParametroWebfopag
    {

        public ParametroWebfopag(DataBase database)
        {
            db = database;
            TABELA = "ParametroWebfopag";

            SELECTPID = @"   SELECT * FROM ParametroWebfopag WHERE id = @id";

            SELECTALL = @"   SELECT   ParametroWebfopag.*
                             FROM ParametroWebfopag";

            INSERT = @"  INSERT INTO ParametroWebfopag
							(codigo, incdata, inchora, incusuario, UtilizaColetorTipoMonsanto,UsuarioApiV1,SenhaApiV1,TokenApiV1,CS,UltimaColeta)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @UtilizaColetorTipoMonsanto,@UsuarioApiV1,@SenhaApiV1,@TokenApiV1,@CS,@UltimaColeta)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE ParametroWebfopag SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,UtilizaColetorTipoMonsanto = @UtilizaColetorTipoMonsanto
                           ,UsuarioApiV1 = @UsuarioApiV1
                           ,SenhaApiV1 = @SenhaApiV1
                           ,TokenApiV1 = @TokenApiV1
                           ,CS = @CS
                           ,UltimaColeta = @UltimaColeta

						WHERE id = @id";

            DELETE = @"  DELETE FROM ParametroWebfopag WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM ParametroWebfopag";

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
                obj = new Modelo.ParametroWebfopag();
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
            ((Modelo.ParametroWebfopag)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.ParametroWebfopag)obj).UtilizaColetorTipoMonsanto = Convert.ToBoolean(dr["UtilizaColetorTipoMonsanto"]);
             ((Modelo.ParametroWebfopag)obj).UsuarioApiV1 = Convert.ToString(dr["UsuarioApiV1"]);
             ((Modelo.ParametroWebfopag)obj).SenhaApiV1 = Convert.ToString(dr["SenhaApiV1"]);
             ((Modelo.ParametroWebfopag)obj).TokenApiV1 = Convert.ToString(dr["TokenApiV1"]);
             ((Modelo.ParametroWebfopag)obj).CS = Convert.ToString(dr["CS"]);
             ((Modelo.ParametroWebfopag)obj).UltimaColeta = Convert.ToDateTime(dr["UltimaColeta"]);

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
                ,new SqlParameter ("@UtilizaColetorTipoMonsanto", SqlDbType.Bit)
                ,new SqlParameter ("@UsuarioApiV1", SqlDbType.VarChar)
                ,new SqlParameter ("@SenhaApiV1", SqlDbType.VarChar)
                ,new SqlParameter ("@TokenApiV1", SqlDbType.VarChar)
                ,new SqlParameter ("@CS", SqlDbType.VarChar)
                ,new SqlParameter ("@UltimaColeta", SqlDbType.DateTime)

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
            parms[1].Value = ((Modelo.ParametroWebfopag)obj).Codigo;
            parms[2].Value = ((Modelo.ParametroWebfopag)obj).Incdata;
            parms[3].Value = ((Modelo.ParametroWebfopag)obj).Inchora;
            parms[4].Value = ((Modelo.ParametroWebfopag)obj).Incusuario;
            parms[5].Value = ((Modelo.ParametroWebfopag)obj).Altdata;
            parms[6].Value = ((Modelo.ParametroWebfopag)obj).Althora;
            parms[7].Value = ((Modelo.ParametroWebfopag)obj).Altusuario;
           parms[8].Value = ((Modelo.ParametroWebfopag)obj).UtilizaColetorTipoMonsanto;
           parms[9].Value = ((Modelo.ParametroWebfopag)obj).UsuarioApiV1;
           parms[10].Value = ((Modelo.ParametroWebfopag)obj).SenhaApiV1;
           parms[11].Value = ((Modelo.ParametroWebfopag)obj).TokenApiV1;
           parms[12].Value = ((Modelo.ParametroWebfopag)obj).CS;
           parms[13].Value = ((Modelo.ParametroWebfopag)obj).UltimaColeta;

        }

        public Modelo.ParametroWebfopag LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ParametroWebfopag obj = new Modelo.ParametroWebfopag();
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

        public List<Modelo.ParametroWebfopag> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.ParametroWebfopag> lista = new List<Modelo.ParametroWebfopag>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ParametroWebfopag>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ParametroWebfopag>>(dr);
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
