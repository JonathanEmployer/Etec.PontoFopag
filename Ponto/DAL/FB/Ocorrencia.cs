using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class Ocorrencia : DAL.FB.DALBase, DAL.IOcorrencia
    {

        private Ocorrencia()
        {
            GEN = "GEN_ocorrencia_id";

            TABELA = "ocorrencia";

            SELECTPID = "SELECT * FROM \"ocorrencia\" WHERE \"id\" = @id";

            SELECTALL = "SELECT   \"ocorrencia\".\"id\"" + 
                               ", \"ocorrencia\".\"descricao\"" +
                               ", \"ocorrencia\".\"codigo\"" +
                        " FROM \"ocorrencia\"";

            INSERT = "  INSERT INTO \"ocorrencia\"" +
                                        "(\"codigo\", \"descricao\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        " VALUES" +
                                        "(@codigo, @descricao, @incdata, @inchora, @incusuario)";                   

            UPDATE = "  UPDATE \"ocorrencia\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"ocorrencia\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"ocorrencia\"";

        }

        #region Singleton

        private static volatile FB.Ocorrencia _instancia = null;

        public static FB.Ocorrencia GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Ocorrencia))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Ocorrencia();
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
                    AuxSetInstance(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Ocorrencia();
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

        private void AuxSetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Ocorrencia)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Ocorrencia)obj).Descricao = Convert.ToString(dr["descricao"]);
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
            parms[1].Value = ((Modelo.Ocorrencia)obj).Codigo;
            parms[2].Value = ((Modelo.Ocorrencia)obj).Descricao;
            parms[3].Value = ((Modelo.Ocorrencia)obj).Incdata;
            parms[4].Value = ((Modelo.Ocorrencia)obj).Inchora;
            parms[5].Value = ((Modelo.Ocorrencia)obj).Incusuario;
            parms[6].Value = ((Modelo.Ocorrencia)obj).Altdata;
            parms[7].Value = ((Modelo.Ocorrencia)obj).Althora;
            parms[8].Value = ((Modelo.Ocorrencia)obj).Altusuario;
        }

        public Modelo.Ocorrencia LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
            try
            {

                SetInstance(dr, objOcorrencia);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objOcorrencia;
        }

        public Hashtable GetHashIdDescricao()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"ocorrencia\"", parms);

            Hashtable lista = new Hashtable();
            try
            {
                while (dr.Read())
                {                    
                    lista.Add(Convert.ToInt32(dr["id"]), Convert.ToString(dr["descricao"]));
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
            }
            return lista;
        }

        public List<Modelo.Ocorrencia> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"ocorrencia\"", parms);

            List<Modelo.Ocorrencia> lista = new List<Modelo.Ocorrencia>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
                    AuxSetInstance(dr, objOcorrencia);
                    lista.Add(objOcorrencia);
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
            }
            return lista;
        }

        public int? getOcorrenciaNome(string pDescricao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@descricao", FbDbType.VarChar)
            };
            parms[0].Value = pDescricao;

            string cmd = " SELECT \"id\" " +
                            " FROM \"ocorrencia\"" +
                            " WHERE UPPER(\"descricao\") = UPPER(@descricao)";

            int? valor = (int?)DataBase.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor;

        }

        #endregion
        public Modelo.Ocorrencia LoadObjectByCodigo(int pCodigo)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Proxy.pxyOcorrenciaEvento> GetAllOcorrenciaEventoList()
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Ocorrencia> GetAllListPorIds(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Ocorrencia> GetAllPorExibePainelRHPorEmpresa(int idEmpresa)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Ocorrencia> GetAllPorExibePaineldoRH()
        {
            throw new NotImplementedException();
        }

        public int? GetIdPorIdIntegracao(int idIntegracao)
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
    }
}
