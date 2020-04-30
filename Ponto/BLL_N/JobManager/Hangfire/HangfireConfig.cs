using Hangfire;
using Hangfire.Console;
using System;
using System.Collections.Generic;
using System.Configuration;
using Hangfire.Azure.ServiceBusQueue;
using Hangfire.SqlServer;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Azure;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireConfig
    {
        public void ConfigureHangfireClient()
        {
            var connectionStringServiceBus = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            if (string.IsNullOrEmpty(connectionStringServiceBus))
            {
                GlobalConfiguration.Configuration
                .UseSqlServerStorage("MonitorPontofopag")
                .UseMsmqQueues(@".\Private$\hangfire-calc", HangfireQueues());
            }
            else
            {
                var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionStringServiceBus);
                string[] queues = HangfireQueues();
                foreach (var queue in queues)
                {
                    if (!namespaceManager.QueueExists(queue))
                        namespaceManager.CreateQueue(queue);
                }

                var sqlStorage = new SqlServerStorage("MonitorPontofopag");
                Action<QueueDescription> configureAction = qd =>
                {
                    qd.MaxSizeInMegabytes = 5120;
                    qd.DefaultMessageTimeToLive = new TimeSpan(0, 1, 0);
                };
                sqlStorage.UseServiceBusQueues(new ServiceBusQueueOptions
                {
                    ConnectionString = connectionStringServiceBus,
                    Configure = configureAction,
                    Queues = queues,
                    CheckAndCreateQueues = false,
                    LoopReceiveTimeout = TimeSpan.FromMilliseconds(500),
                    LockRenewalDelay = new TimeSpan(0, 0, 15)
            });
                GlobalConfiguration.Configuration.UseStorage(sqlStorage);
            }

            GlobalConfiguration.Configuration
                .UseConsole()
                .UseFilter(new AutomaticRetryAttribute { Attempts = 3 })
                .UseFilter(new HangfireJobFilterAttribute());
        }

        private string[] HangfireQueues()
        {
            //A ordem do hangfire é alfabética, não importa os nomes.
            string prioridadeAntigaPontoWeb = BLL.cwkFuncoes.RemoveAcentosECaracteresEspeciais(Environment.MachineName.ToLower()); // Prioridade para atender a progress antiga do Pontoweb
            List<string> queues = new List<string>() { prioridadeAntigaPontoWeb };

            //Quando em Debug não adiciona as prioridades que devem ser processadas apenas pela produção.
            string queuesConfig = ConfigurationManager.AppSettings["hangfireQueues"];

            if (!String.IsNullOrEmpty(queuesConfig))
            {
                queues.AddRange(queuesConfig.Split(','));
            }

            return queues.ToArray();
        }

        public BackgroundJobServer ConfigureHangfireServer()
        {
            var options = new BackgroundJobServerOptions
            {
                Queues = HangfireQueues()
            };
            return new BackgroundJobServer(options);
        }


    }
}
