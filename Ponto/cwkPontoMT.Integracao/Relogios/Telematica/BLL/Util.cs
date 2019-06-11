using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace cwkPontoMT.Integracao.Relogios.Telematica.BLL
{
    public class Util
    {
        public static string ConvertIP15Digitos(string ip)
        {
            List<string> ipPartsStr = ip.Split('.').Select(Int32.Parse).ToList().ConvertAll<string>(x => x.ToString("D3"));
            return String.Join(".", ipPartsStr);
        }

        public static void EscreveLogCaminhoBase(string nomeArquivo, string log, string localArquivo)
        {
            try
            {
                string diretorio = localArquivo;
                CriaPastaInexistente(diretorio);
                EscreveLog(diretorio, nomeArquivo, log);
            }
            catch (Exception)
            {

            }
        }

        public static void CriaPastaInexistente(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        public static void EscreveLog(string caminhoPasta, string nomeArquivo, string log)
        {
            try
            {
                StreamWriter file2 = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt", true);
                CultureInfo cult = new CultureInfo("pt-BR");
                file2.WriteLine(log);
                file2.Close();
            }
            catch (Exception)
            {
            }
        }

        public static string TratarZeroAEsquerda(string valor, int quantidadeZeros)
        {
            return valor = valor.PadLeft(quantidadeZeros, '0');
        }

        public static void LimpaArquivo(string caminhoPasta, string nomeArquivo)
        {
            StreamWriter file = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt");
            file.Write(String.Empty);
            file.Close();
        }

        public string ConvertTimeZone(TimeZoneInfo utc)
        {
            string teste = Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, TimeZoneInfo.Local));
            return teste;
        }

        public static void EscreveArquivo(string caminho, string nomeArquivo, IList<string> texto)
        {
            string diretorio = Path.Combine(caminho, nomeArquivo+".txt");
            using (FileStream fs = new FileStream(diretorio, FileMode.Create))
            {
                foreach (string linha in texto)
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(linha);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                    info = new UTF8Encoding(true).GetBytes(Environment.NewLine);
                    fs.Write(info, 0, info.Length);
                }
            }
        }
    }
}
