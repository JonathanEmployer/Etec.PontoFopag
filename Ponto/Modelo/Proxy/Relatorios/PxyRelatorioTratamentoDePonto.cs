using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelatorioTratamentoDePonto
    {
        [ExportToXls("CPF", 1)]
        public string CPF { get; set; }

        [ExportToXls("Matrícula", 6)]
        public string Matricula { get; set; }

        [ExportToXls("Horário", 2)]
        public string Hora { get; set; }

        [ExportToXls("Ocorrência", 3)]
        public string Ocorrencia { get; set; }

        [ExportToXls("Motivo", 4)]
        public string Motivo { get; set; }

        [ExportToXls("Data", 5)]
        public DateTime Data { get; set; }

        [ExportToXls("Observacao", 7)]
        public string Observacao { get; set; }

        [ExportToXls("Competencia", 7)]
        public string competencia { get; set; }
    }
}
