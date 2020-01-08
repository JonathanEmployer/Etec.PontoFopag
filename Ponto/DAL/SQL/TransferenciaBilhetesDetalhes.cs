using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class TransferenciaBilhetesDetalhes : DAL.SQL.DALBase, DAL.ITransferenciaBilhetesDetalhes
    {

        public TransferenciaBilhetesDetalhes(DataBase database)
        {
            db = database;
            TABELA = "TransferenciaBilhetesDetalhes";

            SELECTPID = @"   SELECT * FROM TransferenciaBilhetesDetalhes tbd WHERE tbd.id = @id";

            SELECTALL = @"   SELECT   tbd.*
                             FROM TransferenciaBilhetesDetalhes tbd
                            Where 1 = 1 ";

            INSERT = @"  INSERT INTO TransferenciaBilhetesDetalhes
							( codigo,  incdata,  inchora,  incusuario,  IdBilhetesImp,  IdTransferenciaBilhetes)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdBilhetesImp, @IdTransferenciaBilhetes)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE TransferenciaBilhetesDetalhes SET  
                                  codigo = @codigo
                                , altdata = @altdata
                                , althora = @althora
                                , altusuario = @altusuario
                                , IdBilhetesImp = @IdBilhetesImp
                                , IdTransferenciaBilhetes = @IdTransferenciaBilhetes

						WHERE id = @id";

            DELETE = @"  DELETE FROM TransferenciaBilhetesDetalhes WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS tbd.codigo FROM TransferenciaBilhetesDetalhes tbd";

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
                obj = new Modelo.TransferenciaBilhetesDetalhes();
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
            ((Modelo.TransferenciaBilhetesDetalhes)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.TransferenciaBilhetesDetalhes)obj).IdBilhetesImp = Convert.ToInt32(dr["IdBilhetesImp"]);
            ((Modelo.TransferenciaBilhetesDetalhes)obj).IdTransferenciaBilhetes = Convert.ToInt32(dr["IdTransferenciaBilhetes"]);
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
                ,new SqlParameter ("@IdBilhetesImp", SqlDbType.Int)
                ,new SqlParameter ("@IdTransferenciaBilhetes", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).Codigo;
            parms[2].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).Incdata;
            parms[3].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).Inchora;
            parms[4].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).Incusuario;
            parms[5].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).Altdata;
            parms[6].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).Althora;
            parms[7].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).Altusuario;
            parms[8].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).IdBilhetesImp;
            parms[9].Value = ((Modelo.TransferenciaBilhetesDetalhes)obj).IdTransferenciaBilhetes;
        }

        public Modelo.TransferenciaBilhetesDetalhes LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.TransferenciaBilhetesDetalhes obj = new Modelo.TransferenciaBilhetesDetalhes();
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

        public List<Modelo.TransferenciaBilhetesDetalhes> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.TransferenciaBilhetesDetalhes> lista = new List<Modelo.TransferenciaBilhetesDetalhes>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.TransferenciaBilhetesDetalhes>();
                lista = AutoMapper.Mapper.Map<List<Modelo.TransferenciaBilhetesDetalhes>>(dr);
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

        public List<Modelo.TransferenciaBilhetesDetalhes> GetAllListByTransferenciaBilhetes(int idTransferenciaBilhetes)
        {
            SqlParameter[] parms = new SqlParameter[1] {
                new SqlParameter("@idTransferenciaBilhetes", SqlDbType.Int)
            };

            parms[0].Value = idTransferenciaBilhetes;
            string sql = SELECTALL + "and tbd.idTransferenciaBilhetes = @idTransferenciaBilhetes";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.TransferenciaBilhetesDetalhes> lista = new List<Modelo.TransferenciaBilhetesDetalhes>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.TransferenciaBilhetesDetalhes>();
                lista = AutoMapper.Mapper.Map<List<Modelo.TransferenciaBilhetesDetalhes>>(dr);
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
