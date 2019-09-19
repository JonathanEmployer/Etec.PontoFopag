using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyUltimoFechamentoPonto
    {
        public string Codigo { get; set; }

        public string CPF { get; set; }

        public string Matricula { get; set; }

        public string Pis { get; set; }

        public string Nome { get; set; }

        public DateTime? UltimoFechamentoPonto { get; set; }

        public DateTime? UltimoFechamentoBanco { get; set; }

        public DateTime? UltimoFechamento { get; set; }
    }
}
