using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyFuncionario : ModeloBase
    {
        public bool Selecionado { get; set; }
        public string Nome { get; set; }
        public string Departamento { get; set; }
        public int IdDepartamento { get; set; }
        public int IdEmpresa { get; set; }
    }
}
