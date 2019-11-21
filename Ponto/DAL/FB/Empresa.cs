using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Empresa : DAL.FB.DALBase, DAL.IEmpresa
    {

        public string SELECTLIST { get; set; }

        private Empresa()
        {
            GEN = "GEN_empresa_id";

            TABELA = "empresa";

            SELECTPID = "SELECT * FROM \"empresa\" WHERE \"id\" = @id";

            SELECTALL = "SELECT \"empresa\".\"id\" " +
                                ", \"empresa\".\"nome\"" +
                                ", \"empresa\".\"codigo\"" +
                                ", case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj_cpf\"" +
                                ", \"empresa\".\"endereco\"" +
                                ", \"empresa\".\"cidade\" || ' - ' || \"empresa\".\"estado\" AS \"cidade\"" +
                                ", \"empresa\".\"cep\" " +
                                ", \"empresa\".\"cei\" " +
                                ", \"empresa\".\"validade\" " +
                                ", \"empresa\".\"ultimoacesso\" " +
                                "FROM \"empresa\"";


            SELECTLIST = " SELECT * FROM \"empresa\" ";

            INSERT = "  INSERT INTO \"empresa\"" +
                                        "(\"codigo\", \"bprincipal\", \"tipolicenca\", \"quantidade\", \"nome\", \"endereco\", \"cidade\", \"estado\", \"cep\", \"cnpj\", \"cpf\", \"chave\", \"incdata\", \"inchora\", \"incusuario\", \"cei\", \"numeroserie\", \"bdalterado\", \"validade\", \"ultimoacesso\")" +
                                        "VALUES" +
                                        "(@codigo, @bprincipal, @tipolicenca, @quantidade, @nome, @endereco, @cidade, @estado, @cep, @cnpj, @cpf, @chave, @incdata, @inchora, @incusuario, @cei, @numeroserie, @bdalterado, @validade, @ultimoacesso)";

            UPDATE = "  UPDATE \"empresa\" SET \"codigo\" = @codigo" +
                                        ", \"bprincipal\" = @bprincipal" +
                                        ", \"tipolicenca\" = @tipolicenca" +
                                        ", \"quantidade\" = @quantidade" +
                                        ", \"nome\" = @nome" +
                                        ", \"endereco\" = @endereco" +
                                        ", \"cidade\" = @cidade" +
                                        ", \"estado\" = @estado" +
                                        ", \"cep\" = @cep" +
                                        ", \"cnpj\" = @cnpj" +
                                        ", \"cpf\" = @cpf" +
                                        ", \"chave\" = @chave" +
                                        ", \"altdata\" = @altdata" +
                                        ", \"althora\" = @althora" +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"cei\" = @cei " +
                                        ", \"numeroserie\" = @numeroserie " +
                                        ", \"bdalterado\" = @bdalterado " +
                                        ", \"validade\" = @validade " +
                                        ", \"ultimoacesso\" = @ultimoacesso " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"empresa\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"empresa\"";

        }

        #region Singleton

        private static volatile FB.Empresa _instancia = null;

        public static FB.Empresa GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Empresa))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Empresa();
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
                obj = new Modelo.Empresa();
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
            ((Modelo.Empresa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Empresa)obj).bPrincipal = Convert.ToBoolean(dr["bprincipal"]);
            ((Modelo.Empresa)obj).TipoLicenca = Convert.ToInt16(dr["tipolicenca"]);
            ((Modelo.Empresa)obj).Quantidade = Convert.ToInt32(dr["quantidade"]);
            ((Modelo.Empresa)obj).Nome = Convert.ToString(dr["nome"]);
            ((Modelo.Empresa)obj).Endereco = Convert.ToString(dr["endereco"]);
            ((Modelo.Empresa)obj).Cidade = Convert.ToString(dr["cidade"]);
            ((Modelo.Empresa)obj).Estado = Convert.ToString(dr["estado"]);
            ((Modelo.Empresa)obj).Cep = Convert.ToString(dr["cep"]);
            ((Modelo.Empresa)obj).Cnpj = Convert.ToString(dr["cnpj"]);
            ((Modelo.Empresa)obj).Cpf = Convert.ToString(dr["cpf"]);
            ((Modelo.Empresa)obj).Chave = Convert.ToString(dr["chave"]);
            ((Modelo.Empresa)obj).CEI = Convert.ToString(dr["cei"]);
            ((Modelo.Empresa)obj).Numeroserie = Convert.ToString(dr["numeroserie"]);
            ((Modelo.Empresa)obj).BDAlterado = dr["bdalterado"] is DBNull ? false : Convert.ToBoolean(dr["bdalterado"]);
            if (dr["validade"] is DBNull)
            {
                ((Modelo.Empresa)obj).Validade = DateTime.MaxValue.Date;
            }
            else
            {
                ((Modelo.Empresa)obj).Validade = Convert.ToDateTime(dr["validade"]);
            }
            ((Modelo.Empresa)obj).UltimoAcesso = dr["ultimoacesso"] is DBNull ? "" : Convert.ToString(dr["ultimoacesso"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
                new FbParameter ("@bprincipal", FbDbType.Boolean),//Indica se é a empresa principal ou não
                new FbParameter ("@tipolicenca", FbDbType.SmallInt),//Tipo de licença => 0 - demonstracao, 1 - empresa, 2 - funcionario
                new FbParameter ("@quantidade", FbDbType.Integer),//Quantidade de funcionarios que podem ser cadastrados dependendo da versao
				new FbParameter ("@nome", FbDbType.VarChar),
				new FbParameter ("@endereco", FbDbType.VarChar),
				new FbParameter ("@cidade", FbDbType.VarChar),
				new FbParameter ("@estado", FbDbType.VarChar),
				new FbParameter ("@cep", FbDbType.VarChar),
				new FbParameter ("@cnpj", FbDbType.VarChar),
				new FbParameter ("@cpf", FbDbType.VarChar),
                new FbParameter ("@chave", SqlDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@cei", FbDbType.VarChar),
                new FbParameter ("@numeroserie", FbDbType.VarChar),
                new FbParameter ("@bdalterado", FbDbType.Integer),
                new FbParameter ("@validade", FbDbType.Date),
                new FbParameter ("@ultimoacesso", FbDbType.VarChar)
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
            parms[1].Value = ((Modelo.Empresa)obj).Codigo;
            parms[2].Value = ((Modelo.Empresa)obj).bPrincipal;
            parms[3].Value = ((Modelo.Empresa)obj).TipoLicenca;
            parms[4].Value = ((Modelo.Empresa)obj).Quantidade;
            parms[5].Value = ((Modelo.Empresa)obj).Nome;
            parms[6].Value = ((Modelo.Empresa)obj).Endereco;
            parms[7].Value = ((Modelo.Empresa)obj).Cidade;
            parms[8].Value = ((Modelo.Empresa)obj).Estado;
            parms[9].Value = ((Modelo.Empresa)obj).Cep;
            parms[10].Value = ((Modelo.Empresa)obj).Cnpj;
            parms[11].Value = ((Modelo.Empresa)obj).Cpf;
            parms[12].Value = ((Modelo.Empresa)obj).Chave;
            parms[13].Value = ((Modelo.Empresa)obj).Incdata;
            parms[14].Value = ((Modelo.Empresa)obj).Inchora;
            parms[15].Value = ((Modelo.Empresa)obj).Incusuario;
            parms[16].Value = ((Modelo.Empresa)obj).Altdata;
            parms[17].Value = ((Modelo.Empresa)obj).Althora;
            parms[18].Value = ((Modelo.Empresa)obj).Altusuario;
            parms[19].Value = ((Modelo.Empresa)obj).CEI;
            parms[20].Value = ((Modelo.Empresa)obj).Numeroserie;
            parms[21].Value = ((Modelo.Empresa)obj).BDAlterado ? 1 : 0;
            parms[22].Value = ((Modelo.Empresa)obj).Validade.GetValueOrDefault();
            parms[23].Value = ((Modelo.Empresa)obj).UltimoAcesso;
        }

        public Modelo.Empresa LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Empresa objEmpresa = new Modelo.Empresa();
            try
            {
                SetInstance(dr, objEmpresa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEmpresa;
        }

        public Modelo.Empresa LoadObjectByCodigo(int codigo)
        {
            throw new NotImplementedException();
        }

        public Modelo.Empresa LoadObjectByDocumento(Int64 documento)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Empresa> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, SELECTLIST, parms);

            List<Modelo.Empresa> lista = new List<Modelo.Empresa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Empresa objEmpresa = new Modelo.Empresa();
                    AuxSetInstance(dr, objEmpresa);
                    lista.Add(objEmpresa);
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

        public DataTable GetEmpresaAtestado(int pEmpresa)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter ("@id", FbDbType.Integer)
            };
            parms[0].Value = pEmpresa;

            DataTable dt = new DataTable();

            string aux = "SELECT \"empresa\".\"id\" " +
                                ", \"empresa\".\"codigo\"" +
                                ", \"empresa\".\"nome\"" +
                                ", case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj\"" +
                                ", \"empresa\".\"endereco\"" +
                                ", \"empresa\".\"cidade\" || ' - ' || \"empresa\".\"estado\" AS \"cidade\"" +
                                ", \"empresa\".\"cep\" " +
                                ", \"empresa\".\"cei\" " +
                                ", \"empresa\".\"numeroserie\" AS \"serie\" " +
                                "FROM \"empresa\" WHERE \"empresa\".\"id\" = @id";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        /// <summary>
        /// Busca no banco a quantidade maxima de funcionarios para a empresa principal.
        /// </summary>
        /// <returns>Numero maximo de funcionarios da empresa</returns>
        public int GetQuantidadeMaximaDeFuncionarios()
        {
            FbParameter[] parms = new FbParameter[] { };

            string aux = "SELECT \"empresa\".\"quantidade\" " +
                         "FROM \"empresa\" WHERE \"empresa\".\"bprincipal\" = 1";

            return (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);

        }

        /// <summary>
        /// Método que retorna a empresa principal
        /// </summary>
        /// <returns></returns>
        /// 24/12/09 - WNO
        public Modelo.Empresa GetEmpresaPrincipal()
        {
            FbParameter[] parms = new FbParameter[] { };

            string aux = "SELECT * FROM \"empresa\" WHERE \"empresa\".\"bprincipal\" = 1";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Empresa objEmpresa = new Modelo.Empresa();
            SetInstance(dr, objEmpresa);

            return objEmpresa;
        }

        public string GetPrimeiroCwk(out string mensagem)
        {
            FbParameter[] parms = new FbParameter[] { };

            string sql = "SELECT CAST (\"cwk\" AS VARCHAR(200)) AS cwk FROM \"cwkvsnsys\" ORDER BY \"id\"";

            try
            {
                FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);

                if (dr.Read())
                {
                    mensagem = "";
                    return ((string)dr["cwk"]);
                }
                mensagem = "A tabela de versões foi alterada.\nEntre em contato com a revenda.";
                return String.Empty;
            }
            catch (Exception)
            {
                mensagem = "A base de dados está em uma versão inferior à versão " + Modelo.Global.Versao + ".\nEntre em contato com a revenda.";
                return String.Empty;
            }
        }

        public bool FazerRestricaoUsuarios()
        { throw new NotImplementedException(); }

        public int? GetIdporIdIntegracao(int? IdIntegracao)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEmpresa Members


        public bool RelatorioAbsenteismoLiberado()
        {
            throw new NotImplementedException();
        }

        public bool ModuloRefeitorioLiberado()
        {
            throw new NotImplementedException();
        }

        #endregion


        public List<Modelo.Proxy.pxyEmpresa> GetAllListPxyEmpresa(string filtro)
        {
            throw new NotImplementedException();
        }


        public Modelo.PeriodoFechamento PeriodoFechamento(int idEmpresa)
        {
            throw new NotImplementedException();
        }


        public Modelo.PeriodoFechamento PeriodoFechamentoPorCodigo(int codigoEmp)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Empresa> GetEmpresaByIds(List<int> ids)
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

        public List<int> GetIdsPorCodigos(List<int> codigos)
        {
            throw new NotImplementedException();
        }

        public List<int> GetAllIds()
        {
            throw new NotImplementedException();
        }

        public bool ConsultaBloqueiousuariosEmpresa()
        {
            throw new NotImplementedException();
        }
        public bool ConsultaUtilizaRegistradorAllEmp()
        {
            throw new NotImplementedException();
        }

        public bool UtilizaControleContratos()
        {
            throw new NotImplementedException();
        }

        public int getCodigo(int pValor, string pCampo, int? pValor2)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Empresa> GetAllListEmpresa()
        {
            throw new NotImplementedException();
        }
    }
}
