using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies.Impl
{
    public class RecebeDataHoraStrategy : IParseStrategy
    {
        public IMessage Parse(string command, string msg)
        {
            ReceiveDateTimeMessage result = new ReceiveDateTimeMessage();
            msg = GetStringSomenteAlfanumerico(msg);
            List<string> split1 = msg.Split('+').ToList();
            if (split1.Count < 4)
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
                List<string> split2 = split1[3].Split(']').ToList();
                if (split2.Count < 3)
                {
                    throw new Exception("Mensagem Malformada");
                }
                List<string> splitData = split2[0].Split(' ').ToList();
                List<string> splitHora = splitData[1].Split(':').ToList();
                splitData = splitData[0].Split('/').ToList();
                int ano, mes, dia, hora, minuto, segundo;
                ano = 2000 + Convert.ToInt32(splitData[2]);
                mes = Convert.ToInt32(splitData[1]);
                dia = Convert.ToInt32(splitData[0]);
                hora = Convert.ToInt32(splitHora[0]);
                minuto = Convert.ToInt32(splitHora[1]);
                segundo = Convert.ToInt32(splitHora[2]);
                result.DataHora = new DateTime(ano, mes, dia, hora, minuto, segundo);
                if (!split2[1].Equals("00/00/00"))
                {
                    splitData = split2[1].Split('/').ToList();
                    result.InicioHorarioVerao = new DateTime(2000 + Convert.ToInt32(splitData[2]), Convert.ToInt32(splitData[1]), Convert.ToInt32(splitData[0]));
                }

                if (!split2[2].Equals("00/00/00"))
                {
                    splitData = split2[2].Split('/').ToList();
                    result.FimHorarioVerao = new DateTime(2000 + Convert.ToInt32(splitData[2]), Convert.ToInt32(splitData[1]), Convert.ToInt32(splitData[0]));
                }
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
    }
}
