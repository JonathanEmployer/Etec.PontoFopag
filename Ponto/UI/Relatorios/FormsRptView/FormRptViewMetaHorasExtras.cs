using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Reflection;
using System.IO;

namespace UI.Relatorios.FormsRptView
{
    public partial class FormRptViewMetaHorasExtras : Form
    {
        private string nomerel;
        private List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
        Dictionary<string, DataTable> source;
        private DataTable dataTableSubRelatorio = new DataTable();
        public UI.Relatorios.Base.FormBaseSemEmpresa formParaClose;
        private DataTable DtSub;

        public FormRptViewMetaHorasExtras(string pNomeRel, string pDS, DataTable pDt, List<Microsoft.Reporting.WinForms.ReportParameter> pParametros, DataTable dtSub)
        {
            InitializeComponent();
            nomerel = pNomeRel;
            parametros = pParametros;
            source = new Dictionary<string, DataTable>();
            source.Add(pDS, pDt);
            DtSub = dtSub;
        }

        private void FormRelatorioBase_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.MinimizeBox = false;

            Assembly assembly = Assembly.LoadFrom("REL.dll");
            Stream stream = assembly.GetManifestResourceStream("REL.Relatorios." + nomerel);
            this.reportViewer1.LocalReport.LoadReportDefinition(stream);                      
            this.reportViewer1.LocalReport.SetParameters(parametros);

           

            ReportDataSource myReportDataSource;// = new ReportDataSource(ds, Dt);
            foreach (var item in source)
            {
                myReportDataSource = new ReportDataSource(item.Key, item.Value);
                this.reportViewer1.LocalReport.DataSources.Add(myReportDataSource);
            }

            this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRelCartaoPontoIndividualPercBH);
            Assembly assemblySub = Assembly.LoadFrom("REL.dll");
            Stream streamSub = assemblySub.GetManifestResourceStream("REL.Relatorios.rptHExtraMetaChart.rdlc");
            this.reportViewer1.LocalReport.LoadSubreportDefinition("rptHExtraMetaChart", streamSub);  

            this.reportViewer1.ShowProgress = true;
            this.reportViewer1.RefreshReport();
        }

        public void RenderizaSubRelCartaoPontoIndividualPercBH(object sender, SubreportProcessingEventArgs e)
        {
            string dsSub = "DataSetHExtraMetaChart";
            int _idDepart = Convert.ToInt32(e.Parameters["idDepto"].Values[0]);

            string _sqlWhere = "idDepto = " + _idDepart;

            DataTable DtSubFiltrado = DtSub.Select(_sqlWhere).CopyToDataTable();
            e.DataSources.Add(new ReportDataSource(dsSub, DtSubFiltrado));
        }

        private void FormRelatorioBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formParaClose != null)
            {
                formParaClose.Close();
            }
        }
    }
}
