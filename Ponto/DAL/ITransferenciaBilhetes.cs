using System.Collections.Generic;

namespace DAL
{
    public interface ITransferenciaBilhetes : DAL.IDAL
    {
        Modelo.TransferenciaBilhetes LoadObject(int id);
        List<Modelo.TransferenciaBilhetes> GetAllList();
        List<Modelo.Proxy.PxyGridTransferenciaBilhetes> GetAllListGrid();
        void TransferirBilhetes(int idTransferenciaBilhetes);
        int CountCampo(string tabela, string campo, int valor, int id);
    }
}

