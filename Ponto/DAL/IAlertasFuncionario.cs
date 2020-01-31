using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IAlertasFuncionario : DAL.IDAL
    {
        Modelo.AlertasFuncionario LoadObject(int id);
        List<Modelo.AlertasFuncionario> GetAllList();
        void IncluirLoteIdsFuncionario(SqlTransaction trans, int idAlerta, List<int> idsFuncs);
        void ExcluirLoteIdsFuncionario(SqlTransaction trans, int idAlerta, List<int> idsFuncs);
        List<Modelo.AlertasFuncionario> GetAllPorAlerta(Int32 idAlerta);
    }
}