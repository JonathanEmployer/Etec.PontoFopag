using System;
using System.Data;
using System.Collections.Generic;
using Modelo;

namespace DAL
{

    public interface IAfastamento : DAL.IDAL
    {
        Modelo.Afastamento LoadObject(int id);
        bool PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario);
        int VerificaExiste(int id, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao);
        DataTable GetPorAfastamentoRel(DateTime pDataInicial, DateTime pDataFinal, string pEmpresas, string pDepartamentos, string pFuncionarios, int Tipo);
        DataTable GetAfastamentoPorOcorrenciaRel(string pEmpresas, string pDepartamentos, string pFuncionarios, int Tipo, int idOcorrencia);
        List<Modelo.Afastamento> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal);
        void Incluir(List<Modelo.Afastamento> afastamentos);
        List<Modelo.Afastamento> GetParaExportacaoFolha(DateTime dataI, DateTime dataF, string idsOcorrencias, bool considerarAbsenteismo, List<int> IdsFuncs);
        DataTable GetParaRelatorioAbono(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento, string pIdsOcorrenciasSelecionados);
        List<Modelo.Afastamento> GetAllList();
        int? GetIdPorIdIntegracao(string idIntegracao);
        List<Modelo.FechamentoPonto> FechamentoPontoAfastamento(int idAfastamento);
        IList<Modelo.Proxy.pxyAbonosPorMarcacao> GetAbonosPorMarcacoes(IList<int> idFuncionarios, DateTime dataIni, DateTime dataFin);
        int? GetIdAfastamentoPorIdMarcacao(int IdMarcacao);
        List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(int idFuncionario, DateTime pDataInicial, DateTime pDataFinal, bool apenasFerias);
        List<Modelo.Afastamento> GetAfastamentoFuncionarioPeriodo(List<int> idFuncionario, DateTime pDataInicial, DateTime pDataFinal, bool apenasFerias);
    }
}
