using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutExportacaoFolha : UI.Base.ManutBase
    {
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcionario bllFuncionario;
        private BLL.ExportacaoCampos bllExportacaoCampos;

        private int IdLayout;
        
        public FormManutExportacaoFolha(int pIdLayout)
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllExportacaoCampos = new BLL.ExportacaoCampos();
            bllDepartamento = new BLL.Departamento();
            bllFuncionario = new BLL.Funcionario();
            rgTipo.SelectedIndex = 0;
            IdLayout = pIdLayout;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch((int)rgTipo.EditValue)
            {
                case 0:
                    cbIdEmpresa.Enabled = true;
                    cbIdDepartamento.Enabled = false;
                    cbIdFuncionario.Enabled = false;
                    sbEmpresa.Enabled = true;
                    sbDepartamento.Enabled = false;
                    sbFuncionario.Enabled = false;
                    cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();
                    break;
                case 1:
                    cbIdEmpresa.Enabled = true;
                    cbIdFuncionario.Enabled = false;
                    cbIdDepartamento.Enabled = true;
                    sbDepartamento.Enabled = true;
                    sbFuncionario.Enabled = false;
                    sbEmpresa.Enabled = true;
                    cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);
                    break;
                case 2:
                    cbIdEmpresa.Enabled = false;
                    cbIdDepartamento.Enabled = false;
                    cbIdFuncionario.Enabled = true;
                    sbFuncionario.Enabled = true;
                    sbEmpresa.Enabled = false;
                    sbDepartamento.Enabled = false;                    
                    cbIdFuncionario.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();
                    break;
                default:
                    break;
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            Cursor.Current = Cursors.WaitCursor;
            int identificacao;
            switch((int)rgTipo.EditValue)
            {
                case 0:
                    identificacao = (int)cbIdEmpresa.EditValue;
                    break;
                case 1:
                    identificacao = (int)cbIdDepartamento.EditValue;
                    break;
                case 2:
                    identificacao = (int)cbIdFuncionario.EditValue;
                    break;
                default:
                    identificacao = -1;
                    break;
            }

            Dictionary<string, string> ret = bllExportacaoCampos.ExportarFolha(txtDataInicial.DateTime, txtDataFinal.DateTime, (int)rgTipo.EditValue, identificacao, txtCaminho.Text, IdLayout);
            Cursor.Current = Cursors.Default;
            return ret;
        }

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);

        }

        private void cbIdDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            cbIdFuncionario.Properties.DataSource = bllFuncionario.GetPorDepartamento((int)cbIdEmpresa.EditValue, (int)cbIdDepartamento.EditValue, false);
        }

        private void sbEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)cbIdEmpresa.EditValue;
            GridSelecao(form, cbIdEmpresa, bllEmpresa);

        }

        private void sbDepartamento_Click(object sender, EventArgs e)
        {
            FormGridDepartamento form = new FormGridDepartamento();
            form.cwTabela = "Departamento";
            form.cwId = (int)cbIdDepartamento.EditValue;
            GridSelecao(form, cbIdDepartamento, bllDepartamento);
        }

        private void sbFuncionario_Click(object sender, EventArgs e)
        {
            FormGridFuncionario form = new FormGridFuncionario();
            form.cwTabela = "Funcionário";
            form.cwId = (int)cbIdFuncionario.EditValue;
            GridSelecao(form, cbIdFuncionario, bllFuncionario);
        }

        
        private void sbCaminho_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.InitialDirectory = @"Meu computador";
                saveFileDialog1.Filter = "Arquivos de Texto(*.txt)|*.txt";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtCaminho.Text = saveFileDialog1.FileName;
                }
                saveFileDialog1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void cbIdEmpresa_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                sbEmpresa.PerformClick();
            }
        }

        private void cbIdDepartamento_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                sbDepartamento.PerformClick();
            }
        }

        private void cbIdFuncionario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                sbFuncionario.PerformClick();
            }
        }

        private void txtCaminho_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                sbCaminho.PerformClick();
            }
        }
    }
}
