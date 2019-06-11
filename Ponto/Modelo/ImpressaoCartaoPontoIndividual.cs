using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class ImpressaoCartaoPontoIndividual : Modelo.ModeloBase
    {
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? DataInicial { get; set; }

        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? DataFinal { get; set; }

        public string TipoArquivo { get; set; }
        public int IdFuncionario { get; set; }
        public int intervaloCPI { get; set; }
    }
}