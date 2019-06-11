using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;

namespace UI.Relatorios
{
    public partial class FormRelatorioEspelho : Form
    {
        private string nomerel;
        private List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
        private DataTable Dt, DtJornada;
        private string ds, dsJornada;

        public FormRelatorioEspelho(string pNomeRel, string pDs, DataTable pDt, string pDsJornada, DataTable pDtJornada, List<Microsoft.Reporting.WinForms.ReportParameter> pParametros)
        {
            InitializeComponent();
            nomerel = pNomeRel;
            parametros = pParametros;
            ds = pDs;
            Dt = pDt;
            dsJornada = pDsJornada;
            DtJornada = pDtJornada;
        }

        void reportViewer1_SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource(dsJornada, DtJornada));
        }

        private void FormRelatorioBase_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.MinimizeBox = false;

            Assembly assembly = Assembly.LoadFrom("REL.dll");
            Stream stream = assembly.GetManifestResourceStream("REL.Relatorios." + nomerel);
            this.reportViewer1.LocalReport.LoadReportDefinition(stream);
            this.reportViewer1.LocalReport.SetParameters(parametros);

            ReportDataSource myReportDataSource = new ReportDataSource(ds, Dt);
            this.reportViewer1.LocalReport.DataSources.Add(myReportDataSource);

            this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(reportViewer1_SubreportProcessingEventHandler);
            Assembly assemblySub = Assembly.LoadFrom("REL.dll");
            Stream streamSub = assemblySub.GetManifestResourceStream("REL.Relatorios.rptJornadaEspelho.rdlc");
            this.reportViewer1.LocalReport.LoadSubreportDefinition("rptJornadaEspelho", streamSub);  

            this.reportViewer1.ShowProgress = true;
            this.reportViewer1.RefreshReport();
        }
    }
}
