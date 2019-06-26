using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Parâmetros para retorno dos dados para relatório de afastamentos
    /// </summary>
    public class RelatorioAfastamentoParam : ParametrosRelatorioBase
    {
        /// <summary>
        /// Considerar ou não afastamentos com parâmetro para relatório de absenteismo (0 - apenas o que não esta marcado, 1 - apenas o que esta marcado, 2 - Ambos)
        /// </summary>
        public Int16 Absenteismo { get; set; }
        /// <summary>
        /// Considerar afastamentos do tipo abonado
        /// </summary>
        public bool ConsiderarAbonado { get; set; }
        /// <summary>
        /// Considerar afastamentos do tipo parcial
        /// </summary>
        public bool considerarParcial { get; set; }
        /// <summary>
        /// Considerar afastamentos do tipo Sem Calculo
        /// </summary>
        public bool considerarSemCalculo { get; set; }
        /// <summary>
        /// Considerar afastamentos do tipo Suspensão
        /// </summary>
        public bool considerarSuspensao { get; set; }
        /// <summary>
        /// Considerar afastamentos do tipo Sem Abono
        /// </summary>
        public bool considerarSemAbono { get; set; }
    }
}