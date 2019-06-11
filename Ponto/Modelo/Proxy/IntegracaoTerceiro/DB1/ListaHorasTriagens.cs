using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.IntegracaoTerceiro.DB1
{
    public class ListaHorasTriagens
    {
        public string Cpf { get; set; }
        public string Usuario { get; set; }
        public List<HorasTriagens> Triagens { get; set; }
    }


    public class HorasTriagens
    {
        public string Data { get; set; }
        public decimal QuantidadeHoras { get; set; }
    }
}
