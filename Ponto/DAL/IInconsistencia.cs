using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace DAL
{
    public interface IInconsistencia : DAL.IDAL
    {
        Modelo.Inconsistencia LoadObject(int id);
        List<Modelo.Inconsistencia> GetAllList();

        //int VerificaExiste(int pId, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao);

        //DataTable LoadRelatorio(DateTime dataInicial, DateTime dataFinal, int tipo, string empresas, string departamentos, string funcionarios);
        //DataTable GetRelatorioResumo(DateTime pDataI, DateTime pDataF, int pTipo, string pEmpresas, string pDepartamentos, string pFuncionarios);
        //DataTable GetRelatorioHorario(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIds);
        //Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF);
        //void getInicioFimBH(int pIdBancoHoras, out DateTime? pDtInicio, out DateTime? pDtFim);
        //Modelo.Inconsistencia LoadPorCodigo(int codigo);

        List<Modelo.Proxy.Relatorios.PxyRelatorioInconsistencias> RelatorioInconsistencias(List<int> idsFuncs, string dataInicial, string dataFinal, List<bool> paramInconsistencia);

        Hashtable GetHashIdDescricao();
        Modelo.Inconsistencia LoadObjectByCodigo(int pCodigo);
    }
}
