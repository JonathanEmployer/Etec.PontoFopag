using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using DAL;

namespace REL
{
    public partial class FormRelatorioBase : Form
    {
        private string nomerel;
        private string ds;
        private List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
        private DataTable Dt;

        public FormRelatorioBase(string pNomeRel, string pDS, DataTable pDt, List<Microsoft.Reporting.WinForms.ReportParameter> pParametros)
        {
            InitializeComponent();
            nomerel = pNomeRel;
            ds = pDS;
            Dt = pDt;
            parametros = pParametros;
        }

        private void FormRelatorioBase_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + "\\Relatorios\\" + nomerel;                       

            this.reportViewer1.LocalReport.SetParameters(parametros);

            ReportDataSource myReportDataSource = new ReportDataSource(ds, Dt);
            this.reportViewer1.LocalReport.DataSources.Add(myReportDataSource);
            this.reportViewer1.ShowProgress = true;
            this.reportViewer1.RefreshReport();
        }
    }
}
