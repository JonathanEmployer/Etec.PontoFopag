using HangfireDLL = Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using System;
using System.Linq;
using Modelo.EntityFramework.MonitorPontofopag;
using System.Globalization;
using Hangfire.Server;
using BLL.Util;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireJobFilterAttribute : JobFilterAttribute, IApplyStateFilter, IClientFilter, IServerFilter
    {
        public void OnCreating(CreatingContext context)
        {
            if (context == null) throw new ArgumentNullException(NameOf.nameof(() => context));

            context.SetJobParameter(
                "CurrentCulture", CultureInfo.CurrentCulture.Name);
            context.SetJobParameter(
                "CurrentUICulture", CultureInfo.CurrentUICulture.Name);
        }

        public void OnCreated(CreatedContext context)
        {

        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var cultureName = filterContext.GetJobParameter<string>("CurrentCulture");
            var uiCultureName = filterContext.GetJobParameter<string>("CurrentUICulture");

            if (!String.IsNullOrEmpty(cultureName))
            {
                filterContext.Items["PreviousCulture"] = CultureInfo.CurrentCulture;
                SetCurrentCulture(new CultureInfo(cultureName));
            }

            if (!String.IsNullOrEmpty(uiCultureName))
            {
                filterContext.Items["PreviousUICulture"] = CultureInfo.CurrentUICulture;
                SetCurrentUICulture(new CultureInfo(uiCultureName));
            }
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException(NameOf.nameof(() => filterContext));

            if (filterContext.Items.ContainsKey("PreviousCulture"))
            {
                SetCurrentCulture((CultureInfo)filterContext.Items["PreviousCulture"]);
            }
            if (filterContext.Items.ContainsKey("PreviousUICulture"))
            {
                SetCurrentUICulture((CultureInfo)filterContext.Items["PreviousUICulture"]);
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // Se essa rotina não for executada o job não será monitorado pela aplicação (para dar certo o segundo parâmetro de todos os métodos deve ser o jobControl)
            context.JobExpirationTimeout = TimeSpan.FromDays(30);

            int idJob = 0;
            Int32.TryParse(context.BackgroundJob.Id, out idJob);
            JobControl jobControl = JobControlManager.StatusUpdate(idJob, context.NewState.Name, context.OldStateName);
            if (jobControl == null)
            {
                jobControl = GetJobControlApplyStateContext(context);
                jobControl = JobControlManager.SalvarJobControl(jobControl);
            }

            //Job esta sendo criado
            if (context.NewState.Name == EnqueuedState.StateName && context.OldStateName != context.NewState.Name)
            {
                Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
                if (!String.IsNullOrEmpty(jobReturn.NomeGrupo))
                {
                    jobReturn.Mensagem = "Aguarde, processamento será iniciado em breve";
                    jobReturn.Progress = -1;
                    BLL_N.Hubs.NotificationHub.ReportarJobProgresso(jobReturn);
                }
            }

            //Processo Iniciado
            if (context.NewState.Name == ProcessingState.StateName && context.OldStateName != context.NewState.Name)
            {
                Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
                if (!String.IsNullOrEmpty(jobReturn.NomeGrupo))
                {
                    jobReturn.Mensagem = "Processo iniciado, aguarde...";
                    jobReturn.Progress = -1;
                    BLL_N.Hubs.NotificationHub.ReportarJobProgresso(jobReturn);
                }
            }

            // Job deu erro.
            if (context.NewState.Name == FailedState.StateName)
            {
                Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
                if (!String.IsNullOrEmpty(jobReturn.NomeGrupo))
                {
                    jobReturn = JobControlManager.GetReturnJobErro(jobControl);
                    BLL_N.Hubs.NotificationHub.ReportarJobProgresso(jobReturn);
                }
            }

            // Job Concluído com sucesso.
            if (context.NewState.Name == SucceededState.StateName)
            {
                Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
                if (!String.IsNullOrEmpty(jobReturn.NomeGrupo))
                {
                    jobReturn.Progress = 100;
                    TimeSpan span = (((HangfireDLL.States.SucceededState)context.NewState).SucceededAt - context.BackgroundJob.CreatedAt);
                    jobReturn.Mensagem = "Processo Concluído em " + String.Format("{0}m {1}s", span.Minutes + (span.Hours * 60), span.Seconds);
                    BLL_N.Hubs.NotificationHub.ReportarJobCompleto(jobReturn);
                }
            }

            // Job Excluído
            if (context.NewState.Name == DeletedState.StateName)
            {
                Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
                if (!String.IsNullOrEmpty(jobReturn.NomeGrupo))
                {
                    jobReturn.Progress = -8;
                    jobReturn.Mensagem = "Processo excluído/cancelado";
                    jobReturn.Reprocessar = true;
                    BLL_N.Hubs.NotificationHub.ReportarJobProgresso(jobReturn);
                }
                context.JobExpirationTimeout = TimeSpan.FromDays(60);
            }

            // Job deu erro e será adicionado na fila novamente.
            if (context.OldStateName == ProcessingState.StateName && context.NewState.Name == ScheduledState.StateName)
            {
                Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
                if (!String.IsNullOrEmpty(jobReturn.NomeGrupo))
                {
                    jobReturn.Mensagem = "Algo deu errado, tentaremos novamente em instantes, aguarde...";
                    jobReturn.Progress = -3;
                    BLL_N.Hubs.NotificationHub.ReportarJobProgresso(jobReturn);
                }
            }

            var enqueuedState = context.NewState as EnqueuedState;

            // Activating only when enqueueing a background job
            if (enqueuedState != null)
            {
                // Checking if an original queue is already set
                var originalQueue = JobHelper.FromJson<string>(context.Connection.GetJobParameter(
                    context.BackgroundJob.Id,
                    "OriginalQueue"));

                if (originalQueue != null)
                {
                    // Override any other queue value that is currently set (by other filters, for example)
                    enqueuedState.Queue = originalQueue;
                }
                else
                {
                    // Queueing for the first time, we should set the original queue
                    context.Connection.SetJobParameter(
                        context.BackgroundJob.Id,
                        "OriginalQueue",
                        JobHelper.ToJson(enqueuedState.Queue));
                }
            }
        }

        private static JobControl GetJobControlBackgroundJob(HangfireDLL.BackgroundJob BackgroundJob)
        {
            try
            {
                if (BackgroundJob != null && BackgroundJob.Job != null && BackgroundJob.Job.Args.Count() > 1)
                {
                    JobControl jobControl = (JobControl)BackgroundJob.Job.Args.ToArray()[1];
                    int idJob = 0;
                    Int32.TryParse(BackgroundJob.Id, out idJob);
                    jobControl.JobId = idJob;
                    return jobControl;
                }
            }
            catch (Exception)
            {
            }
            return new JobControl();
        }

        private static void SetJobControlContext(ApplyStateContext context, JobControl jobControl)
        {
            try
            {
                context.BackgroundJob.Job.Args.ToArray()[1] = jobControl;
            }
            catch (Exception)
            {
            }
        }

        private static JobControl GetJobControlApplyStateContext(ApplyStateContext applyStateContext)
        {
            try
            {
                if (applyStateContext != null && applyStateContext.BackgroundJob != null)
                {
                    JobControl jobControl = GetJobControlBackgroundJob(applyStateContext.BackgroundJob);
                    jobControl.StatusAnterior = applyStateContext.OldStateName;
                    jobControl.StatusNovo = applyStateContext.NewState.Name;
                    return jobControl;
                }
            }
            catch (Exception)
            {
            }
            return new JobControl();
        }

        
        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }

        private static void SetCurrentCulture(CultureInfo value)
        {
#if NETFULL
            System.Threading.Thread.CurrentThread.CurrentCulture = value;
#else
            CultureInfo.DefaultThreadCurrentCulture = value;
#endif
        }

        // ReSharper disable once InconsistentNaming
        private static void SetCurrentUICulture(CultureInfo value)
        {
#if NETFULL
            System.Threading.Thread.CurrentThread.CurrentUICulture = value;
#else
            CultureInfo.DefaultThreadCurrentUICulture = value;
#endif
        }
    }
}