using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.RegraBloqueio
{
    public class Funcionario
    {
        public int ID { get; set; }
        public string CPF { get; set; }
        public string Usuario { get; set; }
        public string Nome { get; set; }
        public string Matricula { get; set; }
        public bool Bloqueado { get; set; }
        public string Mensagem { get; set; }
        public DateTime? Liberacao { get; set; }
        public int? RegraBloqueio { get; set; }
        public bool? FlagBloqueadoGestor { get; set; }
        public DateTime? ExpiracaoFlagGestor { get; set; }
        public string MensagemFlagGestor { get; set; }
        public string DescricaoRegra { get; set; }
        public DateTime? AlertaEnviado { get; set; }
    }
}
