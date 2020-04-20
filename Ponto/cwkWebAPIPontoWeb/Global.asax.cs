using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace cwkWebAPIPontoWeb
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            this.Error += WebApiApplication_Error;
            //GlobalConfiguration.Configuration.Filters.Add(new LogAPI());

        }

        void WebApiApplication_Error(object sender, EventArgs e)
        {
            Employer.PlataformaLog.LogError.WriteErrorGlobal(this.Context);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Employer.PlataformaLog.LogError.Dispose();
        }
    }
}
