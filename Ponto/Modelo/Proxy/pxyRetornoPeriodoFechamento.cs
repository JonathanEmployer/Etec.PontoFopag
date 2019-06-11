using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRetornoPeriodoFechamento
    {
        public bool Sucesso { get; set; }
        public string Erro { get; set; }
        public PeriodoFechamento PeriodoFechamento { get; set; }
    }
}
