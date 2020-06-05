using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelFechamentoPercentualHE : pxyRelPontoWeb
    {
        public int CodigoBancoHoras { get; private set; }


        [Display(Name = "Relatório")]
        public int TipoRelatorio { get; set; }

        //public int Intervalo { get; set; }



        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        public DateTime InicioPeriodo { get; set; }
        [Display(Name = "Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("31/12/1999")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        public string idSelecionados { get; set; }

        [Display(Name = "Prévia")]
        public Boolean bPrevia { get; set; }

        private string _BancoHoras;
        [Display(Name = "Banco de Horas")]
        [RequiredIf("bPrevia", true, "Prévia", "Verdadeiro")]
        public string BancoHoras
        {
            get
            {
                return _BancoHoras;
            }
            set { _BancoHoras = value; }
        }

        public static pxyRelFechamentoPercentualHE Produce(pxyRelPontoWeb p)
        {
            pxyRelFechamentoPercentualHE obj = new pxyRelFechamentoPercentualHE();
            obj.FuncionariosRelatorio = p.FuncionariosRelatorio;
            obj.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            obj.FimPeriodo = DateTime.Now.Date;
            return obj;
        }

        public int BuscaCodigo()
        {
            int codigoBancoHoras = -1;
            if (!String.IsNullOrEmpty(_BancoHoras))
            {
                Int32.TryParse(_BancoHoras.Split('|').FirstOrDefault().Trim(), out codigoBancoHoras);
                CodigoBancoHoras = codigoBancoHoras;
            }

            return CodigoBancoHoras;
        }
    }
}
