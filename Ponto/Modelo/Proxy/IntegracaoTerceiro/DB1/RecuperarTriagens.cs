using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.IntegracaoTerceiro.DB1
{
    public class RecuperarTriagens
    {
        /// <summary>
        /// Data Início para triagem
        /// </summary>
        public string DataInicio { get; set; }
        /// <summary>
        /// Data Fim para Triagem
        /// </summary>
        public string DataFinal { get; set; }
        /// <summary>
        /// Lista com os Cpfs a serem triados
        /// </summary>
        public List<string> Cpfs { get; set; }
        /// <summary>
        /// 0 - PDF; 1 - HTML; 2 - Excel; 3 - Json
        /// </summary>
        public int TipoRelatorio { get; set; }
        /// <summary>
        /// Indica se o relatório deve ser sintético se falso será o analítico
        /// </summary>
        public bool Sintetico { get; set; }
    }
}
