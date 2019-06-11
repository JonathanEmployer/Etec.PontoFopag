using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models;
using PontoWeb.Utils;
using System.Threading;
using System.Web.Mvc;
using System.Web.Caching;
using Newtonsoft.Json;
using PontoWeb.Controllers.BLLWeb;
using Hangfire;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
using Hangfire.States;
using System.Text;
using System.IO;
using System.Linq.Expressions;
using Hangfire.Storage;

namespace PontoWeb.Controllers.JobManager
{
    public class JobManager
    {
        public static readonly JobManager Instance = new JobManager();
        IStorageConnection connection = JobStorage.Current.GetConnection();
        public JobManager()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
        }
        ConcurrentDictionary<string, Job> _runningJobs = new ConcurrentDictionary<string, Job>();
        private IHubContext _hubContext;

        public void DoWorkJob(string id, string processo, string empresa, string request)
        {
            Job job = null;
            try
            {
                if (Instance._runningJobs.TryGetValue(id, out job))
                {
                    job.Invoke();
                }
            }
            catch (Exception ex)
            {
                string erro = string.Concat(ex.GetAllExceptions().ToList().Select(t => t.Message + "<br/>"));
                if (erro.Contains("Não há dados"))
                {
                    if (job != null)
                        job.ReportAviso("Não foram encontrados dados para sua requisição, verifique os parâmetros informados");
                }
                else
                {
                    if (job != null)
                        job.ReportError(erro);
                    throw ex;
                }
            }
            finally
            {
                if (job != null)
                    FinallyJob(job);
            }
        }

        private void FinallyJob(Job job)
        {
            try { job.ReportComplete(); }
            finally { Instance._runningJobs.TryRemove(job.Id, out job); }
        }

        public Job DoJobAsync(Action<Job> action)
        {
            var job = new Job(action);
            try
            {    
                if (Instance._runningJobs.TryAdd(job.Id, job) == false)
                    throw new Exception("Já existe um job rodando com o id " + job.Id);

                AddJobCache(job);
                BroadcastJobStatus(job);
            }
            catch (Exception e)
            {

                throw e;
            }

            try
            {
                StackFrame frame = new StackFrame(1, false);
                MethodBase method = frame.GetMethod();
                Type declaringType = method.DeclaringType;
                string processo = declaringType.FullName + "." + method.Name+"(";
                
                FieldInfo[] myFields = action.Target.GetType().GetFields(BindingFlags.Public
                | BindingFlags.Instance);
                List<string> parms = new List<string>();
                for (int i = 0; i < myFields.Length; i++)
                {
                    parms.Add(myFields[i].Name + ":" + myFields[i].GetValue(action.Target));
                    Console.WriteLine("The value of {0} is: {1}",
                        myFields[i].Name, myFields[i].GetValue(action.Target));
                }
                processo += String.Join(",", parms)+ ")";
                string requestFromPost = "";
                try
                {
                    var dictionary = new Dictionary<string, object>();
                    HttpContext.Current.Request.Form.CopyTo(dictionary);
                    requestFromPost = "Requisicao = " + HttpContext.Current.Request.FilePath + " parametros = " + JsonConvert.SerializeObject(dictionary);
                }
                catch (Exception)
                {
                }

                var usuarioLogado = Usuario.GetUsuarioPontoWebLogadoCache();
                var client = new BackgroundJobClient();
                string nomeQueue = BLL.cwkFuncoes.RemoveAcentosECaracteresEspeciais(Environment.MachineName.ToLower());
                var state = new EnqueuedState(nomeQueue);
                string id = client.Create(() => DoWorkJob(job.Id, processo, "Data Base = " + usuarioLogado.DataBase + "; Usuário = " + usuarioLogado.Login, requestFromPost), state);
                job.IdHangFire = id;

            }
            catch (Exception ex)
            {
                FinallyJob(job);

                throw ex;
            }            

            return job;
        }

        private void BroadcastJobStatus(Job job)
        {
            job.ProgressChanged += HandleJobProgressChanged;
            job.Completed += HandleJobCompleted;
        }

        private void HandleJobCompleted(object sender, EventArgs e)
        {
            var job = (Job)sender;
            bool temArquivo = VerificaExistenciaArquivo(job.Id);

            string json = JsonConvert.SerializeObject(job);
            _hubContext.Clients.Group(job.Id).jobCompleted(job.Id, temArquivo, json);
            job.ProgressChanged -= HandleJobProgressChanged;
            job.Completed -= HandleJobCompleted;
        }

        private void HandleJobProgressChanged(object sender, EventArgs e)
        {
            var job = (Job)sender;
            _hubContext.Clients.Group(job.Id).progressChanged(job.Id, job.Progress, job.msgProgress);
        }

        public Job GetJob(string id)
        {
            Job result;
            result = Instance._runningJobs.TryGetValue(id, out result) ? result : null;
            if (result != null)
            {
                JobData jobData = connection.GetJobData(result.IdHangFire);
                string stateName = jobData.State;
                //Enqueued, Processing, Scheduled
                if (new String[] { "Deleted", "Failed", "Succeeded" }.Contains(stateName) && !result.IsComplete && !result.IsErro)
                {
                    FinallyJob(result);
                }
                else if (new String[] { "Enqueued", "Scheduled" }.Contains(stateName))
                {
                    result.ReportMsgProgress("Seu processo esta na fila, logo estaremos processando, aguarde!");
                }
            }
            return result;
        }

        #region Trata Cache Job
        static void AddJobCache(Job job)
        {
            Cacher<Job> cache = new Cacher<Job>("Job", () => job);
            cache.Refresh();
        }

        public static Job GetJobCache()
        {
            Job jobCache = new Job("");
            Cacher<Job> cache2 = new Cacher<Job>("Job", () => jobCache);
            jobCache = cache2.Value;
            if (jobCache != null && !string.IsNullOrEmpty(jobCache.Id))
            {
                jobCache = Instance.GetJob(jobCache.Id);
                if (jobCache == null || String.IsNullOrEmpty(jobCache.Id))
                {
                    RemoveJobCache(jobCache);
                }
            }

            if (jobCache == null)
            {
                return new Job("");
            }
            return jobCache;
        }

        private static void RemoveJobCache(Job job)
        {
            Cacher<Job> cache = new Cacher<Job>("Job", () => job);
            cache.Clear();
        }

        private static string nomeCacheArquivo(string idJob)
        { return "Arquivo" + idJob; }
        public static void AdicionaArquivoCache(ActionResult arquivo, string idJob)
        {
            Cache cache = HttpRuntime.Cache;
            cache.Insert(nomeCacheArquivo(idJob), arquivo);
        }


        public static ActionResult GetArquivoCache(string idJob)
        {
            ActionResult arquivo = null;

            Cache cacheRecuperada = HttpRuntime.Cache;

            var item = cacheRecuperada.Get(nomeCacheArquivo(idJob));

            if (item != null)
            {
                arquivo = (ActionResult)item;
            }
            cacheRecuperada.Remove(nomeCacheArquivo(idJob));
            return arquivo;
        }

        static bool VerificaExistenciaArquivo(string idJob)
        {
            bool retorno = false;
            Cache cacheRecuperada = HttpRuntime.Cache;
            var item = cacheRecuperada.Get(nomeCacheArquivo(idJob));

            if (item != null)
                retorno = true;

            return retorno;
        }


        #endregion
    }
}