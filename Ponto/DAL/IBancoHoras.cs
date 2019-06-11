using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IBancoHoras : DAL.IDAL
    {
        Modelo.BancoHoras LoadObject(int id);
        List<Modelo.BancoHoras> GetAllList(bool verificaPermissao);

        int VerificaExiste(int pId, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao);

        DataTable LoadRelatorio(DateTime dataInicial, DateTime dataFinal, int tipo, string empresas, string departamentos, string funcionarios);
        DataTable GetRelatorioResumo(DateTime pDataI, DateTime pDataF, int pTipo, string pEmpresas, string pDepartamentos, string pFuncionarios);
        DataTable GetRelatorioHorario(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIds);
        Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF);
        Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, List<int> ids);
        void getInicioFimBH(int pIdBancoHoras, out DateTime? pDtInicio, out DateTime? pDtFim);
        Modelo.BancoHoras LoadPorCodigo(int codigo);
        List<Modelo.Proxy.PxySaldoBancoHoras> SaldoBancoHoras(DateTime dataSaldo, List<int> idsFuncs);
        List<Modelo.Proxy.Relatorios.PxyRelBancoHoras> RelatorioSaldoBancoHoras(string MesInicio, string AnoInicio, string MesFim, string AnoFim, List<int> idsFuncs);
        Modelo.BancoHoras BancoHorasPorFuncionario(DateTime data, int idFuncionario);
        //DataTable GetCreditoDebitoCalculoBanco(DateTime pInicial, DateTime pFinal, List<int> idsFuncs);
        List<Modelo.BancoHoras> GetAllListFuncs(bool verificaPermissao, List<int> idsFuncs);
    }
}
