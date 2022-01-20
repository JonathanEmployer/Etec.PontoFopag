using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

namespace Modelo
{
    public static class cwkFuncoes
    {
        public enum TipoDiaSemana
        {
            Reduzido = 1,
            Completo = 2
        }

        #region Dia
        /// <summary>
        /// Método que recebe uma data e retorna um número correspondente ao dia da semana:
        /// 1 = Segunda; 2 = Terça; 3 = Quarta; 4 = Quinta; 5 = Sexta; 6 = Sábado; 7 = Domingo.
        /// </summary>
        /// <param name="data">data do tipo DateTime</param>
        /// <returns>Inteiro que representa o dia da semana</returns>
        public static int Dia(DateTime data)
        {
            int ret;
            switch (data.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    ret = 1;
                    break;
                case DayOfWeek.Tuesday:
                    ret = 2;
                    break;
                case DayOfWeek.Wednesday:
                    ret = 3;
                    break;
                case DayOfWeek.Thursday:
                    ret = 4;
                    break;
                case DayOfWeek.Friday:
                    ret = 5;
                    break;
                case DayOfWeek.Saturday:
                    ret = 6;
                    break;
                case DayOfWeek.Sunday:
                    ret = 7;
                    break;
                default:
                    ret = -1;
                    break;
            }

            return ret;
            //return ((int)data.DayOfWeek) + 1;            
        }

        /// <summary>
        /// Recebe o dia da semana em string e retorna em numero.
        /// Seg = 1, ter = 2, qua = 3, qui = 4, sex = 5, sab = 6, dom = 7
        /// </summary>
        /// <param name="data">Dia da semana</param>
        /// <returns>Dia da semana equivalente a data</returns>
        public static int Dia(string data)
        {
            switch (data)
            {
                case "Seg.": return 1;
                case "Ter.": return 2;
                case "Qua.": return 3;
                case "Qui.": return 4;
                case "Sex.": return 5;
                case "Sáb.": return 6;
                case "Dom.": return 7;
                default: return -1;
            }
        }

        #endregion

        #region DiaSemana
        /// <summary>
        /// Método para retornar o dia da semana (string)
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Dia da Semana</returns>
        public static string DiaSemana(DateTime data, TipoDiaSemana pDiaSemana)
        {
            string aux = "";

            switch (data.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (pDiaSemana == TipoDiaSemana.Completo)
                    {
                        aux = "Segunda";
                    }
                    else
                    {
                        aux = "Seg.";
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (pDiaSemana == TipoDiaSemana.Completo)
                    {
                        aux = "Terça";
                    }
                    else
                    {
                        aux = "Ter.";
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (pDiaSemana == TipoDiaSemana.Completo)
                    {
                        aux = "Quarta";
                    }
                    else
                    {
                        aux = "Qua.";
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (pDiaSemana == TipoDiaSemana.Completo)
                    {
                        aux = "Quinta";
                    }
                    else
                    {
                        aux = "Qui.";
                    }
                    break;
                case DayOfWeek.Friday:
                    if (pDiaSemana == TipoDiaSemana.Completo)
                    {
                        aux = "Sexta";
                    }
                    else
                    {
                        aux = "Sex.";
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (pDiaSemana == TipoDiaSemana.Completo)
                    {
                        aux = "Sábado";
                    }
                    else
                    {
                        aux = "Sáb.";
                    }
                    break;
                case DayOfWeek.Sunday:
                    if (pDiaSemana == TipoDiaSemana.Completo)
                    {
                        aux = "Domingo";
                    }
                    else
                    {
                        aux = "Dom.";
                    }
                    break;
                default:
                    aux = "";
                    break;
            }

            return aux;
        }


        #endregion

        /// <summary>
        /// Copia o valor das propriedades de um objeto para outro.
        /// </summary>
        /// <param name="objOrigem">Objeto origem.</param>
        /// <param name="objDestino">Objeto destino.</param>
        /// <returns>Objeto cópia.</returns>
        public static object CopiaPropriedades(object objOrigem, object objDestino)
        {
            if (objOrigem != null &&
                    objDestino != null &&
                    objOrigem.GetType().Equals(objDestino.GetType()))
            {
                // Tipo
                Type pTipo = objOrigem.GetType();

                // Nome de todas as propriedades.
                PropertyInfo[] pNome = pTipo.GetProperties();

                // Move todos os dados
                foreach (PropertyInfo propriedade in pNome)
                {
                    if (propriedade.CanWrite)
                    {
                        var valor = propriedade.GetValue(objOrigem, null);
                        propriedade.SetValue(objDestino, valor, null);
                    }
                }
            }
            else
            {
                objDestino = null;
            }
            return objDestino;
        }

        public static void EstendePeriodo(ref DateTime pDataI, ref DateTime pDataF)
        {
            pDataI = Convert.ToDateTime("01/" + pDataI.Month + "/" + pDataI.Year);
            pDataF = Convert.ToDateTime(DateTime.DaysInMonth(pDataF.Year, pDataF.Month) + "/" + pDataF.Month + "/" + pDataF.Year);
        }

        public static void GravaLog(bool append)
        {
            //Escreve o log de tempo no arquivo
            StreamWriter sw = new StreamWriter("log.txt", append);
            for (int i = 0; i < Modelo.Global.logs.Count; i++)
            {
                sw.WriteLine(Modelo.Global.logs[i]);
            }
            sw.Close();
        }

        #region ConvertHorasMinuto

        public static int ConvertBatidaMinuto(string pBatida)
        {
            if (pBatida == null)
            {
                return -1;
            }
            if (pBatida.Length == 0)
                return -1;

            if (pBatida[0] == '-')            
                return -1;

            string[] hora = pBatida.Split(':');
            if (hora.Length == 2)
            {
                return ((Convert.ToInt32(hora[0]) * 60) + Convert.ToInt32(hora[1]));
            }
            else
            {
                return -1;
            }
        }

        public static bool HoraValida(string pBatida)
        {
            bool retorno = false;
            try
            {
                if (string.IsNullOrEmpty(pBatida) || pBatida.Contains("--:--"))
                {
                    retorno = true;
                }
                string[] hora = pBatida.Split(':');
                retorno = Convert.ToInt32(hora[0]) <= 23 && Convert.ToInt32(hora[1]) <= 59;
            }
            catch (Exception)
            {
                retorno = false;
            }
            return retorno;
        }

        /// <summary>
        /// Método que recebe como parâmetro um string no formato HH:MM e converte esse horário em minutos 
        /// </summary>
        /// <param name="pHoras">string no formato HH:MM</param>
        /// <returns>horário convertido em minutos</returns>
        public static int ConvertHorasMinuto(this string pHoras)
        {

                if (String.IsNullOrEmpty(pHoras))
                    return 0;

                if (pHoras.Length < 5)
                    return 0;

                if (pHoras[0] == '-')
                    return 0;

                string[] hora = pHoras.Split(':');
                return ((Convert.ToInt32(hora[0]) * 60) + Convert.ToInt32(hora[1]));


        }

        public static int ConvertHorasMinutoSemFormatacao(this string pHoras)
        {
            int horasInt;
            int minutosInt;
            int horasEmMinutos;
            string horas;
            string minutos;
            if ((pHoras.Contains(':')) ||
                (pHoras.Contains('.')) ||
                (pHoras.Contains(';')))
            {
                if (String.IsNullOrEmpty(pHoras))
                    return 0;

                if (pHoras.Length < 5)
                    return 0;

                if (pHoras[0] == '-')
                    return 0;

                string[] hora = pHoras.Split(':');
                return ((Convert.ToInt32(hora[0]) * 60) + Convert.ToInt32(hora[1]));
            }
            else
            {
                if (pHoras[0] == '-')
                    return 0;
                if (pHoras.Length == 0)
                {
                    return 0;
                }
                else if (pHoras.Length == 1)
                {
                    horasInt = Convert.ToInt32(pHoras);
                    horasEmMinutos = (horasInt * 60);
                    return horasEmMinutos;
                }
                else if (pHoras.Length == 2)
                {
                    horasInt = Convert.ToInt32(pHoras);
                    horasEmMinutos = (horasInt * 60);
                    return horasEmMinutos;
                }
                else if (pHoras.Length > 2)
                {
                    minutos = pHoras.Substring(pHoras.Length - 2, 2);
                    try
                    {
                        minutosInt = Convert.ToInt32(minutos);
                        horas = pHoras.Remove(pHoras.Length - 2, 2);
                        horasInt = Convert.ToInt32(horas);
                        horasEmMinutos = (horasInt * 60) + minutosInt;
                        return horasEmMinutos;
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
               
            }
        }


        #endregion

        #region ConvertMinutosHora
        /// <summary>
        /// Método que recebe como parâmetro um valor decimal em minutos e converte esse número em uma string de horas 
        /// </summary>
        /// <param name="pMinutos">número decimal em minutos</param>
        /// <returns>minutos convertido em uma string de horas no formato HH:MM</returns>
        public static string ConvertMinutosBatida(decimal pMinutos)
        {
            if (pMinutos < 0)
            {
                return "--:--";
            }

            decimal hh;
            decimal mm;
            ConvMinutoHora(pMinutos, out hh, out mm);

            return String.Format("{0:00}:{1:00}", hh, mm);
        }

        /// <summary>
        /// Método que recebe como parâmetro um valor decimal em minutos e converte esse número em uma string de horas 
        /// </summary>
        /// <param name="pMinutos">número decimal em minutos</param>
        /// <returns>minutos convertido em uma string de horas no formato HH:MM</returns>
        public static string ConvertMinutosHora(decimal pMinutos)
        {
            if (pMinutos <= 0)
            {
                return "--:--";
            }

            decimal hh;
            decimal mm;
            ConvMinutoHora(pMinutos, out hh, out mm);

            return String.Format("{0:00}:{1:00}", hh, mm);
        }


        public static string ConvertMinutosHoraExcel(decimal pMinutos)
        {
            if (pMinutos < 0)
            {
                return "--:--";
            }

            decimal hh;
            decimal mm;

            decimal aux = pMinutos / 60;

            hh = Math.Truncate(aux);
            mm = Math.Truncate(Math.Round((aux - hh) * 60, 0));

            return String.Format("{0:00}:{1:00}", hh, mm);
        }


        /// <summary>
        /// Método que recebe como parâmetro um valor decimal em minutos (positivos ou negativos) e converte esse número em uma string de horas 
        /// </summary>
        /// <param name="pMinutos">número decimal em minutos</param>
        /// <returns>minutos convertido em uma string de horas no formato HH:MM quando posito e -HH:MM quando Negativo</returns>
        public static string ConvertMinutosHoraNegativo(decimal pMinutos)
        {
            decimal minutos = Math.Abs(pMinutos);

            decimal hh;
            decimal mm;
            hh = 0;
            mm = 0;

            decimal aux = minutos / 60;

            hh = Math.Truncate(aux);
            mm = Math.Truncate(Math.Round((aux - hh) * 60, 0));

            string retorno = String.Format("{0:00}:{1:00}", hh, mm);
            if (pMinutos < 0)
                retorno = "-" + retorno;
            else retorno = " " + retorno;
            return retorno;
        }
        private static void ConvMinutoHora(decimal pMinutos, out decimal hh, out decimal mm)
        {
            hh = 0;
            mm = 0;

            decimal aux = pMinutos / 60;

            hh = Math.Truncate(aux);
            mm = Math.Truncate(Math.Round((aux - hh) * 60, 0));

            if (hh >= 24)
            {
                hh -= 24;
            }
        }

        public static string ConvertMinutosHora(int pDigitosHoras, decimal pMinutos)
        {
            decimal hh = 0;
            decimal mm = 0;

            decimal aux = pMinutos / 60;

            hh = Math.Truncate(aux);
            mm = Math.Truncate(Math.Round((aux - hh) * 60, 0));

            switch (pDigitosHoras)
            {
                case 2:
                    return String.Format("{0:00}:{1:00}", hh, mm);
                case 3:
                    return String.Format("{0:#00}:{1:00}", hh, mm);
                case 4:
                    return String.Format("{0:##00}:{1:00}", hh, mm);
                case 5:
                    return String.Format("{0:###00}:{1:00}", hh, mm);
                case 6:
                    return String.Format("{0:####00}:{1:00}", hh, mm);
                default:
                    return "--:--";
            }

        }

        public static string ConvertMinutosHora2(int pDigitosHoras, decimal pMinutos)
        {
            decimal hh = 0;
            decimal mm = 0;

            decimal aux = Math.Abs(pMinutos) / 60;

            hh = Math.Truncate(aux);
            mm = Math.Truncate(Math.Round((aux - hh) * 60, 0));

            switch (pDigitosHoras)
            {
                case 2:
                    return String.Format("{0:00}:{1:00}", hh, mm);
                case 3:
                    return String.Format("{0:000}:{1:00}", hh, mm);
                case 4:
                    return String.Format("{0:0000}:{1:00}", hh, mm);
                case 5:
                    return String.Format("{0:00000}:{1:00}", hh, mm);
                case 6:
                    return String.Format("{0:000000}:{1:00}", hh, mm);
                default:
                    return "--:--";
            }

        }

        #endregion

        public static byte[] GetByteArray(string texto)
        {
            byte[] ret = new byte[texto.Length];
            for (int i = 0; i < texto.Length; i++)
            {
                ret[i] = Convert.ToByte(texto[i]);
            }
            return ret;
        }

        public static T[] EnumToArray<T>()
        {
            Type enumType = typeof(T);
            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be a System.Enum");
            }
            return (Enum.GetValues(enumType) as IEnumerable<T>).ToArray();
        }
        /// <summary>
        /// Retorna se a jornada passa de um dia para o outro (inicia antes da meia noite e termina depois)
        /// </summary>
        /// <param name="entradas">Array com as entradas em minutos</param>
        /// <param name="saidas">Array com as saídas em minutos</param>
        /// <returns> Booleano indicando se a jornada ultrapassa a meia noite</returns>
        public static bool JornadaUltrapassaMeiaNoite(int[] entradas, int[] saidas)
        {
            for (int i = 0; i < entradas.Length; i++)
            {
                if (entradas[i] >= 0 && saidas[i] >= 0)
                {
                    if (saidas[i] < entradas[i])
                    {
                        return true;
                    }
                    if (i > 0)
                    {
                        if (entradas[i - 1] > entradas[i])
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static string Description(this Enum value)
        {
            // variables  
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                const int maximoTentativas = 15;
                int tentativas = 0;
                do
                {
                    try
                    {
                        tentativas++;
                        using (MemoryStream stream = new MemoryStream())
                        {
                            serializer.Serialize(stream, serializableObject);
                            stream.Position = 0;
                            xmlDocument.Load(stream);
                            xmlDocument.Save(fileName);
                            stream.Close();
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex is IOException)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(0.2));
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                } while (tentativas <= maximoTentativas);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }
            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                if (!File.Exists(fileName))
                {
                    T obj = (T)Activator.CreateInstance(typeof(T));
                    SerializeObject<T>(obj, fileName);
                }

                const int maximoTentativas = 15;
                int tentativas = 0;
                do
                {
                    try
                    {
                        tentativas++;
                        xmlDocument.Load(fileName);

                        string xmlString = xmlDocument.OuterXml;

                        using (StringReader read = new StringReader(xmlString))
                        {
                            Type outType = typeof(T);

                            XmlSerializer serializer = new XmlSerializer(outType);
                            using (XmlReader reader = new XmlTextReader(read))
                            {
                                objectOut = (T)serializer.Deserialize(reader);
                                reader.Close();
                            }

                            read.Close();
                        }
                        return objectOut;
                    }
                    catch (Exception ex)
                    {
                        if (ex is IOException)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(0.2));
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                } while (tentativas <= maximoTentativas);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Modelo.Proxy.PxyConfigComunicadorServico"))
                {
                    return default(T);
                }
                throw ex;
            }
            return default(T);
        }

        /// <summary>
        /// Data uma lista de números, e um número a ser comparado, o método retorna o número na lista mais próximo do número a ser comparado
        /// </summary>
        /// <param name="listaNumerica">Lista com os números a serem comparados</param>
        /// <param name="numeroBase">Número base para a comparação</param>
        /// <returns>Número da lista mais próximo do número base</returns>
        public static Double NumeroProximo(List<Double> listaNumerica, double numeroBase)
        {
            // Maior após o numero base, pega o primeiro pois vai ser o menor dentro os maiores
            double maior = listaNumerica.Where(x => x >= numeroBase).OrderBy(x => x).FirstOrDefault();
            // Menor após o numero base, pega o ultimo pois vai ser o maior entre os menores
            double menor = listaNumerica.Where(x => x <= numeroBase).OrderBy(x => x).LastOrDefault();
            double igual = listaNumerica.Where(x => x == numeroBase).OrderBy(x => x).FirstOrDefault();

            if (igual == numeroBase)
            {
                return numeroBase;
            }

            double diferencaMaior = Math.Abs(maior - numeroBase);
            double diferencaMenor = Math.Abs(numeroBase - menor);

            if (diferencaMaior <= diferencaMenor)
            {
                return maior;
            }
            else
            {
                return menor;
            }
        }
    }
}
