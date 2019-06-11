using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class Cw_GrupoAcesso : DAL.SQL.DALBase, DAL.ICw_GrupoAcesso
    {
        private string GetAllPorGrupo()
        {
            return @"
                SELECT ga.*
	                , ca.Controller as Controller
	                , ca.Nome as Nome
	                , ca.Menu as Menu
					, ca.Cadastrar as HabilitarCadastro
	                , ca.Consultar as HabilitarConsulta
	                , ca.Alterar as HabilitarAlteracao 
	                , ca.Excluir as HabilitarExclusao 
	
                FROM Cw_GrupoAcesso ga 
                    join cw_acesso ca on ga.IDCw_Acesso = ca.id
                    WHERE ga.IDCw_Grupo = @id";
        }
        public Cw_GrupoAcesso(DataBase database)
        {
            db = database;
            TABELA = "Cw_GrupoAcesso";

            SELECTPID = @"   SELECT * FROM Cw_GrupoAcesso WHERE id = @id";

            SELECTALL = @"   SELECT *
                             FROM Cw_GrupoAcesso";



            INSERT = @"  INSERT INTO Cw_GrupoAcesso
							(codigo, IDCw_Grupo, IdCw_Acesso, Consultar, Excluir, Cadastrar, Alterar, incdata, inchora, incusuario)
							VALUES
							(@codigo, @IDCw_Grupo, @IdCw_Acesso, @Consultar, @Excluir, @Cadastrar, @Alterar, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE Cw_GrupoAcesso SET
							  IDCw_Grupo = @IDCw_Grupo
							, IdCw_Acesso = @IdCw_Acesso
                            , Consultar = @Consultar
                            , Excluir = @Excluir
                            , Cadastrar = @Cadastrar
                            , Alterar = @Alterar
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , codigo = @codigo
						WHERE id = @id";

            DELETE = @"  DELETE FROM Cw_GrupoAcesso WHERE id = @id";

            MAXCOD = @"select max(codigo) as codigo from cw_grupoacesso";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.Cw_GrupoAcesso)obj).IDCw_Acesso = Convert.ToInt32(dr["IDCw_Acesso"]);
                    ((Modelo.Cw_GrupoAcesso)obj).IDCw_Grupo = Convert.ToInt32(dr["IDCw_Grupo"]);
                    ((Modelo.Cw_GrupoAcesso)obj).Consultar = Convert.ToInt32(dr["Consultar"]);
                    ((Modelo.Cw_GrupoAcesso)obj).Excluir = Convert.ToInt32(dr["Excluir"]);
                    ((Modelo.Cw_GrupoAcesso)obj).Cadastrar = Convert.ToInt32(dr["Cadastrar"]);
                    ((Modelo.Cw_GrupoAcesso)obj).Alterar = Convert.ToInt32(dr["Alterar"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Justificativa();
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
            ((Modelo.Cw_GrupoAcesso)obj).IDCw_Acesso = Convert.ToInt32(dr["IDCw_Acesso"]);
            ((Modelo.Cw_GrupoAcesso)obj).IDCw_Grupo = Convert.ToInt32(dr["IDCw_Grupo"]);
            ((Modelo.Cw_GrupoAcesso)obj).Consultar = Convert.ToInt32(dr["Consultar"]);
            ((Modelo.Cw_GrupoAcesso)obj).Excluir = Convert.ToInt32(dr["Excluir"]);
            ((Modelo.Cw_GrupoAcesso)obj).Cadastrar = Convert.ToInt32(dr["Cadastrar"]);
            ((Modelo.Cw_GrupoAcesso)obj).Alterar = Convert.ToInt32(dr["Alterar"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@IDCw_Acesso", SqlDbType.Int),
				new SqlParameter ("@IDCw_Grupo", SqlDbType.Int),
                new SqlParameter ("@Consultar", SqlDbType.Int),
                new SqlParameter ("@Excluir", SqlDbType.Int),
                new SqlParameter ("@Cadastrar", SqlDbType.Int),
                new SqlParameter ("@Alterar", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.Cw_GrupoAcesso)obj).Codigo;
            parms[2].Value = ((Modelo.Cw_GrupoAcesso)obj).IDCw_Acesso;
            parms[3].Value = ((Modelo.Cw_GrupoAcesso)obj).IDCw_Grupo;
            parms[4].Value = ((Modelo.Cw_GrupoAcesso)obj).Consultar;
            parms[5].Value = ((Modelo.Cw_GrupoAcesso)obj).Excluir;
            parms[6].Value = ((Modelo.Cw_GrupoAcesso)obj).Cadastrar;
            parms[7].Value = ((Modelo.Cw_GrupoAcesso)obj).Alterar;
            parms[8].Value = ((Modelo.Cw_GrupoAcesso)obj).Incdata;
            parms[9].Value = ((Modelo.Cw_GrupoAcesso)obj).Inchora;
            parms[10].Value = ((Modelo.Cw_GrupoAcesso)obj).Incusuario;
            parms[11].Value = ((Modelo.Cw_GrupoAcesso)obj).Altdata;
            parms[12].Value = ((Modelo.Cw_GrupoAcesso)obj).Althora;
            parms[13].Value = ((Modelo.Cw_GrupoAcesso)obj).Altusuario;
        }

        public Modelo.Cw_GrupoAcesso LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Cw_GrupoAcesso objCw_GrupoAcesso = new Modelo.Cw_GrupoAcesso();
            try
            {

                SetInstance(dr, objCw_GrupoAcesso);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_GrupoAcesso;
        }

        public List<Modelo.Cw_GrupoAcesso> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM Cw_GrupoAcesso", parms);

            List<Modelo.Cw_GrupoAcesso> lista = new List<Modelo.Cw_GrupoAcesso>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Cw_GrupoAcesso objCw_GrupoAcesso = new Modelo.Cw_GrupoAcesso();
                    AuxSetInstance(dr, objCw_GrupoAcesso);
                    lista.Add(objCw_GrupoAcesso);
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

        public List<Modelo.Proxy.PxyGridGrupodeUsuario> GetAllGrid()
        {
            List<Modelo.Proxy.PxyGridGrupodeUsuario> lista = new List<Modelo.Proxy.PxyGridGrupodeUsuario>();

            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @" SELECT id AS Id, 
	                        codigo AS Codigo, 
	                        nome AS Nome, 
	                        CASE WHEN acesso = 1 THEN 'Sim' ELSE 'Não' END AS StrAcesso
                            FROM dbo.cw_grupo
                                             ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridGrupodeUsuario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridGrupodeUsuario>>(dr);
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

        public List<Modelo.Proxy.PxyGridUsuario> GetAllGridU()
        {
            List<Modelo.Proxy.PxyGridUsuario> lista = new List<Modelo.Proxy.PxyGridUsuario>();

            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @" SELECT   us.id AS Id,
		                    us.codigo AS Codigo, 
		                    us.login AS Login,
		                    us.nome AS Nome,
		                    CAST(gp.codigo as varchar) + ' | ' + gp.nome AS Grupo,
		                    us.EMAIL AS Email,
		                    CASE WHEN us.UtilizaControleEmpresa = 1 THEN 'Sim' ELSE 'Não' END  AS ControlePorEmpresa,
		                    CASE WHEN us.UtilizaControleContratos = 1 THEN 'Sim' ELSE 'Não' END AS ControlePorContrato,
		                    CASE WHEN us.UtilizaControleSupervisor = 1 THEN 'Sim' ELSE 'Não' END AS ControlePorSupervisor                                                         
                            FROM cw_usuario us
                            LEFT JOIN cw_grupo gp ON gp.id = us.idgrupo  
                            WHERE us.login not in ('revenda','cwork')
                            AND us.idUsuarioCentralCliente is not null
                            AND us.idUsuarioCentralCliente <> ''
                                                            ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridUsuario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridUsuario>>(dr);
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

        public List<Modelo.Cw_GrupoAcesso> GetAllListPorGrupo(int idGrupo)
        {
            string sqlAcesso = "select * from cw_acesso where controller != '' and controller is not null";
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drAcc = db.ExecuteReader(CommandType.Text, sqlAcesso, parms);
            List<Modelo.CwAcesso> listaAcc = new List<Modelo.CwAcesso>();
            try
            {
                var mapHor = Mapper.CreateMap<IDataReader, Modelo.CwAcesso>();
                listaAcc = Mapper.Map<List<Modelo.CwAcesso>>(drAcc);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!drAcc.IsClosed)
                {
                    drAcc.Close();
                }
                drAcc.Dispose();
            }

            SqlParameter[] parms2 = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int)
            };
            parms2[0].Value = idGrupo;
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, GetAllPorGrupo(), parms2);
            List<Modelo.Cw_GrupoAcesso> listaGrpAcc = new List<Modelo.Cw_GrupoAcesso>();
            try
            {
                var mapCGA = Mapper.CreateMap<IDataReader, Modelo.Cw_GrupoAcesso>();
                listaGrpAcc = Mapper.Map<List<Modelo.Cw_GrupoAcesso>>(dr);
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

            DateTime dt = DateTime.Now;
            try
            {
                listaAcc.Where(w => !listaGrpAcc.Select(s => s.IDCw_Acesso).Contains(w.Id)).ToList().ForEach(f =>
                    {
                        listaGrpAcc.Add(new Modelo.Cw_GrupoAcesso()
                        {
                            Codigo = MaxCodigo(),
                            IDCw_Grupo = idGrupo,
                            IDCw_Acesso = f.Id,
                            Controller = f.Controller,
                            Nome = f.Nome,
                            Menu = f.Menu,
                            Cadastrar = 0,
                            Consultar = 0,
                            Alterar = 0,
                            Excluir = 0,
                            Incdata = dt,
                            Inchora = dt,
                            HabilitarAlteracao = f.Alterar,
                            HabilitarCadastro = f.Cadastrar,
                            HabilitarConsulta = f.Consultar,
                            HabilitarExclusao = f.Excluir
                        });
                    });
                foreach (var item in listaGrpAcc.Where(w => w.Id == 0))
                {
                    item.Codigo = MaxCodigo();
                    Incluir(item);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return listaGrpAcc;
        }

        public Modelo.Cw_GrupoAcesso LoadObjectIDCw_Grupo(int id)
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM Cw_GrupoAcesso where IDCw_Acesso", parms);
            Modelo.Cw_GrupoAcesso objCw_GrupoAcesso = new Modelo.Cw_GrupoAcesso();
            try
            {
                while (dr.Read())
                {
                    AuxSetInstance(dr, objCw_GrupoAcesso);
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
            return objCw_GrupoAcesso;
        }
        #endregion
    }
}
