using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyUsuarioControleAcessoAdicionarContrato
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Tipo", 1, false, ItensSearch.none, OrderType.none)]
        public string Tipo { get; set; }

        [TableHTMLAttribute("Código", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Nome", 3, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

    }
}