using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace cwkWebAPIPontoWeb.Models
{
    public class AtestadoTecnicoViewModel
    {
        public AtestadoTecnicoViewModel()
        {
        }
        private string m_NomeEmpresaDest;

        public string NomeEmpresaDest
        {
            get { return m_NomeEmpresaDest; }
            set { m_NomeEmpresaDest = value; }
        }
        private string m_RazSocEmpresaDest;

        public string RazSocEmpresaDest
        {
            get { return m_RazSocEmpresaDest; }
            set { m_RazSocEmpresaDest = value; }
        }

        private string m_CnpjEmpresaDest;

        public string CnpjEmpresaDest
        {
            get { return m_CnpjEmpresaDest; }
            set { m_CnpjEmpresaDest = value; }
        }

        private string m_NomeRespLegal;

        public string NomeRespLegal
        {
            get { return m_NomeRespLegal; }
            set { m_NomeRespLegal = value; }
        }

        private string m_CpfRespLegal;

        public string CpfRespLegal
        {
            get { return m_CpfRespLegal; }
            set { m_CpfRespLegal = value; }
        }

        private string m_NomeRespTecnico;

        public string NomeRespTecnico
        {
            get { return m_NomeRespTecnico; }
            set { m_NomeRespTecnico = value; }
        }

        private string m_CpfRespTecnico;

        public string CpfRespTecnico
        {
            get { return m_CpfRespTecnico; }
            set { m_CpfRespTecnico = value; }
        }

        public string CaminhoRelatorio { get; set; }
        private string mimeType;

        public byte[] RenderReport()
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = this.CaminhoRelatorio;
            ReportParameter p1 = new ReportParameter("NomeEmpresaDest", m_NomeEmpresaDest);
            ReportParameter p2 = new ReportParameter("RazSocEmpresaDest", m_RazSocEmpresaDest);
            ReportParameter p3 = new ReportParameter("CnpjEmpresaDest", m_CnpjEmpresaDest);
            ReportParameter p4 = new ReportParameter("NomeRespLegal", m_NomeRespLegal);
            ReportParameter p5 = new ReportParameter("CpfRespLegal", m_CpfRespLegal);
            ReportParameter p6 = new ReportParameter("NomeRespTecnico", m_NomeRespTecnico);
            ReportParameter p7 = new ReportParameter("CpfRespTenico", m_CpfRespTecnico);
            localReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7 });

            string encoding;
            string fileNameExtension;
            string deviceInfo =
                "<DeviceInfo>" +
                "   <OutputFormat>pdf</OutputFormat>" +
                "   <PageWidth>8.3in</PageWidth>" +
                "   <PageHeight>11.7in</PageHeight>" +
                "   <MarginTop>1in</MarginTop>" +
                "   <MarginLeft>1in</MarginLeft>" +
                "   <MarginRight>1in</MarginRight>" +
                "   <MarginBottom>1in</MarginBottom>" +
                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = localReport.Render(
                "pdf",
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return renderedBytes;
        }
    }
}