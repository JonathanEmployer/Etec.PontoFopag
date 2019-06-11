using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Departamento : DAL.FB.DALBase, DAL.IDepartamento
    {
        public string SELECTPEM { get; set; }

        private string SqlGetAllPorCodigo()
        {
            return "SELECT * FROM \"departamento\" WHERE \"codigo\" = @codigo";
        }

        private Departamento()
        {
            GEN = "GEN_departamento_id";

            TABELA = "departamento";

            SELECTPID = "SELECT * FROM \"departamento\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"departamento\".\"id\"" +
                                ", \"departamento\".\"descricao\"" +
                                ", \"departamento\".\"codigo\"" +
                                ", \"empresa\".\"nome\" AS \"empresa\" " +
                        "FROM \"departamento\" " +
                        "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"departamento\".\"idempresa\" "+
                        "ORDER BY LOWER(\"departamento\".\"descricao\")";

            SELECTPEM = "SELECT \"departamento\".* "
                        + ", \"empresa\".\"nome\" AS \"empresa\" "
                        + " FROM \"departamento\""
                        + "INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"departamento\".\"idempresa\"";

            INSERT =    "INSERT INTO \"departamento\"" +
                                        "(\"codigo\", \"descricao\", \"idempresa\", \"incdata\", \"inchora\", \"incusuario\")" +
                        "VALUES" +
                                        "(@codigo, @descricao, @idempresa, @incdata, @inchora, @incusuario)";

            UPDATE =    "UPDATE \"departamento\" SET \"codigo\" = @codigo" +
                                        ", \"descricao\" = @descricao" +
                                        ", \"idempresa\" = @idempresa" +
                                        ", \"altdata\" = @altdata" +
                                        ", \"althora\" = @althora" +
                                        ", \"altusuario\" = @altusuario " +
                        "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"departamento\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"departamento\"";

        }

        #region Singleton

        private static volatile FB.Departamento _instancia = null;

        public static FB.Departamento GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Departamento))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Departamento();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        public Modelo.Departamento LoadObjectByCodigo(int codigo)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlGetAllPorCodigo();

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Departamento objDep = new Modelo.Departamento();
            SetInstance(dr, objDep);
            return objDep;
        }

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiDepartamento(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Departamento();
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

        private void AtribuiDepartamento(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Departamento)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Departamento)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Departamento)obj).IdEmpresa = Convert.ToInt32(dr["idempresa"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@idempresa", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.Departamento)obj).Codigo;
            parms[2].Value = ((Modelo.Departamento)obj).Descricao;
            parms[3].Value = ((Modelo.Departamento)obj).IdEmpresa;
            parms[4].Value = ((Modelo.Departamento)obj).Incdata;
            parms[5].Value = ((Modelo.Departamento)obj).Inchora;
            parms[6].Value = ((Modelo.Departamento)obj).Incusuario;
            parms[7].Value = ((Modelo.Departamento)obj).Altdata;
            parms[8].Value = ((Modelo.Departamento)obj).Althora;
            parms[9].Value = ((Modelo.Departamento)obj).Altusuario;
        }

        public Modelo.Departamento LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Departamento objDepartamento = new Modelo.Departamento();
            try
            {

                SetInstance(dr, objDepartamento);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objDepartamento;
        }

        public List<Modelo.Departamento> LoadPEmpresa(int IdEmpresa)
        {
            FbDataReader dr = GetPorEmpresaAux(IdEmpresa);

            List<Modelo.Departamento> lista = new List<Modelo.Departamento>();
            Modelo.Departamento objDepartamento;
            while (dr.Read())
            {
                objDepartamento = new Modelo.Departamento();

                AtribuiDepartamento(dr, objDepartamento);

                objDepartamento.Acao = Modelo.Acao.Consultar;

                lista.Add(objDepartamento);
            }
            return lista;
        }

        private FbDataReader GetPorEmpresaAux(int IdEmpresa)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idempresa", FbDbType.Integer, 4) };
            parms[0].Value = IdEmpresa;

            return DAL.FB.DataBase.ExecuteReader(CommandType.Text, SELECTPEM + " WHERE \"idempresa\" = @idempresa ORDER BY LOWER(\"departamento\".\"descricao\")" , parms);
        }

        public DataTable GetPorEmpresa(int IdEmpresa)
        {
            DataTable dt = new DataTable();
            dt.Load(GetPorEmpresaAux(IdEmpresa));

            return dt;
        }

        public DataTable GetPorEmpresa(string empresas)
        {
            FbParameter[] parms = new FbParameter[0];            
            DataTable dt = new DataTable();
            dt.Load(DAL.FB.DataBase.ExecuteReader(CommandType.Text, SELECTPEM + " WHERE \"idempresa\" IN " + empresas, parms));

            return dt;
        }

        public List<Modelo.Departamento> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"departamento\"", parms);

            List<Modelo.Departamento> lista = new List<Modelo.Departamento>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Departamento objDepartamento = new Modelo.Departamento();
                    AtribuiDepartamento(dr, objDepartamento);
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
            }
            return lista;
        }

        public List<Modelo.Departamento> GetAllListLike(string desc)
        {
            throw new NotImplementedException();
        }

        public int? GetIdPorIdIntegracao(int Cod)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDepartamento Members


        public int? GetIdPorDesc(string Descricao)
        {
            throw new NotImplementedException();
        }

        public int? GetIdPorCodigo(int Cod)
        {
            throw new NotImplementedException();
        }

        public bool PossuiFuncionarios(int id)
        {
            throw new NotImplementedException();
        }

        #endregion


        public Modelo.Departamento GetDepartamentoPadrao(string cnpj)
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

        public List<Modelo.Departamento> GetAllListByIds(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIdsPorCodigos(List<int> codigos)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }
    }
}
