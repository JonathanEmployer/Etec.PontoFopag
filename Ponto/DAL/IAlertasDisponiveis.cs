using System.Collections.Generic;

namespace DAL
{
    public interface IAlertasDisponiveis : DAL.IDAL
    {
        Modelo.AlertasDisponiveis LoadObject(int id);
        List<Modelo.AlertasDisponiveis> GetAllList();
    }
}

