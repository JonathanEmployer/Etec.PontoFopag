using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyTransferenciaBilhetes : PxyPeriodo
    {
        [Display(Name = "Funcionário")]
        public string FuncionarioOrigem { get; set; }
        public int IdFuncionarioOrigem { get; set; }
        public int IdFuncionarioDestino { get; set; }
    }
}
