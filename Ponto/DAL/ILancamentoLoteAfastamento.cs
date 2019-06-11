using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ILancamentoLoteAfastamento : DAL.IDAL
    {
        Modelo.LancamentoLoteAfastamento LoadObject(int id);
        List<Modelo.LancamentoLoteAfastamento> GetAllList();
        Modelo.LancamentoLoteAfastamento LoadByIdLote(int idLote);
    }
}
