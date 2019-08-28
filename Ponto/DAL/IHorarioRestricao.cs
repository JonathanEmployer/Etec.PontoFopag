using System.Collections.Generic;

namespace DAL
{
    public interface IHorarioDinamicoRestricao : DAL.IDAL
    {
        Modelo.HorarioDinamicoRestricao LoadObject(int id);
        List<Modelo.HorarioDinamicoRestricao> GetAllList();
        List<Modelo.HorarioDinamicoRestricao> GetAllListByHorarios(List<int> idsHorario); 
    }
}

