using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyResumoRepLogImportacao
    {
        public DateTime Inchora { get; set; }

        [TableHTMLAttribute("Data/Hora", 1, true, ItensSearch.text, OrderType.desc)]
        public String DataHoraLog { get { return String.Format("{0:dd/MM/yyyy HH:mm:ss}", Inchora); } }

        [TableHTMLAttribute("Usuário Inclusão", 2, true, ItensSearch.text, OrderType.asc)]
        public string IncUsuario { get; set; }
        
        [TableHTMLAttribute("Lote", 3, true, ItensSearch.text, OrderType.none)]

        public string Lote { get; set; }
        [TableHTMLAttribute("Relógio", 4, true, ItensSearch.text, OrderType.none)]

        public string Relogio { get; set; }
        [TableHTMLAttribute("Qtd Reg. Processados", 5, true, ItensSearch.text, OrderType.none)]
        public int QtdRegistrosProcessados { get; set; }
        [TableHTMLAttribute("Primeiro NSR", 6, true, ItensSearch.text, OrderType.none)]
        public int PrimeiroNSR { get; set; }
        [TableHTMLAttribute("Último NSR", 7, true, ItensSearch.text, OrderType.none)]
        public int UltimoNSR { get; set; }
        [TableHTMLAttribute("Qtd Fila", 8, true, ItensSearch.text, OrderType.none)]
        public int QtdAddFila { get; set; }
        [TableHTMLAttribute("Qtd Bilhete", 9, true, ItensSearch.text, OrderType.none)]
        public int QtdAddBilhetes { get; set; }
        [TableHTMLAttribute("Detalhes", 10, true, ItensSearch.text, OrderType.none)]
        public string Visualizar { get; set; }

        public REP Rep { get; set; }
    }
}
