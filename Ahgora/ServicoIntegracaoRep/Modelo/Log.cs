using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServicoIntegracaoRep.Modelo
{
    public class Log
    {
        public static void EscreverLogTxtLocal(string log)
        {
            EscreverLogTxtLocal("log", log);
        }

        public static void EscreverLogTxtLocal(string nomeArquivo, string log)
        {
            string path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
            path = path + "\\" + nomeArquivo + ".txt";

            CultureInfo cult = new CultureInfo("pt-BR");
            string dta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cult);
            log = dta + " - " + log;

            const int maximoTentativas = 10;
            int tentativas = 0;
            do
            {
                try
                {
                    tentativas++;
                    var list = new List<string>();
                    // Tenta abrir o arquivo...
                    using (FileStream arquivo = new FileStream(path, FileMode.OpenOrCreate,
                        FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(arquivo, Encoding.UTF8))
                        {
                            string line;
                            while ((line = streamReader.ReadLine()) != null)
                            {
                                list.Add(line);
                            }
                        }

                        if (list.Count() >= 5000)
                            list.RemoveAt(0);
                        list.Add(log);

                        using (StreamWriter streamWriter = new StreamWriter(path))
                        {
                            list.ForEach(streamWriter.WriteLine);
                        }
                    }
                    break;
                }
                catch (IOException ex)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.3));
                }
            } while (tentativas <= maximoTentativas);
        }
    }
}
