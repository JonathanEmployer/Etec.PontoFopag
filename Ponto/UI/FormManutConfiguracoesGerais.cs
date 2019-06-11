using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutConfiguracoesGerais : UI.Base.ManutBase
    {
        private BLL.ConfiguracoesGerais bllConfiguracoesGerais;
        private Modelo.ConfiguracoesGerais objConfiguracoesGerais;

        public FormManutConfiguracoesGerais()
        {
            InitializeComponent();
            bllConfiguracoesGerais = new BLL.ConfiguracoesGerais();
            this.Name = "FormManutConfiguracoesGerais";
        }

        public override void CarregaObjeto()
        {
            base.CarregaObjeto();
            try
            {
                objConfiguracoesGerais = bllConfiguracoesGerais.LoadObject(Application.StartupPath);
                txtDataInicial.Value = objConfiguracoesGerais.DataInicial;
                txtDataFinal.Value = objConfiguracoesGerais.DataFinal;
                chbMudarPeriodoAposDataFinal.Checked = objConfiguracoesGerais.MudarPeriodoAposDataFinal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problema ao carregar arquivo de configuração. Verifique: \n" + ex.Message);
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            objConfiguracoesGerais.DataInicial = Convert.ToInt32(txtDataInicial.Value);
            objConfiguracoesGerais.DataFinal = Convert.ToInt32(txtDataFinal.Value);
            objConfiguracoesGerais.MudarPeriodoAposDataFinal = chbMudarPeriodoAposDataFinal.Checked;
            base.Salvar();
            return bllConfiguracoesGerais.Salvar(Application.StartupPath, objConfiguracoesGerais);
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
