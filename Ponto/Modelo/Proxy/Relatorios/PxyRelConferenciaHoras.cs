using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelConferenciaHoras
    {
        [ExportToXls("Período", 1)]
        public string Periodo { get; set; }
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
        public string totalhorastrabalhadas { get; set; }


        public int HorasPontoMin
        {
            get { return Modelo.cwkFuncoes.ConvertHorasMinuto(totalhorastrabalhadas); }
        }

        public int HorasTaskMin { get; set; }
        public int DiferencaPontoTaskMin { 
            get { return HorasPontoMin - HorasTaskMin; } 
        }

        [ExportToXls("Horas Task", 10)]
        public string HorasTask { 
            get { 
                string horas = "00:00";
                if (HorasTaskMin > 0)
	                {
		                 horas = Modelo.cwkFuncoes.ConvertMinutosHora2(3,HorasTaskMin);
	                }
                return horas;
            }
        }

        [ExportToXls("Diferença", 11)]
        public string DiferencaPontoTask {
            get { return Modelo.cwkFuncoes.ConvertMinutosHoraNegativo(DiferencaPontoTaskMin); }
        }

        [ExportToXls("Diferença Minutos", 11)]
        public int DiferencaPontoTaskMinTotal
        {
            get { return DiferencaPontoTaskMin; }
        } 


    }
}
