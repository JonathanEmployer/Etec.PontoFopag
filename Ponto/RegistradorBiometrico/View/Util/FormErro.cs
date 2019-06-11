using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Modelo;
using RegistradorBiometrico.View.Base;
using System.Text;

namespace RegistradorPonto.View.Util
{
    public partial class FormErro : FormBase
    {
        bool expandido;

        public FormErro()
        {
            InitializeComponent();

            this.Size = new Size(461, 167);
            expandido = false;
        }

        public FormErro(String mensagem, Exception exc)
            : this()
        {
            RetornoErro objetoErro = new RetornoErro();
            TentaMontarRetornoErro(exc, ref objetoErro);

            if (objetoErro != null)
            {
                txtMensagem.Text = String.Concat(mensagem, Environment.NewLine, objetoErro.erroGeral);
                txtDetalhes.Text = String.Join(Environment.NewLine, objetoErro.ErrosDetalhados);
            }
            else
            {
                txtMensagem.Text = mensagem;
                txtDetalhes.Text = MontaMensagemErro(exc) + System.Environment.NewLine + "=========================================" + System.Environment.NewLine + System.Environment.NewLine + exc.StackTrace;
            }
        }

        private FormErro(string mensagem, string detalhes) : this()
        {
            txtMensagem.Text = mensagem;
            txtDetalhes.Text = detalhes;

            if (String.IsNullOrEmpty(detalhes))
                btnDetalhes.Enabled = false;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void btnDetalhes_Click(object sender, EventArgs e)
        {
            this.Size = new Size(461, expandido == true ? 167 : 330);
            expandido = !expandido;
        }

        private string MontaMensagemErro(Exception exc)
        {
            if (exc.InnerException != null)
            {
                return ">> " + exc.Message + System.Environment.NewLine + MontaMensagemErro(exc.InnerException);
            }
            else
            {
                return ">> " + exc.Message;
            }
        }

        public static string MontaMensagem(Exception exc)
        {
            if (exc.InnerException != null)
            {
                return exc.Message + " " + FormErro.MontaMensagem(exc.InnerException);
            }
            else
            {
                return exc.Message;
            }
        }

        public static string MontaMensagemComQuebraLinha(Exception exc)
        {
            if (exc.InnerException != null)
            {
                return FormErro.MontaMensagemComQuebraLinha(exc.InnerException) + Environment.NewLine + exc.Message;
            }
            else
            {
                return exc.Message;
            }
        }

        public static void ShowDialog(String mensagem, Exception ex)
        {
            using (var form = new FormErro(mensagem, ex))
            {
                form.ShowDialog();
            }
        }

        public static void Show(String mensagem, Exception ex)
        {
            using (var form = new FormErro(mensagem, ex))
            {
                form.Show();
            }
        }

        public static void ShowDialog(string mensagem, string detalhes)
        {
            using (var form = new FormErro(mensagem, detalhes))
            {
                form.ShowDialog();
            }
        }

        public static void Show(string mensagem, string detalhes)
        {
            using (var form = new FormErro(mensagem, detalhes))
            {
                form.Show();
            }
        }

        private void FormErro_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }


    }
}
