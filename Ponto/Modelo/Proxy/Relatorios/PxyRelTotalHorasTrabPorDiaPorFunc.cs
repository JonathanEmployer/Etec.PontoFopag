using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelTotalHorasTrabPorDiaPorFunc
    {
        [ExportToXls("Data", 2)]
        public DateTime? data { get; set; }
        [ExportToXls("Código Empresa", 3)]
        public int? EmpresaCodigo { get; set; }
        [ExportToXls("Nome Empresa", 4)]
        public string EmpresaNome { get; set; }
        [ExportToXls("CPF", 5)]
        public string cpf { get; set; }
        [ExportToXls("Matrícula", 6)]
        public string FuncionarioMatricula { get; set; }
        [ExportToXls("Código Funcionário", 7)]
        public string FuncionarioCodigo { get; set; }
        [ExportToXls("Nome Funcionário", 8)]
        public string FuncionarioNome { get; set; }
        [ExportToXls("Horas Ponto", 9)]
        public string TotalHorasTrabalhadas { get; set; }


        public int TotalHorasTrabalhadasMin
        {
            get { return Modelo.cwkFuncoes.ConvertHorasMinuto(TotalHorasTrabalhadas); }
        }
    }
}
