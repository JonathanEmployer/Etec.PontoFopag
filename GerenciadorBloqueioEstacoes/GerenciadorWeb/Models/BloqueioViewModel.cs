using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GerenciadorWeb.Models
{
    public class BloqueioViewModel
    {
        public int IDFuncionario { get; set; }
        public bool Bloquear { get; set; }
        public string Motivo { get; set; }
        public string DataConfiguracao { get; set; }
        public string HoraConfiguracao { get; set; }
        public string FuncionariosLote { get; set; }
    }
}