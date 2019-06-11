using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Acesso
{
    public class AcessoAPI
    {
        public int ID { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
        public string Url { get; set; }
        public int Timer { get; set; }
    }
}
