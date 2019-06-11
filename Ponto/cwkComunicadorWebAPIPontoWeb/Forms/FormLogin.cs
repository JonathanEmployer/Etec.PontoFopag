using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace cwkComunicadorWebAPIPontoWeb
{
    public partial class FormLogin : Form
    {
        public bool Logged { get; set; }
        private CancellationToken cts;
        public Progress<ReportaErro> progress { get; set; }
        public bool fechou = false;
        public XDocument xD = null;
        public FormLogin()
        {
            InitializeComponent();
            cts = new CancellationToken();
            progress = new Progress<ReportaErro>(ReportProgress);
        }

        private async void sbEntrar_Click(object sender, EventArgs e)
        {
            string sel = "";
            if (cbWS.SelectedIndex >= 0)
            {
                sel = cbWS.SelectedItem.ToString();
            }
            else
            {
                sel = cbWS.Text;
            }
            ViewModels.VariaveisGlobais.SetEndWS(sel);
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
                LoginBLL bll = new LoginBLL();
                BeginInvoke(new Action(() =>
                {
                    sbEntrar.Text = "Aguarde";
                    sbEntrar.Enabled = false;
                    txtSenha.Enabled = false;
                    txtUsuario.Enabled = false;
                }));
                TokenResponseViewModel login = await bll.LoginAsync(txtUsuario.Text, txtSenha.Text, ViewModels.VariaveisGlobais.URL_WS, cts, progress);
                if (!String.IsNullOrEmpty(login.AccessToken))
                {
                    XDocument xD = bll.GetXmlConf();
                    login.AtualizacaoAutomatica = Convert.ToInt32(AtualizarAplicativo.AtualizacaoAutomatica());
                    login.pass = txtSenha.Text;
                    if (await bll.SetXmlRegisterData(login))
                    {
                        BeginInvoke(new Action(() =>
                        {
                            this.Close();
                        }));
                    }
                    else
                    {
                        lblProgress.Text = "Erro ao gravar configuração";
                    }
                }
                if (!String.IsNullOrEmpty(login.ErrorDescription))
                {
                    if (!login.ErrorDescription.Contains("400 (Bad Request)"))
                    {
                        MessageBox.Show("Ocorreu um erro ao realizar o login:\r\n" + login.ErrorDescription);
                    }
                } 
                BeginInvoke(new Action(() =>
                {
                    sbEntrar.Text = "Entrar";
                    sbEntrar.Enabled = true;
                    txtSenha.Enabled = true;
                    txtUsuario.Enabled = true;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao realizar o login:\r\n" + ex.Message);
                BeginInvoke(new Action(() =>
                {
                    sbEntrar.Text = "Entrar";
                    sbEntrar.Enabled = true;
                    txtSenha.Enabled = true;
                    txtUsuario.Enabled = true;
                }));
            }
        }

        private void SBtnFechar_Click(object sender, EventArgs e)
        {
            fechou = true;
            Application.Exit();
        }

        private void ReportProgress(ReportaErro value)
        {
            lblProgress.Text = value.Mensagem;
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

        private void TrataEnter(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U)
            {
                cbWS.Visible = true;
            }
            switch (e.KeyCode)
            {
                case Keys.Return:
                    if (!String.IsNullOrEmpty(txtSenha.Text) && !String.IsNullOrEmpty(txtUsuario.Text))
                    {
                        Task.Factory.StartNew(() => sbEntrar_Click(sbEntrar, new EventArgs())).ConfigureAwait(false);
                    }
                    break;
                default:
                    break;
            }
        }

        private void FormLogin_Shown(object sender, EventArgs e)
        {
            LoginBLL bll = new LoginBLL();
            XDocument xD = new XDocument();
            try
            {
                xD = bll.GetXmlConf();
                string tk = CriptoString.Decrypt(xD.Element("ConfiguracaoPontofopag").Element("tk").Value);
                if (!String.IsNullOrEmpty(xD.Element("ConfiguracaoPontofopag").Element("un").Value) && String.IsNullOrEmpty(tk))
                {
                    txtUsuario.Text = xD.Element("ConfiguracaoPontofopag").Element("un").Value;
                    txtSenha.Text = CriptoString.Decrypt(xD.Element("ConfiguracaoPontofopag").Element("ps").Value);
                    sbEntrar_Click(null, null);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void FormLogin_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtUsuario_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
