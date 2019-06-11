using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class EmpresaCw_Usuario : DAL.FB.DALBase, DAL.IEmpresaCw_Usuario
    {
        private EmpresaCw_Usuario()
        {
            GEN = "GEN_empresausuario_id";

            TABELA = "empresausuario";

            SELECTPID = "SELECT * FROM \"empresausuario\" WHERE \"id\" = @id";

            SELECTALL = "SELECT * FROM \"empresausuario\"";

            INSERT = "  INSERT INTO \"empresausuario\"" +
                                        "(\"codigo\", \"idempresa\", \"idusuario\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @idempresa, @idusuario, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"empresausuario\" SET \"codigo\" = @codigo " +
                                        ", \"idempresa\" = @idempresa " +
                                        ", \"idusuario\" = @idusuario " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"empresausuario\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"empresausuario\"";

        }

        #region Singleton

        private static volatile FB.EmpresaCw_Usuario _instancia = null;

        public static FB.EmpresaCw_Usuario GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.EmpresaCw_Usuario))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.EmpresaCw_Usuario();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.EmpresaCw_Usuario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.EmpresaCw_Usuario)obj).IdEmpresa = Convert.ToInt32(dr["idempresa"]);
                    ((Modelo.EmpresaCw_Usuario)obj).IdCw_Usuario = Convert.ToInt32(dr["idusuario"]);
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

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@idempresa", FbDbType.Integer),
				new FbParameter ("@idusuario", FbDbType.Integer),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj)
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
            FbDataReader dr = LoadDataReader(id);

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
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idempresa", FbDbType.Integer)
            };

            parms[0].Value = idEmpresa;

            string SQL = "SELECT   \"empresausuario\".\"id\"" +
                                    " , \"empresausuario\".\"codigo\"" +
                                    " , \"empresa\".\"nome\" AS \"empresa\"" +
                                    " , \"cw_usuario\".\"nome\" AS \"usuario\"" +
                             " FROM \"empresausuario\"" +
                             " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"empresausuario\".\"idempresa\"" +
                             " INNER JOIN \"cw_usuario\" ON \"cw_usuario\".\"id\" = \"empresausuario\".\"idusuario\"" +
                             " WHERE \"empresausuario\".\"idempresa\" = @idempresa";
            DataTable dt = new DataTable();
            dt.Load(DataBase.ExecuteReader(CommandType.Text, SQL, parms));
            return dt;
        }
        #endregion

        #region IEmpresaCw_Usuario Members

        Modelo.EmpresaCw_Usuario IEmpresaCw_Usuario.LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        DataTable IEmpresaCw_Usuario.GetPorEmpresa(int idEmpresa)
        {
            throw new NotImplementedException();
        }

        List<Modelo.EmpresaCw_Usuario> IEmpresaCw_Usuario.GetListaPorEmpresa(int idEmpresa)
        {
            throw new NotImplementedException();
        }

        #endregion


        public Modelo.Proxy.pxyEmpresaCwUsuario GetListaUsuariosLiberadosBloquadosPorEmpresa(int idEmpresa)
        {
            throw new NotImplementedException();
        }

        public Modelo.Cw_Usuario GetUsuarioPorCodigo(int codigo)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void InserirRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans, SqlConnection con)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }
    }
}
