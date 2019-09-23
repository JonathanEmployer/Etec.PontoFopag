using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class ParametroWebfopag : Modelo.ModeloBase
    {
         [Display(Name = "UtilizaColetorTipoMonsanto")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Boolean UtilizaColetorTipoMonsanto { get; set; }

         [Display(Name = "UsuarioApiV1")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(60, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String UsuarioApiV1 { get; set; }

         [Display(Name = "SenhaApiV1")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(255, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String SenhaApiV1 { get; set; }

         [Display(Name = "TokenApiV1")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(255, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String TokenApiV1 { get; set; }

         [Display(Name = "CS")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String CS { get; set; }

         [Display(Name = "UltimaColeta")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public DateTime? UltimaColeta { get; set; }


    }
}
