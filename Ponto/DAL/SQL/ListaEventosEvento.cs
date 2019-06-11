using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class ListaEventosEvento : DAL.SQL.DALBase, DAL.IListaEventosEvento
    {

        public ListaEventosEvento(DataBase database)
        {
            db = database;
            TABELA = "TAB_Lista_Eventos_Evento";

            SELECTPID = @"   SELECT * FROM TAB_Lista_Eventos_Evento WHERE Id = @id";

            SELECTALL = @"   SELECT * FROM TAB_Lista_Eventos_Evento";

            INSERT = @"  INSERT INTO TAB_Lista_Eventos_Evento
							(Idf_Lista_Eventos, Idf_Evento)
							VALUES
							(@Idf_Lista_Eventos, @Idf_Evento)
						SET @Id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE TAB_Lista_Eventos_Evento SET  
                            Idf_Lista_Eventos = @Idf_Lista_Eventos
                           ,Idf_Evento = @Idf_Evento

						WHERE Id = @id";

            DELETE = @"  DELETE FROM TAB_Lista_Eventos_Evento WHERE Id = @id";

            MAXCOD = @"  SELECT MAX(Id) AS Id FROM TAB_Lista_Eventos_Evento";

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
                obj = new Modelo.ListaEventosEvento();
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
            //SetInstanceBase(dr, obj);

            ((Modelo.ListaEventosEvento)obj).Id = Convert.ToInt32(dr["Id"]);
            ((Modelo.ListaEventosEvento)obj).Idf_Lista_Eventos = Convert.ToInt32(dr["Idf_Lista_Eventos"]);
            ((Modelo.ListaEventosEvento)obj).Idf_Evento = Convert.ToInt32(dr["Idf_Evento"]);

        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				 new SqlParameter ("@Id", SqlDbType.Int)
                ,new SqlParameter ("@Idf_Lista_Eventos", SqlDbType.Int)
                ,new SqlParameter ("@Idf_Evento", SqlDbType.Int)

			};
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = ((Modelo.ListaEventosEvento)obj).Id;
            if (((Modelo.ListaEventosEvento)obj).Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.ListaEventosEvento)obj).Idf_Lista_Eventos;
            parms[2].Value = ((Modelo.ListaEventosEvento)obj).Idf_Evento;

        }

        public Modelo.ListaEventosEvento LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ListaEventosEvento obj = new Modelo.ListaEventosEvento();
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

        public List<Modelo.ListaEventosEvento> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.ListaEventosEvento> lista = new List<Modelo.ListaEventosEvento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ListaEventosEvento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ListaEventosEvento>>(dr);
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

        public void IncluirLoteIdsEvento(SqlTransaction trans, int idListaEventos, List<int> idsEventos)
        {
            if (idListaEventos > 0 && idsEventos.Count() > 0)
            {
                SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@idsEventos", SqlDbType.VarChar),
                    new SqlParameter("@idListaEventos", SqlDbType.Int)
                };

                parms[0].Value = String.Join(",", idsEventos);
                parms[1].Value = idListaEventos;

                string sql = @"INSERT INTO dbo.TAB_Lista_Eventos_Evento ( Idf_Lista_Eventos, Idf_Evento )
                           SELECT @idListaEventos, id FROM dbo.eventos WHERE id IN (SELECT * FROM dbo.F_ClausulaIn(@idsEventos))";
                try
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, sql, true, parms);
                }
                catch (SqlException ex)
                {
                    List<string> erros = new List<string>();
                    foreach (SqlError error in ex.Errors)
                    {
                        erros.Add(error.Message);
                    }
                    throw new Exception(String.Join("; ", erros));
                }
            }
        }

        public void ExcluirLoteIdsEvento(SqlTransaction trans, int idListaEventos, List<int> idsEventos)
        {
            if (idListaEventos > 0 && idsEventos.Count() > 0)
            {
                SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@idsEventos", SqlDbType.VarChar),
                    new SqlParameter("@Idf_Lista_Eventos", SqlDbType.Int)
                };

                parms[0].Value = String.Join(",", idsEventos);
                parms[1].Value = idListaEventos;

                try
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"DELETE FROM TAB_Lista_Eventos_Evento WHERE Idf_Evento in (SELECT * FROM dbo.F_ClausulaIn(@idsEventos)) and Idf_Lista_Eventos = @Idf_Lista_Eventos", true, parms);
                }
                catch (SqlException ex)
                {
                    List<string> erros = new List<string>();
                    foreach (SqlError error in ex.Errors)
                    {
                        erros.Add(error.Message);
                    }
                    throw new Exception(String.Join("; ", erros));
                }
            }
            
        }

        public void ExcluirLoteidListaEventos(SqlTransaction trans, int idListaEventos)
        {
            if (idListaEventos > 0)
            {
                SqlParameter[] parms = new SqlParameter[1]
                {
                    new SqlParameter("@Idf_Lista_Eventos", SqlDbType.Int)
                };

                parms[0].Value = idListaEventos;

                try
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, @"DELETE FROM TAB_Lista_Eventos_Evento WHERE Idf_Lista_Eventos = @Idf_Lista_Eventos", true, parms);
                }
                catch (SqlException ex)
                {
                    List<string> erros = new List<string>();
                    foreach (SqlError error in ex.Errors)
                    {
                        erros.Add(error.Message);
                    }
                    throw new Exception(String.Join("; ", erros));
                }
            }
        }

        public List<Modelo.ListaEventosEvento> GetAllPorListaEventos(Int32 idListaEventos)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idListaEventos", SqlDbType.Int)
            };

            parms[0].Value = idListaEventos;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM TAB_Lista_Eventos_Evento WHERE Idf_Lista_Eventos = @idListaEventos", parms);

            List<Modelo.ListaEventosEvento> lista = new List<Modelo.ListaEventosEvento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ListaEventosEvento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ListaEventosEvento>>(dr);
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
