using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoLoteBilhetesImp : Modelo.ModeloBase
    {
        public int IdLancamentoLote { get; set; }
        public LancamentoLote LancamentoLote { get; set; }
        /// <summary>
        /// Hora do Bilhete
        /// </summary>
        [Display(Name = "Hora")]
        [Required]
        public string Hora { get; set; }

        public string Hora_Ant { get; set; }
        /// <summary>
        /// Relógio do Bilhete
        /// </summary>
        [Display(Name = "Relógio")]
        public string Relogio { get; set; }
        /// <summary>
        /// Ocorrência da Marcação
        /// </summary>
        [Display(Name = "Ocorrência")]
        [Required]
        public string Ocorrencia { get; set; }
        /// <summary>
        /// Motivo da Ocorrência
        /// </summary>
        [Required]
        public string Motivo { get; set; }
        /// <summary>
        /// Identificação da Justificativa
        /// </summary>
        public int Idjustificativa { get; set; }
        /// <summary>
        /// Identificação da Justificativa para o ponto web, composto por codigo | descrição
        /// </summary>
        [Display(Name = "Justificativa")]
        [Required]
        public string DescJustificativa { get; set; }
    }
}


