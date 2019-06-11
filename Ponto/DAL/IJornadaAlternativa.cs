using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IJornadaAlternativa : DAL.IDAL
    {
        Modelo.JornadaAlternativa LoadObject(int id);
        Modelo.JornadaAlternativa PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao);
        Modelo.JornadaAlternativa LoadParaUmaMarcacao(DateTime pData, int tipo, int identificacao);
        List<Modelo.JornadaAlternativa> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal);
        int VerificaExiste(int pId, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao);
        Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, int? pTipo, List<int> pIdentificacoes);
        List<Modelo.JornadaAlternativa> GetAllList(bool loadDiasJA);
        List<Modelo.JornadaAlternativa> GetPeriodoFuncionarios(DateTime pDataInicial, DateTime pDataFinal, List<int> idsFuncs);
    }
}
