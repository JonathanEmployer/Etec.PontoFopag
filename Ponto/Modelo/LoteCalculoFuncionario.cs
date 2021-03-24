using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LoteCalculoFuncionario : Modelo.ModeloBase
    {
        [Display(Name = "IdLoteCalculo")]
        [Required(ErrorMessage="Campo Obrigatório")]
        [DataTableAttribute()]
        public Int32 IdLoteCalculo { get; set; }

        [Display(Name = "IdFuncionario")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DataTableAttribute()]
        public int IdFuncionario { get; set; }
        public string Erro { get; set; }
        public int QtdTentativas { get; set; }
        public DateTime? DtaProcessamento { get; set; }

    }
}
