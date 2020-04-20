using System.Collections.Generic;

namespace DAL
{
    public interface IJornadaSubstituir : DAL.IDAL
    {
        Modelo.JornadaSubstituir LoadObject(int id);
        List<Modelo.JornadaSubstituir> GetAllList();
    }
}

