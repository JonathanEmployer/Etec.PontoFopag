using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.RegraBloqueio
{
    public class LogScript
    {
        public int Id { get; set; }

        public DateTime DataHora { get; set; }

        public bool Bloqueado { get; set; }

        public string Mensagem { get; set; }

        public DateTime? Liberacao { get; set; }

        public string Usuario { get; set; }

        public int? RegraBloqueio { get; set; }

        public bool? FlagBloqueadoGestor { get; set; }

        public DateTime? ExpiracaoFlagGestor { get; set; }

        public string MensagemFlagGestor { get; set; }

        public DateTime? AlertaEnviado { get; set; }

        public string DescricaoRegra { get; set; }
    }
}
