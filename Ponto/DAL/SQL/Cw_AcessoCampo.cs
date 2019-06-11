using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class Cw_AcessoCampo : DAL.SQL.DALBase, DAL.ICw_AcessoCampo
    {

        private Cw_AcessoCampo()
        {
            TABELA = "cw_acessocampo";

            SELECTPID = @"   SELECT * FROM cw_acessocampo WHERE id = @id";

            SELECTALL = @"   SELECT   a.id
                                    , a.codigo
                                    , a.idacesso
                                    , a.campo
                                    , a.display
                                    , (case when a.acesso = 1 then 'Sim' when a.acesso = 0 then 'Não' end) AS acesso
                             FROM cw_acessocampo a";

            INSERT = @"  INSERT INTO cw_acessocampo
							(codigo, idacesso, campo, display, acesso, incdata, inchora, incusuario)
							VALUES
							(@codigo, @idacesso, @campo, @display, @acesso, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE cw_acessocampo SET 
							 idacesso = @idacesso
                            , codigo = @codigo
							, campo = @campo
                            , display = @display
							, acesso = @acesso
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM cw_acessocampo WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM cw_acessocampo";
        }

        #region Singleton

        private static volatile SQL.Cw_AcessoCampo _instancia = null;

        public static SQL.Cw_AcessoCampo GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(SQL.Cw_AcessoCampo))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new SQL.Cw_AcessoCampo();
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
                    AtribuiCw_AcessoCampo(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Cw_AcessoCampo();
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

        private static void AtribuiCw_AcessoCampo(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            ((Modelo.Cw_AcessoCampo)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Cw_AcessoCampo)obj).IdAcesso = Convert.ToInt32(dr["idacesso"]);
            ((Modelo.Cw_AcessoCampo)obj).Campo = Convert.ToString(dr["campo"]);
            ((Modelo.Cw_AcessoCampo)obj).Display = Convert.ToString(dr["display"]);
            ((Modelo.Cw_AcessoCampo)obj).Acesso = Convert.ToBoolean(dr["acesso"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idacesso", SqlDbType.Int),
                new SqlParameter ("@campo", SqlDbType.VarChar),
                new SqlParameter ("@display", SqlDbType.VarChar),
				new SqlParameter ("@acesso", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.Cw_AcessoCampo)obj).Codigo;
            parms[2].Value = ((Modelo.Cw_AcessoCampo)obj).IdAcesso;
            parms[3].Value = ((Modelo.Cw_AcessoCampo)obj).Campo;
            parms[4].Value = ((Modelo.Cw_AcessoCampo)obj).Display;
            parms[5].Value = ((Modelo.Cw_AcessoCampo)obj).Acesso;
            parms[6].Value = ((Modelo.Cw_AcessoCampo)obj).Incdata;
            parms[7].Value = ((Modelo.Cw_AcessoCampo)obj).Inchora;
            parms[8].Value = ((Modelo.Cw_AcessoCampo)obj).Incusuario;
            parms[9].Value = ((Modelo.Cw_AcessoCampo)obj).Altdata;
            parms[10].Value = ((Modelo.Cw_AcessoCampo)obj).Althora;
            parms[11].Value = ((Modelo.Cw_AcessoCampo)obj).Altusuario;
        }

        public Modelo.Cw_AcessoCampo LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Cw_AcessoCampo objCw_AcessoCampo = new Modelo.Cw_AcessoCampo();
            try
            {
                SetInstance(dr, objCw_AcessoCampo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_AcessoCampo;
        }

        public bool PossuiRegistro(int pIdAcesso, string pCampo)
        {
            SqlParameter[] parms = new SqlParameter[2] 
            { 
                  new SqlParameter("@idacesso", SqlDbType.Int, 4)
                , new SqlParameter("@campo", SqlDbType.VarChar)
            };
            parms[0].Value = pIdAcesso;
            parms[1].Value = pCampo;

            string aux = @" SELECT ISNULL(COUNT(id),0) as qtd 
                            FROM cw_acessocampo 
                            WHERE idacesso = @idacesso AND campo = @campo";

            int qtd = (int)SQL.DataBase.ExecuteScalar(CommandType.Text, aux, parms);

            if (qtd > 0)
                return true;
            else
                return false;
        }

        public bool PossuiAcesso(int pIdAcesso, string pCampo)
        {
            SqlParameter[] parms = new SqlParameter[2] 
            { 
                  new SqlParameter("@idacesso", SqlDbType.Int, 4)
                , new SqlParameter("@campo", SqlDbType.VarChar)
            };
            parms[0].Value = pIdAcesso;
            parms[1].Value = pCampo;

            string aux = @"SELECT acesso FROM cw_acessocampo WHERE idacesso = @idacesso AND campo = @campo";

            int acesso = (int)SQL.DataBase.ExecuteScalar(CommandType.Text, aux, parms);

            if (acesso == 0)
                return false;
            else
                return true;
        }

        public List<Modelo.Cw_AcessoCampo> getListaCampos(int pIdAcesso)
        {
            List<Modelo.Cw_AcessoCampo> Campos = new List<Modelo.Cw_AcessoCampo>();

            SqlParameter[] parms = new SqlParameter[1] 
            { 
                  new SqlParameter("@idacesso", SqlDbType.Int, 4)
            };
            parms[0].Value = pIdAcesso;

            string aux = @"SELECT * FROM cw_acessocampo WHERE idacesso = @idacesso";

            SqlDataReader dr = SQL.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Cw_AcessoCampo objCw_AcessoCampo = new Modelo.Cw_AcessoCampo();
                    try
                    {
                        SetInstanceBase(dr, objCw_AcessoCampo);
                        AtribuiCw_AcessoCampo(dr, objCw_AcessoCampo);
                        objCw_AcessoCampo.Acao = Modelo.Acao.Consultar;
                        Campos.Add(objCw_AcessoCampo);
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }                 
                }
            }

            return Campos;
        }

        #endregion
    }
}