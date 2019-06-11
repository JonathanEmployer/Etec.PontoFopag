using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyEnvioDadosRepGrid
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        public int Operacao { get; set; }

        [TableHTMLAttribute("Operação", 2, true, ItensSearch.text, OrderType.asc)]
        public string operacaoStr { get { return Operacao == 1 ? "Sim" : "Não" ;} }

        [TableHTMLAttribute("Núm. Relógio", 3, true, ItensSearch.text, OrderType.asc)]
        public string NumRelogio { get; set; }

        [TableHTMLAttribute("Local Relógio", 4, true, ItensSearch.text, OrderType.asc)]
        public string LocalRelogio { get; set; }

        [TableHTMLAttribute("Modelo Relógio", 5, true, ItensSearch.text, OrderType.asc)]
        public string ModeloRelogio { get; set; }

        public DateTime IncData { get; set; }

        [TableHTMLAttribute("Data/Hora Inclusão", 6, true, ItensSearch.text, OrderType.asc)]
        public string IncDataStr { get {return IncData.ToString("dd/MM/yyyy"); } }

        [TableHTMLAttribute("Tipo Comunicação", 7, true, ItensSearch.text, OrderType.asc)]
        public string TipoComunicacao { get; set; }
    }
}
