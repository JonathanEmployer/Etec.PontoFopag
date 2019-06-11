using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.Models.Painel
{
    public class UsuarioPainelAutenticacaoRetorno
    {
        public string alterarSenha { get; set; }
        public string mensagem { get; set; }
        public bool status { get; set; }
    }
}