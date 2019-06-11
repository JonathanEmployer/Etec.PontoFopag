using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Acesso
{
    public class Usuario
    {
        public string ID { get; set; }
        public string NomeUsuario { get; set; }
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
        public string NovaSenha { get; set; }
    }
}
