using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Acesso
{
    public class Login
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool Persistente { get; set; }
    }
}
