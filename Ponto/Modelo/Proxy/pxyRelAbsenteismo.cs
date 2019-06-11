using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelAbsenteismo : pxyRelPontoWeb
    {
        [Display(Name = "Faltas")]
        public bool bFaltas { get; set; }

        [Display(Name = "Atrasos")]
        public bool bAtrasos { get; set; }

        [Display(Name = "Horas Abonadas")]
        public bool bHorasAbonadas { get; set; }
        [Display(Name = "Débito Banco Horas")]
        public bool bDebitoBH { get; set; }

        [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }

        public int Intervalo { get; set; }
        public string idSelecionados { get; set; }

        public static pxyRelAbsenteismo Produce(pxyRelPontoWeb p)
        {
            pxyRelAbsenteismo obj = new pxyRelAbsenteismo();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            
            return obj;
        }
    }
}
