using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyFechamentoBHD
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Nome", 1, true, ItensSearch.text, OrderType.none)]
        public virtual string Nome { get; set; }

        [TableHTMLAttribute("Seq", 2, true, ItensSearch.text, OrderType.none)]
        public int Seq { get; set; }

        [TableHTMLAttribute("Identificacao", 3, true, ItensSearch.text, OrderType.none)]
        public int Identificacao { get; set; }

        [TableHTMLAttribute("Crédito", 4, true, ItensSearch.text, OrderType.none)]
        public string Credito { get; set; }

        [TableHTMLAttribute("Débito", 5, true, ItensSearch.text, OrderType.none)]
        public string Debito { get; set; }

        [TableHTMLAttribute("Horas Pagas", 6, true, ItensSearch.text, OrderType.none)]
        public string Saldo { get; set; }

        [TableHTMLAttribute("Saldo BH", 7, true, ItensSearch.text, OrderType.none)]
        public string Saldobh { get; set; }
    }
}
