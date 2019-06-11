using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridFechamentoPontoFunc
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DsCodigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Empresa", 3, true, ItensSearch.text, OrderType.asc)]
        public string Empresa { get; set; }

        [TableHTMLAttribute("Departamento", 4, true, ItensSearch.text, OrderType.asc)]
        public string Departamento { get; set; }

        [TableHTMLAttribute("Função", 5, true, ItensSearch.text, OrderType.asc)]
        public string Funcao { get; set; }

        [TableHTMLAttribute("Alocação", 6, true, ItensSearch.text, OrderType.asc)]
        public string Alocacao { get; set; }
    }
}
