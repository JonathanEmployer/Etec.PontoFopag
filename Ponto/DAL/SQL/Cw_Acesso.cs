using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class Cw_Acesso : DAL.SQL.DALBase, DAL.ICw_Acesso
    {

        private Cw_Acesso()
        {
            TABELA = "cw_acesso";

            SELECTPID = @"   SELECT * FROM cw_acesso WHERE id = @id";

            SELECTALL = @"   SELECT   a.id
                                    , a.codigo
                                    , a.formulario
                                    , g.nome as grupo
                                    , (case when a.acesso = 1 then 'Sim' when a.acesso = 0 then 'Não' end) AS acesso
                             FROM cw_acesso a
                             INNER JOIN cw_grupo g ON g.id = a.idgrupo";

            INSERT = @"  INSERT INTO cw_acesso
							(codigo, idgrupo, formulario, acesso, incdata, inchora, incusuario)
							VALUES
							(@codigo, @idgrupo, @formulario, @acesso, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE cw_acesso SET 
							 codigo = @codigo
							, idgrupo = @idgrupo
                            , formulario = @formulario
							, acesso = @acesso
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM cw_acesso WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM cw_acesso";

        }

        #region Singleton

        private static volatile SQL.Cw_Acesso _instancia = null;

        public static SQL.Cw_Acesso GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(SQL.Cw_Acesso))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new SQL.Cw_Acesso();
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
            DAL.SQL.Cw_AcessoCampo dalAcessoCampo = SQL.Cw_AcessoCampo.GetInstancia;

            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.Cw_Acesso)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.Cw_Acesso)obj).IdGrupo = Convert.ToInt32(dr["idgrupo"]);
                    ((Modelo.Cw_Acesso)obj).Formulario = Convert.ToString(dr["formulario"]);
                    ((Modelo.Cw_Acesso)obj).Acesso = Convert.ToBoolean(dr["acesso"]);

                    if (((Modelo.Cw_Acesso)obj).Campos == null)
                    {
                        ((Modelo.Cw_Acesso)obj).Campos = new List<Modelo.Cw_AcessoCampo>();
                    }

                    ((Modelo.Cw_Acesso)obj).Campos = dalAcessoCampo.getListaCampos(obj.Id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Cw_Acesso();
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
				new SqlParameter ("@idgrupo", SqlDbType.Int),
                new SqlParameter ("@formulario", SqlDbType.VarChar),
				new SqlParameter ("@acesso", SqlDbType.TinyInt),
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
            parms[1].Value = ((Modelo.Cw_Acesso)obj).Codigo;
            parms[2].Value = ((Modelo.Cw_Acesso)obj).IdGrupo;
            parms[3].Value = ((Modelo.Cw_Acesso)obj).Formulario;
            parms[4].Value = ((Modelo.Cw_Acesso)obj).Acesso;
            parms[5].Value = ((Modelo.Cw_Acesso)obj).Incdata;
            parms[6].Value = ((Modelo.Cw_Acesso)obj).Inchora;
            parms[7].Value = ((Modelo.Cw_Acesso)obj).Incusuario;
            parms[8].Value = ((Modelo.Cw_Acesso)obj).Altdata;
            parms[9].Value = ((Modelo.Cw_Acesso)obj).Althora;
            parms[10].Value = ((Modelo.Cw_Acesso)obj).Altusuario;
        }

        public Modelo.Cw_Acesso LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Cw_Acesso objCw_Acesso = new Modelo.Cw_Acesso();
            try
            {
                SetInstance(dr, objCw_Acesso);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_Acesso;
        }

        public Modelo.Cw_Acesso LoadObject(int pIdGrupo, string pFormulario)
        {
            SqlParameter[] parms = new SqlParameter[2] 
            { 
                  new SqlParameter("@idgrupo", SqlDbType.Int, 4)
                , new SqlParameter("@formulario", SqlDbType.VarChar)
            };
            parms[0].Value = pIdGrupo;
            parms[1].Value = pFormulario;

            string aux = @"SELECT id FROM cw_acesso WHERE idgrupo = @idgrupo AND formulario = @formulario";

            int id = (int)SQL.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
            SqlDataReader dr = LoadDataReader(id);
            Modelo.Cw_Acesso objCw_Acesso = new Modelo.Cw_Acesso();

            try
            {
                SetInstance(dr, objCw_Acesso);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_Acesso;
        }

        public bool PossuiRegistro(int pIdGrupo, string pFormulario)
        {
            SqlParameter[] parms = new SqlParameter[2] 
            { 
                  new SqlParameter("@idgrupo", SqlDbType.Int, 4)
                , new SqlParameter("@formulario", SqlDbType.VarChar)
            };
            parms[0].Value = pIdGrupo;
            parms[1].Value = pFormulario;

            string aux = @"SELECT ISNULL(COUNT(codigo),0) as Qtd 
                            FROM cw_acesso WHERE idgrupo = @idgrupo AND formulario = @formulario";

            int qtd = (int)SQL.DataBase.ExecuteScalar(CommandType.Text, aux, parms);

            if (qtd > 0)
                return true;
            else
                return false;
        }

        public bool PossuiAcesso(int pIdGrupo, string pFormulario)
        {
            //Caso o tipo do usuário for Supervisor o sistema libera o acesso
            if (Modelo.Global.objUsuarioLogado.Tipo == 0)
                return true;

            SqlParameter[] parms = new SqlParameter[2] 
            { 
                  new SqlParameter("@idgrupo", SqlDbType.Int, 4)
                , new SqlParameter("@formulario", SqlDbType.VarChar)
            };
            parms[0].Value = pIdGrupo;
            parms[1].Value = pFormulario;

            string aux = @"SELECT acesso FROM cw_acesso WHERE idgrupo = @idgrupo AND formulario = @formulario";

            int qtd = (int)SQL.DataBase.ExecuteScalar(CommandType.Text, aux, parms);

            if (qtd == 0)
                return false;
            else
                return true;
        }

        public override void Alterar(Modelo.ModeloBase obj)
        {
            DAL.SQL.Cw_AcessoCampo dalAcessoCampo = SQL.Cw_AcessoCampo.GetInstancia;

            using (SqlConnection conn = new SqlConnection(SQL.DataBase.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AlterarAux(trans, obj);

                        foreach (Modelo.Cw_AcessoCampo campo in ((Modelo.Cw_Acesso)obj).Campos)
                        {
                            if(campo.Acao!= Modelo.Acao.Alterar)
                                continue;

                            dalAcessoCampo.Alterar(trans, campo);
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        #endregion
    }
}