using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class CamposSelecionadosRelCartaoPonto : DAL.SQL.DALBase, DAL.ICamposSelecionadosRelCartaoPonto
    {

        public CamposSelecionadosRelCartaoPonto(DataBase database)
        {
            db = database;
            TABELA = "CamposSelecionadosRelCartaoPonto";

            SELECTPID = @"   SELECT * FROM CamposSelecionadosRelCartaoPonto WHERE id = @id";

            SELECTALL = @"   SELECT   CamposSelecionadosRelCartaoPonto.*
                             FROM CamposSelecionadosRelCartaoPonto";

            INSERT = @"  INSERT INTO CamposSelecionadosRelCartaoPonto
							(codigo, incdata, inchora, incusuario, Posicao,PropriedadeModelo)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Posicao,@PropriedadeModelo)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE CamposSelecionadosRelCartaoPonto SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,Posicao = @Posicao
                           ,PropriedadeModelo = @PropriedadeModelo

						WHERE id = @id";

            DELETE = @"  DELETE FROM CamposSelecionadosRelCartaoPonto WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM CamposSelecionadosRelCartaoPonto";

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
                obj = new Modelo.CamposSelecionadosRelCartaoPonto();
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
            ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Posicao = Convert.ToInt16(dr["Posicao"]);
             ((Modelo.CamposSelecionadosRelCartaoPonto)obj).PropriedadeModelo = Convert.ToString(dr["PropriedadeModelo"]);

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
                ,new SqlParameter ("@Posicao", SqlDbType.SmallInt)
                ,new SqlParameter ("@PropriedadeModelo", SqlDbType.VarChar)

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
            parms[1].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Codigo;
            parms[2].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Incdata;
            parms[3].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Inchora;
            parms[4].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Incusuario;
            parms[5].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Altdata;
            parms[6].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Althora;
            parms[7].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Altusuario;
           parms[8].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).Posicao;
           parms[9].Value = ((Modelo.CamposSelecionadosRelCartaoPonto)obj).PropriedadeModelo;

        }

        public Modelo.CamposSelecionadosRelCartaoPonto LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.CamposSelecionadosRelCartaoPonto obj = new Modelo.CamposSelecionadosRelCartaoPonto();
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

        public List<Modelo.CamposSelecionadosRelCartaoPonto> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.CamposSelecionadosRelCartaoPonto> lista = new List<Modelo.CamposSelecionadosRelCartaoPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.CamposSelecionadosRelCartaoPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.CamposSelecionadosRelCartaoPonto>>(dr);
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
