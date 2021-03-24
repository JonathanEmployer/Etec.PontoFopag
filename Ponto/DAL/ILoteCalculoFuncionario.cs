using System.Collections.Generic;

namespace DAL
{
    public interface ILoteCalculoFuncionario : DAL.IDAL
    {
        Modelo.LoteCalculoFuncionario LoadObject(int id);
        List<Modelo.LoteCalculoFuncionario> GetAllList();
    }
}

