using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.RegraBloqueio
{
    public class RelatorioHistoricoBloqueio
    {
        public string CPF { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
        public string RealizadoPor { get; set; }
        public int? RegraBloqueio { get; set; }
        public DateTime? Expiracao { get; set; }
        public string Acao { get; set; }
        public DateTime Insercao { get; set; }
    }
}
