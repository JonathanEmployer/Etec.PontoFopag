using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IAlertas : DAL.IDAL
    {
        Modelo.Alertas LoadObject(int id);
        List<Modelo.Alertas> GetAllList();
    }
}

