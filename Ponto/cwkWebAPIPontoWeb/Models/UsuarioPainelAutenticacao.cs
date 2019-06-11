using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class UsuarioPainelAutenticacao
    {
        public int idUsuario { get; set; }
        public string senha { get; set; }
        public string idioma { get; set; }
    }
}