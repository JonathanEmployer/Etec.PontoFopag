using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyJornadaSubstituirCalculo
    {
        public int Id { get; set; }
        public int Codigo { get; set; }

        public int IdJornadaDe { get; set; }

        public int IdJornadaPara { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public int IdFuncionario { get; set; }

        public string Entrada1 { get; set; }

        public string Entrada2 { get; set; }

        public string Entrada3 { get; set; }

        public string Entrada4 { get; set; }

        public string Saida1 { get; set; }

        public string Saida2 { get; set; }

        public string Saida3 { get; set; }

        public string Saida4 { get; set; }
        public DateTime IncHora { get; set; }
    }
}
