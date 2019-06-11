using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GerenciadorWeb.Models
{
    public class RelatorioBloqueioViewModel
    {
        public int? Funcionario { get; set; }
        public string Responsavel { get; set; }
        public string Acao { get; set; }
        public List<SelectListItem> Acoes { get; set; }
        public string Regra { get; set; }
        public List<SelectListItem> Regras { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fim { get; set; }
    }
}