using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Parâmetros de carga do relatório de classificação de horas extras.
    /// </summary>
    public class RelatorioTratamentoDePontoParam
    { 
        public string InicioPeriodo { get; set; }
        public string FimPeriodo { get; set; }

        public List<Modelo.Proxy.PxyCPFMatricula> CPFsMatriculas { get; set; }
    }
}