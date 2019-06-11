using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Justificativa : DAL.FB.DALBase, DAL.IJustificativa
    {

        private Justificativa()
        {
            GEN = "GEN_justificativa_id";

            TABELA = "justificativa";

            SELECTPID = "SELECT * FROM \"justificativa\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"justificativa\".\"id\" "+
                               ", \"justificativa\".\"descricao\" "+
                               ", \"justificativa\".\"codigo\" FROM \"justificativa\"";

            INSERT = "  INSERT INTO \"justificativa\"" +
                                        "(\"codigo\", \"descricao\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"justificativa\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"justificativa\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"justificativa\"";

        }

        #region Singleton

        private static volatile FB.Justificativa _instancia = null;

        public static FB.Justificativa GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Justificativa))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Justificativa();
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
                    ((Modelo.Justificativa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.Justificativa)obj).Descricao = Convert.ToString(dr["descricao"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Justificativa();
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
            parms[1].Value = ((Modelo.Justificativa)obj).Codigo;
            parms[2].Value = ((Modelo.Justificativa)obj).Descricao;
            parms[3].Value = ((Modelo.Justificativa)obj).Incdata;
            parms[4].Value = ((Modelo.Justificativa)obj).Inchora;
            parms[5].Value = ((Modelo.Justificativa)obj).Incusuario;
            parms[6].Value = ((Modelo.Justificativa)obj).Altdata;
            parms[7].Value = ((Modelo.Justificativa)obj).Althora;
            parms[8].Value = ((Modelo.Justificativa)obj).Altusuario;
        }

        public Modelo.Justificativa LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
            try
            {

                SetInstance(dr, objJustificativa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJustificativa;
        }

        public Modelo.Justificativa LoadObjectByCodigo(int pCodigo)
        {
            throw new NotImplementedException();
        }

        public bool BuscaJustificativa(string pNomeDescricao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@descricao", FbDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = " SELECT COUNT (\"id\") as \"quantidade\"" +
                            " FROM \"justificativa\"" +
                            " WHERE \"descricao\" = @descricao";

            int valor = (int)DataBase.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        public List<Modelo.Justificativa> GetAllList()
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Justificativa> GetAllPorExibePaineldoRH()
        {
            throw new NotImplementedException();
        }
        #endregion

        public int? GetIdPorCod(int Cod)
        {
            throw new NotImplementedException();
        }

        public int GetIdPorIdIntegracao(int IdIntegracao)
        {
            throw new NotImplementedException();
        }


        public Modelo.Justificativa LoadObjectByDescricao(string descricao)
        {
            throw new NotImplementedException();
        }

        public Modelo.Justificativa LoadObjectParaColetor()
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

        public List<Modelo.Justificativa> GetAllListPorIds(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }
    }
}
