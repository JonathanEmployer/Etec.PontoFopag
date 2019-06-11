using System.Collections.Generic;

namespace DAL
{
    public interface IParametroPainelRH : DAL.IDAL
    {
        Modelo.ParametroPainelRH LoadObject(int id);
        List<Modelo.ParametroPainelRH> GetAllList();
    }
}

