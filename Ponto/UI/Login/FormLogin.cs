using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Login
{
    public partial class FormLogin : Form
    {
        Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();

        BLL.Cw_Usuario bllCw_Usuario = BLL.Cw_Usuario.GetInstancia;

        string retorno = "";

        public bool cwRetorno = false;

        public FormLogin()
        {
            InitializeComponent();

            lblVersao.Text = "Versão " + Modelo.Global.Versao;
        }

        private void sbEntrar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                MessageBox.Show("Informe o usuário.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Focus();
                return;
            }
            if (txtSenha.Text == "")
            {
                MessageBox.Show("Informe a Senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Focus();
                return;
            }

            objUsuario = bllCw_Usuario.ValidaUsuario(txtUsuario.EditValue.ToString(), txtSenha.EditValue.ToString(), ref retorno);
            if (objUsuario == null)
            {
                MessageBox.Show(retorno, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSenha.Text = "";
                txtUsuario.Focus();
                return;
            }

            Modelo.Global.objUsuarioLogado = objUsuario;
            cwRetorno = true;
            this.Close();
        }

        private void FormLogin_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    simpleButton1_Click(sender, e);
                    break;
                case Keys.Enter:
                    sbEntrar_Click(sender, e);
                    break;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            cwRetorno = false;
            this.Close();
        }
    }
}