using System.Collections.Generic;

namespace DAL
{
    public interface IHorarioRestricao : DAL.IDAL
    {
        Modelo.HorarioRestricao LoadObject(int id);
        List<Modelo.HorarioRestricao> GetAllList();
        List<Modelo.HorarioRestricao> GetAllListByHorarios(List<int> idsHorario); 
    }
}

