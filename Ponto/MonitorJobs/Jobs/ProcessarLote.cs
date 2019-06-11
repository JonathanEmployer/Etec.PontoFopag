using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace MonitorJobs.Jobs
{
    public class ProcessarLote : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            string database = "";
            try
            {
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                database = dataMap.GetString("database");
                log.Debug(database + ": Iniciando novo processamento");
                try
                {
                    Negocio.ImportarRegistrosPonto bllSevImp = new Negocio.ImportarRegistrosPonto(BLL.cwkFuncoes.ConstroiConexao(database));
                    bllSevImp.ProcessarLote();
                }
                finally
                {
                    var jobKey = new TriggerKey(context.Trigger.Key.Name, ((AbstractTrigger)context.Trigger).Group);

                    ITrigger rsiTrigger = TriggerBuilder.Create()
                    .WithIdentity(jobKey)
                    .StartAt(DateTime.Now.AddSeconds(3))
                    .Build();

                    context.Scheduler.RescheduleJob(jobKey, rsiTrigger);
                }
            }
            catch (Exception e)
            {
                LogErro.LogarErro(e, "CS = " + database);
                log.Error(database + ": Erro ao processar lote, erro: " +e.Message, e);
            }
        }
    }
}