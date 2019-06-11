using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_N.Utils
{
    public class Utils
    {
        public static void LogarTxt(string nomeArquivo, string log)
        {
            if (System.Configuration.ConfigurationSettings.AppSettings["Log"] == "1")
            {
                if (String.IsNullOrEmpty(System.Configuration.ConfigurationSettings.AppSettings["CaminhoLog"]))
                {
                    EscreveLog(nomeArquivo, log);
                }
                else
                {
                    EscreveLog(System.Configuration.ConfigurationSettings.AppSettings["CaminhoLog"], nomeArquivo, log);
                }
            }
        }

        public static void EscreveLog(string nomeArquivo, string log)
        {
            try
            {
                string diretorio = System.IO.Directory.GetCurrentDirectory();
                diretorio = Path.Combine(diretorio, "Log");
                CriaPastaInexistente(diretorio);
                EscreveLog(diretorio, nomeArquivo, log);
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
