using System;
using System.Windows.Forms;

namespace ParametrizarComunicadorServico
{
    public partial class FormLogin : Form
    {
        public bool fechou = false;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        }

        private void Notificacao_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            btnEntrar.Text = "Aguarde";
            if (txtUsuario.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Informe o usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Focus();
                return;
            }
            if (txtSenha.Text.Trim() == String.Empty)
            {
                MessageBox.Show("Informe a Senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Focus();
                return;
            }
            try
            {
                BeginInvoke(new Action(() =>
                {
                    btnEntrar.Text = "Aguarde";
                    btnEntrar.Enabled = false;
                    txtSenha.Enabled = false;
                    txtUsuario.Enabled = false;
                }));
                Modelo.Proxy.PxyConfigComunicadorServico login = await Negocio.Login.RealizarLogin(txtUsuario.Text, txtSenha.Text);
                
                if (!String.IsNullOrEmpty(login.Erro))
                {
                    MessageBox.Show(login.Erro);
                }
                else
                {
                    BeginInvoke(new Action(() =>
                    {
                        this.Close();
                    }));
                }

                BeginInvoke(new Action(() =>
                {
                    btnEntrar.Text = "Entrar";
                    btnEntrar.Enabled = true;
                    txtSenha.Enabled = true;
                    txtUsuario.Enabled = true;
                }));
                btnEntrar.Text = "Entrar";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao realizar o login:\r\n" + ex.Message);
                BeginInvoke(new Action(() =>
                {
                    btnEntrar.Text = "Entrar";
                    btnEntrar.Enabled = true;
                    txtSenha.Enabled = true;
                    txtUsuario.Enabled = true;
                }));
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            fechou = true;
            Application.Exit();
        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {
            Modelo.Proxy.PxyConfigComunicadorServico config = null;
            try
            {
                config = Negocio.Configuracao.GetConfiguracao();
                if (config == null || (!String.IsNullOrEmpty(config.Usuario) && !String.IsNullOrEmpty(config.Senha)))
                {
                    txtUsuario.Text = config.Usuario;
                    txtSenha.Text = config.Senha;
                    btnEntrar_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                if (config == null || (!String.IsNullOrEmpty(config.Usuario) && !String.IsNullOrEmpty(config.Senha)))
                {
                    txtUsuario.Text = config.Usuario;
                    txtSenha.Text = config.Senha;
                }
                if (ex.Message.Contains("Nome de usuário"))
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Erro ao realizar o Login, revise os dados de acesso, detalhe: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
            }
        }

        private void txtUsuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEntrar.PerformClick();
            }
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnEntrar.PerformClick();
            }
        }   
    }
}
