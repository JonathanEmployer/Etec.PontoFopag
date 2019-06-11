using Hangfire;
using Hangfire.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireConfig
    {
        public void ConfigureHangfireClient()
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("MonitorPontofopag")
                .UseMsmqQueues(@".\Private$\hangfire-calc", HangfireQueues())
                .UseConsole();
            // Specify other options here

            GlobalConfiguration.Configuration.UseFilter(new AutomaticRetryAttribute
            {
                Attempts = 3
            })
            .UseFilter(new HangfireJobFilterAttribute());
        }

        private string[] HangfireQueues()
        {
            //A ordem do hangfire é alfabética, não importa os nomes.
            string prioridadeAntigaPontoWeb = BLL.cwkFuncoes.RemoveAcentosECaracteresEspeciais(Environment.MachineName.ToLower()); // Prioridade para atender a progress antiga do Pontoweb
            List<string> queues = new List<string>() {
                    prioridadeAntigaPontoWeb
                };

            bool isDebugMode = false;
            #if DEBUG
                isDebugMode = true;
            #endif
            //Quando em Debug não adiciona as prioridades que devem ser processadas apenas pela produção.
            if (!isDebugMode)
            {
                queues.AddRange("critico,normal,pequeno".Split(','));
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
