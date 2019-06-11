using DevExpress.XtraEditors;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.View.Base;
using RegistradorBiometrico.ViewModel;
using System;
using System.Threading;
using System.Windows.Forms;

namespace RegistradorBiometrico.View
{
    public partial class FormLoginConfiguracao : FormBase
    {
        #region Propriedades
        public bool UsuarioLogado { get; private set; }
        #endregion

        #region Atributos
        private LoginViewModel loginViewModel;
        private CancellationToken cts;
        #endregion

        public FormLoginConfiguracao()
        {
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            cts = new CancellationToken();
        }

        #region Métodos

        public void HabilitaOuDesabilitaCampos(String mensagemButton, Boolean bHabilitado)
        {
            BeginInvoke(new Action(() =>
            {
                sbOk.Text = mensagemButton;
                base.HabilitaOuDesabilitaCampos(this, bHabilitado);
            }));
        }

        private async void VerificaUsuarioConfiguracao()
        {
            try
            {
                if (String.IsNullOrEmpty(txtLogin.Text) && String.IsNullOrEmpty(txtSenha.Text))
                {
                    MessageBox.Show("Preencha os campos!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                HabilitaOuDesabilitaCampos("Aguarde", false);
                Usuario usuario = new Usuario() { Login = txtLogin.Text, Senha = txtSenha.Text };

                bool entrar = await loginViewModel.EfetuarLoginConfiguracao(usuario, cts);

                if (entrar)
                {
                    UsuarioLogado = true;
                    FecharTela();
                }
            }
            catch (Exception ex)
            {
                HabilitaOuDesabilitaCampos("Ok", true);
                TrataExcecoes(ex);
            }
        } 

        #endregion

        #region Eventos

        private void btOk_Click(object sender, System.EventArgs e)
        {
            VerificaUsuarioConfiguracao();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            FecharTela();
        }

        private void FormLoginConfiguracao_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    VerificaUsuarioConfiguracao();
                    break;
            }
        }

        #endregion

    }
}
