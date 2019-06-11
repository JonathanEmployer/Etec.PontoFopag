using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class EmpresaLogo : DAL.SQL.DALBase, DAL.IEmpresaLogo
    {
        public string SELECTLIST { get { return @" SELECT * FROM EmpresaLogo "; } }

        public string SELECTPRI { get; set; }

        public EmpresaLogo(DataBase database)
        {
            db = database;
            TABELA = "EmpresaLogo";

            SELECTPID = @"   SELECT * FROM EmpresaLogo WHERE id = @id";

            SELECTPRI = @"   SELECT TOP 1 * FROM parametros ORDER BY parametros.codigo";

            SELECTALL = @"   SELECT   parm.id
                                    , parm.codigo
                                    , parm.descricao
                                    , Logo
                                    , IdEmpresa
                             FROM parametros parm";

            INSERT = @"  INSERT INTO EmpresaLogo
							(codigo, incdata, inchora, incusuario, Logo, IdEmpresa)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Logo, @IdEmpresa) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE EmpresaLogo SET
							  codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , Logo = @Logo
                            , IdEmpresa = @IdEmpresa

						WHERE id = @id";

            DELETE = @"  DELETE FROM EmpresaLogo WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM EmpresaLogo";

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
                obj = new Modelo.EmpresaLogo();
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
            ((Modelo.EmpresaLogo)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.EmpresaLogo)obj).Logo = Convert.ToString(dr["Logo"]);;
            ((Modelo.EmpresaLogo)obj).IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]);
            if (dr["validade"] is DBNull)
            {
                ((Modelo.Empresa)obj).Validade = DateTime.MaxValue.Date;
            }
            else
            {
                ((Modelo.Empresa)obj).Validade = Convert.ToDateTime(dr["validade"]);
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
                new SqlParameter ("@Logo", SqlDbType.VarChar),
                new SqlParameter ("@IdEmpresa", SqlDbType.Int),

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
            parms[1].Value = ((Modelo.EmpresaLogo)obj).Codigo;
            parms[2].Value = ((Modelo.EmpresaLogo)obj).Incdata;
            parms[3].Value = ((Modelo.EmpresaLogo)obj).Inchora;
            parms[4].Value = ((Modelo.EmpresaLogo)obj).Incusuario;
            parms[5].Value = ((Modelo.EmpresaLogo)obj).Altdata;
            parms[6].Value = ((Modelo.EmpresaLogo)obj).Althora;
            parms[7].Value = ((Modelo.EmpresaLogo)obj).Altusuario;
            parms[8].Value = ((Modelo.EmpresaLogo)obj).Logo;
            parms[9].Value = ((Modelo.EmpresaLogo)obj).IdEmpresa;
        }

        public Modelo.EmpresaLogo LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.EmpresaLogo objEmpresaLogo = new Modelo.EmpresaLogo();
            try
            {

                SetInstance(dr, objEmpresaLogo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEmpresaLogo;
        }

        public Modelo.EmpresaLogo LoadPrimeiro()
        {
            Modelo.EmpresaLogo objEmpresaLogo = new Modelo.EmpresaLogo();
            try
            {
                SqlParameter[] parms = new SqlParameter[0];

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPRI, parms);

                SetInstance(dr, objEmpresaLogo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEmpresaLogo;
        }

        public List<Modelo.EmpresaLogo> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM EmpresaLogo", parms);

            List<Modelo.EmpresaLogo> lista = new List<Modelo.EmpresaLogo>();
            try
            {
                while (dr.Read())
                {
                    Modelo.EmpresaLogo objEmpresaLogo = new Modelo.EmpresaLogo();
                    AuxSetInstance(dr, objEmpresaLogo);
                    lista.Add(objEmpresaLogo);
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
        public List<Modelo.EmpresaLogo> GetAllListPorIds(List<int> ids)
        {
            List<Modelo.EmpresaLogo> result = new List<Modelo.EmpresaLogo>();

            try
            {
                var parameters = new string[ids.Count];
                List<SqlParameter> parmList = new List<SqlParameter>();
                for (int i = 0; i < ids.Count; i++)
                {
                    parameters[i] = string.Format("@Id{0}", i);
                    parmList.Add(new SqlParameter(parameters[i], ids[i]));
                }

                string sql = string.Format("SELECT * from EmpresaLogo WHERE Id IN ({0})", string.Join(", ", parameters));

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parmList.ToArray());

                try
                {
                    while (dr.Read())
                    {
                        Modelo.EmpresaLogo objEmpresaLogo = new Modelo.EmpresaLogo();
                        AuxSetInstance(dr, objEmpresaLogo);
                        result.Add(objEmpresaLogo);
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
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
        public List<Modelo.EmpresaLogo> GetAllListPorEmpresa(int IDEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@IdEmpresa", SqlDbType.Int)
            };
            parms[0].Value = IDEmpresa;

            string sql = @" Select * from EmpresaLogo where idEmpresa = @IdEmpresa ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.EmpresaLogo> lista = new List<Modelo.EmpresaLogo>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EmpresaLogo>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EmpresaLogo>>(dr);
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



