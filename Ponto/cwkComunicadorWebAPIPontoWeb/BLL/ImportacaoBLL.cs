using cwkComunicadorWebAPIPontoWeb.BLL.Jobs;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cwkComunicadorWebAPIPontoWeb.BLL
{
    public class ImportacaoBLL
    {
        public async Task<List<Tuple<IJobDetail, ITrigger>>> GetJobsImportacaoAsync(CancellationToken ct, IProgress<ReportaErro> progress)
        {
            RepBLL bllRep = new RepBLL();
            LoginBLL bllLogin = new LoginBLL();

            List<Tuple<IJobDetail, ITrigger>> result = new List<Tuple<IJobDetail, ITrigger>>();

            try
            {
                ct.ThrowIfCancellationRequested();
                ListaRepsViewModel listaReps = await bllRep.GetAllRepsComNsrAsync(ct, progress);
                TokenResponseViewModel userData = await bllLogin.GetXmlRegisterDataAsync();

                foreach (var item in listaReps.Reps.Where(w => w.ImportacaoAtivada))
                {
                    IDictionary<string, object> DadosRep = new Dictionary<string, object>();
                    DadosRep.Add("Rep", item);
                    DadosRep.Add("UserData", userData);
                    DadosRep.Add("Url", VariaveisGlobais.URL_WS);
                    DadosRep.Add("Progress", progress);

                    string msg = "Importação de bilhetes REP: " + item.NumSerie
                        + " (Fabricante:" + item.NomeFabricante + " - Modelo: "
                        + item.NomeModelo + ").";

                    IJobDetail job = JobBuilder.Create<ImportaBilheteJob>()
                    .WithIdentity("ImportRep" + item.Id, "ImportJobGroup" + item.Id)
                    .WithDescription(msg)
                    .UsingJobData(new JobDataMap(DadosRep))
                    .RequestRecovery(true)
                    .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("TriggerRep" + item.Id, "ImportTriggerGroup" + item.Id)
                        .WithSimpleSchedule(x => {
                            switch (item.TipoImportacao)
                            {
                                case TempoImportacao.Horas:
                                    x.WithIntervalInHours(item.TempoImportacao).RepeatForever();
                                    break;
                                case TempoImportacao.Minutos:
                                    x.WithIntervalInMinutes(item.TempoImportacao).RepeatForever();
                                    break;
                                case TempoImportacao.Dias:
                                    x.WithIntervalInHours(24 * item.TempoImportacao).RepeatForever();
                                    break;
                                default:
                                    break;
                            }
                        })
                        .Build();
                    ReportarProgresso(new ReportaErro() { Mensagem = "(Agendador - Jobs) Agendando " + msg, TipoMsg = TipoMensagem.Info }, progress);
                    result.Add(new Tuple<IJobDetail, ITrigger>(job, trigger));
                }
                
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Operação cancelada pelo usuário.", TipoMsg = TipoMensagem.Erro }, progress);
                }
                else
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "JobsImportacao" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("Erro no Job de Importação", e, filePath);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao executar operação: " + e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                    if (result.Count > 0)
                    {
                        result = new List<Tuple<IJobDetail, ITrigger>>();
                    }
                }
            }

            return result;
        }

        public async Task AgendaJobsImportacaoExecucaoInstantaneaAsync(CancellationToken ct, IProgress<ReportaErro> progress)
        {
            RepBLL bllRep = new RepBLL();
            LoginBLL bllLogin = new LoginBLL();

            List<Tuple<IJobDetail, ITrigger>> result = new List<Tuple<IJobDetail, ITrigger>>();

            try
            {
                ct.ThrowIfCancellationRequested();
                ListaRepsViewModel listaReps = await bllRep.GetAllRepsComNsrAsync(ct, progress);
                TokenResponseViewModel userData = await bllLogin.GetXmlRegisterDataAsync();
                if (listaReps.Reps.Where(w => w.ImportacaoAtivada).Count() == 0)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Nenhum Rep Ativado para Importação. Entre na Opção \"Config. Reps\" e Ative a opção \"Ativar Importação\" o Rep Desejado!", TipoMsg = TipoMensagem.Aviso }, progress);
                    System.Windows.Forms.MessageBox.Show("Nenhum Rep Ativado para Importação. Entre na Opção \"Config. Reps\" e Ative a opção \"Ativar Importação\" o Rep Desejado!");
                }
                foreach (var item in listaReps.Reps.Where(w => w.ImportacaoAtivada))
                {
                    IDictionary<string, object> DadosRep = new Dictionary<string, object>();
                    DadosRep.Add("Rep", item);
                    DadosRep.Add("UserData", userData);
                    DadosRep.Add("Url", ViewModels.VariaveisGlobais.URL_WS);
                    DadosRep.Add("MostraMensagem", true);
                    DadosRep.Add("Progress", progress);

                    string msg = "Importação de bilhetes REP: " + item.NumSerie
                        + " (Fabricante:" + item.NomeFabricante + " - Modelo: "
                        + item.NomeModelo + ").";
                    string dt = DateTime.Now.ToString("ddMMyyy_HHmmss");
                    IJobDetail job = JobBuilder.Create<ImportaBilheteJob>()
                    .WithIdentity("ImportRep" + item.Id + "_Instant_" + dt, "ImportJobGroup" + item.Id)
                    .WithDescription(msg)
                    .UsingJobData(new JobDataMap(DadosRep))
                    .RequestRecovery(true)
                    .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("TriggerRep" + item.Id + "_Instant_" + dt, "ImportTriggerGroup" + item.Id)
                        .StartAt(DateBuilder.FutureDate(5, IntervalUnit.Second))
                        .Build();
                    ReportarProgresso(new ReportaErro() { Mensagem = msg, TipoMsg = TipoMensagem.Info }, progress);
                    result.Add(new Tuple<IJobDetail, ITrigger>(job, trigger));
                }

                if (Program.scheduler == null)
                {
                    Program.scheduler = StdSchedulerFactory.GetDefaultScheduler();
                }
                if (!Program.scheduler.IsStarted)
                {
                    Program.scheduler.Start();
                }

                foreach (var item in result)
                {
                    Program.scheduler.ScheduleJob(item.Item1, item.Item2);
                }

            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Operação cancelada pelo usuário.", TipoMsg = TipoMensagem.Erro }, progress);
                }
                else
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "ErroAgendarJobs" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("Erro ao Agendar Job", e, filePath);
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao executar operação: " + e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                    if (result.Count > 0)
                    {
                        result = new List<Tuple<IJobDetail, ITrigger>>();
                    }
                }
            }

        }
        private void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }
    }
}
