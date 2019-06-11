using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelHistorico : pxyRelPontoWeb
    {
       [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }

        public int Intervalo { get; set; }

        public string idSelecionados { get; set; }
        public static pxyRelHistorico Produce(pxyRelPontoWeb p)
        {
            pxyRelHistorico obj = new pxyRelHistorico();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;

            return obj;
        }
    }
}
