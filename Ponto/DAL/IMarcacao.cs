using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;

namespace DAL
{
    public interface IMarcacao : DAL.IDAL
    {
        Modelo.Marcacao LoadObject(int id);        
        
        bool PossuiRegistro(DateTime pDt, int pIdFuncionario);
        Modelo.Marcacao GetPorData(Modelo.Funcionario pFuncionario, DateTime pData);               
                
        DataTable GetParaTotalizaHoras(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        DataTable GetParaTotalizaHorasFuncs(List<int> pIdFuncs, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        DataTable GetParaRelatorioAbstinencia(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal);
        DataTable GetParaACJEF(int pIdEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        /// <summary>
        /// Retorna um DataTable para o relatório de ocorrência
        /// </summary>
        /// <param name="pTipo">0 = Empresa; 1 = Departamento; 2 = Individual</param>
        /// <param name="pIdentificacao">Id do tipo</param>
        /// <param name="pDataI">Data Inicial da Consulta</param>
        /// <param name="pDataF">Data Final da Consulta</param>
        /// <returns>DataTable com as informações do período consultado</returns>
        /// WNO - 17/04/2010
        DataTable GetParaRelatorioOcorrencia(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento);

        List<Modelo.Marcacao> GetListaFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal);
        List<Modelo.Marcacao> GetPorEmpresa(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);        
        List<Modelo.Marcacao> GetPorDepartamento(int pDepartamento, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        List<Modelo.Marcacao> GetPorFuncao(int pIdFuncao, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        List<Modelo.Marcacao> GetPorPeriodo(DateTime pdataInicial, DateTime pDataFinal);
        List<Modelo.Marcacao> GetPorHorario(int pIdHorario, DateTime pdataInicial, DateTime pDataFinal);
        List<Modelo.Marcacao> GetPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        List<Modelo.Marcacao> GetPorFuncionario(int pIdFuncionario);
        List<Modelo.Marcacao> GetPorFuncionarios(List<int> pIdsFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        List<Modelo.Marcacao> GetAllList();
        List<Modelo.Marcacao> GetTratamentosMarcacao(DateTime datainicial, DateTime datafinal);
        List<Modelo.MarcacaoLista> GetPorDepartamentoList(int pIdDepartamento, DateTime pDataFinal, bool PegaInativos);        
        List<Modelo.MarcacaoLista> GetPorEmpresaList(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos);
        List<Modelo.MarcacaoLista> GetPorManutDiariaEmp(int pEmpresa, DateTime pDataInicial, DateTime pDataFinal, bool PegaInativos);
        List<Modelo.MarcacaoLista> GetPorManutDiariaDep(int pIdDepartamento, DateTime pDataInicial, DateTime pDataFinal, bool PegaInativos);
        List<Modelo.MarcacaoLista> GetPorManutDiariaCont(int pIDContrato, DateTime pDataIni, DateTime pDataFin, bool PegaInativos);
        List<Modelo.MarcacaoLista> GetPorDataManutDiaria(DateTime pDataIni, DateTime pDataFin, bool PegaInativos);
        List<Modelo.MarcacaoLista> GetMarcacaoListaPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal);
        List<Modelo.Proxy.pxyMarcacaoMudancaHorario> GetMudancasHorarioExportacao(DateTime dataIni, DateTime dataFim, List<int> idsFuncs);



        List<DateTime> GetDataMarcacoesPeriodo(int pIdFuncionario, DateTime pDataI, DateTime pDataF);

        string MontaUpdateFechamento(int pIdFuncionario, int pIdFechamentoBH, DateTime pDataInicial, DateTime pDataFinal);
        void IncluirMarcacoesEmLote(List<Modelo.Marcacao> marcacaoes);
        void Incluir(List<Modelo.Marcacao> listaObjeto);
        void Alterar(List<Modelo.Marcacao> listaObjeto);
        void Excluir(List<Modelo.Marcacao> listaObjeto);
        void SetaIdCompensadoNulo(int pIdCompensacao);

        int QuantidadeCompensada(int pIdCompensacao);
        int QuantidadeMarcacoes(int pIdFuncionario, DateTime pDataI, DateTime pDataF);
        Dictionary<int, int> QuantidadeMarcacoes(List<int> pIdFuncs, DateTime pDataI, DateTime pDataF);
        void SalvarMarcacoes(List<Modelo.Marcacao> lista);
        void ClearFechamentoBH (int pIdFechamentoBH);

        DateTime? GetUltimaDataFuncionario(int pIdFuncionario);
        DateTime? GetUltimaDataDepartamento(int pIdDepartamento);
        DateTime? GetUltimaDataEmpresa(int pIdEmpresa);
        DateTime? GetUltimaDataFuncao(int pIdFuncao);

        DateTime? GetDataDSRAnterior(int pIdFuncionario, DateTime pData);
        DateTime? GetDataDSRProximo(int pIdFuncionario, DateTime pData);

        
        Hashtable GetMarcDiaFunc(int pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal);

        void MontaMarcFunc(int pIdFuncionario, DateTime pData, ref string comando);

        List<Modelo.Marcacao> GetListaCompesacao(List<DateTime> datas, int tipo, int identificacao);

        void AdicionarFechamentoPonto(SqlTransaction trans, int pIdFechamentoPonto, int pIdFuncionario);
        void AdicionarFechamentoPonto(SqlTransaction trans, int pIdFechamentoPonto);
        void ClearFechamentoPonto(SqlTransaction trans, int pIdFechamentoPonto);
        DataTable GetRelatorioObras(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal, string codsLocalReps);
        List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> GetRelatorioConferenciaHoras(List<String> cpfsFuncionarios, DateTime dataInicial, DateTime DataFinal);
        void AtualizaMudancaHorarioMarcacao(List<int> idsFuncionarios, DateTime dataInicio);
        List<Modelo.Marcacao> GetCartaoPontoV2(List<int> pIdFuncionarios, DateTime pdataInicial, DateTime pDataFinal);
        DataTable GetRelatorioRegistros(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal);
        void AtualizarDataLoginBloqueioEdicaoPnlRh(DateTime dataInicio, DateTime dataFim, int idFunc, string tipoSolicitacao);
        DateTime? GetLastDateMarcacao(int idFunc);
        int GetIdDocumentoWorkflow(int idMarcacao);
        void IncluiUsrDtaConclusaoFluxoPnlRh(int idMarcacao, DateTime dataConclusao, string usrLogin);
        DataTable ConclusoesBloqueioPnlRh(string idsFuncionarios, DateTime dataInicial, DateTime dataFinal, int tipoFiltro);

        void ManipulaDocumentoWorkFlowPnlRH(int idMarcacao, int idDocumentoWorkflow, bool documentoWorkflowAberto);
    }
}
