using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class CartaoPonto : DAL.SQL.DALBase, ICartaoPonto
    {
        public CartaoPonto(DataBase database)
        {
            db = database;
        }
        #region Marcação

        public DataTable GetCartaoPontoRel(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, int normalFlexivel, bool ordenaDeptoFuncionario, string filtro)
        {
            string filtroFuncs = "";
            switch (tipo)
            {
                case 0:
                    filtroFuncs += " AND f.idempresa IN " + empresas;
                    break;
                case 1:
                    filtroFuncs += " AND f.iddepartamento IN " + departamentos;
                    break;
                case 2:
                    filtroFuncs += " AND f.id IN " + funcionarios;
                    break;
            }
            DAL.SQL.Funcionario dalFunc = new DAL.SQL.Funcionario(db);

            IList<Modelo.Proxy.pxyFuncionarioRelatorio> funcs = dalFunc.GetRelFuncionariosRelatorios(filtroFuncs);

            IEnumerable<long> ids = funcs.Select(s => long.Parse(s.Id.ToString()));

            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime),
                    new SqlParameter("@IdsFuncs", SqlDbType.Structured),
                    new SqlParameter("@normalflexivel", SqlDbType.Int)
            };

            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;
            parms[2].Value = CreateDataTableIdentificadores(ids);
            parms[2].TypeName = "Identificadores";
            parms[3].Value = normalFlexivel;

            string aux = @" SELECT *
                              INTO #tratamentos
                              FROM [FnGetTratamentoBilhetesFuncs] (@datainicial, @datafinal, @IdsFuncs)

                            SELECT *
                              INTO #horariophextra
                              FROM dbo.FnGethorariophextra()


                            SELECT i.*,
                                   IIF(i.TotalIntervaloPrevL = '--:--','00:00',i.TotalIntervaloPrevL) TotalIntervaloPrev
                              FROM (
                                SELECT *,
                                        0 TotalIntervalo,
                                        dbo.fnTotalTempoIntervalo(isnull(entrada_1normal,t.entrada_1flexivel), isnull(entrada_2normal,t.entrada_2flexivel), isnull(entrada_3normal,t.entrada_3flexivel), isnull(entrada_4normal,t.entrada_4flexivel), '--:--', '--:--', '--:--', '--:--', isnull(saida_1normal,t.saida_1flexivel), isnull(saida_2normal,t.saida_2flexivel), isnull(saida_3normal,t.saida_3flexivel), isnull(saida_4normal,t.saida_4flexivel), '--:--', '--:--', '--:--') TotalIntervaloPrevL,
                                        0 TotalHorasAlmoco,
                                        '' ObservacaoInconsistencia
                                    FROM (
			                            SELECT marcacao.id,
                                               marcacao.idhorario,
                                               ISNULL(marcacao.legenda, ' ') AS legenda,
                                               marcacao.data,
                                               marcacao.dia,
                                               ISNULL(marcacao.entrada_1,'--:--') entrada_1,
                                               ISNULL(marcacao.entrada_2,'--:--') entrada_2,
                                               ISNULL(marcacao.entrada_3,'--:--') entrada_3,
                                               ISNULL(marcacao.entrada_4,'--:--') entrada_4,
                                               ISNULL(marcacao.entrada_5,'--:--') entrada_5,
                                               ISNULL(marcacao.entrada_6,'--:--') entrada_6,
                                               ISNULL(marcacao.entrada_7,'--:--') entrada_7,
                                               ISNULL(marcacao.entrada_8,'--:--') entrada_8,
                                               ISNULL(marcacao.saida_1,'--:--') saida_1,
                                               ISNULL(marcacao.saida_2,'--:--') saida_2,
                                               ISNULL(marcacao.saida_3,'--:--') saida_3,
                                               ISNULL(marcacao.saida_4,'--:--') saida_4,
                                               ISNULL(marcacao.saida_5,'--:--') saida_5,
                                               ISNULL(marcacao.saida_6,'--:--') saida_6,
                                               ISNULL(marcacao.saida_7,'--:--') saida_7,
                                               ISNULL(marcacao.saida_8,'--:--') saida_8,
                                               ISNULL(marcacao.horastrabalhadas,'--:--') horastrabalhadas,
                                               ISNULL(marcacao.horasextrasdiurna,'--:--') horasextrasdiurna,
                                               ISNULL(marcacao.horasfaltas,'--:--') horasfaltas,
                                               marcacao.entradaextra,
                                               marcacao.saidaextra,
                                               ISNULL(marcacao.horastrabalhadasnoturnas,'--:--') AS horastrabalhadasnoturnas,
                                               ISNULL(marcacao.horasextranoturna,'--:--') AS horasextranoturna,
                                               ISNULL(marcacao.horasfaltanoturna,'--:--') AS horasfaltanoturna,
                                               marcacao.ocorrencia,
                                               funcionario.dscodigo,
                                               funcionario.nome AS funcionario,
                                               funcionario.matricula,
                                               funcionario.dataadmissao,
                                               funcionario.codigofolha,
                                               funcao.descricao AS funcao,
                                               funcionario.pis,
                                               departamento.descricao AS departamento,
                                               empresa.nome AS empresa,
                                               CASE WHEN ISNULL(empresa.cnpj, '') <> '' THEN empresa.cnpj ELSE empresa.cpf END AS cnpj_cpf,
                                               empresa.endereco,
                                               empresa.cidade,
                                               empresa.estado,
                                               parametros.thoraextra,
                                               parametros.thorafalta,
                                               ISNULL(marcacao.valordsr, '--:--') AS valordsr,
                                               funcionario.idempresa,
                                               funcionario.iddepartamento,
                                               funcionario.idfuncao,
                                               marcacao.idfuncionario,
 		                                       isnull(tb.tratent_1,'') tratent_1,
 		                                       isnull(tb.tratent_2,'') tratent_2,
		                                       isnull(tb.tratent_3,'') tratent_3,
		                                       isnull(tb.tratent_4,'') tratent_4,
		                                       isnull(tb.tratent_5,'') tratent_5,
		                                       isnull(tb.tratent_6,'') tratent_6,
		                                       isnull(tb.tratent_7,'') tratent_7,
		                                       isnull(tb.tratent_8,'') tratent_8,
		                                       isnull(tb.tratsai_1,'') tratsai_1,
		                                       isnull(tb.tratsai_2,'') tratsai_2,
		                                       isnull(tb.tratsai_3,'') tratsai_3,
		                                       isnull(tb.tratsai_4,'') tratsai_4,
		                                       isnull(tb.tratsai_5,'') tratsai_5,
		                                       isnull(tb.tratsai_6,'') tratsai_6,
		                                       isnull(tb.tratsai_7,'') tratsai_7,
		                                       isnull(tb.tratsai_8,'') tratsai_8,
		                                       ISNULL(horario.tipohorario, 0) AS tipohorario,
                                               horario.considerasabadosemana,
                                               horario.consideradomingosemana,
                                               horario.tipoacumulo,
                                               hphe.percentualextra50,
                                               hphe.quantidadeextra50,
                                               hphe.percentualextra60,
                                               hphe.quantidadeextra60,
                                               hphe.percentualextra70,
                                               hphe.quantidadeextra70,
                                               hphe.percentualextra80,
                                               hphe.quantidadeextra80,
                                               hphe.percentualextra90,
                                               hphe.quantidadeextra90,
                                               hphe.percentualextra100,
                                               hphe.quantidadeextra100,
                                               hphe.percentualextrasab,
                                               hphe.quantidadeextrasab,
                                               hphe.percentualextradom,
                                               hphe.quantidadeextradom,
                                               hphe.percentualextrafer,
                                               hphe.quantidadeextrafer,
                                               hphe.percentualextrafol,
                                               hphe.quantidadeextrafol,
                                               horariodetalhenormal.totaltrabalhadadiurna AS chdiurnanormal,
                                               horariodetalhenormal.totaltrabalhadanoturna AS chnoturnanormal,
                                               horariodetalhenormal.flagfolga AS flagfolganormal,
                                               horariodetalhenormal.entrada_1 AS entrada_1normal,
                                               horariodetalhenormal.entrada_2 AS entrada_2normal,
                                               horariodetalhenormal.entrada_3 AS entrada_3normal,
                                               horariodetalhenormal.entrada_4 AS entrada_4normal,
                                               horariodetalhenormal.saida_1 AS saida_1normal,
                                               horariodetalhenormal.saida_2 AS saida_2normal,
                                               horariodetalhenormal.saida_3 AS saida_3normal,
                                               horariodetalhenormal.saida_4 AS saida_4normal,
                                               horariodetalhenormal.cargahorariamista AS cargamistanormal,
                                               horariodetalheflexivel.entrada_1 AS entrada_1flexivel,
                                               horariodetalheflexivel.entrada_2 AS entrada_2flexivel,
                                               horariodetalheflexivel.entrada_3 AS entrada_3flexivel,
                                               horariodetalheflexivel.entrada_4 AS entrada_4flexivel,
                                               horariodetalheflexivel.saida_1 AS saida_1flexivel,
                                               horariodetalheflexivel.saida_2 AS saida_2flexivel,
                                               horariodetalheflexivel.saida_3 AS saida_3flexivel,
                                               horariodetalheflexivel.saida_4 AS saida_4flexivel,
                                               horariodetalheflexivel.cargahorariamista AS cargamistaflexivel,
                                               horariodetalheflexivel.totaltrabalhadadiurna AS chdiurnaflexivel,
                                               horariodetalheflexivel.totaltrabalhadanoturna AS chnoturnaflexivel,
                                               horariodetalheflexivel.flagfolga AS flagfolgaflexivel,
                                               ISNULL(marcacao.bancohorascre,'---:--') AS bancohorascre,
                                               ISNULL(marcacao.bancohorasdeb,'---:--') bancohorasdeb,
                                               ISNULL(marcacao.folga, 0) AS folga,
                                               ISNULL(marcacao.chave, '') AS chave,
                                               horario.obs AS observacao,
                                               funcionario.campoobservacao AS observacaofunc,
                                               parametros.imprimeobservacao,
                                               parametros.campoobservacao,
                                               ISNULL(marcacao.exphorasextranoturna,'--:--') exphorasextranoturna,
                                               hphe.percextraprimeiro1,
                                               hphe.tipoacumulo1,
                                               hphe.percextraprimeiro2,
                                               hphe.tipoacumulo2,
                                               hphe.percextraprimeiro3,
                                               hphe.tipoacumulo3,
                                               hphe.percextraprimeiro4,
                                               hphe.tipoacumulo4,
                                               hphe.percextraprimeiro5,
                                               hphe.tipoacumulo5,
                                               hphe.percextraprimeiro6,
                                               hphe.tipoacumulo6,
                                               hphe.percextraprimeiro7,
                                               hphe.tipoacumulo7,
                                               hphe.percextraprimeiro8,
                                               hphe.tipoacumulo8,
                                               hphe.percextraprimeiro9,
                                               hphe.tipoacumulo9,
                                               hphe.percextraprimeiro10,
                                               hphe.tipoacumulo10,
                                               departamento.codigo as codigoDepartamento,
                                               horario.LimiteInterjornada,
                                               horario.limitehorastrabalhadasdia LimiteHorasTrabalhadasDia,
                                               marcacao.totalHorasTrabalhadas TotalHorasTrabalhadas,
                                               horario.limiteminimohorasalmoco LimiteMinimoHorasAlmoco, 
                                               horario.habilitaperiodo01, 
	                                           horario.dias_cafe_1, 
	                                           horario.dias_cafe_2, 
	                                           horario.dias_cafe_3, 
	                                           horario.dias_cafe_4, 
	                                           horario.dias_cafe_5, 
	                                           horario.dias_cafe_6, 
	                                           horario.dias_cafe_7,
                                               marcacao.naoconsiderarcafe,
                                               marcacao.Interjornada,
                                               marcacao.InItinereHrsDentroJornada,
                                               marcacao.InItinerePercDentroJornada,
                                               marcacao.InItinereHrsForaJornada,
                                               marcacao.InItinerePercForaJornada,
                                               marcacao.LegendasConcatenadas,
                                               marcacao.AdicionalNoturno,
                                               parametros.PercAdicNoturno,
											   hphe.percentualextraNoturna50,
                                               hphe.quantidadeextraNoturna50,
                                               hphe.percentualextraNoturna60,
                                               hphe.quantidadeextraNoturna60,
                                               hphe.percentualextraNoturna70,
                                               hphe.quantidadeextraNoturna70,
                                               hphe.percentualextraNoturna80,
                                               hphe.quantidadeextraNoturna80,
                                               hphe.percentualextraNoturna90,
                                               hphe.quantidadeextraNoturna90,
                                               hphe.percentualextraNoturna100,
                                               hphe.quantidadeextraNoturna100,
                                               hphe.percentualextraNoturnasab,
                                               hphe.quantidadeextraNoturnasab,
                                               hphe.percentualextraNoturnadom,
                                               hphe.quantidadeextraNoturnadom,
                                               hphe.percentualextraNoturnafer,
                                               hphe.quantidadeextraNoturnafer,
                                               hphe.percentualextraNoturnafol,
                                               hphe.quantidadeextraNoturnafol,
											   hphe.percextraprimeiroNoturna1,
                                               hphe.percextraprimeiroNoturna2,
                                               hphe.percextraprimeiroNoturna3,
                                               hphe.percextraprimeiroNoturna4,
                                               hphe.percextraprimeiroNoturna5,
                                               hphe.percextraprimeiroNoturna6,
                                               hphe.percextraprimeiroNoturna7,
                                               hphe.percextraprimeiroNoturna8,
                                               hphe.percextraprimeiroNoturna9,
                                               hphe.percextraprimeiroNoturna10,
											   horario.SeparaExtraNoturnaPercentual,
                                                ISNULL(horaExtraInterjornada, '--:--') AS horaExtraInterjornada
				                                , feriado.id idferiado
				                                , feriado.Parcial AS FeriadoParcial 
				                                , feriado.HoraInicio AS FeriadoParcialInicio 
				                                , feriado.HoraFim AS FeriadoParcialFim
                                                , parametros.inicioadnoturno AS inicioAdNoturno
				                                , parametros.fimadnoturno AS fimAdNoturno
                                                , pe.RazaoSocial AS PessoaSupervisor
                                                , marcacao.idjornadasubstituir
                                                , (contrato.codigocontrato +  ' | ' + contrato.descricaocontrato) AS contrato
							                    , horario.horasnormais
                                                , horario.marcacargahorariamista
                                                , jors.entrada_1 entrada_1Substituido
                                                , jors.entrada_2 entrada_2Substituido
                                                , jors.entrada_3 entrada_3Substituido
                                                , jors.entrada_4 entrada_4Substituido
                                                , jors.saida_1 saida_1Substituido
                                                , jors.saida_2 saida_2Substituido
                                                , jors.saida_3 saida_3Substituido
                                                , jors.saida_4 saida_4Substituido
                                                , parametros.toleranciaAdicionalNoturno
                                                , marcacao.SaldoBH
                                          FROM marcacao AS marcacao (NOLOCK)
                                         INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario
                                         INNER JOIN horario     ON horario.id = marcacao.idhorario
                                         INNER JOIN parametros  ON parametros.id = horario.idparametro
                                         INNER JOIN funcao      ON funcao.id = funcionario.idfuncao
                                         INNER JOIN departamento ON departamento.id = funcionario.iddepartamento
                                         INNER JOIN empresa ON empresa.id = funcionario.idempresa
                                          LEFT JOIN #tratamentos as Tb on Tb.dscodigo = marcacao.dscodigo AND marcacao.data = Tb.data
                                         INNER JOIN #horariophextra as hphe on hphe.idhorario = marcacao.idhorario
                                          LEFT JOIN horariodetalhe AS horariodetalhenormal ON horariodetalhenormal.idhorario = marcacao.idhorario AND horario.tipohorario = 1 AND horariodetalhenormal.dia = (CASE WHEN (DATEPART(WEEKDAY, marcacao.data) - 1) = 0 THEN 7 ELSE (DATEPART(WEEKDAY, marcacao.data) - 1) END)
                                          LEFT JOIN horariodetalhe AS horariodetalheflexivel ON horariodetalheflexivel.idhorario = marcacao.idhorario AND horario.tipohorario = 2 AND horariodetalheflexivel.data = marcacao.data
                                          LEFT JOIN pessoa pe ON pe.id = funcionario.IdPessoaSupervisor
                                            
                                         LEFT JOIN contratofuncionario ON contratofuncionario.idfuncionario = funcionario.id AND contratofuncionario.excluido = 0
                                         LEFT JOIN contrato ON contrato.id = contratofuncionario.idcontrato 



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
                                                LEFT JOIN JornadaSubstituir js on marcacao.IdJornadaSubstituir = js.id
                                                LEFT JOIN jornada jors on jors.id = js.idjornadapara
                                         WHERE 
                                            --funcionario.funcionarioativo = 1 AND 
                                            ISNULL(funcionario.excluido, 0) = 0 AND 
                                            marcacao.data BETWEEN @datainicial AND @datafinal AND 
                                            marcacao.idfuncionario IN (SELECT identificador FROM @IdsFuncs) AND 
                                            (@normalflexivel = 0 or horario.tipohorario = @normalflexivel)
			                             ) t 
	                               ) i 
                               WHERE 1 = 1  ";



            DataTable dt = new DataTable();


            if (!string.IsNullOrEmpty(filtro))
            {
                aux += filtro;
            }

            if (ordenaDeptoFuncionario)
            {
                aux += " ORDER BY codigoDepartamento, funcionario, data";
            }
            else
            {
                aux += " ORDER BY empresa, funcionario, data";
            }


            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        /// <summary>
        /// retorna o cartao ponto da manutencao diaria
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="departamentos"></param>
        /// <param name="tipo">0 = Empresa; 1 = Departamento</param>
        /// <returns></returns>
        public DataTable GetCartaoPontoDiaria(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, int tipo)
        {
            string aux = "   SELECT    marcacao.id, marcacao.idhorario "
                       + ", ISNULL(marcacao.legenda, ' ') AS legenda "
                       + ", marcacao.data, marcacao.dia "
                        + " , CASE WHEN marcacao.entrada_1 IS NULL THEN '--:--' ELSE marcacao.entrada_1 END AS entrada_1 "
                        + " , CASE WHEN marcacao.entrada_2 IS NULL THEN '--:--' ELSE marcacao.entrada_2 END AS entrada_2 "
                        + " , CASE WHEN marcacao.entrada_3 IS NULL THEN '--:--' ELSE marcacao.entrada_3 END AS entrada_3 "
                        + " , CASE WHEN marcacao.entrada_4 IS NULL THEN '--:--' ELSE marcacao.entrada_4 END AS entrada_4 "
                        + " , CASE WHEN marcacao.entrada_5 IS NULL THEN '--:--' ELSE marcacao.entrada_5 END AS entrada_5 "
                        + " , CASE WHEN marcacao.entrada_6 IS NULL THEN '--:--' ELSE marcacao.entrada_6 END AS entrada_6 "
                        + " , CASE WHEN marcacao.entrada_7 IS NULL THEN '--:--' ELSE marcacao.entrada_7 END AS entrada_7 "
                        + " , CASE WHEN marcacao.entrada_8 IS NULL THEN '--:--' ELSE marcacao.entrada_8 END AS entrada_8 "
                        + " , CASE WHEN marcacao.saida_1 IS NULL THEN '--:--' ELSE marcacao.saida_1 END AS saida_1 "
                        + " , CASE WHEN marcacao.saida_2 IS NULL THEN '--:--' ELSE marcacao.saida_2 END AS saida_2 "
                        + " , CASE WHEN marcacao.saida_3 IS NULL THEN '--:--' ELSE marcacao.saida_3 END AS saida_3 "
                        + " , CASE WHEN marcacao.saida_4 IS NULL THEN '--:--' ELSE marcacao.saida_4 END AS saida_4 "
                        + " , CASE WHEN marcacao.saida_5 IS NULL THEN '--:--' ELSE marcacao.saida_5 END AS saida_5 "
                        + " , CASE WHEN marcacao.saida_6 IS NULL THEN '--:--' ELSE marcacao.saida_6 END AS saida_6 "
                        + " , CASE WHEN marcacao.saida_7 IS NULL THEN '--:--' ELSE marcacao.saida_7 END AS saida_7 "
                        + " , CASE WHEN marcacao.saida_8 IS NULL THEN '--:--' ELSE marcacao.saida_8 END AS saida_8 "
                        + " , CASE WHEN marcacao.horastrabalhadas IS NULL THEN '--:--' ELSE marcacao.horastrabalhadas END AS horastrabalhadas "
                        + " , CASE WHEN marcacao.horasextrasdiurna IS NULL THEN '--:--' ELSE marcacao.horasextrasdiurna END AS horasextrasdiurna "
                        + " , CASE WHEN marcacao.horasfaltas IS NULL THEN '--:--' ELSE marcacao.horasfaltas END AS horasfaltas "
                        + " , marcacao.entradaextra "
                        + " , marcacao.saidaextra "
                        + " , CASE WHEN marcacao.horastrabalhadasnoturnas IS NULL THEN '--:--' ELSE marcacao.horastrabalhadasnoturnas END AS horastrabalhadasnoturnas "
                        + " , CASE WHEN marcacao.horasextranoturna IS NULL THEN '--:--' ELSE marcacao.horasextranoturna END AS horasextranoturna "
                        + " , CASE WHEN marcacao.horasfaltanoturna IS NULL THEN '--:--' ELSE marcacao.horasfaltanoturna END AS horasfaltanoturna "
                        + " , marcacao.ocorrencia "
                        + " , funcionario.dscodigo "
                        + " , funcionario.nome AS funcionario "
                        + " , funcionario.matricula "
                        + " , funcionario.dataadmissao "
                        + " , funcionario.codigofolha "
                        + " , funcao.descricao AS funcao "
                        + " , departamento.descricao AS departamento "
                        + " , empresa.nome AS empresa "
                        + " , case when ISNULL(empresa.cnpj, '') <> '' then empresa.cnpj else empresa.cpf end AS cnpj_cpf "
                        + " , empresa.endereco "
                        + " , empresa.cidade  "
                        + " , empresa.estado "
                        + " , parametros.thoraextra "
                        + " , parametros.thorafalta  "
                        + " , ISNULL(marcacao.valordsr, '--:--') AS valordsr "
                        + " , funcionario.idempresa "
                        + " , funcionario.iddepartamento "
                        + " , funcionario.idfuncao "
                        + " , marcacao.idfuncionario "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 1),'') AS tratent_1 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 2),'') AS tratent_2 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 3),'') AS tratent_3 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 4),'') AS tratent_4 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 5),'') AS tratent_5 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 6),'') AS tratent_6 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 7),'') AS tratent_7 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'E' and a.posicao = 8),'') AS tratent_8 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 1),'') AS tratsai_1 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 2),'') AS tratsai_2 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 3),'') AS tratsai_3 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 4),'') AS tratsai_4 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 5),'') AS tratsai_5 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 6),'') AS tratsai_6 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 7),'') AS tratsai_7 "
                        + " , ISNULL((SELECT TOP 1 ISNULL(a.ocorrencia,'') FROM bilhetesimp a where a.dscodigo = marcacao.dscodigo and a.mar_data = marcacao.data and a.ent_sai = 'S' and a.posicao = 8),'') AS tratsai_8 "
                        + " , ISNULL(horario.tipohorario, 0) AS tipohorario "
                        + " , marcacao.bancohorascre AS bancohorascre "
                        + " , marcacao.bancohorasdeb AS bancohorasdeb "
                        + " , ISNULL(marcacao.folga, 0) AS folga "
                        + " , ISNULL(marcacao.chave, '') AS chave "
                        + " FROM marcacao AS marcacao "
                        + " INNER JOIN funcionario ON funcionario.id = marcacao.idfuncionario "
                        + " INNER JOIN horario ON horario.id = marcacao.idhorario "
                        + " INNER JOIN parametros ON parametros.id = horario.idparametro "
                        + " INNER JOIN funcao ON funcao.id = funcionario.idfuncao "
                        + " INNER JOIN departamento ON departamento.id = funcionario.iddepartamento "
                        + " INNER JOIN empresa ON empresa.id = funcionario.idempresa "
                        + " WHERE funcionario.funcionarioativo = 1 AND ISNULL(funcionario.excluido, 0) = 0 ";

            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;

            DataTable dt = new DataTable();

            aux += " AND marcacao.data >= @datainicial AND marcacao.data <= @datafinal ";

            switch (tipo)
            {
                case 0:
                    aux += " AND funcionario.idempresa IN " + empresas;
                    break;
                case 1:
                    aux += " AND funcionario.iddepartamento IN " + departamentos;
                    break;
                default:
                    break;
            }

           
           
            string permissao = PermissaoUsuarioFuncionario(UsuarioLogado, aux, "funcionario.idempresa", "funcionario.id", null);
            if (!string.IsNullOrWhiteSpace(permissao))
            {

                aux += permissao ;
            }
            else
            {
                aux += GetWhereSelectAll();
            }
            
           aux += " ORDER BY  funcionario.nome ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = empresa.id) > 0 ";
            }
            return "";
        }

        #endregion

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        protected override SqlParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            throw new NotImplementedException();
        }
    }
}
