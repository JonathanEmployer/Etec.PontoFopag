﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Utils
{
    public static class WebApiUri
    {
        public static string UriApi = "https://localhost:5001/";
        public static string IdentityServerUri = "https://identityserver.webfopag.com.br/";
    }
    public static class TokenUri
    {
        public static string Token = WebApiUri.IdentityServerUri + "connect/token";
    }
    public static class UriLogin
    {
        public static string Login = WebApiUri.IdentityServerUri + "token/get";
    }
    public static class UriMongo
    {
        public static string Parameters = WebApiUri.UriApi + "Configuration/Parameters";
    }
}