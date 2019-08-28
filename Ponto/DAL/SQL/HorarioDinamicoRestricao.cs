using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class HorarioDinamicoRestricao : DAL.SQL.DALBase, DAL.IHorarioDinamicoRestricao
    {

        public HorarioDinamicoRestricao(DataBase database)
        {
            db = database;
            TABELA = "HorarioDinamicoRestricao";

            SELECTPID = @" SELECT * FROM HorarioDinamicoRestricao WHERE id = @id ";

            SELECTALL = @" SELECT hr.*,
	                               Convert(VARCHAR(20), e.codigo)+' | '+e.nome DescEmpresa,
	                               Convert(VARCHAR(20), c.codigo)+' | '+c.codigocontrato+' - ' +c.descricaocontrato DescContrato
                              FROM HorarioDinamicoRestricao hr
                              LEFT JOIN empresa e on hr.IdEmpresa = e.id
                              LEFT JOIN contrato c on hr.IdContrato = c.id
                             where 1 = 1 ";

            INSERT = @"  INSERT INTO HorarioDinamicoRestricao
							(codigo, incdata, inchora, incusuario, IdHorarioDinamico,IdEmpresa,IdContrato)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdHorarioDinamico,@IdEmpresa,@IdContrato)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE HorarioDinamicoRestricao SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdHorarioDinamico = @IdHorarioDinamico
                           ,IdEmpresa = @IdEmpresa
                           ,IdContrato = @IdContrato

						WHERE id = @id";

            DELETE = @"  DELETE FROM HorarioDinamicoRestricao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM HorarioDinamicoRestricao";

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
                obj = new Modelo.HorarioDinamicoRestricao();
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
            ((Modelo.HorarioDinamicoRestricao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.HorarioDinamicoRestricao)obj).IdHorarioDinamico = Convert.ToInt32(dr["IdHorarioDinamico"]);
             ((Modelo.HorarioDinamicoRestricao)obj).IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
             ((Modelo.HorarioDinamicoRestricao)obj).IdContrato = Convert.ToInt32(dr["IdContrato"]);

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
                ,new SqlParameter ("@IdHorarioDinamico", SqlDbType.Int)
                ,new SqlParameter ("@IdEmpresa", SqlDbType.Int)
                ,new SqlParameter ("@IdContrato", SqlDbType.Int)

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
            parms[1].Value = ((Modelo.HorarioDinamicoRestricao)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioDinamicoRestricao)obj).Incdata;
            parms[3].Value = ((Modelo.HorarioDinamicoRestricao)obj).Inchora;
            parms[4].Value = ((Modelo.HorarioDinamicoRestricao)obj).Incusuario;
            parms[5].Value = ((Modelo.HorarioDinamicoRestricao)obj).Altdata;
            parms[6].Value = ((Modelo.HorarioDinamicoRestricao)obj).Althora;
            parms[7].Value = ((Modelo.HorarioDinamicoRestricao)obj).Altusuario;
           parms[8].Value = ((Modelo.HorarioDinamicoRestricao)obj).IdHorarioDinamico;
           parms[9].Value = ((Modelo.HorarioDinamicoRestricao)obj).IdEmpresa;
           parms[10].Value = ((Modelo.HorarioDinamicoRestricao)obj).IdContrato;

        }

        public Modelo.HorarioDinamicoRestricao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioDinamicoRestricao obj = new Modelo.HorarioDinamicoRestricao();
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

        public List<Modelo.HorarioDinamicoRestricao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.HorarioDinamicoRestricao> lista = new List<Modelo.HorarioDinamicoRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioDinamicoRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioDinamicoRestricao>>(dr);
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

        public List<Modelo.HorarioDinamicoRestricao> GetAllListByHorarios(List<int> idsHorario)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string consulta = SELECTALL + String.Format(" and hr.idhorarioDinamico in ({0}) ", String.Join(",",idsHorario));
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);

            List<Modelo.HorarioDinamicoRestricao> lista = new List<Modelo.HorarioDinamicoRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioDinamicoRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioDinamicoRestricao>>(dr);
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

        public List<Modelo.HorarioDinamicoRestricao> LoadObjectByHorarioDinamico(List<int> idsHorarioDinamico)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idHorarioDinamico", SqlDbType.VarChar)
                };
            parms[0].Value = String.Join(",", idsHorarioDinamico);

            string sql = SELECTALL + @" and idhorariodinamico in (SELECT * FROM dbo.f_clausulaIn(@idHorarioDinamico))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.HorarioDinamicoRestricao> lista = new List<Modelo.HorarioDinamicoRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioDinamicoRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioDinamicoRestricao>>(dr);
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
