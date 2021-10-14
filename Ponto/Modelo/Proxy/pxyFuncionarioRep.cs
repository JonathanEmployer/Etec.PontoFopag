using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class pxyFuncionarioRep 
    {
        [TableHTMLAttribute("Código", 4, true, ItensSearch.text, OrderType.none)]
        public string Codigo { get; set; }
        [TableHTMLAttribute("Nome", 4, true, ItensSearch.text, OrderType.none)]
        public string Nome { get; set; }
        [TableHTMLAttribute("Pis", 4, true, ItensSearch.text, OrderType.none)]
        public string Pis { get; set; }
        [TableHTMLAttribute("Senha", 4, true, ItensSearch.text, OrderType.none)]
        public string Senha { get; set; }
        [TableHTMLAttribute("Data e Hora", 4, true, ItensSearch.text, OrderType.none)]
        public string dataHora { get; set; }
        [TableHTMLAttribute("Usúario", 4, true, ItensSearch.text, OrderType.none)]
        public string Usuario { get; set; }
        [TableHTMLAttribute("Local", 4, true, ItensSearch.text, OrderType.none)]
        public string Local { get; set; }
    }
       
}

