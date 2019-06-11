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

namespace UI.Relatorios.Base
{
    public partial class FormRelatorioBase : Form
    {
        private string nomerel;
        private List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
        //private DataTable Dt;
        //private string ds;
        Dictionary<string, DataTable> source;
        private string dataSetSubRelatorio = "";
        private DataTable dataTableSubRelatorio = new DataTable();
        private bool subRelatorio = false;
        public UI.Relatorios.Base.FormBaseSemEmpresa formParaClose;
        private BLL.FechamentoBHDPercentual bllFechamentoBHDPercentual;


        public FormRelatorioBase(string pNomeRel, string pDS, DataTable pDt, List<Microsoft.Reporting.WinForms.ReportParameter> pParametros)
        {
            InitializeComponent();
            nomerel = pNomeRel;
            parametros = pParametros;
            //ds = pDS;
            //Dt = pDt;
            source = new Dictionary<string, DataTable>();
            source.Add(pDS, pDt);
            bllFechamentoBHDPercentual = new BLL.FechamentoBHDPercentual();
        }

        public FormRelatorioBase(string pNomeRel, Dictionary<string, DataTable> pSource, List<Microsoft.Reporting.WinForms.ReportParameter> pParametros)
        {
            InitializeComponent();
            nomerel = pNomeRel;
            parametros = pParametros;
            source = pSource;
        }


        public FormRelatorioBase(string pNomeRel, string pDS, DataTable pDt, List<Microsoft.Reporting.WinForms.ReportParameter> pParametros, 
            string pDsSubRel, DataTable pDtSubRel, bool bSubRelatorio)
        {
            InitializeComponent();
            nomerel = pNomeRel;
            parametros = pParametros;
            source = new Dictionary<string, DataTable>();
            dataSetSubRelatorio = pDsSubRel;
            dataTableSubRelatorio = pDtSubRel;
            subRelatorio = bSubRelatorio;
            source.Add(pDS, pDt);
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

            if (nomerel == "rptCartaoPontoIndividual.rdlc")
            {
                this.reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(RenderizaSubRelCartaoPontoIndividualPercBH);

                Assembly assemblySub = Assembly.LoadFrom("REL.dll");
                Stream streamSub = assemblySub.GetManifestResourceStream("REL.Relatorios.rptCartaoPontoIndividualSubPercBH.rdlc");
                this.reportViewer1.LocalReport.LoadSubreportDefinition("rptCartaoPontoIndividualSubPercBH", streamSub);  
            }

            this.reportViewer1.ShowProgress = true;
            this.reportViewer1.RefreshReport();
        }

        public void RenderizaSubRelCartaoPontoIndividualPercBH(object sender, SubreportProcessingEventArgs e)
        {
            DateTime dtfim = Convert.ToDateTime(parametros[1].Values[0]);
            string dsSub = "dsCartaoPonto_PercentualBH";
            int _idFunc = Convert.ToInt32(e.Parameters["idfuncionario"].Values[0]);
            DataTable DtSub = bllFechamentoBHDPercentual.GetBancoHorasPercentual(null, dtfim, _idFunc, 2);
            e.DataSources.Add(new ReportDataSource(dsSub, DtSub));
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
