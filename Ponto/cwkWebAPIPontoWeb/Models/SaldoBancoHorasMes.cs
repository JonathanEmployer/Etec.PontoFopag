using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class SaldoBancoHorasMes
    {
        public string MesAno { get; set; }
        public DateTime Data { get; set; }
        public string BancoHorasMensal { get; set; }
        public string BancoHorasAcumulado { get; set; }
        public bool ConfirmadoPainel { get; set; }
    }
}