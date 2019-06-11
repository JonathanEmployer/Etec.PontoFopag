using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Modelo.Utils
{
    public class Utils
    {
        public static void EscreveLog(string nomeArquivo, string log)
        {
            try
            {
                string caminhoLog = ConfigurationManager.AppSettings["caminhoLog"];
                if (!String.IsNullOrEmpty(caminhoLog))
                {
                    CriaPastaInexistente(caminhoLog);
                    EscreveLog(caminhoLog, nomeArquivo, log);
                }
            }
            catch (Exception)
            {

            }
        }

        public static void EscreveLog(string caminhoPasta, string nomeArquivo, string log)
        {
            try
            {
                StreamWriter file2 = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt", true);
                CultureInfo cult = new CultureInfo("pt-BR");
                string dta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cult);
                file2.WriteLine(dta + " - " + log);
                file2.Close();
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
    }
}
