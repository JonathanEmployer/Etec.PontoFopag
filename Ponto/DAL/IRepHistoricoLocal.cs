using Modelo.Proxy;
using System.Collections.Generic;

namespace DAL
{
    public interface IRepHistoricoLocal : DAL.IDAL
    {
        Modelo.RepHistoricoLocal LoadObject(int id);

        List<Modelo.RepHistoricoLocal> LoadPorRep(int idRep);

        Modelo.RepHistoricoLocal GetUltimoRepHistLocal(int idRep);

        List<pxyRepHistoricoLocalAgrupado> RepHistoricoLocalAgrupado();

        List<Modelo.RepHistoricoLocal> GetAllGrid(int id);
    }
}
