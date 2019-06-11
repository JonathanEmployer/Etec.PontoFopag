using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyFuncionarioCabecalhoRel
    {
        public int IdFunc { get; set; }
        public string DsCodigo { get; set; }
        public string Nome { get; set; }
        public DateTime DataAdmissao { get; set; }
        public string Matricula { get; set; }
        public string Pis { get; set; }
        public String CPF { get; set; }
        public int IdEmpresa { get; set; }
        public int IdDepartamento { get; set; }
        public int IdFuncao { get; set; }

        public string EmpresaNome { get; set; }
        public string EmpresaEndereco { get; set; }
        public string EmpresaCidade { get; set; }
        public string EmpresaEstado { get; set; }
        public string EmpresaCEP { get; set; }
        public string EmpresaCNPJCPF { get; set; }

        public int DepartamentoCodigo { get; set; }
        public string DepartamentoNome { get; set; }

        public int FuncaoCodigo  { get; set; }
        public string FuncaoDescricao { get; set; }

        public string AlocacaoDescricao { get; set; }
        public string AlocacaoEndereco { get; set; }

        public Int32 Codigofolha { get; set; }
    }
}
