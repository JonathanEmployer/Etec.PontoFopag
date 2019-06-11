using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class ListaEventos : DAL.SQL.DALBase, DAL.IListaEventos
    {

        ListaEventosEvento dalListaEventosEvento;
        public ListaEventos(DataBase database)
        {
            db = database;
            dalListaEventosEvento = new ListaEventosEvento(db);
            TABELA = "TAB_Lista_Eventos";

            SELECTPID = @"   SELECT * FROM TAB_Lista_Eventos WHERE Id = @id";

            SELECTALL = @"    SELECT * FROM TAB_Lista_Eventos";

            INSERT = @"  INSERT INTO TAB_Lista_Eventos
							(Codigo, Des_Lista_Eventos, Idf_Usuario_Cadastro, Dta_Cadastro)
							VALUES
							(@Codigo, @Des_Lista_Eventos, @Idf_Usuario_Cadastro, @Dta_Cadastro)
                        SET @Id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE TAB_Lista_Eventos SET  
                            Codigo = @Codigo
						   ,Des_Lista_Eventos = @Des_Lista_Eventos
                           ,Idf_Usuario_Alteracao = @Idf_Usuario_Alteracao
                           ,Dta_Alteracao = @Dta_Alteracao

						WHERE Id = @id";

            DELETE = @"  DELETE FROM TAB_Lista_Eventos WHERE Id = @id";

            MAXCOD = @"  SELECT MAX(Codigo) AS Codigo FROM TAB_Lista_Eventos";

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

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            TratarInclusaoEExclusaoEvento(trans, obj);
        }

        private void TratarInclusaoEExclusaoEvento(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            List<Int32> IdsEventos = new List<Int32>();
            List<Int32> IdsEventosAntes = new List<Int32>();
            if (!String.IsNullOrEmpty(((Modelo.ListaEventos)obj).IdEventosSelecionados))
            {
                IdsEventos = ((Modelo.ListaEventos)obj).IdEventosSelecionados.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();
            }

            if (!String.IsNullOrEmpty(((Modelo.ListaEventos)obj).IdEventosSelecionados_Ant))
            {
                IdsEventosAntes = ((Modelo.ListaEventos)obj).IdEventosSelecionados_Ant.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();
            }

            List<Int32> idsEventosExcluir = IdsEventosAntes.Except(IdsEventos).ToList();
            List<Int32> idsEventosIncluir = IdsEventos.Except(IdsEventosAntes).ToList();

            dalListaEventosEvento.ExcluirLoteIdsEvento(trans, obj.Id, idsEventosExcluir);
            dalListaEventosEvento.IncluirLoteIdsEvento(trans, obj.Id, idsEventosIncluir);
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            auxExclusao(trans, obj);
            base.ExcluirAux(trans, obj);
        }

        private void auxExclusao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            dalListaEventosEvento.ExcluirLoteidListaEventos(trans, obj.Id);
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
                obj = new Modelo.ListaEventos();
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

            ((Modelo.ListaEventos)obj).Id = Convert.ToInt32(dr["Id"]);

            ((Modelo.ListaEventos)obj).Codigo = Convert.ToInt32(dr["Codigo"]);
            ((Modelo.ListaEventos)obj).Des_Lista_Eventos = Convert.ToString(dr["Des_Lista_Eventos"]);

            ((Modelo.ListaEventos)obj).Idf_Usuario_Cadastro = Convert.ToInt32(dr["Idf_Usuario_Cadastro"]);
            ((Modelo.ListaEventos)obj).Dta_Cadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

            ((Modelo.ListaEventos)obj).Idf_Usuario_Alteracao = (dr["Idf_Usuario_Alteracao"] is DBNull ? (int?)null : Convert.ToInt32(dr["Idf_Usuario_Alteracao"]));
            ((Modelo.ListaEventos)obj).Dta_Alteracao = (dr["Dta_Alteracao"] is DBNull ? (DateTime?)null : Convert.ToDateTime(dr["Dta_Alteracao"]));
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				 new SqlParameter ("@Id", SqlDbType.Int)
				,new SqlParameter ("@Codigo", SqlDbType.Int)
                ,new SqlParameter ("@Des_Lista_Eventos", SqlDbType.VarChar)
                ,new SqlParameter ("@Idf_Usuario_Cadastro", SqlDbType.Int)
                ,new SqlParameter ("@Dta_Cadastro", SqlDbType.DateTime)
                ,new SqlParameter ("@Idf_Usuario_Alteracao", SqlDbType.Int)
                ,new SqlParameter ("@Dta_Alteracao", SqlDbType.DateTime)

            };
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = ((Modelo.ListaEventos)obj).Id;
            if (((Modelo.ListaEventos)obj).Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
                parms[3].Value = UsuarioLogado.Id;
                parms[4].Value = DateTime.Now;
            } else
            {
                parms[5].Value = UsuarioLogado.Id;
                parms[6].Value = DateTime.Now;
            }
            parms[1].Value = ((Modelo.ListaEventos)obj).Codigo;
            parms[2].Value = ((Modelo.ListaEventos)obj).Des_Lista_Eventos;

        }

        public Modelo.ListaEventos LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ListaEventos obj = new Modelo.ListaEventos();
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

        public List<Modelo.ListaEventos> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.ListaEventos> lista = new List<Modelo.ListaEventos>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ListaEventos>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ListaEventos>>(dr);
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
