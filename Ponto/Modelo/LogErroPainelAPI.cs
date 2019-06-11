namespace Modelo
{
    public class LogErroPainelAPI : Modelo.ModeloBase
    {
        public LogErroPainelAPI()
        {
            this.NaoValidaCodigo = true;
        }
        [TableHTMLAttribute("Código Funcionário", 1, true, ItensSearch.text, OrderType.none)]
        public string dscodigo { get; set; }
        [TableHTMLAttribute("Nome", 2, true, ItensSearch.select, OrderType.asc)]
        public string nomeFuncionario { get; set; }
        public int idFuncionario { get; set; }
        [TableHTMLAttribute("Erro", 3, true, ItensSearch.text, OrderType.none)]
        public string logErro { get; set; }
        [TableHTMLAttribute("Erro Detalhado", 5, true, ItensSearch.text, OrderType.none)]
        public string logDetalhe { get; set; }
        [TableHTMLAttribute("Tipo Operação", 4, true, ItensSearch.text, OrderType.none)]
        public string tipoOperacao { get; set; }

    }
}