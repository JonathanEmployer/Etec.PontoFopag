using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies
{
    public interface IParseStrategy
    {
        IMessage Parse(string command, string msg);
        BiometricMessage ParseBiometric(string command, string msg, string TipoBiometria);
    }
}
