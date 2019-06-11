using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridHorariosRelatorioFunc
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.text, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        [TableHTMLAttribute("Tipo", 3, true, ItensSearch.text, OrderType.none)]
        public string TipoHorario { get; set; }
    }
}
