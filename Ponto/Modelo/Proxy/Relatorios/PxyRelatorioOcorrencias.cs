using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelatorioOcorrencias
    {
        [ExportToXls("CPF", 1)]
        public string CPF { get; set; }

        [ExportToXls("Data", 2)]
        public DateTime Data { get; set; }

        [ExportToXls("Dia da Semana", 3)]
        public string DiaSemana { get; set; }

        [ExportToXls("Batidas", 4)]
        public string Batidas { get; set; }

        [ExportToXls("Quantidade de Horas", 5)]
        public string QuantidadeHoras { get; set; }

        [ExportToXls("Descrição Ocorrência", 6)]
        public string DescricaoOcorrencia { get; set; }

        [ExportToXls("Matrícula", 7)]
        public string Matricula { get; set; }

        [ExportToXls("Competência", 8)]
        public string Competencia { get; set; }

        [ExportToXls("Observação", 9)]
        public string Observacao { get; set; }

        [ExportToXls("IdDocumentoWorkflow", 10)]
        public string IdDocumentoWorkflow { get; set; }

    }
}
