using Modelo.EntityFramework.MonitorPontofopag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitorJobs.Models
{
    public class MonitorJobViewModel
    {
        public List<Job> Jobs { get; set; }
        public List<Job> JobsSegundoPlano { get; set; }
        public List<MonitorStatus> monitorStatus { get; set; }
        //public List<JobControl> JobControls { get; set; }
        public string IdsJobs { get; set; }
    }

    public class MonitorStatus
    {
        public string Nome { get; set; }
        public int Quantidade { get; set; }
    }
}