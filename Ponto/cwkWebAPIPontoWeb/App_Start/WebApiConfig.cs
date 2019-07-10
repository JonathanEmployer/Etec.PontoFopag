using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using cwkWebAPIPontoWeb.Controllers;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace cwkWebAPIPontoWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuração e serviços de API Web
            // Configure a API Web para usar somente a autenticação de token de portador.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Rotas de API Web
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new SaveRawPostData());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
