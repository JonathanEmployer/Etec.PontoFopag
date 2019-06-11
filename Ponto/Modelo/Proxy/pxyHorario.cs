using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyHorario : ModeloBase
    {
        public bool Selecionado { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
    }
}
