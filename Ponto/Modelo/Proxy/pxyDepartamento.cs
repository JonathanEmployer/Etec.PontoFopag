using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyDepartamento : ModeloBase
    {
        public bool Selecionado { get; set; }
        public string Descricao { get; set; }
        public int IdEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
    }
}
