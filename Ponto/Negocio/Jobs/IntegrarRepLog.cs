using Quartz;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Negocio.Jobs
{
    public class IntegrarRepLog : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async Task Execute(IJobExecutionContext context)
        {
            ModeloAux.RepViewModel rep = null;
            Modelo.Proxy.PxyConfigComunicadorServico config = null;

            try
            {
                JobKey key = context.JobDetail.Key;
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                rep = (ModeloAux.RepViewModel)dataMap["rep"];
                config = (Modelo.Proxy.PxyConfigComunicadorServico)dataMap["config"];
                log.Debug(rep.NumSerie + ": Iniciando novo processamento");
                try
                {
                    IList<ModeloAux.RepViewModel> reps = Negocio.Rep.GetConfiguracao();

                    IScheduler scheduler = context.Scheduler;
                    reps = reps.Where(r => r.ServicoComunicador && r.AtivoServico).OrderBy(o => o.UltimaIntegracao).ToList();
                    ModeloAux.RepViewModel repAtualizado = reps.Where(w => w.NumSerie == rep.NumSerie).FirstOrDefault();
                    Integracao integra = new Integracao();
                    integra.ProcessarRep(rep, config);
                }
                catch (Exception e)
                {
                    log.Error("*#*#*#*#*#*#*#*#*#*#*#*#*#*#*#*# "+rep.NumSerie + ": Erro ao processar rep na primeira tentativa, erro: " + e.Message, e);
                    if (e.Message.Contains("Requisição não autorizada, solicitando novo token."))
                    {
                        log.Info("Requisitando novo token para tentar processar novamente o rep: "+rep.NumSerie);
                        Configuracao.RequisitarNovoToken();
                        Integracao integra = new Integracao();
                        log.Info("Processando novamente o rep: " + rep.NumSerie);
                        integra.ProcessarRep(rep, config);
                    }
                    else
                    {
                        throw e;
                    }
                }
                finally
                {
                    var jobKey = new TriggerKey(context.Trigger.Key.Name, ((AbstractTrigger)context.Trigger).Group);

                    ITrigger rsiTrigger = TriggerBuilder.Create()
                    .WithIdentity(jobKey)
                    .StartAt(DateTime.Now.AddSeconds(rep.TempoRequisicao))
                    .Build();

                    context.Scheduler.RescheduleJob(jobKey, rsiTrigger);
                }
            }
            catch (Exception e)
            {
                log.Error(rep.NumSerie + ": Erro ao processar rep, erro: " + e.Message, e);
            }
        }
    }
}
