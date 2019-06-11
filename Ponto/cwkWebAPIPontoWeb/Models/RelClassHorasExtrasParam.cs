using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Parâmetros de carga do relatório de classificação de horas extras.
    /// </summary>
    public class RelClassHorasExtrasParam
    {
        public List<string> CPFs { get; set; }
        public string InicioPeriodo { get; set; }
        public string FimPeriodo { get; set; }
        /// <summary>
        /// 0 - Todas; 1 - Classificadas; 2 - Não classificadas.
        /// </summary>
        public int TipoSelecao { get; set; }
        /// <summary>
        /// Formato de saída. Excel ou PDF.
        /// </summary>
        public string Formato { get; set; }
    }
}