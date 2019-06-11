using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using Modelo.Proxy;
using Modelo;

namespace DAL.FB
{
    public class Horario : DAL.FB.DALBase, DAL.IHorario
    {
        public string SELECTREL { get; set; }
        public string SELECTTIP { get; set; }

        private Horario()
        {
            GEN = "GEN_horario_id";

            TABELA = "horario";

            SELECTPID = "SELECT \"horario\".* "
                        + ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datainicial\" "
                        + ", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datafinal\" "
                        + " FROM \"horario\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT   \"horario\".\"id\"" +
                                    ", \"horario\".\"descricao\"" +
                                    ", \"horario\".\"codigo\"" +
                                    ", \"horario\".\"horariodescricao_1\"" +
                                    ", \"horario\".\"horariodescricaosai_1\"" +
                                    ", \"horario\".\"horariodescricao_2\"" +
                                    ", \"horario\".\"horariodescricaosai_2\"" +
                                    ", \"horario\".\"horariodescricao_3\"" +
                                    ", \"horario\".\"horariodescricaosai_3\"" +
                                    ", \"horario\".\"horariodescricao_4\"" +
                                    ", \"horario\".\"horariodescricaosai_4\"" +
                                    ", case when \"horario\".\"tipohorario\" = 1 then 'Normal' else 'Flexível' end AS \"tipohorario\"" +
                             " FROM \"horario\" ORDER BY \"horario\".\"tipohorario\", \"horario\".\"descricao\"";

            SELECTTIP = "   SELECT   \"horario\".\"id\"" +
                                    ", \"horario\".\"descricao\"" +
                                    ", \"horario\".\"codigo\"" +
                                    ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datainicial\" " +
                                    ", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datafinal\" " +
                             "FROM \"horario\"" +
                             "WHERE \"horario\".\"tipohorario\" = @tipohorario ";

            SELECTREL = "   SELECT   \"horario\".\"id\"" +
                                    ", \"horario\".\"codigo\"" +
                                    ", \"horario\".\"descricao\" AS \"descricaoturno\"" +
                                    ", (SELECT FIRST 1 COALESCE(\"hd\".\"entrada_1\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_1\", '--:--') || ' | ' || COALESCE(\"hd\".\"entrada_2\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_2\", '--:--') || + " +
                                    " ' | ' || COALESCE(\"hd\".\"entrada_3\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_3\", '--:--') || ' | ' || COALESCE(\"hd\".\"entrada_4\", '--:--') || ' - ' || COALESCE(\"hd\".\"saida_4\", '--:--') " +
                                    " FROM \"horariodetalhe\" \"hd\" WHERE \"hd\".\"idhorario\" = \"horario\".\"id\" AND (\"hd\".\"dia\" = 1 OR EXTRACT(WEEKDAY FROM \"hd\".\"data\") = 1)) AS \"descricaohorario\"" +
                             " FROM \"horario\"" +
                             " WHERE \"horario\".\"id\" > 0 ";

            INSERT = "  INSERT INTO \"horario\"" +
                                        "(\"codigo\", \"descricao\", \"horariodescricao_1\", \"horariodescricao_2\", \"horariodescricao_3\", \"horariodescricao_4\", \"horariodescricaosai_1\", \"horariodescricaosai_2\", \"horariodescricaosai_3\", \"horariodescricaosai_4\", \"idparametro\", \"horasnormais\", \"somentecargahoraria\", \"marcacargahorariamista\", \"habilitatolerancia\", \"conversaohoranoturna\", \"consideraadhtrabalhadas\", \"ordem_ent\", \"ordenabilhetesaida\", \"limitemin\", \"limitemax\", \"tipoacumulo\", \"habilitaperiodo01\", \"habilitaperiodo02\", \"descontacafemanha\", \"descontacafetarde\", \"dias_cafe_1\", \"dias_cafe_2\", \"dias_cafe_3\", \"dias_cafe_4\", \"dias_cafe_5\", \"dias_cafe_6\", \"dias_cafe_7\", \"descontafalta50\", \"considerasabadosemana\", \"consideradomingosemana\", \"horaextrasab50_100\", \"perchextrasab50_100\", \"refeicao_01\", \"refeicao_02\", \"obs\", \"descontardsr\", \"qtdhorasdsr\", \"diasemanadsr\", \"limiteperdadsr\", \"incdata\", \"inchora\", \"incusuario\", \"tipohorario\", \"intervaloautomatico\", \"preassinaladas1\", \"preassinaladas2\", \"preassinaladas3\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @horariodescricao_1, @horariodescricao_2, @horariodescricao_3, @horariodescricao_4, @horariodescricaosai_1, @horariodescricaosai_2, @horariodescricaosai_3, @horariodescricaosai_4, @idparametro, @horasnormais, @somentecargahoraria, @marcacargahorariamista, @habilitatolerancia, @conversaohoranoturna, @consideraadhtrabalhadas, @ordem_ent, @ordenabilhetesaida, @limitemin, @limitemax, @tipoacumulo, @habilitaperiodo01, @habilitaperiodo02, @descontacafemanha, @descontacafetarde, @dias_cafe_1, @dias_cafe_2, @dias_cafe_3, @dias_cafe_4, @dias_cafe_5, @dias_cafe_6, @dias_cafe_7, @descontafalta50, @considerasabadosemana, @consideradomingosemana, @horaextrasab50_100, @perchextrasab50_100, @refeicao_01, @refeicao_02, @obs, @descontardsr, @qtdhorasdsr, @diasemanadsr, @limiteperdadsr, @incdata, @inchora, @incusuario, @tipohorario, @intervaloautomatico, @preassinaladas1, @preassinaladas2, @preassinaladas3)";

            UPDATE = "  UPDATE \"horario\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"horariodescricao_1\" = @horariodescricao_1 " +
                                        ", \"horariodescricao_2\" = @horariodescricao_2 " +
                                        ", \"horariodescricao_3\" = @horariodescricao_3 " +
                                        ", \"horariodescricao_4\" = @horariodescricao_4 " +
                                        ", \"horariodescricaosai_1\" = @horariodescricaosai_1 " +
                                        ", \"horariodescricaosai_2\" = @horariodescricaosai_2 " +
                                        ", \"horariodescricaosai_3\" = @horariodescricaosai_3 " +
                                        ", \"horariodescricaosai_4\" = @horariodescricaosai_4 " +
                                        ", \"idparametro\" = @idparametro " +
                                        ", \"horasnormais\" = @horasnormais " +
                                        ", \"somentecargahoraria\" = @somentecargahoraria " +
                                        ", \"marcacargahorariamista\" = @marcacargahorariamista " +
                                        ", \"habilitatolerancia\" = @habilitatolerancia " +
                                        ", \"conversaohoranoturna\" = @conversaohoranoturna " +
                                        ", \"consideraadhtrabalhadas\" = @consideraadhtrabalhadas " +
                                        ", \"ordem_ent\" = @ordem_ent " +
                                        ", \"ordenabilhetesaida\" = @ordenabilhetesaida " +
                                        ", \"limitemin\" = @limitemin " +
                                        ", \"limitemax\" = @limitemax " +
                                        ", \"tipoacumulo\" = @tipoacumulo " +
                                        ", \"habilitaperiodo01\" = @habilitaperiodo01 " +
                                        ", \"habilitaperiodo02\" = @habilitaperiodo02 " +
                                        ", \"descontacafemanha\" = @descontacafemanha " +
                                        ", \"descontacafetarde\" = @descontacafetarde " +
                                        ", \"dias_cafe_1\" = @dias_cafe_1 " +
                                        ", \"dias_cafe_2\" = @dias_cafe_2 " +
                                        ", \"dias_cafe_3\" = @dias_cafe_3 " +
                                        ", \"dias_cafe_4\" = @dias_cafe_4 " +
                                        ", \"dias_cafe_5\" = @dias_cafe_5 " +
                                        ", \"dias_cafe_6\" = @dias_cafe_6 " +
                                        ", \"dias_cafe_7\" = @dias_cafe_7 " +
                                        ", \"descontafalta50\" = @descontafalta50 " +
                                        ", \"considerasabadosemana\" = @considerasabadosemana " +
                                        ", \"consideradomingosemana\" = @consideradomingosemana " +
                                        ", \"horaextrasab50_100\" = @horaextrasab50_100 " +
                                        ", \"perchextrasab50_100\" = @perchextrasab50_100 " +
                                        ", \"refeicao_01\" = @refeicao_01 " +
                                        ", \"refeicao_02\" = @refeicao_02 " +
                                        ", \"obs\" = @obs " +
                                        ", \"descontardsr\" = @descontardsr " +
                                        ", \"qtdhorasdsr\" = @qtdhorasdsr " +
                                        ", \"diasemanadsr\" = @diasemanadsr " +
                                        ", \"limiteperdadsr\" = @limiteperdadsr " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"tipohorario\" = @tipohorario " +
                                        ", \"intervaloautomatico\" = @intervaloautomatico " +
                                        ", \"preassinaladas1\" = @preassinaladas1 " +
                                        ", \"preassinaladas2\" = @preassinaladas2 " +
                                        ", \"preassinaladas3\" = @preassinaladas3 " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"horario\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"horario\"";

        }

        #region Singleton

        private static volatile FB.Horario _instancia = null;

        public static FB.Horario GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Horario))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Horario();
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
                obj = new Modelo.Horario();
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
            ((Modelo.Horario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Horario)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Horario)obj).Horariodescricao_1 = Convert.ToString(dr["horariodescricao_1"]);
            ((Modelo.Horario)obj).Horariodescricao_2 = Convert.ToString(dr["horariodescricao_2"]);
            ((Modelo.Horario)obj).Horariodescricao_3 = Convert.ToString(dr["horariodescricao_3"]);
            ((Modelo.Horario)obj).Horariodescricao_4 = Convert.ToString(dr["horariodescricao_4"]);
            ((Modelo.Horario)obj).Horariodescricaosai_1 = Convert.ToString(dr["horariodescricaosai_1"]);
            ((Modelo.Horario)obj).Horariodescricaosai_2 = Convert.ToString(dr["horariodescricaosai_2"]);
            ((Modelo.Horario)obj).Horariodescricaosai_3 = Convert.ToString(dr["horariodescricaosai_3"]);
            ((Modelo.Horario)obj).Horariodescricaosai_4 = Convert.ToString(dr["horariodescricaosai_4"]);
            ((Modelo.Horario)obj).Idparametro = Convert.ToInt32(dr["idparametro"]);
            ((Modelo.Horario)obj).Horasnormais = Convert.ToInt16(dr["horasnormais"]);
            ((Modelo.Horario)obj).Somentecargahoraria = Convert.ToInt16(dr["somentecargahoraria"]);
            ((Modelo.Horario)obj).Marcacargahorariamista = Convert.ToInt16(dr["marcacargahorariamista"]);
            ((Modelo.Horario)obj).Conversaohoranoturna = Convert.ToInt16(dr["conversaohoranoturna"]);
            ((Modelo.Horario)obj).Consideraadhtrabalhadas = Convert.ToInt16(dr["consideraadhtrabalhadas"]);
            ((Modelo.Horario)obj).Ordem_ent = Convert.ToInt16(dr["ordem_ent"]);
            ((Modelo.Horario)obj).Ordenabilhetesaida = Convert.ToInt16(dr["ordenabilhetesaida"]);
            ((Modelo.Horario)obj).Limitemin = Convert.ToString(dr["limitemin"]);
            ((Modelo.Horario)obj).Limitemax = Convert.ToString(dr["limitemax"]);
            ((Modelo.Horario)obj).Tipoacumulo = Convert.ToInt16(dr["tipoacumulo"]);
            ((Modelo.Horario)obj).Habilitaperiodo01 = Convert.ToInt16(dr["habilitaperiodo01"]);
            ((Modelo.Horario)obj).Habilitaperiodo02 = Convert.ToInt16(dr["habilitaperiodo02"]);
            ((Modelo.Horario)obj).Descontacafemanha = Convert.ToInt16(dr["descontacafemanha"]);
            ((Modelo.Horario)obj).Descontacafetarde = Convert.ToInt16(dr["descontacafetarde"]);
            ((Modelo.Horario)obj).Dias_cafe_1 = Convert.ToInt16(dr["dias_cafe_1"]);
            ((Modelo.Horario)obj).Dias_cafe_2 = Convert.ToInt16(dr["dias_cafe_2"]);
            ((Modelo.Horario)obj).Dias_cafe_3 = Convert.ToInt16(dr["dias_cafe_3"]);
            ((Modelo.Horario)obj).Dias_cafe_4 = Convert.ToInt16(dr["dias_cafe_4"]);
            ((Modelo.Horario)obj).Dias_cafe_5 = Convert.ToInt16(dr["dias_cafe_5"]);
            ((Modelo.Horario)obj).Dias_cafe_6 = Convert.ToInt16(dr["dias_cafe_6"]);
            ((Modelo.Horario)obj).Dias_cafe_7 = Convert.ToInt16(dr["dias_cafe_7"]);
            ((Modelo.Horario)obj).DesconsiderarFeriado = Convert.ToInt16(dr["desconsiderarferiado"]);
            ((Modelo.Horario)obj).Descontafalta50 = Convert.ToInt16(dr["descontafalta50"]);
            ((Modelo.Horario)obj).Considerasabadosemana = Convert.ToInt16(dr["considerasabadosemana"]);
            ((Modelo.Horario)obj).Consideradomingosemana = Convert.ToInt16(dr["consideradomingosemana"]);
            ((Modelo.Horario)obj).ConsiderarAdicionalNoturnoInterv = Convert.ToInt16(dr["consideraradicionalnoturnointerv"] is DBNull ? 0 : dr["consideraradicionalnoturnointerv"]);
            ((Modelo.Horario)obj).Horaextrasab50_100 = Convert.ToInt16(dr["horaextrasab50_100"]);
            ((Modelo.Horario)obj).Perchextrasab50_100 = Convert.ToInt16(dr["perchextrasab50_100"]);
            ((Modelo.Horario)obj).Refeicao_01 = Convert.ToString(dr["refeicao_01"]);
            ((Modelo.Horario)obj).Refeicao_02 = Convert.ToString(dr["refeicao_02"]);
            ((Modelo.Horario)obj).Obs = Convert.ToString(dr["obs"]);
            ((Modelo.Horario)obj).Descontardsr = Convert.ToInt16(dr["descontardsr"]);
            ((Modelo.Horario)obj).Qtdhorasdsr = Convert.ToString(dr["qtdhorasdsr"]);
            ((Modelo.Horario)obj).Diasemanadsr = Convert.ToInt32(dr["diasemanadsr"]);
            ((Modelo.Horario)obj).Limiteperdadsr = Convert.ToString(dr["limiteperdadsr"]);
            ((Modelo.Horario)obj).TipoHorario = Convert.ToInt32(dr["tipohorario"]);
            ((Modelo.Horario)obj).DataInicial = (dr["datainicial"] is DBNull ? null : (DateTime?)dr["datainicial"]);
            ((Modelo.Horario)obj).DataFinal = (dr["datafinal"] is DBNull ? null : (DateTime?)dr["datafinal"]);
            ((Modelo.Horario)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.Horario)obj).Preassinaladas1 = (dr["preassinaladas1"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas1"]));
            ((Modelo.Horario)obj).Preassinaladas2 = (dr["preassinaladas2"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas2"]));
            ((Modelo.Horario)obj).Preassinaladas3 = (dr["preassinaladas3"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas3"]));
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@horariodescricao_1", FbDbType.VarChar),
				new FbParameter ("@horariodescricao_2", FbDbType.VarChar),
				new FbParameter ("@horariodescricao_3", FbDbType.VarChar),
				new FbParameter ("@horariodescricao_4", FbDbType.VarChar),
				new FbParameter ("@horariodescricaosai_1", FbDbType.VarChar),
				new FbParameter ("@horariodescricaosai_2", FbDbType.VarChar),
				new FbParameter ("@horariodescricaosai_3", FbDbType.VarChar),
				new FbParameter ("@horariodescricaosai_4", FbDbType.VarChar),
				new FbParameter ("@idparametro", FbDbType.Integer),
				new FbParameter ("@horasnormais", FbDbType.SmallInt),
				new FbParameter ("@somentecargahoraria", FbDbType.SmallInt),
				new FbParameter ("@marcacargahorariamista", FbDbType.SmallInt),
				new FbParameter ("@habilitatolerancia", FbDbType.SmallInt),
				new FbParameter ("@conversaohoranoturna", FbDbType.SmallInt),
				new FbParameter ("@consideraadhtrabalhadas", FbDbType.SmallInt),
				new FbParameter ("@ordem_ent", FbDbType.SmallInt),
				new FbParameter ("@ordenabilhetesaida", FbDbType.SmallInt),
				new FbParameter ("@limitemin", FbDbType.VarChar),
				new FbParameter ("@limitemax", FbDbType.VarChar),
				new FbParameter ("@tipoacumulo", FbDbType.Integer),
				new FbParameter ("@habilitaperiodo01", FbDbType.SmallInt),
				new FbParameter ("@habilitaperiodo02", FbDbType.SmallInt),
				new FbParameter ("@descontacafemanha", FbDbType.SmallInt),
				new FbParameter ("@descontacafetarde", FbDbType.SmallInt),
				new FbParameter ("@dias_cafe_1", FbDbType.SmallInt),
				new FbParameter ("@dias_cafe_2", FbDbType.SmallInt),
				new FbParameter ("@dias_cafe_3", FbDbType.SmallInt),
				new FbParameter ("@dias_cafe_4", FbDbType.SmallInt),
				new FbParameter ("@dias_cafe_5", FbDbType.SmallInt),
				new FbParameter ("@dias_cafe_6", FbDbType.SmallInt),
				new FbParameter ("@dias_cafe_7", FbDbType.SmallInt),
                new FbParameter ("@desconsiderarferiado", FbDbType.SmallInt),
				new FbParameter ("@descontafalta50", FbDbType.SmallInt),
				new FbParameter ("@considerasabadosemana", FbDbType.SmallInt),
				new FbParameter ("@consideradomingosemana", FbDbType.SmallInt),
				new FbParameter ("@horaextrasab50_100", FbDbType.SmallInt),
				new FbParameter ("@perchextrasab50_100", FbDbType.SmallInt),
				new FbParameter ("@refeicao_01", FbDbType.VarChar),
				new FbParameter ("@refeicao_02", FbDbType.VarChar),
				new FbParameter ("@obs", FbDbType.VarChar),
				new FbParameter ("@descontardsr", FbDbType.SmallInt),
				new FbParameter ("@qtdhorasdsr", FbDbType.VarChar),
				new FbParameter ("@diasemanadsr", FbDbType.Integer),
				new FbParameter ("@limiteperdadsr", FbDbType.VarChar),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
				new FbParameter ("@tipohorario", FbDbType.Integer),
                new FbParameter ("@intervaloautomatico", FbDbType.Integer),
                new FbParameter ("@preassinaladas1", FbDbType.Integer),
                new FbParameter ("@preassinaladas2", FbDbType.Integer),
                new FbParameter ("@preassinaladas3", FbDbType.Integer)
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
            parms[1].Value = ((Modelo.Horario)obj).Codigo;
            parms[2].Value = ((Modelo.Horario)obj).Descricao;
            parms[3].Value = ((Modelo.Horario)obj).Horariodescricao_1;
            parms[4].Value = ((Modelo.Horario)obj).Horariodescricao_2;
            parms[5].Value = ((Modelo.Horario)obj).Horariodescricao_3;
            parms[6].Value = ((Modelo.Horario)obj).Horariodescricao_4;
            parms[7].Value = ((Modelo.Horario)obj).Horariodescricaosai_1;
            parms[8].Value = ((Modelo.Horario)obj).Horariodescricaosai_2;
            parms[9].Value = ((Modelo.Horario)obj).Horariodescricaosai_3;
            parms[10].Value = ((Modelo.Horario)obj).Horariodescricaosai_4;
            parms[11].Value = ((Modelo.Horario)obj).Idparametro;
            parms[12].Value = ((Modelo.Horario)obj).Horasnormais;
            parms[13].Value = ((Modelo.Horario)obj).Somentecargahoraria;
            parms[14].Value = ((Modelo.Horario)obj).Marcacargahorariamista;
            parms[15].Value = ((Modelo.Horario)obj).Habilitatolerancia;
            parms[16].Value = ((Modelo.Horario)obj).Conversaohoranoturna;
            parms[17].Value = ((Modelo.Horario)obj).Consideraadhtrabalhadas;
            parms[18].Value = ((Modelo.Horario)obj).Ordem_ent;
            parms[19].Value = ((Modelo.Horario)obj).Ordenabilhetesaida;
            parms[20].Value = ((Modelo.Horario)obj).Limitemin;
            parms[21].Value = ((Modelo.Horario)obj).Limitemax;
            parms[22].Value = ((Modelo.Horario)obj).Tipoacumulo;
            parms[23].Value = ((Modelo.Horario)obj).Habilitaperiodo01;
            parms[24].Value = ((Modelo.Horario)obj).Habilitaperiodo02;
            parms[25].Value = ((Modelo.Horario)obj).Descontacafemanha;
            parms[26].Value = ((Modelo.Horario)obj).Descontacafetarde;
            parms[27].Value = ((Modelo.Horario)obj).Dias_cafe_1;
            parms[28].Value = ((Modelo.Horario)obj).Dias_cafe_2;
            parms[29].Value = ((Modelo.Horario)obj).Dias_cafe_3;
            parms[30].Value = ((Modelo.Horario)obj).Dias_cafe_4;
            parms[31].Value = ((Modelo.Horario)obj).Dias_cafe_5;
            parms[32].Value = ((Modelo.Horario)obj).Dias_cafe_6;
            parms[33].Value = ((Modelo.Horario)obj).Dias_cafe_7;
            parms[34].Value = ((Modelo.Horario)obj).Descontafalta50;
            parms[35].Value = ((Modelo.Horario)obj).Considerasabadosemana;
            parms[36].Value = ((Modelo.Horario)obj).Consideradomingosemana;
            parms[37].Value = ((Modelo.Horario)obj).Horaextrasab50_100;
            parms[38].Value = ((Modelo.Horario)obj).Perchextrasab50_100;
            parms[39].Value = ((Modelo.Horario)obj).Refeicao_01;
            parms[40].Value = ((Modelo.Horario)obj).Refeicao_02;
            parms[41].Value = ((Modelo.Horario)obj).Obs;
            parms[42].Value = ((Modelo.Horario)obj).Descontardsr;
            parms[43].Value = ((Modelo.Horario)obj).Qtdhorasdsr;
            parms[44].Value = ((Modelo.Horario)obj).Diasemanadsr;
            parms[45].Value = ((Modelo.Horario)obj).Limiteperdadsr;
            parms[46].Value = ((Modelo.Horario)obj).Incdata;
            parms[47].Value = ((Modelo.Horario)obj).Inchora;
            parms[48].Value = ((Modelo.Horario)obj).Incusuario;
            parms[49].Value = ((Modelo.Horario)obj).Altdata;
            parms[50].Value = ((Modelo.Horario)obj).Althora;
            parms[51].Value = ((Modelo.Horario)obj).Altusuario;
            parms[52].Value = ((Modelo.Horario)obj).TipoHorario;
            parms[53].Value = ((Modelo.Horario)obj).Intervaloautomatico;
            parms[54].Value = ((Modelo.Horario)obj).Preassinaladas1;
            parms[55].Value = ((Modelo.Horario)obj).Preassinaladas2;
            parms[56].Value = ((Modelo.Horario)obj).Preassinaladas3;
        }

        public Modelo.Horario LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Horario objHorario = new Modelo.Horario();
            try
            {
                SetInstance(dr, objHorario);

                objHorario.HorariosDetalhe = new Modelo.HorarioDetalhe[7];
                DAL.FB.HorarioDetalhe dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
                List<Modelo.HorarioDetalhe> lista = dalHorarioDetalhe.LoadPorHorario(objHorario.Id);

                if (objHorario.TipoHorario == 1)
                {
                    foreach (Modelo.HorarioDetalhe hd in lista)
                    {
                        objHorario.HorariosDetalhe[hd.Codigo] = hd;
                        objHorario.HorariosDetalhe[hd.Codigo].Acao = Modelo.Acao.Alterar;
                    }
                }
                else if (objHorario.TipoHorario == 2)
                {
                    objHorario.HorariosFlexiveis = lista;
                }

                objHorario.HorariosPHExtra = new Modelo.HorarioPHExtra[10];
                DAL.FB.HorarioPHExtra dalHorarioPHExtra = DAL.FB.HorarioPHExtra.GetInstancia;
                List<Modelo.HorarioPHExtra> listaPHE = dalHorarioPHExtra.LoadPorHorario(objHorario.Id);
                foreach (Modelo.HorarioPHExtra hd in listaPHE)
                {
                    if (hd.Codigo < 10)
                        objHorario.HorariosPHExtra[hd.Codigo] = hd;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorario;
        }

        /// <summary>
        /// Método responsável em verificar a quantidade de registros cadastrados com um determinado campo
        /// em uma determinada tabela
        /// </summary>
        /// <param name="trans">Transação (conection)</param>
        /// <param name="tabela">Nome da tabela</param>
        /// <param name="campo">Nome do campo que deseja pesquisar</param>
        /// <param name="valor">Valor que deseja pesquisar</param>
        /// <param name="id">Informar o id do registro no caso de alteração, caso contrário informa 0</param>
        /// <param name="pTipoHorario">Tipo do Horário</param>
        /// <returns></returns>
        private int CountCodigo(FbTransaction trans, string tabela, string campo, int valor, int id, int pTipoHorario)
        {
            //CRNC - 07/01/2010
            StringBuilder str = new StringBuilder("SELECT COUNT(");
            str.Append("\"" + campo + "\"");
            str.Append(") AS \"qt\" FROM ");
            str.Append("\"" + tabela + "\"");
            str.Append(" WHERE \"" + tabela + "\".\"" + campo + "\"");
            str.Append(" = @parametro");
            str.Append(" AND \"tipohorario\" = @tipohorario");

            if (id > 0)
            {
                str.Append(" AND \"id\" <> @id");
            }

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@parametro", FbDbType.Integer, 4),
                new FbParameter("@id", FbDbType.Integer),
                new FbParameter("@tipohorario", FbDbType.Integer)
            };
            parms[0].Value = valor;
            parms[1].Value = id;
            parms[2].Value = pTipoHorario;

            return (int)FB.DataBase.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms);
        }

        public List<Modelo.Horario> getPorParametro(int pIdParametro)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idparametro", FbDbType.Integer) };
            parms[0].Value = pIdParametro;

            string comando = " SELECT \"horario\".* "
                        + ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datainicial\" "
                        + ", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datafinal\" "
                        + "FROM \"horario\" WHERE \"idparametro\" = @idparametro";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }

            return listaHorario;
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (CountCodigo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0, ((Modelo.Horario)obj).TipoHorario) > 0) ////CRNC - 07/01/2010
            {
                parms[1].Value = MaxCodigo(trans);
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            AuxManutencao(trans, obj);

            DAL.FB.HorarioPHExtra dalHorarioPHExtra = DAL.FB.HorarioPHExtra.GetInstancia;
            for (int i = 0; i < ((Modelo.Horario)obj).HorariosPHExtra.Length; i++)
            {
                if (((Modelo.Horario)obj).HorariosPHExtra[i] != null)
                {
                    ((Modelo.Horario)obj).HorariosPHExtra[i].Idhorario = ((Modelo.Horario)obj).Id;
                    ((Modelo.Horario)obj).HorariosPHExtra[i].Codigo = i;
                    dalHorarioPHExtra.Incluir(trans, ((Modelo.Horario)obj).HorariosPHExtra[i]);
                }
            }

            cmd.Parameters.Clear();
        }

        private static void AuxManutencao(FbTransaction trans, Modelo.ModeloBase obj)
        {
            DAL.FB.HorarioDetalhe dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
            if (((Modelo.Horario)obj).TipoHorario == 1)
            {
                for (int i = 0; i < ((Modelo.Horario)obj).HorariosDetalhe.Length; i++)
                {
                    ((Modelo.Horario)obj).HorariosDetalhe[i].Idhorario = ((Modelo.Horario)obj).Id;
                    switch (((Modelo.Horario)obj).HorariosDetalhe[i].Acao)
                    {
                        case Modelo.Acao.Incluir:
                            ((Modelo.Horario)obj).HorariosDetalhe[i].Codigo = i;
                            dalHorarioDetalhe.Incluir(trans, ((Modelo.Horario)obj).HorariosDetalhe[i]);
                            break;
                        case Modelo.Acao.Alterar:
                            dalHorarioDetalhe.Alterar(trans, ((Modelo.Horario)obj).HorariosDetalhe[i]);
                            break;
                    }
                }
            }
            else if (((Modelo.Horario)obj).TipoHorario == 2)
            {
                foreach (Modelo.HorarioDetalhe hd in ((Modelo.Horario)obj).HorariosFlexiveis)
                {
                    hd.Idhorario = ((Modelo.Horario)obj).Id;
                    switch (hd.Acao)
                    {
                        case Modelo.Acao.Incluir:
                            dalHorarioDetalhe.Incluir(trans, hd);
                            break;
                        case Modelo.Acao.Alterar:
                            dalHorarioDetalhe.Alterar(trans, hd);
                            break;
                        case Modelo.Acao.Excluir:
                            dalHorarioDetalhe.Excluir(trans, hd);
                            break;
                    }
                }
            }
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (CountCodigo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id, ((Modelo.Horario)obj).TipoHorario) > 0) //CRNC - 07/01/2010
            {
                throw new Exception("O código informado já está sendo utilizando em outro registro. Verifique.");
            }

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            DAL.FB.HorarioPHExtra dalHorarioPHExtra = DAL.FB.HorarioPHExtra.GetInstancia;
            for (int i = 0; i < ((Modelo.Horario)obj).HorariosPHExtra.Length; i++)
            {
                ((Modelo.Horario)obj).HorariosPHExtra[i].Idhorario = ((Modelo.Horario)obj).Id;
                ((Modelo.Horario)obj).HorariosPHExtra[i].Codigo = i;
                if (((Modelo.Horario)obj).HorariosPHExtra[i].Id == 0)
                {
                    dalHorarioPHExtra.Incluir(trans, ((Modelo.Horario)obj).HorariosPHExtra[i]);
                }
                else
                {
                    dalHorarioPHExtra.Alterar(trans, ((Modelo.Horario)obj).HorariosPHExtra[i]);
                }
            }

            cmd.Parameters.Clear();
        }


        public DataTable GetPorDescricao(string pHorarios)
        {
            FbParameter[] parms = new FbParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND \"horario\".\"id\" IN " + pHorarios;
            aux += " ORDER BY \"horario\".\"descricao\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetHorarioNormal()
        {
            FbParameter[] parms = new FbParameter[1] { new FbParameter("@tipohorario", FbDbType.Integer) };
            parms[0].Value = 1;

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTTIP + " ORDER BY \"horario\".\"descricao\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetHorarioMovel()
        {
            FbParameter[] parms = new FbParameter[1] { new FbParameter("@tipohorario", FbDbType.Integer) };
            parms[0].Value = 2;

            DataTable dt = new DataTable();

            string aux;

            aux = SELECTTIP + " ORDER BY \"horario\".\"descricao\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetHorarioTipo(int pTipoHorario)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@tipohorario", FbDbType.Integer, 4) };
            parms[0].Value = pTipoHorario;

            string aux = "SELECT   \"horario\".\"id\"" +
                                    ", \"horario\".\"descricao\"" +
                                    ", \"horario\".\"codigo\"" +
                           "FROM \"horario\" " +
                           "WHERE \"horario\".\"tipohorario\" = @tipohorario";

            DataTable dt = new DataTable();
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public List<Modelo.Horario> getTodosHorariosDaEmpresa(int pIdEmpresa)
        {
            //Mudar para FB
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idempresa", FbDbType.Integer) };
            parms[0].Value = pIdEmpresa;

            string comando = "SELECT DISTINCT \"horario\".* " +
                             ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datainicial\" " +
                             ", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datafinal\" " +
                             "FROM \"horario\" " +
                             "INNER JOIN \"funcionario\" ON \"funcionario\".\"idhorario\" = \"horario\".\"id\" " +
                             "WHERE \"funcionario\".\"idempresa\" = @idempresa";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);
            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }

            return listaHorario;
        }

        public List<Modelo.Horario> GetParaIncluirMarcacao(Hashtable ids, bool carregaHorarioDetalhe)
        {
            FbParameter[] parms = new FbParameter[0];
            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT \"horario\".* " + ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datainicial\"");
            cmd.AppendLine(", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datafinal\"");
            cmd.AppendLine("FROM \"horario\"");
            cmd.AppendLine("WHERE \"horario\".\"id\" IN (");
            int count = 0;
            foreach (object id in ids.Keys)
            {
                if (count > 0)
                    cmd.Append(", ");
                cmd.Append(Convert.ToInt32(id));
                count++;
            }
            cmd.Append(")");
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, cmd.ToString(), parms);

            DAL.FB.HorarioDetalhe dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.LoadPorHorario(ids);
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);

                #region Horario Detalhe
                if (carregaHorarioDetalhe)
                {
                    if (objHorario.TipoHorario == 1)
                    {
                        objHorario.HorariosDetalhe = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToArray();
                    }
                    else if (objHorario.TipoHorario == 2)
                    {
                        objHorario.HorariosFlexiveis = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToList();
                    }
                }
                #endregion

                listaHorario.Add(objHorario);
            }

            return listaHorario;
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais)
        {
            FbParameter[] parms = new FbParameter[0];
            string cmd = "SELECT \"horario\".* " + ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datainicial\" "
                        + ", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datafinal\" "
                        + " FROM \"horario\"";
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, cmd, parms);

            DAL.FB.HorarioDetalhe dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.GetAllList();
            }

            DAL.FB.HorarioPHExtra dalHorarioPHExtra = DAL.FB.HorarioPHExtra.GetInstancia;
            List<Modelo.HorarioPHExtra> listaPercentuais = null;
            if (carregaPercentuais)
            {
                listaPercentuais = dalHorarioPHExtra.GetAllList();
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);

                #region Horario Detalhe
                if (carregaHorarioDetalhe)
                {
                    objHorario.HorariosDetalhe = new Modelo.HorarioDetalhe[7];

                    if (objHorario.TipoHorario == 1)
                    {
                        objHorario.HorariosDetalhe = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToArray();
                    }
                    else if (objHorario.TipoHorario == 2)
                    {
                        objHorario.HorariosFlexiveis = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToList();
                    }
                }
                #endregion

                #region Percentuais

                if (carregaPercentuais)
                {
                    objHorario.HorariosPHExtra = listaPercentuais.Where(p => p.Idhorario == objHorario.Id).ToArray();
                }

                #endregion

                listaHorario.Add(objHorario);
            }

            return listaHorario;
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipo, string pIds)
        {
            FbParameter[] parms = new FbParameter[0];
            string cmd = "SELECT DISTINCT \"funcionario\".\"idhorario\", \"horario\".* " + ", (SELECT MIN(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datainicial\" "
                        + ", (SELECT MAX(\"data\") FROM \"horariodetalhe\"  WHERE \"horariodetalhe\".\"idhorario\" = \"horario\".\"id\") AS \"datafinal\" "
                        + " FROM \"funcionario\""
                        + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\""
                        + " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\""
                        + " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\"";

            switch (tipo)
            {
                case 0: cmd += " WHERE \"empresa\".\"id\" IN " + pIds; break;
                case 1: cmd += " WHERE \"departamento\".\"id\" IN " + pIds; break;
                case 2: cmd += " WHERE \"funcionario\".\"id\" IN " + pIds; break;
            }

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, cmd, parms);

            DAL.FB.HorarioDetalhe dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.GetAllList();
            }

            DAL.FB.HorarioPHExtra dalHorarioPHExtra = DAL.FB.HorarioPHExtra.GetInstancia;
            List<Modelo.HorarioPHExtra> listaPercentuais = null;
            if (carregaPercentuais)
            {
                listaPercentuais = dalHorarioPHExtra.GetAllList();
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);

                #region Horario Detalhe
                if (carregaHorarioDetalhe)
                {
                    objHorario.HorariosDetalhe = new Modelo.HorarioDetalhe[7];

                    if (objHorario.TipoHorario == 1)
                    {
                        objHorario.HorariosDetalhe = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToArray();
                    }
                    else if (objHorario.TipoHorario == 2)
                    {
                        objHorario.HorariosFlexiveis = listaHorariosDetalhe.Where(h => h.Idhorario == objHorario.Id).ToList();
                    }
                }
                #endregion

                #region Percentuais

                if (carregaPercentuais)
                {
                    objHorario.HorariosPHExtra = listaPercentuais.Where(p => p.Idhorario == objHorario.Id).ToArray();
                }

                #endregion

                listaHorario.Add(objHorario);
            }

            return listaHorario;
        }

        public List<int> GetIds()
        {
            List<int> lista = new List<int>();

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"id\" FROM \"horario\"", new FbParameter[] { });

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

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"codigo\", \"id\" FROM \"horario\"", new FbParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }

            return lista;
        }

        public Hashtable GetHashCodigoIdNormal()
        {
            Hashtable lista = new Hashtable();

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"codigo\", \"id\" FROM \"horario\" WHERE \"tipohorario\" = 1", new FbParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }

            return lista;
        }

        public Hashtable GetHashCodigoIdFlexivel()
        {
            Hashtable lista = new Hashtable();

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT \"codigo\", \"id\" FROM \"horario\" WHERE \"tipohorario\" = 2", new FbParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }

            return lista;
        }

        public int? GetIdPorCodigo(int Cod)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipohorario)
        {
            throw new NotImplementedException();
        }
        #endregion


        public List<Modelo.Horario> GetHorarioNormalMovelList(int tipoHorario)
        {
            throw new NotImplementedException();
        }

        public int MinIdHorarioNormal()
        {
            throw new NotImplementedException();
        }


        public List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.pxyHistoricoAlteracaoHorario> GetHistoricoAlteracaoHorario(int ids)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Horario> GetAllListOrigem(bool a, bool b, int c)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Horario> GetAllListOrigem(bool a, bool b)
        {
            throw new NotImplementedException();
        }

        public Modelo.Horario GetHorEntradaSaidaFunc(int idhorario)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.PxyGridHorarioFlexivel> GetAllGrid(int tipoHorario)
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

        public List<PxyGridHorarioDinamico> GetAllGrid2(int tipoRelogio)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }
    }
}
