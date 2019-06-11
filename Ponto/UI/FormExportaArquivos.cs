using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormExportaArquivos : Form
    {
        private BLL.Empresa bllEmpresa;
        private BLL.ExportaArquivos bllExpArquivos;
        private string nomeArquivo = "";
        private int idEmpresa;

        private string _tabela;
        public string cwTabela
        {
            get { return _tabela; }
            set { _tabela = value; }
        }

        public List<string> TelasAbertas { get; set; }
        
        public FormExportaArquivos()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllExpArquivos = new BLL.ExportaArquivos();
            this.Name = "FormExportaArquivos";
            comboEmpresa.Properties.DataSource = bllEmpresa.GetAll();
            _tabela = "Exportação Ministério do Trabalho";
        }
        

        private void btEmpresa_Click(object sender, EventArgs e)
        {

            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresas";
            form.cwId = (int)comboEmpresa.EditValue;
            GridSelecao(form, comboEmpresa, bllEmpresa);
        }

        protected virtual void GridSelecao<T>(Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
        {

            UI.Util.Funcoes.ChamaGridSelecao(pGrid);
            if (pGrid.cwAtualiza == true)
            {
                pCb.Properties.DataSource = bll.GetAll();
            }
            if (pGrid.cwRetorno != 0)
            {
                pCb.EditValue = pGrid.cwRetorno;
            }
            pCb.Focus();
        }

        private void btDiretorio_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "Arquivos de Texto(*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    nomeArquivo = saveFileDialog1.FileName;
                txtEdtDiretorio.Text = nomeArquivo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btGravar_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (ValidaCampos())
            {
                FormProgressBar formPBExporta = new FormProgressBar();
                formPBExporta.Show();
                
                bllExpArquivos = new BLL.ExportaArquivos(nomeArquivo, comboTipoArquivo.SelectedIndex, idEmpresa, dateEditInicial.DateTime, dateEditFinal.DateTime, formPBExporta.progressBar);
                formPBExporta.Close();

                MessageBox.Show("O arquivo foi exportado com sucesso.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Preencha os campos corretamente.");
            }
            this.Enabled = true;
        }

        private bool ValidaCampos()
        {
            bool ret = true;
            if (comboTipoArquivo.SelectedIndex > -1)
            {
                dxErrorProvider1.SetError(comboTipoArquivo, "");
            }
            else
            {
                dxErrorProvider1.SetError(comboTipoArquivo, "Selecione o tipo de arquivo.");
                ret = false;
            }

            if (dateEditInicial.DateTime != new DateTime())
            {
                dxErrorProvider1.SetError(dateEditInicial, "");
            }
            else
            {
                dxErrorProvider1.SetError(dateEditInicial, "Selecione a data inicial.");
                ret = false;
            }

            if (dateEditFinal.DateTime == new DateTime())
            {
                dxErrorProvider1.SetError(dateEditFinal, "Selecione a data final.");
                ret = false;
            }
            else if (dateEditFinal.DateTime < dateEditInicial.DateTime)
            {
                dxErrorProvider1.SetError(dateEditFinal, "A data final não pode ser menor do que a data inicial.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(dateEditFinal, "");
            }

            if (nomeArquivo != "")
            {
                dxErrorProvider1.SetError(txtEdtDiretorio, "");
            }
            else
            {
                dxErrorProvider1.SetError(txtEdtDiretorio, "Selecione o diretório para salvar o arquivo.");
                ret = false;
            }

            if ((int)comboEmpresa.EditValue > 0)
            {
                dxErrorProvider1.SetError(comboEmpresa, "");
            }
            else
            {
                dxErrorProvider1.SetError(comboEmpresa, "Selecione uma empresa.");
                ret = false;
            }

            return ret;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            idEmpresa = (int) comboEmpresa.EditValue;
        }

        private void FormExportaArquivos_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                    btGravar.Focus();
                    btGravar_Click(sender, e);
                    break;
                case Keys.Enter:
                    btGravar.Focus();
                    btGravar_Click(sender, e);
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela sem salvar as alterações?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        btCancel.Focus();
                        btCancel_Click(sender, e);
                    }
                    break;
                case Keys.F1:
                    btAjuda_Click(sender, e);
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, cwTabela);
                    break;
            }
        }

        private void FormExportaArquivos_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void btAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void txtEdtDiretorio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btDiretorio.PerformClick();
            }
        }
    }
}
