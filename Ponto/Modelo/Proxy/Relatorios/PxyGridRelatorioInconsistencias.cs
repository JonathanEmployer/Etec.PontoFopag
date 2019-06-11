using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyGridRelatorioInconsistencias
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.none)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Empresa", 3, true, ItensSearch.text, OrderType.none)]
        public string Empresa { get; set; }

        [TableHTMLAttribute("Departamento", 4, true, ItensSearch.text, OrderType.none)]
        public string Departamento { get; set; }

        [TableHTMLAttribute("Função", 5, true, ItensSearch.text, OrderType.none)]
        public string Funcao { get; set; }

        [TableHTMLAttribute("Alocação", 6, true, ItensSearch.text, OrderType.none)]
        public string Alocacao { get; set; }

        [TableHTMLAttribute("Horário", 8, true, ItensSearch.select, OrderType.none)]
        public string DescHorario { get; set; }
    }
}
