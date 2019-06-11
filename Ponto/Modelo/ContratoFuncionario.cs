using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class ContratoFuncionario : ModeloBase
    {
        public int IdContrato { get; set; }
        public int IdFuncionario { get; set; }
        public string NomeEmpresa { get; set; }
        public string CodigoContrato { get; set; }
        public string NomeFuncionario { get; set; }
    }
}
