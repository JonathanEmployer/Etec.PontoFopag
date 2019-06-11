using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Modelo;
using System.Xml;
using System.IO;

namespace REL
{
    public partial class FuncionarioHistorico : REL.FormBaseGridFiltro1
    {
        private BLL.FuncionarioHistorico bllFuncionarioHistorico = new BLL.FuncionarioHistorico();

        public FuncionarioHistorico()
        {
            InitializeComponent();
        }

        protected override void FormBase_Load(object sender, EventArgs e)
        {
            txtPeriodoI.EditValue = null;
            txtPeriodoF.EditValue = null;
            base.FormBase_Load(sender, e);
            Carrega();
            rgTipo.SelectedIndex = -1;
            setaNomeArquivo(this.Name);
            LeXML();
            
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                base.btOk_Click(sender, e);
                Dt = bllFuncionarioHistorico.LoadRelatorio(txtPeriodoI.DateTime, txtPeriodoF.DateTime, rgTipo.SelectedIndex, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios());
                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
                parametros.Add(p1);
                nomerel = "rptHistorico.rdlc";
                ds = "dsHistorico_DataTable1";
                FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Histórico Funcionário";
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

