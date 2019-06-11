using System.Collections.Generic;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace Modelo.Relatorios
{
    public class ParametrosReportBase : IParametrosReportView
    {
        public string NomeArquivo { get; set; }
        public Enumeradores.TipoArquivo TipoArquivo { get; set; }
    }
}
