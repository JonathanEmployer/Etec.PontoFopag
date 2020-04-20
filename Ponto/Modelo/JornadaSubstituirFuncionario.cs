using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class JornadaSubstituirFuncionario : Modelo.ModeloBase
    {
         [Display(Name = "IdJornadaSubstituirFuncionario")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 IdJornadaSubstituirFuncionario { get; set; }

         [Display(Name = "IdFuncionario")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 IdFuncionario { get; set; }


    }
}
