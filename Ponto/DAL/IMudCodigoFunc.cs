using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IMudCodigoFunc : DAL.IDAL
    {
        Modelo.MudCodigoFunc LoadObject(int id);
        bool VerificaMarcacao(int pId, DateTime pData);
        List<Modelo.MudCodigoFunc> GetMudancasPeriodo(DateTime datai, DateTime dataf);
        List<Modelo.MudCodigoFunc> GetAllList();
    }
}
