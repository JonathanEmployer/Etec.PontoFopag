using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace Modelo.Relatorios
{
    public class ParametrosReportView : ParametrosReportBase, IParametrosReportView
    {
        /// <summary>
        /// Nome do RDLC a ser utilizado para gerar o relatório em PDF
        /// </summary>
        public string ReportRdlcName { get; set; }
        /// <summary>
        /// Nome do DataSouce a ser utilizado para gerar o relatório em PDF
        /// </summary>
        public string DataSourceName { get; set; }
        /// <summary>
        /// DataTable com os dados para geração do relatório
        /// </summary>
        public DataTable DataTable { get; set; }
        /// <summary>
        /// Parâmetros do relatório (RDLC)
        /// </summary>
        public List<ReportParameter> ReportParameter { get; set; }
        /// <summary>
        /// Dados para o subreport (informar quando houver, exemplo em BLL.Relatorios.V2.RelatorioEspelhoBLL)
        /// </summary>
        public ParametrosSubReportView ParametrosSubReportView { get; set; }
    }
}
