using MonitorJobs.App_Start;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MonitorJobs
{
    public class ApplicationPreload : IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            StreamWriter fs = new StreamWriter(@"c:\temp\log.txt", true);

            fs.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " started ");

            fs.Dispose();
            HangfireBootstrapper.Instance.Start();
        }
    }
}