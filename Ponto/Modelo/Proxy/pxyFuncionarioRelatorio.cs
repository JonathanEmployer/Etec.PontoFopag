using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyFuncionarioRelatorio : ModeloBase
    {
        public pxyFuncionarioRelatorio() {
            selecionadoStr = "N";
        }
        public bool Selecionado { get; set; }
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string Dscodigo { get; set; }
        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }
        public int IdDepartamento { get; set; }
        [TableHTMLAttribute("Departamento", 5, true, ItensSearch.select, OrderType.none)]
        public string Departamento { get; set; }
        public int IdEmpresa { get; set; }
        [TableHTMLAttribute("Empresa", 3, true, ItensSearch.select, OrderType.none)]
        public string Empresa { get; set; }
        public int IdFuncao { get; set; }
        [TableHTMLAttribute("Função", 4, true, ItensSearch.select, OrderType.none)]
        public string Funcao { get; set; }
        public int IdContrato { get; set; }
        [TableHTMLAttribute("Contrato", 7, true, ItensSearch.select, OrderType.none)]
        public string Contrato { get; set; }
        public int IdAlocacao { get; set; }
        [TableHTMLAttribute("Alocação", 6, true, ItensSearch.select, OrderType.none)]
        public string Alocacao { get; set; }       
        private string selecionadoStr;
        public string SelecionadoStr { get
            {
                return this.selecionadoStr;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
	            {
		            this.selecionadoStr = "N";
	            }
                else
                {
                    this.selecionadoStr = value;
                }
            }
        }

        public int Excluido { get; set; }
        
        public int Funcionarioativo { get; set; }

        public string Pis { get; set; }

        [TableHTMLAttribute("Horário", 8, true, ItensSearch.select, OrderType.none)]
        public string DescHorario { get; set; }

        public string Matricula { get; set; }
    }

    public class pxyFuncionarioRelatorioComInativo: pxyFuncionarioRelatorio
    {
        [TableHTMLAttribute("Ativo", 9, true, ItensSearch.select, OrderType.none)]
        public String Ativo
        {
            get
            {
                return Funcionarioativo == 1 ? "Sim" : "Não";
            }
        }
    }
}
