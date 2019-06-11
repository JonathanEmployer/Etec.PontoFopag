using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Data.Common;
using System.Collections.Generic;
using Modelo;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class Feriado : DAL.FB.DALBase, DAL.IFeriado
    {

        private Feriado()
        {
            GEN = "GEN_feriado_id";

            TABELA = "feriado";

            SELECTPID = "SELECT * FROM \"feriado\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"feriado\".\"id\" " +
                               ", \"feriado\".\"descricao\" " +
                               ", \"feriado\".\"codigo\" " +
                               ", \"feriado\".\"data\" " +
                               ", case when \"feriado\".\"tipoferiado\" = 0 then 'Geral' when \"feriado\".\"tipoferiado\" = 1 then 'Empresa' when \"feriado\".\"tipoferiado\" = 2 then 'Departamento' end AS \"tipo\" " +
                               ", case when \"tipoferiado\" = 0 then '' " +
                               "       when \"tipoferiado\" = 2 then (SELECT \"departamento\".\"descricao\" FROM \"departamento\" WHERE \"departamento\".\"id\" = \"feriado\".\"iddepartamento\") " +
                               "       when \"tipoferiado\" = 1 then (SELECT \"empresa\".\"nome\" FROM \"empresa\" WHERE \"empresa\".\"id\" = \"feriado\".\"idempresa\") end AS \"nome\" " +
                        " FROM \"feriado\"";

            INSERT = "  INSERT INTO \"feriado\"" +
                                        "(\"codigo\", \"descricao\", \"data\", \"tipoferiado\", \"idempresa\", \"iddepartamento\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @data, @tipoferiado, @idempresa, @iddepartamento, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"feriado\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"data\" = @data " +
                                        ", \"tipoferiado\" = @tipoferiado " +
                                        ", \"idempresa\" = @idempresa " +
                                        ", \"iddepartamento\" = @iddepartamento " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"feriado\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"feriado\"";

        }

        #region Singleton

        private static volatile FB.Feriado _instancia = null;

        public static FB.Feriado GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Feriado))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Feriado();
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
                obj = new Modelo.Feriado();
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
            ((Modelo.Feriado)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.Feriado)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Feriado)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Feriado)obj).Data = (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            ((Modelo.Feriado)obj).TipoFeriado = Convert.ToInt32(dr["tipoferiado"]);
            ((Modelo.Feriado)obj).IdEmpresa = (dr["idempresa"] is DBNull ? 0 : Convert.ToInt32(dr["idempresa"]));
            ((Modelo.Feriado)obj).IdDepartamento = (dr["iddepartamento"] is DBNull ? 0 : Convert.ToInt32(dr["iddepartamento"]));
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@tipoferiado", FbDbType.SmallInt),
				new FbParameter ("@idempresa", FbDbType.Integer),
				new FbParameter ("@iddepartamento", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.Feriado)obj).Codigo;
            parms[2].Value = ((Modelo.Feriado)obj).Descricao;
            parms[3].Value = ((Modelo.Feriado)obj).Data;
            parms[4].Value = ((Modelo.Feriado)obj).TipoFeriado;
            if (((Modelo.Feriado)obj).IdEmpresa != 0)
            {
                parms[5].Value = ((Modelo.Feriado)obj).IdEmpresa;
            }
            if (((Modelo.Feriado)obj).IdDepartamento != 0)
            {
                parms[6].Value = ((Modelo.Feriado)obj).IdDepartamento;
            }
            parms[7].Value = ((Modelo.Feriado)obj).Incdata;
            parms[8].Value = ((Modelo.Feriado)obj).Inchora;
            parms[9].Value = ((Modelo.Feriado)obj).Incusuario;
            parms[10].Value = ((Modelo.Feriado)obj).Altdata;
            parms[11].Value = ((Modelo.Feriado)obj).Althora;
            parms[12].Value = ((Modelo.Feriado)obj).Altusuario;
        }

        public Modelo.Feriado LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Feriado objFeriado = new Modelo.Feriado();
            try
            {

                SetInstance(dr, objFeriado);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFeriado;
        }

        /// <summary>
        /// Método responsável para retornar todos os registros de feriado em uma determinada data
        /// </summary>
        /// <param name="pData">Data para validação do Feriado</param>
        /// <returns>Retorna os feriados encontrados (SqlDataReader)</returns>
        public List<Modelo.Feriado> getFeriado(DateTime pData)
        {
            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();

            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@data", FbDbType.Date) 
            };
            parms[0].Value = pData;

            string aux = "SELECT * FROM \"feriado\" WHERE \"data\" = @data ORDER BY \"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
                }
            }
            dr.Close();
            return lista;
        }

        public List<Modelo.Feriado> getFeriado(DateTime pDataI, DateTime pDataF)
        {
            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();

            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date) 
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = "SELECT * FROM \"feriado\" WHERE \"data\" >= @datai AND \"data\" <= @dataf ORDER BY \"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
                }
            }
            dr.Close();
            return lista;
        }

        public bool BuscaFeriado(string pNomeDescricao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@descricao", FbDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = " SELECT COUNT (\"id\") as \"quantidade\"" +
                            " FROM \"feriado\"" +
                            " WHERE \"descricao\" = @descricao";

            int valor = (int)DataBase.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        public List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(int idFuncionario, DateTime inicio, DateTime fim)
        {
            throw new NotImplementedException();
        }

        #endregion


        public List<Modelo.Feriado> GetAllList()
        {
            List<Modelo.Feriado> lista = new List<Modelo.Feriado>();

            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@data", FbDbType.Date) 
            };

            string aux = "SELECT * FROM \"feriado\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    AuxSetInstance(dr, objFeriado);
                    lista.Add(objFeriado);
                }
            }
            dr.Close();
            return lista;
        }


        public List<Modelo.Feriado> GetIdPorIdIntegracao(int idIntegracao)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(List<int> idsFuncionarios, DateTime inicio, DateTime fim)
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
