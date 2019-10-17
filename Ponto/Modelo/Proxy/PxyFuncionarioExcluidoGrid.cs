using System;

namespace Modelo.Proxy
{
    public class PxyFuncionarioExcluidoGrid
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }
        [TableHTMLAttribute("Código", 2, true, ItensSearch.text, OrderType.none)]
        public string Codigo { get; set; }
        [TableHTMLAttribute("Nome", 1, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }
        [TableHTMLAttribute("Matrícula", 3, true, ItensSearch.text, OrderType.none)]
        public string Matricula { get; set; }
        [TableHTMLAttribute("Jornada", 4, true, ItensSearch.text, OrderType.none)]
        public string Horario { get; set; }
        [TableHTMLAttribute("Empresa", 5, true, ItensSearch.text, OrderType.none)]
        public string Empresa { get; set; }
        [TableHTMLAttribute("Departamento", 6, true, ItensSearch.text, OrderType.none)]
        public string Departamento { get; set; }
        [TableHTMLAttribute("Função", 7, true, ItensSearch.text, OrderType.none)]
        public string Funcao { get; set; }
        [TableHTMLAttribute("Carteira", 8, true, ItensSearch.text, OrderType.none)]
        public string Carteira { get; set; }
        [TableHTMLAttribute("CPF", 9, true, ItensSearch.text, OrderType.none)]
        public string CPF { get; set; }

        public DateTime? DataAdmissao { get; set; }
        [TableHTMLAttribute("Admissão", 10, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DataadmissaoDesc
        {
            get
            {
                return DataAdmissao == null ? "" : DataAdmissao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        public DateTime? DataDemissao { get; set; }
        [TableHTMLAttribute("Demissão", 11, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DatademissaoDesc
        {
            get
            {
                return DataDemissao == null ? "" : DataDemissao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        public DateTime? DataInativacao { get; set; }
        [TableHTMLAttribute("Inativação", 12, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DataInativacaoDesc
        {
            get
            {
                return DataInativacao == null ? "" : DataInativacao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
    }
}
