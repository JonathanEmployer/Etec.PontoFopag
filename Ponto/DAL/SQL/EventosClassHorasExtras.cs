using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class EventosClassHorasExtras : DAL.SQL.DALBase, DAL.IEventosClassHorasExtras
    {

        public EventosClassHorasExtras(DataBase database)
        {
            db = database;
            TABELA = "EventosClassHorasExtras";

            SELECTPID = @"   SELECT * FROM EventosClassHorasExtras WHERE id = @id";

            SELECTALL = @"   SELECT   EventosClassHorasExtras.*
                             FROM EventosClassHorasExtras";

            INSERT = @"  INSERT INTO EventosClassHorasExtras
							(codigo, incdata, inchora, incusuario, IdEventos,IdClassificacao)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdEventos,@IdClassificacao)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE EventosClassHorasExtras SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdEventos = @IdEventos
                           ,IdClassificacao = @IdClassificacao

						WHERE id = @id";

            DELETE = @"  DELETE FROM EventosClassHorasExtras WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM EventosClassHorasExtras";

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
                obj = new Modelo.EventosClassHorasExtras();
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
            ((Modelo.EventosClassHorasExtras)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.EventosClassHorasExtras)obj).IdEventos = Convert.ToInt32(dr["IdEventos"]);
             ((Modelo.EventosClassHorasExtras)obj).IdClassificacao = Convert.ToInt32(dr["IdClassificacao"]);

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
                ,new SqlParameter ("@IdEventos", SqlDbType.Int)
                ,new SqlParameter ("@IdClassificacao", SqlDbType.Int)

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
            parms[1].Value = ((Modelo.EventosClassHorasExtras)obj).Codigo;
            parms[2].Value = ((Modelo.EventosClassHorasExtras)obj).Incdata;
            parms[3].Value = ((Modelo.EventosClassHorasExtras)obj).Inchora;
            parms[4].Value = ((Modelo.EventosClassHorasExtras)obj).Incusuario;
            parms[5].Value = ((Modelo.EventosClassHorasExtras)obj).Altdata;
            parms[6].Value = ((Modelo.EventosClassHorasExtras)obj).Althora;
            parms[7].Value = ((Modelo.EventosClassHorasExtras)obj).Altusuario;
           parms[8].Value = ((Modelo.EventosClassHorasExtras)obj).IdEventos;
           parms[9].Value = ((Modelo.EventosClassHorasExtras)obj).IdClassificacao;

        }

        public Modelo.EventosClassHorasExtras LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.EventosClassHorasExtras obj = new Modelo.EventosClassHorasExtras();
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

        public List<Modelo.EventosClassHorasExtras> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.EventosClassHorasExtras> lista = new List<Modelo.EventosClassHorasExtras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EventosClassHorasExtras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EventosClassHorasExtras>>(dr);
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

        /// <summary>
        /// Retorna todas as classificações ligadas a um evento
        /// </summary>
        /// <param name="idEvento">Id do evento</param>
        /// <returns>Lista com as Classificações</returns>
        public IList<Modelo.EventosClassHorasExtras> GetListPorEvento(int idEvento)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idEvento", DbType.Int32)
            };

            parms[0].Value = idEvento;

            string sql = @"SELECT ec.* 
                             FROM EventosClassHorasExtras ec
                            WHERE ec.IdEventos = @idEvento";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.EventosClassHorasExtras> lista = new List<Modelo.EventosClassHorasExtras>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.EventosClassHorasExtras>();
                lista = AutoMapper.Mapper.Map<List<Modelo.EventosClassHorasExtras>>(dr);
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

        /// <summary>
        /// Retorna todos os ids separados por virgulas das classificações ligados ao evento
        /// </summary>
        /// <param name="idEvento">Id do evento</param>
        /// <returns>string com os ids separados por virgulas das classificações</returns>
        public string GetIdsClassificacaoPorEvento(int idEvento)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idEvento", DbType.Int32)
            };

            parms[0].Value = idEvento;

            string sql = @"SELECT stuff(list,1,1,'')
                              FROM (
                                    SELECT ',' + cast(ec.IdClassificacao AS varchar(20)) as [text()]
                                      FROM EventosClassHorasExtras ec
		                             WHERE ec.IdEventos = @idEvento
                                       FOR XML PATH('')
                                   ) as Sub(list)";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            string retorno = "";
            try
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        retorno = dr.GetString(0);
                    }   
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
            return retorno;
        }

        public void IncluirList(SqlTransaction trans, List<Modelo.EventosClassHorasExtras> objetos)
        {
            foreach (var item in objetos)
            {
                this.IncluirAux(trans, item);
            }
        }

        public void IncluirPorEvento(SqlTransaction trans, Modelo.Eventos eve)
        {
            if ((eve.EventosClassHorasExtras == null || eve.EventosClassHorasExtras.Count() == 0) && !String.IsNullOrEmpty(eve.IdsClassificadas) && eve.ClassificarHorasExtras)
            {
                eve.EventosClassHorasExtras = GerarEventosClassHorasExtrasPorEvento(eve, eve.IdsClassificadas.Split(',').Select(Int32.Parse).ToList());
            }
            if (eve.ClassificarHorasExtras)
            {
                IncluirList(trans, eve.EventosClassHorasExtras.ToList());
            }
        }

        public void ExluirList(SqlTransaction trans, List<Modelo.EventosClassHorasExtras> objetos)
        {
            foreach (var item in objetos)
            {
                this.ExcluirAux(trans, item);
            }
        }

        public void ExcluirPorEvento(SqlTransaction trans, Modelo.Eventos eve)
        {
            if (eve.EventosClassHorasExtras == null || eve.EventosClassHorasExtras.Count() == 0)
            {
                eve.EventosClassHorasExtras = GetListPorEvento(eve.Id);
            }
            ExluirList(trans, eve.EventosClassHorasExtras.ToList());
        }

        /// <summary>
        /// Deleta o relacionamento entra Evento e Classificações que não existam em uma lista
        /// </summary>
        /// <param name="idEvento">Id do evento</param>
        /// <param name="IdsClassificacao">Ids das Classificações que devem permanecer</param>
        public void ExcluirNotInList(SqlTransaction trans, int idEvento, List<int> IdsClassificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idEvento", DbType.Int32),
                new SqlParameter("@idsClass", DbType.String)
            };

            parms[0].Value = idEvento;
            parms[1].Value = String.Join(",", IdsClassificacao);

            string deleteList = @"DELETE FROM EventosClassHorasExtras WHERE IdEventos = @idEvento AND IdClassificacao NOT IN ((select * from dbo.F_ClausulaIn(@idsClass)))";

            TransactDbOps.ExecuteNonQuery(trans, CommandType.Text, deleteList, parms);
        }

        public void AlterarClassificacoesPorEvento(SqlTransaction trans, Modelo.Eventos eve)
        {
            if (eve.ClassificarHorasExtras)
            {
                string classAnt = GetIdsClassificacaoPorEvento(eve.Id);
                List<int> listClassAnt = new List<int>();
                if (!String.IsNullOrEmpty(classAnt))
                {
                    listClassAnt = classAnt.Split(',').Select(Int32.Parse).ToList();
                }

                List<int> listClassNova = new List<int>();
                if (!String.IsNullOrEmpty(eve.IdsClassificadas))
                {
                    listClassNova = eve.IdsClassificadas.Split(',').Select(Int32.Parse).ToList();
                }

                List<int> incluir = listClassNova.Except(listClassAnt).ToList();
                List<Modelo.EventosClassHorasExtras> novos = GerarEventosClassHorasExtrasPorEvento(eve, incluir);
                IncluirList(trans, novos);
                ExcluirNotInList(trans, eve.Id, listClassNova);
            }
            else
            {
                ExcluirPorEvento(trans, eve);
            }
            
        }

        public List<Modelo.EventosClassHorasExtras> GerarEventosClassHorasExtrasPorEvento(Modelo.Eventos eve, List<int> listClassNova)
        {
            List<Modelo.EventosClassHorasExtras> novos = new List<Modelo.EventosClassHorasExtras>();
            var i = MaxCodigo();
            foreach (int idClass in listClassNova)
            {       
                novos.Add(new Modelo.EventosClassHorasExtras() { IdEventos = eve.Id, IdClassificacao = idClass, Codigo= i});
                i++;
            }            
            return novos;
        }              
    }
}
