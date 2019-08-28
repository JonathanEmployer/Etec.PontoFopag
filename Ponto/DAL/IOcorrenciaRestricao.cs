using System.Collections.Generic;

namespace DAL
{
    public interface IOcorrenciaRestricao : DAL.IDAL
    {
        Modelo.OcorrenciaRestricao LoadObject(int id);
        List<Modelo.OcorrenciaRestricao> GetAllList();
        List<Modelo.OcorrenciaRestricao> GetAllListByOcorrencias(List<int> idsOcorrencia); 
    }
}

