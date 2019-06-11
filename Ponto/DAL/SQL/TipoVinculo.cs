using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class TipoVinculo : DAL.SQL.DALBase, DAL.ITipoVinculo
    {
        public TipoVinculo(DataBase database)
        {
            db = database;
            TABELA = "TipoVinculo";

            SELECTPID = @"   SELECT * FROM TipoVinculo WHERE id = @id";

            SELECTALL = @"   SELECT   TipoVinculo.*
                             FROM TipoVinculo";

            INSERT = @"  INSERT INTO TipoVinculo
							(codigo, descricao, incdata, inchora, incusuario)
							VALUES
							(@codigo, @descricao, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE TipoVinculo SET
							 descricao = @descricao
                           , altdata = @altdata
					       , althora = @althora
						   , altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM TipoVinculo WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM TipoVinculo";

        }

        public string SqlLoadByCodigo()
        {
            return @"   SELECT * FROM TipoVinculo WHERE codigo = @codigo";
        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.TipoVinculo)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.TipoVinculo)obj).Descricao = Convert.ToString(dr["descricao"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.TipoVinculo();
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

        private void AtribuiTipoVinculo(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.TipoVinculo)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.TipoVinculo)obj).Descricao = Convert.ToString(dr["descricao"]);
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
            parms[1].Value = ((Modelo.TipoVinculo)obj).Codigo;
            parms[2].Value = ((Modelo.TipoVinculo)obj).Descricao;
            parms[3].Value = ((Modelo.TipoVinculo)obj).Incdata;
            parms[4].Value = ((Modelo.TipoVinculo)obj).Inchora;
            parms[5].Value = ((Modelo.TipoVinculo)obj).Incusuario;
            parms[6].Value = ((Modelo.TipoVinculo)obj).Altdata;
            parms[7].Value = ((Modelo.TipoVinculo)obj).Althora;
            parms[8].Value = ((Modelo.TipoVinculo)obj).Altusuario;
        }

        public Modelo.TipoVinculo LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.TipoVinculo objTipoVinculo = new Modelo.TipoVinculo();
            try
            {

                SetInstance(dr, objTipoVinculo);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objTipoVinculo;
        }

        public bool BuscaTipoVinculo(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = @" SELECT COUNT (id) as quantidade
                            FROM TipoVinculo
                            WHERE descricao = @descricao";

            int valor = (int)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        public int? getTipoVinculoNome(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = " SELECT id " +
                            " FROM TipoVinculo" +
                            " WHERE UPPER(descricao) = UPPER(@descricao)";

            int? valor = (int?)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor;
        }   

        public int? GetIdPorCod(int Cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from TipoVinculo where codigo = " + Cod, parms));

            return Id;
        }

        public int? GetIdPorIdIntegracao(int? idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from TipoVinculo where idIntegracao = " + idIntegracao, parms));
            return Id;
        }

        public List<Modelo.TipoVinculo> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string cmd = " SELECT * " +
                            " FROM TipoVinculo";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.TipoVinculo> lista = new List<Modelo.TipoVinculo>();
            try
            {
                while (dr.Read())
                {
                    Modelo.TipoVinculo objTipoVinculo = new Modelo.TipoVinculo();
                    AtribuiTipoVinculo(dr, objTipoVinculo);
                    lista.Add(objTipoVinculo);
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

        public Modelo.TipoVinculo LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlLoadByCodigo();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.TipoVinculo objFunc = new Modelo.TipoVinculo();
            SetInstance(dr, objFunc);
            return objFunc;
        }
    }
}
