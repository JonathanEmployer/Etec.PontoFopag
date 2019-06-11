using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class LimiteDDsr : DAL.SQL.DALBase, ILimiteDDsr
    {
        private string SELECTPCOD;
        private string SELECTPHOR;
        public LimiteDDsr(DataBase database)
        {
            db = database;

            TABELA = "limiteddsr";

            SELECTALLLIST = @"
                select ld.*, convert(varchar, h.codigo) + ' | ' + h.descricao as horario from limiteddsr ld
                left join horario h on ld.idhorario = h.id";

            SELECTALL = @"
                select ld.id, ld.codigo, ld.limiteperdadsr, ld.qtdhorasdsr
                    , convert(varchar, h.codigo) + ' | ' + h.descricao as horario from limiteddsr ld
                left join horario h on ld.idhorario = h.id";

            SELECTPID = @"
                select ld.*, convert(varchar, h.codigo) + ' | ' + h.descricao as horario from limiteddsr ld
                left join horario h on ld.idhorario = h.id
                where ld.id = @id";

            SELECTPCOD = @"
                select ld.*, convert(varchar, h.codigo) + ' | ' + h.descricao as horario from limiteddsr ld
                left join horario h on ld.idhorario = h.id
                where ld.codigo = @codigo";
            SELECTPHOR = @"
                select ld.*, convert(varchar, h.codigo) + ' | ' + h.descricao as horario from limiteddsr ld
                left join horario h on ld.idhorario = h.id
                where ld.idhorario = @idhorario";

            INSERT = @" 
                INSERT INTO limiteddsr
                    (codigo,limiteperdadsr,qtdhorasdsr,idhorario,incdata,inchora,incusuario)
                VALUES
                    (@codigo,@limiteperdadsr,@qtdhorasdsr,@idhorario,@incdata,@inchora,@incusuario)
				SET @id = SCOPE_IDENTITY()";

            UPDATE = @"
                UPDATE limiteddsr
                    SET codigo = @codigo
                        ,limiteperdadsr = @limiteperdadsr
                        ,qtdhorasdsr = @qtdhorasdsr
                        ,idhorario = @idhorario
                        ,incdata = @incdata
                        ,inchora = @inchora
                        ,incusuario = @incusuario
                        ,altdata = @altdata
                        ,althora = @althora
                        ,altusuario = @altusuario
				WHERE id = @id";

            DELETE = @"  DELETE FROM limiteddsr WHERE id = @id";

            MAXCOD = @"SELECT COALESCE(MAX(codigo),0) AS codigo FROM limiteddsr";
        }
        protected override bool SetInstance(System.Data.SqlClient.SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiCampos(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Contrato();
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

        private void AtribuiCampos(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.LimiteDDsr)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.LimiteDDsr)obj).IdHorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.LimiteDDsr)obj).LimitePerdaDsr = Convert.ToString(dr["limiteperdadsr"]);
            ((Modelo.LimiteDDsr)obj).QtdHorasDsr = Convert.ToString(dr["qtdhorasdsr"]);
            ((Modelo.LimiteDDsr)obj).Horario = Convert.ToString(dr["horario"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@idhorario", SqlDbType.Int),
				new SqlParameter ("@limiteperdadsr", SqlDbType.VarChar),
                new SqlParameter ("@qtdhorasdsr", SqlDbType.VarChar),
                new SqlParameter ("@horario", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.LimiteDDsr)obj).Codigo;
            parms[2].Value = ((Modelo.LimiteDDsr)obj).IdHorario;
            parms[3].Value = ((Modelo.LimiteDDsr)obj).LimitePerdaDsr;
            parms[4].Value = ((Modelo.LimiteDDsr)obj).QtdHorasDsr;
            parms[5].Value = ((Modelo.LimiteDDsr)obj).Horario;
            parms[6].Value = ((Modelo.LimiteDDsr)obj).Incdata;
            parms[7].Value = ((Modelo.LimiteDDsr)obj).Inchora;
            parms[8].Value = ((Modelo.LimiteDDsr)obj).Incusuario;
            parms[9].Value = ((Modelo.LimiteDDsr)obj).Altdata;
            parms[10].Value = ((Modelo.LimiteDDsr)obj).Althora;
            parms[11].Value = ((Modelo.LimiteDDsr)obj).Altusuario;
        }

        public Modelo.LimiteDDsr LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int)
            };
            parms[0].Value = id;

            string aux = SELECTPID;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.LimiteDDsr obj = new Modelo.LimiteDDsr();
            SetInstance(dr, obj);
            return obj;
        }

        public List<Modelo.LimiteDDsr> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALLLIST, parms);

            List<Modelo.LimiteDDsr> lista = new List<Modelo.LimiteDDsr>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LimiteDDsr>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LimiteDDsr>>(dr);
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

        public Modelo.LimiteDDsr LoadPorCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPCOD, parms);

            Modelo.LimiteDDsr obj = new Modelo.LimiteDDsr();
            SetInstance(dr, obj);
            return obj;
        }

        public List<Modelo.LimiteDDsr> GetAllListPorHorario(int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idhorario", SqlDbType.Int)
            };
            parms[0].Value = idHorario;
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPHOR, parms);

            List<Modelo.LimiteDDsr> lista = new List<Modelo.LimiteDDsr>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LimiteDDsr>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LimiteDDsr>>(dr);
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

        public List<Modelo.LimiteDDsr> GetAllListPorHorarios(List<int> idsHorario)
        {
            string sql = @"select ld.id, ld.codigo, ld.limiteperdadsr, ld.qtdhorasdsr
                    , ld.idhorario from limiteddsr ld where ld.idhorario in ("+String.Join(",", idsHorario)+")";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql);

            List<Modelo.LimiteDDsr> lista = new List<Modelo.LimiteDDsr>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LimiteDDsr>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LimiteDDsr>>(dr);
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
