using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Relogios.Telematica.Modelo
{
    public class RepTelematica : Modelo.DAT07
    {
        public string NumeroRelogio { get; set; }
        public string Conn { get; set; }
        public string CaminhoArquivo { get; set; }
        public string NumSerieRep { get; set; }
    }
}
