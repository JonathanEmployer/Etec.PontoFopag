using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace REL
{
    public partial class BancoHorasResumo : REL.FormBaseGridFiltro1
    {
        private BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras();

        public BancoHorasResumo()
        {
            InitializeComponent();
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

        protected override void btOk_Click(object sender, EventArgs e)
        {
            string dataI;
            dataI = txtPeriodoI.DateTime.ToShortDateString();
            string dataF;
            dataF = txtPeriodoF.DateTime.ToShortDateString();
            try
            {
                base.btOk_Click(sender, e);
                Dt = bllBancoHoras.LoadRelatorio(txtPeriodoI.DateTime, txtPeriodoF.DateTime, rgTipo.SelectedIndex, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios());
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                parametros.Add(p1);
                Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("dataInicial", dataI);
                parametros.Add(p2);
                Microsoft.Reporting.WinForms.ReportParameter p3 = new Microsoft.Reporting.WinForms.ReportParameter("dataFinal", dataF);
                parametros.Add(p3);
                nomerel = "rptBancoHorasResumo.rdlc";
                ds = "dsBancoHorasResumo_DataTable1";
                FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório Resumida do Banco de Horas";
                form.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
        }
    }
}
