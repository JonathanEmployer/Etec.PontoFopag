using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutTipoBilhetes : UI.Base.ManutBase
    {
        private BLL.TipoBilhetes bllTipoBilhetes;
        private BLL.REP bllRep;
        private Modelo.TipoBilhetes objTipoBilhetes;

        public FormManutTipoBilhetes()
        {
            InitializeComponent();
            bllRep = new BLL.REP();
            bllTipoBilhetes = new BLL.TipoBilhetes();
            this.Name = "FormManutTipoBilhetes";
            cbIdRep.Properties.DataSource = bllRep.GetAll();
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objTipoBilhetes = new Modelo.TipoBilhetes();
                    objTipoBilhetes.Codigo = bllTipoBilhetes.MaxCodigo();
                    objTipoBilhetes.FormatoBilhete = -1;
                    objTipoBilhetes.Descricao = "";
                    break;
                default:
                    objTipoBilhetes = bllTipoBilhetes.LoadObject(cwID);
                    break;
            }

            if (String.IsNullOrEmpty(objTipoBilhetes.StrLayout))
            {
                objTipoBilhetes.StrLayout = "";
            }

            txtLayout.EditValue = objTipoBilhetes.StrLayout;
            gcLayoutLivre.Enabled = false;
            

            txtCodigo.DataBindings.Add("EditValue", objTipoBilhetes, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objTipoBilhetes, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDiretorio.DataBindings.Add("EditValue", objTipoBilhetes, "Diretorio", true, DataSourceUpdateMode.OnPropertyChanged);
            cbFormatoBilhete.DataBindings.Add("SelectedIndex", objTipoBilhetes, "FormatoBilhete", true, DataSourceUpdateMode.OnPropertyChanged);
            chbBimporta.DataBindings.Add("Checked", objTipoBilhetes, "BImporta", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdRep.DataBindings.Add("EditValue", objTipoBilhetes, "IdRep", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        private void sbIdDiretorio_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(txtDiretorio.Text))
                {
                    openFileDialog1.FileName = txtDiretorio.Text;
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
                openFileDialog1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            objTipoBilhetes.Descricao = objTipoBilhetes.Descricao.TrimEnd();
            base.Salvar();
            return bllTipoBilhetes.Salvar(cwAcao, objTipoBilhetes);
        }

        #region LayoutBilhete

        private void AlteraLayout(char tipo)
        {
            if (tipo != 'S' && objTipoBilhetes.StrLayout.Contains(tipo.ToString()))
            {
                MessageBox.Show("Este campo só pode ser inserido uma vez no layout.");
            }
            else
            {
                switch (tipo)
                {
                    case 'O':
                        bllTipoBilhetes.MontaLayout(ref objTipoBilhetes.StrLayout, tipo, 3);
                        break;
                    case 'A':
                        bllTipoBilhetes.MontaLayout(ref objTipoBilhetes.StrLayout, tipo, 4);
                        break;
                    case 'S':
                        bllTipoBilhetes.MontaLayout(ref objTipoBilhetes.StrLayout, tipo, 1);
                        break;
                    case 'F':
                        FormQtFuncLayout form = new FormQtFuncLayout();
                        form.ShowDialog();
                        if (form.cwOk)
                        {
                            bllTipoBilhetes.MontaLayout(ref objTipoBilhetes.StrLayout, tipo, form.cwQuantidade);
                        }
                        break;
                    default:
                        bllTipoBilhetes.MontaLayout(ref objTipoBilhetes.StrLayout, tipo, 2);
                        break;
                }

                txtLayout.EditValue = objTipoBilhetes.StrLayout;
            }
        }

        private void sbOrdem_Click(object sender, EventArgs e)
        {
            AlteraLayout('O');
        }

        private void sbDia_Click(object sender, EventArgs e)
        {
            AlteraLayout('D');
        }

        private void sbMes_Click(object sender, EventArgs e)
        {
            AlteraLayout('M');
        }

        private void sbAno2_Click(object sender, EventArgs e)
        {
            AlteraLayout('a');
        }

        private void sbAno4_Click(object sender, EventArgs e)
        {
            AlteraLayout('A');
        }

        private void sbHora_Click(object sender, EventArgs e)
        {
            AlteraLayout('h');
        }

        private void sbMinuto_Click(object sender, EventArgs e)
        {
            AlteraLayout('m');
        }

        private void sbFuncionario_Click(object sender, EventArgs e)
        {
            AlteraLayout('F');
        }

        private void sbRelogio_Click(object sender, EventArgs e)
        {
            AlteraLayout('R');
        }

        private void sbSeparador_Click(object sender, EventArgs e)
        {
            AlteraLayout('S');
        }

        private void sbDesfazer_Click(object sender, EventArgs e)
        {
            if (objTipoBilhetes.StrLayout.Length > 0)
            {
                char aux = objTipoBilhetes.StrLayout[objTipoBilhetes.StrLayout.Length - 1];
                if (aux == 'S')
                {
                    objTipoBilhetes.StrLayout = objTipoBilhetes.StrLayout.Remove(objTipoBilhetes.StrLayout.Length - 1);
                }
                else
                {
                    objTipoBilhetes.StrLayout = objTipoBilhetes.StrLayout.Trim(new char[] { aux });
                }

                txtLayout.EditValue = objTipoBilhetes.StrLayout;
            }
        }

        #endregion

        private void cbFormatoBilhete_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFormatoBilhete.SelectedIndex == 2)
            {
                gcLayoutLivre.Enabled = true;
            }
            else
            {
                gcLayoutLivre.Enabled = false;
            }
        }

        private void txtDiretorio_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F5:
                    sbIdDiretorio_Click(sender, e);
                    break;
            }
        }

        private void sbIdRep_Click(object sender, EventArgs e)
        {
            FormGridREP formRep = new FormGridREP();
            formRep.cwTabela = "REP";
            formRep.cwId = (int)cbIdRep.EditValue;
            GridSelecao(formRep, cbIdRep, bllRep);
        }

    }
}
