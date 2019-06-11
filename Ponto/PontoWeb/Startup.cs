using Owin;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Hangfire.Console;
using BLL_N.JobManager.Hangfire;

[assembly: OwinStartup(typeof(PontoWeb.Startup))]

namespace PontoWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
            ConfigureHangfire();

            //var options = new DashboardOptions
            //{
            //    Authorization = new[] { new HangfireAuthFilter() }
            //};

            //app.UseHangfireDashboard("/hangfire", options);

            //var optionsServer = new BackgroundJobServerOptions
            //{
            //    //A ordem do hangfire é alfabética, não importa os nomes.
            //    Queues = new[] { PontoWeb.Utils.Conversores.RemoveAcentosECaracteresEspeciais(Environment.MachineName.ToLower()) }
            //};
            //app.UseHangfireServer(optionsServer);
        }

        private static void ConfigureHangfire()
        {
            HangfireConfig hangConfig = new HangfireConfig();
            hangConfig.ConfigureHangfireClient();
        }
    }

    //public class HangfireAuthFilter : IDashboardAuthorizationFilter
    //{
    //    public bool Authorize([NotNull] DashboardContext context)
    //    {
    //        //var context = new OwinContext(owinEnvironment);
    //        var hContext = HttpContext.Current;
    //        IPrincipal user = hContext.User;
    //        string userHangfire = ConfigurationManager.AppSettings["UserHangfire"];
    //        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated || userHangfire.ToUpper() != user.Identity.Name.ToUpper())
    //        {
    //            return false;
    //        }            

    //        return true;
    //    }
    //}
}
