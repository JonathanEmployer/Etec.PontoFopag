using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class RepHistoricoLocal : DAL.SQL.DALBase, DAL.IRepHistoricoLocal
    {

        public string SELECTPREP { get; set; }

        public RepHistoricoLocal(DataBase database)
        {
            db = database;
            TABELA = "RepHistoricoLocal";

            SELECTPID = @"   SELECT * FROM RepHistoricoLocal WHERE id = @id";

            SELECTALL = @"   SELECT * FROM RepHistoricoLocal";

            SELECTPREP = @"  SELECT * FROM RepHistoricoLocal WHERE idRep = @idRep";

            INSERT = @"  INSERT INTO RepHistoricoLocal
							(codigo, incdata, inchora, incusuario, idRep, local, data)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @idRep, @local, @data) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE RepHistoricoLocal SET codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idRep = @idRep
                            , local = @local
                            , data = @data
						WHERE id = @id";

            DELETE = @"  DELETE FROM RepHistoricoLocal WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM RepHistoricoLocal";

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
                obj = new Modelo.FuncionarioHistorico();
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
            ((Modelo.RepHistoricoLocal)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.RepHistoricoLocal)obj).IdRep = Convert.ToInt32(dr["IdRep"]);
            ((Modelo.RepHistoricoLocal)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.RepHistoricoLocal)obj).Local = Convert.ToString(dr["local"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@IdRep", SqlDbType.Int),
				new SqlParameter ("@data", SqlDbType.DateTime),
				new SqlParameter ("@local", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.RepHistoricoLocal)obj).Codigo;
            parms[2].Value = ((Modelo.RepHistoricoLocal)obj).IdRep;
            parms[3].Value = ((Modelo.RepHistoricoLocal)obj).Data;
            parms[4].Value = ((Modelo.RepHistoricoLocal)obj).Local;
            parms[5].Value = ((Modelo.RepHistoricoLocal)obj).Incdata;
            parms[6].Value = ((Modelo.RepHistoricoLocal)obj).Inchora;
            parms[7].Value = ((Modelo.RepHistoricoLocal)obj).Incusuario;
            parms[8].Value = ((Modelo.RepHistoricoLocal)obj).Altdata;
            parms[9].Value = ((Modelo.RepHistoricoLocal)obj).Althora;
            parms[10].Value = ((Modelo.RepHistoricoLocal)obj).Altusuario;
        }

        public Modelo.RepHistoricoLocal LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.RepHistoricoLocal objRepHistoricoLocal = new Modelo.RepHistoricoLocal();
            try
            {
                SetInstance(dr, objRepHistoricoLocal);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objRepHistoricoLocal;
        }

        public Modelo.RepHistoricoLocal GetUltimoRepHistLocal(int idRep)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idRep", SqlDbType.Int, 4) };
            parms[0].Value = idRep;

            string sql = @"select top(1) 
					                local, 
					                codigo, 
					                idRep 
			                   from RepHistoricoLocal 
			                  where idrep = @idrep order by data desc";

            SqlDataReader reader = db.ExecuteReader(CommandType.Text, sql, parms);
            Modelo.RepHistoricoLocal ret = new Modelo.RepHistoricoLocal();
            try
            {
                var map = AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RepHistoricoLocal>();
                ret = AutoMapper.Mapper.Map<List<Modelo.RepHistoricoLocal>>(reader).FirstOrDefault();
                if (ret == null)
                {
                    ret = new Modelo.RepHistoricoLocal();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                reader.Dispose();
            }
            return ret;
        }

        /// <summary>
        /// Método que Retorna uma lista de Locais por Rep
        /// </summary>
        /// <returns>Retorna uma lista de Locais por Rep</returns>
        public List<pxyRepHistoricoLocalAgrupado> RepHistoricoLocalAgrupado()
        {
            SqlParameter[] parms = new SqlParameter[] {};

            string sql = @"select t.* 
                              from (
	                            select isnull(rhl.codigo, r.codigolocal) codigoLocal,
		                               isnull(rhl.local, r.local) local,
		                               r.numrelogio
	                              from rep r
	                              left join rephistoricolocal rhl on r.id = rhl.idrep
	                               ) t
                             Group by t.codigoLocal, t.local, numrelogio
                             union
                             select -1 codigoLocal, 'Local não registrado' local, '00'";

            SqlDataReader reader = db.ExecuteReader(CommandType.Text, sql, parms);
            List<pxyRepHistoricoLocalAgrupado> ret = new List<pxyRepHistoricoLocalAgrupado>();
            try
            {
                var map = AutoMapper.Mapper.CreateMap<IDataReader, pxyRepHistoricoLocalAgrupado>();
                ret = AutoMapper.Mapper.Map<List<pxyRepHistoricoLocalAgrupado>>(reader);
                if (ret == null)
                {
                    ret = new List<pxyRepHistoricoLocalAgrupado>();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                reader.Dispose();
            }
            return ret;
        }

        public List<Modelo.RepHistoricoLocal> LoadPorRep(int idRep)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idRep", SqlDbType.Int, 4) };
            parms[0].Value = idRep;

             SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPREP, parms);

             List<Modelo.RepHistoricoLocal> lista = new List<Modelo.RepHistoricoLocal>();
            try
            {
                while (dr.Read())
                {
                    Modelo.RepHistoricoLocal objRepHistoricoLocal = new Modelo.RepHistoricoLocal();
                    AuxSetInstance(dr, objRepHistoricoLocal);
                    lista.Add(objRepHistoricoLocal);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.RepHistoricoLocal> GetAllGrid(int id)
        {
            List<Modelo.RepHistoricoLocal> lista = new List<Modelo.RepHistoricoLocal>();

            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@id", SqlDbType.Int) };

            parms[0].Value = id;

            string aux;

            aux = @" SELECT rephl.id,
	                 rephl.codigo,
	                 rephl.data,
	                 rephl.local
                     FROM dbo.RepHistoricoLocal rephl
                     WHERE rephl.idRep = @id";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.RepHistoricoLocal>();
                lista = AutoMapper.Mapper.Map<List<Modelo.RepHistoricoLocal>>(dr);
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

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.IncluirAux(trans, obj);
            AuxManutencao(trans, obj);
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.AlterarAux(trans, obj);
            AuxManutencao(trans, obj);
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.ExcluirAux(trans, obj);
            AuxManutencao(trans, obj);
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            REP dalRep = new REP(new DataBase(db.ConnectionString));
            dalRep.UsuarioLogado = this.UsuarioLogado;
            dalRep.SetaUltimoLocal(trans,((Modelo.RepHistoricoLocal)obj).IdRep);
        }

        /// <summary>
        /// Exclui os históricos de mudança de um rep.
        /// </summary>
        /// <param name="trans"> Transação da exclusão</param>
        /// <param name="idRep"> Id do rep a ser apagado os históricos</param>
        public void ExcluirPorRep(SqlTransaction trans, int idRep)
        {
            SqlParameter[] parms = { new SqlParameter("@idRep", SqlDbType.Int, 4) };
            parms[0].Value = idRep;
            string strDelete = @"  DELETE FROM RepHistoricoLocal WHERE idRep = @idRep";
            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, strDelete, true, parms);
        }
        #endregion
    }
}
