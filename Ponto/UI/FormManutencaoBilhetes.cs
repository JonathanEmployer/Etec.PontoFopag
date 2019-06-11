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
    public partial class FormManutencaoBilhetes : Form
    {
        private Modelo.Marcacao objMarcacao { get; set; }
        private Modelo.BilhetesImp objBilheteImp { get; set; }
        private BLL.BilhetesImp bllBilhetesImp;

        public FormManutencaoBilhetes(Modelo.Marcacao pMarcacao)
        {
            InitializeComponent();
            bllBilhetesImp = new BLL.BilhetesImp();
            this.Name = "FormManutencaoBilhetes";
            LimpaCampos();

            objMarcacao = pMarcacao;

            CarregaBilhetes();
        }
            
        private void AlteraVisivel(bool visivel)
        {
            lblManutencao.Visible = visivel;
            cbManutencao.Visible = visivel;
            sbGravar.Visible = visivel;
            sbCancelar.Visible = visivel;
            lblAtencao1.Visible = visivel;
            lblAtencao2.Visible = visivel;
            lblAtencao3.Visible = visivel;
            gridControl1.Enabled = !visivel;
        }

        private Int32 RegistroSelecionado()
        {
            Int32 seq;
            try
            {
                seq = (int)bandedGridView1.GetFocusedRowCellValue("Id");
            }
            catch (Exception)
            {
                seq = 0;
            }
            return seq;
        }

        private void sbAlterar_Click(object sender, EventArgs e)
        {
            int id = RegistroSelecionado();
            if (id > 0)
            {
                AlteraVisivel(true);

                objBilheteImp = bllBilhetesImp.LoadObject(id);

                txtDataBilhete.DateTime = objBilheteImp.Data;
                txtHoraBilhete.EditValue = objBilheteImp.Hora;
                txtDataMarcacao.DateTime = objBilheteImp.Mar_data.Value;
                txtHoraMarcacao.EditValue = objBilheteImp.Mar_hora;
                lblRelBilhete.Text = objBilheteImp.Relogio;
                lblRelMarcacao.Text = objBilheteImp.Mar_relogio;
            }
            else
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
        }

        private void sbGravar_Click(object sender, EventArgs e)
        {
            FormProgressBar2 pb = new FormProgressBar2();
            try
            {
                AlteraVisivel(false);
                pb.Show();
                bllBilhetesImp.ObjProgressBar = pb.ObjProgressBar;
                if (bllBilhetesImp.ManutencaoBilhete(objMarcacao, objBilheteImp, cbManutencao.SelectedIndex))
                {
                    LimpaCampos();
                    CarregaBilhetes();
                }
                else
                {
                    MessageBox.Show("Não é possível realizar esta alteração.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                pb.Close();
            }
        }

        private void CarregaBilhetes()
        {
            try
            {
                gridControl1.DataSource = bllBilhetesImp.LoadManutencaoBilhetes(objMarcacao.Dscodigo, objMarcacao.Data, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (ex.InnerException is SystemException)
                {
                    Application.Exit();
                }
            }
        }

        private void LimpaCampos()
        {
            txtDataBilhete.EditValue = null;
            txtHoraBilhete.EditValue = "--:--";
            txtDataMarcacao.EditValue = null;
            txtHoraMarcacao.EditValue = "--:--";
            lblRelBilhete.Text = "";
            lblRelMarcacao.Text = "";
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            AlteraVisivel(false);

            LimpaCampos();
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            sbAlterar_Click(sender, e);
        }

        private void FormManutencaoBilhetes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    sbFechar_Click(sender, e);
                    break;
                case Keys.F1:
                    sbAjuda_Click(sender, e);
                    break;
            }
        }

        private void sbAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }
    }
}
