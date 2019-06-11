using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.RegraBloqueio
{
    public class HistoricoBloqueio
    {
        public int ID { get; set; }
        public Funcionario Funcionario { get; set; }
        public Acesso.Usuario Usuario { get; set; }
        public DateTime Insercao { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? Liberacao { get; set; }
        public string Mensagem { get; set; }
        public int? RegraBloqueio { get; set; }
    }
}
