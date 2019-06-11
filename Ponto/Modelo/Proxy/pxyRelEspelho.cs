using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelEspelho : pxyRelPontoWeb
    {
       [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }

        public int Intervalo { get; set; }

        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime InicioPeriodo { get; set; }

        [Display(Name = "Fim")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        [DateRangeLessThan(31, "InicioPeriodo", "Início")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        public string idSelecionados { get; set; }
        public static pxyRelEspelho Produce(pxyRelPontoWeb p)
        {
            pxyRelEspelho obj = new pxyRelEspelho();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            return obj;
        }
    }
}
