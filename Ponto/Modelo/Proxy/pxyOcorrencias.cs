using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyOcorrencias : ModeloBase
    {
        public bool Selecionado { get; set; }

        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        public int CodigoHtml { get { return this.Codigo; } set { this.Codigo = value; } }

    }
}
