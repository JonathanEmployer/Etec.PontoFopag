using Modelo.Proxy;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Relatorios
{
    public class RelatorioHorasInItinereModel : RelatorioBaseModel, IRelatorioModel
    {
        [Display(Name = "Turno")]
        public int TipoTurno { get; set; }

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
