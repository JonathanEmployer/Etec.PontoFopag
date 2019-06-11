using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyAbonoDsrFuncionario
    {
        public int IdFuncionario { get; set; }
        public string Nome { get; set; }
        public int QtdDiasAbono { get; set; }
        public int QtdMinutosAbono { get; set; }
        public int QtdDiasDsr { get; set; }
        public int QtdMinutosDsr { get; set; }

        public pxyAbonoDsrFuncionario()
        {
        }
    }
}
