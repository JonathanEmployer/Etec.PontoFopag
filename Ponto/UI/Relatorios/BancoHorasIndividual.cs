using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios
{
    public partial class BancoHorasIndividual : UI.Relatorios.Base.FormBaseGridFiltro1
    {
        private BLL.BancoHoras bllBancoHoras;

        public BancoHorasIndividual()
        {
            InitializeComponent();
            this.Name = "BancoHorasIndividual";
            bllBancoHoras = new BLL.BancoHoras();
        }

        protected override void FormBase_Load(object sender, EventArgs e)
        {
            base.FormBase_Load(sender, e);
            rgTipo.SelectedIndex = -1;
            txtPeriodoI.EditValue = null;
            txtPeriodoF.EditValue = null;  
            Carrega();          
            setaNomeArquivo(this.Name);
            LeXML();
        }

        protected virtual void tipo()
        {
            if (rgTipo.SelectedIndex == 0)
            {
                Tipo = "Empresa";
            }
            else if (rgTipo.SelectedIndex == 1)
            {
                Tipo = "Departamento";
            }
            else if (rgTipo.SelectedIndex == 2)
            {
                Tipo = "Funcionário";
            }
        }

        private bool ValidaCampos()
        {
            if (txtPeriodoI.DateTime == new DateTime() || txtPeriodoF.DateTime == new DateTime())
            {
                MessageBox.Show("O relatorio deve ter um período inicial e final");
                return false;
            }
            if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
            {
                MessageBox.Show("Período inicial deve ser menor que o final");
                return false;
            }

            return true;
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;
            if (ValidaCampos())
                ChamaRelatorio(sender, e);
            btOk.Enabled = true;
            btCancelar.Enabled = true;
        }

        private void ChamaRelatorio(object sender, EventArgs e)
        {
            FormProgressBar2 pb = new FormProgressBar2();
            this.Enabled = false;
            pb.Show(this);
            try
            {
                string dataI = txtPeriodoI.DateTime.ToShortDateString();
                string dataF = txtPeriodoF.DateTime.ToShortDateString();
                base.btOk_Click(sender, e);

                bllBancoHoras.ObjProgressBar = pb.ObjProgressBar;
                string ids;
                switch (rgTipo.SelectedIndex)
                {
                    case 0:
                        ids = MontaStringEmpresas();
                        break;
                    case 1:
                        ids = MontaStringDepartamentos();
                        break;
                    case 2:
                        ids = MontaStringFuncionarios();
                        break;
                    default:
                        ids = "";
                        break;
                }
                bllBancoHoras.ObjProgressBar = pb.ObjProgressBar;
                Dt = bllBancoHoras.GetRelatorioHorario(txtPeriodoI.DateTime, txtPeriodoF.DateTime, rgTipo.SelectedIndex, ids);
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", dataI);
                parametros.Add(p1);
                Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", dataF);
                parametros.Add(p2);
                nomerel = "rptBancoHorasHorario.rdlc";
                ds = "dsBancoHorasResumo_BancoHorasHorario";
                UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Banco de Horas Individual";
                form.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                pb.Dispose();
            }
            this.Enabled = true;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }
    }
}
