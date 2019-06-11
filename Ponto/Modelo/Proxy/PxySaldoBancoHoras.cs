using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxySaldoBancoHoras
    {
        public int IdFuncionario { get; set; }

        public string Nome { get; set; }

        public DateTime? DataInicioBH { get; set; }

        public DateTime? DataFechamento { get; set; }

        public DateTime? DataInicioSoma { get; set; }

        public int? SaldoFechamento { get; set; }

        public bool PossuiBH { get; set; }

        public int SaldoPeriodo { get; set; }

        public int Saldo { get; set; }
    }
}
