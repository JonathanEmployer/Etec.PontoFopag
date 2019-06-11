using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IFechamentoPontoFuncionario : DAL.IDAL
    {
        Modelo.FechamentoPontoFuncionario LoadObject(int id);
        List<Modelo.FechamentoPontoFuncionario> GetAllList();
        List<Modelo.FechamentoPontoFuncionario> GetListWhere(string condicao);
        List<Modelo.Proxy.pxyFechamentoPontoFuncionario> ListaFechamentoPontoFuncionario(int tipo, List<int> idsRegistros, DateTime data);
    }
}
