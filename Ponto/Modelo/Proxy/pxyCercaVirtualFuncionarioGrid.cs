namespace Modelo.Proxy
{
    public class pxyCercaVirtualFuncionarioGrid
    {
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public string Codigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Matrícula", 3, true, ItensSearch.text, OrderType.none)]
        public string Matricula { get; set; }

        [TableHTMLAttribute("Empresa", 4, true, ItensSearch.select, OrderType.none)]
        public string Empresa { get; set; }
        [TableHTMLAttribute("Função", 5, true, ItensSearch.select, OrderType.none)]
        public string Funcao { get; set; }
        [TableHTMLAttribute("Departamento", 6, true, ItensSearch.select, OrderType.none)]
        public string Departamento { get; set; }
        [TableHTMLAttribute("Contrato", 7, true, ItensSearch.select, OrderType.none)]
        public string Contrato { get; set; }
    }
}
