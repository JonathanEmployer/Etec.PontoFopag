using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface ITratamentomarcacao : DAL.IDAL
    {
        Modelo.Tratamentomarcacao LoadObject(int id);
        List<Modelo.Tratamentomarcacao> LoadPorMarcacao(int idMarcacao);
        List<Modelo.Tratamentomarcacao> GetAllList();
        List<Modelo.Tratamentomarcacao> LoadPorPeriodo(DateTime pdataInicial, DateTime pDataFinal);

        string MontaStringInsert(Modelo.Tratamentomarcacao pObjTratamentoMarcacao, bool pUserControl);
        string MontaStringInsert(DataRow pRowTratamentoMarcacao, bool pUserControl);

    }
}
