using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Relatorios
{
    public class RelatorioCartaoPontoModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Turno")]
        public int TipoTurno { get; set; }

        [Display(Name = "Ordenar por Departamento")]
        public bool OrdenarPorDepartamento { get; set; }

        [Display(Name = "Quebra automática")]
        public bool quebraAuto { get; set; }
    }
}
