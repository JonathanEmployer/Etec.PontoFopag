using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ILancamentoLoteBilhetesImp : DAL.IDAL
    {
        Modelo.LancamentoLoteBilhetesImp LoadObject(int id);
        List<Modelo.LancamentoLoteBilhetesImp> GetAllList();
        Modelo.LancamentoLoteBilhetesImp LoadByIdLote(int idLote);
    }
}
