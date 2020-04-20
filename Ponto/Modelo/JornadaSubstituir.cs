using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class JornadaSubstituir : Modelo.ModeloBase
    {
         [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 IdJornadaDe { get; set; }

         [Required(ErrorMessage="Campo Obrigatório")]
         public Int32 IdJornadaPara { get; set; }

         [Display(Name = "DataInicio")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public DateTime DataInicio { get; set; }

         [Display(Name = "DataFim")]
         [Required(ErrorMessage="Campo Obrigatório")]
         public DateTime DataFim { get; set; }

        [Display(Name = "Descrição De")]
        public string DescricaoDe { get; set; }

        [Display(Name = "Descrição")]
        public string DescricaoPara { get; set; }
    }
}
