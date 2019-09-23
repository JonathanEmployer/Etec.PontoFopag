using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy.Webfopag
{
    public class PxyRetornoColetor
    {
        public int IdfEmpregado { get; set; }

        public decimal CPF { get; set; }

        public string PIS { get; set; }

        public string Data { get; set; }

        public string Hora { get; set; }

        public string Matricula { get; set; }

        public DateTime? DataCadastro { get; set; }

        public DateTime? DataAlteracao { get; set; }

        public string IdfServico { get; set; }

    }
}
