using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Funcao : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descri��o da Fun��o
        /// </summary>       
        [TableHTMLAttribute("Descri��o", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descri��o")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public string Descricao { get; set; }
        public int? idIntegracao { get; set; }
    }
}
