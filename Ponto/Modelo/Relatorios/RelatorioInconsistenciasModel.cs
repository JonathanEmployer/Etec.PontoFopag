using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Relatorios
{
    public class RelatorioInconsistenciasModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Turno")]
        public int TipoTurno { get; set; }

        [Display(Name = "Máx. Horas Trabalhadas")]
        public bool bLimMaxHorasTrab { get; set; }

        [Display(Name = "Intrajornada")]
        public bool bLimIntrajornada { get; set; }

        [Display(Name = "Interjornada")]
        public bool bMinInterjornada { get; set; }

    }
}
