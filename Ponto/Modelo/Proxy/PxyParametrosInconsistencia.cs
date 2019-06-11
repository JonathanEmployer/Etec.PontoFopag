using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyParametrosInconsistencia
    {
        public bool Interjornada            { get; set; }
        public bool Intrajornada            { get; set; }
        public bool SetimoDiaTrabalhado     { get; set; }
        public bool LimiteHorasTrabalhadas  { get; set; }
        public bool TerceiroDomTrabalhado   { get; set; }
        public bool SeisHorasSemIntervalo   { get; set; }
    }
}
