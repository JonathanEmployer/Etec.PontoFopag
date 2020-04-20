using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class JornadaSubstituir : Modelo.ModeloBase
    {
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 IdJornadaDe { get; set; }

         [Required(ErrorMessage="Campo Obrigat�rio")]
         public Int32 IdJornadaPara { get; set; }

         [Display(Name = "DataInicio")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public DateTime DataInicio { get; set; }

         [Display(Name = "DataFim")]
         [Required(ErrorMessage="Campo Obrigat�rio")]
         public DateTime DataFim { get; set; }

        [Display(Name = "Descri��o De")]
        public string DescricaoDe { get; set; }

        [Display(Name = "Descri��o")]
        public string DescricaoPara { get; set; }
    }
}
