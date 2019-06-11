namespace MonitorJobs
{
    using System;
    using Quartz;
    using CrystalQuartz.Core.SchedulerProviders;

    public class SimpleSchedulerProvider : StdSchedulerProvider
    {
        protected override System.Collections.Specialized.NameValueCollection GetSchedulerProperties()
        {
            var properties = base.GetSchedulerProperties();
            properties.Add("quartz.scheduler.instanceName", "AgendadorPontofopag");
            return properties;
        }

        protected override void InitScheduler(IScheduler scheduler)
        {
            bool debug = false;
            #if DEBUG
                debug = true;
            #endif
            bool debugPrd = BLL.cwkFuncoes.ConstroiConexao("master").ConnectionString.ToUpper().Contains("PRDST") && debug;
            if (!debugPrd)
            {
                //// Define o Job e adiciona a classe ControleInstancias
                IJobDetail job = JobBuilder.Create<Jobs.ControleInstancias>()
                    .WithIdentity("ControleInstancias", "ImportacaoRegistros")
                    .Build();

                //// Adiciona o trabalho para executar a cada 10 segundos
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("VerificaNovasBases", "ImportacaoRegistros")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                    .Build();

                //// Agenda o job
                try
                {
                    scheduler.ScheduleJob(job, trigger);
                }
                catch (Exception e)
                {
                    // Se o erro é dizendo que já existe o job, apenas discarto
                    if (!e.Message.Contains("already exists with this identification"))
                    {
                        LogErro.LogarErro(e);
                        throw;
                    }
                }
                scheduler.Start(); 
            }
        }
    }
}