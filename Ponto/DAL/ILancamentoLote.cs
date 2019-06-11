using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface ILancamentoLote : DAL.IDAL
    {
        Modelo.LancamentoLote LoadObject(int id);
        List<Modelo.LancamentoLote> GetAllList();
        List<Modelo.LancamentoLote> GetAllListTipoLancamento(List<Modelo.TipoLancamento> TipoLancamento);
        void ExcluiLoteSemFilho(SqlTransaction trans);
    }
}
