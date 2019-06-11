using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class Alocacao : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
     
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        [Display(Name = "Endereço")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Endereço", 3, true, ItensSearch.text, OrderType.none)]
        public string Endereco { get; set; }
    }
}
