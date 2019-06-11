using System;

namespace Modelo
{
    public class FechamentoBHDPercentual : Modelo.ModeloBase
    {
        public int IdfechamentoBHD { get; set; }

        public Decimal Percentual { get; set; }

        public string CreditoPercentual { get; set; }

        public string DebitoPercentual { get; set; }

        public string SaldoPercentual { get; set; }

        public string HorasPagasPercentual { get; set; }

        public int idFuncionario { get; set; }
        
    }
}
