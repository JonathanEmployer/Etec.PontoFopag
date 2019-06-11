using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IMudancaHorario : DAL.IDAL
    {
        Modelo.MudancaHorario LoadObject(int id);
        bool MudarHorario(int pTipoMudanca,int pIDFuncao, int pIDEmpresa, int pIDDepartamento, int pIdFuncionario, int pTipoTurno, int pIDHorario, DateTime pData, int? pIdHorarioDinamico, int? cicloSequenciaIndice);
        bool ExcluirMudancao(Modelo.MudancaHorario pMudanca);
        bool VerificaExiste(int pIdFuncionario, DateTime pData);
        DataTable GetPorFuncionario(int pIdFuncionario);
        List<Modelo.MudancaHorario> GetPeriodo(DateTime pDataI, DateTime pDataF, List<int> idsFuncionario);
        DateTime? GetUltimaMudanca(int pIDFuncionario);
        List<Modelo.MudancaHorario> GetAllFuncionarioList(int pIdFuncionario);
        List<Modelo.MudancaHorario> GetAllList();
    }
}
