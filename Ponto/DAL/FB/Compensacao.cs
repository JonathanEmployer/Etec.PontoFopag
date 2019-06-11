using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Compensacao : DAL.FB.DALBase, DAL.ICompensacao
    {

        private Compensacao()
        {
            GEN = "GEN_compensacao_id";

            TABELA = "compensacao";

            SELECTPID = "SELECT * FROM \"compensacao\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT \"compensacao\".\"id\"" +
                                ", case when \"tipo\" = 0 then (SELECT \"empresa\".\"nome\" FROM \"empresa\" WHERE \"empresa\".\"id\" = \"compensacao\".\"identificacao\") " +
                                          " when \"tipo\" = 1 then (SELECT \"departamento\".\"descricao\" FROM \"departamento\" WHERE \"departamento\".\"id\" = \"compensacao\".\"identificacao\") " +
                                          " when \"tipo\" = 2 then (SELECT \"funcionario\".\"nome\" FROM \"funcionario\" WHERE \"funcionario\".\"id\" = \"compensacao\".\"identificacao\") " +
                                          " when \"tipo\" = 3 then (SELECT \"funcao\".\"descricao\" FROM \"funcao\" WHERE \"funcao\".\"id\" = \"compensacao\".\"identificacao\") end AS \"nome\" " +
                                ", \"compensacao\".\"codigo\"" +
                                ", \"compensacao\".\"periodoinicial\"" +
                                ", \"compensacao\".\"periodofinal\"" +
                                ", case when \"tipo\" = 0 then 'Empresa' when \"tipo\" = 1 then 'Departamento' when \"tipo\" = 2 then 'Funcionário' when \"tipo\" = 3 then 'Função' end AS \"tipo\"" +
                             " FROM \"compensacao\" ";

            INSERT = "  INSERT INTO \"compensacao\"" +
                                        "(\"codigo\", \"tipo\", \"identificacao\", \"periodoinicial\", \"periodofinal\", \"dias_1\", \"dias_2\", \"dias_3\", \"dias_4\", \"dias_5\", \"dias_6\", \"dias_7\", \"dias_8\", \"dias_9\", \"dias_10\", \"totalhorassercompensadas_1\", \"totalhorassercompensadas_2\", \"totalhorassercompensadas_3\", \"totalhorassercompensadas_4\", \"totalhorassercompensadas_5\", \"totalhorassercompensadas_6\", \"totalhorassercompensadas_7\", \"totalhorassercompensadas_8\", \"totalhorassercompensadas_9\", \"totalhorassercompensadas_10\", \"diacompensarinicial\", \"diacompensarfinal\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @tipo, @identificacao, @periodoinicial, @periodofinal, @dias_1, @dias_2, @dias_3, @dias_4, @dias_5, @dias_6, @dias_7, @dias_8, @dias_9, @dias_10, @totalhorassercompensadas_1, @totalhorassercompensadas_2, @totalhorassercompensadas_3, @totalhorassercompensadas_4, @totalhorassercompensadas_5, @totalhorassercompensadas_6, @totalhorassercompensadas_7, @totalhorassercompensadas_8, @totalhorassercompensadas_9, @totalhorassercompensadas_10, @diacompensarinicial, @diacompensarfinal, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"compensacao\" SET \"codigo\" = @codigo " +
                                        ", \"tipo\" = @tipo " +
                                        ", \"identificacao\" = @identificacao " +
                                        ", \"periodoinicial\" = @periodoinicial " +
                                        ", \"periodofinal\" = @periodofinal " +
                                        ", \"dias_1\" = @dias_1 " +
                                        ", \"dias_2\" = @dias_2 " +
                                        ", \"dias_3\" = @dias_3 " +
                                        ", \"dias_4\" = @dias_4 " +
                                        ", \"dias_5\" = @dias_5 " +
                                        ", \"dias_6\" = @dias_6 " +
                                        ", \"dias_7\" = @dias_7 " +
                                        ", \"dias_8\" = @dias_8 " +
                                        ", \"dias_9\" = @dias_9 " +
                                        ", \"dias_10\" = @dias_10 " +
                                        ", \"totalhorassercompensadas_1\" = @totalhorassercompensadas_1 " +
                                        ", \"totalhorassercompensadas_2\" = @totalhorassercompensadas_2 " +
                                        ", \"totalhorassercompensadas_3\" = @totalhorassercompensadas_3 " +
                                        ", \"totalhorassercompensadas_4\" = @totalhorassercompensadas_4 " +
                                        ", \"totalhorassercompensadas_5\" = @totalhorassercompensadas_5 " +
                                        ", \"totalhorassercompensadas_6\" = @totalhorassercompensadas_6 " +
                                        ", \"totalhorassercompensadas_7\" = @totalhorassercompensadas_7 " +
                                        ", \"totalhorassercompensadas_8\" = @totalhorassercompensadas_8 " +
                                        ", \"totalhorassercompensadas_9\" = @totalhorassercompensadas_9 " +
                                        ", \"totalhorassercompensadas_10\" = @totalhorassercompensadas_10 " +
                                        ", \"diacompensarinicial\" = @diacompensarinicial " +
                                        ", \"diacompensarfinal\" = @diacompensarfinal " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"compensacao\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"compensacao\"";

        }

        #region Singleton

        private static volatile FB.Compensacao _instancia = null;

        public static FB.Compensacao GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Compensacao))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Compensacao();
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
                obj = new Modelo.Compensacao();
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
            ((Modelo.Compensacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Compensacao)obj).Tipo = Convert.ToInt16(dr["tipo"]);
            ((Modelo.Compensacao)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.Compensacao)obj).Periodoinicial = (dr["periodoinicial"] is DBNull ? null : (DateTime?)dr["periodoinicial"]);
            ((Modelo.Compensacao)obj).Periodofinal = (dr["periodofinal"] is DBNull ? null : (DateTime?)dr["periodofinal"]);
            ((Modelo.Compensacao)obj).Dias_1 = Convert.ToInt16(dr["dias_1"]);
            ((Modelo.Compensacao)obj).Dias_2 = Convert.ToInt16(dr["dias_2"]);
            ((Modelo.Compensacao)obj).Dias_3 = Convert.ToInt16(dr["dias_3"]);
            ((Modelo.Compensacao)obj).Dias_4 = Convert.ToInt16(dr["dias_4"]);
            ((Modelo.Compensacao)obj).Dias_5 = Convert.ToInt16(dr["dias_5"]);
            ((Modelo.Compensacao)obj).Dias_6 = Convert.ToInt16(dr["dias_6"]);
            ((Modelo.Compensacao)obj).Dias_7 = Convert.ToInt16(dr["dias_7"]);
            ((Modelo.Compensacao)obj).Dias_8 = Convert.ToInt16(dr["dias_8"]);
            ((Modelo.Compensacao)obj).Dias_9 = Convert.ToInt16(dr["dias_9"]);
            ((Modelo.Compensacao)obj).Dias_10 = Convert.ToInt16(dr["dias_10"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_1 = Convert.ToString(dr["totalhorassercompensadas_1"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_2 = Convert.ToString(dr["totalhorassercompensadas_2"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_3 = Convert.ToString(dr["totalhorassercompensadas_3"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_4 = Convert.ToString(dr["totalhorassercompensadas_4"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_5 = Convert.ToString(dr["totalhorassercompensadas_5"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_6 = Convert.ToString(dr["totalhorassercompensadas_6"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_7 = Convert.ToString(dr["totalhorassercompensadas_7"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_8 = Convert.ToString(dr["totalhorassercompensadas_8"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_9 = Convert.ToString(dr["totalhorassercompensadas_9"]);
            ((Modelo.Compensacao)obj).Totalhorassercompensadas_10 = Convert.ToString(dr["totalhorassercompensadas_10"]);
            ((Modelo.Compensacao)obj).Diacompensarinicial = (dr["diacompensarinicial"] is DBNull ? null : (DateTime?)dr["diacompensarinicial"]);
            ((Modelo.Compensacao)obj).Diacompensarfinal = (dr["diacompensarfinal"] is DBNull ? null : (DateTime?)dr["diacompensarfinal"]);

            ((Modelo.Compensacao)obj).Tipo_Ant = ((Modelo.Compensacao)obj).Tipo;
            ((Modelo.Compensacao)obj).Identificacao_Ant = ((Modelo.Compensacao)obj).Identificacao;
            ((Modelo.Compensacao)obj).Periodoinicial_Ant = ((Modelo.Compensacao)obj).Periodoinicial;
            ((Modelo.Compensacao)obj).Periodofinal_Ant = ((Modelo.Compensacao)obj).Periodofinal;
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@tipo", FbDbType.SmallInt),
				new FbParameter ("@identificacao", FbDbType.Integer),
				new FbParameter ("@periodoinicial", FbDbType.Date),
				new FbParameter ("@periodofinal", FbDbType.Date),
				new FbParameter ("@dias_1", FbDbType.SmallInt),
				new FbParameter ("@dias_2", FbDbType.SmallInt),
				new FbParameter ("@dias_3", FbDbType.SmallInt),
				new FbParameter ("@dias_4", FbDbType.SmallInt),
				new FbParameter ("@dias_5", FbDbType.SmallInt),
				new FbParameter ("@dias_6", FbDbType.SmallInt),
				new FbParameter ("@dias_7", FbDbType.SmallInt),
				new FbParameter ("@dias_8", FbDbType.SmallInt),
				new FbParameter ("@dias_9", FbDbType.SmallInt),
				new FbParameter ("@dias_10", FbDbType.SmallInt),
				new FbParameter ("@totalhorassercompensadas_1", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_2", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_3", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_4", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_5", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_6", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_7", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_8", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_9", FbDbType.VarChar),
				new FbParameter ("@totalhorassercompensadas_10", FbDbType.VarChar),
				new FbParameter ("@diacompensarinicial", FbDbType.Date),
				new FbParameter ("@diacompensarfinal", FbDbType.Date),
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
            parms[1].Value = ((Modelo.Compensacao)obj).Codigo;
            parms[2].Value = ((Modelo.Compensacao)obj).Tipo;
            parms[3].Value = ((Modelo.Compensacao)obj).Identificacao;
            parms[4].Value = ((Modelo.Compensacao)obj).Periodoinicial;
            parms[5].Value = ((Modelo.Compensacao)obj).Periodofinal;
            parms[6].Value = ((Modelo.Compensacao)obj).Dias_1;
            parms[7].Value = ((Modelo.Compensacao)obj).Dias_2;
            parms[8].Value = ((Modelo.Compensacao)obj).Dias_3;
            parms[9].Value = ((Modelo.Compensacao)obj).Dias_4;
            parms[10].Value = ((Modelo.Compensacao)obj).Dias_5;
            parms[11].Value = ((Modelo.Compensacao)obj).Dias_6;
            parms[12].Value = ((Modelo.Compensacao)obj).Dias_7;
            parms[13].Value = ((Modelo.Compensacao)obj).Dias_8;
            parms[14].Value = ((Modelo.Compensacao)obj).Dias_9;
            parms[15].Value = ((Modelo.Compensacao)obj).Dias_10;
            parms[16].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_1;
            parms[17].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_2;
            parms[18].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_3;
            parms[19].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_4;
            parms[20].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_5;
            parms[21].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_6;
            parms[22].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_7;
            parms[23].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_8;
            parms[24].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_9;
            parms[25].Value = ((Modelo.Compensacao)obj).Totalhorassercompensadas_10;
            parms[26].Value = ((Modelo.Compensacao)obj).Diacompensarinicial;
            parms[27].Value = ((Modelo.Compensacao)obj).Diacompensarfinal;
            parms[28].Value = ((Modelo.Compensacao)obj).Incdata;
            parms[29].Value = ((Modelo.Compensacao)obj).Inchora;
            parms[30].Value = ((Modelo.Compensacao)obj).Incusuario;
            parms[31].Value = ((Modelo.Compensacao)obj).Altdata;
            parms[32].Value = ((Modelo.Compensacao)obj).Althora;
            parms[33].Value = ((Modelo.Compensacao)obj).Altusuario;
        }

        public Modelo.Compensacao LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
            try
            {
                SetInstance(dr, objCompensacao);

                DAL.FB.DiasCompensacao dalDiasCompensacao = DAL.FB.DiasCompensacao.GetInstancia;
                objCompensacao.DiasC = dalDiasCompensacao.LoadPCompensacao(objCompensacao.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCompensacao;
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0) > 0)
            {
                parms[1].Value = MaxCodigo(trans);
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            SalvarDiasCO(trans, (Modelo.Compensacao)obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (DALBase.CountCampo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizando em outro registro. Verifique.");
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            SalvarDiasCO(trans, (Modelo.Compensacao)obj);

            cmd.Parameters.Clear();
        }

        private void SalvarDiasCO(FbTransaction trans, Modelo.Compensacao obj)
        {
            DAL.FB.DiasCompensacao dalDiasCO = DAL.FB.DiasCompensacao.GetInstancia;
            foreach (Modelo.DiasCompensacao dja in obj.DiasC)
            {
                dja.Idcompensacao = obj.Id;
                switch (dja.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalDiasCO.Incluir(trans, dja);
                        break;
                    case Modelo.Acao.Alterar:
                        dalDiasCO.Alterar(trans, dja);
                        break;
                    case Modelo.Acao.Excluir:
                        dalDiasCO.Excluir(trans, dja);
                        break;
                    default:
                        break;
                }
            }
        }

        public List<Modelo.Compensacao> getListaCompensacao(DateTime pData)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                    new FbParameter("@data", FbDbType.Date)
            };
            parms[0].Value = pData;

            string aux = "SELECT * FROM \"compensacao\" WHERE @data >= \"periodoinicial\" AND @data <= \"periodofinal\" ORDER BY \"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Compensacao> lista = new List<Modelo.Compensacao>();
            while (dr.Read())
            {
                Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
                AuxSetInstance(dr, objCompensacao);
                lista.Add(objCompensacao);
            }

            return lista;
        }

        public List<Modelo.Compensacao> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                    new FbParameter("@datai", FbDbType.Date),
                    new FbParameter("@dataf", FbDbType.Date)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string aux = "  SELECT * FROM \"compensacao\" " +
                            "WHERE (@datai <= \"periodoinicial\" AND @dataf >= \"periodofinal\") " +
                            "OR (@datai >= \"periodoinicial\" AND @dataf <= \"periodofinal\") " +
                            "OR (@dataf >= \"periodoinicial\" AND @dataf <= \"periodofinal\") " +
                            "ORDER BY \"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Compensacao> lista = new List<Modelo.Compensacao>();
            while (dr.Read())
            {
                Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
                AuxSetInstance(dr, objCompensacao);
                lista.Add(objCompensacao);
            }

            return lista;
        }

        public DataTable GetTotalCompensado(int pIdCompensacao)
        {
            Modelo.Compensacao objCompensacao = new Modelo.Compensacao();
            objCompensacao = LoadObject(pIdCompensacao);

            FbParameter[] parms = new FbParameter[]
            { 
                    new FbParameter("@idcompensacao", FbDbType.Integer),
                    new FbParameter("@identificacao", FbDbType.Integer)
            };
            parms[0].Value = pIdCompensacao;
            parms[1].Value = objCompensacao.Identificacao;

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT  \"parcial\".\"idfuncionario\", \"parcial\".\"nomefunc\" as \"nomefunc\",");
            sql.AppendLine("        SUM(\"parcial\".\"horascompensadas\") as \"horascompensadas\"");
            sql.AppendLine("FROM (");
            sql.AppendLine("        SELECT \"marcacao\".\"idfuncionario\", \"funcionario\".\"nome\" as \"nomefunc\",");
            sql.AppendLine("                (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horascompensadas\", '--:--'))) AS \"horascompensadas\"");
            sql.AppendLine("        FROM \"marcacao\", \"compensacao\", \"funcionario\"");
            sql.AppendLine("        WHERE \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\"");
            sql.AppendLine("        AND COALESCE(\"funcionario\".\"excluido\", 0) = 0");
            sql.AppendLine("        AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1");
            sql.AppendLine("        AND \"marcacao\".\"data\" >= \"compensacao\".\"periodoinicial\"");
            sql.AppendLine("        AND \"marcacao\".\"data\" <= \"compensacao\".\"periodofinal\"");
            sql.AppendLine("        AND \"compensacao\".\"id\" = @idcompensacao");

            switch (objCompensacao.Tipo)
            {
                case 0://Empresa
                    sql.AppendLine("        AND \"funcionario\".\"idempresa\" = @identificacao");
                    break;
                case 1://Departamento
                    sql.AppendLine("        AND \"funcionario\".\"iddepartamento\" = @identificacao");
                    break;
                case 2://Funcionário
                    sql.AppendLine("        AND \"marcacao\".\"idfuncionario\" = @identificacao");
                    break;
                case 3://Função
                    sql.AppendLine("        AND \"funcionario\".\"idfuncao\" = @identificacao");
                    break;
                default:
                    break;
            }

            sql.AppendLine(" ) AS \"parcial\" ");
            sql.AppendLine("GROUP BY \"parcial\".\"idfuncionario\", \"parcial\".\"nomefunc\"");
            sql.AppendLine("ORDER BY \"parcial\".\"nomefunc\"");

            //string comando = "SELECT \"parcial\".\"idfuncionario\", \"parcial\".\"nomefunc\" as \"nomefunc\","
            //                + " SUM(\"parcial\".\"horascompensadas\") as \"horascompensadas\""
            //                + " FROM"
            //                + " (SELECT \"marcacao\".\"idfuncionario\", \"funcionario\".\"nome\" as \"nomefunc\","
            //                + " (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horascompensadas\", '--:--'))) AS \"horascompensadas\""
            //                + " FROM \"marcacao\", \"compensacao\", \"funcionario\""


            //                + " WHERE \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\""
            //                + " AND COALESCE(\"funcionario\".\"excluido\", 0) = 0"
            //                + " AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1"
            //                + " AND \"marcacao\".\"data\" >= \"compensacao\".\"periodoinicial\""
            //                + " AND \"marcacao\".\"data\" <= \"compensacao\".\"periodofinal\""
            //                + " AND \"compensacao\".\"id\" = @idcompensacao"
            //                + " ) AS \"parcial\" GROUP BY \"parcial\".\"idfuncionario\", \"parcial\".\"nomefunc\""
            //                + " ORDER BY \"parcial\".\"nomefunc\"";

            DataTable dt = new DataTable();
            dt.Load(DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql.ToString(), parms));

            return dt;

        }

        public List<Modelo.Compensacao> GetAllList()
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

        public List<Modelo.Compensacao> GetPeriodoByFuncionario(DateTime pDataInicial, DateTime pDataFinal, List<int> pdIdsFuncs)
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
