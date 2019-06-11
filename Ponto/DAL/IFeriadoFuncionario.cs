using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IFeriadoFuncionario : DAL.IDAL
    {
        Modelo.FeriadoFuncionario LoadObject(int id);
        List<Modelo.FeriadoFuncionario> GetAllList();
        List<Modelo.FeriadoFuncionario> GetListWhere(string condicao);
        List<Modelo.Feriado> ListaFeriadosFuncionario(int idFuncionario);
        List<Modelo.Funcionario> ListaFuncionariosFeriado(int idFeriado);
        void IncluirFeriadoFuncionarioLote(SqlTransaction trans, int idFeriado, string idsFuncionarios);
        void ExcluirFeriadoFuncionarioLote(SqlTransaction trans, int idFeriado);
    }
}
