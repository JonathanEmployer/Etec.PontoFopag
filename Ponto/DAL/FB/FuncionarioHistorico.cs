using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class FuncionarioHistorico : DAL.FB.DALBase, DAL.IFuncionarioHistorico
    {
        public string SELECTPFUN { get; set; }
        public string SELECTREL { get; set; }

        private FuncionarioHistorico()
        {
            GEN = "GEN_funcionariohistorico_id";

            TABELA = "funcionariohistorico";

            SELECTPID = "SELECT * FROM \"funcionariohistorico\" WHERE \"id\" = @id";

            SELECTALL = "SELECT * FROM \"funcionariohistorico\"";

            SELECTPFUN = "  SELECT * FROM \"funcionariohistorico\" WHERE \"idfuncionario\" = @idfuncionario";

            SELECTREL = "   SELECT   \"funcionariohistorico\".\"id\"" + 
                                    ", \"empresa\".\"nome\" AS empresa" + 
                                    ", \"departamento\".\"descricao\" AS departamento" + 
                                    ", \"funcionario\".\"nome\" AS funcionario" + 
                                    ", \"funcionariohistorico\".\"data\"" + 
                                    ", \"funcionariohistorico\".\"hora\"" + 
                                    ", \"funcionariohistorico\".\"historico\""+ 
                             " FROM \"funcionariohistorico\"" + 
                             " INNER JOIN \"funcionario\"  ON \"funcionario\".\"id\" = \"funcionariohistorico\".\"idfuncionario\"" + 
                             " LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" + 
                             " LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\"" +
                             " WHERE \"funcionariohistorico\".\"id\" > 0 ";

            INSERT = "  INSERT INTO \"funcionariohistorico\"" +
                                        "(\"codigo\", \"idfuncionario\", \"data\", \"hora\", \"historico\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @idfuncionario, @data, @hora, @historico, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"funcionariohistorico\" SET \"codigo\" = @codigo " +
                                        ", \"idfuncionario\" = @idfuncionario " +
                                        ", \"data\" = @data " +
                                        ", \"hora\" = @hora " +
                                        ", \"historico\" = @historico " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"funcionariohistorico\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"funcionariohistorico\"";

        }

        #region Singleton

        private static volatile FB.FuncionarioHistorico _instancia = null;

        public static FB.FuncionarioHistorico GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.FuncionarioHistorico))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.FuncionarioHistorico();
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
                obj = new Modelo.FuncionarioHistorico();
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
            ((Modelo.FuncionarioHistorico)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.FuncionarioHistorico)obj).Idfuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.FuncionarioHistorico)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.FuncionarioHistorico)obj).Hora = Convert.ToDateTime(dr["hora"]);
            ((Modelo.FuncionarioHistorico)obj).Historico = Convert.ToString(dr["historico"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@idfuncionario", FbDbType.Integer),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@hora", FbDbType.Date),
				new FbParameter ("@historico", FbDbType.VarChar),
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
            parms[1].Value = ((Modelo.FuncionarioHistorico)obj).Codigo;
            parms[2].Value = ((Modelo.FuncionarioHistorico)obj).Idfuncionario;
            parms[3].Value = ((Modelo.FuncionarioHistorico)obj).Data;
            parms[4].Value = ((Modelo.FuncionarioHistorico)obj).Hora;
            parms[5].Value = ((Modelo.FuncionarioHistorico)obj).Historico;
            parms[6].Value = ((Modelo.FuncionarioHistorico)obj).Incdata;
            parms[7].Value = ((Modelo.FuncionarioHistorico)obj).Inchora;
            parms[8].Value = ((Modelo.FuncionarioHistorico)obj).Incusuario;
            parms[9].Value = ((Modelo.FuncionarioHistorico)obj).Altdata;
            parms[10].Value = ((Modelo.FuncionarioHistorico)obj).Althora;
            parms[11].Value = ((Modelo.FuncionarioHistorico)obj).Altusuario;
        }

        public Modelo.FuncionarioHistorico LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.FuncionarioHistorico objFuncionarioHistorico = new Modelo.FuncionarioHistorico();
            try
            {
                SetInstance(dr, objFuncionarioHistorico);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFuncionarioHistorico;
        }

        public List<Modelo.FuncionarioHistorico> LoadPorFuncionario(int idFuncionario)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idfuncionario", FbDbType.Integer, 4) };
            parms[0].Value = idFuncionario;

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, SELECTPFUN, parms);

            List<Modelo.FuncionarioHistorico> lista = new List<Modelo.FuncionarioHistorico>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FuncionarioHistorico objFuncionarioHistorico = new Modelo.FuncionarioHistorico();
                    AuxSetInstance(dr, objFuncionarioHistorico);
                    lista.Add(objFuncionarioHistorico);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public DataTable LoadRelatorio(DateTime dataInicial, DateTime dataFinal, int tipo, string empresas, string departamentos, string funcionarios)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@datainicial", FbDbType.Date),
                new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = DBNull.Value;
            parms[1].Value = DBNull.Value;

            DataTable dt = new DataTable();
            string aux = SELECTREL;

            if (dataInicial != new DateTime() && dataFinal != new DateTime())
            {
                parms[0].Value = dataInicial;
                parms[1].Value = dataFinal;
                aux += " AND \"funcionariohistorico\".\"data\" >= @datainicial AND \"funcionariohistorico\".\"data\" <= @datafinal ";
            }
            else if (dataInicial != new DateTime())
            {
                parms[0].Value = dataInicial;
                aux += " AND \"funcionariohistorico\".\"data\" >= @datainicial ";
            }
            else if (dataFinal != new DateTime())
            {
                parms[1].Value = dataFinal;
                aux += " AND \"funcionariohistorico\".\"data\" <= @datafinal ";
            }

            switch (tipo)
            {
                case 0:
                    aux += " AND \"empresa\".\"id\" IN " + empresas;
                    break;
                case 1:
                    aux += " AND \"empresa\".\"id\" IN " + empresas + " AND \"departamento\".\"id\" IN " + departamentos;
                    break;
                case 2:
                    aux += " AND \"funcionario\".\"id\" IN " + funcionarios;
                    break;
            }

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
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
        #endregion
    }
}
