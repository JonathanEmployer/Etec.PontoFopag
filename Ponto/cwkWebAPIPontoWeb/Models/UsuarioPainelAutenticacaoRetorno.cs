using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class UsuarioPainelAutenticacaoRetorno
    {
        public string alterarSenha { get; set; }
        public string mensagem { get; set; }
        public bool status { get; set; }
    }
}