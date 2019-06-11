using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public interface IAFD : DAL.IDAL
    {
        Modelo.AFD LoadObject(int id);
        List<Modelo.AFD> GetAllList();

        DataTable GetAllListByLote(string lote, bool nolock);
        Modelo.AFD GetUltimoRegistroByOrigem(string origemRegistro);
    }
}

