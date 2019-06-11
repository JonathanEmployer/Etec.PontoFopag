using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace Modelo.Relatorios
{
    public class ParametrosReportHTML : ParametrosReportBase, IParametrosReportHTML
    {
        public string ResourceName { get; set; }

        public object Dados { get; set; }
    }
}
