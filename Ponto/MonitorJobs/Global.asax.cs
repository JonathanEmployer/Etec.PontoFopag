using MonitorJobs.App_Start;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MonitorJobs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HangfireBootstrapper.Instance.Start();
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/log4net.config")));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            string cs = (context.Session == null || context.Session["CS"] == null) ? "" : context.Session["CS"].ToString();
            Exception exception = Server.GetLastError();
            LogErro.LogarErro(exception);

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;
                int codigoErro = httpException.GetHttpCode();
                switch (codigoErro)
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    //case 500:
                    //    // server error
                    //    action = "HttpError500";
                    //    break;
                    default:
                        {
                            action = "Erro";
                        }
                        break;
                }
                // clear error on server
                Server.ClearError();
                string msg = exception.Message.Replace("\\n", "");
                Response.Redirect(String.Format("~/Erros/{0}/?mensagem={1}", action, msg));
            }
            else
            {
                string msg = exception.Message.Replace("\\n", "");
                Response.Redirect(String.Format("~/Erros/{0}/?mensagem={1}", "Erro", msg));
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            HangfireBootstrapper.Instance.Stop();
        }
    }
}
