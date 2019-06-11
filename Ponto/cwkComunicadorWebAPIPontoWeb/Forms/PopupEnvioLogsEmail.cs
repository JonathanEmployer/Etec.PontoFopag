using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.Utils;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwkComunicadorWebAPIPontoWeb.Forms
{
    public partial class PopupEnvioLogsEmail : Form
    {
        public PopupEnvioLogsEmail()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtDestinatario.Text))
            {
                DialogResult dialogResult = MessageBox.Show("Não foi informado nenhum destinatário para o e-mail, será enviado apenas para o e-mail padrão da Pontofopag", "Deseja continuar?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dialogResult == DialogResult.Yes)
                {
                    EnviarLogs();
                }
            }
            else
            {
                EnviarLogs();
            }
            
        }

        private void EnviarLogs()
        {
            try
            {
                btnEnviar.Text = "Enviando...";
                btnEnviar.Enabled = btnCancelar.Enabled = false;

                cwkPontoMT.Integracao.Entidades.Empresa empregador = new cwkPontoMT.Integracao.Entidades.Empresa();
                empregador = Utils.CwkUtils.EmpresaRep(); 
                string assunto = "Log Comunicador da Empresa " + empregador.RazaoSocial + "Gerado as " + System.DateTime.Now.ToString("G");
                string corpo = "Segue em anexo os logs do comunicador enviado por " + Utils.CwkUtils.NomeUsuario();
                EnviarEmail.EnviarEmailErroComLogs(assunto, corpo, txtDestinatario.Text);
                MessageBox.Show("Logs enviados com sucesso");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar os logs. Erro: " + ex.Message);
                string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "Envio_Logs_Por_Email" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                CwkUtils.LogarExceptions("Erro ao Enviar Logs", ex, filePath);
                btnEnviar.Text = "Enviar";
                btnEnviar.Enabled = btnCancelar.Enabled = true;
            }

            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PopupEnvioLogsEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnEnviar.PerformClick();
        }

        private void txtDestinatario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnEnviar.PerformClick();
        }
    }
}
