using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyFechamentoPontoFuncionario : FechamentoPontoFuncionario
    {
        public int CodigoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
        public string DescricaoFechamento { get; set; }
        public string DSCodigo { get; set; }
        public string NomeFuncionario { get; set; }
    }
}
