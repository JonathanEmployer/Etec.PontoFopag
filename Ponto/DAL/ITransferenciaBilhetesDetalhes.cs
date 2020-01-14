using System.Collections.Generic;

namespace DAL
{
    public interface ITransferenciaBilhetesDetalhes : DAL.IDAL
    {
        Modelo.TransferenciaBilhetesDetalhes LoadObject(int id);
        List<Modelo.TransferenciaBilhetesDetalhes> GetAllList();
        
        List<Modelo.TransferenciaBilhetesDetalhes> GetAllListByTransferenciaBilhetes(int idTransferenciaBilhetes);
    }
}

