using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IListaEventos : DAL.IDAL
    {
        Modelo.ListaEventos LoadObject(int id);
        List<Modelo.ListaEventos> GetAllList();
    }
}

