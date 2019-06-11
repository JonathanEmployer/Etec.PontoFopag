using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class TotalizadorEspelhoPonto
    {
        public string horasTrabDiurna { get; set; }
        public string horasTrabNoturna { get; set; }
        public string horasExtraDiurna { get; set; }
        public string horasExtraNoturna { get; set; }
        public string horasFaltaDiurna { get; set; }
        public string horasFaltaNoturna { get; set; }
        public string horasDDSR { get; set; }
        public string creditoBHPeriodo { get; set; }
        public string debitoBHPeriodo { get; set; }
        public string saldoAnteriorBH { get; set; }
        public string sinalsaldoAnteriorBH { get; set; }
        public string saldoAtualBH { get; set; }
        public string sinalsaldoAtualBH { get; set; }
        public string saldoBHPeriodo { get; set; }
        public string sinalsaldoBHPeriodo { get; set; }
        

        public List<RateioHorasExtras> rateioHorasExtras { get; set; }
    }
}