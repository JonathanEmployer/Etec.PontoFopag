using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class AlertasFuncionario : Modelo.ModeloBase
    {
         [Display(Name = "IDAlertas")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 IDAlertas { get; set; }

         [Display(Name = "IDFuncionario")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 IDFuncionario { get; set; }


    }
}
