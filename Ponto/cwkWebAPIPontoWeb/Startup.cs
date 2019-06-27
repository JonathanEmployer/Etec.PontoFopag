using System;
using System.Collections.Generic;
using System.Linq;
using BLL_N.JobManager.Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(cwkWebAPIPontoWeb.Startup))]

namespace cwkWebAPIPontoWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureHangfire();
        }

        private static void ConfigureHangfire()
        {
            HangfireConfig hangConfig = new HangfireConfig();
            hangConfig.ConfigureHangfireClient();
        }
    }
}
