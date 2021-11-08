using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;
using Hangfire.Storage.Monitoring;
using Modelo.Proxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Modelo.EntityFramework.MonitorPontofopag.Repository;
using Hangfire.Common;
using Hangfire.Console.Monitoring;
using Modelo.EntityFramework.MonitorPontofopag;
using BLL.Util;

namespace BLL_N.JobManager.Hangfire
{
    public class JobControlManager
    {
        public static List<PxyJobReturn> GetJobsPagina(int skip, int qtdRegs, string urlReference, string user)
        {
            List<PxyJobReturn> lista = new List<PxyJobReturn>();

            JobControlRepository jobRepository = new JobControlRepository();
            List<JobControl> jobs = jobRepository.GetJobsPaginado(skip, 10, urlReference, user);

            foreach (var item in jobs.Where(w => w.StatusNovo == ProcessingState.StateName))
            {
                PxyJobReturn processando = GetLastLogConsoleHangfire(item.JobId.ToString());
                if (!String.IsNullOrWhiteSpace(processando.IdTask) && processando.IdTask != "0")
                {
                    lista.Add(processando);
                }
                else
                {
                    if (String.IsNullOrEmpty(item.Mensagem))
                    {
                        item.Mensagem = "Processando...";
                        item.Progresso = -1;
                    }
                }
            }

            foreach (var item in jobs.Where(w => w.StatusNovo == DeletedState.StateName))
            {
                var job = JobStorage.Current.GetMonitoringApi().JobDetails(item.JobId.ToString());
                if (job != null && job.Job != null)
                    item.Reprocessar = true;
            }

            foreach (var item in jobs.Where(w => w.StatusNovo == FailedState.StateName))
            {
                item.Reprocessar = true;
            }

            //foreach (var item in jobs.Where(w => w.StatusNovo == FailedState.StateName))
            //{
            //    PxyJobReturn jobErro = BLL_N.BackgroundJobHF.JobControl.GetReturnJobErro(item);
            //    lista.Add(jobErro);
            //}

            lista.AddRange(PxyJobReturn.JobControlToPxyJobReturn(jobs.Where(w => !lista.Select(s => Convert.ToInt32(s.IdTask)).ToList().Contains(w.JobId)).ToList()));
            lista.Where(w => w.StatusAnterior == ProcessingState.StateName && w.StatusNovo == ScheduledState.StateName).ToList().ForEach(f => { f.Progress = -3; f.Mensagem = "Algo não ocorreu como o esperado, aguarde, vamos tentar novamente"; });
            return lista;
        }

        public static PxyJobReturn GetLastLogConsoleHangfire(string jobId)
        {
            PxyJobReturn retorno = new PxyJobReturn();
            try
            {
                if (string.IsNullOrEmpty(jobId))
                    throw new ArgumentNullException(NameOf.nameof(() => jobId));

                var job = JobStorage.Current.GetMonitoringApi().JobDetails(jobId);
                if (job == null)
                    return retorno;

                var state = job.History.FirstOrDefault(s => s.StateName == ProcessingState.StateName);
                if (state != null)
                {
                    using (var consoleApi = JobStorage.Current.GetConsoleApi())
                    {
                        var lines = consoleApi.GetLines(jobId,
                                JobHelper.DeserializeDateTime(state.Data["StartedAt"]),
                                LineType.Text);
                        if (lines.Count > 0)
                        {
                            var ultimo = lines.ToList().LastOrDefault();
                            string json = ((TextLineDto)ultimo).Text.Replace("Progresso = ", "");
                            retorno = JsonConvert.DeserializeObject<PxyJobReturn>(json);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return retorno;
        }

        public static JobControl SalvarJobControl(JobControl jobControl)
        {
            using (var db = new MONITOR_PONTOFOPAGEntities())
            {
                jobControl.Inchora = DateTime.Now;
                db.JobControl.Add(jobControl);
                db.SaveChanges();
            }
            return jobControl;
        }

        public static JobControl StatusUpdate(int jobId, string statusNovo, string statusAntigo)
        {
            JobControl jc = new JobControl();
            using (var db = new MONITOR_PONTOFOPAGEntities())
            {
                jc = db.JobControl.Where(w => w.JobId == jobId).FirstOrDefault();
                if (jc != null)
                {
                    jc.StatusNovo = statusNovo;
                    jc.StatusAnterior = statusAntigo;
                    db.SaveChanges();
                }
            }
            return jc;
        }

        public static JobControl JobControlUpdate(JobControl jc)
        {
            using (var db = new MONITOR_PONTOFOPAGEntities())
            {
                db.JobControl.Add(jc);
                db.Entry(jc).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return jc;
        }

        public static JobControl SetUsuarioCancelamento(int jobId, string usuario)
        {
            JobControl jc = new JobControl();
            using (var db = new MONITOR_PONTOFOPAGEntities())
            {
                jc = db.JobControl.Where(w => w.JobId == jobId).FirstOrDefault();
                if (jc != null)
                {
                    jc.UsuarioCancelamento = usuario;
                    jc.DataCalcelameto = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return jc;
        }

        public static JobControl ProgressUpdate(string jobId, string mensagem, int progress)
        {
            JobControl jc = new JobControl();
            int id = 0;
            int.TryParse(jobId, out id);
            if (id > 0)
            {
                using (var db = new MONITOR_PONTOFOPAGEntities())
                {
                    jc = db.JobControl.Where(w => w.JobId == id).FirstOrDefault();
                    if (jc != null)
                    {
                        jc.Mensagem = mensagem;
                        jc.Progresso = progress;
                        if (jc.Progresso == -9 || jc.Progresso == -8)
                        {
                            jc.Reprocessar = true;
                        }
                        db.SaveChanges();
                    }
                } 
            }
            return jc;
        }

        public static JobControl UpdateFileDownload(PerformContext context, string FileDownload)
        {
            JobControl jc = new JobControl();
            if (context != null && !String.IsNullOrEmpty(context.BackgroundJob.Id))
            {
                int jobId = 0;
                Int32.TryParse(context.BackgroundJob.Id, out jobId);

                using (var db = new MONITOR_PONTOFOPAGEntities())
                {
                    jc = db.JobControl.Where(w => w.JobId == jobId).FirstOrDefault();
                    if (jc != null)
                    {
                        jc.FileDownload = FileDownload;
                        db.SaveChanges();
                    }
                }
            }
            return jc;
        }

        public static PxyJobReturn GetReturnJobErro(JobControl item)
        {
            IMonitoringApi monitoringApi = JobStorage.Current.GetMonitoringApi();
            JobDetailsDto jobDetails = monitoringApi.JobDetails(item.JobId.ToString());
            string erro = jobDetails.History.FirstOrDefault(s => s.StateName == FailedState.StateName).Data.Where(w => w.Key == "ExceptionMessage").FirstOrDefault().Value;
            PxyJobReturn jobErro = new PxyJobReturn(item);
            jobErro.Mensagem = erro;
            jobErro.Progress = -9;
            if (!jobErro.Mensagem.Contains("não encontrado"))
            {
                jobErro.Reprocessar = true;
            }
            return jobErro;
        }

        public static JobControl GetJobControl(string id)
        {
            int jobId = 0;
            Int32.TryParse(id, out jobId);
            JobControl job = new JobControl();
            using (var db = new MONITOR_PONTOFOPAGEntities())
            {
                try
                {
                    job = db.JobControl.Where(w => w.JobId == jobId).FirstOrDefault();
                    if (job.StatusNovo == DeletedState.StateName || job.StatusNovo == FailedState.StateName)
                    {
                        job.Reprocessar = true;
                    }
                }
                catch (Exception e)
                {

                    throw e;
                }
            }

            return job;
        }
    }
}
