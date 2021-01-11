using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyFuncionarioFechamentosPontoEBH
    {
        public int FuncionarioId { get; set; }

        public string FuncionarioCodigo { get; set; }

        public string FuncionarioNome { get; set; }

        public string FuncionarioCPF { get; set; }

        public string FuncionarioMatricula { get; set; }

        public int FechamentoTipo { get; set; }

        public string FechamentoTipoDesc { get; set; }

        public int FechamentoId { get; set; }

        public int FechamentoCodigo { get; set; }

        public string FechamentoDescricao { get; set; }

        public DateTime FechamentoData { get; set; }
    }
}
