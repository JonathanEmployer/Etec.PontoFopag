using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelClassificacaoHorasExtras
    {
        [ExportToXls("Nome Empresa", 1)]
        public string EmpresaNome { get; set; }

        [ExportToXls("CNPJ/CPF Empresa", 2)]
        public string EmpresaCNPJ { get; set; }

        [ExportToXls("Código Empresa", 3)]
        public int EmpresaCodigo { get; set; }

        [ExportToXls("Cidade Empresa", 4)]
        public string EmpresaCidade { get; set; }

        [ExportToXls("Endereço Empresa", 5)]
        public string EmpresaEndereco { get; set; }

        [ExportToXls("Código Departamento", 6)]
        public int? DepartamentoCodigo { get; set; }

        [ExportToXls("Descrição Departamento", 7)]
        public string DepartamentoDescricao { get; set; }

        [ExportToXls("Código Funcionário", 8)]
        public string FuncionarioCodigo { get; set; }

        [ExportToXls("Matrícula Funcionário", 9)]
        public string FuncionarioMatricula { get; set; }

        [ExportToXls("CPF Funcionário", 10)]
        public string FuncionarioCPF { get; set; }

        [ExportToXls("Nome Funcionário", 11)]
        public string FuncionarioNome { get; set; }

        [ExportToXls("Data", 12)]
        public DateTime Data { get; set; }

        [ExportToXls("Código Classificação", 13)]
        public int? ClassificacaoCodigo { get; set; }

        [ExportToXls("Descrição Classificação", 14)]
        public string ClassificacaoDescricao { get; set; }

        public short? Tipo { get; set; }

        [ExportToXls("Código", 15)]
        public string TipoDescricao { get; set; }

        public int? ClassificadasMin { get; set; }

        [ExportToXls("Classificadas", 16)]
        public string Classificadas { get; set; }

        public int? NaoClassificadasMin { get; set; }

        [ExportToXls("Não Classificadas", 17)]
        public string NaoClassificadas { get; set; }

        public int? HorasExtrasRealizadaMin { get; set; }

        [ExportToXls("Horas Extras", 18)]
        public string HorasExtrasRealizada { get; set; }
    }
}
