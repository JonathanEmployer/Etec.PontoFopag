using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.Modelo
{
    public class RepAFD : ReqResp
    {
        public string nsr_ini { get; set; }
        public string nsr_fim { get; set; }
        public IList<string> registros { get; set; }
    }
}
