using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioClassificacaoHorasExtrasModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Ordenar por Departamento")]
        public bool OrdenarPorDepartamento { get; set; }
    }
}
