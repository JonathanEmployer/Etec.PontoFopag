using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio.Jobs
{
    public class ReiniciaServico : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async Task Execute(IJobExecutionContext context)
        {
            log.Warn("*************** Reiniciando Serviço*************");
            Process process = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "cmd.exe",
                Arguments = @"/c net stop Pontofopag.ServRepPFP & sc start Pontofopag.ServRepPfp"
            };
            process.StartInfo = processInfo;
            process.Start();
        }
    }
}
