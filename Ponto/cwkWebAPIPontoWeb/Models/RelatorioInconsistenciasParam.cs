using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class RelatorioInconsistenciasParam
    {
        public bool Interjornada            { get; set; }
        public bool Intrajornada            { get; set; }
        public bool SetimoDiaTrabalhado     { get; set; }
        public bool LimiteHorasTrabalhadas  { get; set; }
        public bool TerceiroDomTrabalhado   { get; set; }
        public bool SeisHorasSemIntervalo   { get; set; }

        public string InicioPeriodo         { get; set; }
        public string FimPeriodo            { get; set; }
        public List<Modelo.Proxy.PxyCPFMatricula> CPFsMatriculas { get; set; }
    }
}