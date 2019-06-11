using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface ICompensacao : DAL.IDAL
    {
        Modelo.Compensacao LoadObject(int id);

        List<Modelo.Compensacao> getListaCompensacao(DateTime pData);

        List<Modelo.Compensacao> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes);

        DataTable GetTotalCompensado(int pIdCompensacao);

        List<Modelo.Compensacao> GetAllList();
        List<Modelo.Compensacao> GetPeriodoByFuncionario(DateTime pDataInicial, DateTime pDataFinal, List<int> pdIdsFuncs);
    }
}
