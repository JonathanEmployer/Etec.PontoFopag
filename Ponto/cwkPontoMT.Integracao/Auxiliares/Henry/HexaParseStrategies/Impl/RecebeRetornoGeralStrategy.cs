using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies.Impl
{
    public class RecebeRetornoGeralStrategy : IParseStrategy
    {
        public IMessage Parse(string command, string msg)
        {
            GeneralMessage result = new GeneralMessage();
            msg = GetStringSomenteAlfanumerico(msg);
            List<string> split1 = msg.Split('+').ToList();
            if (split1.Count < 3)
            {
                throw new Exception("Mensagem Malformada");
            }
            try
            {
                result.Indice = Convert.ToInt32(split1[0]);
                if (split1[1] != command)
                {
                    result.Comando = command;
                }
                else
                {
                    result.Comando = split1[1];
                }
                result.Info = Convert.ToInt32(split1[2]);
            }
            catch (Exception)
            {
                throw new Exception("Mensagem Malformada");
            }
            return result;
        }

        private static string GetStringSomenteAlfanumerico(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                return String.Empty;
            }
            string r = new string(s.ToCharArray().Where((c => char.IsLetterOrDigit(c) ||
                                                              char.IsWhiteSpace(c) ||
                                                              c == '+' ||
                                                              c == ',' ||
                                                              c == '/' ||
                                                              c == ':' ||
                                                              c == ']'))
                                                              .ToArray());
            return r;
        }

        public BiometricMessage ParseBiometric(string command, string msg, string TipoBiometria)
        {
            throw new NotImplementedException();
        }
    }
}
