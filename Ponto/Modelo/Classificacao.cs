using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Classificacao : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
         [Display(Name = "Descricao")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
         [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)] 
        public String Descricao { get; set; }
         [Display(Name = "Exibe no Painel do RH")]
         public bool ExibePaineldoRH { get; set; }
         [TableHTMLAttribute("Exibe no Painel do RH", 3, true, ItensSearch.text, OrderType.none)]
         public string ExibePaineldoRHStr
         {
             get
             {
                 return ExibePaineldoRH == true ? "Sim" : "Não";
             }
         }
         public string SelecionadoStr { get; set; }
    }
}
