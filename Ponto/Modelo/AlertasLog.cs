using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class AlertasLog : Modelo.ModeloBase
    {
        [Display(Name = "IdAlerta")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public Int32 IdAlerta { get; set; }

        [Display(Name = "Comando")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(300, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Comando", 2, true, ItensSearch.select, OrderType.none)]
        public String Comando { get; set; }

        [Display(Name = "Log")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(-1, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Log", 3, true, ItensSearch.text, OrderType.none)]
        public String Log { get; set; }

        [Display(Name = "Complemento")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(-1, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Complemento", 4, true, ItensSearch.text, OrderType.none)]
        public String Complemento { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public Int32 Status { get; set; }

        [TableHTMLAttribute("Situa��o", 5, true, ItensSearch.select, OrderType.none)]
        public String StatusStr
        {
            get
            {
                return cwkFuncoes.Description(((Modelo.Enumeradores.SituacaoLog)Status));
            }
        }

        [TableHTMLAttribute("Data/Hora", 1, true, ItensSearch.select, OrderType.desc)]
        public String DataHoraLog { get { return String.Format("{0:dd/MM/yyyy HH:mm:ss}", Inchora.GetValueOrDefault()); } }

        public Alertas Alerta { get; set; }        
    }         
}
