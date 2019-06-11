using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Parametros
{
    public class RelatorioHistoricoBloqueio
    {
        public int? RegraBloqueio { get; set; }
        public int? Funcionario { get; set; }
        public string Usuario { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public int? Acao { get; set; }
    }
}
