using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TopPonto
{
    public partial class FormImportacaoTopPonto : Form
    {
        ImportacaoTopPonto importacao;
        Modelo.Parametros objParametro = null;
        BLL.Parametros bllParametros;
        BLL.TipoBilhetes bllBilhetes;

        public int step { get; set; }

        public FormImportacaoTopPonto()
        {
            InitializeComponent();
            bllParametros = new BLL.Parametros();
            bllBilhetes = new BLL.TipoBilhetes();
            chkTodos.Checked = true;
            txtDataFinal.Visible = false;
            labelControl3.Visible = false;
            this.Name = "FormImportacaoTopPonto";
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void sbImportar_Click(object sender, EventArgs e)
        {
            if (ValidaImportacao())
            {
                if (MessageBox.Show("Deseja importar os cadastros selecionados?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    gcCadastros.Enabled = false;
                    gcParametros.Enabled = false;
                    sbImportar.Enabled = false;
                    sbFechar.Enabled = false;
                    sbAjuda.Enabled = false;

                    Application.DoEvents();
                    ImportarCadastros();
                    MessageBox.Show("Processo de Importação Finalizado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    gcCadastros.Enabled = true;
                    gcParametros.Enabled = true;
                    sbImportar.Enabled = true;
                    sbFechar.Enabled = true;
                    this.Close();
                }
            }
        }

        private bool ValidaImportacao()
        {
            if (radioGroup1.SelectedIndex < 0 || radioGroup1.SelectedIndex > 2)
            {
                MessageBox.Show("Não foi selecionado uma versão do TopPonto.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (txtArquivo.Text.Length == 0)
            {
                MessageBox.Show("Não foi selecionado o arquivo mdb.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            if (ObterSteps() == 0)
            {
                MessageBox.Show("Não foi selecionado nenhum cadastro para importação.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            objParametro = bllParametros.LoadPrimeiro();
            if (objParametro.Id == 0)
            {
                MessageBox.Show("Antes de importar a base deve ser cadastrado um parâmetro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (bllBilhetes.ContaNumRegistros() <= 0)
            {
                MessageBox.Show("Antes de importar a base, deve ser cadastrado pelo menos um tipo de bilhete", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (radioGroup1.SelectedIndex == 2 && ((txtDataFinal.DateTime == new DateTime() || txtDataFinal.EditValue == null) || (dtMarcacao.DateTime == new DateTime() || dtMarcacao.EditValue == null))
                && chkMarcacao.Checked)
            {
                MessageBox.Show("Antes de importar a base, deve-se informar as datas para importação.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            return true;
        }

        private void ImportarCadastros()
        {
            pbGeral.Properties.Step = 1;
            pbGeral.Properties.PercentView = true;
            pbGeral.Properties.Minimum = 0;
            pbGeral.Properties.Maximum = ObterSteps();

            if (radioGroup1.SelectedIndex == 0)
                importacao = new ImportacaoTopPonto(1, txtArquivo.Text);
            else if (radioGroup1.SelectedIndex == 1)
                importacao = new ImportacaoTopPonto(2, txtArquivo.Text);
            else
                importacao = new ImportacaoTopPonto(3, txtArquivo.Text);

            if (chkDepartamento.Checked)
            {
                importacao.Departamento(pbTabela);
                AtualizaProgressBar();
            }

            if (chkFuncao.Checked)
            {
                importacao.Funcao(pbTabela);
                AtualizaProgressBar();
            }

            if (chkFeriado.Checked)
            {
                importacao.Feriado(pbTabela);
                AtualizaProgressBar();
            }

            if (chkOcorrencia.Checked)
            {
                importacao.Ocorrencia(pbTabela);
                AtualizaProgressBar();
            }

            if (chkHorario.Checked)
            {
                importacao.Horario(pbTabela);
                AtualizaProgressBar();
            }

            if (chkFuncionario.Checked)
            {
                importacao.Funcionario(pbTabela);
                AtualizaProgressBar();
            }

            if (chkAfastamentos.Checked)
            {
                importacao.Afastamento(pbTabela);
                AtualizaProgressBar();
            }

            if (chkBancoHoras.Checked)
            {
                importacao.BancoHoras(pbTabela);
                AtualizaProgressBar();
            }

            if (chkMarcacao.Checked)
            {
                importacao.Marcacao(pbTabela, dtMarcacao.DateTime, txtDataFinal.DateTime, chbOrdemBilhetes.Checked);
                AtualizaProgressBar();
            }

            if (chkFechamento.Checked)
            {
                importacao.FechamentoBH(pbTabela);
                AtualizaProgressBar();
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

        private void chkTodos_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaCadImportacao();
        }

        private void HabilitaCadImportacao()
        {
            chkDepartamento.Checked = chkTodos.Checked;
            chkFuncao.Checked = chkTodos.Checked;
            chkFeriado.Checked = chkTodos.Checked;
            chkOcorrencia.Checked = chkTodos.Checked;
            chkHorario.Checked = chkTodos.Checked;
            chkFuncionario.Checked = chkTodos.Checked;
            chkAfastamentos.Checked = chkTodos.Checked;
            chkBancoHoras.Checked = chkTodos.Checked;
            chkFechamento.Checked = chkTodos.Checked;
            chkMarcacao.Checked = chkTodos.Checked;

            AtualizaRegraTopPonto34();
        }

        private void chkMarcacao_CheckedChanged(object sender, EventArgs e)
        {
            dtMarcacao.Enabled = chkMarcacao.Checked;
            if (radioGroup1.SelectedIndex == 1)
                chbOrdemBilhetes.Enabled = chkMarcacao.Checked;
            else
                chbOrdemBilhetes.Enabled = false;
        }        

        private void AtualizaRegraTopPonto34()
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                chkFuncao.Checked = false;
                chkOcorrencia.Checked = false;
                chkBancoHoras.Checked = false;
                chkFechamento.Checked = false;
                chkFuncao.Enabled = false;
                chkOcorrencia.Enabled = false;
                chkBancoHoras.Enabled = false;
                chkFechamento.Enabled = false;
                chbOrdemBilhetes.Enabled = false;
            }
            else
            {
                chkFuncao.Enabled = true;
                chkOcorrencia.Enabled = true;
                chkBancoHoras.Enabled = true;
                chkFechamento.Enabled = true;
                chbOrdemBilhetes.Enabled = true;
            }
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
                openFileDialog1.Filter = "*.mdb|*.mdb";

                openFileDialog1.ShowDialog();

                txtArquivo.EditValue = openFileDialog1.FileName;

                openFileDialog1.Dispose();
            }
            catch (Exception)
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
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, "Importação TopPonto");
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

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (radioGroup1.SelectedIndex)
            {
                case 0:
                    AtualizaRegraTopPonto34();
                    txtDataFinal.Visible = false;
                    labelControl3.Visible = false;
                    break;
                case 1:
                    HabilitaCadImportacao();
                    txtDataFinal.Visible = false;
                    labelControl3.Visible = false;
                    break;
                case 2:
                    HabilitaCadImportacao();
                    txtDataFinal.Visible = true;
                    labelControl3.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void txtArquivo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                BotaoDiretorio.PerformClick();
            }
        }
    }
}