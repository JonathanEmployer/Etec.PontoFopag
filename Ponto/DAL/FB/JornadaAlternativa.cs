using System;
using System.Collections;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using Modelo;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class JornadaAlternativa : DAL.FB.DALBase, IJornadaAlternativa
    {
        public string VERIFICA { get; set; }
        public string SELECTPE { get; set; }

        private JornadaAlternativa()
        {
            GEN = "GEN_jornadaalternativa_id";

            TABELA = "jornadaalternativa";

            SELECTPID = "SELECT * FROM \"jornadaalternativa\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT   \"jornadaalternativa\".\"id\"" +
                                    ", \"jornadaalternativa\".\"codigo\"" +
                                    ", \"jornadaalternativa\".\"datainicial\"" +
                                    ", \"jornadaalternativa\".\"datafinal\"" +
                                    ", case when \"jornadaalternativa\".\"tipo\" = 0 then 'Empresa' when \"jornadaalternativa\".\"tipo\" = 1 then 'Departamento' when \"jornadaalternativa\".\"tipo\" = 2 then 'Funcionário' when \"jornadaalternativa\".\"tipo\" = 3 then 'Função' end AS \"tipo\" " +
                                    ", case when \"tipo\" = 0 then (SELECT \"empresa\".\"nome\" FROM \"empresa\" WHERE \"empresa\".\"id\" = \"jornadaalternativa\".\"identificacao\") " +
                                    "      when \"tipo\" = 1 then (SELECT \"departamento\".\"descricao\" FROM \"departamento\" WHERE \"departamento\".\"id\" = \"jornadaalternativa\".\"identificacao\") " +
                                    "      when \"tipo\" = 2 then (SELECT \"funcionario\".\"nome\" FROM \"funcionario\" WHERE \"funcionario\".\"id\" = \"jornadaalternativa\".\"identificacao\") " +
                                    "      when \"tipo\" = 3 then (SELECT \"funcao\".\"descricao\" FROM \"funcao\" WHERE \"funcao\".\"id\" = \"jornadaalternativa\".\"identificacao\") end AS \"nome\" " +
                                    ", \"jornadaalternativa\".\"entrada_1\"" +
                                    ", \"jornadaalternativa\".\"saida_1\"" +
                                    ", \"jornadaalternativa\".\"entrada_2\"" +
                                    ",\"jornadaalternativa\".\"saida_2\"" +
                             "FROM \"jornadaalternativa\" ";

            VERIFICA = "   SELECT COALESCE(COUNT(\"id\"), 0) AS \"qt\" " +
                            "FROM \"jornadaalternativa\" " +
                            "WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\") " +
                            "OR (@datafinal >= \"datainicial\" AND @datafinal <= \"datafinal\") " +
                            "OR (@datainicial <= \"datainicial\" AND @datafinal >= \"datafinal\")) " +
                            "AND \"tipo\" = @tipo " +
                            "AND \"identificacao\" = @identificacao " +
                            "AND \"id\" <> @id";

            SELECTPE = "SELECT * FROM \"jornadaalternativa\" " +
                       "WHERE (@data >= \"datainicial\" AND @data <= \"datafinal\") " +                       
                       "AND \"tipo\" = @tipo " +
                       "AND \"identificacao\" = @identificacao ";

            INSERT = "  INSERT INTO \"jornadaalternativa\"" +
                                        "(\"codigo\", \"tipo\", \"identificacao\", \"datainicial\", \"datafinal\", \"horasnormais\", \"cargamista\", \"somentecargahoraria\", \"ordenabilhetesaida\", \"habilitatolerancia\", \"limitemin\", \"limitemax\", \"entrada_1\", \"entrada_2\", \"entrada_3\", \"entrada_4\", \"saida_1\", \"saida_2\", \"saida_3\", \"saida_4\", \"entrada2_1\", \"entrada2_2\", \"entrada2_3\", \"entrada2_4\", \"saida2_1\", \"saida2_2\", \"saida2_3\", \"saida2_4\", \"totaltrabalhadadiurna\", \"totaltrabalhadanoturna\", \"totalmista\", \"incdata\", \"inchora\", \"incusuario\", \"intervaloautomatico\", \"preassinaladas1\", \"preassinaladas2\", \"preassinaladas3\", \"calculoadnoturno\", \"conversaohoranoturna\", \"idjornada\") " +
                                        " VALUES " +
                                        "(@codigo, @tipo, @identificacao, @datainicial, @datafinal, @horasnormais, @cargamista, @somentecargahoraria, @ordenabilhetesaida, @habilitatolerancia, @limitemin, @limitemax, @entrada_1, @entrada_2, @entrada_3, @entrada_4, @saida_1, @saida_2, @saida_3, @saida_4, @entrada2_1, @entrada2_2, @entrada2_3, @entrada2_4, @saida2_1, @saida2_2, @saida2_3, @saida2_4, @totaltrabalhadadiurna, @totaltrabalhadanoturna, @totalmista, @incdata, @inchora, @incusuario, @intervaloautomatico, @preassinaladas1, @preassinaladas2, @preassinaladas3, @calculoadnoturno, @conversaohoranoturna, @idjornada)";

            UPDATE = "  UPDATE \"jornadaalternativa\" SET \"codigo\" = @codigo " +
                                        ", \"tipo\" = @tipo " +
                                        ", \"identificacao\" = @identificacao " +
                                        ", \"datainicial\" = @datainicial " +
                                        ", \"datafinal\" = @datafinal " +
                                        ", \"horasnormais\" = @horasnormais " +
                                        ", \"cargamista\" = @cargamista " +
                                        ", \"somentecargahoraria\" = @somentecargahoraria " +
                                        ", \"ordenabilhetesaida\" = @ordenabilhetesaida " +
                                        ", \"habilitatolerancia\" = @habilitatolerancia " +
                                        ", \"limitemin\" = @limitemin " +
                                        ", \"limitemax\" = @limitemax " +
                                        ", \"entrada_1\" = @entrada_1 " +
                                        ", \"entrada_2\" = @entrada_2 " +
                                        ", \"entrada_3\" = @entrada_3 " +
                                        ", \"entrada_4\" = @entrada_4 " +
                                        ", \"saida_1\" = @saida_1 " +
                                        ", \"saida_2\" = @saida_2 " +
                                        ", \"saida_3\" = @saida_3 " +
                                        ", \"saida_4\" = @saida_4 " +
                                        ", \"entrada2_1\" = @entrada2_1 " +
                                        ", \"entrada2_2\" = @entrada2_2 " +
                                        ", \"entrada2_3\" = @entrada2_3 " +
                                        ", \"entrada2_4\" = @entrada2_4 " +
                                        ", \"saida2_1\" = @saida2_1 " +
                                        ", \"saida2_2\" = @saida2_2 " +
                                        ", \"saida2_3\" = @saida2_3 " +
                                        ", \"saida2_4\" = @saida2_4 " +
                                        ", \"totaltrabalhadadiurna\" = @totaltrabalhadadiurna " +
                                        ", \"totaltrabalhadanoturna\" = @totaltrabalhadanoturna " +
                                        ", \"totalmista\" = @totalmista " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"intervaloautomatico\" = @intervaloautomatico " +
                                        ", \"preassinaladas1\" = @preassinaladas1 " +
                                        ", \"preassinaladas2\" = @preassinaladas2 " +
                                        ", \"preassinaladas3\" = @preassinaladas3 " +
                                        ", \"calculoadnoturno\" = @calculoadnoturno " +
                                        ", \"conversaohoranoturna\" = @conversaohoranoturna " +
                                        ", \"idjornada\" = @idjornada " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"jornadaalternativa\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"jornadaalternativa\"";

        }

        #region Singleton

        private static volatile FB.JornadaAlternativa _instancia = null;

        public static FB.JornadaAlternativa GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.JornadaAlternativa))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.JornadaAlternativa();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        public List<Modelo.JornadaAlternativa> GetAllList(bool loadDiasJA)
        {
            FbParameter[] parms = new FbParameter[]
            { 
            };

            string aux = "SELECT \"jornadaalternativa\".* FROM \"jornadaalternativa\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.JornadaAlternativa> ret = new List<Modelo.JornadaAlternativa>();

            if (dr.HasRows)
            {
                Modelo.JornadaAlternativa ja;
                if (loadDiasJA)
                {
                    DAL.FB.DiasJornadaAlternativa dalDiasJornadaAlternativa = DAL.FB.DiasJornadaAlternativa.GetInstancia;
                    while (dr.Read())
                    {
                        ja = new Modelo.JornadaAlternativa();
                        AuxSetInstance(dr, ja);

                        ja.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(ja.Id);
                        ret.Add(ja);
                    }
                }
                else
                {
                    while (dr.Read())
                    {
                        ja = new Modelo.JornadaAlternativa();
                        ja.DiasJA = new List<Modelo.DiasJornadaAlternativa>();
                        AuxSetInstance(dr, ja);
                        ret.Add(ja);
                    }
                }
            }

            return ret;
        }

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
                obj = new Modelo.JornadaAlternativa();
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
            ((Modelo.JornadaAlternativa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.JornadaAlternativa)obj).Tipo = Convert.ToInt16(dr["tipo"]);
            ((Modelo.JornadaAlternativa)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.JornadaAlternativa)obj).DataInicial = Convert.ToDateTime(dr["datainicial"]);
            ((Modelo.JornadaAlternativa)obj).DataFinal = Convert.ToDateTime(dr["datafinal"]);
            ((Modelo.JornadaAlternativa)obj).HorasNormais = Convert.ToInt16(dr["horasnormais"]);
            ((Modelo.JornadaAlternativa)obj).CargaMista = (dr["cargamista"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(dr["cargamista"]));
            ((Modelo.JornadaAlternativa)obj).SomenteCargaHoraria = Convert.ToInt16(dr["somentecargahoraria"]);
            ((Modelo.JornadaAlternativa)obj).OrdenaBilheteSaida = Convert.ToInt16(dr["ordenabilhetesaida"]);
            ((Modelo.JornadaAlternativa)obj).HabilitaTolerancia = Convert.ToInt16(dr["habilitatolerancia"]);
            ((Modelo.JornadaAlternativa)obj).LimiteMin = Convert.ToString(dr["limitemin"]);
            ((Modelo.JornadaAlternativa)obj).LimiteMax = Convert.ToString(dr["limitemax"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.JornadaAlternativa)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.JornadaAlternativa)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.JornadaAlternativa)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.JornadaAlternativa)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.JornadaAlternativa)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_1 = Convert.ToString(dr["entrada2_1"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_2 = Convert.ToString(dr["entrada2_2"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_3 = Convert.ToString(dr["entrada2_3"]);
            ((Modelo.JornadaAlternativa)obj).Entrada2_4 = Convert.ToString(dr["entrada2_4"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_1 = Convert.ToString(dr["saida2_1"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_2 = Convert.ToString(dr["saida2_2"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_3 = Convert.ToString(dr["saida2_3"]);
            ((Modelo.JornadaAlternativa)obj).Saida2_4 = Convert.ToString(dr["saida2_4"]);
            ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaDiurna = Convert.ToString(dr["totaltrabalhadadiurna"]);
            ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaNoturna = Convert.ToString(dr["totaltrabalhadanoturna"]);
            ((Modelo.JornadaAlternativa)obj).TotalMista = Convert.ToString(dr["totalmista"]);
            ((Modelo.JornadaAlternativa)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.JornadaAlternativa)obj).Preassinaladas1 = (dr["preassinaladas1"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas1"]));
            ((Modelo.JornadaAlternativa)obj).Preassinaladas2 = (dr["preassinaladas2"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas2"]));
            ((Modelo.JornadaAlternativa)obj).Preassinaladas3 = (dr["preassinaladas3"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas3"]));
            ((Modelo.JornadaAlternativa)obj).CalculoAdicionalNoturno = (dr["calculoadnoturno"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["calculoadnoturno"]));
            ((Modelo.JornadaAlternativa)obj).ConversaoHoraNoturna = (dr["conversaohoranoturna"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["conversaohoranoturna"]));
            ((Modelo.JornadaAlternativa)obj).Idjornada = dr["idjornada"] is DBNull ? 0 : Convert.ToInt32(dr["idjornada"]);
            ((Modelo.JornadaAlternativa)obj).ConverteHoraStringToInt();
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@tipo", FbDbType.SmallInt),
				new FbParameter ("@identificacao", FbDbType.Integer),
				new FbParameter ("@datainicial", FbDbType.Date),
				new FbParameter ("@datafinal", FbDbType.Date),
				new FbParameter ("@horasnormais", FbDbType.SmallInt),
                new FbParameter ("@cargamista", FbDbType.SmallInt),
				new FbParameter ("@somentecargahoraria", FbDbType.SmallInt),
				new FbParameter ("@ordenabilhetesaida", FbDbType.SmallInt),
				new FbParameter ("@habilitatolerancia", FbDbType.SmallInt),
				new FbParameter ("@limitemin", FbDbType.VarChar),
				new FbParameter ("@limitemax", FbDbType.VarChar),
				new FbParameter ("@entrada_1", FbDbType.VarChar),
				new FbParameter ("@entrada_2", FbDbType.VarChar),
				new FbParameter ("@entrada_3", FbDbType.VarChar),
				new FbParameter ("@entrada_4", FbDbType.VarChar),
				new FbParameter ("@saida_1", FbDbType.VarChar),
				new FbParameter ("@saida_2", FbDbType.VarChar),
				new FbParameter ("@saida_3", FbDbType.VarChar),
				new FbParameter ("@saida_4", FbDbType.VarChar),
				new FbParameter ("@entrada2_1", FbDbType.VarChar),
				new FbParameter ("@entrada2_2", FbDbType.VarChar),
				new FbParameter ("@entrada2_3", FbDbType.VarChar),
				new FbParameter ("@entrada2_4", FbDbType.VarChar),
				new FbParameter ("@saida2_1", FbDbType.VarChar),
				new FbParameter ("@saida2_2", FbDbType.VarChar),
				new FbParameter ("@saida2_3", FbDbType.VarChar),
				new FbParameter ("@saida2_4", FbDbType.VarChar),
				new FbParameter ("@totaltrabalhadadiurna", FbDbType.VarChar),
				new FbParameter ("@totaltrabalhadanoturna", FbDbType.VarChar),
                new FbParameter ("@totalmista", FbDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@intervaloautomatico", FbDbType.Integer),
                new FbParameter ("@preassinaladas1", FbDbType.Integer),
                new FbParameter ("@preassinaladas2", FbDbType.Integer),
                new FbParameter ("@preassinaladas3", FbDbType.Integer),
                new FbParameter ("@calculoadnoturno", FbDbType.SmallInt),
                new FbParameter ("@conversaohoranoturna", FbDbType.SmallInt),
                new FbParameter ("@idjornada", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.JornadaAlternativa)obj).Codigo;
            parms[2].Value = ((Modelo.JornadaAlternativa)obj).Tipo;
            parms[3].Value = ((Modelo.JornadaAlternativa)obj).Identificacao;
            parms[4].Value = ((Modelo.JornadaAlternativa)obj).DataInicial;
            parms[5].Value = ((Modelo.JornadaAlternativa)obj).DataFinal;
            parms[6].Value = ((Modelo.JornadaAlternativa)obj).HorasNormais;
            parms[7].Value = ((Modelo.JornadaAlternativa)obj).CargaMista;
            parms[8].Value = ((Modelo.JornadaAlternativa)obj).SomenteCargaHoraria;
            parms[9].Value = ((Modelo.JornadaAlternativa)obj).OrdenaBilheteSaida;
            parms[10].Value = ((Modelo.JornadaAlternativa)obj).HabilitaTolerancia;
            parms[11].Value = ((Modelo.JornadaAlternativa)obj).LimiteMin;
            parms[12].Value = ((Modelo.JornadaAlternativa)obj).LimiteMax;
            parms[13].Value = ((Modelo.JornadaAlternativa)obj).Entrada_1;
            parms[14].Value = ((Modelo.JornadaAlternativa)obj).Entrada_2;
            parms[15].Value = ((Modelo.JornadaAlternativa)obj).Entrada_3;
            parms[16].Value = ((Modelo.JornadaAlternativa)obj).Entrada_4;
            parms[17].Value = ((Modelo.JornadaAlternativa)obj).Saida_1;
            parms[18].Value = ((Modelo.JornadaAlternativa)obj).Saida_2;
            parms[19].Value = ((Modelo.JornadaAlternativa)obj).Saida_3;
            parms[20].Value = ((Modelo.JornadaAlternativa)obj).Saida_4;
            parms[21].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_1;
            parms[22].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_2;
            parms[23].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_3;
            parms[24].Value = ((Modelo.JornadaAlternativa)obj).Entrada2_4;
            parms[25].Value = ((Modelo.JornadaAlternativa)obj).Saida2_1;
            parms[26].Value = ((Modelo.JornadaAlternativa)obj).Saida2_2;
            parms[27].Value = ((Modelo.JornadaAlternativa)obj).Saida2_3;
            parms[28].Value = ((Modelo.JornadaAlternativa)obj).Saida2_4;
            parms[29].Value = ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaDiurna;
            parms[30].Value = ((Modelo.JornadaAlternativa)obj).TotalTrabalhadaNoturna;
            parms[31].Value = ((Modelo.JornadaAlternativa)obj).TotalMista;
            parms[32].Value = ((Modelo.JornadaAlternativa)obj).Incdata;
            parms[33].Value = ((Modelo.JornadaAlternativa)obj).Inchora;
            parms[34].Value = ((Modelo.JornadaAlternativa)obj).Incusuario;
            parms[35].Value = ((Modelo.JornadaAlternativa)obj).Altdata;
            parms[36].Value = ((Modelo.JornadaAlternativa)obj).Althora;
            parms[37].Value = ((Modelo.JornadaAlternativa)obj).Altusuario;
            parms[38].Value = ((Modelo.JornadaAlternativa)obj).Intervaloautomatico;
            parms[39].Value = ((Modelo.JornadaAlternativa)obj).Preassinaladas1;
            parms[40].Value = ((Modelo.JornadaAlternativa)obj).Preassinaladas2;
            parms[41].Value = ((Modelo.JornadaAlternativa)obj).Preassinaladas3;
            parms[42].Value = ((Modelo.JornadaAlternativa)obj).CalculoAdicionalNoturno;
            parms[43].Value = ((Modelo.JornadaAlternativa)obj).ConversaoHoraNoturna;
            parms[44].Value = ((Modelo.JornadaAlternativa)obj).Idjornada;
        }

        public Modelo.JornadaAlternativa LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.JornadaAlternativa objJornadaAlternativa = new Modelo.JornadaAlternativa();
            try
            {
                SetInstance(dr, objJornadaAlternativa);

                DAL.FB.DiasJornadaAlternativa dalDiasJornadaAlternativa = DAL.FB.DiasJornadaAlternativa.GetInstancia;
                objJornadaAlternativa.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(objJornadaAlternativa.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJornadaAlternativa;
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

            SalvarDiasJA(trans, (Modelo.JornadaAlternativa)obj);

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

            SalvarDiasJA(trans, (Modelo.JornadaAlternativa)obj);

            cmd.Parameters.Clear();
        }

        private void SalvarDiasJA(FbTransaction trans, Modelo.JornadaAlternativa obj)
        {
            DAL.FB.DiasJornadaAlternativa dalDiasJA = DAL.FB.DiasJornadaAlternativa.GetInstancia;
            foreach (Modelo.DiasJornadaAlternativa dja in obj.DiasJA)
            {
                dja.IdJornadaAlternativa = obj.Id;
                switch (dja.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalDiasJA.Incluir(trans, dja);
                        break;
                    case Modelo.Acao.Alterar:
                        dalDiasJA.Alterar(trans, dja);
                        break;
                    case Modelo.Acao.Excluir:
                        dalDiasJA.Excluir(trans, dja);
                        break;
                    default:
                        break;
                }
            }
        }

        public List<Modelo.JornadaAlternativa> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                  new FbParameter("@datainicial", FbDbType.Date)
                , new FbParameter("@datafinal", FbDbType.Date)
            };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;


            string aux = "SELECT DISTINCT \"jornadaalternativa\".\"id\", \"jornadaalternativa\".* FROM \"jornadaalternativa\" " + 
                "LEFT JOIN \"diasjornadaalternativa\" ON \"jornadaalternativa\".\"id\" = \"diasjornadaalternativa\".\"idjornadaalternativa\" " +
                "WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\") " +
                "OR (@datafinal >= \"datainicial\" AND @datafinal <= \"datafinal\") " +
                "OR (@datainicial <= \"datainicial\" AND @datafinal >= \"datafinal\") "+ 
                "OR (\"diasjornadaalternativa\".\"datacompensada\" >= @datainicial AND \"diasjornadaalternativa\".\"datacompensada\" <= @datafinal)) "  +
                "ORDER BY \"jornadaalternativa\".\"id\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.JornadaAlternativa> ret = new List<Modelo.JornadaAlternativa>();

            if (dr.HasRows)
            {
                DAL.FB.DiasJornadaAlternativa dalDiasJornadaAlternativa = DAL.FB.DiasJornadaAlternativa.GetInstancia;
                while (dr.Read())
                {
                    Modelo.JornadaAlternativa ja = new Modelo.JornadaAlternativa();
                    AuxSetInstance(dr, ja);
                    
                    ja.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(ja.Id);
                    ret.Add(ja);
                }
            }

            return ret;
        }

        private FbDataReader getPeriodoFuncionario(int pFuncionario)
        {
            FbDataReader dr = null;
            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@funcionario", FbDbType.Integer, 4) 
            };
            parms[0].Value = pFuncionario;

            string aux = "SELECT \"id\", \"datainicial\", \"datafinal\" FROM \"jornadaalternativa\" WHERE \"tipo\" = 2 and \"identificacao\" = @funcionario ORDER BY \"id\"";
            dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            return dr;
        }

        private FbDataReader getPeriodoFuncao(int pFuncao)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@funcao", FbDbType.Integer, 4)
            };
            parms[0].Value = pFuncao;

            string aux = "SELECT \"id\", \"datainicial\", \"datafinal\" FROM \"jornadaalternativa\" WHERE \"tipo\" = 3 and \"identificacao\" = @funcao ORDER BY \"id\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
        }

        private FbDataReader getPeriodoDepartemento(int pDepartamento)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                new FbParameter("@departamento", FbDbType.Integer, 4)
            };
            parms[0].Value = pDepartamento;

            string aux = "SELECT \"id\", \"datainicial\", \"datafinal\" FROM \"jornadaalternativa\" WHERE \"tipo\" = 1 and \"identificacao\" = @departamento ORDER BY \"id\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
        }

        private FbDataReader getPeriodoEmpresa(int pEmpresa)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                  new FbParameter("@empresa", FbDbType.Integer, 4)
            };
            parms[0].Value = pEmpresa;

            string aux = "SELECT \"id\", \"datainicial\", \"datafinal\" FROM \"jornadaalternativa\" WHERE \"tipo\" = 0 and \"identificacao\" = @empresa ORDER BY \"id\"";

            return FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
        }

        private int VerificaPeriodo(DateTime pData, FbDataReader dr)
        {          
            if (!dr.HasRows)
            {
                return 0;
            }

            int jornada = 0;

            while (dr.Read())
            {
                jornada = Convert.ToInt32(dr["id"]);
                if (pData >= Convert.ToDateTime(dr["datainicial"]) && pData <= Convert.ToDateTime(dr["dataFinal"]))
                {
                    return jornada;
                }
            }

            DAL.FB.DiasJornadaAlternativa dalDiasJA = DAL.FB.DiasJornadaAlternativa.GetInstancia;
            if (jornada != 0)
            {
                foreach (Modelo.DiasJornadaAlternativa j in dalDiasJA.LoadPJornadaAlternativa(jornada))
                {
                    if (j.DataCompensada == pData)
                    {
                        return jornada;
                    }
                }
            }

            dr.Close();
            return 0;
        }

        public Modelo.JornadaAlternativa PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao)
        {
            FbDataReader dr;
            int ret = 0;

            //Verifica e possui registro por Funcionario
            dr = this.getPeriodoFuncionario(pFuncionario);
            ret = VerificaPeriodo(pData, dr);
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            //Verifica e possui registro por Funcao
            dr = this.getPeriodoFuncao(pFuncao);
            ret = VerificaPeriodo(pData, dr);
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            //Verifica e possui registro por Departamento
            dr = this.getPeriodoDepartemento(pDepartamento);
            ret = VerificaPeriodo(pData, dr);
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            //Verifica e possui registro por Empresa
            dr = this.getPeriodoEmpresa(pEmpresa);
            ret = VerificaPeriodo(pData, dr);
            if (ret > 0)
            {
                return this.LoadObject(ret);
            }

            return null;
        }

        public int VerificaExiste(int pId, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@datainicial", FbDbType.Date),
                new FbParameter("@datafinal", FbDbType.Date),
                new FbParameter("@tipo", FbDbType.Integer),
                new FbParameter("@identificacao", FbDbType.Integer),
                new FbParameter("@id", FbDbType.Integer)
            };

            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;
            parms[2].Value = tipo;
            parms[3].Value = identificacao;
            parms[4].Value = pId;

            int qt = (int)FB.DataBase.ExecuteScalar(CommandType.Text, VERIFICA, parms);

            return qt;
        }

        public Modelo.JornadaAlternativa LoadParaUmaMarcacao(DateTime pData, int tipo, int identificacao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@data", FbDbType.Date),
                new FbParameter("@tipo", FbDbType.Integer),
                new FbParameter("@identificacao", FbDbType.Integer)
            };

            parms[0].Value = pData;
            parms[1].Value = tipo;
            parms[2].Value = identificacao;

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, SELECTPE, parms);

            Modelo.JornadaAlternativa objJornadaAlternativa = new Modelo.JornadaAlternativa();
            try
            {
                SetInstance(dr, objJornadaAlternativa);

                DAL.FB.DiasJornadaAlternativa dalDiasJornadaAlternativa = DAL.FB.DiasJornadaAlternativa.GetInstancia;
                objJornadaAlternativa.DiasJA = dalDiasJornadaAlternativa.LoadPJornadaAlternativa(objJornadaAlternativa.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJornadaAlternativa;
        }

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, int? pTipo, List<int> pIdentificacoes)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@datainicial", FbDbType.TimeStamp),
                new FbParameter("@datafinal", FbDbType.TimeStamp)
            };

            Hashtable lista = new Hashtable();
            string SQL = "SELECT * FROM \"jornadaalternativa\"";

            if (pDataI != null && pDataF != null)
            {
                parms[0].Value = pDataI;
                parms[1].Value = pDataF;

                SQL += " WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\")"
                       + " OR (@datafinal >= \"datainicial\" AND @datafinal <= \"datafinal\")"
                       + " OR (@datainicial <= \"datainicial\" AND @datafinal >= \"datafinal\"))"
                       + " OR ((SELECT COUNT(\"id\") FROM \"diasjornadaalternativa\""
                       + " WHERE \"diasjornadaalternativa\".\"datacompensada\" >= @datainicial"
                       + " AND \"diasjornadaalternativa\".\"datacompensada\" <= @datafinal"
                       + " AND \"diasjornadaalternativa\".\"idjornadaalternativa\" = \"jornadaalternativa\".\"id\""
                       + " ) > 0)";
            }

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, SQL, parms);

            if (dr.HasRows)
            {
                Modelo.JornadaAlternativa objJornadaAlternativa = null;
                while (dr.Read())
                {
                    objJornadaAlternativa = new Modelo.JornadaAlternativa();
                    AuxSetInstance(dr, objJornadaAlternativa);
                    lista.Add(Convert.ToInt32(dr["id"]), objJornadaAlternativa);
                }
            }

            return lista;
        }

        public List<Modelo.JornadaAlternativa> GetPeriodoFuncionarios(DateTime pDataInicial, DateTime pDataFinal, List<int> idsFuncs)
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

        #endregion
    }
}
