using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyExportacaoWebfopag
    {
        [ExportToXls("nome", 10)]
        public string Nome { get; set; }
        [ExportToXls("cpf", 10)]
        public string CPF { get; set; }
        [ExportToXls("pis", 10)]
        public string Pis { get; set; }
        [ExportToXls("codigo_filial", 10)]
        public string CodigoFilial { get; set; }
        [ExportToXls("matricula", 10)]
        public string Matricula { get; set; }
        [ExportToXls("codigo_contrato", 10)]
        public string CodigoContrato { get; set; }
        [ExportToXls("codigo_verba", 10)]
        public string CodigoEvento { get; set; }
        [ExportToXls("codigo_complemento", 10)]
        public string CodigoComplemento { get; set; }
        [ExportToXls("quantidade", 10)]
        public string ValorEvento { get; set; }
        [ExportToXls("percentual", 10)]
        public string Percentual { get; set; }
        [ExportToXls("valor", 10)]
        public string Valor { get; set; }
        [ExportToXls("ano", 10)]
        public string Ano { get; set; }
        [ExportToXls("mes", 10)]
        public string Mes { get; set; }
        [ExportToXls("estrutura_centro_resultado", 10)]
        public string EstruturaCentroResultado { get; set; }
        [ExportToXls("cei_obra", 10)]
        public string CeiObra { get; set; }
        public string codigoFunc { get; set; }
    }
}
