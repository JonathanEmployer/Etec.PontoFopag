using AutoMapper;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// Controler para retornar uma lista de marcações contendo os totalizadores de horas
    /// </summary>
    public class MarcacaoController : ExtendedApiController
    {
        /// <summary>
        /// Método responsável por retornar a lista de marcações contendo os totalizadores de horas 
        /// </summary>
        /// <param name="CPF">Parâmetro informado para informar o CPF do funcionário para a consulta</param>
        /// <param name="Mês">Parâmetro informado para determinar o mês da consulta</param>
        /// <param name="Ano">Parâmetro informado para determinar o ano da consulta</param>
        /// <param name="Matrícula">Parâmetro informado informar a matrícula do funcionário para a consulta</param>

        /// <returns>Lista de marcações com totalizadores de horas</returns>
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage Marcacoes(string CPF, int Mes, int Ano, string Matricula)
        {
            RetornoErro retErro = new RetornoErro();

            BLL.Justificativa bllJust = new BLL.Justificativa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            CPF = CPF.Replace("-", "").Replace(".", "");
            Int64 CPFint = Convert.ToInt64(CPF);
            DateTime datainicio;
            DateTime datafim;
            int diafechamentoinicial;
            int diafechamentofinal;
            Modelo.PeriodoFechamento periodofechamento = new PeriodoFechamento();
            Modelo.ClassificacaoHorasExtras classificacaohorasextras = new Modelo.ClassificacaoHorasExtras();
            BLL.ClassificacaoHorasExtras bllclasshe = new BLL.ClassificacaoHorasExtras(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    Funcionario func = bllFuncionario.GetFuncionarioPorCpfeMatricula(CPFint, Matricula);
                    if (func == null || func.Id == null)
                    {
                        retErro.erroGeral = "Funcionário não Encontrado - Combinação CPF e Matrícula não encontrada";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }

                    //Busca o período de fechamento
                    BLL.Contrato bllcontrato = new BLL.Contrato(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    BLL.Empresa bllempresa = new BLL.Empresa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Contrato> contratosfunc = bllcontrato.ContratosPorFuncionario(func.Id);
                    Modelo.Contrato contrato = new Modelo.Contrato();
                    contrato = contratosfunc.Where(x => x.DiaFechamentoInicial != 0).FirstOrDefault();

                    //Por contrato
                    if (contrato != null)
                    {
                        diafechamentoinicial = contrato.DiaFechamentoInicial;
                        diafechamentofinal = contrato.DiaFechamentoFinal;

                        BLL.cwkFuncoes.PeriodoFechamentoPorMes(Mes, Ano, diafechamentoinicial, diafechamentofinal, out datainicio, out datafim);
                        periodofechamento.DataFechamentoInicial = datainicio;
                        periodofechamento.DataFechamentoFinal = datafim;
                    }
                    else
                    {
                        //Por empresa
                        Modelo.Empresa empresa = bllempresa.LoadObject(func.Idempresa);
                        diafechamentoinicial = empresa.DiaFechamentoInicial;
                        diafechamentofinal = empresa.DiaFechamentoFinal;
                        if (diafechamentoinicial == 0 || diafechamentoinicial == null)
                        {
                            //Por Geral
                            BLL.Parametros bllparametros = new BLL.Parametros(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                            Modelo.Parametros parametro = new Modelo.Parametros();
                            parametro = bllparametros.LoadPrimeiro();
                            diafechamentoinicial = parametro.DiaFechamentoInicial;
                            diafechamentofinal = parametro.DiaFechamentoFinal;
                            if (diafechamentoinicial == 0 || diafechamentoinicial == null)
                            {
                                diafechamentoinicial = 1;
                                diafechamentofinal = DateTime.DaysInMonth(Ano, Mes);
                            }
                        }

                        BLL.cwkFuncoes.PeriodoFechamentoPorMes(Mes, Ano, diafechamentoinicial, diafechamentofinal, out datainicio, out datafim);
                        periodofechamento.DataFechamentoInicial = datainicio;
                        periodofechamento.DataFechamentoFinal = datafim;
                    }

                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Modelo.MarcacaoLista> ListaMarcacao = new List<Modelo.MarcacaoLista>();
                    ListaMarcacao = bllMarcacao.GetMarcacaoListaPorFuncionario(func.Id, Convert.ToDateTime(periodofechamento.DataFechamentoInicial), Convert.ToDateTime(periodofechamento.DataFechamentoFinal));

                    BLL.BilhetesImp bllBilhetes = new BLL.BilhetesImp(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<BilhetesImp> bilhetes = bllBilhetes.GetBilhetesFuncPeriodo(func.Dscodigo, periodofechamento.DataFechamentoInicial, periodofechamento.DataFechamentoFinal);

                    Models.EspelhoPonto EspelhoPonto = new Models.EspelhoPonto();

                    BLL.ParametroPainelRH bllParametroPainelRH = new BLL.ParametroPainelRH(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    Modelo.ParametroPainelRH paramPainel = bllParametroPainelRH.GetAllList().FirstOrDefault();

                    EspelhoPonto.PermiteAprovarMarcacaoIncorreta = paramPainel == null ? true : paramPainel.PermiteAprovarMarcacaoImpar;

                    List<Models.Marcacao> ListaMarcacoesFunc = new List<Models.Marcacao>();


                    EspelhoPonto.ListaMarcacao = ListaMarcacoesFunc;

                    List<int> lfunc = new List<int>();
                    lfunc.Add(func.Id);
                    IList<Modelo.Proxy.pxyAbonosPorMarcacao> afastamentos = bllAfastamento.GetAbonosPorMarcacoes(lfunc, Convert.ToDateTime(periodofechamento.DataFechamentoInicial), Convert.ToDateTime(periodofechamento.DataFechamentoFinal));

                    BLL.Ocorrencia bllOco = new BLL.Ocorrencia(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Modelo.Ocorrencia> ocorrencias = bllOco.GetAllList(false);

                    var classificacoesPeriodo = bllclasshe.GetClassificacoesMarcacao(ListaMarcacao.Select(s => s.Id).Distinct().ToList());
                    foreach (var item in ListaMarcacao)
                    {
                        Models.Marcacao marc = new Models.Marcacao();
                        marc.Bancohorascre = item.Bancohorascre;
                        marc.Bancohorasdeb = item.Bancohorasdeb;
                        marc.Data = item.Data;
                        marc.Dia = item.Dia;
                        marc.Entrada_1 = item.Entrada_1;
                        marc.Entrada_2 = item.Entrada_2;
                        marc.Entrada_3 = item.Entrada_3;
                        marc.Entrada_4 = item.Entrada_4;
                        marc.Entradaextra = item.Entradaextra;
                        marc.Funcionario = item.Funcionario;
                        marc.Horasextranoturna = item.Horasextranoturna;
                        marc.Horasextrasdiurna = item.Horasextrasdiurna;
                        marc.Horasfaltanoturna = item.Horasfaltanoturna;
                        marc.Horasfaltas = item.Horasfaltas;
                        marc.Horastrabalhadas = item.Horastrabalhadas;
                        marc.Horastrabalhadasnoturnas = item.Horastrabalhadasnoturnas;
                        marc.Id = item.Id;
                        marc.IdFuncionario = item.IdFuncionario;
                        marc.Legenda = item.Legenda;
                        marc.Ocorrencia = item.Ocorrencia;
                        marc.Saida_1 = item.Saida_1;
                        marc.Saida_2 = item.Saida_2;
                        marc.Saida_3 = item.Saida_3;
                        marc.Saida_4 = item.Saida_4;
                        marc.Saidaextra = item.Saidaextra;
                        marc.IdFuncionario = func.Id;
                        marc.IdDocumentoWorkflow = item.IdDocumentoWorkflow;
                        if (item.idFechamentoPonto != null && item.idFechamentoPonto != 0)
                        {
                            marc.PontoFechado = true;
                        }
                        else
                        {
                            marc.PontoFechado = false;
                        }
                        if (item.idFechamentoBH != null && item.idFechamentoBH != 0)
                        {
                            marc.BancoHorasFechado = true;
                        }
                        else
                        {
                            marc.BancoHorasFechado = false;
                        }


                        marc.Classificacoes = classificacoesPeriodo.Where(w => w.IdMarcacao == marc.Id).ToList();
                        if (marc.Classificacoes.FirstOrDefault().NaoClassificadasMin > 0)
                        {
                            marc.ClassificarHorasExtras = true;
                        }
                        else
                        {
                            marc.ClassificarHorasExtras = false;
                        }
                        Modelo.Proxy.pxyClassHorasExtrasMarcacao classhe = new Modelo.Proxy.pxyClassHorasExtrasMarcacao();
                        if (marc.Classificacoes != null && marc.Classificacoes.Count > 0)
                        {
                            classhe = marc.Classificacoes.Where(x => x.Tipo == 1).FirstOrDefault();
                            if (classhe != null)
                            {
                                marc.ClassificacaoDescricao = classhe.ClassificacaoDescricao;
                                marc.ClassificacaoId = classhe.IdClassificacao;
                            }
                        }

                        marc.Afastamento = GetAfastamento(afastamentos, ocorrencias, item);
                        marc.IdJornada = item.IdJornada;
                        marc.MarcacaoIncorreta = (bilhetes.Where(w => w.Mar_data == Convert.ToDateTime(marc.Data) && w.Ocorrencia != 'D' && w.Importado == 1).Count() % 2) == 0 ? false : true;

                        if (item.DataBloqueioEdicaoPnlRh != null)
                        {
                            marc.BloquearEdicaoPnlRh = true;
                        }

                        EspelhoPonto.ListaMarcacao.Add(marc);
                    }

                    //Busca Jornadas Executadas
                    BLL.Jornada bllJornada = new BLL.Jornada(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Modelo.Jornada> jornadas = bllJornada.GetAllList(EspelhoPonto.ListaMarcacao.Select(s => s.IdJornada).Distinct().ToList());
                    Mapper.CreateMap<Modelo.Jornada, Models.Jornada>();
                    List<Models.Jornada> JornadaApi = Mapper.Map<List<Models.Jornada>>(jornadas);
                    EspelhoPonto.JornadasTrabalhadas = JornadaApi;

                    if (periodofechamento.DataFechamentoFinal >= DateTime.Now.Date)
                    {
                        periodofechamento.DataFechamentoFinal = DateTime.Now.Date.AddDays(-1);
                    }

                    Modelo.TotalHoras objTotalHoras = new Modelo.TotalHoras(Convert.ToDateTime(periodofechamento.DataFechamentoInicial), Convert.ToDateTime(periodofechamento.DataFechamentoFinal));
                    var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(func, Convert.ToDateTime(periodofechamento.DataFechamentoInicial), Convert.ToDateTime(periodofechamento.DataFechamentoFinal), usuarioPontoWeb.ConnectionString, null);
                    totalizadorHoras.TotalizeHorasEBancoHoras(objTotalHoras);
                    objTotalHoras.lRateioHorasExtras = new List<RateioHorasExtras>();
                    foreach (var rateio in objTotalHoras.RateioHorasExtras)
                    {
                        RateioHorasExtras nRateio = new RateioHorasExtras();
                        nRateio.percentual = rateio.Key;
                        nRateio.diurnoMin = rateio.Value.Diurno;
                        nRateio.noturnoMin = rateio.Value.Noturno;
                        nRateio.diurno = Modelo.cwkFuncoes.ConvertMinutosHora(4, rateio.Value.Diurno);
                        nRateio.noturno = Modelo.cwkFuncoes.ConvertMinutosHora(4, rateio.Value.Noturno);
                        objTotalHoras.lRateioHorasExtras.Add(nRateio);
                    }

                    Models.TotalizadorEspelhoPonto TotalizadorEspelhoPonto = new Models.TotalizadorEspelhoPonto();
                    TotalizadorEspelhoPonto.rateioHorasExtras = new List<Models.RateioHorasExtras>();

                    TotalizadorEspelhoPonto.creditoBHPeriodo = objTotalHoras.creditoBHPeriodo;
                    TotalizadorEspelhoPonto.debitoBHPeriodo = objTotalHoras.debitoBHPeriodo;
                    TotalizadorEspelhoPonto.horasDDSR = objTotalHoras.horasDDSR;
                    TotalizadorEspelhoPonto.horasExtraDiurna = objTotalHoras.horasExtraDiurna;
                    TotalizadorEspelhoPonto.horasExtraNoturna = objTotalHoras.horasExtraNoturna;
                    TotalizadorEspelhoPonto.horasFaltaDiurna = objTotalHoras.horasFaltaDiurna;
                    TotalizadorEspelhoPonto.horasFaltaNoturna = objTotalHoras.horasFaltaNoturna;
                    TotalizadorEspelhoPonto.horasTrabDiurna = objTotalHoras.horasTrabDiurna;
                    TotalizadorEspelhoPonto.horasTrabNoturna = objTotalHoras.horasTrabNoturna;
                    TotalizadorEspelhoPonto.saldoAnteriorBH = objTotalHoras.saldoAnteriorBH;
                    TotalizadorEspelhoPonto.sinalsaldoAnteriorBH = objTotalHoras.sinalSaldoAnteriorBH.ToString();
                    TotalizadorEspelhoPonto.saldoAtualBH = objTotalHoras.saldoBHAtual;
                    TotalizadorEspelhoPonto.sinalsaldoAtualBH = objTotalHoras.sinalSaldoBHAtual.ToString();
                    TotalizadorEspelhoPonto.saldoBHPeriodo = objTotalHoras.saldoBHPeriodo;
                    TotalizadorEspelhoPonto.sinalsaldoBHPeriodo = objTotalHoras.sinalSaldoBHPeriodo.ToString();

                    foreach (var item in objTotalHoras.lRateioHorasExtras)
                    {
                        Models.RateioHorasExtras rateioHorasExtras = new Models.RateioHorasExtras();
                        rateioHorasExtras.diurno = item.diurno;
                        rateioHorasExtras.noturno = item.noturno;
                        rateioHorasExtras.percentual = item.percentual;
                        TotalizadorEspelhoPonto.rateioHorasExtras.Add(rateioHorasExtras);
                    }

                    List<int> listfunc = new List<int>();
                    listfunc.Add(func.Id);

                    EspelhoPonto.TotalizadorEspelhoPonto = TotalizadorEspelhoPonto;
                    //Criar parâmetro para setar este campo como True ou False, na Empresa.
                    BLL.Empresa bllEmpresa = new BLL.Empresa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    Modelo.Empresa Emp = bllempresa.LoadObject(func.Idempresa);
                    EspelhoPonto.ClassificarHorasExtras = Emp.PermiteClassHorasExtrasPainel;
                    EspelhoPonto.BloqueiaJustificativaForaPeriodo = Emp.BloqueiaJustificativaForaPeriodo;
                    EspelhoPonto.DtInicioJustificativa = Emp.DtInicioJustificativa;
                    EspelhoPonto.PermiteAbonoParcialPainel = Emp.PermiteAbonoParcialPainel;
                    EspelhoPonto.LimitarQtdAbono = Emp.LimitarQtdAbono;
                    try
                    {
                        int dia = Emp.DtFimJustificativa;
                        int ultimoDiaMes = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        if (dia > ultimoDiaMes)
                            EspelhoPonto.DtFimJustificativa = ultimoDiaMes;
                        else
                            EspelhoPonto.DtFimJustificativa = dia;
                    }
                    catch (Exception)
                    {
                        EspelhoPonto.DtFimJustificativa = Emp.DtFimJustificativa;
                    }

                    List<BilhetesImp> bilhetesTratados = (from bilhete in bilhetes
                                                          where
                                                              (bilhete.Ocorrencia == 'I'
                                                              || bilhete.Ocorrencia == 'D')
                                                          orderby bilhete.Data, bilhete.Hora
                                                          select bilhete).ToList();

                    EspelhoPonto.BilhetesTratados = new List<Models.Bilhetes>();
                    foreach (BilhetesImp bilhete in bilhetesTratados)
                    {
                        EspelhoPonto.BilhetesTratados.Add(new cwkWebAPIPontoWeb.Models.Bilhetes
                        {
                            DataBilhete = bilhete.Data,
                            DataMarcacao = bilhete.Mar_data != null ? bilhete.Mar_data.Value : bilhete.Data,
                            Entrada_Saida = bilhete.Ent_sai,
                            Hora = bilhete.Hora,
                            Id = bilhete.Id,
                            IdJustificativa = bilhete.Idjustificativa,
                            Motivo = bilhete.Motivo,
                            Ocorrencia = bilhete.Ocorrencia.ToString(),
                            Posicao = bilhete.Posicao,
                            Relogio = bilhete.Relogio
                        });
                    }

                    EspelhoPonto.PeriodosFerias = new List<Models.PeriodoFerias>();
                    List<Afastamento> afastamentosFerias = bllAfastamento.GetFeriasFuncionarioPeriodo(func.Id, datainicio, datafim.AddMonths(1));

                    foreach(Afastamento afastamento in afastamentosFerias)
                    {
                        Models.PeriodoFerias periodo = new Models.PeriodoFerias();
                        periodo.Inicio = afastamento.Datai.Value;
                        periodo.Fim = afastamento.Dataf == null ? DateTime.Now.AddMonths(1) : afastamento.Dataf.Value;
                        EspelhoPonto.PeriodosFerias.Add(periodo);
                    }

                    BLL.Feriado bllFeriado = new BLL.Feriado(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Modelo.Feriado> feriados = bllFeriado.GetFeriadosFuncionarioPeriodo(func.Id, datainicio, datafim.AddMonths(1));
                    EspelhoPonto.Feriados = new List<Models.Feriados>();
                    foreach (Feriado objFeriado in feriados)
                    {
                        Models.Feriados feriado = new Models.Feriados();
                        feriado.Data = objFeriado.Data.Value;
                        feriado.Descricao = objFeriado.Descricao;
                        feriado.Tipo = objFeriado.TipoFeriado;
                        EspelhoPonto.Feriados.Add(feriado);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, EspelhoPonto);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    retErro.erroGeral = e.Message;
                    return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                }
            }
            return TrataErroModelState();
        }

        private Modelo.Afastamento GetAfastamento(IList<Modelo.Proxy.pxyAbonosPorMarcacao> afastamentos, IList<Modelo.Ocorrencia> ocorrencias, MarcacaoLista item)
        {
            Modelo.Afastamento afastamento = new Modelo.Afastamento();
            Modelo.Proxy.pxyAbonosPorMarcacao afast = afastamentos.Where(w => w.IdMarcacao == item.Id).FirstOrDefault();
            if (afast != null && afast.Id > 0)
            {
                afastamento.Codigo = afast.Codigo;
                afastamento.Datai = afast.DataI;
                afastamento.Dataf = afast.DataF;
                afastamento.Id = afast.Id;
                afastamento.IdFuncionario = afast.IdFuncionario;
                afastamento.IdOcorrencia = afast.IdOcorrencia;
                afastamento.Descricao = afast.DescricaoOcorrencia;
                afastamento.Observacao = afast.Observacao;
                Modelo.Ocorrencia ocorrencia = ocorrencias.Where(oco => oco.Id == afast.IdOcorrencia).FirstOrDefault();
                if (ocorrencia != null)
                    afastamento.OcorrenciaTipoFerias = ocorrencia.OcorrenciaFerias;
            }
            return afastamento;
        }

        /// <summary>
        /// Método responsável por vincular e ou indicar o fechamento do Documento Workflow do Painel do RH no Pontofopag
        /// </summary>
        /// <param name="MarcacaoDocumento">Id da Marcação, Id do Documento Workflow e a situação do mesmo</param>
        /// <returns>Retorna HTTP Ok caso sucesso</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage CadastrarIdDocumentoWorkFlow(Models.MarcacaoDocumento MarcacaoDocumento)
        {
            try
            {
                string connectionStr = MetodosAuxiliares.Conexao();
                BLL.Marcacao BllMarcacao = new BLL.Marcacao(connectionStr);
                if (ModelState.IsValid)
                {
                    Modelo.Marcacao marc = new Marcacao();
                    marc = BllMarcacao.LoadObject(MarcacaoDocumento.idMarcacao);
                    if (marc == null || marc.Id == 0)
                    {
                       RetornoErro retErro = new RetornoErro();
                        retErro.erroGeral = "Registro não encontrado";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    marc.IdDocumentoWorkflow = MarcacaoDocumento.IdDocumentoWorkflow;
                    marc.DocumentoWorkflowAberto = MarcacaoDocumento.DocumentoWorkflowAberto;
                    BllMarcacao.ManipulaDocumentoWorkFlowPnlRH(marc.Id, MarcacaoDocumento.IdDocumentoWorkflow, MarcacaoDocumento.DocumentoWorkflowAberto);
                    return Request.CreateResponse(HttpStatusCode.OK, MarcacaoDocumento);
                }
                return TrataErroModelState();
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                RetornoErro retErro = new RetornoErro();
                retErro.erroGeral = ex.Message;
                return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
            }
        }

    }
}
