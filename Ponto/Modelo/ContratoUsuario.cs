using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class ContratoUsuario : ModeloBase
    {
        public int IdContrato { get; set; }
        public int IdCw_Usuario { get; set; }
        public string NomeEmpresa { get; set; }
        public string CodigoContrato { get; set; }
        public string NomeUsuario { get; set; }
    }
}
