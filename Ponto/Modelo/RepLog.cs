using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class RepLog : Modelo.ModeloBase
    {
         [Display(Name = "IdRep")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 IdRep { get; set; }

         [Display(Name = "Comando")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [TableHTMLAttribute("Comando", 2, true, ItensSearch.select, OrderType.none)]
         public String Comando { get; set; }

         [Display(Name = "DescricaoExecucao")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(2000, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [TableHTMLAttribute("Log", 3, true, ItensSearch.text, OrderType.none)]
         public String DescricaoExecucao { get; set; }

         [Display(Name = "Executor")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
         [TableHTMLAttribute("Executor", 4, true, ItensSearch.select, OrderType.none)]
         public String Executor { get; set; }

         [Display(Name = "Complemento")]
         [TableHTMLAttribute("Complemento", 5, true, ItensSearch.text, OrderType.none)]
         public String Complemento { get; set; }

         [Display(Name = "Status")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 Status { get; set; }

         [TableHTMLAttribute("Situa��o", 6, true, ItensSearch.select, OrderType.none)]
         public String StatusStr
         {
             get
             {
                 return cwkFuncoes.Description(((Modelo.Enumeradores.SituacaoLog)Status));
             }
         }

         [TableHTMLAttribute("Data/Hora", 1, true, ItensSearch.text, OrderType.desc)]
         public String DataHoraLog { get { return String.Format("{0:dd/MM/yyyy HH:mm:ss}", Inchora.GetValueOrDefault()); } }


         public REP Rep { get; set; }

    }
}
