using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IJornadaAlternativaFuncionario : DAL.IDAL
    {

        Modelo.JornadaAlternativaFuncionario LoadObject(int id);
        List<Modelo.JornadaAlternativaFuncionario> GetAllList();
        List<Modelo.JornadaAlternativaFuncionario> GetListWhere(string condicao);

        List<Modelo.JornadaAlternativa> ListaJornadaAlternativaFuncionario(int idFuncionario);
        List<Modelo.Funcionario> ListaFuncionariosJornadaAlternativa(int idJornadaAlternativa);
        void IncluirJornadaAlternativaFuncionarioLote(SqlTransaction trans, int idjornadaAlternativa, string idsFuncionarios);
        void ExcluirJornadaAlternativaFuncionarioLote(SqlTransaction trans, int idjornadaAlternativa);

    }
}
