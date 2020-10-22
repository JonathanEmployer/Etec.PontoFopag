using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyRegistrosValidarPontoExcecao : BilhetesImp
    {
        public DateTime DataMarcacacao { get; set; }
        public string Legenda { get; set; }
        public string EntradaPrevista1 { get; set; }
        public string SaidaPrevista1 { get; set; }
        public string EntradaPrevista2 { get; set; }
        public string SaidaPrevista2 { get; set; }
        public string EntradaPrevista3 { get; set; }
        public string SaidaPrevista3 { get; set; }
        public string EntradaPrevista4 { get; set; }
        public string SaidaPrevista4 { get; set; }
        public bool PontoPorExcecao { get; set; }
        public int IdHorario { get; set; }
    }
}
