using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyGridServicoPontoCom
    {
        public PxyGridServicoPontoCom()
        {
        }

        public PxyGridServicoPontoCom(int id, int? codigo, string descricao, string observacao, DateTime? inchora, string incusuario, DateTime? althora, string altusuario, string serverName, string mAC)
        {
            Id = id;
            Codigo = codigo;
            Descricao = descricao;
            Observacao = observacao;
            Inchora = inchora;
            Incusuario = incusuario;
            Althora = althora;
            Altusuario = altusuario;
            ServerName = serverName;
            MAC = mAC;
        }

        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }
        //[TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public Nullable<int> Codigo { get; set; }
        [TableHTMLAttribute("Descrição", 1, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }
        [TableHTMLAttribute("Observação", 2, true, ItensSearch.text, OrderType.none)]
        public string Observacao { get; set; }
        [TableHTMLAttribute("Server Name", 3, true, ItensSearch.text, OrderType.none)]
        public string ServerName { get; set; }
        [TableHTMLAttribute("MAC", 4, true, ItensSearch.text, OrderType.none)]
        public string MAC { get; set; }
        [TableHTMLAttribute("Inc. Data/Hora", 5, true, ItensSearch.text, OrderType.none)]
        public Nullable<System.DateTime> Inchora { get; set; }
        [TableHTMLAttribute("Inc. Usuário", 6, true, ItensSearch.select, OrderType.none)]
        public string Incusuario { get; set; }
        [TableHTMLAttribute("Alt. Data/Hora", 7, true, ItensSearch.text, OrderType.none)]
        public Nullable<System.DateTime> Althora { get; set; }
        [TableHTMLAttribute("Alt. Usuário", 8, true, ItensSearch.select, OrderType.none)]
        public string Altusuario { get; set; }

    }
}
