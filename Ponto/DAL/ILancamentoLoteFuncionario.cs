using Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface ILancamentoLoteFuncionario : DAL.IDAL
    {
        Modelo.LancamentoLoteFuncionario LoadObject(int id);
        List<Modelo.LancamentoLoteFuncionario> GetAllList();
        List<Modelo.LancamentoLoteFuncionario> GetListWhere(string condicao);
        void ExcluirFuncionariosDataTipo(SqlTransaction trans, List<int> idsFuncionarios, DateTime dataInicial, DateTime dataFinal, TipoLancamento tpLancamento);
    }
}
