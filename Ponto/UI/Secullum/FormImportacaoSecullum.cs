using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UI;

namespace Secullum
{
    public partial class FormImportacaoSecullum : Form
    {
        IList<Modelo.pxyLogErroImportacao> logsErro = new List<Modelo.pxyLogErroImportacao>();

        ImportacaoSecullum importacao;
        Modelo.Parametros objParametro = null;
        private BLL.Parametros bllParametro;
        private BLL.TipoBilhetes bllBilhetes;

        private BLL.Empresa bllEmpresa;
        private BLL.Horario bllHorario;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcao bllFuncao;
        private BLL.Funcionario bllFuncionario;

        public int step { get; set; }

        public FormImportacaoSecullum()
        {
            InitializeComponent();
            
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncionario = new BLL.Funcionario();
            bllFuncao = new BLL.Funcao();
            bllHorario = new BLL.Horario();
            bllParametro = new BLL.Parametros();
            bllBilhetes = new BLL.TipoBilhetes();

            txtDataFinal.Visible = false;
            labelControl3.Visible = false;
            dtMarcacao.Visible = false;
            labelControl1.Visible = false;
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();
            this.Name = "FormImportacaoSecullum";
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sbImportar_Click(object sender, EventArgs e)
        {
            if (!ValidaImportacao())
            {
                if (MessageBox.Show("Deseja importar os cadastros selecionados?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    gcCadastros.Enabled = false;
                    sbImportar.Enabled = false;
                    sbFechar.Enabled = false;
                    sbAjuda.Enabled = false;

                    Application.DoEvents();
                    ImportarCadastros();
                    
                    gcCadastros.Enabled = true;
                    sbImportar.Enabled = true;
                    sbFechar.Enabled = true;
                    this.Close();

                    if (logsErro.Count > 0)
                    {
                        FormLogErroImportacao form = new FormLogErroImportacao(logsErro);
                        form.ShowDialog();
                    }
                    else
                        MessageBox.Show("Processo de Importação Finalizado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private bool ValidaImportacao()
        {
            dxErrorProvider1.ClearErrors();
            bool temErro = false;
            if (!((int)cbIdEmpresa.EditValue > 0))
            {
                dxErrorProvider1.SetError(cbIdEmpresa, "Selecione uma empresa.");
                temErro = true;
            }
            if (!((int)cbIdTurnoNormal.EditValue > 0))
            {
                dxErrorProvider1.SetError(cbIdTurnoNormal, "Não foi selecionado nenhum horário.");
                temErro = true;
            }
            if (txtArquivo.Text.Length == 0)
            {
                dxErrorProvider1.SetError(txtArquivo,"Não foi selecionado o arquivo mdb.");
                temErro = true;
            }

            if (ObterSteps() == 0)
            {
                dxErrorProvider1.SetError(chkDepartamento, "Não foi selecionado nenhum cadastro para importação.");
                temErro = true;
            }

            objParametro = bllParametro.LoadPrimeiro();
            if (objParametro.Id == 0)
            {
                MessageBox.Show("Antes de importar a base deve ser cadastrado um parâmetro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                temErro = true;                
            }
            if (bllBilhetes.ContaNumRegistros() <= 0)
            {
                MessageBox.Show("Antes de importar a base, deve ser cadastrado pelo menos um tipo de bilhete", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                temErro = true; 
            }
            return temErro;
        }

        private void ImportarCadastros()
        {
            pbGeral.Properties.Step = 1;
            pbGeral.Properties.PercentView = true;
            pbGeral.Properties.Minimum = 0;
            pbGeral.Properties.Maximum = ObterSteps();

            importacao = new ImportacaoSecullum(txtArquivo.Text);
            
            logsErro = new List<Modelo.pxyLogErroImportacao>();

            if (importacao.ValidaEmpresa((int)cbIdEmpresa.EditValue))
            {
                if (chkDepartamento.Checked)
                {
                    importacao.Departamento(pbTabela, (int)cbIdEmpresa.EditValue, ref logsErro);
                    AtualizaProgressBar();
                }

                if (chkFuncao.Checked)
                {
                    if (logsErro.Count == 0)
                        importacao.Funcao(pbTabela, ref logsErro);
                    AtualizaProgressBar();
                }

                if (chkFuncionario.Checked)
                {
                    if (logsErro.Count == 0)
                        importacao.Funcionario(pbTabela, (int)cbIdEmpresa.EditValue, (int)cbIdTurnoNormal.EditValue, ref logsErro);
                    AtualizaProgressBar();
                }
            }
            else
            {
                MessageBox.Show("Os dados da empresa para importação não coincidem com os dados da licença", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AtualizaProgressBar()
        {
            pbGeral.PerformStep();
            pbGeral.Update();
            Application.DoEvents();
        }

        private int ObterSteps()
        {
            step = 0;
            foreach (Control ctr in xtraTabPage2.Controls)
            {
                if (ctr.GetType() == typeof(DevExpress.XtraEditors.GroupControl) && ctr.Name == "gcCadastros")
                {
                    foreach (Control c in gcCadastros.Controls)
                    {
                        if (c.GetType() == typeof(DevExpress.XtraEditors.CheckEdit) && ((DevExpress.XtraEditors.CheckEdit)c).Checked)
                        {
                            step += 1;
                        }
                    }
                }
            }
            return step;
        }
          

        private void BotaoDiretorio_Click(object sender, EventArgs e)
        {
            AbrirArquivoMdb();
        }

        private void AbrirArquivoMdb()
        {
            try
            {
                openFileDialog1.Title = "Selecione o arquivo";
                //openFileDialog1.InitialDirectory = Application.CommonAppDataPath;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "*.dat|*.dat| *.mdb|*.mdb";

                openFileDialog1.ShowDialog();

                txtArquivo.EditValue = openFileDialog1.FileName;

                openFileDialog1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir diretorio!");
            }
        }

        private void FormImportacaoTopPonto_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela sem salvar as alterações?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        sbFechar.Focus();
                        sbFechar_Click(sender, e);
                    }
                    break;
                case Keys.Enter:
                    sbImportar.Focus();
                    sbImportar_Click(sender, e);
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, "Importação Secullum");
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

        private void rgTipoHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)rgTipoHorario.EditValue == 1 || (int)rgTipoHorario.EditValue == 2)
            {
                cbIdTurnoNormal.Enabled = true;
                sbIdTurnoNormal.Enabled = true;

                cbIdTurnoNormal.Properties.DataSource = bllHorario.GetHorarioTipo((int)rgTipoHorario.EditValue);
                cbIdTurnoNormal.EditValue = 0;
            }
            else
            {
                cbIdTurnoNormal.Enabled = false;
                sbIdTurnoNormal.Enabled = false;
                cbIdTurnoNormal.EditValue = 0;
            }
        }

        private void sbIdTurnoNormal_Click(object sender, EventArgs e)
        {
            if (rgTipoHorario.SelectedIndex == 0)
            {
                FormGridHorario form = new FormGridHorario();
                form.cwTabela = "Horário Normal";
                form.cwId = (int)cbIdTurnoNormal.EditValue;
                GridSelecao(form, cbIdTurnoNormal, bllHorario);
            }

            else if (rgTipoHorario.SelectedIndex == 1)
            {
                FormGridHorarioMovel form = new FormGridHorarioMovel();
                form.cwTabela = "Horário Móvel";
                form.cwId = (int)cbIdTurnoNormal.EditValue;
                GridSelecao(form, cbIdTurnoNormal, bllHorario);
            }
        }

        protected virtual void GridSelecao<T>(UI.Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
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

        private void sbIdEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresas";
            form.cwId = (int)cbIdEmpresa.EditValue;
            GridSelecao(form, cbIdEmpresa, bllEmpresa);
        }
    }
}