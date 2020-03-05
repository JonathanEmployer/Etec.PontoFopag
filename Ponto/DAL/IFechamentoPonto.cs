﻿using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IFechamentoPonto : DAL.IDAL
    {
        Modelo.FechamentoPonto LoadObject(int id);
        List<Modelo.FechamentoPonto> GetAllList();
        List<Modelo.FechamentoPonto> GetFechamentosPorTipoFiltro(DateTime data, int tipoFiltro, List<int> idsRegistros);
    }
}
