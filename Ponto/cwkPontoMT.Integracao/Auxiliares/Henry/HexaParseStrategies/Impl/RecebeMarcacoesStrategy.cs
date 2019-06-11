using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies.Impl
{
    public class RecebeMarcacoesStrategy : IParseStrategy
    {
        public IMessage Parse(string command, string msg)
        {
            TicketMessage result = new TicketMessage();
            try
            {
                string msg2 = GetStringSomenteAlfanumerico(msg);
                if (!msg2.Contains("]"))
                {
                    throw new Exception("Mensagem MalFormada (Não encontrado \"]\" da separação entre os comando e os dados)");
                }
                if (msg2.Contains("++"))
                {
                    throw new Exception("Mensagem MalFormada (encontrado dois sinais de + juntos no retorno)");
                }
                List<string> split1 = msg2.Split(']').ToList();
                List<string> split2 = split1[0].Split('+').ToList();
                if (split2.Count < 3)
                {
                    throw new Exception("Mensagem MalFormada (encontrado mais sinais de + do que o esperado no retorno)");
                }
                result.Indice = Convert.ToInt32(split2[0]);
                if (split2[1] != command)
                {
                    result.Comando = command;
                }
                else
                {
                    result.Comando = split2[1];
                }
                result.Info = Convert.ToInt32(split2[2]);
                
                if (split1.Count > 1)
                {
                    result = new TicketMessage(split1[1]);
                    result.Indice = Convert.ToInt32(split2[0]);
                    if (split2[1] != command)
                    {
                        result.Comando = command;
                    }
                    else
                    {
                        result.Comando = split2[1];
                    }
                    result.Info = Convert.ToInt32(split2[2]);
                }
                
            }
            catch (Exception e)
            {
                throw new Exception("Mensagem MalFormada", e);
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
