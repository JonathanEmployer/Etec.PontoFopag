using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace Modelo.Relatorios
{
    public class ParametrosSubReportView : IParametrosReportView
    {
        /// <summary>
        /// Nome do RDLC do subreport a ser utilizado para gerar o relatório em PDF
        /// </summary>
        public string ReportRdlcName { get; set; }

        public string ReportName { get; set; }
        /// <summary>
        /// Nome do DataSouce do subreport a ser utilizado para gerar o relatório em PDF
        /// </summary>
        public string DataSourceName { get; set; }
        /// <summary>
        /// DataTable do subreport com os dados para geração do relatório
        /// </summary>
        public DataTable DataTable { get; set; }
    }
}
