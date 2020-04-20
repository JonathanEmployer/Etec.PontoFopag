using System.Collections.Generic;

namespace DAL
{
    public interface IJornadaSubstituirFuncionario : DAL.IDAL
    {
        Modelo.JornadaSubstituirFuncionario LoadObject(int id);
        List<Modelo.JornadaSubstituirFuncionario> GetAllList();
    }
}

