using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelIntervalos : pxyRelPontoWeb
    {
        [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }

        [Display(Name = "Agrupar por Departamento")]
        public bool bAgruparPorDepartamento { get; set; }

        public int Intervalo { get; set; }

        public string idSelecionados { get; set; }
        public string idSelecionadosOcorrencias { get; set; }

        public static pxyRelIntervalos Produce(pxyRelPontoWeb p)
        {
            pxyRelIntervalos obj = new pxyRelIntervalos();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;

            return obj;
        }
    }
}
