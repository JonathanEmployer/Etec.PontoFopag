using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class FiltroCartaoPonto
    {
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataInicial { get; set; }

        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataFinal { get; set; }
        public int intervaloCPI { get; set; }
    }
}
