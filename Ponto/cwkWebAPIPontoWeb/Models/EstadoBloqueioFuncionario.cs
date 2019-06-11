using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Objeto com o estado do bloqueio da estação do funcionário.
    /// </summary>
    public class EstadoBloqueioFuncionario
    {
        /// <summary>
        /// CPF do funcionário.
        /// </summary>
        public string CPF { get; set; }
        /// <summary>
        /// Indica se está bloqueado ou não.
        /// </summary>
        public bool Bloqueado { get; set; }
        /// <summary>
        /// Regra que ocasionou o bloqueio.
        /// </summary>
        public int? RegraBloqueio { get; set; }
        /// <summary>
        /// Mensagem descritiva do bloqueio.
        /// </summary>
        public string Mensagem { get; set; }
        /// <summary>
        /// Data/hora de previsão de liberação, se aplicável.
        /// </summary>
        public string PrevisaoLiberacao { get; set; }
    }
}