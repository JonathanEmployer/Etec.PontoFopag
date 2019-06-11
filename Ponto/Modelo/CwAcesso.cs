using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class CwAcesso : Cw_Acesso
    {
        public string Controller { get; set; }
        public string Nome { get; set; }
        public string Menu { get; set; }
        public bool Cadastrar { get; set; }
        public bool Alterar { get; set; }
        public bool Excluir { get; set; }
        public bool Consultar { get; set; }
    }
}
