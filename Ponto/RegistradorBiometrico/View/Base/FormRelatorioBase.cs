using RegistradorBiometrico.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using RegistradorBiometrico.Model.Util;


namespace RegistradorBiometrico.View.Base
{
    public partial class FormRelatorioBase : Form
    {
        private ComprovanteRegistro ComprovanteDeRegistro;
        private String Caminho;
        private String NomeArquivo;

        public FormRelatorioBase()
        {
            InitializeComponent();
        }


        public FormRelatorioBase(ComprovanteRegistro comprovante, String caminho, String nomeArquivo) : this()
        {
            ComprovanteDeRegistro = comprovante;
            Caminho = caminho;
            NomeArquivo = nomeArquivo;
        }
        
        private void FormRelatorioBase_Load(object sender, EventArgs e)
        {
            this.reportViewer.RefreshReport();
        }

        public void GerarPDF()
        {
            ExportarRelatorio("Pdf", null, "PDF Files|*.pdf");
        }

        public void GerarImagem()
        {
            ExportarRelatorio("Image","<DeviceInfo><OutputFormat>PNG</OutputFormat></DeviceInfo>", "PNG Image|*.png");
        }

        private void ExportarRelatorio(string format, string deviceInfo, string filter)
        {
            byte[] bytes = GerarRelatorio(format, deviceInfo);

            string caminho = String.Empty;

            Thread td = new Thread(new ThreadStart(() => caminho = this.AbrirOpcaoSalvarComo(filter)));
            td.SetApartmentState(ApartmentState.STA);
            td.IsBackground = true;
            td.Start();
            td.Join();
            
            if (String.IsNullOrEmpty(caminho))
                return;

            File.WriteAllBytes(caminho, bytes);
            Process.Start(caminho);
        }
        
        [STAThread]
        private string AbrirOpcaoSalvarComo(string filter)
        {
            try
            {
                SaveFileDialog saveFile = new SaveFileDialog();

                saveFile.Filter = filter;
                saveFile.InitialDirectory = Caminho;
                saveFile.FileName = NomeArquivo;
                DialogResult opcao = saveFile.ShowDialog();

                if (opcao == System.Windows.Forms.DialogResult.Cancel)
                    saveFile.FileName = String.Empty;
                
                return saveFile.FileName;
            }
            catch
            {
                return String.Empty;
            }
        }

        private byte[] GerarRelatorio(string format, string deviceInfo)
        {
            this.comprovanteRegistroBindingSource.DataSource = ComprovanteDeRegistro;
            return reportViewer.LocalReport.Render(format, deviceInfo);
        }
    }
}
