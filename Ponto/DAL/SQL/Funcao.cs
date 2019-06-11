using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class Funcao : DAL.SQL.DALBase, DAL.IFuncao
    {

        public Funcao(DataBase database)
        {
            db = database;
            TABELA = "funcao";

            SELECTPID = @"   SELECT * FROM funcao WHERE id = @id";

            SELECTALL = @"   SELECT   funcao.id
                                    , funcao.descricao
                                    , funcao.codigo
                                    , funcao.idIntegracao
                             FROM funcao";

            INSERT = @"  INSERT INTO funcao
							(codigo, descricao, incdata, inchora, incusuario, idIntegracao)
							VALUES
							(@codigo, @descricao, @incdata, @inchora, @incusuario, @idIntegracao) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE funcao SET
							  codigo = @codigo
							, descricao = @descricao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idIntegracao = @idIntegracao
						WHERE id = @id";

            DELETE = @"  DELETE FROM funcao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM funcao";

        }

        public string SqlLoadByCodigo()
        {
            return @"   SELECT * FROM funcao WHERE codigo = @codigo";
        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.Funcao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.Funcao)obj).Descricao = Convert.ToString(dr["descricao"]);
                    object val = dr["idIntegracao"];
                    Int32? idint = (val == null || val is DBNull) ? (Int32?)null : (Int32?)val;
                    ((Modelo.Funcao)obj).idIntegracao = idint;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Funcao();
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

        private void AtribuiFuncao(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Funcao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Funcao)obj).Descricao = Convert.ToString(dr["descricao"]);
            object val = dr["idIntegracao"];
            Int32? idint = (val == null || val is DBNull) ? (Int32?)null : (Int32?)val;
            ((Modelo.Funcao)obj).idIntegracao = idint;
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@idIntegracao", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.Funcao)obj).Codigo;
            parms[2].Value = ((Modelo.Funcao)obj).Descricao;
            parms[3].Value = ((Modelo.Funcao)obj).Incdata;
            parms[4].Value = ((Modelo.Funcao)obj).Inchora;
            parms[5].Value = ((Modelo.Funcao)obj).Incusuario;
            parms[6].Value = ((Modelo.Funcao)obj).Altdata;
            parms[7].Value = ((Modelo.Funcao)obj).Althora;
            parms[8].Value = ((Modelo.Funcao)obj).Altusuario;
            parms[9].Value = ((Modelo.Funcao)obj).idIntegracao;
        }

        public Modelo.Funcao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Funcao objFuncao = new Modelo.Funcao();
            try
            {
                SetInstance(dr, objFuncao);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFuncao;
        }

        public bool BuscaFuncao(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = @" SELECT COUNT (id) as quantidade
                            FROM funcao
                            WHERE descricao = @descricao";

            int valor = (int)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        public int? getFuncaoNome(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = " SELECT id " +
                            " FROM funcao" +
                            " WHERE UPPER(descricao) = UPPER(@descricao)";

            int? valor = (int?)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor;
        }

        public int? GetIdPorCod(int Cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from funcao where codigo = " + Cod, parms));

            return Id;
        }

        public int? GetIdPorIdIntegracao(int? idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from funcao where idIntegracao = " + idIntegracao, parms));
            return Id;
        }

        public List<Modelo.Funcao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string cmd = " SELECT * " +
                            " FROM funcao";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.Funcao> lista = new List<Modelo.Funcao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Funcao objFuncao = new Modelo.Funcao();
                    AtribuiFuncao(dr, objFuncao);
                    lista.Add(objFuncao);
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

        #endregion

        public Modelo.Funcao LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlLoadByCodigo();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Funcao objFunc = new Modelo.Funcao();
            SetInstance(dr, objFunc);
            return objFunc;
        }
    }
}
