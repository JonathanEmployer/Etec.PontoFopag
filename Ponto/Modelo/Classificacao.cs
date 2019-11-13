using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Classificacao : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        [Display(Name = "Descricao")]
        [Required(ErrorMessage="Campo Obrigat�rio")]
        [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Descri��o", 2, true, ItensSearch.text, OrderType.asc)] 
        public String Descricao { get; set; }
        [Display(Name = "Exibe no Painel do RH")]
        public bool ExibePaineldoRH { get; set; }
        [TableHTMLAttribute("Exibe no Painel do RH", 3, true, ItensSearch.text, OrderType.none)]
        public string ExibePaineldoRHStr
        {
            get
            {
            return ExibePaineldoRH == true ? "Sim" : "N�o";
            }
        }
        [Display(Name = "Exibe no Painel do RH")]
        public string SelecionadoStr { get; set; }
        [Display(Name = "Considerar para Exporta��o")]
        public bool ConsiderarParaExportacao { get; set; }
        [TableHTMLAttribute("Considerar para Exporta��o", 4, true, ItensSearch.text, OrderType.none)]
        public string ConsiderarParaExportacaoStr
        {
            get
            {
                return ConsiderarParaExportacao == true ? "Sim" : "N�o";
            }
        }
    }
}
