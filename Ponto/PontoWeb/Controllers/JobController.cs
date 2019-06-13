using BLL_N.IntegracaoTerceiro;
using Ionic.Zip;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Proxy;
using Modelo.Proxy.Relatorios;
using PontoWeb.Controllers;
using PontoWeb.Models;
using PontoWeb.Security;
using PontoWeb.Utils;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PontoWeb.Controllers.JobManager;
using Hangfire.Server;
using Hangfire.Console;
using Newtonsoft.Json;
using BLL_N.Hubs;
using Modelo.Relatorios;
using BLL.Util;

namespace ProgressReporting.Controllers
{
    public partial class JobController : Controller
    {
        int minprogress;
        int maxprogress;
        int valorcorrenteprogress;
        private Modelo.ProgressBar pb = new Modelo.ProgressBar();
        PxyJobReturn jobRetorno;
        PerformContext _context;
        int progress = 0;
        Job job;
        public JobController()
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
        }
        private Modelo.ProgressBar objProgressBar = new Modelo.ProgressBar();
        DataTable dtSub = new DataTable();
        String dsSub = String.Empty;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            private set { objProgressBar = value; }
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Job()
        {
            return View();
        }

        [Authorize]
        public ActionResult ArquivoRetorno(string id)
        {
            return JobManager.GetArquivoCache(id);
        }

        
        [Authorize]
        [HttpPost]
        public ActionResult GetJob()
        {
            Job jobrec = JobManager.GetJobCache();

            return Json(new
            {
                JobId = jobrec.Id,
                Progress = jobrec.Progress
            });
        }

        #region metodos progress
        #region Métodos para progress
        public void IncrementaProgressBarCMensagem(int incremento, string mensagem)
        {
            valorcorrenteprogress = valorcorrenteprogress + 1;
            decimal percProgress = ((valorcorrenteprogress * 100) / maxprogress);
            progress = Convert.ToInt32(Decimal.Round(percProgress));
            if (jobRetorno.Progress != progress || (jobRetorno.Mensagem != mensagem && !String.IsNullOrEmpty(mensagem)))
            {
                jobRetorno.Progress = progress;
                jobRetorno.Mensagem = !String.IsNullOrEmpty(mensagem) ? mensagem : jobRetorno.Mensagem;
                ReportProgress();
            }
        }

        private void ReportProgress()
        {
            if (_context != null)
            {
                jobRetorno.IdTask = _context.BackgroundJob.Id;
                _context.WriteLine("Progresso = " + JsonConvert.SerializeObject(jobRetorno));
            }
            NotificationHub.ReportarJobProgresso(jobRetorno);
        }

        public void SetaValorProgressBarCMensagem(int valor, string mensagem)
        {
            if (jobRetorno.Progress != valor || (jobRetorno.Mensagem != mensagem && !String.IsNullOrEmpty(mensagem)))
            {
                jobRetorno.Progress = valor;
                jobRetorno.Mensagem = !String.IsNullOrEmpty(mensagem) ? mensagem : jobRetorno.Mensagem;
                ReportProgress();
            }
        }

        public void IncrementaProgressBar(int incremento)
        {
            valorcorrenteprogress = valorcorrenteprogress + 1;
            decimal percProgress = ((valorcorrenteprogress * 100) / maxprogress);
            if (job != null)
            {
                job.ReportProgress(Convert.ToInt32(Decimal.Round(percProgress))); 
            }
        }

        public void SetaValorProgressBar(int valor)
        {
        }

        public void SetaMinMaxProgressBar(int min, int max)
        {
            minprogress = min;
            maxprogress = max;
            valorcorrenteprogress = 0;
        }

        public void SetaMensagem(string mensagem)
        {
            if (job != null)
            {
                job.ReportMsgProgress(mensagem); 
            }
        }

        public void IncrementaProgressBarVazio(int incremento)
        {
        }

        public void SetaValorProgressBarVazio(int valor)
        {
        }

        public void SetaMinMaxProgressBarVazio(int min, int max)
        {
        }

        public void SetaMensagemVazio(string mensagem)
        {
        }
        #endregion
        #endregion

        [Authorize]
        public Job GetRelatorioInconsistencias(Modelo.Proxy.pxyRelCartaoPonto imp, string empresas, string departamentos, string funcionarios, cw_usuario usuario)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;


            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    Modelo.UsuarioPontoWeb usuarioLogado = new UsuarioPontoWeb();
                    usuarioLogado.Login = usuario.login;
                    usuarioLogado.Nome = usuario.nome;
                    usuarioLogado.Senha = usuario.senha;
                    usuarioLogado.ConnectionString = usuario.connectionString;
                    BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(usuario.ConnectionStringDecrypt, usuarioLogado);
                    BLL.Parametros bllParametro = new BLL.Parametros(usuario.ConnectionStringDecrypt, usuarioLogado);

                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(usuario.ConnectionStringDecrypt, usuarioLogado);
                    BLL.Funcionario bllFunc = new BLL.Funcionario(usuario.ConnectionStringDecrypt, usuarioLogado);
                    BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(usuario.ConnectionStringDecrypt, usuarioLogado);

                    DataTable Dt = bllCartaoPonto.GetCartaoPontoRel(imp.InicioPeriodo,
                        imp.FimPeriodo, empresas, departamentos,
                        funcionarios, imp.TipoSelecao, imp.TipoTurno, imp.TipoSelecao, objProgressBar, false, "");

                    if (Dt.Rows.Count > 0)
                    {

                        DataTable DtIteracao;

                        try
                        {
                            DtIteracao = Dt.AsEnumerable().Where(s => (VerificaLimiteCargaHoraria(s, imp.bLimMaxHorasTrab)) ||
                                                                      (VerificaLimiteMinAlmoco(s, imp.bLimIntrajornada)) ||
                                                                      (VerificaMinInterjornada(s, imp.bMinInterjornada))
                                ).CopyToDataTable();
                        }
                        catch (Exception)
                        {
                            DtIteracao = new DataTable();
                        }

                        Dt = DtIteracao;
                    }

                    Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);
                    ReportParameter p3 = new ReportParameter("observacao",
                            objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
                    parametros.Add(p3);
                    ReportParameter p4 = new ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
                    parametros.Add(p4);

                    string nomerel = String.Empty;
                    string ds = "dsCartaoPonto_DataTable1";

                    if (imp.TipoTurno == 0)
                    {
                        nomerel = "rptInconsistencias.rdlc";
                        String nomeDoArquivo = "Relatório_de_Inconsistências_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                        if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                        {
                            JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_de_Inconsistencias(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            return;
                        }
                    }
                    else if (imp.TipoTurno == 1)
                    {
                        nomerel = "rptInconsistenciasOrdemAlt.rdlc";
                        String nomeDoArquivo = "Relatório_de_Inconsistências_Ordem_Alt_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                        if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                        {
                            JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_de_Inconsistencias(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            return;
                        }
                    }



                    ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo,
                        "Relatório_Inconsistências_" + imp.InicioPeriodo.ToShortDateString() + "_" +
                        imp.FimPeriodo.ToShortDateString() + "_" + usuario.login, Dt, ds, nomerel, parametros);

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });
            job.CompleteNovaAba = true;
            return job;
        }

        private static bool VerificaMinInterjornada(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string Interjornada = s.Field<String>("Interjornada");
                string LimiteInterjornada = s.Field<String>("LimiteInterjornada");

                retorno = ((cwkFuncoes.ConvertHorasMinuto(Interjornada) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(LimiteInterjornada) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(Interjornada) < cwkFuncoes.ConvertHorasMinuto(LimiteInterjornada)));
            }
            return retorno;
        }

        private static bool VerificaLimiteMinAlmoco(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string totalHorasTrabalhadas = s.Field<String>("TotalHorasTrabalhadas");
                string limiteminimohorasalmoco = s.Field<String>("limiteminimohorasalmoco");
                string terceirabatida = s.Field<String>("entrada_2");
                string totalHorasAlmoco = s.Field<String>("TotalHorasAlmoco");

                retorno = ((cwkFuncoes.ConvertHorasMinuto(limiteminimohorasalmoco) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas)>0 && terceirabatida !=null) &&
                    (cwkFuncoes.ConvertHorasMinuto(totalHorasAlmoco) < cwkFuncoes.ConvertHorasMinuto(limiteminimohorasalmoco)));
            }
            return retorno;
        }

        private static bool VerificaLimiteCargaHoraria(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string totalHorasTrabalhadas = s.Field<String>("TotalHorasTrabalhadas");
                string limitehorastrabalhadasdia = s.Field<String>("limitehorastrabalhadasdia");

                retorno = ((cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(limitehorastrabalhadasdia) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > cwkFuncoes.ConvertHorasMinuto(limitehorastrabalhadasdia)));
            }
            return retorno;
        }

        [Authorize]
        public Job GetRelatorioCartaoPontoV2(Modelo.Proxy.pxyRelCartaoPonto imp, cw_usuario usuario, ControllerContext controllerContext, int modeloRelatorio)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    Modelo.UsuarioPontoWeb usuarioLogado = new UsuarioPontoWeb();
                    usuarioLogado.Login = usuario.login;
                    usuarioLogado.Nome = usuario.nome;
                    usuarioLogado.Senha = usuario.senha;
                    usuarioLogado.ConnectionString = usuario.connectionString;
                    BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(usuario.ConnectionStringDecrypt, usuarioLogado);
                    IList<int> funcs = imp.idSelecionados.Split(',').Select(int.Parse).ToList();
                    IList<pxyCartaoPontoEmployer> cps = bllCartaoPonto.BuscaDadosRelatorio(funcs, imp.InicioPeriodo, imp.FimPeriodo, objProgressBar, imp.OrdemRelatorio);

                    PxyCPEMarcacao marc = new PxyCPEMarcacao();
                    List<Modelo.Utils.CartaoPontoCamposParaCustomizacao> campos = GetPropertiesCartaoPontoCustom.GetProperties(marc.GetType());
                    BLL.CamposSelecionadosRelCartaoPonto bllCamposSelecionadosRelCartaoPonto = new BLL.CamposSelecionadosRelCartaoPonto(usuario.ConnectionStringDecrypt, usuarioLogado);
                    List<Modelo.CamposSelecionadosRelCartaoPonto> camposSelionadosCartao = bllCamposSelecionadosRelCartaoPonto.GetAllList();
                    foreach (Modelo.CamposSelecionadosRelCartaoPonto item in camposSelionadosCartao)
                    {
                        item.PropriedadesCampo = campos.Where(c => c.NomePropriedade == item.PropriedadeModelo).FirstOrDefault();
                    }
                    cps.ToList().ForEach(f => f.CamposSelecionados = camposSelionadosCartao);


                    ConcurrentBag<RelatorioParts> cartoes = new ConcurrentBag<RelatorioParts>();
                    objProgressBar.setaMensagem("Criando " + cps.Count() + " cartões.");
                    objProgressBar.setaValorPB(-1);
                    int partes = cps.Count();
                    if (partes >= 3)
                    {
                        partes = cps.Count() / 3;
                    }
                    IList<List<Modelo.Proxy.pxyCartaoPontoEmployer>> cpParciais = BLL.cwkFuncoes.SplitList(cps, partes);
                    HtmlReport htmlReport = new HtmlReport();

                    Dictionary<int, string> htmls = new Dictionary<int, string>();
                    int ordem = 0;
                    foreach (List<Modelo.Proxy.pxyCartaoPontoEmployer> cpi in cpParciais)
                    {
                        string razorText = "";
                        if (modeloRelatorio == 0)
                        {
                            razorText = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Views/RelatorioCartaoPontoHTML/CartaoPontoHtml.cshtml"));
                        }
                        else
                        {
                            razorText = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Views/RelatorioCartaoPontoCustom/CartaoPontoHtml.cshtml"));
                        }

                        //string htmlText = Razor.Parse(razorText, cpi);
                        string htmlText = Engine.Razor.RunCompile(razorText, Guid.NewGuid().ToString(), null, cpi);
                        htmls.Add(ordem, htmlText);
                        ordem++;
                    }
                    FileContentResult retorno = null;
                    if (imp.TipoArquivo == "PDF")
                    {
                        objProgressBar.setaMensagem("Renderizando " + cps.Count() + " cartões.");
                        objProgressBar.setaValorPB(-1);
                        Parallel.ForEach(htmls, (ht) =>
                        {
                            RelatorioParts cpb = new RelatorioParts();
                            cpb.Parte = ht.Key;
                            byte[] buffer = htmlReport.RenderPDF(ht.Value, false, false);
                            cpb.Arquivo = buffer;
                            cartoes.Add(cpb);
                        });
                        objProgressBar.setaMensagem("Agrupando " + cps.Count() + " cartões.");
                        objProgressBar.setaValorPB(-1);
                        byte[] buffer1 = htmlReport.MergeFiles(cartoes.OrderBy(o => o.Parte).Select(s => s.Arquivo).ToList(), true, false);
                        retorno = File(buffer1, "application/PDF");
                    }
                    else
                    {
                        retorno = File(System.Text.Encoding.UTF8.GetBytes(String.Join(String.Empty, htmls.Select(s => s.Value).ToArray())), "text/html");
                    }

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    if (e.GetAllExceptionAsString().Contains("Não há paginas para imprimir."))
                    {
                        throw new Exception("Não há paginas para imprimir.");
                    }
                    else
                    {
                        throw new Exception(string.Concat(e.GetAllExceptions().ToList().Select(t => t.Message + "<br/>")));
                    }
                }
            });
            job.CompleteNovaAba = true;
            job.TipoArquivo = imp.TipoArquivo;
            return job;
        }

        public Job GetRelatorioInconsistencias(Modelo.Proxy.pxyRelCartaoPonto imp, cw_usuario usuario)
        {
            return GetRelatorioInconsistencias(imp, "", "", "(" + imp.idSelecionados + ")", usuario);
        }

        //public Job GetRelatorioCartaoPonto(Modelo.Proxy.pxyRelCartaoPonto imp, cw_usuario usuario)
        //{
        //    List<int> ids = imp.idSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).Select(s => int.Parse(s)).ToList(); // Solução para quando o sistema envia dados de cache e falta id, ficando os dados por exemplo: ,124,315,355,      Como essa lista de string é jogada direto para o select colocando entre os parenteses isso da problema, essa solução "retira" as virgulas desnecessárias
        //    return GetRelatorioCartaoPonto(imp, "", "", "(" + String.Join(",", ids) + ")", usuario);
        //}

        private static List<ReportParameter> SetaParametrosRelatorioAbsenteismo(Modelo.Proxy.pxyRelAbsenteismo imp)
        {
            List<ReportParameter> parametros;
            string tipo = String.Format("[{0}] Faltas [{1}] Atrasos [{2}] Horas Abonadas",
                (imp.bFaltas ? "X" : " "), (imp.bAtrasos ? "X" : " "), (imp.bHorasAbonadas ? "X" : " "));


            parametros = new List<ReportParameter>();
            var p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
            var p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
            var p3 = new ReportParameter("tipo", tipo);

            parametros.Add(p1);
            parametros.Add(p2);
            parametros.Add(p3);
            return parametros;
        }

        private static void GeraTotaisRelatorioAbsenteismo(IEnumerable<BLL.AbsenteismoLinha> absenteismos, out Dictionary<string, string> totaisDepartamentos, out Dictionary<string, string> totaisEmpresas, out Dictionary<string, string> totaisFuncionario)
        {
            totaisDepartamentos = (from a in absenteismos
                                   group a by a.Departamento into dep
                                   select new
                                   {
                                       QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, dep.Sum(d => d.QuantidadeHoras)),
                                       Departamento = dep.Key
                                   }).ToDictionary(k => k.Departamento, v => v.QuantidadeHoras);

            totaisEmpresas = (from a in absenteismos
                              group a by a.Empresa into emp
                              select new
                              {
                                  QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, emp.Sum(d => d.QuantidadeHoras)),
                                  Empresa = emp.Key
                              }).ToDictionary(k => k.Empresa, v => v.QuantidadeHoras);

            totaisFuncionario = (from a in absenteismos
                                 group a by a.Funcionario into func
                                 select new
                                 {
                                     QuantidadeHoras = Modelo.cwkFuncoes.ConvertMinutosHora(4, func.Sum(d => d.QuantidadeHoras)),
                                     Funcionario = func.Key
                                 }).ToDictionary(k => k.Funcionario, v => v.QuantidadeHoras);
        }

        [Authorize]
        public Job ImportacaoBilhete(IList<REP> listaReps, DateTime? dataInicial, DateTime? dataFinal, FileInfo arquivo, bool pIndividual, string dsCodFuncionario, string ConnectionString, Modelo.UsuarioPontoWeb usuarioLogado)
        {
            BLL.ImportacaoBilhetes bllImportacaoBilhetes = new BLL.ImportacaoBilhetes(ConnectionString, usuarioLogado);

            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            List<TipoBilhetes> lstTipoBilhete = MontaListaTipoBilhete(listaReps, arquivo.FullName, ConnectionString, usuarioLogado);

            string nomeArquivo = "LogImportação_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            job = JobManager.Instance.DoJobAsync(j =>
            {
                List<string> log = new List<string>();
                try
                {
                    BLL_N.ImportacaoBilhetesNova bllImp = new BLL_N.ImportacaoBilhetesNova(ConnectionString, usuarioLogado);
                    log = bllImp.ImportacaoBilhete(objProgressBar, lstTipoBilhete, arquivo.FullName, 1, pIndividual, dsCodFuncionario, dataInicial, dataFinal, usuarioLogado);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        StreamWriter sw = new StreamWriter(ms);
                        foreach (var item in log)
                            sw.WriteLine(item);
                        sw.Flush();
                        FileContentResult fcr = File(ms.ToArray(), "text/plain", nomeArquivo + ".txt");
                        JobManager.AdicionaArquivoCache(fcr, job.Id);
                    }
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        StreamWriter sw = new StreamWriter(ms);
                        sw.WriteLine(e.Message + Environment.NewLine);
                        sw.Write(e.StackTrace);
                        sw.Flush();
                        FileContentResult fcr = File(ms.ToArray(), "text/plain", nomeArquivo + ".txt");
                        JobManager.AdicionaArquivoCache(fcr, job.Id);
                    }
                }

            });
            job.CompleteNovaAba = false;
            return job;
        }

        private List<TipoBilhetes> MontaListaTipoBilhete(IList<REP> listaReps, string arquivo, string ConnectionString, Modelo.UsuarioPontoWeb usuarioLogado)
        {
            List<TipoBilhetes> retorno = new List<TipoBilhetes>();
            TipoBilhetes tpBilhete;
            BLL.TipoBilhetes blltipoBilhete = new BLL.TipoBilhetes(ConnectionString, usuarioLogado);

            foreach (var rep in listaReps)
            {
                tpBilhete = new TipoBilhetes();
                tpBilhete.Codigo = 1;
                tpBilhete.Descricao = "Coleta AFD";
                tpBilhete.Diretorio = arquivo;
                tpBilhete.FormatoBilhete = 3;
                tpBilhete.BImporta = true;
                tpBilhete.Ordem_c = 0;
                tpBilhete.Ordem_t = 0;
                tpBilhete.Dia_c = 0;
                tpBilhete.Dia_t = 0;
                tpBilhete.Mes_c = 0;
                tpBilhete.Mes_t = 0;
                tpBilhete.Ano_c = 0;
                tpBilhete.Ano_t = 0;
                tpBilhete.Hora_c = 0;
                tpBilhete.Hora_t = 0;
                tpBilhete.Minuto_c = 0;
                tpBilhete.Minuto_t = 0;
                tpBilhete.IdRep = rep.Id;

                retorno.Add(tpBilhete);

            }

            return retorno;
        }


        #region Geração Relatorios Demorados
        private ActionResult ImprimeRelatorioMultDtDsProgressBar(string tipoArquivo, string NomeArquivoFinal, DataTable Dt, DataTable Dt2, string NomeDataSet, string NomeDataSet2, string NomeArquivoRDLC, List<ReportParameter> paramsRelatorio, string subReportName, string subReportRdlcName)
        {
            try
            {
                TipoArquivo tipo;
                if (!String.IsNullOrEmpty(tipoArquivo))
                {
                    if (!tipoArquivo.ToLower().Equals("pdf"))
                    {
                        tipo = TipoArquivo.Excel;
                    }
                    else
                    {
                        tipo = TipoArquivo.PDF;
                    }
                }
                else
                {
                    tipo = TipoArquivo.PDF;
                }
                var retorno = GeraRelatorioMultiDtDs(tipo, Dt, Dt2, NomeArquivoRDLC, NomeDataSet, NomeDataSet2, NomeArquivoFinal, paramsRelatorio, subReportName, subReportRdlcName);
                return retorno;

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult ImprimeRelatorioGenericoProgressBar(string tipoArquivo, string NomeArquivoFinal, DataTable Dt, string NomeDataSet, string NomeArquivoRDLC, List<ReportParameter> paramsRelatorio)
        {
            try
            {
                TipoArquivo tipo;
                if (!String.IsNullOrEmpty(tipoArquivo))
                {
                    if (!tipoArquivo.ToLower().Equals("pdf"))
                    {
                        tipo = TipoArquivo.Excel;
                    }
                    else
                    {
                        tipo = TipoArquivo.PDF;
                    }
                }
                else
                {
                    tipo = TipoArquivo.PDF;
                }
                var retorno = GeraRelatorio(tipo, Dt, NomeArquivoRDLC, NomeDataSet, NomeArquivoFinal, paramsRelatorio);
                return retorno;

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private FileContentResult GeraRelatorio(TipoArquivo tipoArquivo, DataTable Dt, string nomerel, string ds, string nomeArquivo, List<ReportParameter> parametros)
        {
            try
            {
                if (tipoArquivo == TipoArquivo.Excel)
                {
                    return new CustomFileResult(BLL.RelatorioExcelGenerico.GeraRelatorio(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeArquivo + ".xls");
                }

                LocalReport lr = new LocalReport();
                string dll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "REL.dll");
                Assembly assembly = Assembly.LoadFrom(dll);
                Stream stream = assembly.GetManifestResourceStream("REL.Relatorios." + nomerel);
                lr.LoadReportDefinition(stream);

                ReportDataSource rd = new ReportDataSource(ds, Dt);
                lr.DataSources.Add(rd);
                lr.SetParameters(parametros);
                ReportPageSettings rps = lr.GetDefaultPageSettings();
                string reportType = tipoArquivo == TipoArquivo.PDF ? "PDF" : "Excel";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>";

                if ((rps.IsLandscape) &&
                     (rps.PaperSize.Height > rps.PaperSize.Width))
                {
                    deviceInfo = DefineFormatoPaisagem(deviceInfo);
                }
                else
                {
                    deviceInfo = DefineFormatoRetrato(deviceInfo);
                }

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                if (fileNameExtension == "TIF")
                {
                    fileNameExtension = "jpeg";
                }

                var Rel = new CustomFileResult(renderedBytes, mimeType, nomeArquivo + "." + fileNameExtension);
                Thread.Sleep(2000);
                return Rel;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        private static string DefineFormatoRetrato(string deviceInfo)
        {
            deviceInfo += "  <PageWidth>8.5in</PageWidth>" +
                         "  <PageHeight>11in</PageHeight>" +
                         "  <MarginTop>0.5in</MarginTop>" +
                         "  <MarginLeft>0.3in</MarginLeft>" +
                         "  <MarginRight>0.3in</MarginRight>" +
                         "  <MarginBottom>0.5in</MarginBottom>" +
                         "</DeviceInfo>";
            return deviceInfo;
        }

        private static string DefineFormatoPaisagem(string deviceInfo)
        {
            deviceInfo += "  <PageWidth>29.7cm</PageWidth>" +
                            "  <PageHeight>21cm</PageHeight>" +
                            "</DeviceInfo>";
            return deviceInfo;
        }

        private FileContentResult GeraRelatorioMultiDtDs(TipoArquivo tipoArquivo, DataTable Dt, DataTable DtSub, string nomerel, string ds, string DsSub, string nomeArquivo, List<ReportParameter> parametros, string subReportName, string subReportRdlcName)
        {
            try
            {
                if (tipoArquivo == TipoArquivo.Excel)
                {
                    return new CustomFileResult(BLL.RelatorioExcelGenerico.GeraRelatorio(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeArquivo + ".xls");
                }

                dtSub = DtSub;
                dsSub = DsSub;

                LocalReport lr = new LocalReport();
                string dll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "REL.dll");
                Assembly assembly = Assembly.LoadFrom(dll);
                Stream stream = assembly.GetManifestResourceStream("REL.Relatorios." + nomerel);
                lr.LoadReportDefinition(stream);

                if (!String.IsNullOrEmpty(subReportName))
                {
                    Stream streamSub = assembly.GetManifestResourceStream("REL.Relatorios." + subReportRdlcName);
                    lr.LoadSubreportDefinition(subReportName, streamSub);
                }

                ReportDataSource rd = new ReportDataSource(ds, Dt);

                lr.DataSources.Add(rd);
                lr.SetParameters(parametros);

                ReportPageSettings rps = lr.GetDefaultPageSettings();

                string reportType = tipoArquivo == TipoArquivo.PDF ? "PDF" : "Excel";
                string mimeType;
                string encoding;
                string fileNameExtension;
                string deviceInfo =

                "<DeviceInfo>" +
                "  <OutputFormat>" + reportType + "</OutputFormat>";

                if ((rps.IsLandscape) &&
                     (rps.PaperSize.Height > rps.PaperSize.Width))
                {
                    deviceInfo = DefineFormatoPaisagem(deviceInfo);
                }
                else
                {
                    deviceInfo = DefineFormatoRetrato(deviceInfo);
                }

                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;

                lr.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRel);

                renderedBytes = lr.Render(
                    reportType,
                    deviceInfo,
                    out mimeType,
                    out encoding,
                    out fileNameExtension,
                    out streams,
                    out warnings);

                if (fileNameExtension == "TIF")
                {
                    fileNameExtension = "jpeg";
                }

                var Rel = new CustomFileResult(renderedBytes, mimeType, nomeArquivo + "." + fileNameExtension);
                Thread.Sleep(2000);
                return Rel;

                //if (tipoArquivo != TipoArquivo.PDF)
                //{
                //    var Rel = File(renderedBytes, mimeType);
                //    Thread.Sleep(2000);
                //    return Rel;
                //}
                //else
                //{
                //    var Rel = File(renderedBytes, mimeType);
                //    Thread.Sleep(2000);
                //    return Rel;
                //}
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        private void RenderizaSubRel(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource(dsSub, dtSub));
        }

        #endregion

        [Authorize]
        public Job GetRelatorioOcorrencias(pxyRelOcorrencias imp, DataTable Dt, IList<bool> pegaOcorrencias, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");

                    string idsTipoEscolhido = string.Empty;

                    int agruparDepartamento = 0;
                    if (imp.bAgruparPorDepartamento)
                        agruparDepartamento = 1;

                    BLL.RelatorioOcorrenciaPontoWeb bllRelatorioOcorrencia = new BLL.RelatorioOcorrenciaPontoWeb(imp.InicioPeriodo, imp.FimPeriodo, imp.TipoSelecao, "(" + imp.idSelecionados + ")", 0, imp.TipoRelatorio, imp.bAgruparPorDepartamento, pegaOcorrencias, imp.idSelecionadosOcorrencias, imp.idSelecionadosJustificativas, ConnectionString, usuarioLogado);
                    Dt = bllRelatorioOcorrencia.GeraRelatorio();


                    string nomerel = String.Empty;
                    string texto = String.Empty;
                    string nomeDoArquivo = String.Empty;

                    switch (imp.TipoRelatorio)
                    {
                        case 0:
                            nomerel = "rptOcorrenciaPorDataFuncionario.rdlc";
                            texto = "Relatório de Ocorrências por Data/Funcionário";
                            nomeDoArquivo = "Relatório_de_Ocorrências_por_Data_Funcionário_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_de_Ocorrencias_por_Data_Funcionario(Dt), BLL.RelatorioExcelGenerico.ContentType(1), nomeDoArquivo + ".xlsx"), job.Id);
                                return;
                            }
                            break;
                        case 1:
                            nomerel = "rptOcorrenciaPorFuncionarioData.rdlc";
                            texto = "Relatório de Ocorrências por Funcionário/Data";
                            nomeDoArquivo = "Relatório_de_Ocorrências_por_Funcionário_Data_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_de_Ocorrencias_por_Funcionario_Data(Dt), BLL.RelatorioExcelGenerico.ContentType(1), nomeDoArquivo + ".xlsx"), job.Id);
                                return;
                            }
                            break;
                        case 2:
                            nomerel = "rptOcorrenciaPorFuncionarioData.rdlc";
                            texto = "Relatório de Ocorrências por Matrícula";
                            nomeDoArquivo = "Relatório_de_Ocorrências_por_Matrícula_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_de_Ocorrencias_por_Funcionario_Data(Dt), BLL.RelatorioExcelGenerico.ContentType(1), nomeDoArquivo + ".xlsx"), job.Id);
                                return;
                            }
                            break;
                    }
                    string ds = "dsOcorrencia_marcacao";

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);
                    ReportParameter p3 = new ReportParameter("nomeRelatorio", texto.ToString());
                    parametros.Add(p3);
                    ReportParameter p4 = new ReportParameter("quebraDepartamento", agruparDepartamento.ToString());
                    parametros.Add(p4);

                    ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, ds, nomerel, parametros);
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        public Job GetRelatorioAbono(pxyRelOcorrencias imp, DataTable dt, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");

                    string idsTipoEscolhido = string.Empty;

                    int agruparDepartamento = 0;
                    if (imp.bAgruparPorDepartamento)
                        agruparDepartamento = 1;

                    BLL.RelatorioAbono bllRelatorioAbono = new BLL.RelatorioAbono(imp.InicioPeriodo, imp.FimPeriodo, 2, "(" + imp.idSelecionados + ")", 0, agruparDepartamento, imp.idSelecionadosOcorrencias, ConnectionString, usuarioLogado);
                    dt = bllRelatorioAbono.GeraRelatorio();


                    string nomerel = "rptAbonoPorFuncionarioData.rdlc";
                    string texto = "Relatório de Abono por Funcionário/Data";
                    string nomeDoArquivo = "Relatório_de_Abono_por_Funcionário_Data_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();

                    string ds = "dsOcorrencia_abono";
                    if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                    {
                        JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_de_Abono_por_Funcionario_Data(dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                        return;
                    }

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);
                    ReportParameter p3 = new ReportParameter("nomeRelatorio", texto.ToString());
                    parametros.Add(p3);
                    ReportParameter p4 = new ReportParameter("quebraDepartamento", agruparDepartamento.ToString());
                    parametros.Add(p4);

                    ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, dt, ds, nomerel, parametros);
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        public Job GetRelatorioIntervalos(pxyRelIntervalos imp, DataTable Dt, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;


            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(ConnectionString, usuarioLogado);
                    BLL.Parametros bllParametro = new BLL.Parametros(ConnectionString, usuarioLogado);

                    objProgressBar.setaMensagem("Carregando dados...");

                    Dt = bllCartaoPonto.GetCartaoPontoRel(imp.InicioPeriodo, imp.FimPeriodo, "()", "()", "(" + imp.idSelecionados + ")", 2, 0, 0, objProgressBar, false, "");
                    if (Dt.Rows.Count > 0)
                    {
                        DataTable DtFiltrado = null;
                        try
                        {
                            DtFiltrado = Dt.AsEnumerable().Where(s => s.Field<String>("TotalIntervalo") != "--:--").CopyToDataTable();
                        }
                        catch (Exception)
                        {
                            DtFiltrado = new DataTable();
                        }
                        Dt = DtFiltrado;
                    }

                    Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();
                    string nomerel = "rptIntervalos.rdlc";
                    string nomeDoArquivo = "Relatório_de_Intervalos_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();

                    string ds = "dsCartaoPonto_DataTable1";

                    if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                    {
                        JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Intervalos(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                        return;
                    }

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);
                    ReportParameter p3 = new ReportParameter("observacao", objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
                    parametros.Add(p3);
                    ReportParameter p4 = new ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
                    parametros.Add(p4);

                    ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, ds, nomerel, parametros);
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }


        [Authorize]
        public Job GetRelatorioHistorico(pxyRelHistorico imp, DataTable Dt, string nomeEmpresa, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            BLL.FuncionarioHistorico bllFuncionarioHistorico = new BLL.FuncionarioHistorico(ConnectionString, usuarioLogado);

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    string nomerel = String.Empty;
                    string texto = String.Empty;
                    string nomeDoArquivo = String.Empty;
                    string ds = String.Empty;
                    List<ReportParameter> parametros = new List<ReportParameter>();

                    Dt = bllFuncionarioHistorico.LoadRelatorio(imp.InicioPeriodo, imp.FimPeriodo, imp.TipoSelecao, "", "", "(" + imp.idSelecionados + ")");

                    ReportParameter p1 = new ReportParameter("empresa", nomeEmpresa);
                    parametros.Add(p1);

                    nomerel = "rptHistorico.rdlc";
                    texto = "Relatório de Histórico Funcionário";
                    nomeDoArquivo = "Relatório_de_Histórico_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                    ds = "dsHistorico_DataTable1";

                    if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                    {
                        JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Historico(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                    }
                    else
                    {
                        ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, ds, nomerel, parametros);
                        JobManager.AdicionaArquivoCache(retorno, job.Id);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        public Job GetRelatorioEspelho(pxyRelEspelho imp, DataTable Dt, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            BLL.RelatorioEspelho bllEspelho = new BLL.RelatorioEspelho(ConnectionString, usuarioLogado);

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    string nomerel = String.Empty;
                    string subReportName = String.Empty;
                    string subReportRdlcName = String.Empty;
                    string texto = String.Empty;
                    string nomeDoArquivo = String.Empty;
                    string ds = String.Empty;
                    string ids = String.Empty;

                    List<ReportParameter> parametros = new List<ReportParameter>();

                    List<string> jornadas = new List<string>();
                    Dt = bllEspelho.GetEspelhoPontoRel(imp.InicioPeriodo, imp.FimPeriodo, "(" + imp.idSelecionados + ")", imp.TipoSelecao, objProgressBar, jornadas);
                    nomerel = "rptEspelhoPonto.rdlc";
                    subReportName = "rptJornadaEspelho";
                    subReportRdlcName = "rptJornadaEspelho.rdlc";
                    ds = "dsCartaoPonto_Espelho";
                    texto = "Relatório Espelho de Ponto Eletrônico";
                    nomeDoArquivo = "Relatório_Espelho_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();

                    if (Dt.Rows.Count == 0)
                        throw new Exception("Não existem marcações registradas no período consultado.");

                    DataTable DtJornadas = bllEspelho.GetJornadasEspelho(jornadas, imp.TipoSelecao);
                    string dsJornada = "dsCartaoPonto_Jornada";

                    ReportParameter p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);

                    ReportParameter p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);

                    if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                    {
                        JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Espelho(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                        return;
                    }

                    ActionResult retorno = ImprimeRelatorioMultDtDsProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, DtJornadas, ds, dsJornada, nomerel, parametros, subReportName, subReportRdlcName);
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        public Job GetRelatorioManutencaoDiaria(pxyRelPresenca imp, string empresas, string departamentos, string funcionarios, string ConnectionString, Modelo.UsuarioPontoWeb usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    string nomerel = String.Empty;
                    string nomeDoArquivo = String.Empty;
                    string ds = String.Empty;
                    string ids = String.Empty;
                    BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(ConnectionString, usuarioLogado);
                    DataTable Dt = bllCartaoPonto.GetCartaoPontoDiariaWeb(imp.InicioPeriodo, imp.FimPeriodo, empresas, departamentos, imp.TipoSelecao, objProgressBar);
                    nomerel = "rptManutencaoDiaria.rdlc";
                    ds = "dsCartaoPonto_DataTable1";

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("dataInicial", imp.InicioPeriodo.ToShortDateString());
                    ReportParameter p2 = new ReportParameter("dataFinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    parametros.Add(p2);

                    nomeDoArquivo = "Relatório_Manutenção_Diária_" + imp.Data.ToShortDateString();

                    ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, ds, nomerel, parametros);
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        public Job GetRelatorioManutencaoDiaria(pxyRelManutDiaria imp, string departamentos, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    string nomerel = String.Empty;
                    string nomeDoArquivo = String.Empty;
                    string ds = String.Empty;
                    string ids = String.Empty;
                    BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(ConnectionString, usuarioLogado);
                    DataTable Dt = bllCartaoPonto.GetCartaoPontoDiariaWeb(imp.Data, imp.Data, string.Empty, departamentos, imp.TipoSelecao, objProgressBar);
                    nomerel = "rptManutencaoDiaria.rdlc";
                    ds = "dsCartaoPonto_DataTable1";

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("dataInicial", imp.Data.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("dataFinal", imp.Data.ToShortDateString());
                    parametros.Add(p2);

                    nomeDoArquivo = "Relatório_Manutenção_Diária_" + imp.Data.ToShortDateString();

                    if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                    {
                        JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Manutencao_Diaria(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                        return;
                    }

                    ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, ds, nomerel, parametros);
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        public Job GetRelatorioFuncionario(pxyRelFuncionario imp, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    DateTime inicioRel = DateTime.Now;
                    string nomerel = String.Empty;
                    string nomeDoArquivo = String.Empty;
                    string ds = String.Empty;
                    string ids = String.Empty;
                    BLL.CartaoPonto bllCartaoPonto = new BLL.CartaoPonto(ConnectionString, usuarioLogado);
                    DataTable Dt = new DataTable();

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString, usuarioLogado);
                    BLL.Empresa bllEmpresa = new BLL.Empresa(ConnectionString, usuarioLogado);
                    Modelo.Empresa empPrincipal = bllEmpresa.GetEmpresaPrincipal();
                    string NomeEmpresa = empPrincipal.Nome;
                    List<int> idsFuncs = imp.idSelecionados.Split(',').Select(int.Parse).ToList();
                    switch (imp.Relatorio)
                    {
                        case "1":
                            Dt = bllFuncionario.GetOrdenadoPorNomeRel(idsFuncs);
                            ReportParameter p11 = new ReportParameter("empresa", NomeEmpresa);
                            ReportParameter p21 = new ReportParameter("ordenacao", "Nome");
                            parametros.Add(p11);
                            parametros.Add(p21);
                            nomerel = "rptFuncionarios.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionário_Por_Nome";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Nome(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        case "2":
                            Dt = bllFuncionario.GetOrdenadoPorCodigoRel(idsFuncs);
                            ReportParameter p12 = new ReportParameter("empresa", NomeEmpresa);
                            ReportParameter p22 = new ReportParameter("ordenacao", "Código");
                            parametros.Add(p12);
                            parametros.Add(p22);
                            nomerel = "rptFuncionarios.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionários_por_Código";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Codigo(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        case "3":
                            Dt = bllFuncionario.GetPorDepartamentoRel(idsFuncs);
                            ReportParameter p13 = new ReportParameter("empresa", NomeEmpresa);
                            parametros.Add(p13);
                            nomerel = "rptFuncionariosPorDepartamento.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionários_por_Departamento";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Departamento(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        case "4":
                            Dt = bllFuncionario.GetRelatorio(idsFuncs);
                            ReportParameter p14 = new ReportParameter("empresa", NomeEmpresa);
                            parametros.Add(p14);
                            nomerel = "rptFuncionariosPorEmpresa.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionários_por_Empresa";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Empresa(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        case "5":
                            Dt = bllFuncionario.GetPorHorarioRel(idsFuncs);
                            ReportParameter p15 = new ReportParameter("empresa", NomeEmpresa);
                            parametros.Add(p15);
                            nomerel = "rptFuncionariosPorHorario.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionários_por_Horário";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Por_Horario(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        case "6":
                            string Ordenacao = string.Empty;
                            if (imp.AtivoInativo == 0)
                            {
                                Ordenacao = "Ativos";
                            }
                            else
                            {
                                Ordenacao = "Inativos";
                            }
                            Dt = bllFuncionario.GetAtivosInativosRel(!Convert.ToBoolean(imp.AtivoInativo), idsFuncs);
                            ReportParameter p16 = new ReportParameter("empresa", NomeEmpresa);
                            parametros.Add(p16);
                            ReportParameter p26 = new ReportParameter("ordenacao", Ordenacao);
                            parametros.Add(p26);
                            nomerel = "rptFuncionariosAtivosInativos.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionários_Ativos-Inativos";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                nomeDoArquivo = "Relatório_Funcionários_" + Ordenacao;
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Ativo_Inativo(Dt, Ordenacao), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        case "7":
                            Dt = bllFuncionario.GetPorDataAdmissaoRel(imp.InicioPeriodo, imp.FimPeriodo, idsFuncs);
                            ReportParameter p17 = new ReportParameter("empresa", NomeEmpresa);
                            parametros.Add(p17);
                            nomerel = "rptFuncionariosAdmissao.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionários_Admitidos";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Admissao(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        case "8":
                            Dt = bllFuncionario.GetPorDataDemissaoRel(imp.InicioPeriodo, imp.FimPeriodo, idsFuncs);
                            ReportParameter p18 = new ReportParameter("empresa", NomeEmpresa);
                            parametros.Add(p18);
                            nomerel = "rptFuncionariosDemissao.rdlc";
                            ds = "dsFuncionarios_Funcionarios";
                            nomeDoArquivo = "Relatório_Funcionários_Demitidos";
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Funcionario_Demissao(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                            }
                            break;
                        default:
                            break;
                    }

                    if (imp.TipoArquivo.ToLower().Equals("pdf"))
                    {
                        ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, ds, nomerel, parametros);
                        JobManager.AdicionaArquivoCache(retorno, job.Id);
                    }

                    if (DateTime.Now.Subtract(inicioRel).TotalSeconds <= 2)
                    {
                        Thread.Sleep(2);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        internal Job GetRelatorioHoraExtra(pxyRelHoraExtra imp, string empresas, string departamentos, string funcionarios, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    string nomerel = String.Empty;
                    string nomeDoArquivo = String.Empty;
                    string ds = String.Empty;
                    string ids = String.Empty;
                    DataTable Dt = new DataTable();

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);

                    BLL.HorarioPHExtra bllPercentualHExtra = new BLL.HorarioPHExtra(ConnectionString, usuarioLogado);
                    switch (imp.TipoRelatorio)
                    {
                        case 0:

                            Dt = bllPercentualHExtra.GetHoraExtraWeb(imp.InicioPeriodo, imp.FimPeriodo, empresas, departamentos, funcionarios, imp.TipoSelecao, false, objProgressBar);
                            nomerel = "rptHExtraPorFuncionario.rdlc";
                            ds = "dsPercExtra_PercExtraFuncionario";
                            nomeDoArquivo = "Relatório_Hora_Extra_Por_Funcionário_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Hora_Extra_Por_Funcionario(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                                return;
                            }
                            break;
                        case 1:
                            Dt = bllPercentualHExtra.GetHoraExtraWeb(imp.InicioPeriodo, imp.FimPeriodo, empresas, departamentos, funcionarios, imp.TipoSelecao, true, objProgressBar);
                            nomerel = "rptHExtraPorDepartamento.rdlc";
                            ds = "dsPercExtra_PercExtraDepartamento";
                            nomeDoArquivo = "Relatório_Hora_Extra_Por_Departamento_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Hora_Extra_Por_Departamento(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                                return;
                            }
                            break;
                        case 2:
                            Dt = bllPercentualHExtra.GetPercentualHoraExtraWeb(imp.InicioPeriodo, imp.FimPeriodo, empresas, departamentos, funcionarios, imp.TipoSelecao, objProgressBar);
                            nomerel = "rptPercentualHExtra.rdlc";
                            ds = "dsPercExtra_PercExtra";
                            nomeDoArquivo = "Relatório_Hora_Extra_Por_Percentual_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                            if (!imp.TipoArquivo.ToLower().Equals("pdf"))
                            {
                                JobManager.AdicionaArquivoCache(new CustomFileResult(BLL.RelatorioExcelGenerico.Relatorio_Hora_Extra_Por_Percentual(Dt), BLL.RelatorioExcelGenerico.ContentType(0), nomeDoArquivo + ".xls"), job.Id);
                                return;
                            }
                            break;
                        default:
                            break;
                    }

                    ActionResult retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, Dt, ds, nomerel, parametros);
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        internal Job GetRelatorioBancoHoras(pxyRelBancoHoras imp, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    string nomerel = String.Empty;
                    string nomeDoArquivo = String.Empty;
                    string ds = String.Empty;
                    string ids = String.Empty;
                    // Essa linha foi adicionado para remover ids sem informação Ex: 52,32,,12,35, pois esse ,, estava dando erro. Isso normalmente acontece quando o funcionário manda imprimir o relatório com dados da grid de seleção que estavam em cache e um dos funcionários não existe mais (Excluído ou inativo)
                    List<int> idsSel = imp.idSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).ToList().Select(s => Convert.ToInt32(s)).ToList();
                    ids = "(" + String.Join(",", idsSel) + ")";
                    DataTable dt = new DataTable();

                    List<ReportParameter> parametros = new List<ReportParameter>();
                    ReportParameter p1 = new ReportParameter("datainicial", imp.InicioPeriodo.ToShortDateString());
                    parametros.Add(p1);
                    ReportParameter p2 = new ReportParameter("datafinal", imp.FimPeriodo.ToShortDateString());
                    parametros.Add(p2);

                    BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(ConnectionString, usuarioLogado);
                    bllBancoHoras.ObjProgressBar = ObjProgressBar;
                    ActionResult retorno = null;
                    switch (imp.TipoRelatorio)
                    {
                        case 0:
                            dt = bllBancoHoras.GetRelatorioHorario(imp.InicioPeriodo, imp.FimPeriodo, imp.TipoSelecao, ids);
                            if (imp.TipoArquivo == "Excel")
                            {
                                nomeDoArquivo = String.Format("RelatorioBancoHorasIndividual_Periodo_{0}_{1}_Emissao_{2}", imp.InicioPeriodo.ToString("ddMMyyyy"), imp.FimPeriodo.ToString("ddMMyyyy"), DateTime.Now.ToString("ddMMyyyy"));
                                objProgressBar.setaMensagem("Formatando dados...");
                                foreach (DataRow row in dt.Rows)
                                {
                                    row["data"] = Convert.ToDateTime((row["data"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    row["dataadmissao"] = Convert.ToDateTime((row["dataadmissao"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                                    row["bancohorascre"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["bancohorascre"]).ToString());
                                    row["bancohorasdeb"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["bancohorasdeb"]).ToString());
                                    row["saldobh"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["saldobh"]).ToString());

                                    BLL.cwkFuncoes.RemoveTracosHoraRow(row, new List<string>() { "horEntrada1",
                                                                                  "horSaida1",
                                                                                  "horEntrada2",
                                                                                  "horSaida2",
                                                                                  "entrada_1",
                                                                                  "saida_1",
                                                                                  "entrada_2",
                                                                                  "saida_2",
                                                                                  "entrada_3",
                                                                                  "saida_3",
                                                                                  "entrada_4",
                                                                                  "saida_4"
                                                                                });
                                }
                                objProgressBar.setaMensagem("Gerando Arquivo Excel...");
                                // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
                                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                                #region Dados Empresa
                                colunasExcel.Add("empresa", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("endereco", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Endereço", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("cidade", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cidade", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("estado", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "UF", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("cep", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.CEP, NomeColuna = "CEP", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("cnpj_cpf", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "CNPJ/CPF", Visivel = true, NomeColunaNegrito = true });
                                #endregion
                                #region Dados Empregado
                                colunasExcel.Add("funcionario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcionário", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("dataadmissao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data Admissão", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("horario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Horário", Visivel = true, NomeColunaNegrito = true });
                                #endregion
                                #region Dados Relatório
                                colunasExcel.Add("data", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("horEntrada1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Ent.1", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("horSaida1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Sai.1", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("horEntrada2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Ent.2", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("horSaida2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jor. Sai.2", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("legenda", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Legenda", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("entrada_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.1", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("saida_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.1", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("entrada_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.2", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("saida_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.2", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("entrada_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.3", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("saida_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.3", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("entrada_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent.4", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("saida_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sai.4", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("bancohorascre", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Crédito", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("bancohorasdeb", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Débito", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("saldobh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("legendaSaldo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Crédito/Débito", Visivel = true, NomeColunaNegrito = true });
                                #endregion



                                byte[] Arquivo = null;
                                Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dt);
                                retorno = new CustomFileResult(Arquivo, "application/ms-excel", nomeDoArquivo + ".xlsx");
                            }
                            else
                            {
                                nomerel = "rptBancoHorasHorario.rdlc";
                                ds = "dsBancoHorasResumo_BancoHorasHorario";
                                nomeDoArquivo = "Relatório_Banco_de_Horas_Individual_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();
                                objProgressBar.setaMensagem("Gerando Arquivo PDF...");
                                retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, dt, ds, nomerel, parametros);
                            }
                            break;
                        case 1:
                            nomeDoArquivo = "Relatorio_Resumido_Banco_de_Horas_" + imp.InicioPeriodo.ToShortDateString() + "_" + imp.FimPeriodo.ToShortDateString();

                            if (imp.TipoArquivo == "Excel")
                            {
                                dt = bllBancoHoras.GetRelatorioResumo(imp.InicioPeriodo, imp.FimPeriodo, imp.TipoSelecao, "", "", ids, imp.BuscaSaldoInicioFechamento);
                                objProgressBar.setaMensagem("Gerando Arquivo Excel...");
                                dt.DefaultView.Sort = imp.Ordenacao;
                                dt = dt.DefaultView.ToTable();
                                // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
                                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                                colunasExcel.Add("nomeempresa", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("nomedepartamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("nomeFuncao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("dscodigo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Código", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("nomefuncionario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcionário", Visivel = true, NomeColunaNegrito = true });
                                //Adicionar matricula no datatable
                                colunasExcel.Add("matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                                // Criar a coluna Periodo no dataTable
                                colunasExcel.Add("Periodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Período", Visivel = true, NomeColunaNegrito = true });
                                if (imp.BuscaSaldoInicioFechamento)
                                {
                                    colunasExcel.Add("saldoAnteriorComSinal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo Anterior", Visivel = true, NomeColunaNegrito = true });
                                }
                                colunasExcel.Add("creditobh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Crédito", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("debitobh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Débito", Visivel = true, NomeColunaNegrito = true });
                                colunasExcel.Add("saldoFinalComSinal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo", Visivel = true, NomeColunaNegrito = true });
                                byte[] Arquivo = null;
                                Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dt);
                                retorno = new CustomFileResult(Arquivo, "application/ms-excel", nomeDoArquivo + ".xlsx");
                            }
                            else
                            {
                                dt = bllBancoHoras.GetRelatorioResumo(imp.InicioPeriodo, imp.FimPeriodo, imp.TipoSelecao, "", "", ids, imp.BuscaSaldoInicioFechamento);
                                nomerel = "rptBancoHorasResumo.rdlc";
                                ds = "dsBancoHorasResumo_DataTable1";
                                objProgressBar.setaMensagem("Gerando Arquivo PDF...");
                                retorno = ImprimeRelatorioGenericoProgressBar(imp.TipoArquivo, nomeDoArquivo, dt, ds, nomerel, parametros);
                            }
                            break;
                        default:
                            break;
                    }
                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        public Job GetRelatorioHorasExtrasLocal(pxyRelPontoWeb imp, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, usuarioLogado);

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");

                    byte[] Arquivo = GerarRelatorioHorasExtrasLocal(imp, bllMarcacao);

                    var Rel = new CustomFileResult(Arquivo, "application/vnd.ms-excel", "Relatorio de Horas Extras - Local.xlsx");
                    Thread.Sleep(1000);
                    JobManager.AdicionaArquivoCache(Rel, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        private byte[] GerarRelatorioHorasExtrasLocal(pxyRelPontoWeb imp, BLL.Marcacao bllMarcacao)
        {
            try
            {
                objProgressBar.setaMensagem("Carregando dados de " + imp.InicioPeriodo.ToShortDateString() + " a " + imp.FimPeriodo.ToShortDateString() + " para " + imp.idSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
                DataTable dtMarcacoes = bllMarcacao.GetRelatorioObras(imp.idSelecionados, imp.InicioPeriodo, imp.FimPeriodo, imp.idSelecionados2);
                List<Int64> lIndex = new List<Int64>();
                int index = 0;
                foreach (DataRow dr in dtMarcacoes.Rows)
                {
                    if ((Convert.ToInt32(dr["contFunc"])) == 1 && index > 0)
                    {
                        lIndex.Add(index);
                    }
                    index++;
                }
                foreach (DataColumn col in dtMarcacoes.Columns)
                {
                    col.AllowDBNull = true;
                }

                objProgressBar.setaMensagem("Carregado " + dtMarcacoes.Rows.Count.ToString() + " Registros");
                BLL.HoraExtra HE = new BLL.HoraExtra(dtMarcacoes);
                objProgressBar.setaMensagem("Calculando Horas Extras de " + dtMarcacoes.Rows.Count.ToString() + " Registros");
                IList<HorasExtrasPorDia> horasExtrasDoPeriodo = HE.CalcularHoraExtraDiaria();
                IList<string> ColunasAddDinamic = new List<string>();
                //Pegas os percentuais existentes
                IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.SelectMany(x => x.HorasExtras).ToList();
                var TotalHoras = heMS.GroupBy(l => l.Percentual)
                              .Select(lg =>
                                    new
                                    {
                                        Percentual = lg.Key
                                    }).OrderBy(x => x.Percentual);

                foreach (var horasExtras in TotalHoras) // Adiciona os percentuais existentes como coluna no datatable
                {
                    string nomeColuna = "Extras " + horasExtras.Percentual + "%";
                    dtMarcacoes.Columns.Add(nomeColuna, typeof(System.String));
                    ColunasAddDinamic.Add(nomeColuna);
                }

                objProgressBar.setaMinMaxPB(0, dtMarcacoes.Rows.Count);
                int cont = 1;
                foreach (DataRow dr in dtMarcacoes.Rows) // Adiciona os valores nos percentuais
                {
                    DateTime dataMarc = Convert.ToDateTime(dr["data"]);
                    int idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
                    objProgressBar.incrementaPB(1);
                    objProgressBar.setaMensagem("Atribuindo Hora Extra do dia  " + dataMarc.ToShortDateString() + " do funcionário " + dr["nome"].ToString());
                    //Busca as horas extras do dia do funcionário
                    IList<Modelo.Proxy.HoraExtra> HEDiaFuncionario = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == idFuncionario && x.DataMarcacao == dataMarc).SelectMany(x => x.HorasExtras).ToList();
                    var horasExtrasFuncDia = HEDiaFuncionario.GroupBy(l => l.Percentual)
                                    .Select(lg =>
                                        new
                                        {
                                            Percentual = lg.Key,
                                            HoraDiurna = lg.Sum(w => w.HoraDiurna),
                                            HoraNoturna = lg.Sum(w => w.HoraNoturna)
                                        }).OrderBy(x => x.Percentual);

                    foreach (var item in horasExtrasFuncDia)// Adiciona os percentuais nas respectivas colunas
                    {
                        string nomeColuna = "Extras " + item.Percentual + "%";
                        dr[nomeColuna] = Modelo.cwkFuncoes.ConvertMinutosHora(item.HoraDiurna + item.HoraNoturna).Replace("--:--", "");
                    }
                }

                index = 0;
                objProgressBar.setaMensagem("Atribuindo separadores entre os funcionários");
                objProgressBar.setaMinMaxPB(0, lIndex.Count);
                foreach (int ind in lIndex)
                {
                    DataRow dr = dtMarcacoes.NewRow();
                    dtMarcacoes.Rows.InsertAt(dr, ind + index);
                    index++;
                    objProgressBar.incrementaPB(1);
                }

                objProgressBar.setaMensagem("Gerando Arquivo...");
                // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                colunasExcel.Add("obra", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Obra", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("data", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("dia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Dia", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("ocorrencia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ocorrência", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 01", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 01", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 02", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 02", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 03", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 03", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 04", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 04", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 05", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 05", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 06", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 06", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 07", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 07", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("entrada_8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 08", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("saida_8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 08", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasTrabalhadas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "H. Trabalhadas", Visivel = true, NomeColunaNegrito = true });
                foreach (string nomeColuna in ColunasAddDinamic)
                {
                    colunasExcel.Add(nomeColuna, new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = nomeColuna, Visivel = true, NomeColunaNegrito = true });
                }
                colunasExcel.Add("totalHorasTrabalhadas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Total", Visivel = true, NomeColunaNegrito = true });

                byte[] Arquivo = null;
                try
                {
                    Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtMarcacoes);
                }
                catch (Exception)
                {
                    throw;
                }
                return Arquivo;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        public Job GetRelatorioHomemHora(pxyRelPontoWeb imp, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");

                    byte[] Arquivo = GerarRelatorioHomemHora(imp, ConnectionString);
                    string nomeRel = "Relatorio Homem Hora - Periodo " + imp.InicioPeriodo.ToShortDateString() + " a " + imp.FimPeriodo.ToShortDateString() + ".xls";
                    var Rel = new CustomFileResult(Arquivo, "application/vnd.ms-excel", nomeRel);
                    Thread.Sleep(1000);
                    JobManager.AdicionaArquivoCache(Rel, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        private byte[] GerarRelatorioHomemHora(pxyRelPontoWeb imp, string conn)
        {
            try
            {
                objProgressBar.setaMensagem("Carregando dados de " + imp.InicioPeriodo.ToShortDateString() + " a " + imp.FimPeriodo.ToShortDateString() + " para " + imp.idSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
                BLL.Relatorios.RelatorioHomemHora bllRel = new BLL.Relatorios.RelatorioHomemHora(conn);
                DataTable dtHomemHora = bllRel.GetRelatorioHomemHora(imp.idSelecionados, imp.InicioPeriodo, imp.FimPeriodo, imp.bOcorrencia ? imp.idSelecionadosOcorrencias : "");
                dtHomemHora.TableName = "Homem Hora";
                foreach (DataColumn col in dtHomemHora.Columns)
                {
                    col.AllowDBNull = true;
                }

                objProgressBar.setaMensagem("Gerando Arquivo...");
                // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                colunasExcel.Add("Contrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Contrato", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("CIA", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "CIA", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("COY", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "COY", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Planta", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Planta", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Empregado", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empregado", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("TipoMaoObra", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Tipo de Mão de Obra", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("DataRescisao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data de Rescisão", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("DescricaoHorario", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Descrição do Horário", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasHorista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Horista", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasMensalista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Mensalista", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Bancohorascre", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Crédito de B.H.", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Bancohorasdeb", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Débito de B.H.", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasExtrasHorista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Extras Horistas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("HorasExtrasMensalista", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Horas Extras Mensalistas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FaltaAbonadaLegal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Falta Abonada Legal", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("FaltaAbonadaNaoLegal", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Falta Abonada Não Legal", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("OutrosAbonos", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Outros Abonos", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Atraso", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Faltas / Atraso", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Faltas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "Faltas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Absenteismo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.VALOR, NomeColuna = "% Absenteísmo", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("SiglasAfastamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Sigla Afastamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Comentarios", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Comentários", Visivel = true, NomeColunaNegrito = true });
                byte[] Arquivo = null;
                try
                {
                    Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtHomemHora);
                }
                catch (Exception)
                {
                    throw;
                }
                return Arquivo;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        [Authorize]
        public Job GetRelLocalizacaoRegistroPonto(Modelo.Proxy.pxyRelCartaoPonto imp, cw_usuario usuario, ControllerContext controllerContext)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    objProgressBar.setaValorPB(-1);
                    Modelo.UsuarioPontoWeb usuarioLogado = new UsuarioPontoWeb();
                    usuarioLogado.Login = usuario.login;
                    usuarioLogado.Nome = usuario.nome;
                    usuarioLogado.Senha = usuario.senha;
                    usuarioLogado.ConnectionString = usuario.connectionString;
                    BLL.LocalizacaoRegistroPonto bllLocalizacaoRegistroPonto = new BLL.LocalizacaoRegistroPonto(usuario.ConnectionStringDecrypt, usuarioLogado);
                    IList<int> funcs = imp.idSelecionados.Split(',').Select(int.Parse).ToList();

                    objProgressBar.setaMensagem("Agrupando dados...");
                    IList<Modelo.Proxy.Relatorios.PxyRelLocalizacaoRegistroPonto> LocRegPonto = bllLocalizacaoRegistroPonto.RelLocalizacaoRegistroPonto(funcs.ToList(), imp.InicioPeriodo, imp.FimPeriodo);

                    objProgressBar.setaMensagem("Gerando relatório...");

                    FileContentResult retorno = null;
                    if (imp.TipoArquivo == "Excel")
                    {
                        ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
                        Byte[] arquivo = objToExcel.ObjectToExcel("LocalizacaoRegistro", LocRegPonto.ToList());
                        retorno = File(arquivo, "application/vnd.ms-excel", "LocalizacaoRegistro" + DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".xls");
                    }
                    else
                    {
                        string razorText = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Views/RelatorioLocalizacaoRegistroPonto/RelatorioLocalizacaoRegistroPontoHtml.cshtml"));
                        string htmlText = Razor.Parse(razorText, LocRegPonto);

                        if (imp.TipoArquivo == "PDF")
                        {
                            HtmlReport htmlReport = new HtmlReport();
                            objProgressBar.setaMensagem("Renderizando " + LocRegPonto.Count() + " registros.");
                            byte[] buffer = htmlReport.RenderPDF(htmlText, true, true);
                            retorno = File(buffer, "application/PDF");
                        }
                        else
                        {
                            retorno = File(System.Text.Encoding.UTF8.GetBytes(htmlText), "text/html");
                        }
                    }

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    throw e;
                }
            });
            job.CompleteNovaAba = true;
            job.TipoArquivo = imp.TipoArquivo;
            return job;
        }

        [Authorize]
        public Job GetRelGradeHorarioMovel(Modelo.Proxy.PxyHorarioMovel horMovel, cw_usuario usuario, ControllerContext controllerContext)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    objProgressBar.setaValorPB(-1);
                    Modelo.UsuarioPontoWeb usuarioLogado = new UsuarioPontoWeb();
                    usuarioLogado.Login = usuario.login;
                    usuarioLogado.Nome = usuario.nome;
                    usuarioLogado.Senha = usuario.senha;
                    usuarioLogado.ConnectionString = usuario.connectionString;
                    BLL.HorarioDetalhe bllHorariODetalhe = new BLL.HorarioDetalhe(usuario.ConnectionStringDecrypt, usuarioLogado);
                    objProgressBar.setaMensagem("Agrupando dados...");

                    //IList<Modelo.HorarioDetalhe> listaHorarioDetalhe = bllHorariODetalhe.GetGradeHorarioMovel(id);
                    IList<Modelo.Proxy.PxyHorarioMovel> listaPxyHorarioMovel = bllHorariODetalhe.GetPxyGradeHorario(horMovel.Id, horMovel.DataInicial, horMovel.DataFinal);
                    objProgressBar.setaMensagem("Gerando relatório...");

                    FileContentResult retorno = null;
                    string razorText = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Views/RelatorioHorarioMovel/RelatorioHorarioMovelHtml.cshtml"));
                    string htmlText = Engine.Razor.RunCompile(razorText, Guid.NewGuid().ToString(), null, listaPxyHorarioMovel, null);


                    HtmlReport htmlReport = new HtmlReport();
                    objProgressBar.setaMensagem("Renderizando " + listaPxyHorarioMovel.Count() + " registros.");
                    byte[] buffer = htmlReport.RenderPDF(htmlText, true, true);
                    retorno = File(buffer, "application/PDF");

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    throw e;
                }
            });
            job.CompleteNovaAba = true;
            return job;
        }

        [Authorize]
        public Job GetRelClassificacaoHorasExtras(Modelo.Proxy.pxyRelCartaoPonto imp, cw_usuario usuario, ControllerContext controllerContext)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    objProgressBar.setaValorPB(-1);
                    Modelo.UsuarioPontoWeb usuarioLogado = new UsuarioPontoWeb();
                    usuarioLogado.Login = usuario.login;
                    usuarioLogado.Nome = usuario.nome;
                    usuarioLogado.Senha = usuario.senha;
                    usuarioLogado.ConnectionString = usuario.ConnectionStringDecrypt;

                    BLL_N.Relatorios.RelClassHorasExtras bllClassificacaoHorasExtras = new BLL_N.Relatorios.RelClassHorasExtras(usuarioLogado);
                    IList<int> funcs = imp.idSelecionados.Split(',').Select(int.Parse).ToList();

                    List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> listRel = bllClassificacaoHorasExtras.GetDadosRelatorio(funcs.ToList(), imp.InicioPeriodo, imp.FimPeriodo, imp.TipoSelecao).ToList();

                    objProgressBar.setaMensagem("Gerando relatório...");

                    FileContentResult retorno = null;
                    if (imp.TipoArquivo == "Excel")
                    {
                        PxyFileResult arquivo = bllClassificacaoHorasExtras.GerarRelatorioExcel(listRel);
                        retorno = File(arquivo.Arquivo, "application/vnd.ms-excel", "Classificação Horas Extras" + DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".xls");
                    }
                    else
                    {
                        objProgressBar.setaMensagem("Agrupando dados");
                        List<RelatorioParts> RelPartsPDF = new List<RelatorioParts>();
                        objProgressBar.setaValorPB(-1);

                        if (imp.TipoArquivo == "PDF")
                        {
                            objProgressBar.setaMensagem("Renderizando " + listRel.Count() + " Registros.");
                            objProgressBar.setaValorPB(-1);
                            PxyFileResult arquivo = bllClassificacaoHorasExtras.GerarRelatorioPdf(listRel);
                            objProgressBar.setaMensagem("Agrupando relatório.");
                            retorno = File(arquivo.Arquivo, "application/PDF", "Classificação Horas Extras" + DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".pdf");
                        }
                        else
                        {
                            PxyFileResult arquivo = bllClassificacaoHorasExtras.GerarRelatorioHtml(listRel);
                            retorno = File(arquivo.Arquivo, "text/html");
                        }
                    }

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    throw e;
                }
            });
            job.CompleteNovaAba = true;
            job.TipoArquivo = imp.TipoArquivo;
            return job;
        }

        public Job GetRelatorioRegistros(pxyRelPontoWeb imp, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, usuarioLogado);

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    byte[] Arquivo = GerarRelatorioRegistros(imp, bllMarcacao, ConnectionString, usuarioLogado);
                    var Rel = new CustomFileResult(Arquivo, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Relatorio de Registros - " + imp.InicioPeriodo + " a " + imp.FimPeriodo + ".xlsx");
                    Thread.Sleep(1000);
                    JobManager.AdicionaArquivoCache(Rel, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        private byte[] GerarRelatorioRegistros(pxyRelPontoWeb imp, BLL.Marcacao bllMarcacao, string connectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            try
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                DataTable dtRelatorio = new DataTable();
                IList<string> ColunasAddDinamic = new List<string>();
                objProgressBar.setaMensagem("Carregando dados de " + imp.InicioPeriodo.ToShortDateString() + " a " + imp.FimPeriodo.ToShortDateString() + " para " + imp.idSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
                objProgressBar.setaMinMaxPB(0, 1);
                objProgressBar.setaValorPB(1);
                List<int> idsFuncs = imp.idSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                int partes = idsFuncs.Count();
                if (partes >= 3)
                {
                    partes = idsFuncs.Count() / 3;
                }
                ConcurrentBag<DataTable> PedacosRelatorio = new ConcurrentBag<DataTable>();
                IList<List<int>> idsParciais = BLL.cwkFuncoes.SplitList(idsFuncs, partes);
                Parallel.ForEach(idsParciais,
                ids =>
                {
                    try
                    {
                        PedacosRelatorio.Add(bllMarcacao.GetRelatorioRegistros(String.Join(",", ids), imp.InicioPeriodo, imp.FimPeriodo));
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                });

                objProgressBar.setaMensagem("Calculando Horas Extras");
                objProgressBar.setaMinMaxPB(0, 1);
                objProgressBar.setaValorPB(1);
                BLL.HoraExtra.ValidaHorariosDiferenteDiarioMensal(connectionString, usuarioLogado, PedacosRelatorio.ToList());

                Parallel.ForEach(PedacosRelatorio,
                    dtMarcacoes =>
                    {
                        try
                        {
                            BLL.HoraExtra HE = new BLL.HoraExtra(dtMarcacoes);
                            IList<HorasExtrasPorDia> horasExtrasDoPeriodo = HE.CalcularHoraExtraDiaria();

                            //Pegas os percentuais existentes
                            IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.SelectMany(x => x.HorasExtras).ToList();
                            var TotalHoras = heMS.GroupBy(l => l.Percentual)
                                          .Select(lg =>
                                                new
                                                {
                                                    Percentual = lg.Key
                                                }).OrderBy(x => x.Percentual);

                            foreach (var horasExtras in TotalHoras) // Adiciona os percentuais existentes como coluna no datatable
                            {
                                string nomeColuna = "Extras " + horasExtras.Percentual + "%";
                                dtMarcacoes.Columns.Add(nomeColuna, typeof(System.String));
                                ColunasAddDinamic.Add(nomeColuna);
                            }

                            foreach (DataRow dr in dtMarcacoes.Rows) // Adiciona os valores nos percentuais
                            {
                                

                                DateTime dataMarc = Convert.ToDateTime(dr["data"], culture);
                                int idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
                                //Busca as horas extras do dia do funcionário
                                IList<Modelo.Proxy.HoraExtra> HEDiaFuncionario = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == idFuncionario && x.DataMarcacao == dataMarc).SelectMany(x => x.HorasExtras).ToList();
                                var horasExtrasFuncDia = HEDiaFuncionario.GroupBy(l => l.Percentual)
                                                .Select(lg =>
                                                    new
                                                    {
                                                        Percentual = lg.Key,
                                                        HoraDiurna = lg.Sum(w => w.HoraDiurna),
                                                        HoraNoturna = lg.Sum(w => w.HoraNoturna)
                                                    }).OrderBy(x => x.Percentual);

                                foreach (var item in horasExtrasFuncDia)// Adiciona os percentuais nas respectivas colunas
                                {
                                    string nomeColuna = "Extras " + item.Percentual + "%";
                                    dr[nomeColuna] = Modelo.cwkFuncoes.ConvertMinutosHora(item.HoraDiurna + item.HoraNoturna).Replace("--:--", "");
                                }
                            }
                            lock (dtRelatorio)
                            {
                                dtRelatorio.Merge(dtMarcacoes);
                            }
                        }
                        catch (Exception e)
                        {

                            throw e;
                        }
                    });

                objProgressBar.setaMensagem("Ordenando Dados");
                objProgressBar.setaMinMaxPB(0, 1);
                objProgressBar.setaValorPB(1);
                DataView dtV = dtRelatorio.DefaultView;
                dtV.Sort = "nome,dataSemFormat,Matrícula";
                dtRelatorio = dtV.ToTable();

                objProgressBar.setaMensagem("Gerando Arquivo...");

                dtRelatorio.Columns["Créd. BH"].ReadOnly = false;
                dtRelatorio.Columns["Déb. BH"].ReadOnly = false;
                dtRelatorio.Columns["Hra_Banco_Horas"].ReadOnly = false;

                foreach (DataRow row in dtRelatorio.Rows)
                {
                        row["Créd. BH"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["Créd. BH"]).ToString());
                        row["Déb. BH"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["Déb. BH"]).ToString());
                        row["Hra_Banco_Horas"] = BLL.cwkFuncoes.RemoveZeroEsqerdaHora((row["Hra_Banco_Horas"]).ToString());
                }

                // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                colunasExcel.Add("Data", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Dia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Dia", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Matrícula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Alocação", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocação", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Tipo de Vínculo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Tipo de Vínculo", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Data de Admissão", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data de Admissão", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Data de Demissão", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data de Demissão", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                //colunasExcel.Add("Contrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Contrato", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Função", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Jornada", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jornada", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 01", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 01", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 02", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 02", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 03", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 03", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 04", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 04", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 05", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 05", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 06", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 06", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 07", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 07", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ent8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 08", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Sai8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 08", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Desconsideradas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Desconsideradas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("H. Diurnas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "H. Diurnas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("H. Noturnas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "H. Noturnas", Visivel = true, NomeColunaNegrito = true });
                ColunasAddDinamic = ColunasAddDinamic.Distinct().ToList();
                try
                {
                    ColunasAddDinamic = ColunasAddDinamic.OrderBy(o => Convert.ToInt32(o.Replace("Horas ", "").Replace("%", ""))).ToList();
                }
                catch (Exception)
                {

                }
                foreach (string nomeColuna in ColunasAddDinamic.Distinct())
                {
                    colunasExcel.Add(nomeColuna, new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = nomeColuna, Visivel = true, NomeColunaNegrito = true });
                }
                colunasExcel.Add("Ad. Noturno", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ad. Noturno", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Dsr", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Dsr", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Faltas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Faltas", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Créd. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Créd. BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Déb. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Déb. BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Hra_Banco_Horas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo BH", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Total", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Total", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Ocorrência", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ocorrência", Visivel = true, NomeColunaNegrito = true });

                byte[] Arquivo = null;
                try
                {
                    Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtRelatorio);
                }
                catch (Exception)
                {
                    throw;
                }
                return Arquivo;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }

        }

        private static string RemoveZeroEsqerdaHora(string valor)
        {
            string horaSem0Esquerda = "--:--";
            string[] registro = valor.Split(':');
            int h = 0;
            if (Int32.TryParse(registro[0], out h) && registro.Count() == 2)
            {
                horaSem0Esquerda = h + ":" + registro[1];
            }

            return horaSem0Esquerda;
        }


        public Job GetRelConclusoesBloqueioPnlRh(pxyRelPontoWeb imp, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, usuarioLogado);

            job = JobManager.Instance.DoJobAsync(j =>
                {
                    try
                    {
                        objProgressBar.setaMensagem("Carregando dados...");

                        byte[] Arquivo = GerarRelatorioConclusoesBloqueioPnlRh(imp, bllMarcacao);

                        var Rel = new CustomFileResult(Arquivo, "application/vnd.ms-excel", "Relatorio de Conclusoes e Bloqueio Painel RH - " + imp.InicioPeriodo + " a " + imp.FimPeriodo + ".xlsx");
                        Thread.Sleep(1000);
                        JobManager.AdicionaArquivoCache(Rel, job.Id);
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        throw ex;
                    }
                });
            job.CompleteNovaAba = true;
            return job;
        }

        private byte[] GerarRelatorioConclusoesBloqueioPnlRh(pxyRelPontoWeb imp, BLL.Marcacao bllMarcacao)
        {
            try
            {
                objProgressBar.setaMensagem("Carregando dados de " + imp.InicioPeriodo.ToShortDateString() + " a " + imp.FimPeriodo.ToShortDateString() + " para " + imp.idSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
                DataTable dtMarcacoes = bllMarcacao.ConclusoesBloqueioPnlRh(imp.idSelecionados, imp.InicioPeriodo, imp.FimPeriodo, imp.TipoSelecao);
                objProgressBar.setaMensagem("Carregado " + dtMarcacoes.Rows.Count.ToString() + " Registros");
                objProgressBar.setaMensagem("Gerando Arquivo...");
                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                colunasExcel.Add("Data", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Dia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Dia", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matricula", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Alocacao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocacao", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Empresa", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Empresa", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("CNPJ", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "CNPJ", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcao", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Data_Bloqueio_Edicao_PnlRh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data_Bloqueio_Edicao_PnlRh", Visivel = true, NomeColunaNegrito = true });
                colunasExcel.Add("Data_Concl_Fluxo_PnlRh", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data_Concl_Fluxo_PnlRh", Visivel = true, NomeColunaNegrito = true });

                byte[] arquivo = null;
                try
                {
                    arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtMarcacoes);
                }
                catch (Exception ex)
                {
                    throw;
                }
                return arquivo;

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        [Authorize]
        public Job GetRelHorasTrabalhadasNoturnas(Modelo.Proxy.pxyRelCartaoPonto imp, cw_usuario usuario)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    Modelo.UsuarioPontoWeb usuarioLogado = new UsuarioPontoWeb();
                    usuarioLogado.Login = usuario.login;
                    usuarioLogado.Nome = usuario.nome;
                    usuarioLogado.Senha = usuario.senha;
                    usuarioLogado.ConnectionString = usuario.connectionString;
                    BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(usuario.ConnectionStringDecrypt, usuarioLogado);
                    IList<int> funcs = imp.idSelecionados.Split(',').Select(int.Parse).ToList();
                    IList<pxyCartaoPontoEmployer> cps = bllCartaoPonto.BuscaDadosRelatorio(funcs, imp.InicioPeriodo, imp.FimPeriodo, objProgressBar, imp.OrdemRelatorio);


                    IList<pxyRelatorioHorasNoturnas> rel = new List<pxyRelatorioHorasNoturnas>();
                    foreach (pxyCartaoPontoEmployer cartao in cps)
                    {
                        pxyRelatorioHorasNoturnas func = new pxyRelatorioHorasNoturnas();

                        func.Periodo = cartao.Periodo;
                        func.EmpresaNome = cartao.pxyFuncionarioCabecalhoRel.EmpresaNome;
                        func.FuncionarioMatricula = cartao.pxyFuncionarioCabecalhoRel.Matricula;
                        func.FuncionarioCodigo = cartao.pxyFuncionarioCabecalhoRel.DsCodigo;
                        func.FuncionarioNome = cartao.pxyFuncionarioCabecalhoRel.Nome;
                        func.PIS = cartao.pxyFuncionarioCabecalhoRel.Pis;
                        func.TotalHorasNoturnas = cartao.Totalizador.TrabalhadasNoturnas;
                        func.TotalHorasNoturnasComReducao = cartao.Totalizador.HorasAdNoturna;
                        rel.Add(func);
                    }

                    objProgressBar.setaMensagem("Gerando relatório...");

                    FileContentResult retorno = null;
                    //if (imp.TipoArquivo == "Excel")
                    //{
                    ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
                    Byte[] arquivo = objToExcel.ObjectToExcel("Horas Noturnas", rel.ToList());
                    retorno = File(arquivo, "application/vnd.ms-excel", "HorasTrabalhadasNoturnas" + DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".xls");
                    //}
                    //else
                    //{
                    //    objProgressBar.setaMensagem("Agrupando dados");
                    //    List<RelatorioParts> RelPartsPDF = new List<RelatorioParts>();
                    //    objProgressBar.setaValorPB(-1);

                    //    if (imp.TipoArquivo == "PDF")
                    //    {
                    //        objProgressBar.setaMensagem("Renderizando " + listRel.Count() + " Registros.");
                    //        objProgressBar.setaValorPB(-1);
                    //        PxyFileResult arquivo = bllClassificacaoHorasExtras.GerarRelatorioPdf(listRel);
                    //        objProgressBar.setaMensagem("Agrupando relatório.");
                    //        retorno = File(arquivo.Arquivo, "application/PDF", "Classificação Horas Extras" + DateTime.Now.ToString("dd-MM-yyyy-HH:mm") + ".pdf");
                    //    }
                    //    else
                    //    {
                    //        PxyFileResult arquivo = bllClassificacaoHorasExtras.GerarRelatorioHtml(listRel);
                    //        retorno = File(arquivo.Arquivo, "text/html");
                    //    }
                    //}

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    throw e;
                }
            });
            job.CompleteNovaAba = true;
            job.TipoArquivo = imp.TipoArquivo;
            return job;
        }

        public Job GetRelatorioFechamentoPercentualHE(pxyRelFechamentoPercentualHE imp, string ConnectionString, Modelo.Cw_Usuario usuarioLogado)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;

            BLL.FechamentobhdHE bllFechamentobhdHE = new BLL.FechamentobhdHE(ConnectionString, usuarioLogado);
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(ConnectionString, usuarioLogado);

            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");

                    byte[] Arquivo = GerarRelatorioFechamentoPercentualHE(imp, bllFechamentobhdHE, bllBancoHoras);

                    var Rel = new CustomFileResult(Arquivo, "application/vnd.ms-excel",
                        "Relatorio de Fechamento Percentual de Horas Extras.xlsx");
                    Thread.Sleep(1000);
                    JobManager.AdicionaArquivoCache(Rel, job.Id);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });

            job.CompleteNovaAba = true;
            return job;
        }

        private byte[] GerarRelatorioFechamentoPercentualHE(pxyRelFechamentoPercentualHE imp, BLL.FechamentobhdHE bllFechamentobhdHE, BLL.BancoHoras bllBancoHoras)
        {
            try
            {
                objProgressBar.setaMensagem("Carregando dados de " + imp.InicioPeriodo.ToShortDateString() + " a " + imp.FimPeriodo.ToShortDateString() + " para " + imp.idSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
                DataTable dtFechamentoPercentual = new DataTable();

                // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
                Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();

                if (imp.TipoRelatorio == 0)
                {
                    dtFechamentoPercentual = bllFechamentobhdHE.GetRelatorioFechamentoPercentualHEAnalitico(imp.idSelecionados, imp.InicioPeriodo, imp.FimPeriodo);
                    dtFechamentoPercentual.TableName = "Fechamento Percentual HE";

                    if (imp.bPrevia)
                    {
                        int codBancoHoras = imp.BuscaCodigo();
                        Modelo.BancoHoras objBancoHoras = bllBancoHoras.LoadObjectByCodigo(codBancoHoras);

                        List<pxyFechamentobhdHEAnalitico> lstPxyFechamentobhdHEAnalitico = Conversores.ConvertTo<pxyFechamentobhdHEAnalitico>(dtFechamentoPercentual);
                        List<pxyFechamentobhdHEAnalitico> lstPxyFechamentobhdHEAnaliticoParcial =
                        bllFechamentobhdHE.BuscaFechamentobhdHEAnaliticoPrevia(objBancoHoras, 2, imp.idSelecionados, imp.InicioPeriodo, imp.FimPeriodo);

                        lstPxyFechamentobhdHEAnalitico.AddRange(lstPxyFechamentobhdHEAnaliticoParcial);
                        lstPxyFechamentobhdHEAnalitico = lstPxyFechamentobhdHEAnalitico.OrderBy(s => s.Matricula).ThenBy(t => t.DataBatida).ToList();
                        dtFechamentoPercentual = Conversores.ToDataTable<pxyFechamentobhdHEAnalitico>(lstPxyFechamentobhdHEAnalitico);
                    }

                    objProgressBar.setaMensagem("Gerando Arquivo...");

                    //Analítico
                    colunasExcel.Add("Matrícula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Alocação", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocação", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Função", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });

                    colunasExcel.Add("Jornada", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Jornada", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("DataBatida", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });

                    colunasExcel.Add("Ent. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Sai. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Ent. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Sai. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Ent. 3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 3", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Sai. 3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 3", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Ent. 4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 4", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Sai. 4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 4", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Créd. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Créd. BH", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Déb. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Déb. BH", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Saldo BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo BH", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Perc. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Perc. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Supervisor", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Supervisor", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Ocorrência", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ocorrência", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Código Fechamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cód. Fechamento", Visivel = true, NomeColunaNegrito = true });
                }
                else
                {
                    dtFechamentoPercentual = bllFechamentobhdHE.GetRelatorioFechamentoPercentualHESintetico(imp.idSelecionados, imp.InicioPeriodo, imp.FimPeriodo);
                    dtFechamentoPercentual.TableName = "Fechamento Percentual HE";

                    if (imp.bPrevia)
                    {
                        int codBancoHoras = imp.BuscaCodigo();
                        Modelo.BancoHoras objBancoHoras = bllBancoHoras.LoadObjectByCodigo(codBancoHoras);

                        List<pxyFechamentobhdHESintetico> lstPxyFechamentobhdHESintetico = Conversores.ConvertTo<pxyFechamentobhdHESintetico>(dtFechamentoPercentual);
                        List<pxyFechamentobhdHESintetico> lstPxyFechamentobhdHESinteticoParcial =
                        bllFechamentobhdHE.BuscaFechamentobhdHESinteticoPrevia(ref lstPxyFechamentobhdHESintetico, objBancoHoras, 2, imp.idSelecionados,
                                                                               imp.InicioPeriodo, imp.FimPeriodo);

                        lstPxyFechamentobhdHESintetico.AddRange(lstPxyFechamentobhdHESinteticoParcial);
                        lstPxyFechamentobhdHESintetico = lstPxyFechamentobhdHESintetico.OrderBy(s => s.Matricula).ToList();
                        dtFechamentoPercentual = Conversores.ToDataTable<pxyFechamentobhdHESintetico>(lstPxyFechamentobhdHESintetico);
                    }

                    objProgressBar.setaMensagem("Gerando Arquivo...");

                    //Sintético
                    colunasExcel.Add("Período", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Período", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Matrícula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Alocação", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocação", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Função", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Horário", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Horário", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Créd. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Créd. BH", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Déb. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Déb. BH", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Saldo BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo BH", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Perc. 1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 1", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Perc. 2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Perc. 2", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Código Fechamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cód. Fechamento", Visivel = true, NomeColunaNegrito = true });
                    colunasExcel.Add("Data Fechamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Data Fechamento", Visivel = true, NomeColunaNegrito = true });
                }

                foreach (DataColumn col in dtFechamentoPercentual.Columns)
                {
                    col.AllowDBNull = true;
                }

                byte[] Arquivo = null;
                try
                {
                    Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtFechamentoPercentual);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Arquivo;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }

        [Authorize]
        public Job GetRelatorioInItinere(Modelo.Proxy.pxyRelCartaoPonto imp, cw_usuario usuario, ControllerContext controllerContext)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    Modelo.UsuarioPontoWeb usuarioLogado = new UsuarioPontoWeb();
                    usuarioLogado.Login = usuario.login;
                    usuarioLogado.Nome = usuario.nome;
                    usuarioLogado.Senha = usuario.senha;
                    usuarioLogado.ConnectionString = usuario.connectionString;
                    BLL.HorasInItinere bllHorasInItinere = new BLL.HorasInItinere(usuario.ConnectionStringDecrypt, usuarioLogado);
                    IList<int> funcs = imp.idSelecionados.Split(',').Select(int.Parse).ToList();
                    IList<PxyRelatorioHorasInItinere> rels = bllHorasInItinere.BuscaDadosRelatorio(funcs, imp.InicioPeriodo, imp.FimPeriodo, objProgressBar);

                    ConcurrentBag<RelatorioParts> relatorios = new ConcurrentBag<RelatorioParts>();
                    objProgressBar.setaMensagem("Criando " + rels.Count() + " páginas.");
                    objProgressBar.setaValorPB(-1);
                    IList<List<PxyRelatorioHorasInItinere>> partes = rels.GroupBy(x => new { x.PxyFuncionarioCabecalhoRel.EmpresaNome, x.PxyFuncionarioCabecalhoRel.EmpresaCNPJCPF }).Select(s => s.ToList()).ToList();

                    HtmlReport htmlReport = new HtmlReport();

                    IList<string> htmls = new List<string>();
                    foreach (List<PxyRelatorioHorasInItinere> parte in partes)
                    {
                        string razorText = "";
                        razorText = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Views/RelatorioHorasInItinere/RelatorioHTML.cshtml"));
                        string htmlText = Razor.Parse(razorText, parte);
                        htmls.Add(htmlText);
                    }
                    FileContentResult retorno = null;
                    if (imp.TipoArquivo == "PDF")
                    {
                        objProgressBar.setaMensagem("Renderizando " + partes.Count() + " páginas.");
                        objProgressBar.setaValorPB(-1);
                        int part = 0;
                        Parallel.ForEach(htmls, (ht) =>
                        {
                            part++;
                            RelatorioParts cpb = new RelatorioParts();
                            cpb.Parte = part;
                            byte[] buffer = htmlReport.RenderPDF(ht, false, false);
                            cpb.Arquivo = buffer;
                            relatorios.Add(cpb);
                        });
                        objProgressBar.setaMensagem("Agrupando " + partes.Count() + " páginas.");
                        objProgressBar.setaValorPB(-1);
                        byte[] buffer1 = htmlReport.MergeFiles(relatorios.OrderBy(o => o.Parte).Select(s => s.Arquivo).ToList(), true, false);
                        retorno = File(buffer1, "application/PDF");
                    }
                    else
                    {
                        retorno = File(System.Text.Encoding.UTF8.GetBytes(String.Join(String.Empty, htmls.ToArray())), "text/html");
                    }

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    if (e.GetAllExceptionAsString().Contains("Não há paginas para imprimir."))
                    {
                        throw new Exception("Não há paginas para imprimir.");
                    }
                    else
                    {
                        throw new Exception(string.Concat(e.GetAllExceptions().ToList().Select(t => t.Message + "<br/>")));
                    }
                }
            });
            job.CompleteNovaAba = true;
            job.TipoArquivo = imp.TipoArquivo;
            return job;
        }

        [Authorize]
        public Job GerarRelatorioAbsenteismoV2(pxyRelPontoWeb imp, string connectionString)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados");
                    objProgressBar.setaValorPB(-1);
                    List<int> idsFuncs = imp.idSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList();
                    BLL.Relatorios.RelatorioAbsenteismoV2 bllRelatorioAbsenteismoV2 = new BLL.Relatorios.RelatorioAbsenteismoV2(connectionString);
                    DataTable dt = bllRelatorioAbsenteismoV2.GetDados(imp.idSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList(), imp.InicioPeriodo, imp.FimPeriodo);
                    if (dt.Rows.Count > 0)
                    {
                        objProgressBar.setaMensagem("Gerando Relatório");
                        byte[] retorno = bllRelatorioAbsenteismoV2.GetRelatorio(dt);
                        var Rel = new CustomFileResult(retorno, "application/vnd.ms-excel",
                            "Relatorio absenteismo.xlsx");
                        Thread.Sleep(1000);
                        JobManager.AdicionaArquivoCache(Rel, job.Id);
                    }
                    else
                    {
                        Exception ex = new Exception("Não há dados");
                        BLL.cwkFuncoes.LogarErro(ex);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            });
            return job;
        }

        public Job GetRelatorioTotalHoras(Modelo.Proxy.pxyRelPontoWeb imp, UsuarioPontoWeb usuario)
        {
            objProgressBar.incrementaPB = this.IncrementaProgressBar;
            objProgressBar.setaMensagem = this.SetaMensagem;
            objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
            objProgressBar.setaValorPB = this.SetaValorProgressBar;
            job = JobManager.Instance.DoJobAsync(j =>
            {
                try
                {
                    objProgressBar.setaMensagem("Carregando dados...");
                    objProgressBar.setaValorPB(-1);
                    IList<PxyRelTotalHoras> totais = GetTotalizadoresFuncionarios(imp, usuario);
                    if (imp.Generico) // Campo no relátório de Total de Horas foi utilizado para controlar a quebra de página
                    {
                        totais.ToList().ForEach(F => F.UmFuncPorPagina = true); 
                    }
                    objProgressBar.setaMensagem("Gerando relatório...");
                    objProgressBar.setaValorPB(-1);
                    string nomeRel = "Total de Horas Por Funcionário";
                    FileContentResult retorno = null;
                    if (imp.TipoArquivo == "Excel")
                    {
                        objProgressBar.setaMensagem("Organizando dados...");
                        IList<string> ColunasAddDinamic = new List<string>();
                        DataTable dados = Conversores.ToDataTable<PxyRelTotalHoras>(totais);
                        IList<int> percs = totais.SelectMany(x => x.LRateioHorasExtras).Select(s => s.percentual).ToList();
                        percs = percs.Distinct().OrderBy(x => x).ToList();

                        foreach (var perc in percs) // Adiciona os percentuais existentes como coluna no datatable
                        {
                            string nomeColuna = "Extras " + perc + "%";
                            dados.Columns.Add(nomeColuna, typeof(System.String));
                            ColunasAddDinamic.Add(nomeColuna);
                        }

                        foreach (DataRow dr in dados.Rows) // Adiciona os valores nos percentuais
                        {
                            //Busca as horas extras do dia do funcionário
                            IList<RateioHorasExtras> extras = totais.Where(x => x.FuncionarioDsCodigo == Convert.ToString(dr["FuncionarioDsCodigo"]) && x.FuncionarioMatricula == Convert.ToString(dr["FuncionarioMatricula"])).SelectMany(x => x.LRateioHorasExtras).ToList();
                            var horasExtrasFunc = extras.GroupBy(l => l.percentual)
                                            .Select(lg =>
                                                new
                                                {
                                                    Percentual = lg.Key,
                                                    HoraDiurna = lg.Sum(w => w.diurnoMin),
                                                    HoraNoturna = lg.Sum(w => w.noturnoMin)
                                                }).OrderBy(x => x.Percentual);

                            foreach (var item in horasExtrasFunc)// Adiciona os percentuais nas respectivas colunas
                            {
                                string nomeColuna = "Extras " + item.Percentual + "%";
                                dr[nomeColuna] = Modelo.cwkFuncoes.ConvertMinutosHora(item.HoraDiurna + item.HoraNoturna).Replace("--:--", "");
                            }
                        }

                        objProgressBar.setaMensagem("Gerando Excel...");
                        byte[] Arquivo = null;
                        try
                        {
                            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
                            #region Dados Empregado
                            colunasExcel.Add("FuncionarioDsCodigo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Cód. Funcionário", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("FuncionarioNome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Funcionário", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("FuncionarioMatricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasTrabDiurna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA3, NomeColuna = "Trab. diurna", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasTrabNoturna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA3, NomeColuna = "Trab. noturna", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasAdNoturno", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Adicional Noturno", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasExtraDiurna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Horas Extras Diurna", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasExtraNoturna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Horas Extras Noturnas", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasExtraInterjornada", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Interjornada", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasFaltaDiurna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Faltas Diurnas", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasFaltaNoturna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Faltas Noturnas", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("HorasDDSR", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "DSR", Visivel = true, NomeColunaNegrito = true });
                            foreach (string nomeColuna in ColunasAddDinamic)
                            {
                                colunasExcel.Add(nomeColuna, new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = nomeColuna, Visivel = true, NomeColunaNegrito = true });
                            }
                            colunasExcel.Add("SaldoAnteriorBH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo B.H. Anterior", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("SinalSaldoAnteriorBH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo B.H. Anterior Tipo", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("CreditoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Crédito B.H. Período", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("DebitoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Débito B.H. Período", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("SaldoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo B.H. Período", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("SinalSaldoBHPeriodo", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo B.H. Período Tipo", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("SaldoBHAtual", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saldo B.H. Atual", Visivel = true, NomeColunaNegrito = true });
                            colunasExcel.Add("SinalSaldoBHAtual", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saldo B.H. Atual Tipo", Visivel = true, NomeColunaNegrito = true });

                            #endregion


                            Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dados);
                        }
                        catch (Exception e)
                        {

                            throw e;
                        }
                        retorno = new CustomFileResult(Arquivo, "application/ms-excel", nomeRel + ".xlsx");
                    }
                    else
                    {
                        string razorText = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Views/RelatorioTotalHoras/RelatorioTotalHorasHtml.cshtml"));
                        string htmlText = Engine.Razor.RunCompile(razorText, Guid.NewGuid().ToString(), null, totais);

                        if (imp.TipoArquivo == "PDF")
                        {
                            HtmlReport htmlReport = new HtmlReport();
                            objProgressBar.setaMensagem("Renderizando " + totais.Count() + " registros.");
                            byte[] buffer = htmlReport.RenderPDF(htmlText, true, true);
                            retorno = File(buffer, "application/PDF", nomeRel + ".pdf");
                        }
                        else
                        {
                            retorno = File(System.Text.Encoding.UTF8.GetBytes(htmlText), "text/html");
                        }
                    }

                    JobManager.AdicionaArquivoCache(retorno, job.Id);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    throw e;
                }
            });
            job.CompleteNovaAba = true;
            job.TipoArquivo = imp.TipoArquivo;
            return job;
        }

        public List<Modelo.Proxy.Relatorios.PxyRelTotalHoras> GetTotalizadoresFuncionarios(pxyRelPontoWeb imp, UsuarioPontoWeb usuario)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuario.ConnectionString, usuario);
            List<Funcionario> funcionarios = bllFuncionario.GetAllListByIds(imp.idSelecionados);
            BLL.Relatorios.RelatorioTotalHoras relTotal = new BLL.Relatorios.RelatorioTotalHoras(usuario);
            List<Modelo.TotalHoras> totais = relTotal.GerarTotaisFuncionarios(imp.InicioPeriodo, imp.FimPeriodo, funcionarios, objProgressBar);
            List<Modelo.Proxy.Relatorios.PxyRelTotalHoras> ret = new List<PxyRelTotalHoras>();
            objProgressBar.setaMensagem("Preparando dados...");
            foreach (var item in totais)
            {
                ret.Add(new Modelo.Proxy.Relatorios.PxyRelTotalHoras()
                {
                    FuncionarioDsCodigo = item.funcionario.Dscodigo,
                    FuncionarioNome = item.funcionario.Nome,
                    FuncionarioMatricula = item.funcionario.Matricula,
                    HorasTrabDiurna = item.horasTrabDiurna,
                    HorasTrabNoturna = item.horasTrabNoturna,
                    HorasAdNoturno = item.horasAdNoturno,
                    HorasExtraDiurna = item.horasExtraDiurna,
                    HorasExtraNoturna = item.horasAdNoturno,
                    HorasExtraInterjornada = item.horasExtraInterjornada,
                    HorasFaltaDiurna = item.horasFaltaDiurna,
                    HorasFaltaNoturna = item.horasFaltaNoturna,
                    HorasDDSR = item.horasDDSR,
                    LRateioHorasExtras = item.lRateioHorasExtras,
                    CreditoBHPeriodoMin  = item.creditoBHPeriodoMin,
                    DebitoBHPeriodoMin = item.debitoBHPeriodoMin,
                    CreditoBHPeriodo = item.creditoBHPeriodo,
                    DebitoBHPeriodo = item.debitoBHPeriodo,
                    SinalSaldoBHAtual = item.sinalSaldoBHAtual == '+' ? "Crédito" : (item.sinalSaldoBHAtual == '-' ? "Débito" : ""),
                    SaldoAnteriorBH = item.saldoAnteriorBH,
                    SinalSaldoAnteriorBH = item.sinalSaldoAnteriorBH == '+' ? "Crédito" : (item.sinalSaldoAnteriorBH == '-' ? "Débito" : ""),
                    SaldoBHPeriodo = item.saldoBHPeriodo,
                    SinalSaldoBHPeriodo = item.sinalSaldoBHPeriodo == '+' ? "Crédito" : (item.sinalSaldoBHPeriodo == '-' ? "Débito" : ""),
                    SaldoBHAtual = item.saldoBHAtual,
                    DataIni = imp.InicioPeriodo,
                    DataFin = imp.FimPeriodo
                });
            }
            return ret;
        }
    }
}