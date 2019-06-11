using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public interface IFuncionarioHistorico : DAL.IDAL
    {
        Modelo.FuncionarioHistorico LoadObject(int id);

        DataTable LoadRelatorio(DateTime dataInicial, DateTime dataFinal, int tipo, string empresas, string departamentos, string funcionarios);

        List<Modelo.FuncionarioHistorico> LoadPorFuncionario(int idFuncionario);
    }
}
