using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class JustificativaRestricao : DAL.SQL.DALBase, DAL.IJustificativaRestricao
    {

        public JustificativaRestricao(DataBase database)
        {
            db = database;
            TABELA = "JustificativaRestricao";

            SELECTPID = @" SELECT * FROM JustificativaRestricao WHERE id = @id ";

            SELECTALL = @" SELECT jr.*,
	                               Convert(VARCHAR(20), e.codigo)+' | '+e.nome DescEmpresa,
	                               Convert(VARCHAR(20), c.codigo)+' | '+c.codigocontrato+' - ' +c.descricaocontrato DescContrato
                              FROM JustificativaRestricao jr
                              LEFT JOIN empresa e on jr.IdEmpresa = e.id
                              LEFT JOIN contrato c on jr.IdContrato = c.id
                             where 1 = 1 ";

            INSERT = @"  INSERT INTO JustificativaRestricao
							(codigo, incdata, inchora, incusuario, IdJustificativa,IdEmpresa,IdContrato)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdJustificativa,@IdEmpresa,@IdContrato)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE JustificativaRestricao SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdJustificativa = @IdJustificativa
                           ,IdEmpresa = @IdEmpresa
                           ,IdContrato = @IdContrato

						WHERE id = @id";

            DELETE = @"  DELETE FROM JustificativaRestricao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM JustificativaRestricao";

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
                obj = new Modelo.JustificativaRestricao();
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
            ((Modelo.JustificativaRestricao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.JustificativaRestricao)obj).IdJustificativa = Convert.ToInt32(dr["IdJustificativa"]);
             ((Modelo.JustificativaRestricao)obj).IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
             ((Modelo.JustificativaRestricao)obj).IdContrato = Convert.ToInt32(dr["IdContrato"]);
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
                ,new SqlParameter ("@IdJustificativa", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.JustificativaRestricao)obj).Codigo;
            parms[2].Value = ((Modelo.JustificativaRestricao)obj).Incdata;
            parms[3].Value = ((Modelo.JustificativaRestricao)obj).Inchora;
            parms[4].Value = ((Modelo.JustificativaRestricao)obj).Incusuario;
            parms[5].Value = ((Modelo.JustificativaRestricao)obj).Altdata;
            parms[6].Value = ((Modelo.JustificativaRestricao)obj).Althora;
            parms[7].Value = ((Modelo.JustificativaRestricao)obj).Altusuario;
           parms[8].Value =  ((Modelo.JustificativaRestricao)obj).IdJustificativa;
           parms[9].Value =  ((Modelo.JustificativaRestricao)obj).IdEmpresa;
           parms[10].Value = ((Modelo.JustificativaRestricao)obj).IdContrato;

        }

        public Modelo.JustificativaRestricao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.JustificativaRestricao obj = new Modelo.JustificativaRestricao();
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

        public List<Modelo.JustificativaRestricao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.JustificativaRestricao> lista = new List<Modelo.JustificativaRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.JustificativaRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.JustificativaRestricao>>(dr);
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

        public List<Modelo.JustificativaRestricao> GetAllListByJustificativas(List<int> idsHorario)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string consulta = SELECTALL + String.Format(" and jr.idJustificativa in ({0}) ", String.Join(",",idsHorario));
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);

            List<Modelo.JustificativaRestricao> lista = new List<Modelo.JustificativaRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.JustificativaRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.JustificativaRestricao>>(dr);
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

        public List<Modelo.JustificativaRestricao> LoadObjectByJustificativa(List<int> idsJustificativa)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idJustificativa", SqlDbType.VarChar)
                };
            parms[0].Value = String.Join(",", idsJustificativa);

            string sql = SELECTALL + @" and idJustificativa in (SELECT * FROM dbo.f_clausulaIn(@idJustificativa))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.JustificativaRestricao> lista = new List<Modelo.JustificativaRestricao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.JustificativaRestricao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.JustificativaRestricao>>(dr);
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

        public void ExcluirByJustificativas(SqlTransaction trans, List<int> idJustificativas)
        {
            SqlParameter[] parms = new SqlParameter[]{};

            string aux = String.Format(" delete from JustificativaRestricao where IdJustificativa in ({0}) ", String.Join(",",idJustificativas));

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }
    }
}
