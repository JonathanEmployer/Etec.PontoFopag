using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies.Impl
{
    public class RecebeBiometriaStrategy : IParseStrategy
    {
        public BiometricMessage ParseBiometric(string command, string msg, string TipoBiometria)
        {
            BiometricMessage result = new BiometricMessage();

            if (command == "Q")
            {
                try
                {
                    //01+RD+000+Q]1816264}2
                    var split = msg.Split('}').ToList();
                    if (split.Count >= 2)
                    {
                        result.Dados.Add(Encoding.UTF8.GetBytes(GetStringSomenteAlfanumerico(split[1])));
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Mensagem Malformada");
                }
            }
            else
            {
                if (TipoBiometria == "Verde")
                {
                    try
                    {
                        //"01+RD+000+Tð00375556}H}B}0}768" +
                        //"{AwNWGgABIAF/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAIMAAAAAAMzMzzPPP//P////uqqqqqqqqqZVVVVVVVVVVERAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABLBgLeFYuRXi6PE/5MkRj+OhqC3hKj5h4xo8E+WjkeHjk7Sb5jwF5+DgapPzqNgf9cJRxfQSaY3yIqJ58orY2fZ7pH3xq84r82wkhfSqxHXDQsT91KsBwdOi4F+DiyEo03s85tNrQiTQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAwNaIwABIAGBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAIUAAAAAAAAAAM///////////uqqqqqqqqZVlVmVVZVVVVVEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABHBcE+RY5OvhEpjD4tNiOeO7nj3kW/Jn4lhiT/dIhc31YImN83DGc/EhXjf3Qdnf9PHkl/MJ+iXzEt4r93sdvfajNbX2E9Ff9gDsdcUy/GPD2PjX1fEpx9S6OJfUimyz1eGkaaYxocW0y2JjtyQtb7dkLBG1kzXFhTNmr5UBVI90yVjZdPlwk3VTRRVwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA\0"
                        var split = msg.Split('{').ToList();

                        if (split.Count >= 2)
                        {
                            var biometria = Encoding.Default.GetBytes(split[1]);
                            //var biometria = Encoding.UTF8.GetBytes(GetStringSomenteAlfanumerico(split[1]));
                            result.Dados.Add(biometria);
                        }
                        var parametros = GetStringSomenteAlfanumerico(split[0]);
                        if (parametros.Length <= 7 && !parametros.Contains("RD") && !parametros.Contains("22"))
                        {
                            throw new Exception("Mensagem Malformada");
                        }

                        if (parametros.Substring(1, 1) == "R")
                            result.Indice = Convert.ToInt32(parametros.Substring(0, 1));
                        else
                            result.Indice = Convert.ToInt32(parametros.Substring(0, 2));

                        result.Comando = command;

                        if ((parametros != "01RD022") || (parametros.Length > 7))
                        {
                            //"01RD00D37556" "01üD0D375556" "01RD00D35556" "01RD000D375556"
                            if (parametros.Substring(3, 1) == "D" && parametros.Substring(5, 1) == "D")
                                result.Info = Convert.ToInt32(parametros.Substring(4, 1));
                            else if (parametros.Substring(3, 1) == "D" && parametros.Substring(7, 1) == "D")
                                result.Info = Convert.ToInt32(parametros.Substring(4, 3));
                            else
                                result.Info = Convert.ToInt32(parametros.Substring(4, 2));
                        }
                        else
                        {
                            result.Info = 22;
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Mensagem Malformada");
                    }

                }
                else
                {
                    try
                    {
                        var split = msg.Split('}').ToList();

                        if (split.Count >= 3)
                        {
                            for (int i = 0; i < int.Parse(split[1]); i++)
                            {
                                var biometria = Encoding.Default.GetBytes(split[2 + i].Substring(2).ToCharArray());
                                result.Dados.Add(biometria);
                            }
                        }
                        var parametros = GetStringSomenteAlfanumerico(split[0]);
                        if (parametros.Length <= 7 && !parametros.Contains("RD") && !parametros.Contains("22"))
                        {
                            throw new Exception("Mensagem Malformada");
                        }

                        if (parametros.Substring(1, 1) == "R")
                            result.Indice = Convert.ToInt32(parametros.Substring(0, 1));
                        else
                            result.Indice = Convert.ToInt32(parametros.Substring(0, 2));

                        result.Comando = command;

                        if ((parametros != "01RD022") || (parametros.Length > 7))
                        {
                            //"01RD00D37556" "01üD0D375556" "01RD00D35556" "01RD000D375556"
                            if (parametros.Substring(3, 1) == "D" && parametros.Substring(5, 1) == "D")
                                result.Info = Convert.ToInt32(parametros.Substring(4, 1));
                            else if (parametros.Substring(3, 1) == "D" && parametros.Substring(7, 1) == "D")
                                result.Info = Convert.ToInt32(parametros.Substring(4, 3));
                            else
                                result.Info = Convert.ToInt32(parametros.Substring(4, 2));
                        }
                        else
                        {
                            result.Info = 22;
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("Mensagem Malformada");
                    }
                }
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
                                                              char.IsWhiteSpace(c)))
                                                              .ToArray());
            return r;
        }

        public IMessage Parse(string command, string msg)
        {
            throw new NotImplementedException();
        }

        public static string RemoverAcentos(string Texto)
        {
            string ComAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string SemAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < ComAcentos.Length; i++)
            {
                Texto = Texto.Replace(ComAcentos[i].ToString(), SemAcentos[i].ToString());
            }
            return Texto;
        }
    }
}
