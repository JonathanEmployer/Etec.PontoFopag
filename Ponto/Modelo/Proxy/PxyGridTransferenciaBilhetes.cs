using System;

namespace Modelo.Proxy
{
    public class PxyGridTransferenciaBilhetes
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }
        [TableHTMLAttribute("Início", 2, true, ItensSearch.text, OrderType.asc, ColumnType.data)]
        public DateTime DataInicio { get; set; }
        [TableHTMLAttribute("Fim", 3, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public DateTime DataFim { get; set; }
        [TableHTMLAttribute("Func. Origem", 4, true, ItensSearch.select, OrderType.none)]
        public string Origem { get; set; }
        [TableHTMLAttribute("Func. Destino", 5, true, ItensSearch.select, OrderType.none)]
        public string Destino { get; set; }
        [TableHTMLAttribute("Usuário Inclusão", 6, true, ItensSearch.select, OrderType.none)]
        public string IncUsuario { get; set; }
        [TableHTMLAttribute("Data Inclusão", 7, true, ItensSearch.text, OrderType.none,ColumnType.datatime)]
        public DateTime IncHora { get; set; }
        [TableHTMLAttribute("Qtd. Bilhetes Afetados", 8, true, ItensSearch.text, OrderType.none)]
        public int QtdBilhetesAfetados { get; set; }
    }
}
