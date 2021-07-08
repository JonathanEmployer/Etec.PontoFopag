using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class ExportacaoFechamentoPontoModel : RelatorioCartaoPontoModel, IRelatorioModel
    {
        [Display(Name = "Id Fechamento Ponto")]
        public int? IdFechamentoPonto { get; set; }

    }
}
