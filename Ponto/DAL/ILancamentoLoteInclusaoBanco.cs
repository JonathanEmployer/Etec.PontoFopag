using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ILancamentoLoteInclusaoBanco : DAL.IDAL
    {
        Modelo.LancamentoLoteInclusaoBanco LoadObject(int id);
        List<Modelo.LancamentoLoteInclusaoBanco> GetAllList();
        Modelo.LancamentoLoteInclusaoBanco LoadByIdLote(int idLote);
    }
}
