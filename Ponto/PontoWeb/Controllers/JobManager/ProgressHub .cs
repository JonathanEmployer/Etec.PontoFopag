using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace PontoWeb.Controllers.JobManager
{
    public class ProgressHub: Hub
    {
        public void TrackJob(string jobId)
        {
            if (jobId != null)
            {
                Groups.Add(Context.ConnectionId, jobId);  
            }    
        }

        public void CancelJob(string jobId)
        {
            var job = JobManager.Instance.GetJob(jobId);
            if (job != null)
            {
                job.Cancel();
            }
        }

        public void ProgressChanged(string jobId, int progress, string msgProgress)
        {
            Clients.Group(jobId).progressChanged(jobId, progress, msgProgress);
        }

        public void JobCompleted(string jobId)
        {
            Clients.Group(jobId).jobCompleted(jobId);
        }
    }
}