using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class Alocacao : DAL.SQL.DALBase, DAL.IAlocacao
    {

        public Alocacao(DataBase database)
        {
            db = database;
            TABELA = "Alocacao";

            SELECTPID = @"   SELECT * FROM Alocacao WHERE id = @id";

            SELECTALL = @"   SELECT   Alocacao.id
                                    , Alocacao.descricao
                                    , Alocacao.codigo
                                    , Alocacao.Endereco
                             FROM Alocacao";

            INSERT = @"  INSERT INTO Alocacao
							(codigo, descricao, incdata, inchora, incusuario, endereco)
							VALUES
							(@codigo, @descricao, @incdata, @inchora, @incusuario, @endereco) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE Alocacao SET
							  codigo = @codigo
							, descricao = @descricao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , endereco = @endereco
						WHERE id = @id";

            DELETE = @"  DELETE FROM Alocacao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM Alocacao";

        }

        public string SqlLoadByCodigo()
        {
            return @"   SELECT * FROM Alocacao WHERE codigo = @codigo";
        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.Alocacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.Alocacao)obj).Descricao = Convert.ToString(dr["descricao"]);
                    ((Modelo.Alocacao)obj).Endereco = Convert.ToString(dr["endereco"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Alocacao();
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

        private void AtribuiAlocacao(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Alocacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Alocacao)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Alocacao)obj).Endereco = Convert.ToString(dr["endereco"]);
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
                new SqlParameter ("@endereco", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.Alocacao)obj).Codigo;
            parms[2].Value = ((Modelo.Alocacao)obj).Descricao;
            parms[3].Value = ((Modelo.Alocacao)obj).Incdata;
            parms[4].Value = ((Modelo.Alocacao)obj).Inchora;
            parms[5].Value = ((Modelo.Alocacao)obj).Incusuario;
            parms[6].Value = ((Modelo.Alocacao)obj).Altdata;
            parms[7].Value = ((Modelo.Alocacao)obj).Althora;
            parms[8].Value = ((Modelo.Alocacao)obj).Altusuario;
            parms[9].Value = ((Modelo.Alocacao)obj).Endereco;
        }

        public Modelo.Alocacao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Alocacao objAlocacao = new Modelo.Alocacao();
            try
            {

                SetInstance(dr, objAlocacao);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objAlocacao;
        }

        public bool BuscaAlocacao(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = @" SELECT COUNT (id) as quantidade
                            FROM Alocacao
                            WHERE descricao = @descricao";

            int valor = (int)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        public int? getAlocacaoNome(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = " SELECT id " +
                            " FROM Alocacao" +
                            " WHERE UPPER(descricao) = UPPER(@descricao)";

            int? valor = (int?)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor;
        }   

        public int? GetIdPorCod(int Cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from Alocacao where codigo = " + Cod, parms));

            return Id;
        }

        public int? GetIdPorIdIntegracao(int? idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from Alocacao where idIntegracao = " + idIntegracao, parms));
            return Id;
        }

        public List<Modelo.Alocacao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string cmd = " SELECT * " +
                            " FROM Alocacao";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.Alocacao> lista = new List<Modelo.Alocacao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Alocacao objAlocacao = new Modelo.Alocacao();
                    AtribuiAlocacao(dr, objAlocacao);
                    lista.Add(objAlocacao);
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

        public Modelo.Alocacao LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlLoadByCodigo();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Alocacao objFunc = new Modelo.Alocacao();
            SetInstance(dr, objFunc);
            return objFunc;
        }
    }
}
