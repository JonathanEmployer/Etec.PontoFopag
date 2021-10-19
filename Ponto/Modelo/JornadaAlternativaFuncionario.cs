using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class JornadaAlternativaFuncionario : Modelo.ModeloBase
    {
        public int IdJornadaAlternativa { get; set; }
        public int IdFuncionario { get; set; }
        public JornadaAlternativa JornadaAlternativa { get; set; }
        public Funcionario Funcionario { get; set; }
    }

}
