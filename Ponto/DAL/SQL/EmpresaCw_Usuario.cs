using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Modelo.Proxy;
using AutoMapper;
using System.Linq;

namespace DAL.SQL
{
    public class EmpresaCW_Usuario : DAL.SQL.DALBase, DAL.IEmpresaCw_Usuario
    {
        public EmpresaCW_Usuario(DataBase database)
        {
            db = database;
            TABELA = "empresacwusuario";

            SELECTPID = @"   SELECT * FROM empresacwusuario WHERE id = @id";

            SELECTALL = "SELECT * FROM empresacwusuario";

            INSERT = @"  INSERT INTO empresacwusuario
							(codigo, idempresa, idcw_usuario, incdata, inchora, incusuario)
							VALUES
							(@codigo, @idempresa, @idcw_usuario, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE empresacwusuario SET
							  codigo = @codigo
							, idempresa = @idempresa
							, idcw_usuario = @idcw_usuario
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM empresacwusuario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM empresacwusuario";

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
                obj = new Modelo.EmpresaCw_Usuario();
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
            ((Modelo.EmpresaCw_Usuario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.EmpresaCw_Usuario)obj).IdEmpresa = Convert.ToInt32(dr["idempresa"]);
            ((Modelo.EmpresaCw_Usuario)obj).IdCw_Usuario = Convert.ToInt32(dr["idcw_usuario"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idempresa", SqlDbType.Int),
				new SqlParameter ("@idcw_usuario", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.EmpresaCw_Usuario)obj).Codigo;
            parms[2].Value = ((Modelo.EmpresaCw_Usuario)obj).IdEmpresa;
            parms[3].Value = ((Modelo.EmpresaCw_Usuario)obj).IdCw_Usuario;
            parms[4].Value = ((Modelo.EmpresaCw_Usuario)obj).Incdata;
            parms[5].Value = ((Modelo.EmpresaCw_Usuario)obj).Inchora;
            parms[6].Value = ((Modelo.EmpresaCw_Usuario)obj).Incusuario;
            parms[7].Value = ((Modelo.EmpresaCw_Usuario)obj).Altdata;
            parms[8].Value = ((Modelo.EmpresaCw_Usuario)obj).Althora;
            parms[9].Value = ((Modelo.EmpresaCw_Usuario)obj).Altusuario;
        }

        public Modelo.EmpresaCw_Usuario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.EmpresaCw_Usuario objEmpresaUsuario = new Modelo.EmpresaCw_Usuario();
            try
            {
                SetInstance(dr, objEmpresaUsuario);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEmpresaUsuario;
        }

        public DataTable GetPorEmpresa(int idEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idempresa", SqlDbType.Int)
            };

            parms[0].Value = idEmpresa;

            string SQL = @"SELECT   empresacwusuario.id
                                    , empresacwusuario.codigo
                                    , empresa.nome AS empresa
                                    , cw_usuario.nome AS usuario
                             FROM empresacwusuario
                             INNER JOIN empresa ON empresa.id = empresacwusuario.idempresa
                             INNER JOIN cw_usuario ON cw_usuario.id = idcw_usuario
                             WHERE empresacwusuario.idempresa = @idempresa";
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SQL, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public List<Modelo.EmpresaCw_Usuario> GetListaPorEmpresa(int idEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idempresa", SqlDbType.Int)
            };
            parms[0].Value = idEmpresa;

            var query = @" SELECT * FROM empresacwusuario 
                           WHERE empresacwusuario.idempresa = @idempresa";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, query, parms);

            List<Modelo.EmpresaCw_Usuario> lista = new List<Modelo.EmpresaCw_Usuario>();
            try
            {
                while (dr.Read())
                {
                    Modelo.EmpresaCw_Usuario objEmpresa = new Modelo.EmpresaCw_Usuario();
                    AuxSetInstance(dr, objEmpresa);
                    lista.Add(objEmpresa);
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

        public pxyEmpresaCwUsuario GetListaUsuariosLiberadosBloquadosPorEmpresa(int idEmpresa)
        {
            pxyEmpresaCwUsuario res = new pxyEmpresaCwUsuario();
            string sqlEmpresa = @"select emp.id, emp.codigo, convert(varchar, emp.codigo) + ' | ' + emp.nome + ' (' + coalesce(emp.cnpj, emp.cpf, '') + ')' as NomeEmpresa, coalesce(emp.cnpj, emp.cpf, '') as CpfCnpj from empresa emp where id = @idempresa";
            string sqlBloqueados = @"select cwu.id, cwu.codigo, cwu.login, cwu.nome, 0 as IdEmpCwUsuario from cw_usuario cwu where cwu.UtilizaControleEmpresa = 1 and cwu.id not in (select ecw.idcw_usuario from empresacwusuario ecw where ecw.idempresa = @idempresa)";
            string sqlLiberados = @"select cw.id, cw.codigo, cw.login, cw.nome, ecwu.id as IdEmpCwUsuario from empresacwusuario ecwu join cw_usuario cw on ecwu.idcw_usuario = cw.id where ecwu.idempresa = @idempresa";

            SqlParameter[] parms1 = new SqlParameter[]
            {
                new SqlParameter("@idempresa", SqlDbType.Int)
            };
            
            SqlParameter[] parms2 = new SqlParameter[] 
            {
                new SqlParameter("@idempresa", SqlDbType.Int)
            };

            SqlParameter[] parms3 = new SqlParameter[]
            {
                new SqlParameter("@idempresa", SqlDbType.Int)
            };
            parms1[0].Value = idEmpresa;
            parms2[0].Value = idEmpresa;
            parms3[0].Value = idEmpresa;
            

            List<Modelo.EmpresaCw_Usuario> lista = new List<Modelo.EmpresaCw_Usuario>();
            try
            {
                SqlDataReader dr1 = db.ExecuteReader(CommandType.Text, sqlEmpresa, parms1);
                SqlDataReader dr2 = db.ExecuteReader(CommandType.Text, sqlBloqueados, parms2);
                SqlDataReader dr3 = db.ExecuteReader(CommandType.Text, sqlLiberados, parms3);

                var mapEmp = Mapper.CreateMap<IDataReader, pxyEmpresaNomeCnpjCodigo>();
                var mapLib = Mapper.CreateMap<IDataReader, pxyEmpresaCwUsuarioDetalhe>();
                var mapBloq = Mapper.CreateMap<IDataReader, pxyEmpresaCwUsuarioDetalhe>();

                pxyEmpresaNomeCnpjCodigo emp = Mapper.Map<List<pxyEmpresaNomeCnpjCodigo>>(dr1).FirstOrDefault();
                res.NomeEmpresa = emp.NomeEmpresa;
                res.ListaBloqueados = Mapper.Map<List<pxyEmpresaCwUsuarioDetalhe>>(dr2);
                res.ListaLiberados = Mapper.Map<List<pxyEmpresaCwUsuarioDetalhe>>(dr3);

                if (!dr1.IsClosed)
                {
                    dr1.Close();
                }
                dr1.Dispose();
                if (!dr2.IsClosed)
                {
                    dr2.Close();
                }
                dr2.Dispose();
                if (!dr3.IsClosed)
                {
                    dr3.Close();
                }
                dr3.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public Modelo.Cw_Usuario GetUsuarioPorCodigo(int codigo)
        {
            Modelo.Cw_Usuario result = new Modelo.Cw_Usuario();

            try
            {
                string sql = "select * from cw_usuario cw where cw.codigo = @codigo";
                SqlParameter[] parms = new SqlParameter[]{ new SqlParameter("@codigo", SqlDbType.Int) };

                parms[0].Value = codigo;
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

                var mapUsr = Mapper.CreateMap<IDataReader, Modelo.Cw_Usuario>();
                result = Mapper.Map<List<Modelo.Cw_Usuario>>(dr).FirstOrDefault();
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            catch (Exception ew)
            {
                throw ew;
            }

            return result;
        }

        #endregion

    }
}
