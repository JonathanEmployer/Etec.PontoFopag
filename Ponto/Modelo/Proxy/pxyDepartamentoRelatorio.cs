using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyDepartamentoRelatorio : ModeloBase
    {
        public bool Selecionado { get; set; }
        public string Nome { get; set; }
        public int IdEmpresa { get; set; }
        public string Empresa { get; set; }
    }
}
