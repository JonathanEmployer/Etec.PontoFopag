using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace DAL
{
    public interface IFechamentoBHDPercentual : DAL.IDAL
    {
        IList<Modelo.FechamentoBHDPercentual> GetFechamentoBHPercentualPorIdFechamentoBHD(int idFechamentoBHD);
        Modelo.FechamentoBHDPercentual LoadObject(int id);
        List<Modelo.FechamentoBHDPercentual> GetAllList();
        List<int> GetIds();
        Hashtable GetHashCodigoId();
        DataTable GetBancoHorasPercentual(DateTime? dataInicial, DateTime dataFinal, int idFuncionario, int considerarUltimoFechamento);
    }
}
