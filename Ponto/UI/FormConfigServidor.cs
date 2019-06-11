using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormConfigServidor : UI.Base.ManutBase
    {
        public FormConfigServidor()
        {
            InitializeComponent();
            this.Name = "FormConfigServidor";
        }

        public override void CarregaObjeto()
        {
            base.CarregaObjeto();
            try
            {
                string servidor, compartilhamento;
                BLL.ConfigServidor.CarregaCaminhos(out servidor, out compartilhamento);
                txtServidor.Text = servidor;
                txtCompartilhamento.Text = compartilhamento;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao carregar arquivo de configuração. Verifique: \n" + ex.Message);
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            return BLL.ConfigServidor.Salvar(txtServidor.Text, txtCompartilhamento.Text);
        }

        private void FormManutConfiguracoesGerais_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    sbGravar.Focus();
                    sbGravar_Click(sender, e);
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela sem salvar as alterações?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        sbCancelar.Focus();
                        sbCancelar_Click(sender, e);
                    }
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, cwTabela);
                    break;
            }
        }
    }
}
