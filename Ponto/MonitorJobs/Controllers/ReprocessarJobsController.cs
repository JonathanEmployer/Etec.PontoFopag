using BLL_N.JobManager.Hangfire;
using Hangfire;
using Modelo.EntityFramework.MonitorPontofopag;
using MonitorJobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonitorJobs.Controllers
{
    public class ReprocessarJobsController : Controller
    {
        // GET: ReprocessarJobs
        public ActionResult Index()
        {
            return View(new ListaJobs());
        }

        [HttpPost]
        public ActionResult Index(ListaJobs listaJobs)
        {
            List<string> idsProcessar = new List<string>();
            if (listaJobs.IdsJobs.Contains(','))
            {
                idsProcessar = listaJobs.IdsJobs.Split(',').ToList();
            }
            else
            {
                idsProcessar = listaJobs.IdsJobs.Split(
                                                    new[] { Environment.NewLine },
                                                    StringSplitOptions.None
                                                ).ToList();
            }
            foreach (var jobId in idsProcessar)
            {
                int id = 0;
                int.TryParse(jobId, out id);
                if (id > 0)
                {
                        var client = new BackgroundJobClient();
                        client.Requeue(jobId);
                }
            }
            return View(listaJobs);
        }

    }
}