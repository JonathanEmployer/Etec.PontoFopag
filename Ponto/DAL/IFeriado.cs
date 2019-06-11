using System;
using System.Data;
using System.Collections.Generic;
using Modelo;

namespace DAL
{
    public interface IFeriado : DAL.IDAL
    {
        Modelo.Feriado LoadObject(int id);
        List<Modelo.Feriado> getFeriado(DateTime pData);
        List<Modelo.Feriado> getFeriado(DateTime pDataI, DateTime pDataF);
        bool BuscaFeriado(string pNomeDescricao);
        List<Modelo.Feriado> GetAllList();
        List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(int idFuncionario, DateTime inicio, DateTime fim);
        List<Modelo.Feriado> GetFeriadosFuncionarioPeriodo(List<int> idsFuncionarios, DateTime inicio, DateTime fim);
        List<Modelo.Feriado> GetIdPorIdIntegracao(int idIntegracao);
    }
}
