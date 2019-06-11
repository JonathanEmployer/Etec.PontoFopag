using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class ListaEventosEvento : Modelo.ModeloBase
    {
         [Display(Name = "Idf_Lista_Eventos")]
        [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 Idf_Lista_Eventos { get; set; }

         [Display(Name = "Idf_Evento")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 Idf_Evento { get; set; }


    }
}
