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
        [Required(ErrorMessage = "O campo Data Inicial é obrigatório.")]
        public string DataInicial { get; set; }
        [Required(ErrorMessage = "O campo Data Final é obrigatório.")]
        public string DataFinal { get; set; }
        public List<LancamentoCartaoPontoRegistros> Regs { get; set; }
        public int QuantidadeRegistros { get; set; }
        [Required(ErrorMessage = "O campo Justificativa é obrigatório.")]
        [Display(Name = "Justificativa")]
        public string DescJustificativa { get; set; }
        [Required(ErrorMessage = "O campo Motivo é obrigatório.")]
        public string Motivo { get; set; }
        public int IdJustificativa { get; set; }
    }
}
