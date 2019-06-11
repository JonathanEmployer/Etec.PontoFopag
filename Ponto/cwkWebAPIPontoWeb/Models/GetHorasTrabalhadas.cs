using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class GetHorasTrabalhadas
    {
        public List<string> Cpfs { get; set; }
        public string DataFinal { get; set; }
        public string DataInicio { get; set; }
    }
}