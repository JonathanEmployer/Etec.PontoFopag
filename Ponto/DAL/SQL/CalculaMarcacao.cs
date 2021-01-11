using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class CalculaMarcacao : DAL.ICalculaMarcacao
    {
        private Modelo.Cw_Usuario _UsuarioLogado;
        public Modelo.Cw_Usuario UsuarioLogado
        {
            get
            {
                return _UsuarioLogado;
            }
            set
            {
                if (value != null)
                {
                    _UsuarioLogado = value;
                }
                else
                {
                    _UsuarioLogado = cwkControleUsuario.Facade.getUsuarioLogado;
                }
                if (UsuarioLogado != null && dalMarcacao != null)
                {
                    dalMarcacao.UsuarioLogado = UsuarioLogado;
                }
            }
        }
        private DataBase db;
        private DAL.SQL.Marcacao _dalMarcacao;

        public DAL.SQL.Marcacao dalMarcacao
        {
            get { return _dalMarcacao; }
            set { _dalMarcacao = value; }
        }

        public CalculaMarcacao(DataBase database)
        {
            db = database;
            dalMarcacao = new DAL.SQL.Marcacao(db);
        }

        #region sqls
        public const string _sqlCalculoMarcacao = @" SELECT 
                          marcacao.id 
                        , marcacao.codigo 
                        , marcacao.idcompensado 
                        , marcacao.data 
                        , marcacao.dia 
                        , marcacao.ocorrencia 
                        , marcacao.abonardsr 
                        , marcacao.totalizadoresalterados 
                        , marcacao.calchorasextrasdiurna 
                        , marcacao.calchorasextranoturna 
                        , marcacao.calchorasfaltas 
                        , marcacao.calchorasfaltanoturna 
                        , marcacao.naoentrarbanco 
                        , marcacao.naoentrarnacompensacao 
                        , marcacao.naoconsiderarcafe 
                        , marcacao.semcalculo 
                        , marcacao.folga 
                        , marcacao.neutro 
                        , marcacao.totalHorasTrabalhadas 
                        , marcacao.dscodigo 
                        , marcacao.idfechamentobh 
                        , marcacao.idhorario 
                        , marcacao.idfuncionario 
                        , marcacao.entradaextra 
                        , marcacao.saidaextra 
                        , marcacao.entrada_1 
                        , marcacao.entrada_2 
                        , marcacao.entrada_3 
                        , marcacao.entrada_4 
                        , marcacao.entrada_5 
                        , marcacao.entrada_6 
                        , marcacao.entrada_7 
                        , marcacao.entrada_8 
                        , marcacao.saida_1 
                        , marcacao.saida_2 
                        , marcacao.saida_3 
                        , marcacao.saida_4 
                        , marcacao.saida_5 
                        , marcacao.saida_6 
                        , marcacao.saida_7 
                        , marcacao.saida_8 
                        , marcacao.ent_num_relogio_1 
                        , marcacao.ent_num_relogio_2 
                        , marcacao.ent_num_relogio_3 
                        , marcacao.ent_num_relogio_4 
                        , marcacao.ent_num_relogio_5 
                        , marcacao.ent_num_relogio_6 
                        , marcacao.ent_num_relogio_7 
                        , marcacao.ent_num_relogio_8 
                        , marcacao.sai_num_relogio_1 
                        , marcacao.sai_num_relogio_2 
                        , marcacao.sai_num_relogio_3 
                        , marcacao.sai_num_relogio_4 
                        , marcacao.sai_num_relogio_5 
                        , marcacao.sai_num_relogio_6 
                        , marcacao.sai_num_relogio_7 
                        , marcacao.sai_num_relogio_8 
                        , marcacao.incdata 
                        , marcacao.inchora 
                        , marcacao.incusuario  
						, ([dbo].CONVERTHORAMINUTOV2(marcacao.horastrabalhadas)) AS horastrabalhadasmin 
                        , ([dbo].CONVERTHORAMINUTOV2(marcacao.horastrabalhadasnoturnas)) AS horastrabalhadasnoturnasmin 
                        , ([dbo].CONVERTHORAMINUTOV2(marcacao.horasfaltas)) AS horasfaltasmin 
                        , ([dbo].CONVERTHORAMINUTOV2(marcacao.horasfaltanoturna)) AS horasfaltanoturnamin 
                        , ([dbo].CONVERTHORAMINUTOV2(marcacao.horasextrasdiurna)) AS horasextrasdiurnamin 
                        , ([dbo].CONVERTHORAMINUTOV2(marcacao.horasextranoturna)) AS horasextranoturnamin 
                        , ([dbo].CONVERTHORAMINUTOV2(ISNULL(marcacao.valordsr, '--:--'))) AS valordsrmin 
                        , ([dbo].CONVERTHORAMINUTOV2(ISNULL(marcacao.horascompensadas, '--:--'))) AS horascompensadasmin 
						, ([dbo].[convertbatidaminutoV2](marcacao.entrada_1)) AS entrada_1min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.entrada_2)) AS entrada_2min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.entrada_3)) AS entrada_3min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.entrada_4)) AS entrada_4min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.entrada_5)) AS entrada_5min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.entrada_6)) AS entrada_6min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.entrada_7)) AS entrada_7min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.entrada_8)) AS entrada_8min 
						, ([dbo].[convertbatidaminutoV2](marcacao.saida_1)) AS saida_1min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.saida_2)) AS saida_2min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.saida_3)) AS saida_3min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.saida_4)) AS saida_4min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.saida_5)) AS saida_5min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.saida_6)) AS saida_6min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.saida_7)) AS saida_7min 
                        , ([dbo].[convertbatidaminutoV2](marcacao.saida_8)) AS saida_8min 

                        , funcionario.idfuncao 
                        , funcionario.iddepartamento 
                        , funcionario.idempresa 
                        , funcionario.dataadmissao 
                        , funcionario.datademissao 
                        , funcionario.naoentrarbanco AS naoentrarbancofunc 
                        , funcionario.naoentrarcompensacao AS naocompensacaofunc 
                        , horario.tipohorario 
                        , horario.consideraadhtrabalhadas 
                        , horario.conversaohoranoturna 
                        , horario.dias_cafe_1 
                        , horario.dias_cafe_2 
                        , horario.dias_cafe_3 
                        , horario.dias_cafe_4 
                        , horario.dias_cafe_5 
                        , horario.dias_cafe_6 
                        , horario.dias_cafe_7 
                        , horario.diasemanadsr 
                        , horario.marcacargahorariamista AS marcacargahorariamistahorario 
                        , horario.habilitaperiodo01 
                        , horario.habilitaperiodo02 
                        , horario.consideraradicionalnoturnointerv 
                        , ISNULL(parametros.toleranciaAdicionalNoturno, 0) AS toleranciaAdicionalNoturno 
                        , [dbo].CONVERTHORAMINUTOV2(horario.limitemax) AS limitemax 
                        , [dbo].CONVERTHORAMINUTOV2(horario.limitemin) AS limitemin 
                        , ISNULL(horario.ordenabilhetesaida, 0) AS ordenabilhetesaida 
                        , ISNULL(horario.DescontarAtrasoInItinere, 0) DescontarAtrasoInItinere 
                        , ISNULL(horario.DescontarFaltaInItinere, 0) DescontarFaltaInItinere 
                        , ISNULL(horario.HabilitaInItinere, -1)  HabilitaInItinere 
                        , horariodetalhe.diadsr 
                        , horariodetalhe.intervaloautomatico 
                        , horariodetalhe.preassinaladas1 
                        , horariodetalhe.preassinaladas2 
                        , horariodetalhe.preassinaladas3 
                        , horariodetalhe.dia
                        , horariodetalhe.entrada_1 AS entrada_1hd 
                        , horariodetalhe.entrada_2 AS entrada_2hd 
                        , horariodetalhe.entrada_3 AS entrada_3hd 
                        , horariodetalhe.entrada_4 AS entrada_4hd 
                        , horariodetalhe.saida_1 AS saida_1hd 
                        , horariodetalhe.saida_2 AS saida_2hd 
                        , horariodetalhe.saida_3 AS saida_3hd 
                        , horariodetalhe.saida_4 AS saida_4hd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.entrada_1) AS entrada_1minhd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.entrada_2) AS entrada_2minhd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.entrada_3) AS entrada_3minhd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.entrada_4) AS entrada_4minhd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.saida_1) AS saida_1minhd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.saida_2) AS saida_2minhd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.saida_3) AS saida_3minhd 
                        , [dbo].CONVERTBATIDAMINUTOV2(horariodetalhe.saida_4) AS saida_4minhd 
                        , [dbo].CONVERTHORAMINUTOV2(horariodetalhe.totaltrabalhadadiurna) AS totaltrabalhadadiurnamin 
                        , [dbo].CONVERTHORAMINUTOV2(horariodetalhe.totaltrabalhadanoturna) AS totaltrabalhadanoturnamin 
                        , [dbo].CONVERTHORAMINUTOV2(horariodetalhe.cargahorariamista) AS cargahorariamistamin 
                        , horariodetalhe.totaltrabalhadadiurna 
                        , horariodetalhe.totaltrabalhadanoturna 
                        , horariodetalhe.cargahorariamista 
                        , horario.marcacargahorariamista AS marcacargahorariamistahd 
                        , horariodetalhe.bcarregar 
                        , horariodetalhe.flagfolga 
                        , horariodetalhe.neutro flagneutro 

						,(SELECT id FROM [dbo].[F_BancoHoras] (marcacao.data, funcionario.id)) AS idbancohoras  

						, feriado.id AS idferiado 
                        , afastamentofunc.id AS idafastamentofunc 
                        , afastamentofunc.abonado AS abonadofunc 
                        , afastamentofunc.semcalculo AS semcalculofunc 
                        , afastamentofunc.semabono AS semabonofunc 
                        , afastamentofunc.horai AS horaifunc 
                        , afastamentofunc.horaf AS horaffunc 
                        , afastamentofunc.idocorrencia AS idocorrenciafunc
                        , afastamentofunc.contabilizarjornada AS contabilizarjornadafunc
                        , afastamentodep.id AS idafastamentodep 
                        , afastamentodep.contabilizarjornada AS contabilizarjornadadep
                        , afastamentodep.abonado AS abonadodep 
                        , afastamentodep.semcalculo AS semcalculodep 
                        , afastamentodep.semabono AS semabonodep 
                        , afastamentodep.horai AS horaidep 
                        , afastamentodep.horaf AS horafdep 
                        , afastamentodep.idocorrencia AS idocorrenciadep 
                        , afastamentoemp.id AS idafastamentoemp 
                        , afastamentoemp.abonado AS abonadoemp 
                        , afastamentoemp.semcalculo AS semcalculoemp 
                        , afastamentoemp.semabono AS semabonoemp 
                        , afastamentoemp.horai AS horaiemp 
                        , afastamentoemp.horaf AS horafemp 
                        , afastamentoemp.idocorrencia AS idocorrenciaemp
                        , afastamentoemp.contabilizarjornada AS contabilizarjornadaemp
                        , afastamentocont.id AS idafastamentocont 
                        , afastamentocont.abonado AS abonadocont 
                        , afastamentocont.semcalculo AS semcalculocont 
                        , afastamentocont.semabono AS semabonocont 
                        , afastamentocont.horai AS horaicont 
                        , afastamentocont.horaf AS horafcont 
                        , afastamentocont.idocorrencia AS idocorrenciacont 
                        , afastamentocont.contabilizarjornada AS contabilizarjornadacont
                        , jornadaalternativa_view.id AS idjornadaalternativa

						,(SELECT TOP(1) id FROM mudancahorario WHERE mudancahorario.idfuncionario = marcacao.idfuncionario AND mudancahorario.data = marcacao.data ORDER BY mudancahorario.id DESC) AS idmudancahorario 

						, marcacao.tipohoraextrafalta 
                        , parametros.thoraextra 
                        , parametros.thorafalta 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.thoraextra) AS thoraextramin 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.thoraextraEntrada) AS thoraextraEntradamin 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.thoraextraSaida) AS thoraextraSaidamin 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.thorafalta) AS thorafaltamin 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.thorafaltaEntrada) AS thorafaltaEntradamin 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.thorafaltaSaida) AS thorafaltaSaidamin 
                        , [dbo].CONVERTHORAMINUTOV2(parametros.inicioadnoturno) AS inicioadnoturnomin 
                        , [dbo].CONVERTHORAMINUTOV2(parametros.fimadnoturno) AS fimadnoturnomin 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.TIntervaloExtra) AS TIntervaloExtra 
                        , [dbo].ConvertHoraMinutoV2Nulavel(parametros.TIntervaloFalta) AS TIntervaloFalta 

						, marcacao.exphorasextranoturna 
                        , parametros.bConsiderarHEFeriadoPHoraNoturna
                        , parametros.Flg_Estender_Periodo_Noturno
                        , parametros.Flg_Separar_Trabalhadas_Noturna_Extras_Noturna
                        , parametros.reducaohoranoturna 
                        , parametros.HabilitarControleInItinere 
                        , funcionario.nome nomeFuncionario 
                        , marcacao.IdDocumentoWorkflow 
                        , marcacao.DocumentoWorkflowAberto 
                        , marcacao.InItinereHrsDentroJornada 
                        , marcacao.InItinerePercDentroJornada 
                        , marcacao.InItinereHrsForaJornada 
                        , marcacao.InItinerePercForaJornada 
                        , marcacao.NaoConsiderarInItinere 
                        , marcacao.idfechamentoponto 
                        , marcacao.Interjornada 
                        , marcacao.LegendasConcatenadas 
                        , marcacao.AdicionalNoturno 
                        , marcacao.DataBloqueioEdicaoPnlRh 
                        , marcacao.LoginBloqueioEdicaoPnlRh 
                        , HorarioInItinere.MarcaDiaBool DiaPossuiInItinere 
                        , HorarioInItinere.PercentualDentroJornada PercentualDentroJornadaInItinere 
                        , HorarioInItinere.PercentualDentroFora PercentualForaJornadaInItinere 
                        , feriado.Parcial AS FeriadoParcial 
                        , feriado.HoraInicio AS FeriadoParcialInicio 
                        , feriado.HoraFim AS FeriadoParcialFim 
                        , marcacao.horaExtraInterjornada 
                        , marcacao.horasTrabalhadasDentroFeriadoDiurna 
                        , marcacao.horasTrabalhadasDentroFeriadoNoturna 
                        , marcacao.horasPrevistasDentroFeriadoDiurna 
                        , marcacao.horasPrevistasDentroFeriadoNoturna 
                        , horariodetalhe.id idHorarioDetalhe 
                        , horario.idhorariodinamico 
                        , parametros.MomentoPreAssinalado 
                        , marcacao.Legenda
						, marcacao.Bancohorascre
						, marcacao.Bancohorasdeb
						, marcacao.Horascompensadas
                        , marcacao.Dsr
                        , funcionario.datainativacao
                        , marcacao.naoconsiderarferiado
                        , marcacao.ContabilizarFaltas
                        , marcacao.ContAtrasosSaidasAntec
                        , marcacao.ContabilizarCreditos
                        , marcacao.IdJornadaSubstituir
                        , horariodetalhe.idjornada IdJornadaHorario
               FROM marcacao_view as marcacao with (nolock)
               INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario 
               INNER JOIN horario ON horario.id = marcacao.idhorario 
               INNER JOIN parametros ON parametros.id = horario.idparametro 
               LEFT JOIN horariodetalhe 
               ON horariodetalhe.idhorario = marcacao.idhorario AND 
               ((horario.tipohorario = 2 AND horariodetalhe.data = marcacao.data) OR 
               (horariodetalhe.idhorario = marcacao.idhorario 
               AND horario.tipohorario = 1  
               AND horariodetalhe.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, marcacao.data) AS INT)-1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, marcacao.data) AS INT)-1) END))) 
               OUTER APPLY (                              
                               SELECT TOP 1 *              
                                 FROM afastamento          
                                WHERE afastamento.tipo = 0 
                                  AND afastamento.idfuncionario = marcacao.idfuncionario 
                                  AND marcacao.data BETWEEN afastamento.datai AND isnull(afastamento.dataf, '9999-12-31') 
                                ORDER BY inchora 
                               ) afastamentofunc 
               LEFT JOIN afastamento afastamentodep ON afastamentodep.tipo = 1 
               AND afastamentodep.iddepartamento = funcionario.iddepartamento 
               AND marcacao.data >= afastamentodep.datai 
               AND marcacao.data <= isnull(afastamentodep.dataf, '9999-12-31') 
               LEFT JOIN afastamento afastamentoemp ON afastamentoemp.tipo = 2 
               AND afastamentoemp.idempresa = funcionario.idempresa 
               AND marcacao.data >= afastamentoemp.datai 
               AND marcacao.data <= isnull(afastamentoemp.dataf, '9999-12-31') 
               --afastamento por contrato
               LEFT JOIN afastamento afastamentocont ON afastamentocont.tipo = 3 AND afastamentocont.idcontrato in (SELECT idcontrato FROM contratofuncionario cf WHERE cf.idfuncionario = funcionario.id) AND marcacao.data >= afastamentocont.datai AND marcacao.data <= isnull(afastamentocont.dataf, '9999-12-31') 
               --Busca Feriado
                OUTER APPLY (SELECT TOP(1) * FROM feriado where horario.desconsiderarferiado = 0 
                        AND feriado.data = marcacao.data 
                        AND ( feriado.tipoferiado = 0 
                            OR ( feriado.tipoferiado = 1 
                                AND feriado.idempresa = funcionario.idempresa 
                                ) 
                            OR ( feriado.tipoferiado = 2 
                                AND feriado.iddepartamento = funcionario.iddepartamento 
                                ) 
                            OR ( feriado.tipoferiado = 3 
                                AND EXISTS ( SELECT * 
                                                FROM   FeriadoFuncionario FFUNC 
                                                WHERE  feriado.id = FFUNC.idFeriado 
                                                    AND FFUNC.idFuncionario = funcionario.id ) 
                                ) 
                            )) feriado 
               -- Busca InItinere
               LEFT JOIN dbo.HorarioInItinere ON HorarioInItinere.idhorario = marcacao.idhorario 
                                   AND HorarioInItinere.Dia = ( CASE WHEN Feriado.id IS NOT NULL THEN 8 
               													WHEN horariodetalhe.flagfolga = 1 OR marcacao.folga = 1 THEN 9 
               													WHEN ( CAST(DATEPART(WEEKDAY, marcacao.data) AS INT) - 1 ) = 0 THEN 7 
               													ELSE ( CAST(DATEPART(WEEKDAY, marcacao.data) AS INT) - 1 ) 
                                                      END ) 
               LEFT JOIN jornadaalternativa_view ON 
               ((jornadaalternativa_view.tipo = 0 AND jornadaalternativa_view.identificacao = funcionario.idempresa) 
               OR (jornadaalternativa_view.tipo = 1 AND jornadaalternativa_view.identificacao = funcionario.iddepartamento) 
               OR (jornadaalternativa_view.tipo = 2 AND jornadaalternativa_view.identificacao = funcionario.id) 
               OR (jornadaalternativa_view.tipo = 3 AND jornadaalternativa_view.identificacao = funcionario.idfuncao)) 
               AND (jornadaalternativa_view.datacompensada = marcacao.data 
               OR (jornadaalternativa_view.datacompensada IS NULL 
               AND marcacao.data >= jornadaalternativa_view.datainicial 
               AND marcacao.data <= jornadaalternativa_view.datafinal))
            ";
        #endregion

        #region Calculo DSR
        public DataTable GetFuncionariosDSRWebApi(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@identificacao", SqlDbType.Int),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };

            parms[0].Value = pIdentificacao;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string comando = "SELECT marcacao.idfuncionario, funcionario.nome, SUM(ISNULL(horario.descontardsr, 0)) AS qtddsr "
                             + " FROM marcacao_view AS marcacao"
                             + " INNER JOIN horario ON horario.id = marcacao.idhorario"
                             + " INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario"
                             + " WHERE ISNULL(marcacao.idfechamentoponto,0) = 0 AND marcacao.data >= @datai AND marcacao.data <= @dataf";
            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " AND funcionario.idempresa = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " AND funcionario.iddepartamento = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " AND marcacao.idfuncionario = @identificacao";
                        break;
                    case 3://Função
                        comando += " AND funcionario.idfuncao = @identificacao";
                        break;
                    case 4://Horário
                        comando += " AND marcacao.idhorario = @identificacao";
                        break;
                    default:
                        break;
                }
            }
            comando += " GROUP BY marcacao.idfuncionario, funcionario.nome";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public DataTable GetFuncionariosDSR(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@identificacao", SqlDbType.Int),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };

            parms[0].Value = pIdentificacao;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string comando = "SELECT marcacao.idfuncionario, funcionario.nome, SUM(ISNULL(horario.descontardsr, 0)) AS qtddsr "
                             + " FROM marcacao_view AS marcacao"
                             + " INNER JOIN horario ON horario.id = marcacao.idhorario"
                             + " INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario"
                             + " WHERE ISNULL(marcacao.idfechamentoponto,0) = 0 AND marcacao.data >= @datai AND marcacao.data <= @dataf";
            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " AND funcionario.idempresa = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " AND funcionario.iddepartamento = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " AND marcacao.idfuncionario = @identificacao";
                        break;
                    case 3://Função
                        comando += " AND funcionario.idfuncao = @identificacao";
                        break;
                    case 4://Horário
                        comando += " AND marcacao.idhorario = @identificacao";
                        break;
                    default:
                        break;
                }
            }
            comando += DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, comando, "funcionario.idempresa", "funcionario.id", null);
            comando += " GROUP BY marcacao.idfuncionario, funcionario.nome";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }
        public DataTable GetFuncionariosDSR(List<int> idsFuncionarios, DateTime pDataI, DateTime pDataF)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime),
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar)
            };

            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = String.Join(",", idsFuncionarios);

            string comando = "SELECT marcacao.idfuncionario, funcionario.nome, SUM(ISNULL(horario.descontardsr, 0)) AS qtddsr "
                             + " FROM marcacao_view AS marcacao"
                             + " INNER JOIN horario ON horario.id = marcacao.idhorario"
                             + " INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario"
                             + " WHERE marcacao.data >= @datai AND marcacao.data <= @dataf"
                             + "   AND ISNULL(marcacao.idfechamentoponto,0) = 0 AND marcacao.idfuncionario in (select * from dbo.f_clausulaIn(@idsFuncionarios)) ";

            comando += GetRestricaoUsuario();
            comando += " GROUP BY marcacao.idfuncionario, funcionario.nome";

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public Modelo.Proxy.pxyAbonoDsrFuncionario GetAbonoDsrFuncionario(int idFuncionario, DateTime dataInicio, DateTime dataFim)
        {
            Modelo.Proxy.pxyAbonoDsrFuncionario pxy = new Modelo.Proxy.pxyAbonoDsrFuncionario();
            SqlParameter[] parms1 = new SqlParameter[]
            {
                new SqlParameter("@idFuncionario", SqlDbType.Int),
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFim", SqlDbType.DateTime)
            };

            parms1[0].Value = idFuncionario;
            parms1[1].Value = dataInicio;
            parms1[2].Value = dataFim;

            SqlParameter[] parms2 = new SqlParameter[]
            {
                new SqlParameter("@idFuncionario", SqlDbType.Int),
                new SqlParameter("@dataInicio", SqlDbType.DateTime),
                new SqlParameter("@dataFim", SqlDbType.DateTime)
            };

            parms2[0].Value = idFuncionario;
            parms2[1].Value = dataInicio;
            parms2[2].Value = dataFim;

            string comando1 = @"
                        select die.idfuncionario id,
	                           die.nome_func nome,
	                           SUM(die.horasAbonadasParcial+horasAbonadasTotal) horas,
	                           count(*) dias
                                  from (
					                                select di.idfuncionario,
						                                   di.nome_func nome_func,
						                                   di.Marc_data,
						                                   dbo.FN_CONVHORA(horai)+ dbo.FN_CONVHORA(horaf) horasAbonadasParcial,
						                                   case when (dbo.FN_CONVHORA(horai)+ dbo.FN_CONVHORA(horaf)) = 0 then
										                                ((di.sai_1_min - di.entr_1_min) +
										                                 (di.sai_2_min - di.entr_2_min) +
										                                 (di.sai_3_min - di.entr_3_min) +
										                                 (di.sai_4_min - di.entr_4_min))
								                                else 0 end horasAbonadasTotal, a.id
									
					              from (
						            select f.nome nome_func,
							               dbo.FN_CONVHORA(ISNULL(ja.entrada_1, t.entrada_1)) entr_1_min,
							               dbo.FN_CONVHORA(ISNULL(ja.entrada_2, t.entrada_2)) entr_2_min,
							               dbo.FN_CONVHORA(ISNULL(ja.entrada_3, t.entrada_3)) entr_3_min,
							               dbo.FN_CONVHORA(ISNULL(ja.entrada_4, t.entrada_4)) entr_4_min,
							               dbo.FN_CONVHORA(ISNULL(ja.saida_1, t.saida_1)) sai_1_min,
							               dbo.FN_CONVHORA(ISNULL(ja.saida_2, t.saida_2)) sai_2_min,
							               dbo.FN_CONVHORA(ISNULL(ja.saida_3, t.saida_3)) sai_3_min,
							               dbo.FN_CONVHORA(ISNULL(ja.saida_4, t.saida_4)) sai_4_min,
							               t.Marc_dia,
							               t.Marc_data,
							               t.idfuncionario,							   
							               f.idempresa,
							               f.iddepartamento
						              from (
							            select h.tipohorario,
								               hd.entrada_1,
								               hd.entrada_2,
								               hd.entrada_3,
								               hd.entrada_4,
								               hd.saida_1,
								               hd.saida_2,
								               hd.saida_3,
								               hd.saida_4,
								               m.dia Marc_dia,
								               case when hd.dia = 1 then 'Seg.' when hd.dia = 2 then 'Ter.' when hd.dia = 3 then 'Qua.' when hd.dia = 4 then 'Qui.'
										            when hd.dia = 5 then 'Sex.' when hd.dia = 6 then 'Sáb.' when hd.dia = 7 then 'Dom.' else '' end AS dia,
								               m.data Marc_data,
								               hd.data,
								               m.idfuncionario
							              from marcacao as m
							             inner join horario as h on m.idhorario = h.id
							             inner join horariodetalhe as hd on h.id = hd.idhorario and 
																		    ((h.tipohorario = 1 and hd.dia = (CASE WHEN (DATEPART(WEEKDAY, m.data) - 1) = 0 THEN 7 ELSE (DATEPART(WEEKDAY, m.data) - 1)END)) or 
																			 (h.tipohorario = 2 and hd.data = m.data))
							             where convert(date,m.data) between convert(date,@dataInicio) and convert(date,@dataFim)
							               and m.idfuncionario = @idFuncionario
							               ) as t
							               JOIN funcionario AS f ON f.ID = t.idfuncionario
						              LEFT JOIN departamento AS d ON f.iddepartamento = d.id
						              LEFT JOIN empresa AS e ON f.idempresa = e.id
						              LEFT JOIN jornadaalternativa AS ja ON (((ja.tipo = 0 and ja.identificacao = e.id) or
																              (ja.tipo = 1 and ja.identificacao = d.id) or
																              (ja.tipo = 2 and ja.identificacao = f.id)) and
																              t.Marc_data between ja.datainicial and ja.datafinal)
									   ) di
					              inner join afastamento as a on a.abonado = 1 and
																 di.Marc_data between a.datai and isnull(a.dataf, '9999-12-31') and 
																 ( a.idfuncionario = di.idfuncionario or 
																  (a.iddepartamento = di.iddepartamento and a.idempresa = di.idempresa) or 
																  (a.iddepartamento is null and a.idempresa = di.idempresa)
																 )
				            ) die
				      where die.horasAbonadasParcial+horasAbonadasTotal > 0
		              group by die.idfuncionario,
				               die.nome_func";

            string comando2 = @"
                                select coalesce(COUNT(m.dia), 0) as diasDsr, 
                                coalesce(case 
	                                when h.bUtilizaDDSRProporcional = 0 then (coalesce(sum(dbo.FN_CONVHORA(h.qtdhorasdsr)),0)) 
	                                else (select max(dbo.FN_CONVHORA(ld.qtdhorasdsr)) from limiteddsr ld where ld.idhorario = h.id) * count(m.dia)
                                end, 0) as horasDsr 
                                from marcacao m
                                join horario h on m.idhorario = h.id
                                where m.dsr != 0
                                and m.idfuncionario = @idFuncionario
                                and m.data between CAST(@dataInicio as DATE) 
                                and CAST(@dataFim as DATE)
                                group by h.id, h.bUtilizaDDSRProporcional";
            DataTable dt1 = new DataTable();
            SqlDataReader dr1 = db.ExecuteReader(CommandType.Text, comando1.ToString(), parms1);
            dt1.Load(dr1);
            if (!dr1.IsClosed)
                dr1.Close();
            dr1.Dispose();

            DataTable dt2 = new DataTable();
            SqlDataReader dr2 = db.ExecuteReader(CommandType.Text, comando2.ToString(), parms2);
            dt2.Load(dr2);
            if (!dr2.IsClosed)
                dr2.Close();
            dr2.Dispose();

            if (dt1.Rows.Count > 0)
            {
                pxy.IdFuncionario = Convert.ToInt32(dt1.Rows[0]["id"]);
                pxy.Nome = Convert.ToString(dt1.Rows[0]["nome"]);
                pxy.QtdDiasAbono = Convert.ToInt32(dt1.Rows[0]["dias"]);
                pxy.QtdMinutosAbono = Convert.ToInt32(dt1.Rows[0]["horas"]);
            }
            if (dt2.Rows.Count > 0)
            {
                var iedt = dt2.AsEnumerable();
                pxy.QtdDiasDsr = iedt.Sum(s => s.Field<int>("diasDsr")); //Convert.ToInt32(dt2.Rows[0]["diasDsr"]);
                pxy.QtdMinutosDsr = iedt.Sum(s => s.Field<int>("horasDsr")); //Convert.ToInt32(dt2.Rows[0]["horasDsr"]);
            }
            return pxy;
        }

        public DataTable GetMarcacoesDSR(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idfuncionario", SqlDbType.Int),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };

            parms[0].Value = pIdFuncionario;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            StringBuilder comando = new StringBuilder("SELECT marcacao.id");
            comando.AppendLine(", marcacao.dscodigo");
            comando.AppendLine(", marcacao.dsr");
            comando.AppendLine(", marcacao.legenda");
            comando.AppendLine(", marcacao.idfuncionario");
            comando.AppendLine(", marcacao.dia");
            comando.AppendLine(", marcacao.data");
            comando.AppendLine(", marcacao.ocorrencia");
            comando.AppendLine(", marcacao.horastrabalhadas");
            comando.AppendLine(", marcacao.horastrabalhadasnoturnas");
            comando.AppendLine(", marcacao.horasextrasdiurna");
            comando.AppendLine(", marcacao.horasextranoturna");
            comando.AppendLine(", marcacao.horasfaltas");
            comando.AppendLine(", marcacao.horasfaltanoturna");
            comando.AppendLine(", marcacao.bancohorascre");
            comando.AppendLine(", marcacao.bancohorasdeb");
            comando.AppendLine(", marcacao.folga");
            comando.AppendLine(", marcacao.neutro");
            comando.AppendLine(", marcacao.totalHorasTrabalhadas");
            comando.AppendLine(", marcacao.naoconsiderarcafe");
            comando.AppendLine(", marcacao.naoentrarbanco");
            comando.AppendLine(", marcacao.semcalculo");
            comando.AppendLine(", marcacao.horascompensadas");
            comando.AppendLine(", marcacao.idcompensado");
            comando.AppendLine(", marcacao.idhorario");
            comando.AppendLine(", marcacao.entrada_1");
            comando.AppendLine(", marcacao.entrada_2");
            comando.AppendLine(", marcacao.entrada_3");
            comando.AppendLine(", marcacao.entrada_4");
            comando.AppendLine(", marcacao.entrada_5");
            comando.AppendLine(", marcacao.entrada_6");
            comando.AppendLine(", marcacao.entrada_7");
            comando.AppendLine(", marcacao.entrada_8");
            comando.AppendLine(", marcacao.saida_1");
            comando.AppendLine(", marcacao.saida_2");
            comando.AppendLine(", marcacao.saida_3");
            comando.AppendLine(", marcacao.saida_4");
            comando.AppendLine(", marcacao.saida_5");
            comando.AppendLine(", marcacao.saida_6");
            comando.AppendLine(", marcacao.saida_7");
            comando.AppendLine(", marcacao.saida_8");
            comando.AppendLine(", marcacao.entradaextra");
            comando.AppendLine(", marcacao.saidaextra");
            comando.AppendLine(", marcacao.ent_num_relogio_1");
            comando.AppendLine(", marcacao.ent_num_relogio_2");
            comando.AppendLine(", marcacao.ent_num_relogio_3");
            comando.AppendLine(", marcacao.ent_num_relogio_4");
            comando.AppendLine(", marcacao.ent_num_relogio_5");
            comando.AppendLine(", marcacao.ent_num_relogio_6");
            comando.AppendLine(", marcacao.ent_num_relogio_7");
            comando.AppendLine(", marcacao.ent_num_relogio_8");
            comando.AppendLine(", marcacao.sai_num_relogio_1");
            comando.AppendLine(", marcacao.sai_num_relogio_2");
            comando.AppendLine(", marcacao.sai_num_relogio_3");
            comando.AppendLine(", marcacao.sai_num_relogio_4");
            comando.AppendLine(", marcacao.sai_num_relogio_5");
            comando.AppendLine(", marcacao.sai_num_relogio_6");
            comando.AppendLine(", marcacao.sai_num_relogio_7");
            comando.AppendLine(", marcacao.sai_num_relogio_8");
            comando.AppendLine(", marcacao.naoentrarnacompensacao");
            comando.AppendLine(", marcacao.idfechamentobh");
            comando.AppendLine(", marcacao.abonardsr");
            comando.AppendLine(", marcacao.totalizadoresalterados");
            comando.AppendLine(", marcacao.calchorasextrasdiurna");
            comando.AppendLine(", marcacao.calchorasextranoturna");
            comando.AppendLine(", marcacao.calchorasfaltas");
            comando.AppendLine(", marcacao.calchorasfaltanoturna");
            comando.AppendLine(", marcacao.incdata");
            comando.AppendLine(", marcacao.inchora");
            comando.AppendLine(", marcacao.incusuario");
            comando.AppendLine(", marcacao.codigo");
            comando.AppendLine(", (CAST((SELECT [dbo].CONVERTHORAMINUTO(marcacao.horasfaltas)) AS INTEGER)");
            comando.AppendLine("+ CAST((SELECT [dbo].CONVERTHORAMINUTO(marcacao.horasfaltanoturna)) AS INTEGER)) AS horasfaltasdsr");
            comando.AppendLine(", (SELECT [dbo].CONVERTHORAMINUTO(marcacao.valordsr)) AS valordsrmin");
            comando.AppendLine(", horario.diasemanadsr");
            comando.AppendLine(", horario.descontardsr");
            comando.AppendLine(", horario.DDSRConsideraFaltaDuranteSemana");
            comando.AppendLine(", horario.DescontoHorasDSR");
            comando.AppendLine(", case when horario.bUtilizaDDSRProporcional = 0 then (SELECT [dbo].CONVERTHORAMINUTO(horario.limiteperdadsr))");
            comando.AppendLine("   else coalesce((select top(1) dbo.converthoraminuto(ld.limiteperdadsr) from limiteddsr ld where ld.idhorario = horario.id and dbo.converthoraminuto(ld.limiteperdadsr) <= ((CAST((SELECT [dbo].CONVERTHORAMINUTO(marcacao.horasfaltas)) AS INTEGER)+ CAST((SELECT [dbo].CONVERTHORAMINUTO(marcacao.horasfaltanoturna)) AS INTEGER))) order by dbo.converthoraminuto(ld.limiteperdadsr) desc), (select top(1)dbo.converthoraminuto(ld.limiteperdadsr) from limiteddsr ld where ld.idhorario = horario.id  order by dbo.converthoraminuto(ld.limiteperdadsr)), 0) end AS limiteperdadsr");
            comando.AppendLine(", case when horario.bUtilizaDDSRProporcional = 0 then (SELECT [dbo].CONVERTHORAMINUTO(horario.qtdhorasdsr))");
            comando.AppendLine("   else coalesce((select top(1) dbo.converthoraminuto(ld.qtdhorasdsr) from limiteddsr ld where ld.idhorario = horario.id and dbo.converthoraminuto(ld.limiteperdadsr) <= ((CAST((SELECT [dbo].CONVERTHORAMINUTO(marcacao.horasfaltas)) AS INTEGER)+ CAST((SELECT [dbo].CONVERTHORAMINUTO(marcacao.horasfaltanoturna)) AS INTEGER))) order by dbo.converthoraminuto(ld.limiteperdadsr) desc), (select top(1)dbo.converthoraminuto(ld.qtdhorasdsr) from limiteddsr ld where ld.idhorario = horario.id  order by dbo.converthoraminuto(ld.limiteperdadsr)), 0) end AS qtdhorasdsrmin");
            comando.AppendLine(", ISNULL(marcacao.exphorasextranoturna, '--:--') AS exphorasextranoturna");
            comando.AppendLine(", marcacao.idfechamentoponto ");
            comando.AppendLine(", marcacao.Interjornada ");
            comando.AppendLine(", marcacao.IdDocumentoWorkflow ");
            comando.AppendLine(", marcacao.DocumentoWorkflowAberto ");
            comando.AppendLine(", marcacao.InItinereHrsDentroJornada ");
            comando.AppendLine(", marcacao.InItinerePercDentroJornada ");
            comando.AppendLine(", marcacao.InItinereHrsForaJornada ");
            comando.AppendLine(", marcacao.InItinerePercForaJornada  ");
            comando.AppendLine(", marcacao.NaoConsiderarInItinere ");
            comando.AppendLine(", marcacao.LegendasConcatenadas ");
            comando.AppendLine(", marcacao.AdicionalNoturno");
            comando.AppendLine(", marcacao.DataBloqueioEdicaoPnlRh");
            comando.AppendLine(", marcacao.LoginBloqueioEdicaoPnlRh");
            comando.AppendLine(", marcacao.horaExtraInterjornada");
            comando.AppendLine(", marcacao.horasTrabalhadasDentroFeriadoDiurna");
            comando.AppendLine(", marcacao.horasTrabalhadasDentroFeriadoNoturna");
            comando.AppendLine(", marcacao.horasPrevistasDentroFeriadoDiurna");
            comando.AppendLine(", marcacao.horasPrevistasDentroFeriadoNoturna");
            comando.AppendLine(", horario.DSRPorPercentual");
            comando.AppendLine(", marcacao.naoconsiderarferiado");
            comando.AppendLine(", marcacao.contabilizarfaltas");
            comando.AppendLine(", marcacao.contabilizarcreditos");
            comando.AppendLine(", marcacao.IdJornadaSubstituir");
            comando.AppendLine(", marcacao.contatrasossaidasantec");
            comando.AppendLine(" FROM marcacao_view AS marcacao");
            comando.AppendLine(" INNER JOIN horario ON horario.id = marcacao.idhorario");
            comando.AppendLine(" WHERE marcacao.idfuncionario = @idfuncionario");
            comando.AppendLine(" AND marcacao.data >= @datai AND marcacao.data <= @dataf");
            comando.AppendLine("ORDER BY marcacao.data");

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando.ToString(), parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }
        #endregion

        #region Calculo Marcação
        /// <summary>
        /// Pega as marcações para realizar o calculo
        /// </summary>
        /// <param name="pTipo">Tipo</param>
        /// <param name="identificacao">Identificação</param>
        /// <param name="pDataI">Data inicial</param>
        /// <param name="pDataF">Data final</param>
        /// <param name="PegaInativos">true = pega inativos, false = não pega os inativos</param>
        /// <returns>Datatable com as marcações e outros dados.</returns>
        /// 

        public string GetLegenda(int pidFuncionario, DateTime pData)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@data", SqlDbType.DateTime),
                    new SqlParameter("@idfuncionario", SqlDbType.Int)
            };
            parms[0].Value = pData;
            parms[1].Value = pidFuncionario;

            string aux = "SELECT ISNULL(legenda, '') AS legenda FROM marcacao_view WHERE data = @data AND idfuncionario = @idfuncionario";

            return (string)db.ExecuteScalar(CommandType.Text, aux, parms);

        }

        public DataTable GetMarcacoesCalculoWebApi(int? pTipo, int identificacao, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = _sqlCalculoMarcacao;
            aux += " WHERE ISNULL(marcacao.idfechamentoponto,0) = 0 AND ";
            if (pegaInativos)
            {
                if (!pegaExcluidos)
                    aux += "ISNULL(funcionario.excluido,0) = 0 AND ";
            }
            else
            {
                if (!pegaExcluidos)
                {
                    aux += "ISNULL(funcionario.excluido,0) = 0 AND ";
                }
                aux += "funcionarioativo = 1 AND ";
            }

            if (pTipo.HasValue && identificacao != 0)
            {
                switch (pTipo)
                {
                    case 0:
                        aux += "funcionario.idempresa = ";
                        break;
                    case 1:
                        aux += "funcionario.iddepartamento = ";
                        break;
                    case 2:
                        aux += "marcacao.idfuncionario = ";
                        break;
                    case 3:
                        aux += "funcionario.idfuncao = ";
                        break;
                    case 4:
                        aux += "horario.id = ";
                        break;
                    case 6:
                        aux += "cf.idcontrato = ";
                        break;
                }

                aux += identificacao.ToString() + " AND ";
            }

            aux += "marcacao.data >= @datai AND marcacao.data <= @dataf ";
            aux += "ORDER BY funcionario.id, marcacao.data";

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            foreach (System.Data.DataColumn col in dt.Columns) col.ReadOnly = false;
            return dt;
        }

        public DataTable GetMarcacoesCalculo(int? pTipo, int identificacao, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos, bool recalculaBHFechado)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = _sqlCalculoMarcacao;
            aux += " WHERE ISNULL(marcacao.idfechamentoponto,0) = 0 AND ";
            if (!recalculaBHFechado)
            {
                aux += "ISNULL(marcacao.idfechamentobh,0) = 0 AND ";
            }
            if (pegaInativos)
            {
                if (!pegaExcluidos)
                    aux += "ISNULL(funcionario.excluido,0) = 0 AND ";
            }
            else
            {
                if (!pegaExcluidos)
                {
                    aux += "ISNULL(funcionario.excluido,0) = 0 AND ";
                }
                aux += "funcionarioativo = 1 AND ";
            }

            if (pTipo.HasValue && identificacao != 0)
            {
                switch (pTipo)
                {
                    case 0:
                        aux += "funcionario.idempresa = ";
                        break;
                    case 1:
                        aux += "funcionario.iddepartamento = ";
                        break;
                    case 2:
                        aux += "marcacao.idfuncionario = ";
                        break;
                    case 3:
                        aux += "funcionario.idfuncao = ";
                        break;
                    case 4:
                        aux += "horario.id = ";
                        break;
                    case 6:
                        aux += "cf.idcontrato = ";
                        break;
                }

                aux += identificacao.ToString() + " AND ";
            }

            aux += "marcacao.data >= @datai AND marcacao.data <= @dataf ";
            aux += DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, aux, "funcionario.idempresa", "funcionario.id", null);
            aux += "ORDER BY funcionario.id, marcacao.data";

            DataTable dt = new DataTable();

            //SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            //dt.Load(dr);
            dt = db.ExecuteReaderToDataTable(aux, parms);
            //if (!dr.IsClosed)
            //    dr.Close();
            //dr.Dispose();

            return dt;
        }

        public DataTable GetMarcacoesCalculo(List<int> idsFuncionarios, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                    new SqlParameter("@datai", SqlDbType.DateTime),
                    new SqlParameter("@dataf", SqlDbType.DateTime),
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = String.Join(",", idsFuncionarios);

            string aux = _sqlCalculoMarcacao;
            aux += " WHERE 1 = 1 AND ";
            aux += " ISNULL(marcacao.idfechamentoponto,0) = 0 AND ";
            if (pegaInativos)
            {
                if (!pegaExcluidos)
                    aux += " ISNULL(funcionario.excluido,0) = 0 AND ";
            }
            else
            {
                if (!pegaExcluidos)
                {
                    aux += " ISNULL(funcionario.excluido,0) = 0 AND ";
                }
                aux += " funcionarioativo = 1 AND ";
            }

            aux += " marcacao.idfuncionario in (select * from dbo.f_clausulaIn(@idsFuncionarios)) AND ";

            aux += " marcacao.data >= @datai AND marcacao.data <= @dataf ";
            aux += DALBase.PermissaoUsuarioFuncionario(UsuarioLogado, aux, "funcionario.idempresa", "marcacao.idfuncionario", null);
            aux += " ORDER BY funcionario.id, marcacao.data option(MaxDop 8) ";

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetMarcacoesGerarHorariosDinamicos()
        {
            SqlParameter[] parms = new SqlParameter[] { };
            
            DataTable dt = new DataTable();
            string aux = _sqlCalculoMarcacao;
            aux += @" WHERE ISNULL(marcacao.idfechamentoponto,0) = 0 
            	        AND horario.idhorariodinamico is not null 
				        AND horariodetalhe.id is null ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public void ExecutarComandos(List<string> comandos)
        {
            db.ExecutarComandos(comandos, 100);
        }

        private string MontaStringUpdateTratamentoMarc(Modelo.BilhetesImp bil)
        {
            string login = String.Empty;

            if (cwkControleUsuario.Facade.getUsuarioLogado == null)
                login = UsuarioLogado.Login;
            else
                login = cwkControleUsuario.Facade.getUsuarioLogado.Login;

            StringBuilder str = new StringBuilder("UPDATE bilhetesimp SET ");
            str.AppendLine("codigo = " + bil.Codigo);
            str.AppendLine(", ordem = '" + bil.Ordem + "'");
            str.AppendLine(", data = CONVERT(DATETIME, '" + bil.Data.Day + "-" + bil.Data.Month + "-" + bil.Data.Year + "',105)");
            str.AppendLine(", hora = '" + bil.Hora + "'");
            str.AppendLine(", func = '" + bil.Func + "'");
            str.AppendLine(", relogio = '" + bil.Relogio + "'");
            str.AppendLine(", importado = " + bil.Importado);
            str.AppendLine(", mar_data = CONVERT(DATETIME, '" + bil.Mar_data.Value.Day + "-" + bil.Mar_data.Value.Month + "-" + bil.Mar_data.Value.Year + "', 105)");
            str.AppendLine(", mar_hora = '" + bil.Mar_hora + "'");
            str.AppendLine(", mar_relogio = '" + bil.Mar_relogio + "'");
            str.AppendLine(", posicao = " + bil.Posicao);
            str.AppendLine(", ent_sai = '" + bil.Ent_sai + "'");
            str.AppendLine(", chave = '" + bil.ToMD5() + "'");
            str.AppendLine(", dscodigo = '" + bil.DsCodigo + "'");
            str.AppendLine(", motivo = '" + bil.Motivo + "'");
            if (bil.Ocorrencia != '\0')
                str.AppendLine(", ocorrencia = '" + bil.Ocorrencia.ToString() + "'");
            if (bil.Idjustificativa > 0)
                str.AppendLine(", idjustificativa = " + bil.Idjustificativa);
            else
                str.AppendLine(", idjustificativa = NULL");
            DateTime dt = DateTime.Now;
            str.AppendLine(", altdata = CONVERT(DATETIME, '" + dt.Day + "-" + dt.Month + "-" + dt.Year + "',105)");
            str.AppendLine(", althora = CONVERT(DATETIME, '" + dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.ToLongTimeString() + "',120)");

            str.AppendLine(", altusuario = '" + login + "'");
            str.AppendLine(", IdFuncionario = " + bil.IdFuncionario);
            str.AppendLine(", PIS = '" + bil.PIS + "'");
            str.AppendLine("WHERE id = " + bil.Id);

            return str.ToString();
        }

        private string MontaStringInsertTratamentoMarc(Modelo.BilhetesImp bil)
        {
            StringBuilder str = new StringBuilder();

            bil.Chave = bil.ToMD5();
            str.Remove(0, str.Length);
            str.Append("INSERT INTO bilhetesimp ");
            str.Append("(codigo,ordem, data, hora, func, relogio, importado, mar_data, mar_hora, mar_relogio, posicao, ent_sai, incdata, inchora, incusuario, chave, dscodigo, motivo, ocorrencia, idjustificativa, IdFuncionario, PIS) ");
            str.Append("VALUES (");
            str.Append(bil.Codigo);
            str.Append(", '");
            str.Append(bil.Ordem);
            str.Append("', ");
            str.Append("CONVERT(DATETIME, '" + bil.Data.Day + "-" + bil.Data.Month + "-" + bil.Data.Year + "',105)");
            str.Append(", '");
            str.Append(bil.Hora);
            str.Append("', '");
            str.Append(bil.Func);
            str.Append("', '");
            str.Append(bil.Relogio);
            str.Append("', ");
            str.Append(bil.Importado);
            str.Append(", ");
            str.Append("CONVERT(DATETIME, '" + bil.Mar_data.Value.Day + "-" + bil.Mar_data.Value.Month + "-" + bil.Mar_data.Value.Year + "', 105)");
            str.Append(", '");
            str.Append(bil.Mar_hora);
            str.Append("', '");
            str.Append(bil.Mar_relogio);
            str.Append("', ");
            str.Append(bil.Posicao);
            str.Append(", '");
            str.Append(bil.Ent_sai);
            str.Append("', ");
            str.Append("CONVERT(DATETIME, '" + DateTime.Now.Date.Day + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Date.Year + "', 105)");
            str.Append(", ");
            str.Append("CONVERT(DATETIME, '" + DateTime.Now.Date.Year + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Date.Day + " " + DateTime.Now.ToLongTimeString() + "', 120)");
            str.Append(", '");
            str.Append(UsuarioLogado.Login);
            str.Append("', '");
            str.Append(bil.Chave);
            str.Append("', '");
            str.Append(bil.DsCodigo);
            str.Append("', '");
            str.Append(bil.Motivo);
            str.Append("', '");
            str.Append(bil.Ocorrencia);
            str.Append("', ");
            if (bil.Idjustificativa > 0)
                str.Append(bil.Idjustificativa);
            else
                str.Append("NULL");
            str.Append(", ");
            str.Append(bil.IdFuncionario);
            str.Append(", '");
            str.Append(bil.PIS);
            str.Append("')");

            return str.ToString();
        }

        private string MontaStringDeleteTratamentoMarc(Modelo.BilhetesImp bil)
        {
            StringBuilder ret = new StringBuilder("DELETE FROM bilhetesimp WHERE bilhetesimp.id = " + bil.Id);

            return ret.ToString();
        }

        public List<string> RetornaComandosTratamentoMarcacao(List<Modelo.BilhetesImp> pTratamentos)
        {
            List<string> ret = new List<string>();
            foreach (Modelo.BilhetesImp trat in pTratamentos)
            {
                switch (trat.Acao)
                {
                    case Modelo.Acao.Alterar:
                        ret.Add(this.MontaStringUpdateTratamentoMarc(trat));
                        break;
                    case Modelo.Acao.Incluir:
                        ret.Add(this.MontaStringInsertTratamentoMarc(trat));
                        break;
                    case Modelo.Acao.Excluir:
                        ret.Add(this.MontaStringDeleteTratamentoMarc(trat));
                        break;
                }
            }
            return ret;
        }

        public void PersistirDados(IEnumerable<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (marcacoes != null)
                        {
                            dalMarcacao.AtualizarMarcacoesEmLote(marcacoes, conn, trans);
                        }

                        SalvarBilhetes(bilhetes, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        conn.Dispose();
                        throw (ex);
                    }
                }

            }
        }

        public void PersistirDadosWebApi(IEnumerable<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes, string login)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (marcacoes != null)
                        {
                            dalMarcacao.AtualizarMarcacoesEmLoteWebApi(marcacoes, conn, trans, login);
                        }
                        SalvarBilhetes(bilhetes, trans);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        conn.Dispose();
                        throw (ex);
                    }
                }
            }
        }

        private void SalvarBilhetes(List<Modelo.BilhetesImp> bilhetes, SqlTransaction trans)
        {
            if (bilhetes != null && bilhetes.Count() > 0)
            {
                DAL.SQL.BilhetesImp dalBilhetesImp = new DAL.SQL.BilhetesImp(db);
                dalBilhetesImp.UsuarioLogado = this.UsuarioLogado;
                List<Modelo.BilhetesImp> bilhetesprocessar = new List<Modelo.BilhetesImp>();
                bilhetesprocessar = bilhetes.Where(w => w.Acao == Modelo.Acao.Excluir).ToList();
                if (bilhetesprocessar.Count() > 0)
                {
                    dalBilhetesImp.ExcluirLote(trans, bilhetesprocessar);
                }
                bilhetesprocessar = bilhetes.Where(w => w.Acao == Modelo.Acao.Alterar).ToList();
                if (bilhetesprocessar.Count() > 0)
                {
                    dalBilhetesImp.AtualizarBilhetesEmLote(bilhetesprocessar, trans);
                }
                bilhetesprocessar = bilhetes.Where(w => w.Acao == Modelo.Acao.Incluir).ToList();
                if (bilhetesprocessar.Count() > 0)
                {
                    dalBilhetesImp.IncluirbilhetesEmLote(trans, bilhetesprocessar);
                }
                //TransactDbOps.ExecutarComandos(comandos, 100, trans);
            }
        }
        #endregion

        private string GetRestricaoUsuario()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = funcionario.idempresa) > 0 ";
            }
            return "";
        }
    }
}
