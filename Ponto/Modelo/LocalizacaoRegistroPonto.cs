using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LocalizacaoRegistroPonto : Modelo.ModeloBase
    {
         [Display(Name = "IdBilhetesImp")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [DataTableAttribute()]
         public Int32 IdBilhetesImp { get; set; }

         [Display(Name = "IP Público")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String IpPublico { get; set; }

         [Display(Name = "IP Interno")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String IpInterno { get; set; }

         [Display(Name = "IPS")]
         [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String X_FORWARDED_FOR { get; set; }

         [Display(Name = "Latitude")]
         [DataTableAttribute()]
         public Decimal Latitude { get; set; }

         [Display(Name = "Longitude")]
         [DataTableAttribute()]
         public Decimal Longitude { get; set; }

         [Display(Name = "Navegador")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String Browser { get; set; }

         [Display(Name = "Versão Navegador")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String BrowserVersao { get; set; }

         [Display(Name = "Plataforma Navegador")]
         [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
         [DataTableAttribute()]
         public String BrowserPlatform { get; set; }


    }
}
