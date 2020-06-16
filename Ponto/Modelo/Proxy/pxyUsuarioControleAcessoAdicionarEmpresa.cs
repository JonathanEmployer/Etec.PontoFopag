using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyUsuarioControleAcessoAdicionarEmpresa
    {
        public int Idempresa { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [TableHTMLAttribute("CNPJ/CPF", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "CNPJ/CPF")]
        public string CpfCnpj { get; set; }
    }
}