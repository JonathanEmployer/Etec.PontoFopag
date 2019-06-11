using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Funcao : DAL.FB.DALBase, IFuncao
    {

        private Funcao()
        {
            GEN = "GEN_funcao_id";

            TABELA = "funcao";

            SELECTPID = "SELECT * FROM \"funcao\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"funcao\".\"id\" "+
                               ", \"funcao\".\"descricao\" "+
                               ", \"funcao\".\"codigo\" FROM \"funcao\"";

            INSERT = "  INSERT INTO \"funcao\"" +
                                        "(\"codigo\", \"descricao\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"funcao\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"funcao\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"funcao\"";

        }

        #region Singleton

        private static volatile FB.Funcao _instancia = null;

        public static FB.Funcao GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Funcao))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Funcao();
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
                    ((Modelo.Funcao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.Funcao)obj).Descricao = Convert.ToString(dr["descricao"]);
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

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
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
            parms[1].Value = ((Modelo.Funcao)obj).Codigo;
            parms[2].Value = ((Modelo.Funcao)obj).Descricao;
            parms[3].Value = ((Modelo.Funcao)obj).Incdata;
            parms[4].Value = ((Modelo.Funcao)obj).Inchora;
            parms[5].Value = ((Modelo.Funcao)obj).Incusuario;
            parms[6].Value = ((Modelo.Funcao)obj).Altdata;
            parms[7].Value = ((Modelo.Funcao)obj).Althora;
            parms[8].Value = ((Modelo.Funcao)obj).Altusuario;
        }

        public Modelo.Funcao LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

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
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@descricao", FbDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = " SELECT COUNT (\"id\") as \"quantidade\"" +
                            " FROM \"funcao\"" +
                            " WHERE \"descricao\" = @descricao";

            int valor = (int)DataBase.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        public int? getFuncaoNome(string pNomeDescricao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@descricao", FbDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = " SELECT \"id\" " +
                            " FROM \"funcao\"" +
                            " WHERE UPPER(\"descricao\") = UPPER(@descricao)";

            int? valor = (int?)DataBase.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor;
        }

        public int? GetIdPorIdIntegracao(int? idIntegracao)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Funcao> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.Funcao> lista = new List<Modelo.Funcao>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Funcao objDepartamento = new Modelo.Funcao();
                    SetInstance(dr, objDepartamento);
                    lista.Add(objDepartamento);
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

        #region IFuncao Members


        public int? GetIdPorCod(int Cod)
        {
            throw new NotImplementedException();
        }

        #endregion


        public Modelo.Funcao LoadObjectByCodigo(int idFuncao)
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
