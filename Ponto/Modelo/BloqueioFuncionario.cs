using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class BloqueioFuncionario
    {
        public Funcionario Funcionario { get; set; }
        public int? RegraBloqueio { get; set; }
        public string Motivo { get; set; }
        public DateTime? PrevisaoLiberacao { get; set; }
        public bool Bloqueado { get; set; }
    }
}
