using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorWeb.Models
{
    public class EstacaoViewModel
    {
        public int ID { get; set; }
        public string CPF { get; set; }
        public string Usuario { get; set; }
        public bool Bloqueado { get; set; }
        public string Regra { get; set; }
        public string Mensagem { get; set; }
        public DateTime? Expiracao { get; set; }
        public bool ForcadoGestor { get; set; }
        public bool Selecionado { get; set; }
    }
}