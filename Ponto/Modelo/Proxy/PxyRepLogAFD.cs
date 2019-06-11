using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyRepLogAFD
    {
        [ExportToXls("Data/Hora Inc. AFD", 1)]
        public string DataHoraInclusaoAFD { get; set; }

        [ExportToXls("Linha AFD", 2)]
        public string LinhaAFD { get; set; }

        [ExportToXls("Situação AFD", 3)]
        public string Situacao { get; set; }

        [ExportToXls("Relógio", 4)]
        public string Relogio { get; set; }

        [ExportToXls("Data/Hora Registro", 5)]
        public DateTime? DataHoraRegistro { get; set; }

        [ExportToXls("NSR", 6)]
        public int? Nsr { get; set; }

        [ExportToXls("Data/Hora Inc. Fila", 7)]
        public string DataHoraInclusaoFila { get; set; }

        [ExportToXls("Situação Fila", 8)]
        public string SituacaoFila { get; set; }

        [ExportToXls("Data/Hora Registro Ponto", 9)]
        public DateTime? HoraDoRegistroPonto { get; set; }

        [ExportToXls("Data Marcação", 10)]
        public DateTime? DataAlocacaoMarcacao { get; set; }

        [ExportToXls("Nome Funcionário", 11)]
        public string NomeFuncionario { get; set; }

        [ExportToXls("Pis", 12)]
        public string Pis { get; set; }

        [ExportToXls("CPF", 13)]
        public string CPF { get; set; }

        [ExportToXls("Matrícula", 14)]
        public string Matricula { get; set; }

        [ExportToXls("Empresa", 15)]
        public string Empresa { get; set; }

        [ExportToXls("CNPJ Empresa", 16)]
        public string CNPJEmpresa { get; set; }

    }
}
