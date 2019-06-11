using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Relatorios
{
    public partial class PercentualHExtra: UI.Relatorios.Base.FormBaseGridFiltro1
    {
        private BLL.HorarioPHExtra bllPercentualHExtra;

        public PercentualHExtra()
        {
            InitializeComponent();
            bllPercentualHExtra = new BLL.HorarioPHExtra();
            this.Name = "PercentualHExtra";
            rgTipo.SelectedIndex = -1;
        }

        private void SelecionarFuncionario()
        {
            rgTipo.SelectedIndex = 2;
            SelecionaRegistroPorID("id", objFuncionario.Idempresa, gvEmpresas);
            SelecionaRegistroPorID("id", objFuncionario.Iddepartamento, gvDepartamentos);
            SelecionaRegistroPorID("id", objFuncionario.Id, gvFuncionarios);
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            setaNomeArquivo(this.Name);
            Carrega();
            LeXML();

        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
                {
                    MessageBox.Show("Data inicial não pode ser maior que Data final.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (ValidaCampos())
                    {
                        base.btOk_Click(sender, e);
                        this.Enabled = false;
                        Dt = bllPercentualHExtra.GetPercentualHoraExtra(txtPeriodoI.DateTime, txtPeriodoF.DateTime, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo(), pbRelatorio);
                        parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                        Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", txtPeriodoI.DateTime.ToShortDateString());
                        parametros.Add(p1);
                        Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", txtPeriodoF.DateTime.ToShortDateString());
                        parametros.Add(p2);
                        nomerel = "rptPercentualHExtra.rdlc";
                        ds = "dsPercExtra_PercExtra";
                        UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                        form.Text = "Relatório de Horas Extras por Percentuais";
                        form.Show();
                        this.Enabled = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Preencha os campos corretamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool ValidaCampos()
        {
            bool ret = true;

            if (txtPeriodoI.DateTime == new DateTime() || txtPeriodoI.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoI, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoI, "");
            }

            if (txtPeriodoF.DateTime == new DateTime() || txtPeriodoF.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoF, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoF, "");
            }

            return ret;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }


    }
}
