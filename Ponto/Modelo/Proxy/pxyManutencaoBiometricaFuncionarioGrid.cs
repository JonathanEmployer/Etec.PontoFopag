using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyManutencaoBiometricaFuncionarioGrid
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public string Codigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Matrícula", 3, true, ItensSearch.text, OrderType.none)]
        public string Matricula { get; set; }

        [TableHTMLAttribute("Horário", 4, true, ItensSearch.select, OrderType.none)]
        public string Horario { get; set; }

        [TableHTMLAttribute("Empresa", 5, true, ItensSearch.select, OrderType.none)]
        public string Empresa { get; set; }

        [TableHTMLAttribute("Departamento", 6, true, ItensSearch.select, OrderType.none)]
        public string Departamento { get; set; }
    }
}
