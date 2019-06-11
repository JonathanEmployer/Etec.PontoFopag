using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridDepartamentoRelatorioFunc
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Empresa", 2, true, ItensSearch.select, OrderType.none)]
        public string Empresa { get; set; }
    }
}
