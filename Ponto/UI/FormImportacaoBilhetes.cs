using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace UI
{
    public partial class FormImportacaoBilhetes : Form
    {
        private BLL.Funcionario bllFuncionario;
        private BLL.TipoBilhetes bllTipoBilhetes;
        private BLL.BilhetesImp bllBilhetesImp;
        private BLL.ImportacaoBilhetes bllImportacaoBilhetes;
        private BLL.Marcacao bllMarcacao;
        private BLL.MudancaHorario bllMudancaHorario;
        private BLL.JornadaAlternativa bllJornadaAlternativa;
        private BLL.Parametros bllParametro;
        private BLL.Horario bllHorario;
        private BLL.BancoHoras bllBancoHoras;
        private BLL.Feriado bllFeriado;
        private BLL.Afastamento bllAfastamento;


        private Modelo.TipoBilhetes objTipoBilhete = new Modelo.TipoBilhetes();
        private List<Modelo.TipoBilhetes> listaTipoBilhetes = new List<Modelo.TipoBilhetes>();

        public List<string> TelasAbertas { get; set; }

        public FormImportacaoBilhetes()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            bllTipoBilhetes = new BLL.TipoBilhetes();
            bllBilhetesImp = new BLL.BilhetesImp();
            bllImportacaoBilhetes = new BLL.ImportacaoBilhetes();
            bllMarcacao = new BLL.Marcacao();
            bllMudancaHorario = new BLL.MudancaHorario();
            bllJornadaAlternativa = new BLL.JornadaAlternativa();
            bllParametro = new BLL.Parametros();
            bllHorario = new BLL.Horario();
            bllBancoHoras = new BLL.BancoHoras();
            bllFeriado = new BLL.Feriado();
            bllAfastamento = new BLL.Afastamento();
            this.Name = "FormImportacaoBilhetes";
            openFileDialog1.Multiselect = false;
            cbIdFuncionario.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();

            listaTipoBilhetes = bllTipoBilhetes.getListaImportacao();

            gcTotaisMarcacao.DataSource = listaTipoBilhetes;
        }

        private void sbIdDiretorio_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtDiretorio.Text))
                {
                    openFileDialog1.InitialDirectory = (string)txtDiretorio.EditValue;
                    openFileDialog1.FileName = "";
                }
                else
                {
                    openFileDialog1.FileName = "";
                }
                openFileDialog1.Filter = "Arquivos de Texto(*.txt)|*.txt";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtDiretorio.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void sbIdIdentificacao_Click(object sender, EventArgs e)
        {
            FormGridFuncionario form = new FormGridFuncionario();
            form.cwTabela = "Funcionário";
            form.cwId = Convert.ToInt32(cbIdFuncionario.EditValue);
            GridSelecao(form, cbIdFuncionario, bllFuncionario);
        }

        private void FormImportacaoBilhetes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                    if (sbImportar.Enabled == true)
                    {
                        sbImportar.Focus();
                        sbImportar_Click(sender, e);
                    }
                    break;
                case Keys.Enter:
                    if (sbImportar.Enabled == true)
                    {
                        sbImportar.Focus();
                        sbImportar_Click(sender, e);
                    }
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        sbCancelar.Focus();
                        sbCancelar_Click(sender, e);
                    }
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, "Importação de Bilhetes");
                    break;
                case Keys.F1:
                    sbAjuda_Click(sender, e);
                    break;
            }
        }

        private void sbImportar_Click(object sender, EventArgs e)
        {
            ImportarBilhetes();
        }

        private void ImportarBilhetes()
        {
            this.Enabled = false;
            FormProgressBar2 pb = new FormProgressBar2();
            try
            {
                string func = String.Empty;
                if (cbIdFuncionario.EditValue != null)
                {
                    Modelo.Funcionario objFunc = bllFuncionario.LoadObject(Convert.ToInt32(cbIdFuncionario.EditValue));
                    func = objFunc.Dscodigo;
                }

                if (gvTotaisMarcacao.RowCount > 0)
                {
                    int bilhete = (int)gvTotaisMarcacao.GetFocusedRowCellValue("Codigo");
                    string mensagem = "";
                    DateTime? datai = DTInicial.DateTime <= new DateTime() ? null : (DateTime?)DTInicial.DateTime
                            , dataf = DTFinal.DateTime <= new DateTime() ? null : (DateTime?)DTFinal.DateTime;

                    foreach (Modelo.TipoBilhetes item in listaTipoBilhetes)
                    {
                        if ((item.FormatoBilhete == 3 || item.FormatoBilhete == 4) && item.BImporta && (datai == null || dataf == null))
                        {
                            MessageBox.Show("Para importar um arquivo de bilhetes do tipo AFD ou REP é necessário informar o período de importação.", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Enabled = true;
                            pb.Dispose();
                            return;
                        }
                    }

                    pb.Show(this);
                    bool bErro = false;
                    bool ok = bllImportacaoBilhetes.ImportacaoBilhete(pb.ObjProgressBar, listaTipoBilhetes, txtDiretorio.Text, bilhete, chbMarcacaoIndividual.Checked, func, datai, dataf, out mensagem);
                    openFileDialog1.Dispose();
                    pb.Close();

                    if (MessageBox.Show(mensagem + "\n\nDeseja visualizar o arquivo de log?", "Mensagem", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(Modelo.cwkGlobal.DirApp + "\\logImportacao.txt");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Não foi possível abrir o arquivo de log:\n" + ex);
                        }
                    }
                    this.Close();
                }
                else
                {
                    pb.Close();
                    MessageBox.Show("Para realizar a importação de bilhetes é necessário no mínimo 1 tipo de bilhete.", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                pb.Close();
                MessageBox.Show(ex.Message, "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Enabled = true;
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvTotaisMarcacao_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle >= 0)
            {
                if (gvTotaisMarcacao.GetFocusedRowCellValue("FormatoBilhete").ToString() != "4")
                    txtDiretorio.EditValue = (string)gvTotaisMarcacao.GetFocusedRowCellValue("Diretorio");
                else
                    txtDiretorio.EditValue = String.Empty;
            }
        }

        private void FormImportacaoBilhetes_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void sbAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void chbMarcacaoIndividual_CheckedChanged(object sender, EventArgs e)
        {
            if (chbMarcacaoIndividual.Checked)
            {
                cbIdFuncionario.Enabled = true;
                sbIdIdentificacao.Enabled = true;
            }
            else
            {
                cbIdFuncionario.Enabled = false;
                sbIdIdentificacao.Enabled = false;
            }
        }

        private void txtDiretorio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                sbIdDiretorio.PerformClick();
            }
        }
    }
}
