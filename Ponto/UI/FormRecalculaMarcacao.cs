using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormRecalculaMarcacao : Form
    {
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcionario bllFuncionario;
        private BLL.Funcao bllFuncao;
        private BLL.Marcacao bllMarcacao;

        public List<string> TelasAbertas { get; set; }

        public FormRecalculaMarcacao()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncao = new BLL.Funcao();
            bllFuncionario = new BLL.Funcionario();
            bllMarcacao = new BLL.Marcacao();
            this.Name = "FormRecalculaMarcacao";
            rgTipo.SelectedIndex = -1;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btIdentificacao_Click(object sender, EventArgs e)
        {
            if (rgTipo.EditValue != null)
            {
                switch ((int)rgTipo.EditValue)
                {
                    case 0:
                        FormGridEmpresa formEmp = new FormGridEmpresa();
                        formEmp.cwTabela = "Empresa";
                        formEmp.cwId = (int)cbIdentificacao.EditValue;
                        GridSelecao(formEmp, cbIdentificacao, bllEmpresa);
                        break;
                    case 1:
                        FormGridDepartamento formDep = new FormGridDepartamento();
                        formDep.cwTabela = "Departamento";
                        formDep.cwId = (int)cbIdentificacao.EditValue;
                        GridSelecao(formDep, cbIdentificacao, bllDepartamento);
                        break;
                    case 2:
                        FormGridFuncionario formFunc = new FormGridFuncionario();
                        formFunc.cwTabela = "Funcionário";
                        formFunc.cwId = (int)cbIdentificacao.EditValue;
                        GridSelecao(formFunc, cbIdentificacao, bllFuncionario);
                        break;
                    case 3:
                        FormGridFuncao formFuncao = new FormGridFuncao();
                        formFuncao.cwTabela = "Função";
                        formFuncao.cwId = (int)cbIdentificacao.EditValue;
                        GridSelecao(formFuncao, cbIdentificacao, bllFuncao);
                        break;
                    default:
                        break;
                }
            }
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIdentificacao.Properties.DataSource = null;
            switch ((int)rgTipo.EditValue)
            {
                case -1:
                    cbIdentificacao.Properties.DataSource = null;
                    break;
                case 0: // Empresa
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllEmpresa.GetAll();
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    break;
                case 1: // Departamento
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllDepartamento.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = true;
                    break;
                case 2: // Funcionário
                    cbIdentificacao.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    break;
                case 3: // Função
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllFuncao.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    break;
            }
        }

        private void GridSelecao<T>(Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
        {
            //pGrid.cwSelecionar = true;
            //pGrid.ShowDialog();
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

        private void btRecalcula_Click(object sender, EventArgs e)
        {
            if (ValidaCampos())
            {
                //System.Diagnostics.Stopwatch tempo = new System.Diagnostics.Stopwatch();
                //tempo.Start();

                FormProgressBar2 formPBRecalcula = new FormProgressBar2();
                try
                {
                    btRecalcula.Enabled = false;
                    btCancel.Enabled = false;
                    formPBRecalcula.Show(this);
                    formPBRecalcula.SetaMensagem("Recalculando Marcações...");

                    bllMarcacao.RecalculaMarcacao((int)rgTipo.EditValue, Convert.ToInt32(cbIdentificacao.EditValue), dateInicial.DateTime, dateFinal.DateTime, formPBRecalcula.ObjProgressBar);

                    formPBRecalcula.Close();
                    MessageBox.Show("Marcações recalculadas com sucesso.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    formPBRecalcula.Close();
                    MessageBox.Show("Erro ao realizar recalculo: " + ex.Message);
                }
                finally
                {
                    btRecalcula.Enabled = true;
                    btCancel.Enabled = true;
                    if (!formPBRecalcula.IsDisposed)
                    {
                        formPBRecalcula.Dispose();
                    }
                }

                //tempo.Stop();
            }
            else
            {
                MessageBox.Show("Preencha os campos corretamente.");
            }
        }

        private bool ValidaCampos()
        {
            bool ret = true;

            if (dateInicial.DateTime != new DateTime())
                dxErrorProvider1.SetError(dateInicial, "");
            else
            {
                dxErrorProvider1.SetError(dateInicial, "Selecione a Data Inicial.");
                ret = false;
            }

            if (dateFinal.DateTime != new DateTime())
                dxErrorProvider1.SetError(dateFinal, "");
            else
            {
                dxErrorProvider1.SetError(dateFinal, "Selecione a Data Inicial.");
                ret = false;
            }

            if (rgTipo.SelectedIndex != -1)
                dxErrorProvider1.SetError(rgTipo, "");
            else
            {
                dxErrorProvider1.SetError(rgTipo, "Selecione o tipo de recalculo a ser realizado.");
                ret = false;
            }


            if ((int)cbIdentificacao.EditValue != 0)
                dxErrorProvider1.SetError(cbIdentificacao, "");
            else
            {
                dxErrorProvider1.SetError(cbIdentificacao, "A identificação.");
                ret = false;
            }

            return ret;
        }

        private void FormRecalculaMarcacao_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void FormRecalculaMarcacao_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    btCancel.Focus();
                    btCancel_Click(sender, e);
                    break;
                case Keys.Enter:
                    if (btRecalcula.Enabled == true)
                    {
                        btRecalcula.Focus();
                        btRecalcula_Click(sender, e);
                    }
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, "Recalcula Marcações");
                    break;
                case Keys.F1:
                    btAjuda_Click(sender, e);
                    break;
            }
        }

        private void btAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }
    }
}
