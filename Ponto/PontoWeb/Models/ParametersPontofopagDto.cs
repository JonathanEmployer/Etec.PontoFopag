using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class ParametersPontofopagDto
    {
        public ConnectionDataBaseDto DataBase { get; set; }
        public bool EnableEPays { get; set; }
        public string TokenPontofopag { get; set; }
        public string Cnpj { get; set; }
        public string UserEPays { get; set; }
        public string PasswordEPays { get; set; }

    }
}