using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using Modelo.Relatorios;
using System.Linq;
using Modelo.EntityFramework.MonitorPontofopag;
using Hangfire.States;
using Hangfire.Server;
using Hangfire.Common;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireManagerBase
    {
        #region variaveis
        protected string usuarioLogado;
        protected string hostAddress;
        protected string dataBase;
        protected string urlReferencia;
        // Se estiver em modo debug processa apenas com o nome da máquina em questão, para que o server não processe os processos gerado localmente(desenvolvimento)
        #if DEBUG
        protected EnqueuedState _enqueuedStateNormal = new EnqueuedState(BLL.cwkFuncoes.RemoveAcentosECaracteresEspeciais(Environment.MachineName.ToLower()));
        #else
                protected EnqueuedState _enqueuedStateNormal = new EnqueuedState("normal");
        #endif
        #endregion

        #region Construtores
        public HangfireManagerBase(string dataBase, string usuario, string hostAddress, string urlReferencia)
        {
            this.usuarioLogado = string.IsNullOrEmpty(usuario) ? HttpContext.Current.User.Identity.Name : usuario;
            this.hostAddress = string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority);
            this.dataBase = dataBase;
            this.urlReferencia = urlReferencia;
            if (string.IsNullOrEmpty(this.urlReferencia))
            {
                this.urlReferencia = HttpContext.Current.Request.UrlReferrer == null ? HttpContext.Current.Request.Url.AbsolutePath : HttpContext.Current.Request.UrlReferrer.AbsolutePath;
            }
        }

        public HangfireManagerBase(string dataBase) : this (dataBase, "", "", "")
        {
        }
        #endregion

        #region Metodos auxiliares
        protected static Modelo.Proxy.PxyJobReturn GerarJobReturn(JobControl jobControl, string idJob)
        {
            int jobid = 0;
            Int32.TryParse(idJob, out jobid);
            if (jobControl.JobId == 0)
            {
                jobControl.Mensagem = "Adicionado na Fila, aguardando processamento";
                jobControl.Progresso = -2;
            }
            jobControl.JobId = jobid;
            Modelo.Proxy.PxyJobReturn jobReturn = new Modelo.Proxy.PxyJobReturn(jobControl);
            return jobReturn;
        }

        public JobControl GerarJobControl(string nomeProcesso, string parametrosExibicao)
        {
            JobControl jobControl = new JobControl() { NomeProcesso = nomeProcesso, Progresso = -1, Usuario = usuarioLogado, UrlHost = hostAddress, Inchora = DateTime.Now };
            try
            {
                var dictionary = new Dictionary<string, object>();
                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    dictionary.Add(key, HttpContext.Current.Request.Form[key]);
                }
                jobControl.Parametros = JsonConvert.SerializeObject(dictionary);
                jobControl.ParametrosExibicao = parametrosExibicao;
                jobControl.UrlRota = HttpContext.Current.Request.FilePath;
                jobControl.UrlReferencia = urlReferencia;
            }
            catch (Exception)
            {
            }

            return jobControl;
        }


        public static JobControl GerarJobControl(string nomeProcesso,JobControl jobPai)
        {
            JobControl jobControl = new JobControl() { NomeProcesso = nomeProcesso, Progresso = -1, Usuario = jobPai.Usuario, UrlHost = jobPai.UrlHost };
            try
            {
                jobControl.Parametros = JsonConvert.SerializeObject(jobPai);
                jobControl.ParametrosExibicao = "Recalculo originado pela importação "+jobPai.JobId+"( "+ jobPai.ParametrosExibicao + " ) ";
                jobControl.UrlRota = jobPai.UrlRota;
                jobControl.UrlReferencia = jobPai.UrlReferencia;
            }
            catch (Exception)
            {
            }

            return jobControl;
        }

        public string GetDescricaoParametrosJob(IRelatorioModel parametros)
        {
            RelatorioBaseModel parms = (RelatorioBaseModel)parametros;
            return String.Format("Período {0} a {1} de {2} funcionários {3}", parms.InicioPeriodo.ToShortDateString(), parms.FimPeriodo.ToShortDateString(), parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "": String.Format("({0})", parms.TipoArquivo));
        }

		public string GetDescricaoParametrosDataJob(IRelatorioModel parametros)
		{
			RelatorioBaseModel parms = (RelatorioBaseModel)parametros;
			return String.Format("Data {0} de {1} funcionários {2}", parms.InicioPeriodo.ToShortDateString(), parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
		}
        #endregion

        public static string GetOriginalQueue(PerformContext hangfireContext)
        {
            return SerializationHelper.Deserialize<string>(hangfireContext.Connection.GetJobParameter(
                                        hangfireContext.BackgroundJob.Id,
                                        "OriginalQueue"));
        }
    }
}
