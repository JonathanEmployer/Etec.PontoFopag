using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.CalculoMarcacoes.EstrategiasCalculo.Interfaces
{
    public interface IDDSRStrategy
    {
        List<Modelo.Marcacao> CalcularDDSR();
    }
}
