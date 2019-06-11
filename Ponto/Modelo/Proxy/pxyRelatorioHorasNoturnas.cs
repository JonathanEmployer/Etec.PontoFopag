using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelatorioHorasNoturnas
    {
        [ExportToXls("Período", 1)]
        public string Periodo { get; set; }

        [ExportToXls("Nome Empresa", 2)]
        public string EmpresaNome { get; set; }
        [ExportToXls("PIS   ", 3)]
        public string PIS { get; set; }
        [ExportToXls("Matrícula", 4)]
        public string FuncionarioMatricula { get; set; }
        [ExportToXls("Código Funcionário", 5)]
        public string FuncionarioCodigo { get; set; }
        [ExportToXls("Nome Funcionário", 6)]
        public string FuncionarioNome { get; set; }
        [ExportToXls("Horas Trabalhadas Noturnas", 7)]
        public string TotalHorasNoturnas { get; set; }
        [ExportToXls("Total Trab. Not.(AD)", 8)]
        public string TotalHorasNoturnasComReducao { get; set; }
    }
}
