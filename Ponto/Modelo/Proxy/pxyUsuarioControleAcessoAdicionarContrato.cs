using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyUsuarioControleAcessoAdicionarContrato
    {
        public int IdContrato { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        public string Nome { get; set; }
    }
}