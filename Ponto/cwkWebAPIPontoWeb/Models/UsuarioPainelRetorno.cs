using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class UsuarioPainelRetorno
    {
        public string celular { get; set; }
        public string documentoFormatado { get; set; }
        public string email { get; set; }
        public bool exibePrimeiroAcesso { get; set; }
        public bool exibirCampoCelular { get; set; }
        public bool exibirCampoEmail { get; set; }
        public int idUsuario { get; set; }
    }
}