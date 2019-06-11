using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace Modelo.Relatorios
{
    public class ParametrosReportExcel : ParametrosReportBase, IParametrosReportExcel
    {
        public byte[] RenderedBytes { get; set; }
    }
}
