using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyGridUsuario
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Login", 2, true, ItensSearch.select, OrderType.none)]
        public string Login { get; set; }

        [TableHTMLAttribute("Nome", 3, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Grupo", 4, true, ItensSearch.text, OrderType.none)]
        public string Grupo { get; set; }

        [TableHTMLAttribute("Email", 5, true, ItensSearch.text, OrderType.none)]
        public string Email { get; set; }

        [TableHTMLAttribute("Controle por Empresa", 6, true, ItensSearch.text, OrderType.none)]
        public string ControlePorEmpresa { get; set; }

        [TableHTMLAttribute("Controle por Contrato", 7, true, ItensSearch.text, OrderType.none)]
        public string ControlePorContrato { get; set; }

        [TableHTMLAttribute("Controle por Supervisor", 8, true, ItensSearch.text, OrderType.none)]
        public string ControlePorSupervisor { get; set; }
    }
}
