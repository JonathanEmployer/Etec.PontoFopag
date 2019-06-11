using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies
{
    public class ReceiveDateTimeMessage : IMessage
    {
        public DateTime DataHora { get; set; }
        public DateTime? InicioHorarioVerao { get; set; }
        public DateTime? FimHorarioVerao { get; set; }
    }
}
