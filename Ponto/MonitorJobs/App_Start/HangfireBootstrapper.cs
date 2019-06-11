using BLL_N.JobManager.Hangfire;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Web.Hosting;

namespace MonitorJobs.App_Start
{
    public class HangfireBootstrapper : IRegisteredObject
    {
        public static readonly HangfireBootstrapper Instance = new HangfireBootstrapper();

        private readonly object _lockObject = new object();
        private bool _started;

        private BackgroundJobServer _backgroundJobServer;

        private HangfireBootstrapper()
        {
        }

        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;
                _started = true;

                HostingEnvironment.RegisterObject(this);

                HangfireConfig hangConfig = new HangfireConfig();
                hangConfig.ConfigureHangfireClient();
                _backgroundJobServer = hangConfig.ConfigureHangfireServer();

                SimpleSchedulerProvider s = new SimpleSchedulerProvider();
                s.Init();
            }
        }

        public void Stop()
        {
            lock (_lockObject)
            {
                if (_backgroundJobServer != null)
                {
                    _backgroundJobServer.Dispose();
                }

                HostingEnvironment.UnregisterObject(this);
            }
        }

        void IRegisteredObject.Stop(bool immediate)
        {
            Stop();
        }
    }
}