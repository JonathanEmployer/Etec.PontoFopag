using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace cwkComunicadorWebAPIPontoWeb.Utils
{
    public static class RichTextBoxExtensions
    {
        public static void NewLineText(this RichTextBox box, string text, Color color)
        {
            const int numMaximoLinhas = 501;
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text + Environment.NewLine);
            VerificaNumMaximoLinhas(box, numMaximoLinhas);
            box.SelectionColor = box.ForeColor;
            try
            {
                SalvaLogEmArquivo(box);
            }
            catch (Exception)
            {
                Thread.Sleep(500);
                SalvaLogEmArquivo(box);
            }
        }

        private static void SalvaLogEmArquivo(RichTextBox box)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "txt";
            saveFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFile.CreatePrompt = true;
            saveFile.OverwritePrompt = true;
            saveFile.FileName = @CwkUtils.FileLogStringUtil() + "\\" + "LogsTela.txt";
            box.SaveFile(saveFile.FileName, RichTextBoxStreamType.PlainText);
        }

        private static void VerificaNumMaximoLinhas(RichTextBox box, int numMaximoLinhas)
        {
            int numLinhasExcedentes = RetornaNumeroLinhas(box) - numMaximoLinhas;
            if (numLinhasExcedentes > 0)
            {
                RetiraNumPrimeirasLinhas(numLinhasExcedentes, box);
            }
        }

        private static void RetiraNumPrimeirasLinhas(int numLinhasExcedentes, RichTextBox box)
        {
            string[] vetorLinhas = box.Text.Split('\n');

            IList<string> listaLinhas = vetorLinhas.ToList();
            IList<string> listaLinhasEditada = new List<string>();

            int contador = 0;

            foreach (var linha in listaLinhas)
            {
                if (contador >= numLinhasExcedentes)
                {
                    listaLinhasEditada.Add(linha);
                }
                else
                {
                    contador++;
                }
            }

            box.Text = String.Join(Environment.NewLine, listaLinhasEditada.ToArray());
        }

        public static int RetornaNumeroLinhas(this RichTextBox box)
        {
            int numLines = box.Text.Split('\n').Length;
            return numLines;
        }
    }
}
