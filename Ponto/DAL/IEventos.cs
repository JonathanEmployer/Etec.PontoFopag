using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IEventos : DAL.IDAL
    {
        Modelo.Eventos LoadObject(int id);
        List<Modelo.Eventos> GetAllList();
        Modelo.Eventos LoadObjectByCodigo(int codigo);
    }
}
