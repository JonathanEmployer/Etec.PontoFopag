using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioAbsenteismoModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Faltas")]
        public bool bFaltas { get; set; }

        [Display(Name = "Atrasos")]
        public bool bAtrasos { get; set; }

        [Display(Name = "Horas Abonadas")]
        public bool bHorasAbonadas { get; set; }
        [Display(Name = "Débito Banco Horas")]
        public bool bDebitoBH { get; set; }
    }
}
