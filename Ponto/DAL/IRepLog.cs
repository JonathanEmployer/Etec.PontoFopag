using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public interface IRepLog : DAL.IDAL
    {
        Modelo.RepLog LoadObject(int id);
        List<Modelo.RepLog> GetAllList();
        List<Modelo.RepLog> GetAllListByRep(int idRep);
        void DeletaLogAntigo();
        List<Modelo.Proxy.PxyRepLogAFD> GetRepLogAFD(string lote);
        DataTable GetRepLogAFDResumo(string relogio);
    }
}

