using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IListaEventosEvento : DAL.IDAL
    {
        Modelo.ListaEventosEvento LoadObject(int id);
        List<Modelo.ListaEventosEvento> GetAllList();
        void IncluirLoteIdsEvento(SqlTransaction trans, int idListaEventos, List<int> idsEventos);
        void ExcluirLoteIdsEvento(SqlTransaction trans, int idListaEventos, List<int> idsEventos);
        List<Modelo.ListaEventosEvento> GetAllPorListaEventos(Int32 idListaEventos);
    }
}

