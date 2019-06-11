using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Collections;
using Modelo;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class FechamentoBH : DAL.FB.DALBase, DAL.IFechamentoBH
    {

        private FechamentoBH()
        {
            GEN = "GEN_fechamentobh_id";

            TABELA = "fechamentobh";

            SELECTPID = "SELECT * FROM \"fechamentobh\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT       \"id\"" + 
                            ", \"codigo\"" + 
                            ", \"data\"" + 
                            ", CASE WHEN \"tipo\" = 0 THEN 'Empresa' WHEN \"tipo\" = 1 THEN 'Departamento' WHEN \"tipo\" = 2 THEN 'Funcionário' WHEN \"tipo\" = 3 THEN 'Função' ELSE '' END AS \"tipo\" " +
                            ", CASE WHEN \"tipo\" = 0 then (SELECT \"empresa\".\"nome\" FROM \"empresa\" WHERE \"empresa\".\"id\" = \"fechamentobh\".\"identificacao\") " +
                                    "      when \"tipo\" = 1 then (SELECT \"departamento\".\"descricao\" FROM \"departamento\" WHERE \"departamento\".\"id\" = \"fechamentobh\".\"identificacao\") " +
                                    "      when \"tipo\" = 2 then (SELECT \"funcionario\".\"nome\" FROM \"funcionario\" WHERE \"funcionario\".\"id\" = \"fechamentobh\".\"identificacao\") " +
                                    "      when \"tipo\" = 3 then (SELECT \"funcao\".\"descricao\" FROM \"funcao\" WHERE \"funcao\".\"id\" = \"fechamentobh\".\"identificacao\") end AS \"nome\" " +
                 " FROM \"fechamentobh\"";

            INSERT = "  INSERT INTO \"fechamentobh\"" +
                                        "(\"codigo\", \"data\", \"tipo\", \"efetivado\", \"identificacao\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @data, @tipo, @efetivado, @identificacao, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"fechamentobh\" SET \"codigo\" = @codigo " +
                                        ", \"data\" = @data " +
                                        ", \"tipo\" = @tipo " +
                                        ", \"efetivado\" = @efetivado " +
                                        ", \"identificacao\" = @identificacao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"fechamentobh\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"fechamentobh\"";

        }

        #region Singleton

        private static volatile FB.FechamentoBH _instancia = null;

        public static FB.FechamentoBH GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.FechamentoBH))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.FechamentoBH();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        #region Métodos Básicos

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
                obj = new Modelo.FechamentoBH();
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
            ((Modelo.FechamentoBH)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.FechamentoBH)obj).Data = dr["data"] is DBNull ? null : (DateTime?)dr["data"];
            ((Modelo.FechamentoBH)obj).Tipo = Convert.ToInt16(dr["tipo"]);
            ((Modelo.FechamentoBH)obj).Efetivado = Convert.ToInt16(dr["efetivado"]);
            ((Modelo.FechamentoBH)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@tipo", FbDbType.SmallInt),
				new FbParameter ("@efetivado", FbDbType.SmallInt),
				new FbParameter ("@identificacao", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.FechamentoBH)obj).Codigo;
            parms[2].Value = ((Modelo.FechamentoBH)obj).Data;
            parms[3].Value = ((Modelo.FechamentoBH)obj).Tipo;
            parms[4].Value = ((Modelo.FechamentoBH)obj).Efetivado;
            parms[5].Value = ((Modelo.FechamentoBH)obj).Identificacao;
            parms[6].Value = ((Modelo.FechamentoBH)obj).Incdata;
            parms[7].Value = ((Modelo.FechamentoBH)obj).Inchora;
            parms[8].Value = ((Modelo.FechamentoBH)obj).Incusuario;
            parms[9].Value = ((Modelo.FechamentoBH)obj).Altdata;
            parms[10].Value = ((Modelo.FechamentoBH)obj).Althora;
            parms[11].Value = ((Modelo.FechamentoBH)obj).Altusuario;
        }

        public Modelo.FechamentoBH LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.FechamentoBH objFechamentoBH = new Modelo.FechamentoBH();
            try
            {
                SetInstance(dr, objFechamentoBH);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFechamentoBH;
        }
        
        public List<Modelo.FechamentoBH> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"fechamentobh\"", parms);

            List<Modelo.FechamentoBH> lista = new List<Modelo.FechamentoBH>();
            try
            {
                while (dr.Read())
                {
                    Modelo.FechamentoBH objFechamentoBH = new Modelo.FechamentoBH();
                    AuxSetInstance(dr, objFechamentoBH);
                    lista.Add(objFechamentoBH);
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

        public List<int> GetIds()
        {
            List<int> lista = new List<int>();

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"id\" FROM \"fechamentobh\"", new FbParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]));
                }
            }

            return lista;
        }

        public Hashtable GetHashCodigoId()
        {
            Hashtable lista = new Hashtable();

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"codigo\", \"id\" FROM \"fechamentobh\"", new FbParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }

            return lista;
        }

        #endregion

        public void ClearFechamentoBH(int id)
        {
            this.ClearFechamentoBHD(id);

            FbParameter[] parms = new FbParameter[1]
            { 
                new 
                    FbParameter("@idfechamentobh", FbDbType.Integer)
            };
            parms[0].Value = id;

            string aux = "DELETE FROM \"fechamentobh\" " +
                         " WHERE \"id\" = @idfechamentobh";

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
            cmd.Parameters.Clear();            
        }

        private void ClearFechamentoBHD(int pIdFechamentoBH)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                new FbParameter("@idfechamentobh", FbDbType.Integer)
            };
            parms[0].Value = pIdFechamentoBH;

            string aux = "DELETE FROM \"fechamentobhd\"" +
                         " WHERE \"idfechamentobh\" = @idfechamentobh";

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
            cmd.Parameters.Clear();
        }

        /// <summary>
        /// Esse método pega os totais de credito e débito do banco de horas do funcionario.
        /// </summary>
        /// <param name="pTipo">Tipo do fechamento</param>
        /// <param name="pIdentificacao">Id do tipo</param>
        /// <param name="pDataI">Data inicial do banco de horas</param>
        /// <param name="pDataF">Data final do banco de horas</param>
        /// <returns>Id do funcionario, credito, débito</returns>
        //PAM - 26/03/2010
        // Esse método utiliza dois selects básicos: O select interno tras o valor do credito e débito de todas
        // as marcações de um funcionario naquele período. O resultado desse select é renomeado com o nome de TODOS
        // Em seguida, com todas as marcações do funcinario em mãos, soma os campos de credito e débito e ordena por funcionario
        public DataTable getTotaisFuncionarios(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@identificacao", FbDbType.Integer),
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date)
            };

            parms[0].Value = pIdentificacao;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string comando = 
                "SELECT \"todos\".\"id\" " + //Esse select faz a soma dos totais
                ", SUM(\"todos\".\"creditobh\") AS \"creditobh\" "+
                ", SUM(\"todos\".\"debitobh\") AS \"debitobh\" " +
                "FROM " + //O select de baixo tras todas os totais de todos os dias do funcionario
                "(SELECT (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"bancohorascre\", '--:--'))) AS \"creditobh\" " +
	            ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"bancohorasdeb\", '--:--'))) AS \"debitobh\" " +
	            ", \"funcionario\".\"id\" " +
	            " FROM \"marcacao\" " +
    	        " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                " WHERE COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"funcionarioativo\" = 1 AND COALESCE(\"marcacao\".\"idfechamentobh\",0) = 0 ";

            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " AND \"funcionario\".\"idempresa\" = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " AND \"funcionario\".\"iddepartamento\" = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " AND \"marcacao\".\"idfuncionario\" = @identificacao";
                        break;
                    case 3://Função
                        comando += " AND \"funcionario\".\"idfuncao\" = @identificacao";
                        break;
                    default:
                        break;
                }
            }

            comando +=  " AND \"marcacao\".\"data\" >= @datai AND \"marcacao\".\"data\" <= @dataf " +
                        " ORDER BY \"funcionario\".\"id\") AS \"todos\" GROUP BY \"todos\".\"id\"";//Nesse caso o todos.id = id funcionario

            DataTable dt = new DataTable();
            dt.Load(DataBase.ExecuteReader(CommandType.Text, comando, parms));
            return dt;
        }

        /// <summary>
        /// Pega os saldo anterior do banco de dados para todos os funcionarios daquele tipo de fechamento
        /// </summary>
        /// <param name="pTipo">Tipo do fechamento</param>
        /// <param name="pIdentificacao">Id do Tipo</param>
        /// <returns>Uma hash table cuja chave é o id do funcionario e contém os seguintes dados:
        /// Id do funcionario, saldo do banco de horas, tipo do saldo = credito/debito</returns>
        // Esse método realiza um select que retorna o ultimo saldo do banco de horas dos funcionarios
        // que estao incluidos naquele fechamento
        // O select é realizado da seguinte maneira: 
        // O Select 3 pega o id do funcionario juntamente com o id do ultimo fechamento realizado para ele
        // O Select 2 pega os 3 campos daqueles Ids encontrados no Select 3
        // O Select 1 junta com a tabela de funcionarios para pegar somente aqueles que fazem parte do fechamento em questao
        public Hashtable getSaldoAnterior(int? pTipo, int pIdentificacao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@identificacao", FbDbType.Integer)
            };

            parms[0].Value = pIdentificacao;

            string comando =
               "SELECT  \"ultimo\".\"identificacao\" AS id " + // Select 1
               ",(SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"ultimo\".\"tiposaldo\", '----:--'))) AS tiposaldo " +
               ",(SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"ultimo\".\"saldobh\", '----:--'))) AS saldo " +
               "FROM \"funcionario\" \"func\", " + //Junção de funcionario com um select
                    "(SELECT  \"fbhd\".\"identificacao\", \"fbhd\".\"tiposaldo\", \"fbhd\".\"saldobh\" " + //Select 2
                    "FROM \"fechamentobhd\" \"fbhd\", " + // Junção de Fechamento BHD com um Select
                        "(SELECT \"fbhd1\".\"identificacao\" , max(\"fbhd1\".\"id\") AS \"maxid\" " + //Select 3
                        "FROM \"fechamentobhd\" \"fbhd1\" group by \"fbhd1\".\"identificacao\") AS \"maxfech\" " +
                        "WHERE \"fbhd\".\"id\" = \"maxfech\".\"maxid\") AS \"ultimo\" " +
               "WHERE \"ultimo\".\"identificacao\" = \"func\".\"id\" ";

            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " AND \"func\".\"idempresa\" = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " AND \"func\".\"iddepartamento\" = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " AND \"func\".\"id\" = @identificacao";
                        break;
                    case 3://Função
                        comando += " AND \"func\".\"idfuncao\" = @identificacao";
                        break;
                    default:
                        break;
                }
            }

            comando += " ORDER BY \"func\".\"id\" ";

            Hashtable ht = new Hashtable();

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);

            if (dr.HasRows)
            {
                Modelo.DadosFechamento dadosFechamento;
                while (dr.Read())
                {
                    dadosFechamento = new Modelo.DadosFechamento();
                    dadosFechamento.idFuncionario = Convert.ToInt32(dr["id"]);
                    dadosFechamento.saldoBH = Convert.ToInt32(dr["saldo"]);
                    dadosFechamento.TipoSaldoBH = Convert.ToInt32(dr["tiposaldo"]);
                    ht.Add(dadosFechamento.idFuncionario, dadosFechamento);
                }
            }

            return ht;
        }

        public bool VerificaSeExisteFechamento(int pCodigo)
        {
            string aux;
            int retorno;
            FbParameter[] parms = new FbParameter[1]
            { 
                new FbParameter("@codigo", FbDbType.Integer),    
            };
            parms[0].Value = pCodigo;
            aux = "SELECT COUNT(\"id\") FROM \"fechamentobh\" WHERE \"codigo\" = @codigo";

            retorno = (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
            if (retorno > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs, bool ValidaPermissao)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs)
        {
            return this.GetAllListFuncs(idsFuncs,true);
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
