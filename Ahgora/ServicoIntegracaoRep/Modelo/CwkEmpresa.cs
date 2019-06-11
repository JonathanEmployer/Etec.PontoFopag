using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.Modelo
{
    public class CwkEmpresa
    {
        public string SenhaMenu { get; set; }
        public string Tipo { get; set; }
        public string CNPJouCPF { get; set; }
        public string Local { get; set; }
        public string RazaoSocial { get; set; }
        public string Identificador { get; set; }
        public string CEI { get; set; }
        public int IdEnvioDadosRep { get; set; }
        public int IdEnvioDadosRepDet { get; set; }
        public bool BOperacao { get; set; }
    }
}
