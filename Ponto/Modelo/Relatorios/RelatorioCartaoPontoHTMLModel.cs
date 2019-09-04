using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioCartaoPontoHTMLModel : RelatorioBaseModel, IRelatorioModel
    {
        /// <summary>
        /// 0 = por empresa, 1 = por funcionário
        /// </summary>
        public int OrdemRelatorio { get; set; }

        [Display(Name = "Ordenar por Departamento")]
        public bool OrdenarPorDepartamento { get; set; }

        [Display(Name = "Quebra automática")]
        public bool quebraAuto { get; set; }
    }
}
