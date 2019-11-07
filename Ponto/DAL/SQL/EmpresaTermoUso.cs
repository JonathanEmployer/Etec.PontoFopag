using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class EmpresaTermoUso : DAL.SQL.DALBase, DAL.IEmpresaTermoUso
    {

        public EmpresaTermoUso(DataBase database)
        {
            db = database;
            TABELA = "EmpresaTermoUso";

            SELECTPID = @"   SELECT * FROM EmpresaTermoUso WHERE id = @id";

            SELECTALL = @"   SELECT   EmpresaTermoUso.*
                             FROM EmpresaTermoUso";

            INSERT = @"  INSERT INTO EmpresaTermoUso
							(codigo, incdata, inchora, incusuario, Tipo,IdEmpresa,TermoAceito,DataAceite,IdUsuario,UtilizaReconhecimentoFacial)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Tipo,@IdEmpresa,@TermoAceito,@DataAceite,@IdUsuario,@UtilizaReconhecimentoFacial)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE EmpresaTermoUso SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,Tipo = @Tipo
                           ,IdEmpresa = @IdEmpresa
                           ,TermoAceito = @TermoAceito
                           ,DataAceite = @DataAceite
                           ,IdUsuario = @IdUsuario
                           ,UtilizaReconhecimentoFacial = @UtilizaReconhecimentoFacial

						WHERE id = @id";

            DELETE = @"  DELETE FROM EmpresaTermoUso WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM EmpresaTermoUso";

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
                obj = new Modelo.EmpresaTermoUso();
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
            ((Modelo.EmpresaTermoUso)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.EmpresaTermoUso)obj).Tipo = Convert.ToInt32(dr["Tipo"]);
             ((Modelo.EmpresaTermoUso)obj).IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
             ((Modelo.EmpresaTermoUso)obj).TermoAceito = Convert.ToString(dr["TermoAceito"]);
             ((Modelo.EmpresaTermoUso)obj).DataAceite = Convert.ToDateTime(dr["DataAceite"]);
             ((Modelo.EmpresaTermoUso)obj).IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
             ((Modelo.EmpresaTermoUso)obj).UtilizaReconhecimentoFacial = Convert.ToBoolean(dr["UtilizaReconhecimentoFacial"]);

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
                ,new SqlParameter ("@Tipo", SqlDbType.Int)
                ,new SqlParameter ("@IdEmpresa", SqlDbType.Int)
                ,new SqlParameter ("@TermoAceito", SqlDbType.VarChar)
                ,new SqlParameter ("@DataAceite", SqlDbType.DateTime)
                ,new SqlParameter ("@IdUsuario", SqlDbType.Int)
                ,new SqlParameter ("@UtilizaReconhecimentoFacial", SqlDbType.Bit)

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
            parms[1].Value = ((Modelo.EmpresaTermoUso)obj).Codigo;
            parms[2].Value = ((Modelo.EmpresaTermoUso)obj).Incdata;
            parms[3].Value = ((Modelo.EmpresaTermoUso)obj).Inchora;
            parms[4].Value = ((Modelo.EmpresaTermoUso)obj).Incusuario;
            parms[5].Value = ((Modelo.EmpresaTermoUso)obj).Altdata;
            parms[6].Value = ((Modelo.EmpresaTermoUso)obj).Althora;
            parms[7].Value = ((Modelo.EmpresaTermoUso)obj).Altusuario;
           parms[8].Value = ((Modelo.EmpresaTermoUso)obj).Tipo;
           parms[9].Value = ((Modelo.EmpresaTermoUso)obj).IdEmpresa;
           parms[10].Value = ((Modelo.EmpresaTermoUso)obj).TermoAceito;
           parms[11].Value = ((Modelo.EmpresaTermoUso)obj).DataAceite;
           parms[12].Value = ((Modelo.EmpresaTermoUso)obj).IdUsuario;
           parms[13].Value = ((Modelo.EmpresaTermoUso)obj).UtilizaReconhecimentoFacial;

        }

        public Modelo.EmpresaTermoUso LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.EmpresaTermoUso obj = new Modelo.EmpresaTermoUso();
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

        public List<Modelo.EmpresaTermoUso> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.EmpresaTermoUso> lista = new List<Modelo.EmpresaTermoUso>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EmpresaTermoUso>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EmpresaTermoUso>>(dr);
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

        public List<Modelo.EmpresaTermoUso> LoadObjectsByIdsEmpresa(List<int> idsEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idsEmpresa", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsEmpresa);

            string aux = "select * from EmpresaTermoUso where idempresa in (select * from dbo.F_ClausulaIn(@idsEmpresa))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.EmpresaTermoUso> lista = new List<Modelo.EmpresaTermoUso>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EmpresaTermoUso>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EmpresaTermoUso>>(dr);
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

        public void DeleteByIdsEmpresas(List<int> idsEmpresas)
        {
            SqlParameter[] parms = { new SqlParameter("@idsEmpresas", SqlDbType.VarChar) };
            parms[0].Value = String.Join(",", idsEmpresas);
            string delete = @"delete from EmpresaTermoUso where idempresa in (select * from F_ClausulaIn(@idsEmpresas))";

            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, delete, true, parms);
        }
    }
}
