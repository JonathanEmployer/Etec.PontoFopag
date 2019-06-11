using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class EventosClassHorasExtras : Modelo.ModeloBase
    {
         [Display(Name = "IdEventos")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 IdEventos { get; set; }

         [Display(Name = "IdClassificacao")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 IdClassificacao { get; set; }


    }
}
