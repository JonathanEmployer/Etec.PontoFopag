using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class HorarioRestricao : DAL.SQL.DALBase, DAL.IHorarioRestricao
    {

        public HorarioRestricao(DataBase database)
        {
            db = database;
            TABELA = "HorarioRestricao";

            SELECTPID = @"   SELECT * FROM HorarioRestricao WHERE id = @id";

            SELECTALL = @" SELECT hr.*,
	                               Convert(VARCHAR(20), e.codigo)+' | '+e.nome DescEmpresa,
	                               Convert(VARCHAR(20), c.codigo)+' | '+c.codigocontrato+' - ' +c.descricaocontrato DescContrato
                              FROM HorarioRestricao hr
                              LEFT JOIN empresa e on hr.IdEmpresa = e.id
                              LEFT JOIN contrato c on hr.IdContrato = c.id
                             where 1 = 1 ";

            INSERT = @"  INSERT INTO HorarioRestricao
							(codigo, incdata, inchora, incusuario, IdHorario,IdEmpresa,IdContrato)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdHorario,@IdEmpresa,@IdContrato)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE HorarioRestricao SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdHorario = @IdHorario
                           ,IdEmpresa = @IdEmpresa
                           ,IdContrato = @IdContrato

						WHERE id = @id";

            DELETE = @"  DELETE FROM HorarioRestricao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM HorarioRestricao";

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
                obj = new Modelo.HorarioRestricao();
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
            ((Modelo.HorarioRestricao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.HorarioRestricao)obj).IdHorario = Convert.ToInt32(dr["IdHorario"]);
             ((Modelo.HorarioRestricao)obj).IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
             ((Modelo.HorarioRestricao)obj).IdContrato = Convert.ToInt32(dr["IdContrato"]);

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
                ,new SqlParameter ("@IdHorario", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.HorarioRestricao)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioRestricao)obj).Incdata;
            parms[3].Value = ((Modelo.HorarioRestricao)obj).Inchora;
            parms[4].Value = ((Modelo.HorarioRestricao)obj).Incusuario;
            parms[5].Value = ((Modelo.HorarioRestricao)obj).Altdata;
            parms[6].Value = ((Modelo.HorarioRestricao)obj).Althora;
            parms[7].Value = ((Modelo.HorarioRestricao)obj).Altusuario;
           parms[8].Value = ((Modelo.HorarioRestricao)obj).IdHorario;
           parms[9].Value = ((Modelo.HorarioRestricao)obj).IdEmpresa;
           parms[10].Value = ((Modelo.HorarioRestricao)obj).IdContrato;

        }

        public Modelo.HorarioRestricao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioRestricao obj = new Modelo.HorarioRestricao();
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

        public List<Modelo.HorarioRestricao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.HorarioRestricao> lista = new List<Modelo.HorarioRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioRestricao>>(dr);
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

        public List<Modelo.HorarioRestricao> GetAllListByHorarios(List<int> idsHorario)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string consulta = SELECTALL + String.Format(" and hr.idhorario in ({0}) ", String.Join(",",idsHorario));
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);

            List<Modelo.HorarioRestricao> lista = new List<Modelo.HorarioRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioRestricao>>(dr);
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
