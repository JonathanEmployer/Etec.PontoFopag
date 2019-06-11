using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyOcorrenciaEvento : Ocorrencia
    {
        public bool Selecionado { get; set; }

        public pxyOcorrenciaEvento() : base()
        {

        }
    }
}
