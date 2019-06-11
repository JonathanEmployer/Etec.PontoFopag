using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.PontofopagAPI.ModeloAPI
{
    public class Funcionario
    {
        public int? ID { get; set; }
        public string CPF { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }

        public string NumerosCPF
        {
            get
            {
                return CPF.Replace("-", "").Replace(".", "");
            }
        }
    }
}
