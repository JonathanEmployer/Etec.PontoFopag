using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class AlertasRepAcompanhamento : Modelo.ModeloBase
    {
         [Display(Name = "IDAlertas")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 IDAlertas { get; set; }

         [Display(Name = "IdRep")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 IdRep { get; set; }
    }
}
