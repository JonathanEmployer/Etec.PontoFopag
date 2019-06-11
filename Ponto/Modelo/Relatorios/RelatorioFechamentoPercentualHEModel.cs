using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Modelo.Relatorios
{
    public class RelatorioFechamentoPercentualHEModel : RelatorioBaseModel, IRelatorioModel
    {
        public int CodigoBancoHoras { get; private set; }

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
