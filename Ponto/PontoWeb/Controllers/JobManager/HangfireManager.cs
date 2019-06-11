using Hangfire;
using Hangfire.States;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Modelo.EntityFramework.MonitorPontofopag;

namespace PontoWeb.Controllers.JobManager
{
    public class HangfireManager
    {
        string _usuarioLogado = "";
        string _hostAddress = "";

        public HangfireManager(string usuario)
        {
            _usuarioLogado = usuario;
            _hostAddress = string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority);
        }

        public Modelo.Proxy.PxyJobReturn Calculo()
        {
            string nomeProcesso = "Calculo Teste Progress";
            int progresso = -1;
            string mensagem = "Aguarde...";

            JobControl jobControl = new JobControl() { NomeProcesso = nomeProcesso, Progresso = progresso, Mensagem = mensagem };

            jobControl.Usuario = _usuarioLogado;
            jobControl.UrlHost = _hostAddress;

            try
            {
                var dictionary = new Dictionary<string, object>();
                HttpContext.Current.Request.Form.CopyTo(dictionary);
                jobControl.Parametros = JsonConvert.SerializeObject(dictionary);
                jobControl.ParametrosExibicao = String.Format("Período {0} a {1} para {2} funcionários", DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy HH:mm"), DateTime.Now.ToString("dd/MM/yyyy HH:mm"), 10);
                jobControl.UrlRota = HttpContext.Current.Request.FilePath;
                jobControl.UrlReferencia = HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            }
            catch (Exception)
            {
            }

            var client = new BackgroundJobClient();
            var state = new EnqueuedState("normal");
            string idJob = client.Create<BLL_N.JobManager.CalculoMarcacoes>(x => x.QueueCalculo(null, jobControl), state);

            Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
            return jobReturn;
        }

        private static void SalvarJobControl(JobControl jobControl)
        {
            using (var db = new MONITOR_PONTOFOPAGEntities())
            {
                jobControl.Inchora = DateTime.Now;
                db.JobControl.Add(jobControl);
                db.SaveChanges();
            }
        }
    }
}