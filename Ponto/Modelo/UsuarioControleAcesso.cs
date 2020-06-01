using Modelo.EntityFramework.MonitorPontofopag;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class UsuarioControleAcesso
    {
        public int Idfuncionario { get; set; }

        [TableHTMLAttribute("Tipo", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

    }
}
