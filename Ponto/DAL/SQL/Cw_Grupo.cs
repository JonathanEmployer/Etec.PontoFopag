using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class Cw_Grupo : DAL.SQL.DALBase, DAL.ICw_Grupo
    {

        private Cw_Grupo()
        {

            TABELA = "cw_grupo";

            SELECTPID = @"   SELECT * FROM cw_grupo WHERE id = @id";

            SELECTALL = @"   SELECT   id
                                    , nome
                                    , codigo
                                    , (case when acesso = 1 then 'Sim' when acesso = 0 then 'Não' end) AS acesso
                             FROM cw_grupo";

            INSERT = @"  INSERT INTO cw_grupo
							(codigo, nome, acesso, incdata, inchora, incusuario)
							VALUES
							(@codigo, @nome, @acesso, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE cw_grupo SET 
							 codigo = @codigo
							, nome = @nome
							, acesso = @acesso
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM cw_grupo WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM cw_grupo";

        }

        #region Singleton

        private static volatile SQL.Cw_Grupo _instancia = null;

        public static SQL.Cw_Grupo GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(SQL.Cw_Grupo))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new SQL.Cw_Grupo();
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
                    AtribuiDadosGrupo(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Cw_Grupo();
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

        private void AtribuiDadosGrupo(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Cw_Grupo)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Cw_Grupo)obj).Nome = Convert.ToString(dr["nome"]);
            ((Modelo.Cw_Grupo)obj).Acesso = Convert.ToInt32(dr["acesso"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@nome", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.Cw_Grupo)obj).Codigo;
            parms[2].Value = ((Modelo.Cw_Grupo)obj).Nome;
            parms[3].Value = ((Modelo.Cw_Grupo)obj).Acesso;
            parms[4].Value = ((Modelo.Cw_Grupo)obj).Incdata;
            parms[5].Value = ((Modelo.Cw_Grupo)obj).Inchora;
            parms[6].Value = ((Modelo.Cw_Grupo)obj).Incusuario;
            parms[7].Value = ((Modelo.Cw_Grupo)obj).Altdata;
            parms[8].Value = ((Modelo.Cw_Grupo)obj).Althora;
            parms[9].Value = ((Modelo.Cw_Grupo)obj).Altusuario;
        }

        public Modelo.Cw_Grupo LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Cw_Grupo objCw_Grupo = new Modelo.Cw_Grupo();
            try
            {
                SetInstance(dr, objCw_Grupo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_Grupo;
        }

        public List<Modelo.Cw_Grupo> getListaGrupo()
        {
            List<Modelo.Cw_Grupo> grupos = new List<Modelo.Cw_Grupo>();

            SqlParameter[] parms = new SqlParameter[0];
            string aux = @"SELECT id, codigo, nome FROM cw_grupo";

            SqlDataReader dr = SQL.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Cw_Grupo objGrupo = new Modelo.Cw_Grupo();
                    AtribuiDadosGrupo(dr, objGrupo);

                    grupos.Add(objGrupo);
                }
            }
            dr.Close();

            return grupos;
        }

        #endregion
    }
}
