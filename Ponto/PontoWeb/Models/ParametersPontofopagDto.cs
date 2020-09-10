using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class ParametersPontofopagDto
    {
        public bool EnableEPays { get; set; }
        public string UserEPays { get; set; }
        public string PasswordEPays { get; set; }
    }
}