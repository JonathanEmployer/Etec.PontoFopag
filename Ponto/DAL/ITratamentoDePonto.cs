using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ITratamentoDePonto : DAL.IDAL
    {
        List<Modelo.Proxy.Relatorios.PxyRelatorioTratamentoDePonto> RelatorioTratamentoDePonto(List<int> idsFuncs, DateTime dataInicial, DateTime dataFinal);
    }
}

