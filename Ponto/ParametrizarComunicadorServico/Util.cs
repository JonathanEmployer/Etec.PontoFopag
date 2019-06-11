using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametrizarComunicadorServico
{
    public class Util
    {
        public static void EscreveArquivo(string caminho, string nomeArquivo, IList<string> texto)
        {
            string diretorio = Path.Combine(caminho, nomeArquivo + ".txt");
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
