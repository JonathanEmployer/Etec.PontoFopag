using RegistradorBiometrico.Model;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.View.Base;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegistradorBiometrico.Model.Util;

namespace RegistradorBiometrico.ViewModel
{
    public class ImpressaoComprovante
    {

        public static void CriarArquivoPDF(string caminhoArquivo, string nomeArquivo, ComprovanteRegistro objComprovanteDeRegistro)
        {
            try
            {
                FormRelatorioBase form = new FormRelatorioBase(objComprovanteDeRegistro, caminhoArquivo, nomeArquivo);
                form.GerarPDF();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar o PDF", ex);
            }
            
        }

        public static void CriarArquivoPNG(string caminhoArquivo, string nomeArquivo, ComprovanteRegistro objComprovanteDeRegistro)
        {
            try
            {
                FormRelatorioBase form = new FormRelatorioBase(objComprovanteDeRegistro, caminhoArquivo, nomeArquivo);
                form.GerarImagem();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar a Imagem", ex);
            }
        }

    }
}
