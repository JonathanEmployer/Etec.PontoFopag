using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Controllers.JobManager.Interfaces
{
    public interface IJobManager
    {
        Job DoJobAsync(Action<Job> action);
        Job GetJob(string id);
    }
}