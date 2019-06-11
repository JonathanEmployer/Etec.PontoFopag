using Hangfire;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitorJobs.Jobs
{
    public class ControleInstancias : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            IList<Models.Bases> lbases = Negocio.Bases.GetBasesPontofopagAtivas();

            log.Debug("Bases para manitorar = " + String.Join("; ", lbases.Select(s => s.Nome)));

            IScheduler scheduler = context.Scheduler;
            int interacao = 0;
            foreach (Models.Bases item in lbases)
            {
                log.Debug(item.Nome + ": Agendando");
                AgendarProcessamentoLote(scheduler, item.Nome);

                #region Agenda processos recorrentes para o Hangfire
                AgendarGeracaoMarcacao(interacao, item);
                interacao++;
                #endregion

                #region Processo em teste
                AgendarEnvioRegistros(item);
                #endregion
            }

        }

        private static void AgendarGeracaoMarcacao(int interacao, Models.Bases item)
        {
            DateTime database = Convert.ToDateTime("2018-09-01 04:00:00").AddMinutes(interacao).ToUniversalTime();
            int hora = database.Hour;
            int minuto = database.Minute;
            //Lógica para remover a palavra "Pontofpag" do nome da base
            string[] nome = item.Nome.Split('_');
            String idJob = "GerarMarc{0}";
            if (nome.Count() > 1)
                idJob = String.Format(idJob, String.Join("_", nome.Skip(1).ToArray()));
            else
                idJob = String.Format(idJob, item.Nome);
            RecurringJob.AddOrUpdate(idJob, () => Negocio.GerarMarcacoes.GerarMarcacoesCS(item.Nome), String.Format("{0} {1} * * *", minuto, hora), queue: "normal");
        }

        private static void AgendarEnvioRegistros(Models.Bases item)
        {
            //Lógica para remover a palavra "Pontofpag" do nome da base
            if (item.Nome == "PONTOFOPAG_EMPLOYER")
            {
                string[] nome = item.Nome.Split('_');
                String idJob = "EnvioRegistro{0}";
                if (nome.Count() > 1)
                    idJob = String.Format(idJob, String.Join("_", nome.Skip(1).ToArray()));
                else
                    idJob = String.Format(idJob, item.Nome);
                RecurringJob.AddOrUpdate(idJob, () => Negocio.EnviarRegistroPonto.EnviarRegistroPontoCS(item.Nome), "*/1 * * * *", queue: "pequeno"); 
            }
        }

        public static void AgendarProcessamentoLote(IScheduler scheduler, string cs)
        {
            //Crias os jobs que verificaram novos registros de ponto a cada 3 minutos
            string nomeGrupo = cs;
            string nomeJob = "ProcessarLote";
            var jobKey = new JobKey(nomeJob, nomeGrupo);
            //Verifica se já não existe o Job para o cliente
            log.Debug(cs + ": Agendando com jobkey = " + jobKey.Group + "-" + jobKey.Name);
            if (!scheduler.CheckExists(jobKey))
            {
                log.Debug(cs + ": Agendado com sucesso com jobkey = " + jobKey.Group + "-" + jobKey.Name);
                IJobDetail job = JobBuilder.Create<Jobs.ProcessarLote>()
                    .WithIdentity(nomeJob, nomeGrupo)
                    .UsingJobData("database", nomeGrupo)
                    .Build();

                // Adiciona o trabalho para executar a cada 3 segundos
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("ProcessaRegistrosPonto", cs)
                    .StartAt(DateTime.Now.AddSeconds(3))
                    .Build();

                // Agenda o job
                scheduler.ScheduleJob(job, trigger);
            }
            else
            {
                log.Debug(cs + ": Não é necessário agendar o job com a jobkey = " + jobKey.Group + "-" + jobKey.Name + "; Key já exite");
            }
        }
    }
}
