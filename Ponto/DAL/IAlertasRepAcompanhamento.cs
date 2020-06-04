using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IAlertasRepAcompanhamento : DAL.IDAL
    {
        Modelo.AlertasRepAcompanhamento LoadObject(int id);
        List<Modelo.AlertasRepAcompanhamento> GetAllList();
        void IncluirLoteIdsRep(SqlTransaction trans, int idAlerta, List<int> idsFuncs);
        void ExcluirLoteIdsRep(SqlTransaction trans, int idAlerta, List<int> idsFuncs);
        List<Modelo.AlertasRepAcompanhamento> GetAllPorAlerta(Int32 idAlerta);
    }
}

