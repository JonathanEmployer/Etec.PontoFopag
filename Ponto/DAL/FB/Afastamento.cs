using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using Modelo;
using System.Data.SqlClient;
using Modelo.Proxy.Relatorios;

namespace DAL.FB
{
    public class Afastamento : DAL.FB.DALBase, DAL.IAfastamento
    {
        public string SELECTRELF { get; set; }
        public string SELECTRELD { get; set; }
        public string SELECTRELE { get; set; }

        public string SELECTMAR { get; set; }
        public string VERIFICA { get; set; }

        private Afastamento()
        {
            GEN = "GEN_afastamento_id";

            TABELA = "afastamento";

            SELECTPID = "SELECT * FROM \"afastamento\" WHERE \"id\" = @id";

            SELECTRELF = " SELECT     \"afastamento\".\"id\"" +
                                    ", \"ocorrencia\".\"descricao\" AS \"ocorrencia\"" +
                                    ", \"funcionario\".\"nome\" || ' - ' || \"departamento\".\"descricao\" || ' - ' || \"empresa\".\"nome\" AS \"nome\"" + 
                                    ", \"afastamento\".\"datai\"" +
                                    ", \"afastamento\".\"dataf\"" + 
                                    ", \"afastamento\".\"abonado\""+
                            " FROM    \"afastamento\" " +
                            " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"afastamento\".\"idfuncionario\"" + 
                            " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"" +
                            " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\"" +                              
                            " INNER JOIN \"ocorrencia\" ON \"ocorrencia\".\"id\" = \"afastamento\".\"idocorrencia\"" +
                            " WHERE \"afastamento\".\"id\" > 0 AND \"afastamento\".\"tipo\" = 0";

            SELECTRELD = " SELECT     \"afastamento\".\"id\"" +
                                    ", \"ocorrencia\".\"descricao\" AS \"ocorrencia\"" +
                                    ", \"departamento\".\"descricao\" || ' - ' || \"empresa\".\"nome\" AS \"nome\"" +
                                    ", \"afastamento\".\"datai\"" +
                                    ", \"afastamento\".\"dataf\"" +
                                    ", \"afastamento\".\"abonado\"" +
                            " FROM    \"afastamento\" " +                            
                            " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"afastamento\".\"iddepartamento\"" +
                            " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"departamento\".\"idempresa\"" +
                            " INNER JOIN \"ocorrencia\" ON \"ocorrencia\".\"id\" = \"afastamento\".\"idocorrencia\"" +
                            " WHERE \"afastamento\".\"id\" > 0 AND \"afastamento\".\"tipo\" = 1";

            SELECTRELE = " SELECT     \"afastamento\".\"id\"" +
                                    ", \"ocorrencia\".\"descricao\" AS \"ocorrencia\"" +
                                    ", \"empresa\".\"nome\" AS \"nome\"" +
                                    ", \"afastamento\".\"datai\"" +
                                    ", \"afastamento\".\"dataf\"" +
                                    ", \"afastamento\".\"abonado\"" +
                            " FROM    \"afastamento\" " +
                            " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"afastamento\".\"idempresa\"" +
                            " INNER JOIN \"ocorrencia\" ON \"ocorrencia\".\"id\" = \"afastamento\".\"idocorrencia\"" +
                            " WHERE \"afastamento\".\"id\" > 0 AND \"afastamento\".\"tipo\" = 2";

            SELECTALL = "   SELECT   \"afastamento\".\"id\"" +
                                    ", case when \"tipo\" = 2 then (SELECT \"empresa\".\"nome\" FROM \"empresa\" WHERE \"empresa\".\"id\" = \"afastamento\".\"idempresa\") " +
                                    "       when \"tipo\" = 1 then (SELECT \"departamento\".\"descricao\" FROM \"departamento\" WHERE \"departamento\".\"id\" = \"afastamento\".\"iddepartamento\") " +
                                    "       when \"tipo\" = 0 then (SELECT \"funcionario\".\"nome\" FROM \"funcionario\" WHERE \"funcionario\".\"id\" = \"afastamento\".\"idfuncionario\") end AS \"nome\" " +
                                    ", \"ocorrencia\".\"descricao\" as \"ocorrencia\"" +
                                    ", \"afastamento\".\"codigo\"" +
                                    ", \"afastamento\".\"datai\"" +
                                    ", \"afastamento\".\"dataf\"" +
                                    ", case when \"afastamento\".\"tipo\" = 0 then 'Individual' when \"afastamento\".\"tipo\" = 1 then 'Departamento' when \"afastamento\".\"tipo\" = 2 then 'Empresa' else '' end as \"tipo\"" +
                             " FROM \"afastamento\"  INNER JOIN \"ocorrencia\"  ON \"ocorrencia\".\"id\" = \"afastamento\".\"idocorrencia\"";

            SELECTMAR = "   SELECT * FROM \"afastamento\" " +
                             " WHERE \"afastamento\".\"datai\" = @data" +  
                             " AND \"afastamento\".\"dataf\" = @data" +  
                             " AND \"afastamento\".\"idfuncionario\" = @idfuncionario" +
                             " AND \"afastamento\".\"tipo\" = 0";

            VERIFICA = "   SELECT COALESCE(COUNT(\"id\"), 0) AS \"qt\"" + 
                            " FROM \"afastamento\"" +
                            " WHERE ((@datainicial >= \"datai\" AND @datainicial <= \"dataf\") " +
                            " OR (@datafinal >= \"datai\" AND @datafinal <= \"dataf\") " +
                            " OR (@datainicial <= \"datai\" AND @datafinal >= \"dataf\")) " +
                            " AND \"tipo\" = @tipo " + 
                            " AND \"id\" <> @id " +
                            " AND ((\"idfuncionario\" = @identificacao AND \"tipo\" = 0)" + 
                            " OR (\"idempresa\" = @identificacao AND \"tipo\" = 2)" +
                            " OR (\"iddepartamento\" = @identificacao AND \"tipo\" = 1))";

            INSERT = "  INSERT INTO \"afastamento\"" +
                                        "(\"codigo\", \"descricao\", \"idocorrencia\", \"tipo\", \"abonado\", \"datai\", \"dataf\", \"idfuncionario\", \"idempresa\", \"iddepartamento\", \"horai\", \"horaf\", \"parcial\", \"semcalculo\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @idocorrencia, @tipo, @abonado, @datai, @dataf, @idfuncionario, @idempresa, @iddepartamento, @horai, @horaf, @parcial, @semcalculo, @incdata, @inchora, @incusuario)";                                     

            UPDATE = "  UPDATE \"afastamento\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"idocorrencia\" = @idocorrencia " +
                                        ", \"tipo\" = @tipo " +
                                        ", \"abonado\" = @abonado " +
                                        ", \"datai\" = @datai " +
                                        ", \"dataf\" = @dataf " +
                                        ", \"idfuncionario\" = @idfuncionario " +
                                        ", \"idempresa\" = @idempresa " +
                                        ", \"iddepartamento\" = @iddepartamento " +
                                        ", \"horai\" = @horai " +
                                        ", \"horaf\" = @horaf " +
                                        ", \"parcial\" = @parcial " +
                                        ", \"semcalculo\" = @semcalculo " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"afastamento\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"afastamento\"";

        }

        #region Singleton

        private static volatile FB.Afastamento _instancia = null;

        public static FB.Afastamento GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Afastamento))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Afastamento();
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
                    AtribuiCampos(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Afastamento();
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

        private void AtribuiCampos(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Afastamento)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Afastamento)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Afastamento)obj).IdOcorrencia = Convert.ToInt32(dr["idocorrencia"]);
            ((Modelo.Afastamento)obj).Tipo = Convert.ToInt32(dr["tipo"]);
            ((Modelo.Afastamento)obj).Abonado = Convert.ToInt16(dr["abonado"]);
            ((Modelo.Afastamento)obj).Datai = Convert.ToDateTime(dr["datai"]);
            ((Modelo.Afastamento)obj).Dataf = Convert.ToDateTime(dr["dataf"]);
            ((Modelo.Afastamento)obj).IdFuncionario = (dr["idfuncionario"] is DBNull ? 0 : Convert.ToInt32(dr["idfuncionario"]));
            ((Modelo.Afastamento)obj).IdEmpresa = (dr["idempresa"] is DBNull ? 0 : Convert.ToInt32(dr["idempresa"]));
            ((Modelo.Afastamento)obj).IdDepartamento = (dr["iddepartamento"] is DBNull ? 0 : Convert.ToInt32(dr["iddepartamento"]));
            ((Modelo.Afastamento)obj).Horai = Convert.ToString(dr["horai"]);
            ((Modelo.Afastamento)obj).Horaf = Convert.ToString(dr["horaf"]);
            ((Modelo.Afastamento)obj).Parcial = Convert.ToInt16(dr["parcial"]);
            ((Modelo.Afastamento)obj).SemCalculo = Convert.ToInt16(dr["semcalculo"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@idocorrencia", FbDbType.Integer),
				new FbParameter ("@tipo", FbDbType.Integer),
				new FbParameter ("@abonado", FbDbType.SmallInt),
				new FbParameter ("@datai", FbDbType.Date),
				new FbParameter ("@dataf", FbDbType.Date),
				new FbParameter ("@idfuncionario", FbDbType.Integer),
				new FbParameter ("@idempresa", FbDbType.Integer),
				new FbParameter ("@iddepartamento", FbDbType.Integer),
				new FbParameter ("@horai", FbDbType.VarChar),
				new FbParameter ("@horaf", FbDbType.VarChar),
				new FbParameter ("@parcial", FbDbType.SmallInt),
				new FbParameter ("@semcalculo", FbDbType.SmallInt),
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
            parms[1].Value = ((Modelo.Afastamento)obj).Codigo;
            parms[2].Value = ((Modelo.Afastamento)obj).Descricao;
            parms[3].Value = ((Modelo.Afastamento)obj).IdOcorrencia;
            parms[4].Value = ((Modelo.Afastamento)obj).Tipo;
            parms[5].Value = ((Modelo.Afastamento)obj).Abonado;
            parms[6].Value = ((Modelo.Afastamento)obj).Datai;
            parms[7].Value = ((Modelo.Afastamento)obj).Dataf;
            if (((Modelo.Afastamento)obj).IdFuncionario > 0)
            {
                parms[8].Value = ((Modelo.Afastamento)obj).IdFuncionario;
            }
            if (((Modelo.Afastamento)obj).IdEmpresa > 0)
            {
                parms[9].Value = ((Modelo.Afastamento)obj).IdEmpresa;
            }
            if (((Modelo.Afastamento)obj).IdDepartamento > 0)
            {
                parms[10].Value = ((Modelo.Afastamento)obj).IdDepartamento;
            }
            parms[11].Value = ((Modelo.Afastamento)obj).Horai;
            parms[12].Value = ((Modelo.Afastamento)obj).Horaf;
            parms[13].Value = ((Modelo.Afastamento)obj).Parcial;
            parms[14].Value = ((Modelo.Afastamento)obj).SemCalculo;
            parms[15].Value = ((Modelo.Afastamento)obj).Incdata;
            parms[16].Value = ((Modelo.Afastamento)obj).Inchora;
            parms[17].Value = ((Modelo.Afastamento)obj).Incusuario;
            parms[18].Value = ((Modelo.Afastamento)obj).Altdata;
            parms[19].Value = ((Modelo.Afastamento)obj).Althora;
            parms[20].Value = ((Modelo.Afastamento)obj).Altusuario;
        }

        public Modelo.Afastamento LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
            try
            {
                SetInstance(dr, objAfastamento);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objAfastamento;
        }

        public DataTable GetPorAfastamentoRel(DateTime pDataInicial, DateTime pDataFinal, string pEmpresas, string pDepartamentos, string pFuncionarios, int pTipo)
        {
            FbParameter[] parms = new FbParameter[]
                {
                    new FbParameter("@datai", FbDbType.Date),
                    new FbParameter("@dataf", FbDbType.Date)
                };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            DataTable dt = new DataTable();

            string aux = "";

            switch (pTipo)
            {
                case 0: aux = @SELECTRELE + " AND \"afastamento\".\"idempresa\"  IN " + pEmpresas; break;
                case 1: aux = @SELECTRELD + " AND \"afastamento\".\"iddepartamento\" IN" + pDepartamentos; break;
                case 2: aux = @SELECTRELF + " AND \"afastamento\".\"idfuncionario\" IN " + pFuncionarios; break;
                default: break;
            }
            
            aux += " AND \"afastamento\".\"datai\" >= @datai AND \"afastamento\".\"dataf\" <= @dataf";

            switch (pTipo)
            {
                case 0: aux = aux += " ORDER BY LOWER(\"empresa\".\"nome\"), \"datai\""; break;
                case 1: aux = aux += " ORDER BY LOWER(\"departamento\".\"descricao\"), \"datai\""; break;
                case 2: aux = aux += " ORDER BY LOWER(\"funcionario\".\"nome\"), \"datai\""; break;
                default: break;
            }
  
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetAfastamentoPorOcorrenciaRel(string pEmpresas, string pDepartamentos, string pFuncionarios, int pTipo, int pIdOcorrencia)
        {
            FbParameter[] parms = new FbParameter[1]
                {
                    new FbParameter("@idocorrencia", FbDbType.Integer)
                };

            parms[0].Value = pIdOcorrencia;

            DataTable dt = new DataTable();

            string aux = "";

            switch (pTipo)
            {
                case 0: aux = @SELECTRELE + " AND \"afastamento\".\"idempresa\" IN " + pEmpresas; break;
                case 1: aux = @SELECTRELD + " AND \"afastamento\".\"iddepartamento\" IN" + pDepartamentos; break;
                case 2: aux = @SELECTRELF + " AND \"afastamento\".\"idfuncionario\" IN " + pFuncionarios; break;
                default: break;
            }

            if (pIdOcorrencia > 0)
                aux += " AND \"afastamento\".\"idocorrencia\" = @idocorrencia ";
            switch (pTipo)
            {
                case 0: aux += " ORDER BY LOWER(\"empresa\".\"nome\"), \"ocorrencia\", \"datai\""; break;
                case 1: aux += " ORDER BY LOWER(\"departamento\".\"descricao\"), \"ocorrencia\", \"datai\""; break;
                case 2: aux += " ORDER BY LOWER(\"funcionario\".\"nome\"),\"ocorrencia\", \"datai\""; break;
                default: break;
            }

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        private FbDataReader getPeriodoFuncionario(int pIdFuncionario)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@idfuncionario", FbDbType.Integer, 4) 
            };
            parms[0].Value = pIdFuncionario;

            string aux = "SELECT * " +
                         "FROM \"afastamento\" " +
                         "WHERE \"tipo\" = 0 and \"idfuncionario\" = @idfuncionario " +
                         "ORDER BY \"id\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
        }
        private FbDataReader getPeriodoDepartemento(int pIdEmpresa, int pIdDepartamento)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                  new FbParameter("@idempresa", FbDbType.Integer, 4) 
                , new FbParameter("@iddepartamento", FbDbType.Integer, 4) 
            };
            parms[0].Value = pIdEmpresa;
            parms[1].Value = pIdDepartamento;

            string aux =    "SELECT * " + 
                            "FROM \"afastamento\" " + 
                            "WHERE \"tipo\" = 1 and \"idempresa\" = @idempresa and \"iddepartamento\" = @iddepartamento " +
                            "ORDER BY \"id\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
        }

        private FbDataReader getPeriodoEmpresa(int pIdEmpresa)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                  new FbParameter("@idempresa", FbDbType.Integer, 4) 
            };
            parms[0].Value = pIdEmpresa;

            string aux =    "SELECT * " + 
                            "FROM \"afastamento\" " + 
                            "WHERE \"tipo\" = 2 and \"idempresa\" = @idempresa ORDER BY \"id\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
        }

        public List<Modelo.Afastamento> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@datai", FbDbType.Date) 
                ,new FbParameter("@dataf", FbDbType.Date) 
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string aux = "SELECT * FROM \"afastamento\" WHERE ((@datai >= \"datai\" AND @datai <= \"dataf\") " +
                              " OR (@dataf >= \"datai\" AND @dataf <= \"dataf\") " +
                              " OR (@datai <= \"datai\" AND @dataf >= \"dataf\")) " +
                              " ORDER BY \"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Afastamento> ret = new List<Modelo.Afastamento>();

            Modelo.Afastamento objAfastamento = null;
            while (dr.Read())
            {
                objAfastamento = new Modelo.Afastamento();
                AtribuiCampos(dr, objAfastamento);
                ret.Add(objAfastamento);
            }
            dr.Close();

            return ret;
        }        

        private bool VerificaPeriodo(DateTime pData, FbDataReader dr)
        {
            Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
            while (dr.Read())
            {
                AtribuiCampos(dr, objAfastamento);

                if (pData >= objAfastamento.Datai && pData <= objAfastamento.Dataf)
                {
                    return true;
                }
            }
            dr.Close();
            return false;
        }

        private bool VerificaOcorrencia(DateTime pData, FbDataReader dr, ref string pOcorrencia, ref int pAbono, ref bool pSemCalc, ref string pAbonoD, ref string pAbonoN)
        {
            bool ret = false;

            pOcorrencia = "";
            pAbono = 0;
            pSemCalc = false;
            pAbonoD = "--:--";
            pAbonoN = "--:--";

            Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
            while (dr.Read())
            {
                AtribuiCampos(dr, objAfastamento);                
                if (pData >= objAfastamento.Datai && pData <= objAfastamento.Dataf)
                {
                    Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
                    DAL.FB.Ocorrencia dalOcorrencia = DAL.FB.Ocorrencia.GetInstancia;
                    objOcorrencia = dalOcorrencia.LoadObject(objAfastamento.IdOcorrencia);
                    pOcorrencia = objOcorrencia.Descricao;

                    if (objAfastamento.Horai == "--:--" && objAfastamento.Horaf == "--:--" && objAfastamento.Abonado == 1)
                        pAbono = 2;
                    else
                        pAbono = objAfastamento.Abonado;

                    pSemCalc = Convert.ToBoolean(objAfastamento.SemCalculo);
                    pAbonoD = objAfastamento.Horai;
                    pAbonoN = objAfastamento.Horaf;

                    ret = true;
                    break;
                }
            }
            dr.Close();

            if (ret)
            {
                if (pSemCalc)
                {
                    pAbono = 0;
                    pAbonoD = "--:--";
                    pAbonoN = "--:--";
                }
            }

            return ret;
        }

        public bool PossuiRegistro(DateTime pData, int pIdEmpresa, int pIdDepartamento, int pIdFuncionario)
        {
            FbDataReader dr;

            //Verifica e possui registro por Funcionario
            dr = this.getPeriodoFuncionario(pIdFuncionario);
            if (VerificaPeriodo(pData, dr))
            {
                return true;
            }

            //Verifica e possui registro por Departamento
            dr = this.getPeriodoDepartemento(pIdEmpresa, pIdDepartamento);
            if (VerificaPeriodo(pData, dr))
            {
                return true;
            }

            //Verifica e possui registro por Empresa
            dr = this.getPeriodoEmpresa(pIdEmpresa);
            if (VerificaPeriodo(pData, dr))
            {
                return true;
            }

            return false;
        }

        public int VerificaExiste(int pId, DateTime? pDataInicial, DateTime? pDataFinal, int pTipo, int pIdentificacao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@datainicial", FbDbType.Date),
                new FbParameter("@datafinal", FbDbType.Date),
                new FbParameter("@tipo", FbDbType.Integer),
                new FbParameter("@identificacao", FbDbType.Integer),
                new FbParameter("@id", FbDbType.Integer)
            };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;
            parms[2].Value = pTipo;
            parms[3].Value = pIdentificacao;
            parms[4].Value = pId;

            int qt = (int)FB.DataBase.ExecuteScalar(CommandType.Text, VERIFICA, parms);

            return qt;
        }

        public Modelo.Afastamento LoadParaManutencao(DateTime pData, int pIdFuncionario)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@data", FbDbType.Date),
                new FbParameter("@idfuncionario", FbDbType.Integer)
            };

            parms[0].Value = pData;
            parms[1].Value = pIdFuncionario;

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, SELECTMAR, parms);
            Modelo.Afastamento objAfastamento = new Modelo.Afastamento();
            try
            {
                SetInstance(dr, objAfastamento);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objAfastamento;
        }

        public void Incluir(List<Modelo.Afastamento> afastamentos)
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
                        foreach (Modelo.Afastamento afast in afastamentos)
                        {
                            SetDadosInc(afast);
                            SetParameters(parms, afast);
                            cmd = DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
                            afast.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

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

        #endregion

        #region IAfastamento Members


        public List<Modelo.Afastamento> GetParaRelatorioAbsenteismo(DateTime DataInicial, DateTime DataFinal)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAfastamento Members


        public List<Modelo.Afastamento> GetParaExportacaoFolha(DateTime dataI, DateTime dataF, string idsOcorrencias, bool considerarAbsenteismo, List<int> IdsFuncs)
        {
            throw new NotImplementedException();
        }

        #endregion


        public List<Modelo.Afastamento> GetAllList()
        {
            throw new NotImplementedException();
        }

        public DataTable GetParaRelatorioAbono(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento, string pIdsOcorrenciasSelecionados)
        {
            throw new NotImplementedException();
        }

        public int? GetIdPorIdIntegracao(string idIntegracao)
        {
            throw new NotImplementedException();
        }

        public int? GetIdAfastamentoPorIdMarcacao(int IdMarcacao)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoAfastamento(int idAfastamento)
        {
            throw new NotImplementedException();
        }


        public IList<Modelo.Proxy.pxyAbonosPorMarcacao> GetAbonosPorMarcacoes(IList<int> idFuncionarios, DateTime dataIni, DateTime dataFin)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(int idFuncionario, DateTime pDataInicial, DateTime pDataFinal, bool apenasFerias)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(List<int> idFuncionario, DateTime pDataInicial, DateTime pDataFinal, bool apenasFerias)
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

        public List<PxyRelAfastamento> GetRelatorioAfastamentoFolha(List<int> idsFuncs, DateTime pDataI, DateTime pDataF, short absenteismo, bool considerarAbonado, bool considerarParcial, bool considerarSemCalculo, bool considerarSuspensao, bool considerarSemAbono)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }
    }
}
