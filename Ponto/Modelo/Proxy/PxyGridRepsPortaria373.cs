namespace Modelo.Proxy
{
    public class PxyGridRepsPortaria373
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.select, OrderType.none)]
        public string Id { get { return string.Format("{0}|{1}", this.IdEmpresa, this.NumRelogio); } }

        [TableHTMLAttribute("Núm. Relógio", 1, true, ItensSearch.select, OrderType.asc)]
        public string NumRelogio { get; set; }
        [TableHTMLAttribute("Relógio", 2, true, ItensSearch.select, OrderType.none)]
        public string App { get; set; }
        [TableHTMLAttribute("Código Empresa", 3, true, ItensSearch.select, OrderType.none)]
        public int EmpresaCodigo { get; set; }
        [TableHTMLAttribute("CNPJ Empresa", 4, true, ItensSearch.select, OrderType.none)]
        public string EmpresaCnpj { get; set; }
        [TableHTMLAttribute("Nome Empresa", 5, true, ItensSearch.select, OrderType.none)]
        public string EmpresaNome { get; set; }

        public int IdEmpresa { get; set; }
    }
}
