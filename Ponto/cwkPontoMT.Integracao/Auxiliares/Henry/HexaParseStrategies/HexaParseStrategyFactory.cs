using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies.Impl;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies
{
    public static class HexaParseStrategyFactory
    {
        public static IParseStrategy Produce(string command)
        {
            switch (command)
            {
                case "RH":
                    return new RecebeDataHoraStrategy();
                case "RR":
                    return new RecebeMarcacoesStrategy();
                case "RQ":
                    return new RecebeTotalMarcacoesStrategy();
                case "RD":
                    return new RecebeBiometriaStrategy();
                default:
                    return new RecebeRetornoGeralStrategy();
            }
        }
    }
}
