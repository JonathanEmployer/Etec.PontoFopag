using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.ViewModel;
using RegistradorPonto.Util;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RegistradorBiometrico.View
{
    public partial class FormLogin : Form
    {
        LoginViewModel login = new LoginViewModel();
        public bool Loggado { get; set; }
        private CancellationToken cts;
        public XDocument xmlConfiguracao = null;
        public string mac_adress { get; set; }

        public FormLogin(String pMac_adress)
        {
            InitializeComponent();

            cts = new CancellationToken();
            mac_adress = pMac_adress;
        }

        #region Eventos

        private async void sbEntrar_Click(object sender, EventArgs e)
        {
            if (ValidaCamposLogin())
            {
                DesabilitaBotaoEntrar();
                Boolean conectividadeInternet = await ConectividadeUtil.FazVerificacaoInternet();
                if (conectividadeInternet)
                {
                    Boolean conectividadeWS = await ConectividadeUtil.FazVerificacaoWS();
                    if (conectividadeWS)
                    {
                        try
                        {
                            Usuario usuario = new Usuario() { Login = txtUsuario.Text, Senha = txtSenha.Text };

                            bool entrar = await login.EfetuarLogin(usuario, mac_adress, cts);

                            if (entrar)
                            {
                                FecharTela();

                                FormRegistraPonto form = new FormRegistraPonto(false);
                                form.Closed += (s, args) => this.Close();
                                form.ShowDialog();
                            }
                        }
                        catch (Exception ex)
                        {
                            HabilitaOuDesabilitaCampos(true, "Entrar");
                            lblProgress.Text = ex.Message;

                            MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        SetaMensagemErro("Não é possível comunicar com o servidor. Aguarde alguns minutos, caso o problema persista contate o Suporte.");
                    }
                }
                else
                {
                    SetaMensagemErro("Verifique sua conexão com a Internet! Nâo será possível importar/exportar dados enquanto estiver off-line.");
                }
            }
        }

        private void SetaMensagemErro(String msg)
        {
            MessageBox.Show(msg, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            HabilitaOuDesabilitaCampos(true, "Entrar");
            lblProgress.Text = String.Empty;
        }

        private void DesabilitaBotaoEntrar()
        {
            BeginInvoke(new Action(() =>
            {
                HabilitaOuDesabilitaCampos(false, "Aguarde");
            }));

            lblProgress.Text = "Enviado dados para login";
            lblProgress.ForeColor = System.Drawing.Color.White;
        }

        public void HabilitaOuDesabilitaCampos(bool habilitar, string mensagemButton)
        {
            sbEntrar.Text = mensagemButton;
            sbEntrar.Enabled = habilitar;
            txtSenha.Enabled = habilitar;
            txtUsuario.Enabled = habilitar;
        }

        private void FecharTela()
        {
            this.Hide();
        }

        private void SBtnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormLogin_KeyUp(object sender, KeyEventArgs e)
        {
            TrataEnter(e);
        }

        private void txtUsuario_KeyUp(object sender, KeyEventArgs e)
        {
            TrataEnter(e);
        }

        private void txtSenha_KeyUp(object sender, KeyEventArgs e)
        {
            TrataEnter(e);
        }

        #endregion

        private Boolean ValidaCamposLogin()
        {
            Boolean camposValidos = true;

            if (camposValidos && string.IsNullOrEmpty(txtUsuario.Text))
            {
                MessageBox.Show("Informe o usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Focus();
                camposValidos = false;
            }

            if (camposValidos && String.IsNullOrEmpty(txtSenha.Text))
            {
                MessageBox.Show("Informe a Senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Focus();
                camposValidos = false;
            }

            return camposValidos;
        }

        private void TrataEnter(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (ValidaCamposLogin())
                    {
                        sbEntrar_Click(sbEntrar, new EventArgs());
                    }
                    break;
            }
        }

    }
}
