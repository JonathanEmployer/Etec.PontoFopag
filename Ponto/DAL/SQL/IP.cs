using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class IP : DAL.SQL.DALBase, DAL.IIP
    {

        public IP(DataBase database)
        {
            db = database;
            TABELA = "IP";

            SELECTPID = @"   SELECT * FROM IP WHERE id = @id";

            SELECTALL = @"   SELECT   IP.*
                             FROM IP";

            INSERT = @"  INSERT INTO IP
							(codigo, incdata, inchora, incusuario, IPDNS, Tipo, IdEmpresa, bloqueiaRegistrador, bloqueiaPontoFopag)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IPDNS, @Tipo, @IdEmpresa, @bloqueiaRegistrador, @bloqueiaPontoFopag) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE IP SET
							  codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , IPDNS = @IPDNS
                            , Tipo = @Tipo
                            , IdEmpresa = @IdEmpresa
                            , bloqueiaRegistrador = @bloqueiaRegistrador
                            , bloqueiaPontoFopag = @bloqueiaPontoFopag
						WHERE id = @id";

            DELETE = @"  DELETE FROM IP WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM IP";

        }

        #region Metodos

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
                obj = new Modelo.IP();
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
            ((Modelo.IP)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.IP)obj).IPDNS = Convert.ToString(dr["IPDNS"]);
            ((Modelo.IP)obj).Tipo = Convert.ToInt16(dr["tipo"]);
            ((Modelo.IP)obj).IdEmpresa = Convert.ToInt32(dr["idEmpresa"]);
            ((Modelo.IP)obj).BloqueiaRegistrador = Convert.ToBoolean(dr["bloqueiaRegistrador"]);
            ((Modelo.IP)obj).BloqueiaPontoFopag = Convert.ToBoolean(dr["bloqueiaPontoFopag"]);
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
                new SqlParameter ("@IPDNS", SqlDbType.VarChar),
                new SqlParameter ("@tipo", SqlDbType.SmallInt),
                new SqlParameter ("@idEmpresa", SqlDbType.Int),
                new SqlParameter ("@bloqueiaRegistrador", SqlDbType.Bit),
                new SqlParameter ("@bloqueiaPontoFopag", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.IP)obj).Codigo;
            parms[2].Value = ((Modelo.IP)obj).Incdata;
            parms[3].Value = ((Modelo.IP)obj).Inchora;
            parms[4].Value = ((Modelo.IP)obj).Incusuario;
            parms[5].Value = ((Modelo.IP)obj).Altdata;
            parms[6].Value = ((Modelo.IP)obj).Althora;
            parms[7].Value = ((Modelo.IP)obj).Altusuario;
            parms[8].Value = ((Modelo.IP)obj).IPDNS;
            parms[9].Value = ((Modelo.IP)obj).Tipo;
            parms[10].Value = ((Modelo.IP)obj).IdEmpresa;
            parms[11].Value = ((Modelo.IP)obj).BloqueiaRegistrador;
            parms[12].Value = ((Modelo.IP)obj).BloqueiaPontoFopag;
        }

        public Modelo.IP LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.IP objIP = new Modelo.IP();
            try
            {

                SetInstance(dr, objIP);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objIP;
        }

        public List<Modelo.IP> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM IP", parms);

            List<Modelo.IP> lista = new List<Modelo.IP>();
            try
            {
                while (dr.Read())
                {
                    Modelo.IP objIP = new Modelo.IP();
                    AuxSetInstance(dr, objIP);
                    lista.Add(objIP);
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

        public List<Modelo.IP> GetAllListPorIds(List<int> ids)
        {
            List<Modelo.IP> result = new List<Modelo.IP>();

            try
            {
                var parameters = new string[ids.Count];
                List<SqlParameter> parmList = new List<SqlParameter>();
                for (int i = 0; i < ids.Count; i++)
                {
                    parameters[i] = string.Format("@Id{0}", i);
                    parmList.Add(new SqlParameter(parameters[i], ids[i]));
                }

                string sql = string.Format("SELECT * from IP WHERE Id IN ({0})", string.Join(", ", parameters));

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parmList.ToArray());

                try
                {
                    while (dr.Read())
                    {
                        Modelo.IP objIP = new Modelo.IP();
                        AuxSetInstance(dr, objIP);
                        result.Add(objIP);
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
        #endregion


        public List<Modelo.IP> GetAllListPorEmpresa(int IDEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@IdEmpresa", SqlDbType.Int)
            };
            parms[0].Value = IDEmpresa;

            string sql = @" Select * from ip where idEmpresa = @Idempresa ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.IP> lista = new List<Modelo.IP>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.IP>();
                lista = AutoMapper.Mapper.Map<List<Modelo.IP>>(dr);
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
