using System.Collections.Generic;

namespace DAL
{
    public interface IHorarioDinamicoPHExtra : DAL.IDAL
    {
        Modelo.HorarioDinamicoPHExtra LoadObject(int id);
        List<Modelo.HorarioDinamicoPHExtra> GetAllList();
        List<Modelo.HorarioDinamicoPHExtra> LoadObjectByHorarioDinamico(int idHorarioDinamico);

        List<Modelo.HorarioDinamicoPHExtra> LoadObjectByHorarioDinamico(List<int> idsHorarioDinamico);
    }
}

