using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class Inconsistencia : DAL.FB.DALBase, DAL.IInconsistencia
    {

        private Inconsistencia()
        {
            GEN = "GEN_Inconsistencia_id";

            TABELA = "Inconsistencia";

            SELECTPID = "SELECT * FROM \"Inconsistencia\" WHERE \"id\" = @id";

            SELECTALL = "SELECT   \"inconsistencia\".\"id\"" +
                               ", \"inconsistencia\".\"descricao\"" +
                               ", \"inconsistencia\".\"codigo\"" +
                        " FROM \"Inconsistencia\"";

            INSERT = "  INSERT INTO \"inconsistencia\"" +
                                        "(\"codigo\", \"descricao\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        " VALUES" +
                                        "(@codigo, @descricao, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"inconsistencia\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"inconsistencia\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"inconsistencia\"";

        }

        #region Singleton

        private static volatile FB.Inconsistencia _instancia = null;

        public static FB.Inconsistencia GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Inconsistencia))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Inconsistencia();
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
                obj = new Modelo.Inconsistencia();
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
            ((Modelo.Inconsistencia)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Inconsistencia)obj).Descricao = Convert.ToString(dr["descricao"]);
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
            parms[1].Value = ((Modelo.Inconsistencia)obj).Codigo;
            parms[2].Value = ((Modelo.Inconsistencia)obj).Descricao;
            parms[3].Value = ((Modelo.Inconsistencia)obj).Incdata;
            parms[4].Value = ((Modelo.Inconsistencia)obj).Inchora;
            parms[5].Value = ((Modelo.Inconsistencia)obj).Incusuario;
            parms[6].Value = ((Modelo.Inconsistencia)obj).Altdata;
            parms[7].Value = ((Modelo.Inconsistencia)obj).Althora;
            parms[8].Value = ((Modelo.Inconsistencia)obj).Altusuario;
        }

        public Modelo.Inconsistencia LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Inconsistencia objInconsistencia = new Modelo.Inconsistencia();
            try
            {

                SetInstance(dr, objInconsistencia);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objInconsistencia;
        }

        public Hashtable GetHashIdDescricao()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"Inconsistencia\"", parms);

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

        public List<Modelo.Inconsistencia> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"Inconsistencia\"", parms);

            List<Modelo.Inconsistencia> lista = new List<Modelo.Inconsistencia>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Inconsistencia objInconsistencia = new Modelo.Inconsistencia();
                    AuxSetInstance(dr, objInconsistencia);
                    lista.Add(objInconsistencia);
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

        #endregion
        public Modelo.Inconsistencia LoadObjectByCodigo(int pCodigo)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias> RelatorioInconsistencias(List<int> idsFuncs, string datainicial, string datafinal, List<bool> paramInconsistencia)
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
