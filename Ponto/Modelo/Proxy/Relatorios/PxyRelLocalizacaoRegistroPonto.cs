using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelLocalizacaoRegistroPonto
    {
        [ExportToXls("Nome Empresa", 1)]
        public string EmpresaNome { get; set; }

        [ExportToXls("Endereço Empresa", 2)]
        public string EmpresaEndereco { get; set; }

        [ExportToXls("Código Funcionário", 3)]
        public string FuncionarioDsCodigo { get; set; }

        [ExportToXls("Nome Funcionário", 4)]
        public string FuncionarioNome { get; set; }

        public string FuncionarioDsCodigoNome { get; set; }

        [ExportToXls("Código Departamento", 5)]
        public int DepartamentoCodigo { get; set; }

        [ExportToXls("Nome Departamento", 6)]
        public string DepartamentoDescricao { get; set; }

        public string DepartamentoCodigoDescricao { get; set; }

        [ExportToXls("Código Função", 7)]
        public int FuncaoCodigo { get; set; }

        [ExportToXls("Descrição Função", 8)]
        public string FuncaoDescricao { get; set; }

        public string FuncaoCodigoDescricao { get; set; }

        [ExportToXls("Data Registro", 9)]
        public DateTime mar_data { get; set; }

        [ExportToXls("Hora Registro", 10)]
        public string mar_hora { get; set; }
        [ExportToXls("Relógio", 11)]
        public string Relogio { get; set; }

        [ExportToXls("IP", 12)]
        public string IP { get; set; }

        [ExportToXls("Navegador", 13)]
        public string Browser { get; set; }

        [ExportToXls("Versão Navegador", 14)]
        public string BrowserVersao { get; set; }

        public string Periodo { get; set; }
        [ExportToXls("Latitude", 15)]
        public string Latitude { get; set; }
        [ExportToXls("Longitude", 16)]
        public string Longitude { get; set; }
        [ExportToXls("Ver Mapa", 17)]
        public string VerMapa { get; set; }
    }
}
