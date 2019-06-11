using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class ParametroPainelRH : DAL.SQL.DALBase, DAL.IParametroPainelRH
    {

        public ParametroPainelRH(DataBase database)
        {
            db = database;
            TABELA = "ParametroPainelRH";

            SELECTPID = @"   SELECT * FROM ParametroPainelRH WHERE id = @id";

            SELECTALL = @"   SELECT   ParametroPainelRH.*
                             FROM ParametroPainelRH";

            INSERT = @"  INSERT INTO ParametroPainelRH
							(codigo, incdata, inchora, incusuario, IntegraPainel, UsuarioAPIPainel, SenhaAPIPainel, PermiteAprovarMarcacaoImpar, CS)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IntegraPainel, @UsuarioAPIPainel, @SenhaAPIPainel, @PermiteAprovarMarcacaoImpar, @CS)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE ParametroPainelRH SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IntegraPainel = @IntegraPainel
                           ,UsuarioAPIPainel = @UsuarioAPIPainel
                           ,SenhaAPIPainel = @SenhaAPIPainel
                           ,PermiteAprovarMarcacaoImpar = @PermiteAprovarMarcacaoImpar
                           ,CS = @CS

						WHERE id = @id";

            DELETE = @"  DELETE FROM ParametroPainelRH WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM ParametroPainelRH";

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
                obj = new Modelo.ParametroPainelRH();
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
            ((Modelo.ParametroPainelRH)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.ParametroPainelRH)obj).IntegraPainel = Convert.ToBoolean(dr["IntegraPainel"]);
             ((Modelo.ParametroPainelRH)obj).UsuarioAPIPainel = Convert.ToString(dr["UsuarioAPIPainel"]);
             ((Modelo.ParametroPainelRH)obj).SenhaAPIPainel = Convert.ToString(dr["SenhaAPIPainel"]);
             ((Modelo.ParametroPainelRH)obj).PermiteAprovarMarcacaoImpar = Convert.ToBoolean(dr["PermiteAprovarMarcacaoImpar"]);
             ((Modelo.ParametroPainelRH)obj).CS = Convert.ToString(dr["CS"]);

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
                ,new SqlParameter ("@IntegraPainel", SqlDbType.Bit)
                ,new SqlParameter ("@UsuarioAPIPainel", SqlDbType.VarChar)
                ,new SqlParameter ("@SenhaAPIPainel", SqlDbType.VarChar)
                ,new SqlParameter ("@PermiteAprovarMarcacaoImpar", SqlDbType.Bit)
                ,new SqlParameter ("CS", SqlDbType.VarChar)

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
            parms[1].Value = ((Modelo.ParametroPainelRH)obj).Codigo;
            parms[2].Value = ((Modelo.ParametroPainelRH)obj).Incdata;
            parms[3].Value = ((Modelo.ParametroPainelRH)obj).Inchora;
            parms[4].Value = ((Modelo.ParametroPainelRH)obj).Incusuario;
            parms[5].Value = ((Modelo.ParametroPainelRH)obj).Altdata;
            parms[6].Value = ((Modelo.ParametroPainelRH)obj).Althora;
            parms[7].Value = ((Modelo.ParametroPainelRH)obj).Altusuario;
            parms[8].Value = ((Modelo.ParametroPainelRH)obj).IntegraPainel;
            parms[9].Value = ((Modelo.ParametroPainelRH)obj).UsuarioAPIPainel;
            parms[10].Value = ((Modelo.ParametroPainelRH)obj).SenhaAPIPainel;
            parms[11].Value = ((Modelo.ParametroPainelRH)obj).PermiteAprovarMarcacaoImpar;
            parms[12].Value = ((Modelo.ParametroPainelRH)obj).CS;

        }

        public Modelo.ParametroPainelRH LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ParametroPainelRH obj = new Modelo.ParametroPainelRH();
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

        public List<Modelo.ParametroPainelRH> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.ParametroPainelRH> lista = new List<Modelo.ParametroPainelRH>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ParametroPainelRH>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ParametroPainelRH>>(dr);
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
