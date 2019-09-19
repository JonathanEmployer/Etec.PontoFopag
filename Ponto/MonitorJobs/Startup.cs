using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using MonitorJobs.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using Hangfire.SqlServer;
using Quartz;
using Quartz.Impl;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(MonitorJobs.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace MonitorJobs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureAuth(app);
                //CriaUsuarioPadrao();

                ConfigHangFireDashboard(app);
                Modelo.AutoMapper.ConfigureAutoMapper.Initialize();
            }
            catch (Exception e)
            {
                LogErro.LogarErro(e);
                throw;
            }
        }

        private static void ConfigHangFireDashboard(IAppBuilder app)
        {
            var optionsDash = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthFilter() }
            };
            app.UseHangfireDashboard("/hangfire", optionsDash);
        }

        private void CriaUsuarioPadrao()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (context.Users.Select(s => s.UserName == "job@pontofopag.com.br").Count() == 0)
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                    var user = new ApplicationUser();
                    user.UserName = "job@pontofopag.com.br";
                    user.Email = "job@pontofopag.com.br";

                    string userPWD = "Pfp#j0b";

                    var chkUser = UserManager.Create(user, userPWD);
                }
            }
            
        }

        public class HangfireAuthFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                // In case you need an OWIN context, use the next line, `OwinContext` class
                // is the part of the `Microsoft.Owin` package.
                var owinContext = new OwinContext(context.GetOwinEnvironment());

                // Allow all authenticated users to see the Dashboard (potentially dangerous).
                return owinContext.Authentication.User.Identity.IsAuthenticated;
            }
        }
    }
}
