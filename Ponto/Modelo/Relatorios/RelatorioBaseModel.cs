using Modelo.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioBaseModel
    {
        [Display(Name = "Tipo")]
        public int TipoSelecao { get; set; }

		[Required(ErrorMessage ="Tipo do arquivo é obrigatório")]
        public string TipoArquivo { get; set; }

        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime InicioPeriodo { get; set; }

        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        public int Intervalo { get; set; }

        [Display(Name = "Orientação")]
        public int Orientacao { get; set; }

        /// <summary>
        /// Ids dos registros selecionados na grid da página.
        /// </summary>
        public string IdSelecionados { get; set; }

        public string NomeArquivo { get; set; }

        /// <summary>
        /// Indica se relatório é Analítico ou Sintético, 0 = Analítico e 1 = Sintético
        /// </summary>
        [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }
        public bool Generico { get; set; }
    }
}
