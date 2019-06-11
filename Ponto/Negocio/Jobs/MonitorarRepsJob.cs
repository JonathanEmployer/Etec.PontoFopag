using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using ModeloAux;
using Newtonsoft.Json;

namespace Negocio.Jobs
{
    public class MonitorarRepsJob : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async Task Execute(IJobExecutionContext context)
        {
            log.Info("Verificação da situação dos Reps (processo responsável por verificar novos reps adicionados ou mudança de configuração nos reps)");

            log.Debug("Validando configurações");
            
            Modelo.Proxy.PxyConfigComunicadorServico config = Negocio.Configuracao.GetConfiguracao();
            if (String.IsNullOrEmpty(config.Usuario) || String.IsNullOrEmpty(config.Senha))
            {
                string erro = "Serviço não foi configurado (Utilize a aplicação de configuração do serviço para configurar o mesmo).";
                log.Error(erro);
                throw new Exception(erro);
            }
            Negocio.Rep nRep = new Negocio.Rep();
            IList<ModeloAux.RepViewModel> reps = Negocio.Rep.GetRepConfig(config);

            reps = reps.Where(r => r.ServicoComunicador && r.AtivoServico).OrderBy(o => o.UltimaIntegracao).ToList();
            if (reps.Count == 0)
            {
                string erro = "Nenhum rep configurado para integração.";
                log.Error(erro);
                throw new Exception(erro);
            }

            IScheduler scheduler = context.Scheduler;
            reps = reps.Where(r => r.ServicoComunicador && r.AtivoServico).OrderBy(o => o.UltimaIntegracao).ToList();
            foreach (ModeloAux.RepViewModel rep in reps)
            {
                log.Debug("Verificando agendamento do Rep = " + rep.NumSerie);
                AgendarIntegracao(scheduler, rep, config);
            }
        }

        public static void AgendarIntegracao(IScheduler scheduler, ModeloAux.RepViewModel rep, Modelo.Proxy.PxyConfigComunicadorServico config)
        {
            string nomeGrupo = rep.NumSerie;
            string nomeJob = "Integracao";
            var jobKey = new JobKey(nomeJob, nomeGrupo);
            //Verifica se já não existe o Job para o cliente
            log.Debug(nomeGrupo + ": Agendando com jobkey = " + jobKey.Group + "-" + jobKey.Name);
            JobDataMap m = new JobDataMap();
            m.Put("rep", rep);
            m.Put("config", config);
            if (!scheduler.CheckExists(jobKey).Result)
            {
                log.Debug(nomeGrupo + ": Criando agendador com jobkey = " + jobKey.Group + "-" + jobKey.Name);
                IJobDetail job = JobBuilder.Create<Jobs.IntegrarRepLog>()
                    .WithIdentity(nomeJob, nomeGrupo)
                    .UsingJobData(m)
                    .Build();

                // Adiciona o trabalho para executar a cada 3 segundos
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("IntegrarRep", nomeGrupo)
                    .StartNow()
                    .Build();

                // Agenda o job
                scheduler.ScheduleJob(job, trigger);
            }
            else
            {
                log.Debug(nomeGrupo + ": Não é necessário agendar o job com a jobkey = " + jobKey.Group + "-" + jobKey.Name + "; Key já exite");
            }
        }
    }
}
