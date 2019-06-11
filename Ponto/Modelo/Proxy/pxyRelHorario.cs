using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyRelHorario
    {
        public string TipoArquivo { get; set; }
        public List<pxyHorario> Horarios { get; set; }
        public string IdSelecionados { get; set; }
    }
}
