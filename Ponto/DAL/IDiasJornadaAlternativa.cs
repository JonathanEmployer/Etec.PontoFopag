using System;
using System.Data;

namespace DAL
{
    public interface IDiasJornadaAlternativa : DAL.IDAL
    {

        Modelo.DiasJornadaAlternativa LoadObject(int id);

    }
}
