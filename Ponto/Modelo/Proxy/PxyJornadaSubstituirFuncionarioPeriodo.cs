using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyJornadaSubstituirFuncionarioPeriodo
    {
        public int FuncionarioId { get; set; }

        public string FuncionarioCodigo { get; set; }

        public string FuncionarioNome { get; set; }

        public string FuncionarioCPF { get; set; }

        public string FuncionarioMatricula { get; set; }

        public int JornadaSubstituirId { get; set; }

        public int JornadaSubstituirCodigo { get; set; }

        public DateTime JornadaSubstituirDataInicio { get; set; }

        public DateTime JornadaSubstituirDataFim { get; set; }

        public int IdJornadaSubstituirFuncionario { get; set; }

        public int JornadaSubstituirIdJornadaDe { get; set; }

        public int JornadaSubstituirIdJornadaPara { get; set; }
    }
}
