using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridRelatorioManutencaoDiaria
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Departamento", 2, true, ItensSearch.text, OrderType.asc)]
        public string Departamento { get; set; }

        [TableHTMLAttribute("Empresa", 3, true, ItensSearch.select, OrderType.none)]
        public string Empresa { get; set; }
    }
}
