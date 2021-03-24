using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Hangfire;
using Hangfire.States;
using System.Text;
using System.IO;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using BLL_N.JobManager.Hangfire;
using PontoWeb.Controllers.BLLWeb;
using Newtonsoft.Json;

namespace PontoWeb.Controllers
{
    [Authorize]
    public class JobManagerController : Controller
    {

        [Authorize]
        public ActionResult Index()
        {
            List<PxyJobReturn> lista = GetJobsParaRetorno(0);
            return View(lista);
        }


        [Authorize]
        // GET: JobManager
        public ActionResult LoadJobs(int skip)
        {
            List<PxyJobReturn> lista = GetJobsParaRetorno(skip);
            return Json(lista, JsonRequestBehavior.DenyGet);
        }


        [Authorize]
        public List<PxyJobReturn> GetJobsParaRetorno(int skip)
        {
            List<PxyJobReturn> lista = new List<PxyJobReturn>();
            String urlReference = HttpContext.Request.UrlReferrer == null ? HttpContext.Request.Url.AbsolutePath : HttpContext.Request.UrlReferrer.AbsolutePath;
            
            if (urlReference.Contains("/JobManager/Index") || urlReference.Contains("/JobManager"))
            {
                urlReference = "";
            }

            string user = HttpContext.User.Identity.Name;

            List<JobControl> jobs = new List<JobControl>();
            lista = JobControlManager.GetJobsPagina(skip, 10, urlReference, user);
            
            return lista;
        }


        [Authorize]
        public ActionResult ReprocessarJob(string jobId)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();

            int id = 0;
            int.TryParse(jobId, out id);
            JobControl job = new JobControl();
            if (id > 0)
            {
                job = JobControlManager.GetJobControl(id.ToString());
                if (job.JobId > 0)
                {
                    if (usr.ServicoCalculo == 0)
                    {
                        var client = new BackgroundJobClient();
                        client.Requeue(jobId);
                    }
                    else
                    {
                        BLL.RabbitMQ.RabbitMQ rabbitMQ = new BLL.RabbitMQ.RabbitMQ();
                        var loteEnviar = new
                        {
                            IdJobControl = job.Id,
                            DataBase = usr.DataBase
                        };
                        rabbitMQ.SendMessage("Pontofopag_Calculo_Dados", JsonConvert.SerializeObject(loteEnviar));
                    }
                    return Json(new PxyJobReturn(job), JsonRequestBehavior.DenyGet);

                }
            }

            job.JobId = id;
            job.Mensagem = "Não foi possível reexecutar o processo "+jobId+", não encontrado";
            job.Progresso = -9;
            return Json(new PxyJobReturn(job), JsonRequestBehavior.DenyGet);
        }

        [Authorize]
        public ActionResult DeleteJob(string jobId)
        {
            int id = 0;
            int.TryParse(jobId, out id);
            JobControl job = new JobControl();
            if (id > 0)
            {
                job = JobControlManager.GetJobControl(id.ToString());
                if (job.JobId > 0)
                {
                    var client = new BackgroundJobClient();
                    client.Delete(jobId);
                    JobControlManager.SetUsuarioCancelamento(id, HttpContext.User.Identity.Name);
                    job = JobControlManager.GetJobControl(id.ToString());
                    var jobHangfire = JobStorage.Current.GetMonitoringApi().JobDetails(jobId);
                    using (var con = JobStorage.Current.GetConnection())
                    {
                        var jobState = con.GetStateData(jobId);
                        if (jobState == null || jobState.Name == DeletedState.StateName)
                        {
                            job.Progresso = -8;
                            job.Mensagem = "Processo excluído/cancelado";
                            if (jobState == null) // Job esta com algum problema
                            {
                                job.StatusNovo = "Deleted";
                                JobControlManager.JobControlUpdate(job);
                            }
                        }
                        else
                        {
                            job.Progresso = -2;
                            job.Mensagem = "Aguarde, seu job será cancelado.";
                            job.Reprocessar = false;
                        }
                    }
                    return Json(new PxyJobReturn(job), JsonRequestBehavior.DenyGet);
                }
            }

            job.JobId = id;
            job.Mensagem = "Não foi possível reexecutar o processo " + jobId + ", não encontrado";
            job.Progresso = -9;
            job.Reprocessar = false;
            return Json(new PxyJobReturn(job), JsonRequestBehavior.DenyGet);
        }

        [Authorize]
        public PartialViewResult JobView()
        {
            return PartialView();
        }

        public FileResult FileUpload(string jobId)
        {
            int id = 0;
            int.TryParse(jobId, out id);
            JobControl job = new JobControl();
            FileContentResult result = null;
            if (id > 0)
            {
                job = JobControlManager.GetJobControl(id.ToString());
                try
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(job.FileUpload);
                    string fileName = Path.GetFileName(job.FileUpload);
                    return File(fileBytes, "text/plain", fileName);
                }
                catch (DirectoryNotFoundException)
                {
                    result = ReturnFileErro(jobId, "Arquivo não encontrado");
                    return result;
                }
                catch (Exception ex)
                {
                    result = ReturnFileErro(jobId, ex.Message);
                    return result;
                }

            }
            result = ReturnFileErro(jobId, "O processo solicitado não foi encontrado");
            return result;
        }

        public FileResult FileDownload(string jobId)
        {
            int id = 0;
            int.TryParse(jobId, out id);
            JobControl job = new JobControl();
            FileContentResult result = null;
            if (id > 0)
            {
                job = JobControlManager.GetJobControl(id.ToString());
                try
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(job.FileDownload);
                    string fileName = Path.GetFileName(job.FileDownload);
                    return File(fileBytes, "text/plain", fileName);
                }
                catch (DirectoryNotFoundException)
                {
                    result = ReturnFileErro(jobId, "Arquivo não encontrado");
                    return result;
                }
                catch (Exception ex)
                {
                    result = ReturnFileErro(jobId, ex.Message);
                    return result;
                }

            }
            result = ReturnFileErro(jobId, "O processo solicitado não foi encontrado");
            return result;
        }

        private static FileContentResult ReturnFileErro(string jobId, string mensagem)
        {
            var contentType = "text/plain";
            var content = mensagem;
            var bytes = Encoding.UTF8.GetBytes(content);
            var result = new FileContentResult(bytes, contentType);
            result.FileDownloadName = "Retorno processo" + jobId + ".txt";
            return result;
        }
    }
}