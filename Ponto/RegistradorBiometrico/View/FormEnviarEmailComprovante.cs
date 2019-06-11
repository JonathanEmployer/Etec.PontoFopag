using RegistradorBiometrico.EnviaEmailProducao;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.View.Base;
using RegistradorBiometrico.ViewModel;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegistradorBiometrico.Model.Util;

namespace RegistradorBiometrico.View
{
    public partial class FormEnviarEmailComprovante : FormBase
    {
        private String ComprovanteRegistroHTML;

        public FormEnviarEmailComprovante(ComprovanteRegistro objComprovanteRegistro)
        {
            InitializeComponent();

            ComprovanteRegistroHTML = objComprovanteRegistro.ToHtml<ComprovanteRegistro>("Comprovante de Registro de Ponto");
        }

        public void EnviarEmail()
        {
            try
            {
                if (String.IsNullOrEmpty(txtDestinatario.Text))
                {
                    MessageBox.Show("Preencha o e-mail destinatário.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                EnviaEmailClient client = new EnviaEmailClient();
                client.EnviaEmail("no-reply@employer.com.br", txtDestinatario.Text, "Comprovante de registro de ponto - " + DateTime.Now.ToShortDateString(), 
                    ComprovanteRegistroHTML, null, null, "remetente", String.Empty, String.Empty, null, null, null);
                client.Close(); 

                MessageBox.Show("Email enviado!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FecharTela();
            }
            catch (Exception ex)
            {
                TrataExcecoes("Não foi possível enviar o e-mail.", ex);
            }
        }

        private void sbEnviar_Click(object sender, EventArgs e)
        {
            EnviarEmail();
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void FormEnviarEmailComprovante_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    EnviarEmail();
                    break;
            }
        }
    
    }

}
