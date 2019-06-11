using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class RelatorioSaldoBancoHoras
    {
        public string MesInicio { get; set; }
        public string AnoInicio { get; set; }
        public string MesFim { get; set; }
        public string AnoFim { get; set; }
        public List<Modelo.Proxy.PxyCPFMatricula> CPFsMatriculas { get; set; }
    }
}