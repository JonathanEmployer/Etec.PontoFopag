using System.Collections.Generic;

namespace DAL
{
    public interface IHorarioDinamicoLimiteDdsr : DAL.IDAL
    {
        Modelo.HorarioDinamicoLimiteDdsr LoadObject(int id);
        List<Modelo.HorarioDinamicoLimiteDdsr> GetAllList();
        List<Modelo.HorarioDinamicoLimiteDdsr> LoadObjectByHorarioDinamico(int idHorarioDinamico);
    }
}

