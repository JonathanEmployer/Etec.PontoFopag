using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class OcorrenciaRestricao : DAL.SQL.DALBase, DAL.IOcorrenciaRestricao
    {

        public OcorrenciaRestricao(DataBase database)
        {
            db = database;
            TABELA = "OcorrenciaRestricao";

            SELECTPID = @" SELECT * FROM OcorrenciaRestricao WHERE id = @id ";

            SELECTALL = @" SELECT ocr.*,
	                               Convert(VARCHAR(20), e.codigo)+' | '+e.nome DescEmpresa,
	                               Convert(VARCHAR(20), c.codigo)+' | '+c.codigocontrato+' - ' +c.descricaocontrato DescContrato
                              FROM OcorrenciaRestricao ocr
                              LEFT JOIN empresa e on ocr.IdEmpresa = e.id
                              LEFT JOIN contrato c on ocr.IdContrato = c.id
                             where 1 = 1 ";

            INSERT = @"  INSERT INTO OcorrenciaRestricao
							(codigo, incdata, inchora, incusuario, IdOcorrencia,IdEmpresa,IdContrato)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdOcorrencia,@IdEmpresa,@IdContrato)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE OcorrenciaRestricao SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdOcorrencia = @IdOcorrencia
                           ,IdEmpresa = @IdEmpresa
                           ,IdContrato = @IdContrato

						WHERE id = @id";

            DELETE = @"  DELETE FROM OcorrenciaRestricao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM OcorrenciaRestricao";

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
                obj = new Modelo.OcorrenciaRestricao();
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
            ((Modelo.OcorrenciaRestricao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.OcorrenciaRestricao)obj).IdOcorrencia = Convert.ToInt32(dr["IdOcorrencia"]);
             ((Modelo.OcorrenciaRestricao)obj).IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
             ((Modelo.OcorrenciaRestricao)obj).IdContrato = Convert.ToInt32(dr["IdContrato"]);
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
                ,new SqlParameter ("@IdOcorrencia", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.OcorrenciaRestricao)obj).Codigo;
            parms[2].Value = ((Modelo.OcorrenciaRestricao)obj).Incdata;
            parms[3].Value = ((Modelo.OcorrenciaRestricao)obj).Inchora;
            parms[4].Value = ((Modelo.OcorrenciaRestricao)obj).Incusuario;
            parms[5].Value = ((Modelo.OcorrenciaRestricao)obj).Altdata;
            parms[6].Value = ((Modelo.OcorrenciaRestricao)obj).Althora;
            parms[7].Value = ((Modelo.OcorrenciaRestricao)obj).Altusuario;
           parms[8].Value =  ((Modelo.OcorrenciaRestricao)obj).IdOcorrencia;
           parms[9].Value =  ((Modelo.OcorrenciaRestricao)obj).IdEmpresa;
           parms[10].Value = ((Modelo.OcorrenciaRestricao)obj).IdContrato;

        }

        public Modelo.OcorrenciaRestricao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.OcorrenciaRestricao obj = new Modelo.OcorrenciaRestricao();
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

        public List<Modelo.OcorrenciaRestricao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.OcorrenciaRestricao> lista = new List<Modelo.OcorrenciaRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.OcorrenciaRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.OcorrenciaRestricao>>(dr);
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

        public List<Modelo.OcorrenciaRestricao> GetAllListByOcorrencias(List<int> idsHorario)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string consulta = SELECTALL + String.Format(" and ocr.idOcorrencia in ({0}) ", String.Join(",",idsHorario));
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);

            List<Modelo.OcorrenciaRestricao> lista = new List<Modelo.OcorrenciaRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.OcorrenciaRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.OcorrenciaRestricao>>(dr);
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

        public List<Modelo.OcorrenciaRestricao> LoadObjectByOcorrencia(List<int> idsOcorrencia)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idOcorrencia", SqlDbType.VarChar)
                };
            parms[0].Value = String.Join(",", idsOcorrencia);

            string sql = SELECTALL + @" and idOcorrencia in (SELECT * FROM dbo.f_clausulaIn(@idOcorrencia))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.OcorrenciaRestricao> lista = new List<Modelo.OcorrenciaRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.OcorrenciaRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.OcorrenciaRestricao>>(dr);
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

        public void ExcluirByOcorrencias(SqlTransaction trans, List<int> idOcorrencias)
        {
            SqlParameter[] parms = new SqlParameter[]{};

            string aux = String.Format(" delete from OcorrenciaRestricao where IdOcorrencia in ({0}) ", String.Join(",",idOcorrencias));

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }
    }
}
