using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Providers;
using PontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PontoWeb
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
            ModelBinders.Binders.Add( typeof(decimal), new DecimalModelBinder()); ModelBinders.Binders.Add( typeof(decimal?), new DecimalModelBinder());
            this.Error += WebApiApplication_Error;
            FilterProviders.Providers.Add(new AntiForgeryTokenFilterProvider());
            UnityConfig.RegisterComponents();
        }

        void WebApiApplication_Error(object sender, EventArgs e)
        {
            Employer.PlataformaLog.LogError.WriteErrorGlobal(this.Context);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            if (exception.Message == "Erro ao recuperar usuário")
            {
                Response.Redirect("~/Usuario/LogIn");
            }
            else
            {

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
                    if (User.Identity.IsAuthenticated)
                    {
                        Response.Redirect(String.Format("~/Home/{0}/?mensagem={1}", action, exception.Message));
                    }
                    else
                    {
                        Response.Redirect(String.Format("~/Page/{0}/?mensagem={1}", action, exception.Message));
                    }
                }
                else
                {
                    if (User != null && User.Identity.IsAuthenticated)
                    {
                        Response.Redirect(String.Format("~/Home/{0}/?mensagem={1}", "Erro", exception.Message));
                    }
                    else
                    {
                        Response.Redirect(String.Format("~/Page/{0}/?mensagem={1}", "Erro", exception.Message));
                    }
                }
            }
            Employer.PlataformaLog.LogError.Dispose();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Employer.PlataformaLog.LogError.Dispose();
        }
    }
}
