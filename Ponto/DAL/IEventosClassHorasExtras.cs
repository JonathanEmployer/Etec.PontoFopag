using System.Collections.Generic;

namespace DAL
{
    public interface IEventosClassHorasExtras : DAL.IDAL
    {
        Modelo.EventosClassHorasExtras LoadObject(int id);
        List<Modelo.EventosClassHorasExtras> GetAllList();
        IList<Modelo.EventosClassHorasExtras> GetListPorEvento(int idEvento);
        string GetIdsClassificacaoPorEvento(int idEvento);
    }
}

