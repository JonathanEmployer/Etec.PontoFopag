using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyUsuarioControleAcessoCopiar
    {

        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Login", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [TableHTMLAttribute("Nome", 3, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

    }
}
