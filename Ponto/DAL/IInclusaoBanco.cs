using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IInclusaoBanco : DAL.IDAL
    {
        Modelo.InclusaoBanco LoadObject(int id);
        void getSaldo(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao, out int credito, out int debito);
        int getCreditoPeriodoAtual(int idFuncionario, DateTime dataInicio, DateTime dataFim);

        int getCreditoPeriodoAcumuladoMes(int idFuncionario, DateTime dataInicio, DateTime dataFim);

        int getCreditoPeriodoAcumuladoMesPDia(int idFuncionario, DateTime dataInicio, DateTime dataFim, int diaInt);

        List<Modelo.InclusaoBanco> GetAllList();

        List<Modelo.InclusaoBanco> GetAllListByFuncionarios(List<int> idsFuncs);
    }
}
