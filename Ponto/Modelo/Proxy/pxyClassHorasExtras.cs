using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyClassHorasExtras
    {
        public string Empresa { get; set; }
        public string Departamento { get; set; }
        public string Funcionario { get; set; }
        public DateTime DataIni { get; set; }
        public DateTime DataFin { get; set; }
        public IList<pxyClassHorasExtrasMarcacao> pxyClassHorasExtrasMarcacoes { get; set; }
    }
}
