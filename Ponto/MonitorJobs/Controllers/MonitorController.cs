using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Modelo.EntityFramework.MonitorPontofopag;
using MonitorJobs.Models;
using Hangfire;

namespace MonitorJobs.Controllers
{
    public class MonitorController : Controller
    {
        private MONITOR_PONTOFOPAGEntities db = new MONITOR_PONTOFOPAGEntities();

        // GET: Monitor
        public async Task<ActionResult> Index()
        {
            MonitorJobViewModel monitorJobViewModel = new MonitorJobViewModel();

            try
            {
                List<MonitorStatus> monitorStatus = new List<MonitorStatus>();

                var query = db.Job.Where(c => c.StateName != "Deleted" && c.StateName != "Succeeded" && c.CreatedAt > new DateTime(2022, 01, 01))
               .GroupBy(p => new { p.StateName })
               .Select(g => new MonitorStatus { Nome = g.Key.StateName , Quantidade = g.Count() });

                var Jobs = await db.Job.Where(c => c.CreatedAt > new DateTime(2022, 01, 20)).OrderByDescending(c => c.CreatedAt).Select( c => c.StateName ).ToListAsync();

                List<int> jobControls = await db.JobControl.Where(c => c.StatusNovo != "Deleted" && c.StatusNovo != "Succeeded" && c.Inchora > new DateTime(2022, 01, 01)).OrderByDescending(c => c.Inchora).Select(c => c.JobId).ToListAsync();

                monitorJobViewModel.Jobs = await db.Job.Where(c => jobControls.Contains(c.Id)).OrderByDescending(c => c.CreatedAt).Take(200).ToListAsync();
                monitorJobViewModel.JobsSegundoPlano = await db.Job.Where(c => c.StateName != "Deleted" && c.StateName != "Succeeded" && c.CreatedAt > new DateTime(2022, 01, 01)).OrderByDescending(c => c.CreatedAt).Take(200).ToListAsync();
                monitorJobViewModel.monitorStatus = query.ToList();

                Parallel.ForEach(monitorJobViewModel.Jobs, c => c.CreatedAt = c.CreatedAt.AddHours(-3));
                Parallel.ForEach(monitorJobViewModel.JobsSegundoPlano, c => c.CreatedAt = c.CreatedAt.AddHours(-3));

                string ids = "";
                foreach (var item in monitorJobViewModel.JobsSegundoPlano.Take(200))
                {
                    if (ids != "")
                        ids += ","; 
                    ids += item.Id.ToString();

                }
                monitorJobViewModel.IdsJobs = ids;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(monitorJobViewModel);
        }

        // GET: Monitor/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Job.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Monitor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Monitor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,StateId,StateName,InvocationData,Arguments,CreatedAt,ExpireAt")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Job.Add(job);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET: Monitor/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Job.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Monitor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,StateId,StateName,InvocationData,Arguments,CreatedAt,ExpireAt")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: Monitor/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Job.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Monitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await db.Job.FindAsync(id);
            db.Job.Remove(job);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        //[HttpPost]
        public ActionResult Reprocessar(string IdsJobs)
        {
            if (IdsJobs != null)
            {
                List<string> idsProcessar = new List<string>();
                if (IdsJobs.Contains(','))
                {
                    idsProcessar = IdsJobs.Split(',').ToList();
                }
                else
                {
                    idsProcessar = IdsJobs.Split(
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
            }
            return RedirectToAction("Index");
        }

        public ActionResult ReprocessarId(string Id)
        {
            var client = new BackgroundJobClient();
            client.Requeue(Id);
            return RedirectToAction("Index");
        }

        public void DeletarId(string Id)
        {
            var client = new BackgroundJobClient();
            client.Delete(Id);
        }

        public ActionResult Deletar(string Id)
        {
            if (Id != null)
            {
                List<string> idsProcessar = new List<string>();
                if (Id.Contains(','))
                {
                    idsProcessar = Id.Split(',').ToList();
                }
                else
                {
                    idsProcessar = Id.Split(
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
                        client.Delete(jobId);
                    }
                }
            }
            return RedirectToAction("Index");
        }

    }
}
