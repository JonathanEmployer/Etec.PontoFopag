using BLL_N.JobManager.Hangfire;
using Modelo.Relatorios;
using System;
using System.Web.Mvc;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class TesteFilaController : Controller
    {
        // GET: TesteFila
        public string Index(int quantidade, string queueds)
        {
            string[] lQueueds = queueds.Split(',');
            string ret = "";
            for (int i = 0; i < quantidade; i++)
            {
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios("pontofopag_employer_hom");
                Random random = new Random();
                int tempo = random.Next(0, 100);
                int index = random.Next(lQueueds.Length);
                ret = hfm.GerarJobsTesteProcessamento(tempo, lQueueds[index]);
            }

            return ret;
        }
    }
}