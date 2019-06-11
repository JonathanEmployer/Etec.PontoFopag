using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioAbonoModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Faltas")]
        public bool bFaltas { get; set; }

        [Display(Name = "Ocorrência")]
        public bool bOcorrencia { get; set; }

        [Display(Name = "Entrada Atrasada")]
        public bool bEntradaAtrasada { get; set; }

        [Display(Name = "Horas Extras")]
        public bool bHorasExtras { get; set; }

        [Display(Name = "Débto B.H.")]
        public bool bDebitoBH { get; set; }

        [Display(Name = "Marcações Incorretas")]
        public bool bMarcacoesIncorretas { get; set; }

        [Display(Name = "Saída Antecipada")]
        public bool bSaidaAntecipada { get; set; }

        [Display(Name = "Atrasos")]
        public bool bAtrasos { get; set; }

        [Display(Name = "Horas Abonadas")]
        public bool bHorasAbonadas { get; set; }

        [Display(Name = "Agrupar por Departamento")]
        public bool bAgruparPorDepartamento { get; set; }

        public string idSelecionadosOcorrencias { get; set; }
    }
}
