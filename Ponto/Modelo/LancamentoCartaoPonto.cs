using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoCartaoPonto : ModeloBase
    {
        public string Empresa { get; set; }
        public string Departamento { get; set; }
        public string Contrato { get; set; }
        [Required]
        public string Funcionario { get; set; }
        public int IdFuncionario { get; set; }
        [Required]
        public string DataInicial { get; set; }
        [Required]
        public string DataFinal { get; set; }
        public List<LancamentoCartaoPontoRegistros> Regs { get; set; }
        public int QuantidadeRegistros { get; set; }
        [Required]
        [Display(Name = "Justificativa")]
        public string DescJustificativa { get; set; }
        [Required]
        public string Motivo { get; set; }
        public int IdJustificativa { get; set; }
    }
}
