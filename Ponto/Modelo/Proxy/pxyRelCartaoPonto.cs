using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelCartaoPonto : pxyRelPontoWeb
    {
        [Display(Name = "Turno")]
        public int TipoTurno { get; set; }
        [Display(Name = "Orientação")]
        public int Orientacao { get; set; }
        [Display(Name = "Ordenar por Departamento")]
        public bool OrdenarPorDepartamento { get; set; }

        [Display(Name = "Máx. Horas Trabalhadas")]
        public bool bLimMaxHorasTrab { get; set; }

        [Display(Name = "Intrajornada")]
        public bool bLimIntrajornada { get; set; }

        [Display(Name = "Interjornada")]
        public bool bMinInterjornada { get; set; }

        public static pxyRelCartaoPonto Produce(pxyRelPontoWeb p)
        {
            return new pxyRelCartaoPonto()
            {
                FuncionariosRelatorio = p.FuncionariosRelatorio,
                InicioPeriodo = DateTime.Now.Date.AddMonths(-1),
                FimPeriodo = DateTime.Now.Date,
            };
        }
        /// <summary>
        /// 0 = por empresa, 1 = por funcionário
        /// </summary>
        public int OrdemRelatorio { get; set; }
    }
}
