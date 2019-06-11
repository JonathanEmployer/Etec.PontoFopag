using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Security
{
    public class PermissoesFiltro : AuthorizeAttribute
    {
        public string AccessDeniedViewName { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result is HttpUnauthorizedResult)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    //filterContext.HttpContext.Response.Redirect("/Usuario/Negado");
                    if (string.IsNullOrWhiteSpace(AccessDeniedViewName))
                        AccessDeniedViewName = "~/Usuario/Negado";

                    var requestUrl = filterContext.HttpContext.Request.UrlReferrer;

                    filterContext.Result = new RedirectResult(String.Format("{0}?RequestUrl={1}", AccessDeniedViewName, requestUrl));
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("/Usuario/LogIn");
                }
            }
        }
    }
}