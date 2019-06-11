using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridEnvioDadosRepFunc
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Pis", 3, true, ItensSearch.text, OrderType.none)]
        public string Pis { get; set; }

        [TableHTMLAttribute("Empresa", 4, true, ItensSearch.text, OrderType.none)]
        public string Empresa { get; set; }

        [TableHTMLAttribute("Departamento", 5, true, ItensSearch.text, OrderType.none)]
        public string Departamento { get; set; }

        [TableHTMLAttribute("Função", 6, true, ItensSearch.text, OrderType.none)]
        public string Funcao { get; set; }

    }
}
