using Quartz;
using Quartz.Impl;
using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace ComunicadorServico
{
    public partial class ServicoComunicador : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog eventLog1;
        private const string Group1 = "VerificaReps";
        private const string Job = "Job";
        public ServicoComunicador()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("ServIntegracaoPontofopag"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "ServIntegracaoPontofopag", "LogServComunicadorPontofopag");
            }
            eventLog1.Source = "ServIntegracaoPontofopag";
            eventLog1.Log = "LogServComunicadorPontofopag";
        }

        private static IScheduler _scheduler;
        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Serviço Comunicador Iniciado.", EventLogEntryType.Information);
            log.Info("Serviço Comunicador Iniciado, gerando agendamentos...");
            ISchedulerFactory sf = new StdSchedulerFactory();
            _scheduler = sf.GetScheduler().Result;
            if (!_scheduler.IsStarted)
                _scheduler.Start();

            IJobDetail job = JobBuilder.Create<Negocio.Jobs.MonitorarRepsJob>()
                    .WithIdentity("MonitorarReps", "MonitorReps")
                    .Build();

            // Adiciona o trabalho para executar a cada 1 minuto
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("VericarReps", "MonitorReps")
                .StartNow()
                .WithSimpleSchedule(x => x
                      .WithIntervalInMinutes(1)
                      .RepeatForever())
                .Build();

            // Agenda o job
            _scheduler.ScheduleJob(job, trigger);

            #region Agendador para reiniciar o serviço
            IJobDetail jobReciclar = JobBuilder.Create<Negocio.Jobs.ReiniciaServico>()
                        .WithIdentity("ReciclarServico", "ReciclaServico")
                        .Build();
            ITrigger triggerReciclar = TriggerBuilder.Create()
                .WithIdentity("Recicla", "ReciclaServico")
                .WithCronSchedule("0 0 2 ? * SAT *")
                .Build();
            _scheduler.ScheduleJob(jobReciclar, triggerReciclar); 
            #endregion
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Serviço Comunicador Parado.", EventLogEntryType.Information);
            log.Warn("Parando Serviço Comunicador.");
            Negocio.Rep.SetarTodosRepsProcessando(false);
            //timer.Dispose();
            if (_scheduler != null)
            {
                _scheduler.Shutdown();
            }
            log.Warn("Serviço Comunicador Parado.");
        }
    }
}
