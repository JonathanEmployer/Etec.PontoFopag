using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies
{
    public class BiometricMessage : IMessage
    {
        public List<object> Dados { get; set; }

        public BiometricMessage()
        {
            Dados = new List<object>();
        }
    }
}
