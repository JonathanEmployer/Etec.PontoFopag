using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class AlertasDisponiveis : DAL.SQL.DALBase, DAL.IAlertasDisponiveis
    {

        public AlertasDisponiveis(DataBase database)
        {
            db = database;
            TABELA = "AlertasDisponiveis";

            SELECTPID = @"   SELECT * FROM AlertasDisponiveis WHERE id = @id";

            SELECTALL = @"   SELECT   AlertasDisponiveis.*
                             FROM AlertasDisponiveis";

            INSERT = @"  INSERT INTO AlertasDisponiveis
							(codigo, incdata, inchora, incusuario, Nome,Descricao,NomeProcedure)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Nome,@Descricao,@NomeProcedure)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE AlertasDisponiveis SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,Nome = @Nome
                           ,Descricao = @Descricao
                           ,NomeProcedure = @NomeProcedure

						WHERE id = @id";

            DELETE = @"  DELETE FROM AlertasDisponiveis WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM AlertasDisponiveis";

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
                obj = new Modelo.AlertasDisponiveis();
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
            ((Modelo.AlertasDisponiveis)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.AlertasDisponiveis)obj).Nome = Convert.ToString(dr["Nome"]);
             ((Modelo.AlertasDisponiveis)obj).Descricao = Convert.ToString(dr["Descricao"]);
             ((Modelo.AlertasDisponiveis)obj).NomeProcedure = Convert.ToString(dr["NomeProcedure"]);

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
                ,new SqlParameter ("@Nome", SqlDbType.VarChar)
                ,new SqlParameter ("@Descricao", SqlDbType.VarChar)
                ,new SqlParameter ("@NomeProcedure", SqlDbType.VarChar)

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
            parms[1].Value = ((Modelo.AlertasDisponiveis)obj).Codigo;
            parms[2].Value = ((Modelo.AlertasDisponiveis)obj).Incdata;
            parms[3].Value = ((Modelo.AlertasDisponiveis)obj).Inchora;
            parms[4].Value = ((Modelo.AlertasDisponiveis)obj).Incusuario;
            parms[5].Value = ((Modelo.AlertasDisponiveis)obj).Altdata;
            parms[6].Value = ((Modelo.AlertasDisponiveis)obj).Althora;
            parms[7].Value = ((Modelo.AlertasDisponiveis)obj).Altusuario;
           parms[8].Value = ((Modelo.AlertasDisponiveis)obj).Nome;
           parms[9].Value = ((Modelo.AlertasDisponiveis)obj).Descricao;
           parms[10].Value = ((Modelo.AlertasDisponiveis)obj).NomeProcedure;

        }

        public Modelo.AlertasDisponiveis LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.AlertasDisponiveis obj = new Modelo.AlertasDisponiveis();
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

        public List<Modelo.AlertasDisponiveis> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.AlertasDisponiveis> lista = new List<Modelo.AlertasDisponiveis>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.AlertasDisponiveis>();
                lista = AutoMapper.Mapper.Map<List<Modelo.AlertasDisponiveis>>(dr);
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
