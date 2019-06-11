using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class Cw_Usuario : DAL.SQL.DALBase, DAL.ICw_Usuario
    {
        string SELECTLOGIN = "";
        
        private Cw_Usuario()
        {

            TABELA = "cw_usuario";

            SELECTPID = @"   SELECT * FROM cw_usuario WHERE id = @id";

            SELECTALL = @"   SELECT   us.id
                                    , us.login
                                    , us.nome
                                    , us.codigo                                    
                                    , gp.nome AS grupo
                             FROM cw_usuario us
                             LEFT JOIN cw_grupo gp ON gp.id = us.idgrupo";

            INSERT = @"  INSERT INTO cw_usuario
							(codigo, login, senha, nome, tipo, idgrupo, incdata, inchora, incusuario)
							VALUES
							(@codigo, @login, @senha, @nome, @tipo, @idgrupo, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE cw_usuario SET 
							 codigo = @codigo
							, login = @login
							, senha = @senha
							, nome = @nome
							, tipo = @tipo
							, idgrupo = @idgrupo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM cw_usuario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM cw_usuario";

            SELECTLOGIN = @"SELECT * FROM cw_usuario WHERE login = @login";
        }

        #region Singleton

        private static volatile SQL.Cw_Usuario _instancia = null;

        public static SQL.Cw_Usuario GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(SQL.Cw_Usuario))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new SQL.Cw_Usuario();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.Cw_Usuario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.Cw_Usuario)obj).Login = Convert.ToString(dr["login"]);
                    ((Modelo.Cw_Usuario)obj).Senha = Convert.ToString(dr["senha"]);
                    ((Modelo.Cw_Usuario)obj).Nome = Convert.ToString(dr["nome"]);
                    ((Modelo.Cw_Usuario)obj).Tipo = Convert.ToInt32(dr["tipo"]);
                    ((Modelo.Cw_Usuario)obj).IdGrupo = Convert.ToInt32(dr["idgrupo"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Cw_Usuario();
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

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@login", SqlDbType.VarChar),
				new SqlParameter ("@senha", SqlDbType.VarChar),
				new SqlParameter ("@nome", SqlDbType.VarChar),
				new SqlParameter ("@tipo", SqlDbType.Int),
				new SqlParameter ("@idgrupo", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.Cw_Usuario)obj).Codigo;
            parms[2].Value = ((Modelo.Cw_Usuario)obj).Login;
            parms[3].Value = ((Modelo.Cw_Usuario)obj).Senha;
            parms[4].Value = ((Modelo.Cw_Usuario)obj).Nome;
            parms[5].Value = ((Modelo.Cw_Usuario)obj).Tipo;
            parms[6].Value = ((Modelo.Cw_Usuario)obj).IdGrupo;
            parms[7].Value = ((Modelo.Cw_Usuario)obj).Incdata;
            parms[8].Value = ((Modelo.Cw_Usuario)obj).Inchora;
            parms[9].Value = ((Modelo.Cw_Usuario)obj).Incusuario;
            parms[10].Value = ((Modelo.Cw_Usuario)obj).Altdata;
            parms[11].Value = ((Modelo.Cw_Usuario)obj).Althora;
            parms[12].Value = ((Modelo.Cw_Usuario)obj).Altusuario;
        }

        public Modelo.Cw_Usuario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Cw_Usuario objCw_Usuario = new Modelo.Cw_Usuario();
            try
            {
                SetInstance(dr, objCw_Usuario);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_Usuario;
        }

        public Modelo.Cw_Usuario LoadObjectLogin(string pLogin)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@login", SqlDbType.VarChar, 15) };
            parms[0].Value = pLogin;

            SqlDataReader dr = DataBase.ExecuteReader(CommandType.Text, SELECTLOGIN, parms);

            Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
            try
            {
                SetInstance(dr, objUsuario);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objUsuario;
        }

        #endregion
    }
}
