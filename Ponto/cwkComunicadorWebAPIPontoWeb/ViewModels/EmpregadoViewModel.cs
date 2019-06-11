using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    class EmpregadoViewModel : cwkPontoMT.Integracao.Entidades.Empregado
    {
        public string PisFormat
        {
            get
            {
                return Convert.ToUInt64(Pis).ToString(@"000\.00000\.00\-0");
            }
        }
    }
}
