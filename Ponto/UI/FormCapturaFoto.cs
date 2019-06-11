using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Drawing.Imaging;

namespace UI
{
    public partial class FormCapturaFoto : Form
    {
        private bool ExisteDispositivo = false;
        private FilterInfoCollection DispositivosDeVideo;
        private VideoCaptureDevice FonteDeVideo = null;
        private string codigoFuncionario { get; set; }
        Image imagem;
        public bool formFechado { get; set; }

        public FormCapturaFoto()
        {
            InitializeComponent();
        }

        public void CarregaDispositivo(FilterInfoCollection Dispositivos)
        {
            for (int i = 0; i < Dispositivos.Count; i++)
            {
                lbcDispositivos.Items.Add(Dispositivos[0].Name.ToString());
            }
            lbcDispositivos.Text = lbcDispositivos.Items[0].ToString();
        }

        public void BuscarDispositivos()
        {
            DispositivosDeVideo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (DispositivosDeVideo.Count == 0)
            {
                ExisteDispositivo = false;
                throw new Exception("Não existe Dispositivos de captura de video configurados");
            }
            else
            {
                ExisteDispositivo = true;
                CarregaDispositivo(DispositivosDeVideo);
            }
        }
        public void TerminarFonteDeVideo()
        {
            if (!(FonteDeVideo == null))
            {
                if (FonteDeVideo.IsRunning)
                {
                    FonteDeVideo.SignalToStop();
                    FonteDeVideo = null;
                }
            }
        }

        private void video_NovoFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagem = (Bitmap)eventArgs.Frame.Clone();
            picCapture.Image = Imagem;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BuscarDispositivos();
            this.lbcDispositivos.SelectedIndex = -1;
        }

        private void sbIniciarVisualizacao_Click(object sender, EventArgs e)
        {
            if (ExisteDispositivo)
            {
                FonteDeVideo = new VideoCaptureDevice(DispositivosDeVideo[lbcDispositivos.SelectedIndex].MonikerString);
                FonteDeVideo.NewFrame += new NewFrameEventHandler(video_NovoFrame);
                FonteDeVideo.Start();
                picCapture.Enabled = false;
            }
            else
            {
                MessageBox.Show("Erro: Nenhum dispositivo encontrado");
            }   
        }

        private void sbAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void FormCapturaFoto_Load(object sender, EventArgs e)
        {
            BuscarDispositivos();
        }

        private void sbSalvar_Click(object sender, EventArgs e)
        {
            TerminarFonteDeVideo();
            imagem = picCapture.Image;                 
            FecharTela();
        }


        public Image retornaImagem()
        {
            if (imagem != null)
                return imagem;
            else
                throw new Exception("Ocorreu um erro ao gravar a imagem, por favor verifique");            
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            imagem = null;
            FecharTela();
            formFechado = true;
        }

        private void FecharTela()
        {
            TerminarFonteDeVideo();
            this.Dispose();
        }

        private void FormCapturaFoto_FormClosing(object sender, FormClosingEventArgs e)
        {
            FecharTela();
        }
    }
}
