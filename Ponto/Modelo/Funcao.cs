using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Funcao : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descrição da Função
        /// </summary>       
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Descricao { get; set; }
        public int? idIntegracao { get; set; }
    }
}
