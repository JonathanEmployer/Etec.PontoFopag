using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelOcorrencias : pxyRelPontoWeb
    {
        [Display(Name = "Faltas")]
        public bool bFaltas { get; set; }

        [Display(Name = "Ocorrência")]
        public bool bOcorrencia { get; set; }
        
        [Display(Name = "Entrada Atrasada")]
        public bool bEntradaAtrasada { get; set; }
        
        [Display(Name = "Horas Extras")]
        public bool bHorasExtras { get; set; }
        
        [Display(Name = "Débto B.H.")]
        public bool bDebitoBH { get; set; }
        
        [Display(Name = "Marcações Incorretas")]
        public bool bMarcacoesIncorretas { get; set; }

        [Display(Name = "Saída Antecipada")]
        public bool bSaidaAntecipada { get; set; }

        [Display(Name = "Atrasos")]
        public bool bAtrasos { get; set; }

        [Display(Name = "Horas Abonadas")]
        public bool bHorasAbonadas { get; set; }

        [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }

        [Display(Name = "Agrupar por Departamento")]
        public bool bAgruparPorDepartamento { get; set; }

        [Display(Name = "Manutenção Manual")]
        public bool bManutencoesManuais { get; set; }

        public int Intervalo { get; set; }

        public string idSelecionados { get; set; }
        public string idSelecionadosOcorrencias { get; set; }
        public string idSelecionadosJustificativas { get; set; }

        public static pxyRelOcorrencias Produce(pxyRelPontoWeb p)
        {
            pxyRelOcorrencias obj = new pxyRelOcorrencias();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            obj.Ocorrencias = p.Ocorrencias;
            obj.Justificativas = p.Justificativas;

            return obj;
        }
    }
}
