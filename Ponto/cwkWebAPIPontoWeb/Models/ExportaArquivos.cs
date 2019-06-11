using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Objeto com os parâmetros dos dados a serem exportados.
    /// </summary>
    public class ExportaArquivos
    {
        /// <summary>
        /// CNPJ da Empresa que deseja exportar o arquivo
        /// </summary>
        public Int64 DocumentoEmpresa { get; set; }
        /// <summary>
        /// Tipo do arquivo que deseja exportar (0 = AFDT, 1 = ACJEF)
        /// </summary>
        public int TipoArquivo { get; set; }
        /// <summary>
        /// Data Inicial do período que deseja exportar o arquivo
        /// </summary>
        public string DataInicial { get; set; }
        /// <summary>
        /// Data Final do período que deseja exportar o arquivo
        /// </summary>
        public string DataFinal { get; set; }
    }
}