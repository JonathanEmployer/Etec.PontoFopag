using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyTransferirBilhetesContrato
    {
        public PxyFuncionarioCabecalhoRel FuncAlocado { get; set; }
        public PxyFuncionarioCabecalhoRel FuncDestino { get; set; }
        public List<BilhetesImp> BilhetesAlocados { get; set; }
        public List<BilhetesImp> BilhetesDestino { get; set; }
    }
}
