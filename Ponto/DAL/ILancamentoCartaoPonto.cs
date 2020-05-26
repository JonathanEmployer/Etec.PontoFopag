using System.Collections.Generic;

namespace DAL
{
    public interface ILancamentoCartaoPonto : DAL.IDAL
    {
        Modelo.LancamentoCartaoPonto LoadObject(int id);
        List<Modelo.LancamentoCartaoPonto> GetAllList();
        void IncluirRegistros(List<Modelo.BilhetesImp> bilhetes, List<Modelo.LancamentoCartaoPontoRegistros> registros);
    }
}

