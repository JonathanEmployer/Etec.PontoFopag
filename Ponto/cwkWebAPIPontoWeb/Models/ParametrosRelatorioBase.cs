using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Parâmetros basicos utilizados nos relatórios
    /// </summary>
    public class ParametrosRelatorioBase
    {
        /// <summary>
        /// Data inicial do período
        /// </summary>
        public string InicioPeriodo { get; set; }
        /// <summary>
        /// Data Finalo do Período
        /// </summary>
        public string FimPeriodo { get; set; }

        /// <summary>
        /// Lista de CPF's e Matriculas dos Funcionários
        /// </summary>
        public List<Modelo.Proxy.PxyCPFMatricula> CPFsMatriculas { get; set; }
    }
}