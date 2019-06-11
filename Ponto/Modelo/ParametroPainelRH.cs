using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class ParametroPainelRH : Modelo.ModeloBase
    {
         [Display(Name = "IntegraPainel")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Boolean IntegraPainel { get; set; }

         [Display(Name = "UsuarioAPIPainel")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(250, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String UsuarioAPIPainel { get; set; }

         [Display(Name = "SenhaAPIPainel")]
         [Required(ErrorMessage="Campo Obrigatório")]
         [StringLength(250, ErrorMessage = "Número máximo de caracteres: {1}")]
         public String SenhaAPIPainel { get; set; }

         [Display(Name = "PermiteAprovarMarcacaoImpar")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Boolean PermiteAprovarMarcacaoImpar { get; set; }

         [Display(Name = "CSAPIPainel")]
         [Required(ErrorMessage = "Campo Obrigatório")]
         public string CS { get; set; }
    }
}
