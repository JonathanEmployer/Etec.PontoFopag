using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyGridLocaisHorasExtrasLocal
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoLocal { get; set; }

        [TableHTMLAttribute("Local", 2, true, ItensSearch.text, OrderType.none)]
        public string Local { get; set; }
    }
}
