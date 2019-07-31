using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Collections;
using Modelo;
using System.Data.SqlClient;
using Modelo.Proxy;

namespace DAL.FB
{
    public class Funcionario : DAL.FB.DALBase, DAL.IFuncionario
    {
        public string SELECTREL { get; set; }
        public string SELECTPRO { get; set; }
        public string SELECTDEL { get; set; }
        public string SELECTATIVOS { get; set; }

        private Funcionario()
        {
            GEN = "GEN_funcionario_id";

            TABELA = "funcionario";

            SELECTPID = "   SELECT   \"funcionario\".*" +
                                    ", \"horario\".\"descricao\" AS \"jornada\" " +
                                    ", \"empresa\".\"nome\" AS \"empresa\" " +
                                    ", \"departamento\".\"descricao\" AS \"departamento\" " +
                             "FROM \"funcionario\"" +
                             "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             "LEFT JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                             " WHERE \"funcionario\".\"id\" = @id";

            SELECTALL = "   SELECT     \"funcionario\".\"id\"" +
                                    ", \"funcionario\".\"nome\"" +
                                    ", CAST(\"funcionario\".\"dscodigo\" AS BIGINT) AS \"dscodigo\"" +
                                    ", case when COALESCE(\"funcionario\".\"matricula\", '') = '' then '' else CAST(\"funcionario\".\"matricula\" AS BIGINT) end AS \"matricula\" " +
                                    ", \"horario\".\"descricao\" AS \"jornada\"" +
                                    ", \"empresa\".\"nome\" AS \"empresa\"" +
                                    ", \"departamento\".\"descricao\" AS \"departamento\"" +
                                    ", \"funcionario\".\"carteira\"" +
                                    ", \"funcionario\".\"dataadmissao\"" +
                                    ", \"funcionario\".\"funcionarioativo\" " +
                             "FROM \"funcionario\" " +
                             "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             "LEFT JOIN \"horario\" ON \"horario\".\"id\"= \"funcionario\".\"idhorario\" " +
                             " WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 0 " +
                             " ORDER BY \"funcionario\".\"nome\"";

            SELECTDEL = "   SELECT   \"funcionario\".\"id\" " +
                                    ", \"funcionario\".\"nome\" " +
                                    ", CAST(\"funcionario\".\"dscodigo\" AS BIGINT) AS \"dscodigo\"" +
                                    ", case when COALESCE(\"funcionario\".\"matricula\", '') = '' then 0 else CAST(\"funcionario\".\"matricula\" AS BIGINT) end AS \"matricula\" " +
                                    ", \"horario\".\"descricao\" AS \"jornada\"" +
                                    ", \"empresa\".\"nome\" AS \"empresa\"" +
                                    ", \"departamento\".\"descricao\" AS \"departamento\"" +
                                    ", \"funcionario\".\"carteira\"" +
                                    ", \"funcionario\".\"dataadmissao\" " +
                             "FROM \"funcionario\" " +
                             "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             "LEFT JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\" " +
                             " WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 1 " +
                             " ORDER BY \"funcionario\".\"nome\"";

            SELECTPRO = "   SELECT   \"funcionario\".\"id\"" +
                                    ", \"funcionario\".\"codigo\"" +
                                    ", COALESCE(\"funcionario\".\"nome\", '') AS \"nome\"" +
                                    ", \"funcionario\".\"dscodigo\"" +
                                    ", case when COALESCE(\"funcionario\".\"matricula\", '') = '' then 0 else CAST(\"funcionario\".\"matricula\" AS BIGINT) end AS \"matricula\" " +
                                   ",\"horario\".\"descricao\" AS \"jornada\"" +
                                    ", \"empresa\".\"nome\" AS \"empresa\"" +
                                    ", \"departamento\".\"descricao\" AS \"departamento\"" +
                                    ", \"funcionario\".\"carteira\"" +
                                    ", \"funcionario\".\"dataadmissao\" " +
                             "FROM \"funcionario\" " +
                             "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             "LEFT JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\" " +
                             "WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 0 AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 " +
                             "ORDER BY LOWER(\"funcionario\".\"nome\")";

            SELECTREL = "   SELECT  \"funcionario\".\"id\" " +
                                    ", CAST(\"funcionario\".\"dscodigo\" AS BIGINT) AS \"codigo\"" +
                                    ", \"funcionario\".\"nome\"" +
                                    ", case when COALESCE(\"funcionario\".\"matricula\", '') = '' then '' else CAST(\"funcionario\".\"matricula\" AS BIGINT) end AS \"matricula\" " +
                                    ", \"funcionario\".\"dataadmissao\" " +
                                    ", \"funcionario\".\"datademissao\" " +
                                    ", \"departamento\".\"descricao\" AS \"departamento\"" +
                                    ", \"empresa\".\"nome\" AS \"empresa\"" +
                                    ", \"horario\".\"descricao\" AS \"horario\"" +
                                    ", (SELECT FIRST 1 COALESCE(\"hd\".\"entrada_1\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_1\", '--:--') || ' | ' || COALESCE(\"hd\".\"entrada_2\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_2\", '--:--') || + " +
                                    " ' | ' || COALESCE(\"hd\".\"entrada_3\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_3\", '--:--') || ' | ' || COALESCE(\"hd\".\"entrada_4\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_4\", '--:--') " +
                                    " FROM \"horariodetalhe\" \"hd\" WHERE \"hd\".\"idhorario\" = \"horario\".\"id\" AND (\"hd\".\"dia\" = 1 OR EXTRACT(WEEKDAY FROM \"hd\".\"data\") = 1)) AS \"horariodescricao\"" +
                             "FROM \"funcionario\"" +
                             " LEFT JOIN \"empresa\"  ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                             " LEFT JOIN \"departamento\"  ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\"" +
                             " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                             " WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 0 ";

            INSERT = "  INSERT INTO \"funcionario\"" +
                                        "(\"codigo\", \"dscodigo\", \"matricula\", \"nome\", \"codigofolha\", \"idempresa\", \"iddepartamento\", \"idfuncao\", \"idhorario\", \"tipohorario\", \"carteira\", \"dataadmissao\", \"datademissao\", \"salario\", \"funcionarioativo\", \"naoentrarbanco\", \"naoentrarcompensacao\", \"excluido\", \"campoobservacao\", \"foto\", \"incdata\", \"inchora\", \"incusuario\", \"pis\")" +
                                        "VALUES" +
                                        "(@codigo, @dscodigo, @matricula, @nome, @codigofolha, @idempresa, @iddepartamento, @idfuncao, @idhorario, @tipohorario, @carteira, @dataadmissao, @datademissao, @salario, @funcionarioativo, @naoentrarbanco, @naoentrarcompensacao, @excluido, @campoobservacao, @foto, @incdata, @inchora, @incusuario, @pis)";

            UPDATE = "  UPDATE \"funcionario\" SET \"codigo\" = @codigo " +
                                        ", \"dscodigo\" = @dscodigo " +
                                        ", \"matricula\" = @matricula " +
                                        ", \"nome\" = @nome " +
                                        ", \"codigofolha\" = @codigofolha " +
                                        ", \"idempresa\" = @idempresa " +
                                        ", \"iddepartamento\" = @iddepartamento " +
                                        ", \"idfuncao\" = @idfuncao " +
                                        ", \"idhorario\" = @idhorario " +
                                        ", \"tipohorario\" = @tipohorario " +
                                        ", \"carteira\" = @carteira " +
                                        ", \"dataadmissao\" = @dataadmissao " +
                                        ", \"datademissao\" = @datademissao " +
                                        ", \"salario\" = @salario " +
                                        ", \"funcionarioativo\" = @funcionarioativo " +
                                        ", \"naoentrarbanco\" = @naoentrarbanco " +
                                        ", \"naoentrarcompensacao\" = @naoentrarcompensacao " +
                                        ", \"excluido\" = @excluido " +
                                        ", \"campoobservacao\" = @campoobservacao " +
                                        ", \"foto\" = @foto " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"pis\" = @pis " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"funcionario\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"funcionario\"";

        }

        #region Singleton

        private static volatile FB.Funcionario _instancia = null;

        public static FB.Funcionario GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Funcionario))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Funcionario();
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
                obj = new Modelo.Funcionario();
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
            ((Modelo.Funcionario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Funcionario)obj).Dscodigo = Convert.ToString(dr["dscodigo"]);
            ((Modelo.Funcionario)obj).Matricula = Convert.ToString(dr["matricula"]);
            ((Modelo.Funcionario)obj).Nome = Convert.ToString(dr["nome"]);
            ((Modelo.Funcionario)obj).Codigofolha = (dr["codigofolha"] is DBNull ? Convert.ToInt32(0) : Convert.ToInt32(dr["codigofolha"]));
            ((Modelo.Funcionario)obj).Idempresa = Convert.ToInt32(dr["idempresa"]);
            ((Modelo.Funcionario)obj).Iddepartamento = Convert.ToInt32(dr["iddepartamento"]);
            ((Modelo.Funcionario)obj).Idfuncao = Convert.ToInt32(dr["idfuncao"]);
            ((Modelo.Funcionario)obj).Idhorario = (dr["idhorario"] is DBNull ? 0 : Convert.ToInt32(dr["idhorario"]));
            ((Modelo.Funcionario)obj).Tipohorario = Convert.ToInt16(dr["tipohorario"]);
            ((Modelo.Funcionario)obj).Carteira = Convert.ToString(dr["carteira"]);
            ((Modelo.Funcionario)obj).Dataadmissao = Convert.ToDateTime(dr["dataadmissao"]);
            ((Modelo.Funcionario)obj).Datademissao = (dr["datademissao"] is DBNull ? null : (DateTime?)(dr["datademissao"]));
            ((Modelo.Funcionario)obj).Salario = Convert.ToDecimal(dr["salario"]);
            ((Modelo.Funcionario)obj).Funcionarioativo = Convert.ToInt16(dr["funcionarioativo"]);
            ((Modelo.Funcionario)obj).Funcionarioativo_Ant = ((Modelo.Funcionario)obj).Funcionarioativo;
            ((Modelo.Funcionario)obj).Naoentrarbanco = Convert.ToInt16(dr["naoentrarbanco"]);
            ((Modelo.Funcionario)obj).Naoentrarbanco_Ant = ((Modelo.Funcionario)obj).Naoentrarbanco;
            ((Modelo.Funcionario)obj).Naoentrarcompensacao = Convert.ToInt16(dr["naoentrarcompensacao"]);
            ((Modelo.Funcionario)obj).Naoentrarcompensacao_Ant = ((Modelo.Funcionario)obj).Naoentrarcompensacao;
            ((Modelo.Funcionario)obj).Excluido = Convert.ToInt16(dr["excluido"]);
            ((Modelo.Funcionario)obj).Campoobservacao = Convert.ToString(dr["campoobservacao"]);
            ((Modelo.Funcionario)obj).Foto = Convert.ToString(dr["foto"]);
            ((Modelo.Funcionario)obj).Empresa = Convert.ToString(dr["empresa"]);
            ((Modelo.Funcionario)obj).Horario = Convert.ToString(dr["jornada"]);
            ((Modelo.Funcionario)obj).Pis = Convert.ToString(dr["pis"]);

            ((Modelo.Funcionario)obj).Iddepartamento_Ant = ((Modelo.Funcionario)obj).Iddepartamento;
            ((Modelo.Funcionario)obj).Idempresa_Ant = ((Modelo.Funcionario)obj).Idempresa;
            ((Modelo.Funcionario)obj).Idfuncao_Ant = ((Modelo.Funcionario)obj).Idfuncao;
            ((Modelo.Funcionario)obj).Dataadmissao_Ant = ((Modelo.Funcionario)obj).Dataadmissao;
            ((Modelo.Funcionario)obj).Datademissao_Ant = ((Modelo.Funcionario)obj).Datademissao;
            ((Modelo.Funcionario)obj).Funcionarioativo_Ant = ((Modelo.Funcionario)obj).Funcionarioativo;
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@dscodigo", FbDbType.VarChar),
				new FbParameter ("@matricula", FbDbType.VarChar),
				new FbParameter ("@nome", FbDbType.VarChar),
				new FbParameter ("@codigofolha", FbDbType.SmallInt),
				new FbParameter ("@idempresa", FbDbType.Integer),
				new FbParameter ("@iddepartamento", FbDbType.Integer),
				new FbParameter ("@idfuncao", FbDbType.Integer),
				new FbParameter ("@idhorario", FbDbType.Integer),
				new FbParameter ("@tipohorario", FbDbType.SmallInt),
				new FbParameter ("@carteira", FbDbType.VarChar),
				new FbParameter ("@dataadmissao", FbDbType.Date),
				new FbParameter ("@datademissao", FbDbType.Date),
				new FbParameter ("@salario", FbDbType.Decimal),
				new FbParameter ("@funcionarioativo", FbDbType.SmallInt),
				new FbParameter ("@naoentrarbanco", FbDbType.SmallInt),
				new FbParameter ("@naoentrarcompensacao", FbDbType.SmallInt),
				new FbParameter ("@excluido", FbDbType.SmallInt),
				new FbParameter ("@campoobservacao", FbDbType.VarChar),
				new FbParameter ("@foto", FbDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
				new FbParameter ("@pis", FbDbType.VarChar)
                
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
            parms[1].Value = ((Modelo.Funcionario)obj).Codigo;
            parms[2].Value = ((Modelo.Funcionario)obj).Dscodigo;
            parms[3].Value = ((Modelo.Funcionario)obj).Matricula;
            parms[4].Value = ((Modelo.Funcionario)obj).Nome;
            parms[5].Value = ((Modelo.Funcionario)obj).Codigofolha;
            parms[6].Value = ((Modelo.Funcionario)obj).Idempresa;
            parms[7].Value = ((Modelo.Funcionario)obj).Iddepartamento;
            parms[8].Value = ((Modelo.Funcionario)obj).Idfuncao;
            if (((Modelo.Funcionario)obj).Idhorario > 0)
                parms[9].Value = ((Modelo.Funcionario)obj).Idhorario;
            parms[10].Value = ((Modelo.Funcionario)obj).Tipohorario;
            parms[11].Value = ((Modelo.Funcionario)obj).Carteira;
            parms[12].Value = ((Modelo.Funcionario)obj).Dataadmissao;

            if (((Modelo.Funcionario)obj).Datademissao != new DateTime())
                parms[13].Value = ((Modelo.Funcionario)obj).Datademissao;
            else
                parms[13].Value = DBNull.Value;

            parms[14].Value = ((Modelo.Funcionario)obj).Salario;
            parms[15].Value = ((Modelo.Funcionario)obj).Funcionarioativo;
            parms[16].Value = ((Modelo.Funcionario)obj).Naoentrarbanco;
            parms[17].Value = ((Modelo.Funcionario)obj).Naoentrarcompensacao;
            parms[18].Value = ((Modelo.Funcionario)obj).Excluido;
            parms[19].Value = ((Modelo.Funcionario)obj).Campoobservacao;
            parms[20].Value = ((Modelo.Funcionario)obj).Foto;
            parms[21].Value = ((Modelo.Funcionario)obj).Incdata;
            parms[22].Value = ((Modelo.Funcionario)obj).Inchora;
            parms[23].Value = ((Modelo.Funcionario)obj).Incusuario;
            parms[24].Value = ((Modelo.Funcionario)obj).Altdata;
            parms[25].Value = ((Modelo.Funcionario)obj).Althora;
            parms[26].Value = ((Modelo.Funcionario)obj).Altusuario;
            parms[27].Value = ((Modelo.Funcionario)obj).Pis;
        }

        public int GetIdporIdIntegracao(int? idIntegracao)
        {
            throw new NotImplementedException();
        }

        public bool VerificaCodigoDuplicado(string pCodigo)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                  new FbParameter("@dscodigo", FbDbType.VarChar)
            };
            parms[0].Value = pCodigo;

            string aux = "SELECT COALESCE(COUNT(\"id\"),0) AS qtd FROM \"funcionario\" WHERE \"dscodigo\" = @dscodigo";

            FbDataReader dr;
            dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.Read())
            {
                if (Convert.ToInt32(dr["qtd"]) > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public Modelo.Funcionario LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            try
            {
                SetInstance(dr, objFuncionario);

                DAL.FB.FuncionarioHistorico dalFuncionarioHistorico = DAL.FB.FuncionarioHistorico.GetInstancia;
                objFuncionario.Historico = dalFuncionarioHistorico.LoadPorFuncionario(objFuncionario.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objFuncionario;
        }

        #region Relatórios

        public DataTable GetOrdenadoPorNomeRel(string pInicial, string pFinal, string pEmpresas)
        {
            FbParameter[] parms = new FbParameter[0];
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND \"funcionario\".\"idempresa\" IN " + pEmpresas + " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 ";
            if (!String.IsNullOrEmpty(pInicial))
            {
                aux += " AND UPPER (\"funcionario\".\"nome\") >= '" + pInicial.ToUpper() + "'";
            }
            if (!String.IsNullOrEmpty(pFinal))
            {
                aux += " AND SUBSTRING (UPPER (\"funcionario\".\"nome\") FROM 1 FOR " + pFinal.Length + ") <= '" + pFinal.ToUpper() + "'";
            }
            aux += " ORDER BY \"funcionario\".\"nome\" ";
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetOrdenadoPorCodigoRel(string pInicial, string pFinal, string pEmpresas)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                  new FbParameter("@inicial", FbDbType.VarChar)
                , new FbParameter("@final", FbDbType.VarChar)
            };
            parms[0].Value = pInicial;
            parms[1].Value = pFinal;
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND \"funcionario\".\"idempresa\" IN " + pEmpresas + " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 ";
            if ((pInicial != "0") || (pFinal != "0"))
            {
                aux = aux + " AND ((CAST(\"funcionario\".\"dscodigo\" AS BIGINT) >= @inicial) AND (CAST(\"funcionario\".\"dscodigo\" AS BIGINT) <= @final)) ORDER BY \"codigo\", \"funcionario\".\"nome\" ";
            }
            else
            {
                aux = aux + " ORDER BY CAST(\"funcionario\".\"dscodigo\" AS BIGINT), \"funcionario\".\"nome\" ";
            }
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetRelatorio(string pEmpresas)
        {
            FbParameter[] parms = new FbParameter[0];
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND \"funcionario\".\"idempresa\" IN " + pEmpresas + " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 ";
            aux += " ORDER BY \"funcionario\".\"nome\" ";
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetPorDepartamentoRel(string pDepartamentos)
        {
            FbParameter[] parms = new FbParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND \"funcionario\".\"iddepartamento\" IN " + pDepartamentos + " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 ";
            aux += " ORDER BY \"departamento\", \"funcionario\".\"nome\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetPorDepartamento(string pDepartamentos)
        {
            FbParameter[] parms = new FbParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND \"funcionario\".\"iddepartamento\" IN " + pDepartamentos;
            aux += " ORDER BY \"departamento\", \"funcionario\".\"nome\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetPorHorarioRel(string pHorarios, string pEmpresas)
        {
            FbParameter[] parms = new FbParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND \"funcionario\".\"idhorario\" IN " + pHorarios;
            aux += " AND \"funcionario\".\"idempresa\" IN " + pEmpresas;
            aux += " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 ";
            aux += " ORDER BY \"horario\", \"funcionario\".\"nome\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                  new FbParameter("@inicial", FbDbType.Date)
                , new FbParameter("@final", FbDbType.Date)
            };
            parms[0].Value = pInicial;
            parms[1].Value = pFinal;
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND \"funcionario\".\"idempresa\" IN " + pEmpresas;
            aux += " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 ";

            if (pInicial != new DateTime() && pFinal != new DateTime())
            {
                parms[0].Value = pInicial;
                parms[1].Value = pFinal;
                aux = aux + " AND ((\"funcionario\".\"dataadmissao\" >= @inicial) AND (\"funcionario\".\"dataadmissao\" <= @final))";
            }
            else if (pInicial != new DateTime())
            {
                parms[0].Value = pInicial;
                aux = aux + " AND ((\"funcionario\".\"dataadmissao\" >= @inicial)";
            }
            else if (pFinal != new DateTime())
            {
                parms[1].Value = pFinal;
                aux = aux + " AND ((\"funcionario\".\"dataadmissao\" <= @final)";
            }
            aux = aux + " ORDER BY \"funcionario\".\"dataadmissao\", \"codigo\"";
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                  new FbParameter("@inicial", FbDbType.Date)
                , new FbParameter("@final", FbDbType.Date)
            };
            parms[0].Value = pInicial;
            parms[1].Value = pFinal;
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND \"funcionario\".\"idempresa\" IN " + pEmpresas;
            aux += " AND \"funcionario\".\"datademissao\" IS NOT NULL ";

            if (pInicial != new DateTime() && pFinal != new DateTime())
                aux = aux + " AND ((\"funcionario\".\"datademissao\" >= @inicial) AND (\"funcionario\".\"datademissao\" <= @final))";

            else if (pInicial != new DateTime())
                aux = aux + " AND (\"funcionario\".\"datademissao\" >= @inicial)";

            else if (pFinal != new DateTime())
                aux = aux + " AND (\"funcionario\".\"datademissao\" <= @final)";

            aux = aux + " ORDER BY \"funcionario\".\"datademissao\", \"codigo\"";
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetAtivosInativosRel(bool pAtivo, string pEmpresas)
        {
            FbParameter[] parms = new FbParameter[0];
            DataTable dt = new DataTable();

            string aux = SELECTREL + " AND \"funcionario\".\"idempresa\" IN " + pEmpresas + " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = ";
            aux += (pAtivo ? "1" : "0");
            aux += " ORDER BY \"funcionario\".\"nome\"";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetListaPresenca(DateTime dataInicial, int tipo, string empresas, string departamentos, string funcionarios)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@datainicial", FbDbType.Date)
            };
            parms[0].Value = dataInicial;

            DataTable dt = new DataTable();
            string aux = "SELECT   \"empresa\".\"nome\" AS \"empresa\" " +
                                  " , \"empresa\".\"endereco\" " +
                                  " , \"empresa\".\"cnpj\" " +
                                  " , \"funcionario\".\"nome\" " +
                                  " , \"funcionario\".\"dscodigo\" " +
                                  " , \"departamento\".\"descricao\" AS \"departamento\" " +
                                  " , \"marcacao\".\"entrada_1\" " +
                                  " , \"marcacao\".\"saida_1\" " +
                                  " , \"marcacao\".\"entrada_2\" " +
                                  " , \"marcacao\".\"saida_2\" " +
                                  " , \"marcacao\".\"entrada_3\" " +
                                  " , \"marcacao\".\"saida_3\" " +
                                  " , \"marcacao\".\"entrada_4\" " +
                                  " , \"marcacao\".\"saida_4\" " +
                                  " , \"marcacao\".\"entrada_5\" " +
                                  " , \"marcacao\".\"saida_5\" " +
                                  " , \"marcacao\".\"entrada_6\" " +
                                  " , \"marcacao\".\"saida_6\" " +
                                  " , \"marcacao\".\"entrada_7\" " +
                                  " , \"marcacao\".\"saida_7\" " +
                                  " , \"marcacao\".\"entrada_8\" " +
                                  " , \"marcacao\".\"saida_8\" " +
                            "FROM \"funcionario\" " +
                            "INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                            "INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                            "INNER JOIN \"marcacao\" ON \"marcacao\".\"idfuncionario\" = \"funcionario\".\"id\" AND \"marcacao\".\"data\" = @datainicial " +
                            "WHERE COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 AND \"funcionario\".\"datademissao\" IS NULL";

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

            aux += " ORDER BY \"funcionario\".\"nome\", \"empresa\".\"nome\", \"departamento\".\"descricao\"";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        #endregion

        #region Listagens em DataTables

        public DataTable GetAll(bool pegaTodos)
        {
            FbParameter[] parms = new FbParameter[] { };

            string aux = "SELECT \"funcionario\".* " +
                         ", \"horario\".\"descricao\" AS \"jornada\"" +
                         ", \"empresa\".\"nome\" AS \"empresa\"" +
                         " FROM \"funcionario\" " +
                         " LEFT JOIN \"empresa\"  ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                         " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"";
            if (!pegaTodos)
            {
                aux += " WHERE COALESCE(\"funcionario\".\"excluido\",0)=0 AND COALESCE(\"funcionario\".\"funcionarioativo\",0)=1";
            }

            aux += " ORDER BY \"funcionario\".\"nome\"";

            DataTable dt = new DataTable();

            dt.Load(DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetPorEmpresa(int pIDEmpresa, bool pPegaInativos)
        {
            DataTable dt = new DataTable();
            dt.Load(GetEmpresaDR(pIDEmpresa, pPegaInativos));
            DataRow row = dt.NewRow();
            return dt;
        }

        public DataTable GetPorDepartamento(int pIDEmpresa, int pIDDepartamento, bool pPegaInativos)
        {
            DataTable dt = new DataTable();
            dt.Load(GetDepartamentoDR(pIDEmpresa, pIDDepartamento, pPegaInativos));

            return dt;
        }

        public DataTable GetFuncionariosAtivos()
        {
            DataTable dt = new DataTable();
            dt.Load(GetAtivosDR());
            return dt;

        }

        public DataTable GetParaProvisorio()
        {
            FbParameter[] parms = new FbParameter[0];

            DataTable dt = new DataTable();

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, SELECTPRO, parms));

            return dt;
        }

        public DataTable GetExcluidos()
        {
            FbParameter[] parms = new FbParameter[0];

            DataTable dt = new DataTable();

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, SELECTDEL, parms));

            return dt;
        }

        public DataTable GetParaDSR(int? pTipo, int pIdentificacao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@identificacao", FbDbType.Integer)
            };
            parms[0].Value = pIdentificacao;

            DataTable dt = new DataTable();

            string comando = "SELECT \"funcionario\".\"id\", \"funcionario\".\"nome\", \"funcionario\".\"idhorario\" FROM \"funcionario\"";

            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " WHERE \"funcionario\".\"idempresa\" = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " WHERE \"funcionario\".\"iddepartamento\" = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " WHERE \"funcionario\".\"id\" = @identificacao";
                        break;
                    case 3://Função
                        comando += " WHERE \"funcionario\".\"idfuncao\" = @identificacao";
                        break;
                    case 4://Horário
                        comando += " WHERE \"funcionario\".\"idhorario\" = @identificacao";
                        break;
                    default:
                        break;
                }
            }

            dt.Load(DataBase.ExecuteReader(CommandType.Text, comando, parms));

            return dt;
        }

        #endregion

        #region DataReades

        public FbDataReader GetTabelaMarcacaoDR(int tipo, int identificacao, string consultaNomeFuncionario)
        {
            FbParameter[] parms = new FbParameter[1]
                {
                    new FbParameter("@identificacao", FbDbType.Integer)
                };

            parms[0].Value = identificacao;

            string comando = "   SELECT     \"funcionario\".*" +
                            ", \"horario\".\"descricao\" AS \"jornada\"" +
                            ", \"empresa\".\"nome\" AS \"empresa\"" +
                            ", \"departamento\".\"descricao\" AS \"departamento\"" +
                     "FROM \"funcionario\" " +
                     "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                     "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                     "LEFT JOIN \"horario\" ON \"horario\".\"id\"= \"funcionario\".\"idhorario\" ";

            switch (tipo)
            {
                case 1:
                    comando += "WHERE \"funcionario\".\"idempresa\" = @identificacao ";
                    break;
                case 2:
                    comando += "WHERE \"funcionario\".\"iddepartamento\" = @identificacao ";
                    break;
                case 3:
                    comando += "WHERE \"funcionario\".\"id\" > 0 ";
                    break;
            }

            comando += "AND \"funcionario\".\"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 ";
            comando += "ORDER BY LOWER(\"funcionario\".\"nome\")";

            return FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Método que retorna uma lista de funcionários de uma determinada empresa
        /// </summary>
        /// <param name="pIDEmpresa">Id da empresa que deseja a lista de funcionários</param>
        ///<param name="pPegaInativos"> false = Pega apenas funcionarios ativos, true = Pega funcionarios ativos e inativos</param>
        /// <returns>Lista (FbDataReader) dos funcionários</returns>
        public FbDataReader GetEmpresaDR(int pIDEmpresa, bool pPegaInativos)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                  new FbParameter("@idempresa", FbDbType.Integer)
            };
            parms[0].Value = pIDEmpresa;

            string comando;
            if (pPegaInativos)
                comando = "  SELECT \"id\", \"dscodigo\", \"pis\", \"tipohorario\", \"idhorario\", \"nome\" " +
                            "FROM \"funcionario\" " +
                            "WHERE \"idempresa\" = @idempresa AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 " +
                            "ORDER BY \"nome\"";
            else
                comando = "     SELECT \"id\", \"dscodigo\", \"pis\", \"tipohorario\", \"idhorario\", \"nome\" " +
                                "FROM \"funcionario\" " +
                                "WHERE \"idempresa\" = @idempresa AND \"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 AND \"funcionario\".\"datademissao\" IS NULL " +
                                "ORDER BY \"nome\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Método que retorna uma lista de funcionários de um determinado departamento
        /// </summary>
        /// <param name="pIDEmpresa">ID da empresa</param>
        /// <param name="pIDDepartamento">ID do departamento</param>
        /// <returns>Lista (FbDataReader) dos funcionários</returns>
        public FbDataReader GetDepartamentoDR(int pIDEmpresa, int pIDDepartamento, bool pPegaInativos)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                    new FbParameter("@idempresa", FbDbType.Integer)
                  , new FbParameter("@iddepartamento", FbDbType.Integer)
            };
            parms[0].Value = pIDEmpresa;
            parms[1].Value = pIDDepartamento;

            string comando;
            if (pPegaInativos)
                comando = "SELECT \"id\", \"dscodigo\", \"tipohorario\", \"idhorario\", \"nome\" FROM \"funcionario\" WHERE \"idempresa\" = @idempresa AND \"iddepartamento\" = @iddepartamento AND COALESCE(\"funcionario\".\"excluido\", 0) = 0  ORDER BY \"nome\"";
            else
                comando = "SELECT \"id\", \"dscodigo\", \"tipohorario\", \"idhorario\", \"nome\" FROM \"funcionario\" WHERE \"idempresa\" = @idempresa AND \"iddepartamento\" = @iddepartamento AND COALESCE(\"funcionario\".\"funcionarioativo\",0) = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 AND \"funcionario\".\"datademissao\" IS NULL ORDER BY \"nome\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
        }

        public FbDataReader GetFuncaoDR(int pIDFuncao, bool pPegaInativos)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                  new FbParameter("@idfuncao", FbDbType.Integer)
            };
            parms[0].Value = pIDFuncao;

            string comando;
            if (pPegaInativos)
                comando = "  SELECT \"id\", \"dscodigo\", \"pis\", \"tipohorario\", \"idhorario\", \"nome\" " +
                            "FROM \"funcionario\" " +
                            "WHERE \"idfuncao\" = @idfuncao AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 " +
                            "ORDER BY \"nome\"";
            else
                comando = "     SELECT \"id\", \"dscodigo\", \"pis\", \"tipohorario\", \"idhorario\", \"nome\" " +
                                "FROM \"funcionario\" " +
                                "WHERE \"idfuncao\" = @idfuncao AND \"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 AND \"funcionario\".\"datademissao\" IS NULL " +
                                "ORDER BY \"nome\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
        }

        public FbDataReader GetAtivosDR()
        {
            FbParameter[] parms = new FbParameter[0];

            string comando =

            "   SELECT     \"funcionario\".\"id\"" +
                            ", \"funcionario\".\"nome\"" +
                            ", \"funcionario\".\"dscodigo\" " +
                            ", \"funcionario\".\"matricula\"" +
                            ", \"horario\".\"descricao\" AS \"jornada\"" +
                            ", \"empresa\".\"nome\" AS \"empresa\"" +
                            ", \"departamento\".\"descricao\" AS \"departamento\"" +
                            ", \"funcionario\".\"carteira\"" +
                            ", \"funcionario\".\"dataadmissao\"" +
                            ", \"funcionario\".\"funcionarioativo\"" +
                     "FROM \"funcionario\" " +
                     "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                     "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                     "LEFT JOIN \"horario\" ON \"horario\".\"id\"= \"funcionario\".\"idhorario\" " +
                     " WHERE \"funcionario\".\"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 " +
                     " ORDER BY \"funcionario\".\"nome\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Método que retorna uma lista de funcionários de um determinado departamento
        /// </summary>
        /// <param name="pIDEmpresa">ID da empresa</param>
        /// <param name="pIDDepartamento">ID do departamento</param>
        /// <returns>Lista (FbDataReader) dos funcionários</returns>
        //PAM - 11/12/2009
        public FbDataReader GetDepDR(int pIDEmpresa, int pIDDepartamento, bool pPegaInativos)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                  new FbParameter("@iddepartamento", FbDbType.Integer)
            };
            parms[0].Value = pIDDepartamento;

            string comando;
            if (pPegaInativos)
                comando = "SELECT \"id\", \"dscodigo\", \"tipohorario\", \"idhorario\", \"nome\" FROM \"funcionario\" WHERE \"iddepartamento\" = @iddepartamento AND COALESCE(\"funcionario\".\"excluido\", 0) = 0  ORDER BY \"nome\"";
            else
                comando = "SELECT \"id\", \"dscodigo\", \"tipohorario\", \"idhorario\", \"nome\" FROM \"funcionario\" WHERE \"iddepartamento\" = @iddepartamento AND COALESCE(\"funcionario\".\"funcionarioativo\",0) = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0  ORDER BY \"nome\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Método que retorna o funcionário escolhido para mudança de horário
        /// </summary>
        /// <param name="pID">Id do funcionário</param>
        /// <returns>Lista (FbDataReader) dos funcionários</returns>
        public FbDataReader GetIDDR(int pID)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                  new FbParameter("@id", FbDbType.Integer)
            };
            parms[0].Value = pID;

            string aux = "SELECT \"funcionario\".\"id\", \"funcionario\".\"dscodigo\", \"funcionario\".\"tipohorario\", \"funcionario\".\"idhorario\", \"funcionario\".\"nome\" FROM \"funcionario\" WHERE \"id\" = @id";

            return FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
        }

        #endregion

        #region Auxiliares

        /// <summary>
        /// Pega o numero de funcionarios ativos de todo o banco de dados que esta sendo pesquisado
        /// </summary>
        /// <returns>Numero de funcionarios do banco</returns>
        
        public int GetNumFuncionarios()
        {
            FbParameter[] parms = new FbParameter[0] { };

            //Select considerando somente os funcionários ativos
            //22/12/2009 - crnc
            string aux = "SELECT COUNT(*) " +
                         "FROM \"funcionario\" " +
                         "WHERE COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1 " +
                         "AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 ";

            return (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
        }

        public bool MudaCodigoFuncionario(int pFuncionarioID, string pCodigoNovo, DateTime pData)
        {
            //Verifica se o código já está atribuido para algun funcionário
            if (this.VerificaCodigoDuplicado(pCodigoNovo) == true)
            {
                throw new Exception("O código " + pCodigoNovo + " já está sendo utilizado por outro funcionário.");
            }

            //Rotina que troca o código do funcionário
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Carrega o registro do funcionário
                        Modelo.Funcionario objFunc = new Modelo.Funcionario();
                        objFunc = this.LoadObject(pFuncionarioID);

                        //Altera o código do funcionario
                        FbParameter[] parms = new FbParameter[]
                        { 
                              new FbParameter("@id", FbDbType.Integer)
                            , new FbParameter("@dscodigo", FbDbType.VarChar)
                            , new FbParameter("@data", FbDbType.Date)
                        };
                        parms[0].Value = pFuncionarioID;
                        parms[1].Value = pCodigoNovo;
                        parms[2].Value = pData;

                        string aux = "UPDATE \"funcionario\" SET \"dscodigo\" = @dscodigo WHERE \"id\" = @id";

                        string aux1 = "UPDATE \"marcacao\" SET \"dscodigo\" = @dscodigo WHERE \"marcacao\".\"idfuncionario\" = @id AND \"marcacao\".\"data\" >= @data";

                        FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, aux, true, parms);
                        FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, aux1, true, parms);

                        //Registra a mudança do código do funcionário
                        Modelo.MudCodigoFunc objMudanca = new Modelo.MudCodigoFunc();
                        DAL.FB.MudCodigoFunc dalMudanca = DAL.FB.MudCodigoFunc.GetInstancia;

                        objMudanca.Codigo = dalMudanca.MaxCodigo();
                        objMudanca.IdFuncionario = objFunc.Id;
                        objMudanca.DSCodigoAntigo = objFunc.Dscodigo;
                        objMudanca.DSCodigoNovo = pCodigoNovo;
                        objMudanca.Datainicial = System.DateTime.Today;
                        objMudanca.Idempresa = objFunc.Idempresa;
                        objMudanca.Iddepartamento = objFunc.Iddepartamento;
                        objMudanca.Idhorarionormal = objFunc.Idhorario;
                        objMudanca.Tipohorario = objFunc.Tipohorario;

                        dalMudanca.Incluir(trans, objMudanca);

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "dscodigo", Convert.ToDouble(((Modelo.Funcionario)obj).Dscodigo), 0) > 0)
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "dscodigo", Convert.ToDouble(((Modelo.Funcionario)obj).Dscodigo), ((Modelo.Funcionario)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizando em outro registro. Verifique.");
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        private static void AuxManutencao(FbTransaction trans, Modelo.ModeloBase obj)
        {
            DAL.FB.FuncionarioHistorico dalFuncionarioHistorico = DAL.FB.FuncionarioHistorico.GetInstancia;

            foreach (Modelo.FuncionarioHistorico fh in ((Modelo.Funcionario)obj).Historico)
            {
                fh.Idfuncionario = ((Modelo.Funcionario)obj).Id;
                switch (fh.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalFuncionarioHistorico.Incluir(trans, fh);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFuncionarioHistorico.Alterar(trans, fh);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFuncionarioHistorico.Excluir(trans, fh);
                        break;
                }
            }
        }

        public void Incluir(List<Modelo.Funcionario> funcionarios, bool salvarHistorico)
        {
            FbParameter[] parms = GetParameters();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        FbCommand cmd;
                        foreach (Modelo.Funcionario func in funcionarios)
                        {
                            SetDadosInc(func);
                            SetParameters(parms, func);
                            cmd = DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
                            func.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);
                            if (salvarHistorico)
                                AuxManutencao(trans, func);

                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw (ex);
                    }
                }
                conn.Close();
            }
        }

        public DataTable GetPisCodigo()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Listagem em List

        public List<int> GetIds()
        {
            List<int> lista = new List<int>();

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"id\" FROM \"funcionario\"", new FbParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]));
                }
            }

            return lista;
        }


        public List<Modelo.Funcionario> GetAllList(bool pegaTodos)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            FbParameter[] parms = new FbParameter[] { };

            string aux = "SELECT \"funcionario\".* " +
                         ", \"horario\".\"descricao\" AS \"jornada\"" +
                         ", \"empresa\".\"nome\" AS \"empresa\"" +
                         " FROM \"funcionario\" " +
                         " LEFT JOIN \"empresa\"  ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                         " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"";
            if (!pegaTodos)
            {
                aux += " WHERE COALESCE(\"funcionario\".\"excluido\",0)=0 AND COALESCE(\"funcionario\".\"funcionarioativo\",0)=1";
            }

            aux += " ORDER BY \"funcionario\".\"nome\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            dr.Close();

            return lista;
        }

        public List<Modelo.Funcionario> getLista(int pempresa)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@idempresa", FbDbType.Integer)
            };
            parms[0].Value = pempresa;

            string aux = "SELECT \"funcionario\".* " +
                         ", \"horario\".\"descricao\" AS \"jornada\"" +
                         ", \"empresa\".\"nome\" AS \"empresa\"" +
                         " FROM \"funcionario\" " +
                         " LEFT JOIN \"empresa\"  ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                         " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                         " WHERE COALESCE(\"funcionario\".\"excluido\",0)=0 AND COALESCE(\"funcionario\".\"funcionarioativo\",0)=1 " +
                         " AND \"funcionario\".\"idempresa\" = @idempresa " +
                         " ORDER BY \"funcionario\".\"nome\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            dr.Close();

            return lista;
        }

        public List<Modelo.Funcionario> getLista(int pempresa, int pdepartamento)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@idempresa", FbDbType.Integer),
                new FbParameter("@iddepartamento", FbDbType.Integer)
            };
            parms[0].Value = pempresa;
            parms[1].Value = pdepartamento;

            string aux = "SELECT \"funcionario\".* " +
                         ", \"horario\".\"descricao\" AS \"jornada\"" +
                         ", \"empresa\".\"nome\" AS \"empresa\"" +
                         " FROM \"funcionario\" " +
                         " LEFT JOIN \"empresa\"  ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                         " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                         " WHERE COALESCE(\"funcionario\".\"excluido\",0)=0 AND COALESCE(\"funcionario\".\"funcionarioativo\",0)=1 " +
                         " AND \"funcionario\".\"idempresa\" = @idempresa " +
                         " AND \"funcionario\".\"iddepartamento\" = @iddepartamento " +
                         " ORDER BY \"funcionario\".\"nome\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            dr.Close();

            return lista;
        }

        public Modelo.Funcionario RetornaFuncDsCodigo(string pCodigo)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                  new FbParameter("@dscodigo", FbDbType.VarChar)
            };
            parms[0].Value = pCodigo;

            string aux = " SELECT   \"funcionario\".*" +
                                    ", \"horario\".\"descricao\" AS \"jornada\" " +
                                    ", \"empresa\".\"nome\" AS \"empresa\" " +
                                    ", \"departamento\".\"descricao\" AS \"departamento\" " +
                             " FROM \"funcionario\"" +
                             " LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             " LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             " LEFT JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                             " WHERE \"funcionario\".\"dscodigo\" = @dscodigo ";

            FbDataReader dr;
            dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            if (dr.Read())
            {
                AuxSetInstance(dr, objFuncionario);
            }
            else
            {
                objFuncionario = null;
            }
            return objFuncionario;

        }

        public Modelo.Funcionario RetornaFuncPis(int idFuncionario, string pis)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                  new FbParameter("@pis", FbDbType.VarChar),
                  new FbParameter("@id", FbDbType.Integer)
            };
            parms[0].Value = pis;
            parms[1].Value = idFuncionario;

            string aux = " SELECT   \"funcionario\".*" +
                                    ", \"horario\".\"descricao\" AS \"jornada\" " +
                                    ", \"empresa\".\"nome\" AS \"empresa\" " +
                                    ", \"departamento\".\"descricao\" AS \"departamento\" " +
                             " FROM \"funcionario\"" +
                             " LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             " LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             " LEFT JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                             " WHERE \"funcionario\".\"pis\" = @pis AND \"funcionario\".\"id\" <> @id AND \"funcionario\".\"datademissao\" is NULL AND \"funcionario\".\"excluido\" <> 1";

            FbDataReader dr;
            dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            if (dr.Read())
            {
                AuxSetInstance(dr, objFuncionario);
            }
            else
            {
                objFuncionario = null;
            }
            return objFuncionario;
        }

        /// <summary>
        /// Método utilizado para carregar os funcionarios na tabela de marcações
        /// </summary>
        /// <param name="tipo">1-Empresa, 2-Departamento, 3-Todos</param>
        /// <param name="identificacao">id da empresa ou do departamento</param>
        /// <returns>Lista dos funcionários</returns>
        public List<Modelo.Funcionario> GetTabelaMarcacao(int tipo, int identificacao, string consultaNomeFuncionario)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            FbDataReader dr = GetTabelaMarcacaoDR(tipo, identificacao, consultaNomeFuncionario);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            return lista;
        }

        /// <summary>
        /// Pega os funcionarios por departamento e retorna uma lista
        /// </summary>
        /// <param name="pIDDepartamento">Id do departamento</param>
        /// <param name="pPegaInativos">true = pega ativos e inativos, false = pega somente ativos</param>
        /// <returns></returns>
        //PAM - 11/12/2009
        public List<Modelo.Funcionario> GetPorDepartamentoList(int pIDDepartamento)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@iddepartamento", FbDbType.Integer)
            };

            parms[0].Value = pIDDepartamento;

            string aux = "SELECT \"funcionario\".* " +
                         ", \"horario\".\"descricao\" AS \"jornada\"" +
                         ", \"empresa\".\"nome\" AS \"empresa\"" +
                         " FROM \"funcionario\" " +
                         " LEFT JOIN \"empresa\"  ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                         " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                         " WHERE \"funcionario\".\"iddepartamento\" = @iddepartamento AND" +
                         " COALESCE(\"funcionario\".\"excluido\",0)=0 AND COALESCE(\"funcionario\".\"funcionarioativo\",0)=1" +
                         " ORDER BY \"funcionario\".\"nome\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            dr.Close();

            return lista;
        }

        /// <summary>
        /// Retorna do banco a lista de funcionarios daquela funcao
        /// </summary>
        /// <param name="pIdFuncao"> ID da funcao</param>
        /// <returns> Lista de funcionarios daquela funcao</returns>
        //PAM - 11/12/2009
        public List<Modelo.Funcionario> GetPorFuncaoList(int pIdFuncao)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@idfuncao", FbDbType.Integer)
            };
            parms[0].Value = pIdFuncao;

            string aux = "SELECT \"funcionario\".* " +
                         ", \"horario\".\"descricao\" AS \"jornada\"" +
                         ", \"empresa\".\"nome\" AS \"empresa\"" +
                         " FROM \"funcionario\" " +
                         " LEFT JOIN \"empresa\"  ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                         " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                         " WHERE \"funcionario\".\"idfuncao\" = @idfuncao AND" +
                         " COALESCE(\"funcionario\".\"excluido\",0)=0 AND COALESCE(\"funcionario\".\"funcionarioativo\",0)=1" +
                         " ORDER BY \"funcionario\".\"nome\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            dr.Close();

            return lista;
        }

        /// <summary>
        /// Pega todos os funcionarios que tem aquele horario
        /// </summary>
        /// <param name="pIdHorario">Id do horário</param>
        /// <returns>Lista (List) de funcionarios daquele horario</returns>
        //PAM 05/04/2010
        public List<Modelo.Funcionario> GetPorHorario(int pIdHorario)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@idhorario", FbDbType.Integer)
            };

            parms[0].Value = pIdHorario;

            string aux = "SELECT \"funcionario\".* " +
                         " FROM \"funcionario\" " +
                         " LEFT JOIN \"horario\"  ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"" +
                         " WHERE \"funcionario\".\"idhorario\" = @idhorario AND" +
                         " COALESCE(\"funcionario\".\"excluido\",0)=0 AND COALESCE(\"funcionario\".\"funcionarioativo\",0)=1" +
                         " ORDER BY \"funcionario\".\"nome\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            dr.Close();

            return lista;
        }

        public Hashtable GetHashCodigoId()
        {
            Hashtable lista = new Hashtable();

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"codigo\", \"id\" FROM \"funcionario\"", new FbParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }

            return lista;
        }

        public Hashtable GetHashCodigoFunc()
        {
            Hashtable lista = new Hashtable();

            string aux = "SELECT   \"funcionario\".*" +
                                    ", \"horario\".\"descricao\" AS \"jornada\" " +
                                    ", \"empresa\".\"nome\" AS \"empresa\" " +
                                    ", \"departamento\".\"descricao\" AS \"departamento\" " +
                             "FROM \"funcionario\"" +
                             "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             "LEFT JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"";

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, aux, new FbParameter[] { });
            Modelo.Funcionario obj = null;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    obj = new Modelo.Funcionario();
                    AuxSetInstance(dr, obj);
                    lista.Add(Convert.ToInt32(dr["codigo"]), obj);
                }
            }

            return lista;
        }

        public Hashtable GetHashIdFunc()
        {
            Hashtable lista = new Hashtable();

            string aux = "SELECT   \"funcionario\".*" +
                                    ", \"horario\".\"descricao\" AS \"jornada\" " +
                                    ", \"empresa\".\"nome\" AS \"empresa\" " +
                                    ", \"departamento\".\"descricao\" AS \"departamento\" " +
                             "FROM \"funcionario\"" +
                             "LEFT JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" " +
                             "LEFT JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" " +
                             "LEFT JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"";

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, aux, new FbParameter[] { });
            Modelo.Funcionario obj = null;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    obj = new Modelo.Funcionario();
                    AuxSetInstance(dr, obj);
                    lista.Add(Convert.ToInt32(dr["id"]), obj);
                }
            }

            return lista;
        }

        public string GetDsCodigo(string pPis)
        {
            string aux;
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@pis", FbDbType.VarChar, 20)
               
            };
            parms[0].Value = pPis;
            if (pPis != null && pPis != "")
            {
                aux = "SELECT \"dscodigo\" FROM \"funcionario\" WHERE \"pis\" = @pis";
                return (string)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
            }
            else
            {
                return null;
            }

        }

        #endregion

        #endregion

        #region IFuncionario Members


        public DataTable GetPorEmpresa(string pEmpresas)
        {
            throw new NotImplementedException();
        }

        public IList<Modelo.Funcionario> GetPorEmpresaList(string pEmpresas)
        {
            throw new NotImplementedException();
        }

        public DataTable GetRelatorioAbsenteismo(int tipo, string empresas, string departamentos, string funcionarios)
        {
            throw new NotImplementedException();
        }

        #endregion

        public int GetIdDsCodigo(string pDsCodigo)
        {
            try
            {
                FbParameter[] parms = new FbParameter[] 
                {
                    new FbParameter("@dscodigo", SqlDbType.VarChar)
                };
                parms[0].Value = pDsCodigo;

                string aux = "SELECT id FROM funcionario WHERE dscodigo = @dscodigo";

                return (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int GetIdDsCodigoProximidade(string pDsCodigo)
        {
            try
            {
                FbParameter[] parms = new FbParameter[] 
                {
                    new FbParameter("@dscodigo", SqlDbType.VarChar)
                };
                parms[0].Value = pDsCodigo;

                string auxCount = "SELECT count(id) FROM funcionario WHERE dscodigo like '%@dscodigo'";
                int qtd = (int)FB.DataBase.ExecuteScalar(CommandType.Text, auxCount, parms);
                if (qtd > 1)
                    return -1;

                string aux = "SELECT id FROM funcionario WHERE dscodigo like '%@dscodigo'";

                return (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }


        #region IFuncionario Members


        public Modelo.Funcionario LoadPorCPF(string CPF)
        {
            throw new NotImplementedException();
        }

        public Modelo.Funcionario LoadAtivoPorCPF(string CPF)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetAllListLike(bool pegaTodos, string nome)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetAllListByIds(string funcionarios)
        {
            throw new NotImplementedException();
        }
        #endregion


        public Modelo.Funcionario LoadObjectByCodigo(int codigo)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetExcluidosList()
        {
            throw new NotImplementedException();
        }


        public DataTable GetPisCodigo(bool webApi)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Funcionario> GetAllListPorContrato(int idContrato)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Funcionario> GetAllListContratos()
        {
            throw new NotImplementedException();
        }
        public IList<Modelo.Proxy.pxyFuncionarioRelatorio> GetRelFuncionariosRelatorios(string filtro)
        {
            throw new NotImplementedException();
        }

        public IList<Modelo.Funcionario> GetFuncionariosPorIds(string pIDs)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.FechamentoPonto> FechamentoPontoFuncionario(List<int> ids)
        {
            throw new NotImplementedException();
        }


        public void AtualizaHorariosFuncionariosMudanca(List<int> idsFuncionarios)
        {
            throw new NotImplementedException();
        }

        public void AtualizaIdIntegracaoPainel(Modelo.Funcionario funcionarioIdIntegracaoPnl)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos)
        {
            throw new NotImplementedException();
        }
        
        public Modelo.Funcionario GetFuncionarioPorCpfeMatricula(Int64 cpf, string matricula)
        {
            throw new NotImplementedException();
        }


        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(int idDep, int idCont, int idEmp)
        {
            throw new NotImplementedException();
        }


        public IList<Modelo.Proxy.PxyFuncionarioCabecalhoRel> GetFuncionariosCabecalhoRel(IList<int> IdFuncs)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Funcionario> GetAllListPorPis(List<string> lPis)
        {
            throw new NotImplementedException();
        }

        public List<int> GetAllListPorCPF (List<string> lCPF)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos, IList<int> idsFuncs)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetPorHorarioVigencia(int idHorario)
        {
            throw new NotImplementedException();
        }

public List<int> GetIdsFuncsAtivos(string condicao)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> RetornaFuncDsCodigos(List<string> pCodigo)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Funcionario> GetAllPisDuplicados(List<string> lPis)
        {
            throw new NotImplementedException();
        }

        public Modelo.Funcionario LoadObjectByPis(string pis)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.pxyFuncionarioGrid> GetAllGrid()
        {
            throw new NotImplementedException();
        }


        public DataTable GetOrdenadoPorNomeRel(List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable GetOrdenadoPorCodigoRel(List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable GetPorDepartamentoRel(List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable GetRelatorio(List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable GetPorHorarioRel(List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable GetAtivosInativosRel(bool pAtivo, List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.PxyFuncionarioDiaUtil> GetDiaUtilFuncionario(List<int> idsFuncs, DateTime dataIni, DateTime dataFin)
        {
            throw new NotImplementedException();
        }

        public DataTable CarregarTodosParaAPI()
        {
            throw new NotImplementedException();
        }

        public DataTable CarregarHorarioMarcacao(int idMarcacao, int diaSemana)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetAllFuncsListPorCPF(List<string> lCPF)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos, IList<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public DataTable CarregarTodosParaAPI(short Ativo, short Excluido)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> LoadAtivoPorListCPF(List<string> CPFs)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Funcionario> GetProximoOuAnterior(int tipo, int identificacao, int qtdRegistros, string nomeFuncionario, int tipoOrdenacao, int proximoOuAnterior)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIDsByDsCodigos(List<string> lDsCodigos)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIDsByTipo(int? pTipo, List<int> pIdentificacao, bool pegaExcluidos, bool pegaInativos)
        {
            throw new NotImplementedException();
        }

        public DataTable GetPeriodoFechamentoPonto(List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp, bool consideraInativos = true, bool consideraExcluidos = true)
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

        public List<pxyFuncionarioGrid> GetAllGrid(int flag)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIds(bool VerificarPermissaoUsuario)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(int idFuncao, int idDep, int idCont, int idEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido)
        {
            throw new NotImplementedException();
        }

        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }

        public DataTable GetPisCodigo(List<string> pis)
        {
            throw new NotImplementedException();
        }
    }
}
