using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;

namespace DAL.SQL
{
    public class Horario : DAL.SQL.DALBase, DAL.IHorario
    {
        private DAL.SQL.HorarioDetalhe _dalHorarioDetalhe;
        public DAL.SQL.HorarioDetalhe dalHorarioDetalhe {
            get { return _dalHorarioDetalhe; }
            set { _dalHorarioDetalhe = value; }
        }

        private DAL.SQL.HorarioPHExtra _dalHorarioPHExtra;
        public DAL.SQL.HorarioPHExtra dalHorarioPHExtra {
            get { return _dalHorarioPHExtra; }
            set { _dalHorarioPHExtra = value; }
        }

        private DAL.SQL.LimiteDDsr _dalLimitesDDsr;

        public DAL.SQL.LimiteDDsr dalLimitesDDsr {
            get { return _dalLimitesDDsr; }
            set { _dalLimitesDDsr = value; }
        }

        private DAL.SQL.HorarioAItinere _dalHorarioAItinere;
        public DAL.SQL.HorarioAItinere dalHorarioAItinere {
            get { return _dalHorarioAItinere; }
            set { _dalHorarioAItinere = value; }
        }

        private DAL.SQL.Parametros _dalParametros;
        public DAL.SQL.Parametros dalParametros {
            get { return _dalParametros; }
            set { _dalParametros = value; }
        }

        private DAL.SQL.HorarioRestricao _dalHorarioRestricao;

        public DAL.SQL.HorarioRestricao dalHorarioRestricao
        {
            get { return _dalHorarioRestricao; }
            set { _dalHorarioRestricao = value; }
        }

        public string SELECTREL { get; set; }
        public string SELECTTIP { get; set; }

        public Horario(DataBase database)
        {
            db = database;
            dalHorarioDetalhe = new HorarioDetalhe(db);
            dalHorarioPHExtra = new HorarioPHExtra(db);
            dalLimitesDDsr = new LimiteDDsr(db);
            dalHorarioAItinere = new HorarioAItinere(db);
            dalParametros = new Parametros(db);
            dalHorarioRestricao = new HorarioRestricao(db);
            
            TABELA = "horario";

            SELECTPID = @"  SELECT   
                                horario.* 
                                , ISNULL((SELECT MIN(data) FROM horariodetalhe hd WHERE hd.idhorario = horario.id), 0) AS datainicial
                                , ISNULL((SELECT MAX(data) FROM horariodetalhe hd WHERE hd.idhorario = horario.id), 0) AS datafinal
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro 
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                             FROM horario 
                                LEFT JOIN parametros parms ON parms.id = horario.idparametro 
                                LEFT JOIN classificacao class ON class.id = horario.idclassificacao
                                LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                             WHERE 
                                horario.id = @id ";

            SELECTALL = @"  SELECT   
                                hor.id
                                , hor.descricao
                                , hor.codigo
                                , hor.Ativo
                                , case when hor.tipohorario = 1 then 'Normal' else 'Flexível' end AS tipohorario
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro 
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            FROM horario hor
                                LEFT JOIN HorarioDinamico hdm on hdm.id = hor.idhorariodinamico
                                LEFT JOIN parametros parms ON parms.id = hor.idparametro
                                LEFT JOIN classificacao class ON class.id = hor.idclassificacao ";

            SELECTTIP = @"  SELECT   
                                hor.id
                                , hor.descricao
                                , hor.codigo
                                , hor.Ativo
                                , (SELECT MIN(data) FROM horariodetalhe hd WHERE hd.idhorario = hor.id) AS datainicial
                                , (SELECT MAX(data) FROM horariodetalhe hd WHERE hd.idhorario = hor.id) AS datafinal 
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro     
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            FROM horario hor
                                LEFT JOIN HorarioDinamico hdm on hdm.id = hor.idhorariodinamico
                                LEFT JOIN parametros parms ON parms.id = hor.idparametro
                                LEFT JOIN classificacao class ON class.id = hor.idclassificacao
                            WHERE 
                                hor.tipohorario = @tipohorario ";


            SELECTREL = @"  SELECT   
                                hor.id
                                , hor.codigo
                                , hor.descricao AS descricaoturno
                                , hor.Ativo
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro 
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                                , (SELECT TOP 1 ISNULL(hd.entrada_1, '--:--') + ' - ' + ISNULL(hd.saida_1, '--:--') + ' | ' + ISNULL(hd.entrada_2, '--:--') + ' - ' + ISNULL(hd.saida_2, '--:--') 
                                        + ' | ' + ISNULL(hd.entrada_3, '--:--') + ' - ' + ISNULL(hd.saida_3, '--:--') + ' | ' + ISNULL(hd.entrada_4, '--:--') + ' - ' + ISNULL(hd.saida_4, '--:--')
                                        FROM horariodetalhe hd WHERE hd.idhorario = hor.id AND (hd.dia = 1 OR DATEPART(WEEKDAY, hd.data) = 2)) 
                                AS descricaoHorario
                             FROM horario hor
                                 LEFT JOIN HorarioDinamico hdm on hdm.id = hor.idhorariodinamico
                                 LEFT JOIN parametros parms ON parms.id = hor.idparametro
                                 LEFT JOIN classificacao class ON class.id = hor.idclassificacao
                             WHERE hor.id > 0 ";

            INSERT = @"  INSERT INTO horario
							(codigo, descricao, horariodescricao_1, horariodescricao_2, horariodescricao_3, horariodescricao_4, horariodescricaosai_1, 
                             horariodescricaosai_2, horariodescricaosai_3, horariodescricaosai_4, idparametro, horasnormais, somentecargahoraria, 
                             marcacargahorariamista, habilitatolerancia, conversaohoranoturna, consideraadhtrabalhadas, ordem_ent, ordenabilhetesaida, 
                             limitemin, limitemax, tipoacumulo, habilitaperiodo01, habilitaperiodo02, descontacafemanha, descontacafetarde, dias_cafe_1, 
                             dias_cafe_2, dias_cafe_3, dias_cafe_4, dias_cafe_5, dias_cafe_6, dias_cafe_7, descontafalta50, considerasabadosemana, 
                             consideradomingosemana, horaextrasab50_100, perchextrasab50_100, refeicao_01, refeicao_02, obs, descontardsr, qtdhorasdsr, 
                             diasemanadsr, limiteperdadsr, incdata, inchora, incusuario, tipohorario, intervaloautomatico, preassinaladas1, preassinaladas2,
                             preassinaladas3, SegundaPercBanco, TercaPercBanco, QuartaPercBanco, QuintaPercBanco, SextaPercBanco, SabadoPercBanco,
                             DomingoPercBanco, FeriadoPercBanco, FolgaPercBanco, MarcaSegundaPercBanco, MarcaTercaPercBanco, MarcaQuartaPercBanco, MarcaQuintaPercBanco, MarcaSextaPercBanco, 
                             MarcaSabadoPercBanco, MarcaDomingoPercBanco, MarcaFeriadoPercBanco, MarcaFolgaPercBanco, bUtilizaDDSRProporcional, LimiteHorasTrabalhadasDia, LimiteMinimoHorasAlmoco, 
                             LimiteInterjornada, desconsiderarferiado, HoristaMensalista, DescontarFeriadoDDSR, QtdHEPreClassificadas, IdClassificacao, HabilitaInItinere, DescontarAtrasoInItinere, 
                             DescontarFaltaInItinere, IdHorarioOrigem, InicioVigencia, DDSRConsideraFaltaDuranteSemana, Ativo, separaExtraNoturnaPercentual, consideraradicionalnoturnointerv, DescontoHorasDSR, DSRPorPercentual, IdHorarioDinamico, CicloSequenciaIndice, DataBaseCicloSequencia, PontoPorExcecao)
							VALUES
							(@codigo, @descricao, @horariodescricao_1, @horariodescricao_2, @horariodescricao_3, @horariodescricao_4, @horariodescricaosai_1, 
                            @horariodescricaosai_2, @horariodescricaosai_3, @horariodescricaosai_4, @idparametro, @horasnormais, @somentecargahoraria, 
                            @marcacargahorariamista, @habilitatolerancia, @conversaohoranoturna, @consideraadhtrabalhadas, @ordem_ent, @ordenabilhetesaida, 
                            @limitemin, @limitemax, @tipoacumulo, @habilitaperiodo01, @habilitaperiodo02, @descontacafemanha, @descontacafetarde, 
                            @dias_cafe_1, @dias_cafe_2, @dias_cafe_3, @dias_cafe_4, @dias_cafe_5, @dias_cafe_6, @dias_cafe_7, @descontafalta50, 
                            @considerasabadosemana, @consideradomingosemana, @horaextrasab50_100, @perchextrasab50_100, @refeicao_01, @refeicao_02, 
                            @obs, @descontardsr, @qtdhorasdsr, @diasemanadsr, @limiteperdadsr, @incdata, @inchora, @incusuario, @tipohorario, 
                            @intervaloautomatico, @preassinaladas1, @preassinaladas2, @preassinaladas3, @SegundaPercBanco, @TercaPercBanco, @QuartaPercBanco,
                            @QuintaPercBanco, @SextaPercBanco, @SabadoPercBanco, @DomingoPercBanco, @FeriadoPercBanco, @FolgaPercBanco, @MarcaSegundaPercBanco, @MarcaTercaPercBanco, 
                            @MarcaQuartaPercBanco, @MarcaQuintaPercBanco, @MarcaSextaPercBanco, @MarcaSabadoPercBanco, @MarcaDomingoPercBanco, @MarcaFeriadoPercBanco, @MarcaFolgaPercBanco, @bUtilizaDDSRProporcional, 
                            @LimiteHorasTrabalhadasDia, @LimiteMinimoHorasAlmoco, @LimiteInterjornada, @desconsiderarferiado, @HoristaMensalista, @DescontarFeriadoDDSR, @QtdHEPreClassificadas, @IdClassificacao, 
                            @HabilitaInItinere, @DescontarAtrasoInItinere, @DescontarFaltaInItinere, @IdHorarioOrigem, @InicioVigencia, @DDSRConsideraFaltaDuranteSemana, @Ativo, @separaExtraNoturnaPercentual, @consideraradicionalnoturnointerv, @DescontoHorasDSR, @DSRPorPercentual,
                            @IdHorarioDinamico, @CicloSequenciaIndice, @DataBaseCicloSequencia, @PontoPorExcecao) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE horario SET codigo = @codigo
							, descricao = @descricao
							, horariodescricao_1 = @horariodescricao_1
							, horariodescricao_2 = @horariodescricao_2
							, horariodescricao_3 = @horariodescricao_3
							, horariodescricao_4 = @horariodescricao_4
							, horariodescricaosai_1 = @horariodescricaosai_1
							, horariodescricaosai_2 = @horariodescricaosai_2
							, horariodescricaosai_3 = @horariodescricaosai_3
							, horariodescricaosai_4 = @horariodescricaosai_4
							, idparametro = @idparametro
							, horasnormais = @horasnormais
							, somentecargahoraria = @somentecargahoraria
							, marcacargahorariamista = @marcacargahorariamista
							, habilitatolerancia = @habilitatolerancia
							, conversaohoranoturna = @conversaohoranoturna
							, consideraadhtrabalhadas = @consideraadhtrabalhadas
							, ordem_ent = @ordem_ent
							, ordenabilhetesaida = @ordenabilhetesaida
							, limitemin = @limitemin
							, limitemax = @limitemax
							, tipoacumulo = @tipoacumulo
							, habilitaperiodo01 = @habilitaperiodo01
							, habilitaperiodo02 = @habilitaperiodo02
							, descontacafemanha = @descontacafemanha
							, descontacafetarde = @descontacafetarde
							, dias_cafe_1 = @dias_cafe_1
							, dias_cafe_2 = @dias_cafe_2
							, dias_cafe_3 = @dias_cafe_3
							, dias_cafe_4 = @dias_cafe_4
							, dias_cafe_5 = @dias_cafe_5
							, dias_cafe_6 = @dias_cafe_6
							, dias_cafe_7 = @dias_cafe_7
							, descontafalta50 = @descontafalta50
							, considerasabadosemana = @considerasabadosemana
							, consideradomingosemana = @consideradomingosemana
							, horaextrasab50_100 = @horaextrasab50_100
							, perchextrasab50_100 = @perchextrasab50_100
							, refeicao_01 = @refeicao_01
							, refeicao_02 = @refeicao_02
							, obs = @obs
							, descontardsr = @descontardsr
							, qtdhorasdsr = @qtdhorasdsr
							, diasemanadsr = @diasemanadsr
							, limiteperdadsr = @limiteperdadsr
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , tipohorario = @tipohorario
                            , intervaloautomatico = @intervaloautomatico 
                            , preassinaladas1 = @preassinaladas1
                            , preassinaladas2 = @preassinaladas2
                            , preassinaladas3 = @preassinaladas3
                            , SegundaPercBanco = @SegundaPercBanco
                            , TercaPercBanco = @TercaPercBanco
                            , QuartaPercBanco = @QuartaPercBanco
                            , QuintaPercBanco = @QuintaPercBanco
                            , SextaPercBanco = @SextaPercBanco
                            , SabadoPercBanco = @SabadoPercBanco
                            , DomingoPercBanco = @DomingoPercBanco
                            , FeriadoPercBanco = @FeriadoPercBanco
                            , FolgaPercBanco = @FolgaPercBanco
                            , MarcaSegundaPercBanco = @MarcaSegundaPercBanco
                            , MarcaTercaPercBanco = @MarcaTercaPercBanco
                            , MarcaQuartaPercBanco = @MarcaQuartaPercBanco
                            , MarcaQuintaPercBanco = @MarcaQuintaPercBanco
                            , MarcaSextaPercBanco = @MarcaSextaPercBanco
                            , MarcaSabadoPercBanco = @MarcaSabadoPercBanco
                            , MarcaDomingoPercBanco = @MarcaDomingoPercBanco
                            , MarcaFeriadoPercBanco = @MarcaFeriadoPercBanco
                            , MarcaFolgaPercBanco = @MarcaFolgaPercBanco
                            , bUtilizaDDSRProporcional = @bUtilizaDDSRProporcional
                            , LimiteHorasTrabalhadasDia = @LimiteHorasTrabalhadasDia
                            , LimiteMinimoHorasAlmoco = @LimiteMinimoHorasAlmoco
                            , LimiteInterjornada = @LimiteInterjornada
                            , desconsiderarferiado = @desconsiderarferiado
                            , HoristaMensalista = @HoristaMensalista
                            , DescontarFeriadoDDSR = @DescontarFeriadoDDSR
                            , QtdHEPreClassificadas = @QtdHEPreClassificadas
                            , IdClassificacao = @IdClassificacao 
                            , HabilitaInItinere = @HabilitaInItinere
                            , DescontarAtrasoInItinere = @DescontarAtrasoInItinere 
                            , DescontarFaltaInItinere = @DescontarFaltaInItinere
                            , IdHorarioOrigem = @IdHorarioOrigem
                            , InicioVigencia = @InicioVigencia
                            , DDSRConsideraFaltaDuranteSemana = @DDSRConsideraFaltaDuranteSemana
                            , Ativo = @Ativo
                            , separaExtraNoturnaPercentual = @separaExtraNoturnaPercentual
                            , consideraradicionalnoturnointerv = @consideraradicionalnoturnointerv
                            , DescontoHorasDSR = @DescontoHorasDSR
                            , DSRPorPercentual = @DSRPorPercentual
                            , IdHorarioDinamico = @IdHorarioDinamico
                            , CicloSequenciaIndice = @CicloSequenciaIndice
                            , DataBaseCicloSequencia = @DataBaseCicloSequencia
                            , PontoPorExcecao = @PontoPorExcecao
						WHERE id = @id";

            DELETE = @"  DELETE FROM horario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM horario";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
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
                dr.Dispose();
            }
        }

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
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
            DateTime aux = Convert.ToDateTime("01/01/1900");
            if (((Modelo.Horario)obj).DataInicial <= aux)
            {
                ((Modelo.Horario)obj).DataInicial = null;
            }
            if (((Modelo.Horario)obj).DataFinal <= aux)
            {
                ((Modelo.Horario)obj).DataFinal = null;
            }
            ((Modelo.Horario)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.Horario)obj).Preassinaladas1 = (dr["preassinaladas1"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas1"]));
            ((Modelo.Horario)obj).Preassinaladas2 = (dr["preassinaladas2"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas2"]));
            ((Modelo.Horario)obj).Preassinaladas3 = (dr["preassinaladas3"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas3"]));
            ((Modelo.Horario)obj).SegundaPercBanco = (dr["SegundaPercBanco"] is DBNull ? "" : Convert.ToString(dr["SegundaPercBanco"]));
            ((Modelo.Horario)obj).TercaPercBanco = (dr["TercaPercBanco"] is DBNull ? "" : Convert.ToString(dr["TercaPercBanco"]));
            ((Modelo.Horario)obj).QuartaPercBanco = (dr["QuartaPercBanco"] is DBNull ? "" : Convert.ToString(dr["QuartaPercBanco"]));
            ((Modelo.Horario)obj).QuintaPercBanco = (dr["QuintaPercBanco"] is DBNull ? "" : Convert.ToString(dr["QuintaPercBanco"]));
            ((Modelo.Horario)obj).SextaPercBanco = (dr["SextaPercBanco"] is DBNull ? "" : Convert.ToString(dr["SextaPercBanco"]));
            ((Modelo.Horario)obj).SabadoPercBanco = (dr["SabadoPercBanco"] is DBNull ? "" : Convert.ToString(dr["SabadoPercBanco"]));
            ((Modelo.Horario)obj).DomingoPercBanco = (dr["DomingoPercBanco"] is DBNull ? "" : Convert.ToString(dr["DomingoPercBanco"]));
            ((Modelo.Horario)obj).FeriadoPercBanco = (dr["FeriadoPercBanco"] is DBNull ? "" : Convert.ToString(dr["FeriadoPercBanco"]));
            ((Modelo.Horario)obj).FolgaPercBanco = (dr["FolgaPercBanco"] is DBNull ? "" : Convert.ToString(dr["FolgaPercBanco"]));
            ((Modelo.Horario)obj).MarcaSegundaPercBanco = (dr["MarcaSegundaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaSegundaPercBanco"]));
            ((Modelo.Horario)obj).MarcaTercaPercBanco = (dr["MarcaTercaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaTercaPercBanco"]));
            ((Modelo.Horario)obj).MarcaQuartaPercBanco = (dr["MarcaQuartaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaQuartaPercBanco"]));
            ((Modelo.Horario)obj).MarcaQuintaPercBanco = (dr["MarcaQuintaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaQuintaPercBanco"]));
            ((Modelo.Horario)obj).MarcaSextaPercBanco = (dr["MarcaSextaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaSextaPercBanco"]));
            ((Modelo.Horario)obj).MarcaSabadoPercBanco = (dr["MarcaSabadoPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaSabadoPercBanco"]));
            ((Modelo.Horario)obj).MarcaDomingoPercBanco = (dr["MarcaDomingoPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaDomingoPercBanco"]));
            ((Modelo.Horario)obj).MarcaFeriadoPercBanco = (dr["MarcaFeriadoPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaFeriadoPercBanco"]));
            ((Modelo.Horario)obj).MarcaFolgaPercBanco = (dr["MarcaFolgaPercBanco"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["MarcaFolgaPercBanco"]));
            ((Modelo.Horario)obj).bUtilizaDDSRProporcional = (dr["bUtilizaDDSRProporcional"] is DBNull ? false : Convert.ToBoolean(dr["bUtilizaDDSRProporcional"]));
            ((Modelo.Horario)obj).LimiteHorasTrabalhadasDia = Convert.ToString(dr["LimiteHorasTrabalhadasDia"]);
            ((Modelo.Horario)obj).LimiteMinimoHorasAlmoco = Convert.ToString(dr["LimiteMinimoHorasAlmoco"]);
            ((Modelo.Horario)obj).LimiteInterjornada = (dr["LimiteInterjornada"] is DBNull ? "" : Convert.ToString(dr["LimiteInterjornada"]));
            ((Modelo.Horario)obj).HoristaMensalista = (dr["HoristaMensalista"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["HoristaMensalista"]));
            ((Modelo.Horario)obj).DescontarFeriadoDDSR = (dr["DescontarFeriadoDDSR"] is DBNull ? false : Convert.ToBoolean(dr["DescontarFeriadoDDSR"]));
            ((Modelo.Horario)obj).DescParametro = Convert.ToString(dr["DescParametro"]);
            ((Modelo.Horario)obj).QtdHEPreClassificadas = Convert.ToString(dr["QtdHEPreClassificadas"]);
            if (!(dr["IdClassificacao"] is DBNull))
            {
                ((Modelo.Horario)obj).IdClassificacao = Convert.ToInt32(dr["IdClassificacao"]);
            }
            ((Modelo.Horario)obj).DescClassificacao = Convert.ToString(dr["DescClassificacao"]);
            ((Modelo.Horario)obj).HabilitaInItinere = (dr["HabilitaInItinere"]) is DBNull ? (Int32)0 : Convert.ToInt32(dr["HabilitaInItinere"]);
            ((Modelo.Horario)obj).DescontarAtrasoInItinere = (dr["DescontarAtrasoInItinere"]) is DBNull ? false : Convert.ToBoolean(dr["DescontarAtrasoInItinere"]);
            ((Modelo.Horario)obj).DescontarFaltaInItinere = (dr["DescontarFaltaInItinere"]) is DBNull ? false : Convert.ToBoolean(dr["DescontarFaltaInItinere"]);


            ((Modelo.Horario)obj).IdHorarioOrigem = (dr["IdHorarioOrigem"] is DBNull ? 0 : Convert.ToInt32(dr["IdHorarioOrigem"]));
            ((Modelo.Horario)obj).InicioVigencia = (dr["InicioVigencia"] is DBNull ? null : (DateTime?)dr["InicioVigencia"]);

            ((Modelo.Horario)obj).DDSRConsideraFaltaDuranteSemana = (dr["DDSRConsideraFaltaDuranteSemana"] is DBNull ? false : Convert.ToBoolean(dr["DDSRConsideraFaltaDuranteSemana"]));
            ((Modelo.Horario)obj).Ativo = (dr["Ativo"]) is DBNull ? false : Convert.ToBoolean(dr["Ativo"]);
            ((Modelo.Horario)obj).SeparaExtraNoturnaPercentual = (dr["separaExtraNoturnaPercentual"]) is DBNull ? false : Convert.ToBoolean(dr["separaExtraNoturnaPercentual"]);
            ((Modelo.Horario)obj).Descontohorasdsr = (dr["DescontoHorasDSR"]) is DBNull ? 0.18m : Convert.ToDecimal(dr["DescontoHorasDSR"]);
            ((Modelo.Horario)obj).DSRPorPercentual = (dr["DSRPorPercentual"]) is DBNull ? false : Convert.ToBoolean(dr["DSRPorPercentual"]);
            ((Modelo.Horario)obj).IdHorarioDinamico = (dr["IdHorarioDinamico"] is DBNull ? null : (int?)dr["IdHorarioDinamico"]);
            ((Modelo.Horario)obj).CicloSequenciaIndice = dr["CicloSequenciaIndice"] is DBNull ? (Int32?)null : Convert.ToInt32(dr["CicloSequenciaIndice"]);
            ((Modelo.Horario)obj).DataBaseCicloSequencia = dr["DataBaseCicloSequencia"] is DBNull ? (DateTime?)null : Convert.ToDateTime(dr["DataBaseCicloSequencia"]);
            ((Modelo.Horario)obj).HorarioDinamico = Convert.ToString(dr["DescHorarioDinamico"]);
            ((Modelo.Horario)obj).PontoPorExcecao = Convert.ToBoolean(dr["PontoPorExcecao"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@descricao", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricao_1", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricao_2", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricao_3", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricao_4", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricaosai_1", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricaosai_2", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricaosai_3", SqlDbType.VarChar),
                new SqlParameter ("@horariodescricaosai_4", SqlDbType.VarChar),
                new SqlParameter ("@idparametro", SqlDbType.Int),
                new SqlParameter ("@horasnormais", SqlDbType.TinyInt),
                new SqlParameter ("@somentecargahoraria", SqlDbType.TinyInt),
                new SqlParameter ("@marcacargahorariamista", SqlDbType.TinyInt),
                new SqlParameter ("@habilitatolerancia", SqlDbType.TinyInt),
                new SqlParameter ("@conversaohoranoturna", SqlDbType.TinyInt),
                new SqlParameter ("@consideraadhtrabalhadas", SqlDbType.TinyInt),
                new SqlParameter ("@ordem_ent", SqlDbType.TinyInt),
                new SqlParameter ("@ordenabilhetesaida", SqlDbType.TinyInt),
                new SqlParameter ("@limitemin", SqlDbType.VarChar),
                new SqlParameter ("@limitemax", SqlDbType.VarChar),
                new SqlParameter ("@tipoacumulo", SqlDbType.Int),
                new SqlParameter ("@habilitaperiodo01", SqlDbType.TinyInt),
                new SqlParameter ("@habilitaperiodo02", SqlDbType.TinyInt),
                new SqlParameter ("@descontacafemanha", SqlDbType.TinyInt),
                new SqlParameter ("@descontacafetarde", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_1", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_2", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_3", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_4", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_5", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_6", SqlDbType.TinyInt),
                new SqlParameter ("@dias_cafe_7", SqlDbType.TinyInt),
                new SqlParameter ("@descontafalta50", SqlDbType.TinyInt),
                new SqlParameter ("@considerasabadosemana", SqlDbType.TinyInt),
                new SqlParameter ("@consideradomingosemana", SqlDbType.TinyInt),
                new SqlParameter ("@horaextrasab50_100", SqlDbType.TinyInt),
                new SqlParameter ("@perchextrasab50_100", SqlDbType.TinyInt),
                new SqlParameter ("@refeicao_01", SqlDbType.VarChar),
                new SqlParameter ("@refeicao_02", SqlDbType.VarChar),
                new SqlParameter ("@obs", SqlDbType.VarChar),
                new SqlParameter ("@descontardsr", SqlDbType.TinyInt),
                new SqlParameter ("@qtdhorasdsr", SqlDbType.VarChar),
                new SqlParameter ("@diasemanadsr", SqlDbType.Int),
                new SqlParameter ("@limiteperdadsr", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@tipohorario", SqlDbType.VarChar),
                new SqlParameter ("@intervaloautomatico", SqlDbType.Int),
                new SqlParameter ("@preassinaladas1", SqlDbType.Int),
                new SqlParameter ("@preassinaladas2", SqlDbType.Int),
                new SqlParameter ("@preassinaladas3", SqlDbType.Int),
                new SqlParameter ("@MarcaSegundaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaTercaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaQuartaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaQuintaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaSextaPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaSabadoPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaDomingoPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaFeriadoPercBanco", SqlDbType.Int),
                new SqlParameter ("@MarcaFolgaPercBanco", SqlDbType.Int),
                new SqlParameter ("@SegundaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@TercaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@QuartaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@QuintaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@SextaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@SabadoPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@DomingoPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@FeriadoPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@FolgaPercBanco", SqlDbType.VarChar),
                new SqlParameter ("@bUtilizaDDSRProporcional", SqlDbType.Bit),
                new SqlParameter ("@LimiteHorasTrabalhadasDia", SqlDbType.VarChar),
                new SqlParameter ("@LimiteMinimoHorasAlmoco", SqlDbType.VarChar),
                new SqlParameter ("@LimiteInterjornada", SqlDbType.VarChar),
                new SqlParameter ("@desconsiderarferiado", SqlDbType.TinyInt),
                new SqlParameter ("@HoristaMensalista", SqlDbType.TinyInt),
                new SqlParameter ("@DescontarFeriadoDDSR", SqlDbType.Bit),
                new SqlParameter ("@DescParametro", SqlDbType.VarChar),
                new SqlParameter ("@QtdHEPreClassificadas", SqlDbType.VarChar),
                new SqlParameter ("@IdClassificacao", SqlDbType.Int),
                new SqlParameter ("@DescClassificacao", SqlDbType.VarChar),
                new SqlParameter ("@HabilitaInItinere", SqlDbType.Int),
                new SqlParameter ("@DescontarAtrasoInItinere", SqlDbType.Bit),
                new SqlParameter ("@DescontarFaltaInItinere", SqlDbType.Bit),
                new SqlParameter ("@IdHorarioOrigem", SqlDbType.Int),
                new SqlParameter ("@InicioVigencia", SqlDbType.DateTime),
                new SqlParameter ("@DDSRConsideraFaltaDuranteSemana", SqlDbType.Bit),
                new SqlParameter ("@Ativo", SqlDbType.Bit),
                new SqlParameter ("@SeparaExtraNoturnaPercentual", SqlDbType.Bit),
                 new SqlParameter ("@consideraradicionalnoturnointerv", SqlDbType.Bit),
                new SqlParameter ("@DescontoHorasDSR", SqlDbType.Decimal),
                new SqlParameter ("@DSRPorPercentual", SqlDbType.Bit),
                new SqlParameter ("@IdHorarioDinamico", SqlDbType.Int),
                new SqlParameter ("@CicloSequenciaIndice", SqlDbType.Int),
                new SqlParameter ("@DataBaseCicloSequencia", SqlDbType.DateTime),
                new SqlParameter ("@PontoPorExcecao", SqlDbType.Bit)
            };
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
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
            parms[57].Value = ((Modelo.Horario)obj).MarcaSegundaPercBanco;
            parms[58].Value = ((Modelo.Horario)obj).MarcaTercaPercBanco;
            parms[59].Value = ((Modelo.Horario)obj).MarcaQuartaPercBanco;
            parms[60].Value = ((Modelo.Horario)obj).MarcaQuintaPercBanco;
            parms[61].Value = ((Modelo.Horario)obj).MarcaSextaPercBanco;
            parms[62].Value = ((Modelo.Horario)obj).MarcaSabadoPercBanco;
            parms[63].Value = ((Modelo.Horario)obj).MarcaDomingoPercBanco;
            parms[64].Value = ((Modelo.Horario)obj).MarcaFeriadoPercBanco;
            parms[65].Value = ((Modelo.Horario)obj).MarcaFolgaPercBanco;
            parms[66].Value = ((Modelo.Horario)obj).SegundaPercBanco;
            parms[67].Value = ((Modelo.Horario)obj).TercaPercBanco;
            parms[68].Value = ((Modelo.Horario)obj).QuartaPercBanco;
            parms[69].Value = ((Modelo.Horario)obj).QuintaPercBanco;
            parms[70].Value = ((Modelo.Horario)obj).SextaPercBanco;
            parms[71].Value = ((Modelo.Horario)obj).SabadoPercBanco;
            parms[72].Value = ((Modelo.Horario)obj).DomingoPercBanco;
            parms[73].Value = ((Modelo.Horario)obj).FeriadoPercBanco;
            parms[74].Value = ((Modelo.Horario)obj).FolgaPercBanco;
            parms[75].Value = ((Modelo.Horario)obj).bUtilizaDDSRProporcional;
            parms[76].Value = ((Modelo.Horario)obj).LimiteHorasTrabalhadasDia;
            parms[77].Value = ((Modelo.Horario)obj).LimiteMinimoHorasAlmoco;
            parms[78].Value = ((Modelo.Horario)obj).LimiteInterjornada;
            parms[79].Value = ((Modelo.Horario)obj).DesconsiderarFeriado;
            parms[80].Value = ((Modelo.Horario)obj).HoristaMensalista;
            parms[81].Value = ((Modelo.Horario)obj).DescontarFeriadoDDSR;
            parms[82].Value = ((Modelo.Horario)obj).DescParametro;
            parms[83].Value = ((Modelo.Horario)obj).QtdHEPreClassificadas;
            parms[84].Value = ((Modelo.Horario)obj).IdClassificacao;
            parms[85].Value = ((Modelo.Horario)obj).DescClassificacao;
            parms[86].Value = ((Modelo.Horario)obj).HabilitaInItinere;
            parms[87].Value = ((Modelo.Horario)obj).DescontarAtrasoInItinere;
            parms[88].Value = ((Modelo.Horario)obj).DescontarFaltaInItinere;
            parms[89].Value = ((Modelo.Horario)obj).IdHorarioOrigem;
            parms[90].Value = ((Modelo.Horario)obj).InicioVigencia;
            parms[91].Value = ((Modelo.Horario)obj).DDSRConsideraFaltaDuranteSemana;
            parms[92].Value = ((Modelo.Horario)obj).Ativo;
            parms[93].Value = ((Modelo.Horario)obj).SeparaExtraNoturnaPercentual;
            parms[94].Value = ((Modelo.Horario)obj).ConsiderarAdicionalNoturnoInterv;
            parms[95].Value = ((Modelo.Horario)obj).Descontohorasdsr;
            parms[96].Value = ((Modelo.Horario)obj).DSRPorPercentual;
            parms[97].Value = ((Modelo.Horario)obj).IdHorarioDinamico;
            parms[98].Value = ((Modelo.Horario)obj).CicloSequenciaIndice;
            parms[99].Value = ((Modelo.Horario)obj).DataBaseCicloSequencia;
            parms[100].Value = ((Modelo.Horario)obj).PontoPorExcecao;
        }

        public Modelo.Horario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Horario objHorario = new Modelo.Horario();
            try
            {
                SetInstance(dr, objHorario);

                objHorario.HorariosDetalhe = new Modelo.HorarioDetalhe[7];
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

                objHorario.LimitesDDsrProporcionais = dalLimitesDDsr.GetAllListPorHorario(objHorario.Id);
                foreach (var item in objHorario.LimitesDDsrProporcionais)
                {
                    item.Acao = Modelo.Acao.Alterar;
                }

                objHorario.HorariosPHExtra = new Modelo.HorarioPHExtra[10];
                List<Modelo.HorarioPHExtra> listaPHE = dalHorarioPHExtra.LoadPorHorario(objHorario.Id);
                foreach (Modelo.HorarioPHExtra hd in listaPHE)
                {
                    if (hd.Codigo < 10)
                        objHorario.HorariosPHExtra[hd.Codigo] = hd;
                }

                objHorario.HorariosAItinere = new Modelo.HorarioAItinere[9];

                List<Modelo.HorarioAItinere> listaAI = dalHorarioAItinere.LoadPorHorario(objHorario.Id);
                if (listaAI != null)
                {
                    objHorario.HorariosAItinere = listaAI.ToArray();
                    objHorario.LHorariosAItinere = listaAI;
                }

                objHorario.HorarioRestricao = dalHorarioRestricao.GetAllListByHorarios(new List<int>() { objHorario.Id });
                foreach (var item in objHorario.HorarioRestricao)
                {
                    item.Acao = Modelo.Acao.Alterar;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorario;
        }

        public List<Modelo.Horario> LoadObjectAllChildren(int idHorarioDinamico)
        {

            List<Modelo.Horario> listaHorario = GetHorarioByHorarioDinamico(idHorarioDinamico);

            //Carrega e vincula os Parametros de hora extra e vincula aos seus horários

            foreach (var horario in listaHorario)
            {
                horario.LHorariosPHExtra = dalHorarioPHExtra.LoadPorHorario(horario.Id);
                horario.LimitesDDsrProporcionais = dalLimitesDDsr.GetAllListPorHorario(horario.Id);
            }

            return listaHorario;
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
        private int CountCodigo(SqlTransaction trans, string tabela, string campo, int valor, int id, int pTipoHorario)
        {
            //CRNC - 07/01/2010
            StringBuilder str = new StringBuilder("SELECT COUNT(");
            str.Append(campo);
            str.Append(") AS qt FROM ");
            str.Append(tabela);
            str.Append(" WHERE " + tabela + "." + campo);
            str.Append(" = @parametro");
            str.Append(" AND tipohorario = @tipohorario");
            str.Append(" AND ((ISNULL(IdHorarioOrigem,0) NOT IN (SELECT hi.IdHorarioOrigem FROM dbo.horario hi WHERE hi.id = @id) AND ISNULL(IdHorarioOrigem,0) > 0) OR ISNULL(IdHorarioOrigem,0) = 0) ");
            str.Append(" AND id NOT IN (SELECT hi.IdHorarioOrigem FROM dbo.horario hi WHERE hi.id = @id) ");

            if (id > 0)
            {
                str.Append(" AND id <> @id");
            }

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@parametro", SqlDbType.Int, 4),
                new SqlParameter("@id", SqlDbType.Int),
                new SqlParameter("@tipohorario", SqlDbType.Int)
            };
            parms[0].Value = valor;
            parms[1].Value = id;
            parms[2].Value = pTipoHorario;

            return (int)TransactDbOps.ExecuteScalar(trans, CommandType.Text, str.ToString(), parms);
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.Horario)obj).LimitesDDsrProporcionais != null)
            {
                foreach (var item in ((Modelo.Horario)obj).LimitesDDsrProporcionais)
                {
                    dalLimitesDDsr.Excluir(item);
                }
            }
            if (((Modelo.Horario)obj).HorarioRestricao != null)
            {
                ((Modelo.Horario)obj).HorarioRestricao.ToList().ForEach(f => f.Excluir = true);
                HorarioRestricaoSalvar(trans, obj); 
            }
            base.ExcluirAux(trans, obj);
        }

        public List<Modelo.Horario> getPorParametro(int pIdParametro)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idparametro", SqlDbType.Int) };
            parms[0].Value = pIdParametro;

            string comando = @" SELECT 
                                    horario.*
                                    , (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datainicial
                                    , (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datafinal
                                    , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
                                    , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                    , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                                FROM horario 
                                    LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                    LEFT JOIN parametros parms ON parms.id = horario.idparametro
                                    LEFT JOIN classificacao class on class.id = horario.idclassificacao
                                WHERE 
                                    horario.idparametro = @idparametro ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (CountCodigo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, 0, ((Modelo.Horario)obj).TipoHorario) > 0) ////CRNC - 07/01/2010
            {
                parms[1].Value = TransactDbOps.MaxCodigo(trans, MAXCOD);
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);
            if (((Modelo.Horario)obj).LimitesDDsrProporcionais != null)
            {
                foreach (var item in ((Modelo.Horario)obj).LimitesDDsrProporcionais)
                {
                    item.Acao = Modelo.Acao.Incluir;
                }
            }
            AuxManutencao(trans, obj);

            ((Modelo.Horario)obj).HorariosPHExtra.ToList().ForEach(f => { f.Idhorario = ((Modelo.Horario)obj).Id; });
            dalHorarioPHExtra.InserirRegistros(((Modelo.Horario)obj).HorariosPHExtra.ToList(), trans);

            ((Modelo.Horario)obj).HorariosAItinere.ToList().ForEach(f => { f.Idhorario = ((Modelo.Horario)obj).Id; });
            dalHorarioAItinere.InserirRegistros(((Modelo.Horario)obj).HorariosAItinere.ToList(), trans);

            cmd.Parameters.Clear();
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
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


                ((Modelo.Horario)obj).HorariosFlexiveis.ForEach(f => f.Idhorario = ((Modelo.Horario)obj).Id);
                ((Modelo.Horario)obj).HorariosFlexiveis.Where(w => w.Idjornada == 0).ToList().ForEach(f => f.Idjornada = null);

                var listaHorarioFlexivelAcao = ((Modelo.Horario)obj).HorariosFlexiveis.GroupBy(h => h.Acao).ToList();
                var listaIncluir = listaHorarioFlexivelAcao.Where(x => x.Key == Modelo.Acao.Incluir).SelectMany(s => s).ToList();
                var listaAlterar = listaHorarioFlexivelAcao.Where(x => x.Key == Modelo.Acao.Alterar).SelectMany(s => s).ToList();
                var listaExcluir = listaHorarioFlexivelAcao.Where(x => x.Key == Modelo.Acao.Excluir).SelectMany(s => s).ToList();

                if (listaIncluir.Count > 0)
                    dalHorarioDetalhe.InserirRegistros<Modelo.HorarioDetalhe>(listaIncluir, trans);

                if (listaAlterar.Count > 0)
                    dalHorarioDetalhe.AtualizarRegistros<Modelo.HorarioDetalhe>(listaAlterar, trans);

                foreach (Modelo.HorarioDetalhe hd in ((Modelo.Horario)obj).HorariosFlexiveis.Where(w => w.Acao == Modelo.Acao.Excluir))
                {
                    hd.Idhorario = ((Modelo.Horario)obj).Id;
                    switch (hd.Acao)
                    {
                        case Modelo.Acao.Excluir:
                            dalHorarioDetalhe.Excluir(trans, hd);
                            break;
                    }
                }
            }

            if (((Modelo.Horario)obj).LimitesDDsrProporcionais != null)
            {
                if (((Modelo.Horario)obj).LimitesDDsrProporcionais.Count > 0)
                {
                    Modelo.Horario o = ((Modelo.Horario)obj);
                    foreach (var item in o.LimitesDDsrProporcionais)
                    {
                        if (!o.bUtilizaDDSRProporcional)
                        {
                            item.Acao = Modelo.Acao.Excluir;
                            item.Delete = true;
                        }
                        if (item.Acao == 0)
                        {
                            item.Acao = Modelo.Acao.Incluir;
                        }
                        if (item.Delete && item.Id > 0)
                        {
                            item.Acao = Modelo.Acao.Excluir;
                        }
                        if (item.Delete && item.Id == 0)
                        {
                            continue;
                        }
                        item.IdHorario = o.Id;
                        switch (item.Acao)
                        {
                            case Modelo.Acao.Incluir:
                                item.Codigo = TransactDbOps.MaxCodigo(trans, "SELECT COALESCE(MAX(codigo),0) AS codigo FROM limiteddsr");
                                dalLimitesDDsr.Incluir(trans, item);
                                break;
                            case Modelo.Acao.Alterar:
                                dalLimitesDDsr.Alterar(trans, item);
                                break;
                            case Modelo.Acao.Excluir:
                                dalLimitesDDsr.Excluir(trans, item);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            HorarioRestricaoSalvar(trans, obj);
        }

        private void HorarioRestricaoSalvar(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            if (((Modelo.Horario)obj).HorarioRestricao != null)
            {
                IList<Modelo.HorarioRestricao> restricoes = ((Modelo.Horario)obj).HorarioRestricao;
                restricoes.ToList().ForEach(f => f.IdHorario = ((Modelo.Horario)obj).Id);
                if (restricoes != null && restricoes.Count > 0)
                {
                    var despresaDuplicados = restricoes.GroupBy(g => new { g.Acao, g.Excluir, g.IdContrato, g.IdEmpresa, g.IdHorario, g.TipoRestricao });
                    List<Modelo.HorarioRestricao> registrosSalvar = new List<Modelo.HorarioRestricao>();
                    foreach (var grupos in despresaDuplicados)
                    {
                        Modelo.HorarioRestricao horarioRestricaoOperacao = grupos.FirstOrDefault();
                        registrosSalvar.Add(horarioRestricaoOperacao);
                    }
                    var RegistrosExcluir = registrosSalvar.Where(w => w.Excluir && w.Id > 0).ToList();
                    if (RegistrosExcluir.Count() > 0)
                    {
                        List<Modelo.ModeloBase> RegistrosExcluirBase = RegistrosExcluir.ConvertAll(x => (Modelo.ModeloBase)x);
                        dalHorarioRestricao.ExcluirRegistros(RegistrosExcluirBase, trans);
                    }

                    var RegistrosIncluir = registrosSalvar.Where(w => !w.Excluir && w.Id == 0).ToList();
                    if (RegistrosIncluir.Count() > 0)
                    {
                        dalHorarioRestricao.InserirRegistros(RegistrosIncluir, trans);
                    }
                }
            }
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            Modelo.Horario _horario = ((Modelo.Horario)obj);
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (CountCodigo(trans, TABELA, "codigo", ((Modelo.ModeloBase)obj).Codigo, ((Modelo.ModeloBase)obj).Id, ((Modelo.Horario)obj).TipoHorario) > 0) //CRNC - 07/01/2010
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            Modelo.Horario horarioAntigo = LoadObject(obj.Id);
            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);
            AuxManutencao(trans, obj);

            if (horarioAntigo.Idparametro != ((Modelo.Horario)obj).Idparametro)
            {
                DateTime datainicial;
                if (DateTime.Now.Month == 1)
                {
                    datainicial = new DateTime(Convert.ToInt16(DateTime.Now.AddYears(-1).Year), Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                }
                else if (DateTime.Now.Month == 12)
                {
                    datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                }
                else
                {
                    datainicial = new DateTime(DateTime.Now.Year, Convert.ToInt16(DateTime.Now.AddMonths(-1).Month), 1);
                }

                Modelo.Parametros paramAntigo = dalParametros.LoadObject(horarioAntigo.Idparametro);
                Modelo.Parametros param = dalParametros.LoadObject(((Modelo.Horario)obj).Idparametro);
                if (paramAntigo.TipoHoraExtraFalta != param.TipoHoraExtraFalta)
                {
                    SqlParameter[] parms2 = new SqlParameter[2]
                                {
                                      new SqlParameter("@idhorario", SqlDbType.Int)
                                    , new SqlParameter("@data", SqlDbType.DateTime)
                                };
                    parms2[0].Value = ((Modelo.Horario)obj).Id;
                    parms2[1].Value = datainicial;

                    string update = @" UPDATE dbo.marcacao
                                   SET tipohoraextrafalta = p.tipohoraextrafalta
                                  FROM dbo.marcacao m
                                 INNER JOIN dbo.horario h ON m.idhorario = h.id AND h.id = @idhorario AND m.data >= @data
                                 INNER JOIN dbo.parametros p ON p.id = h.idparametro ";

                    SqlCommand cmd2 = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, update, false, parms2);
                    cmd2.Parameters.Clear();
                }
            }

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

            _horario.LHorariosAItinere.ToList().ForEach((l) =>
            {
                l.Idhorario = _horario.Id;

                if (l.Id == 0)
                {
                    dalHorarioAItinere.Incluir(trans, l);
                }
                else
                {
                    dalHorarioAItinere.Alterar(trans, l);
                }
            });



            //for (int i = 0; i < ((Modelo.Horario)obj).HorariosAItinere.Length; i++)
            //{
            //    if (((Modelo.Horario)obj).HorariosAItinere[i] != null)
            //    {
            //        //pq o idhorario é zero aqui?
            //        ((Modelo.Horario)obj).HorariosAItinere[i].Idhorario = ((Modelo.Horario)obj).Id;

            //        if (((Modelo.Horario)obj).HorariosAItinere[i].Id == 0)
            //        {
            //            ((Modelo.Horario)obj).HorariosAItinere[i].Codigo = i;
            //            dalHorarioAItinere.Incluir(trans, ((Modelo.Horario)obj).HorariosAItinere[i]);
            //        }
            //        else
            //        {
            //            dalHorarioAItinere.Alterar(trans, ((Modelo.Horario)obj).HorariosAItinere[i]);
            //        }
            //    }
            //}

            cmd.Parameters.Clear();
        }

        public int? GetIdPorCodigo(int Cod , int Tipo, bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string consulta = "select top 1 id from horario where codigo = " + Cod + " and tipohorario =  " + Tipo;
            if (validaPermissaoUser)
            {
                consulta += AddPermissaoUsuario(" horario.id");
            }
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, consulta, parms));

            return Id;
        }

        public DataTable GetPorDescricao(string pHorarios)
        {
            SqlParameter[] parms = new SqlParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND hor.id IN " + pHorarios;
            aux += " ORDER BY descricaoturno";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetHorarioNormal()
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@tipohorario", SqlDbType.Int) };
            parms[0].Value = 1;

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTTIP + " ORDER BY hor.descricao ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public DataTable GetHorarioMovel()
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@tipohorario", SqlDbType.Int) };
            parms[0].Value = 2;

            DataTable dt = new DataTable();

            string aux;

            aux = SELECTTIP + " ORDER BY hor.descricao ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public List<Modelo.Horario> GetHorarioNormalMovelList(int tipoHorario, bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@tipohorario", SqlDbType.Int) };
            parms[0].Value = tipoHorario;

            string aux;

            aux = @"SELECT   
                        hor.*
                        , (SELECT MIN(data) FROM horariodetalhe hd WHERE hd.idhorario = hor.id) AS datainicial
                        , (SELECT MAX(data) FROM horariodetalhe hd WHERE hd.idhorario = hor.id) AS datafinal
                        , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
                        , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                        , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                    FROM horario hor
                        LEFT JOIN HorarioDinamico hdm on hdm.id = hor.idhorariodinamico
                        LEFT JOIN parametros parms ON parms.id = hor.idparametro
                        LEFT JOIN classificacao class on class.id = hor.idclassificacao
                    WHERE 
                        hor.tipohorario = @tipohorario
                        and hor.idhorariodinamico is null ";
            if (validaPermissaoUser)
            {
                aux += AddPermissaoUsuario("hor.id");
            }
            aux += @" ORDER BY hor.descricao ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public DataTable GetHorarioTipo(int pTipoHorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@tipohorario", SqlDbType.Int, 4) };
            parms[0].Value = pTipoHorario;

            string aux = @" SELECT   
                                hor.id
                                , hor.descricao
                                , hor.codigo
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro 
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            FROM horario hor
                                LEFT JOIN HorarioDinamico hdm on hdm.id = hor.idhorariodinamico
                                LEFT JOIN parametros parms ON parms.id = hor.idparametro
                                LEFT JOIN classificacao class on class.id = hor.idclassificacao
                            WHERE 
                                hor.tipohorario = @tipohorario ";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<Modelo.Horario> getTodosHorariosDaEmpresa(int pIdEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idempresa", SqlDbType.Int) };
            parms[0].Value = pIdEmpresa;

            string comando = @" SELECT  DISTINCT 
                                    horario.*
                                    , (SELECT MIN(data) FROM horariodetalhe hd WHERE hd.idhorario = horario.id) AS datainicial
                                    , (SELECT MAX(data) FROM horariodetalhe hd WHERE hd.idhorario = horario.id) AS datafinal
                                    , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro   
                                    , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                    , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                                FROM horario
                                    LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                    LEFT JOIN parametros parms ON parms.id = horario.idparametro
                                    LEFT JOIN classificacao class on class.id = horario.idclassificacao
                                    INNER JOIN funcionario ON funcionario.idhorario = horario.id                        
                                WHERE 
                                    funcionario.idempresa = @idempresa ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public List<Modelo.Horario> GetParaIncluirMarcacao(Hashtable ids, bool carregaHorarioDetalhe)
        {
            int[] arIDs = new int[ids.Count];
            ids.Keys.CopyTo(arIDs, 0);
            var strIDs = string.Join(",", arIDs);
            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            if (ids.Count > 0)
            {
                var sql = string.Format(@"  SELECT 
                                                horario.* 
                                                , (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datainicial
                                                , (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datafinal
                                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
                                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                                , convert(varchar, hdm.codigo) + ' | ' + hdm.descricao as DescHorarioDinamico
                                            FROM horario
                                                LEFT JOIN parametros parms ON parms.id = horario.idparametro
                                                LEFT JOIN classificacao class ON class.id = horario.idclassificacao
                                                LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                            WHERE 
                                                horario.id IN ({0}) ", strIDs);

                SqlParameter[] parms = new SqlParameter[0];

                List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
                if (carregaHorarioDetalhe)
                {
                    listaHorariosDetalhe = dalHorarioDetalhe.LoadPorHorario(ids);
                }

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
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
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }

            return listaHorario;
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais)
        {
            SqlParameter[] parms = new SqlParameter[0];
            string cmd = @" SELECT 
                                horario.* 
                                , (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datainicial 
                                , (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datafinal 
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro  
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            FROM horario
                                LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                LEFT JOIN parametros parms ON parms.id = horario.idparametro 
                                LEFT JOIN classificacao class ON class.id = horario.idclassificacao ";

            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.GetAllList();
            }

            List<Modelo.HorarioPHExtra> listaPercentuais = null;
            if (carregaPercentuais)
            {
                listaPercentuais = dalHorarioPHExtra.GetAllList();
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
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
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public List<Modelo.Proxy.PxyGridHorarioFlexivel> GetAllGrid(int tipoHorario)
        {
            List<Modelo.Proxy.PxyGridHorarioFlexivel> lista = new List<Modelo.Proxy.PxyGridHorarioFlexivel>();

            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@tipohorario", SqlDbType.Int) };

            parms[0].Value = tipoHorario;

            string aux;

            aux = @"SELECT 
                        h.id
	                    , h.codigo
                        , h.descricao
                        , h.ativo
                        , h.DSRPorPercentual
	                    , (SELECT MIN(data) FROM horariodetalhe hd WHERE hd.idhorario = h.id) AS Datainicial
                        , (SELECT MAX(data) FROM horariodetalhe hd WHERE hd.idhorario = h.id) AS Datafinal
	                    , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
	                    , convert(varchar,h.limitemin) as LimiteMin
	                    , convert(varchar,h.limitemax) as LimiteMax
	                    , CASE WHEN h.conversaohoranoturna = 1 THEN 'Sim' ELSE 'Não' END conversaohoranoturna
	                    , CASE WHEN h.consideraadhtrabalhadas = 1 THEN 'Sim' ELSE 'Não' END Consideraadhtrabalhadas
	                    , CASE WHEN h.intervaloautomatico = 1 THEN 'Sim' ELSE 'Não' END intervaloautomatico
	                    , CASE WHEN h.horasnormais = 1 THEN 'Sim' ELSE 'Não' END horasnormais
	                    , CASE WHEN h.descontardsr = 1 THEN 'Sim' ELSE 'Não' END descontardsr
	                    , CASE WHEN h.bUtilizaDDSRProporcional = 1 THEN 'Sim' ELSE 'Não' END bUtilizaDDSRProporcional
	                    , CASE WHEN h.DescontarFeriadoDDSR = 1 THEN 'Sim' ELSE 'Não' END DescontarFeriadoDDSR
                        , CASE WHEN h.PontoPorExcecao = 1 THEN 'Sim' ELSE 'Não' END PontoPorExcecao
                    FROM horario h
	                    LEFT JOIN parametros parms ON parms.id = h.idparametro
	                WHERE 
                        h.tipohorario = @tipohorario 
                        and h.idhorariodinamico is null " + AddPermissaoUsuario("h.id");

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridHorarioFlexivel>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridHorarioFlexivel>>(dr);
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
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipohorario, bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@tipohorario", SqlDbType.Int) };
            parms[0].Value = tipohorario;
            string cmd = @" SELECT 
                                horario.*
                                , (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datainicial
                                , (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datafinal
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            FROM horario
                                LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                LEFT JOIN parametros parms ON parms.id = horario.idparametro   
                                LEFT JOIN classificacao class ON class.id = horario.idclassificacao 
                            WHERE 
                                (horario.tipohorario = @tipohorario or @tipohorario = 0) ";

            if (validaPermissaoUser)
            {
                cmd += AddPermissaoUsuario("horario.id");
            }

            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.GetAllList();
            }

            List<Modelo.HorarioPHExtra> listaPercentuais = null;
            if (carregaPercentuais)
            {
                listaPercentuais = dalHorarioPHExtra.GetAllList();
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
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
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public List<Modelo.Horario> GetAllListOrigem(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipohorario)
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@tipohorario", SqlDbType.Int) };
            parms[0].Value = tipohorario;
            string cmd = @" SELECT 
                                horario.*
                                , (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datainicial
                                , (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datafinal
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                             FROM horario
                                LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                LEFT JOIN parametros parms ON parms.id = horario.idparametro   
                                LEFT JOIN classificacao class ON class.id = horario.idclassificacao 
                            WHERE  
                                ((
                                    (ISNULL(horario.IdHorarioOrigem,0) > 0) AND horario.id = (SELECT max(id) FROM horario hv WHERE hv.IdHorarioOrigem = horario.IdHorarioOrigem)) 
                                    OR (ISNULL(horario.IdHorarioOrigem,0) = 0 AND NOT EXISTS(SELECT * FROM horario hv WHERE hv.IdHorarioOrigem = horario.id )
                                )) ";

            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.GetAllList();
            }

            List<Modelo.HorarioPHExtra> listaPercentuais = null;
            if (carregaPercentuais)
            {
                listaPercentuais = dalHorarioPHExtra.GetAllList();
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
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
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public List<Modelo.Horario> GetAllListOrigem(bool carregaHorarioDetalhe, bool carregaPercentuais)
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@tipohorario", SqlDbType.Int) };
            string cmd = @" SELECT 
                                horario.*
                                , (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datainicial
                                , (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datafinal
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                             FROM horario
                                LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                LEFT JOIN parametros parms ON parms.id = horario.idparametro   
                                LEFT JOIN classificacao class ON class.id = horario.idclassificacao 
                            WHERE  
                                ((
                                    (ISNULL(horario.IdHorarioOrigem,0) > 0) AND horario.id = (SELECT max(id) FROM horario hv WHERE hv.IdHorarioOrigem = horario.IdHorarioOrigem)) 
                                    OR (ISNULL(horario.IdHorarioOrigem,0) = 0 AND NOT EXISTS(SELECT * FROM horario hv WHERE hv.IdHorarioOrigem = horario.id )
                                )) ";

            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.GetAllList();
            }

            List<Modelo.HorarioPHExtra> listaPercentuais = null;
            if (carregaPercentuais)
            {
                listaPercentuais = dalHorarioPHExtra.GetAllList();
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
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
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipo, string pIds)
        {
            SqlParameter[] parms = new SqlParameter[0];
            string cmd = @" SELECT DISTINCT 
                                funcionario.idhorario
                                , horario.* 
                                , (SELECT MIN(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datainicial
                                , (SELECT MAX(data) FROM horariodetalhe  WHERE horariodetalhe.idhorario = horario.id) AS datafinal
                                , convert(varchar,parms.codigo) +' | '+ parms.descricao as DescParametro
                                , convert(varchar,class.codigo) +' | '+ class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            FROM funcionario
                                INNER JOIN horario ON horario.id = funcionario.idhorario
                                LEFT JOIN HorarioDinamico hdm on hdm.id = horario.idhorariodinamico
                                INNER JOIN empresa ON empresa.id = funcionario.idempresa
                                LEFT JOIN parametros parms ON parms.id = horario.idparametro
                                LEFT JOIN classificacao class ON class.id = horario.idclassificacao
                                INNER JOIN departamento ON departamento.id = funcionario.iddepartamento ";

            switch (tipo)
            {
                case 0: cmd += " WHERE empresa.id IN " + pIds; break;
                case 1: cmd += " WHERE departamento.id IN " + pIds; break;
                case 2: cmd += " WHERE funcionario.id IN " + pIds; break;
            }

            List<Modelo.HorarioDetalhe> listaHorariosDetalhe = null;
            if (carregaHorarioDetalhe)
            {
                listaHorariosDetalhe = dalHorarioDetalhe.GetAllList();
            }

            List<Modelo.HorarioPHExtra> listaPercentuais = null;
            if (carregaPercentuais)
            {
                listaPercentuais = dalHorarioPHExtra.GetAllList();
            }

            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);
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
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }

        public List<int> GetIds()
        {
            List<int> lista = new List<int>();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT id FROM horario", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public int MinIdHorarioNormal()
        {
            int Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select isnull(min(id),0) from horario where tipohorario = 1", new SqlParameter[] { }));

            return Id;
        }

        public Hashtable GetHashCodigoId()
        {
            Hashtable lista = new Hashtable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT codigo, id FROM horario", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashCodigoIdNormal()
        {
            Hashtable lista = new Hashtable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT codigo, id FROM horario WHERE tipohorario = 1", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashCodigoIdFlexivel()
        {
            Hashtable lista = new Hashtable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT codigo, id FROM horario WHERE tipohorario = 2", new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", ids);

            string sql = @" select top(3) 
                                f.id
                                , f.codigo
                                , f.dataFechamento
                                , f.descricao
                                , f.observacao
                            from horario h
                                 inner join marcacao m on m.idhorario = h.id and m.idFechamentoPonto is not null
                                 inner join FechamentoPonto f on m.idFechamentoPonto = f.id
                            where 
                                h.id in (select * from f_clausulain(@ids))
                            Group by 
                                f.id, 
                                f.codigo, 
                                f.dataFechamento, 
                                f.descricao, 
                                f.observacao
                            order by 
                                f.dataFechamento desc";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.FechamentoPonto> lista = new List<Modelo.FechamentoPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.FechamentoPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.FechamentoPonto>>(dr);
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
                dr.Dispose();
            }
            return lista;
        }



        public List<Modelo.Proxy.pxyHistoricoAlteracaoHorario> GetHistoricoAlteracaoHorario(int id)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = String.Join(",", id);

            string sql = @" SELECT * FROM (   
                                SELECT    
                                    h.id IdHorario 
                                    , h.IdHorarioOrigem 
                                    , h.InicioVigencia AS InicioVigencia 
                                    , CONVERT(VARCHAR, h.InicioVigencia, 103) AS InicioVigenciaStr 
                                    , h.incusuario 
                                    , h.inchora AS IncHora 
                                    , CONVERT(VARCHAR, h.inchora, 103) AS IncHoraStr 
                                    , h.codigo AS CodigoHorario 
                                    , h.descricao AS DescricaoHorario 
                                    , p.descricao AS DescricaoParametro 
                                    , h.limitemin AS HorasMin 
                                    , h.limitemax AS HorasMax 
                                    , h.conversaohoranoturna AS CalcAdicionalNoturno 
                                    , h.consideraadhtrabalhadas AS ConversaoAdnoturno 
                                    , h.intervaloautomatico AS IntervaloAutomatico 
                                    , CASE WHEN h.horasnormais = 0 THEN 'Mista' ELSE 'Normal' END AS CargaHoraria 
                                    , h.descontardsr AS DescontarDSR 
                                    , h.bUtilizaDDSRProporcional AS DescontarDSRProp 
                                    , h.diasemanadsr AS DiaSemanaDSR
                                FROM  horario h
                                    INNER JOIN dbo.parametros p ON p.id = h.idparametro
                                WHERE   
                                    IdHorarioOrigem = @id  or h.id = @id
                        ) t";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.pxyHistoricoAlteracaoHorario> lista = new List<Modelo.Proxy.pxyHistoricoAlteracaoHorario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyHistoricoAlteracaoHorario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyHistoricoAlteracaoHorario>>(dr);
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
                dr.Dispose();
            }
            return lista;
        }

        public Modelo.Horario GetHorEntradaSaidaFunc(int idhorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idhorario", SqlDbType.Int) };
            parms[0].Value = idhorario;

            string aux = @" SELECT 
                                hor.*
                                , (SELECT MIN(data) FROM dbo.horariodetalhe WHERE hord.idhorario = hor.id) AS datainicial
                                , (SELECT MAX(data) FROM dbo.horariodetalhe WHERE hord.idhorario = hor.id) AS datafinal
                                , convert(varchar,parms.codigo)+' | '+parms.descricao as DescParametro
                                , convert(varchar,class.codigo)+' | '+class.descricao as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                                , hord.dia
                                , hord.entrada_1 AS PrimeiraEntrada
                                , hord.saida_1 AS PrimeiraSaida
                                , hord.entrada_2 AS SegundaEntrada
                                , hord.saida_2 AS SegundaSaida
                                , hord.entrada_3 AS TerceiraEntrada
                                , hord.saida_3 AS TerceiraSaida
                                , hord.entrada_4 AS QuartaEntrada
                                , hord.saida_4 AS QuartaSaida
                            FROM dbo.horariodetalhe hord
                                INNER JOIN dbo.horario hor
                                LEFT JOIN HorarioDinamico hdm on hdm.id = hor.idhorariodinamico    
                                LEFT JOIN parametros parms ON parms.id = hor.idparametro 
                                LEFT JOIN classificacao class on class.id = hor.idclassificacao ON hor.id = hord.idhorario
                            WHERE hord.idhorario = @idhorario ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Horario objHor = new Modelo.Horario();
            SetInstance(dr, objHor);
            return objHor;

            //SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            //List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            //while (dr.Read())
            //{
            //    Modelo.Horario objHorario = new Modelo.Horario();
            //    AuxSetInstance(dr, objHorario);
            //    listaHorario.Add(objHorario);
            //}
            //if (!dr.IsClosed)
            //    dr.Close();
            //dr.Dispose();

            //return listaHorario;
        }

        public Modelo.Horario GetHorarioByHorarioDinamicoDataSequencia(int idHorarioDinamico, DateTime data, int cicloSequenciaIndice)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@idHorarioDinamico", SqlDbType.Int),
                new SqlParameter("@data", SqlDbType.DateTime),
                new SqlParameter("@cicloSequenciaIndice", SqlDbType.Int)
            };
            parms[0].Value = idHorarioDinamico;
            parms[1].Value = data;
            parms[2].Value = cicloSequenciaIndice;

            string aux = @" select 
                                h.*
                                , null AS datainicial
                                , null AS datafinal
                                , '' as DescParametro 
                                , '' as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            from horario h
                                inner join horariodetalhe hd on h.id = hd.idhorario
                                LEFT JOIN HorarioDinamico hdm on hdm.id = h.idhorariodinamico
                            where 
                                h.idhorariodinamico = @idHorarioDinamico
                                and hd.data = @data
                                and hd.ciclosequenciaindice = @cicloSequenciaIndice ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Horario objHor = new Modelo.Horario();
            SetInstance(dr, objHor);
            return objHor;
        }

        public List<Modelo.Horario> GetHorarioByHorarioDinamico(int idHorarioDinamico)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idHorarioDinamico", SqlDbType.Int) };
            parms[0].Value = idHorarioDinamico;

            string aux = @" select 
                                h.*
                                , null AS datainicial
                                , null AS datafinal
                                , '' as DescParametro 
                                , '' as DescClassificacao
                                , convert(varchar,hdm.codigo)+' | '+hdm.descricao as DescHorarioDinamico
                            FROM horario h
                                LEFT JOIN HorarioDinamico hdm on hdm.id = h.idhorariodinamico
                            where 
                                h.idhorariodinamico = @idHorarioDinamico ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Horario> listaHorario = new List<Modelo.Horario>();
            while (dr.Read())
            {
                Modelo.Horario objHorario = new Modelo.Horario();
                AuxSetInstance(dr, objHorario);
                listaHorario.Add(objHorario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return listaHorario;
        }
        #endregion

        #region Permissões
        /// <summary>
        /// Esse método adicionará a condição de acordo com a permissão do usuário e a restrição de permissão do horário.
        /// </summary>
        /// <param name="campoIdHorario">informar o campo que está o id do horário, por exemplo em uma consulta "select * from horario where 1 = 1 " informar horario.id em uma consulta com alias "select h.* from horario h where 1 = 1 " informar h.id ... </param>
        /// <returns></returns>
        public string AddPermissaoUsuario(string campoIdHorario)
        {
            if (UsuarioLogado != null)
            {
                string condicionalEmpresa = String.Format(@" (EXISTS ((SELECT 1
									 FROM empresacwusuario eu
									inner join HorarioRestricao hr on eu.idempresa = hr.IdEmpresa
												   WHERE eu.idcw_usuario = {0} AND hr.IdHorario = {1}))
								  ) ", UsuarioLogado.Id, campoIdHorario);
                string condicionalContrato = String.Format(@" (EXISTS ((SELECT 1
									 FROM contratousuario cu
									inner join HorarioRestricao hr on cu.idcontrato = hr.IdContrato
												   WHERE cu.idcwusuario = {0} AND hr.IdHorario = {1}))
								  ) ", UsuarioLogado.Id, campoIdHorario);
                string condicionalHorarioSemRestricao = String.Format(@" (NOT EXISTS(SELECT 1
                                                      FROM HorarioRestricao hr
                                                    WHERE hr.IdHorario = {0} )) ", campoIdHorario);
            
                if (UsuarioLogado.UtilizaControleEmpresa && UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1} OR {2}) ", condicionalHorarioSemRestricao, condicionalEmpresa, condicionalContrato);
                }
                else if (UsuarioLogado.UtilizaControleEmpresa && !UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalHorarioSemRestricao, condicionalEmpresa);
                }
                else if (UsuarioLogado.UtilizaControleContratos && !UsuarioLogado.UtilizaControleEmpresa)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalHorarioSemRestricao, condicionalContrato);
                }
            }
            return "";
        }
        #endregion
    }
}
