using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyFuncionariosLote : pxyFuncionarioRelatorio
    {
        public bool Efetivado { get; set; }
        [TableHTMLAttribute("Efetivado", 11, true, ItensSearch.select, OrderType.none)]
        public string EfetivadoStr { 
            get {
                return Efetivado ? "Sim" : "Não";
                } 
        }
        public int UltimaAcao { get; set; }

        [TableHTMLAttribute("Última Ação", 12, true, ItensSearch.select, OrderType.none)]
        public string UltimaAcaoStr
        {
            get
            {
                return Enum.GetName(typeof(Acao), UltimaAcao);
            }
        }
        [TableHTMLAttribute("Erro", 13, true, ItensSearch.select, OrderType.none)]
        public string DescricaoErro { get; set; }
    }
}
