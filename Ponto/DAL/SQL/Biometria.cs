using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class Biometria : DAL.SQL.DALBase, DAL.IBiometria
    {
        public Biometria(DataBase database)
        {
            db = database;
            TABELA = "biometria";

            SELECTPID = @"   SELECT * FROM biometria WHERE id = @id";

            SELECTALL = @"   SELECT * FROM biometria";

            INSERT = @"  INSERT INTO biometria
							(codigo, valorBiometria, idfuncionario, incdata, inchora, incusuario, IdRep)
							VALUES
							(@codigo, @valorBiometria, @idfuncionario, @incdata, @inchora, @incusuario, @idrep) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE biometria SET
							  codigo = @codigo
							, valorBiometria = @valorBiometria
                            , idfuncionario = @idfuncionario
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM biometria WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM biometria";
        }

        private string SqlLoadByCodigo()
        {
            return @"   SELECT * FROM biometria WHERE codigo = @codigo";
        }

        private string SqlLoadPorFuncionario()
        {
            return @"   SELECT * FROM biometria WHERE idfuncionario = @idfuncionario";
        }

        protected override bool SetInstance(System.Data.SqlClient.SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.Biometria)obj).valorBiometria = (dr["valorBiometria"] is DBNull ? new Byte[0] : ((Byte[])dr["valorBiometria"]));
                    ((Modelo.Biometria)obj).idfuncionario = (dr["idfuncionario"] is DBNull ? 0 : Convert.ToInt32(dr["idfuncionario"]));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Biometria();
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

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@valorBiometria", SqlDbType.VarBinary),
                new SqlParameter ("@idfuncionario", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idrep", SqlDbType.VarChar)
            };
            return parms;
        }

        protected override void SetParameters(System.Data.SqlClient.SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.Biometria)obj).Codigo;
            parms[2].Value = ((Modelo.Biometria)obj).valorBiometria;
            parms[3].Value = ((Modelo.Biometria)obj).idfuncionario;
            parms[4].Value = ((Modelo.Biometria)obj).Incdata;
            parms[5].Value = ((Modelo.Biometria)obj).Inchora;
            parms[6].Value = ((Modelo.Biometria)obj).Incusuario;
            parms[7].Value = ((Modelo.Biometria)obj).Altdata;
            parms[8].Value = ((Modelo.Biometria)obj).Althora;
            parms[9].Value = ((Modelo.Biometria)obj).Altusuario;
            parms[10].Value = ((Modelo.Biometria)obj).idRep;
        }

        public Modelo.Biometria LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Biometria objBiometria = new Modelo.Biometria();
            try
            {
                SetInstance(dr, objBiometria);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objBiometria;
        }

        public List<Modelo.Biometria> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.Biometria> lista = new List<Modelo.Biometria>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Biometria>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Biometria>>(dr);
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

        public Modelo.Biometria LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlLoadByCodigo(), parms);
            Modelo.Biometria objBiometria = new Modelo.Biometria();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Biometria>();
                objBiometria = AutoMapper.Mapper.Map<Modelo.Biometria>(dr);
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

            return objBiometria;
        }

        public List<Modelo.Biometria> LoadPorFuncionario(int idfuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = idfuncionario;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlLoadPorFuncionario(), parms);
            List<Modelo.Biometria> lstBiometria = new List<Modelo.Biometria>();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Biometria>();
                lstBiometria = AutoMapper.Mapper.Map<List<Modelo.Biometria>>(dr);
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

            return lstBiometria;
        }

        private string SqlGetBiometriaTipoBiometria()
        {
            string sql = @"
    select 
		count(1) as Quantidade,
		tb.Descricao as Tecnologia
	from biometria b
	join rep r on r.id = b.IdRep
	join EquipamentoTipoBiometria etb on etb.id = r.IdEquipamentoTipoBiometria 
	join TipoBiometria tb on tb.id = etb.IdTipoBiometria
	where b.idfuncionario = @idfuncionario
	group by b.IdRep, tb.Descricao";
            return sql;
        }

        public List<Modelo.Biometria> GetBiometriaTipoBiometria(int IdFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idfuncionario", IdFuncionario )
            };

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SqlGetBiometriaTipoBiometria(), parms);

            List<Modelo.Biometria> lista = new List<Modelo.Biometria>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Biometria objBiometria = new Modelo.Biometria();
                    AuxSetInstanceBiometriaTipoBiometria(dr, objBiometria);
                    lista.Add(objBiometria);
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

        private void AuxSetInstanceBiometriaTipoBiometria(SqlDataReader dr, Modelo.Biometria obj)
        {
            ((Modelo.Biometria)obj).Quantidade = Convert.ToInt32(dr["Quantidade"]);
            ((Modelo.Biometria)obj).Tecnologia = Convert.ToString(dr["Tecnologia"]);
        }
    }
}
