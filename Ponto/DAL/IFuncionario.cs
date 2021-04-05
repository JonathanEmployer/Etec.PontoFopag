using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using Modelo;
using Modelo.Proxy;
using static Modelo.Enumeradores;

namespace DAL
{
    public interface IFuncionario : DAL.IDAL
    {
        void Incluir(List<Modelo.Funcionario> funcionarios, bool salvarHistorico);
        Modelo.Funcionario LoadObject(int id);
        Modelo.Funcionario LoadPorCPF(string CPF);
        Modelo.Funcionario LoadAtivoPorCPF(string CPF);
        List<Modelo.Funcionario> LoadAtivoPorListCPF(List<string> CPFs);
        DataTable GetAll(bool pegaTodos);
        DataTable GetOrdenadoPorNomeRel(string pInicial, string pFinal, string pEmpresas);
        DataTable GetOrdenadoPorCodigoRel(string pInicial, string pFinal, string pEmpresas);
        DataTable GetOrdenadoPorCodigoRel(List<int> idsFuncs);
        DataTable GetPorDepartamentoRel(string pDepartamentos);
        DataTable GetPorDepartamentoRel(List<int> idsFuncs);
        DataTable GetPorDepartamento(string pDepartamentos);
        DataTable GetPorEmpresa(string pEmpresas);
        DataTable GetPorHorarioRel(string pHorarios, string pEmpresas);
        DataTable GetPorHorarioRel(List<int> idsFuncs);
        DataTable GetRelatorio(string pEmpresas);
        DataTable GetRelatorio(List<int> idsFuncs);
        DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas);
        DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs);
        DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas);
        DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs);
        DataTable GetAtivosInativosRel(bool pAtivo, string pEmpresas);
        DataTable GetAtivosInativosRel(bool pAtivo, List<int> idsFuncs);
        DataTable GetParaProvisorio();
        DataTable GetListaPresenca(DateTime dataInicial, int tipo, string empresas, string departamentos, string funcionarios);
        DataTable GetExcluidos();
        DataTable GetPorDepartamento(int pIDEmpresa, int pIDDepartamento, bool pPegarInativos);
        DataTable GetPorEmpresa(int pIDEmpresa, bool pPegaInativos);
        IList<Modelo.Funcionario> GetPorEmpresaList(string pEmpresas);
        DataTable GetFuncionariosAtivos();
        DataTable GetParaDSR(int? pTipo, int pIdentificacao);
        DataTable GetRelatorioAbsenteismo(int tipo, string empresas, string departamentos, string funcionarios);

        int GetNumFuncionarios();

        bool MudaCodigoFuncionario(int pFuncionarioID, string pCodigoNovo, DateTime pData);
        bool VerificaCodigoDuplicado(string pCodigo);

        Modelo.Funcionario RetornaFuncDsCodigo(string pCodigo);
        Modelo.Funcionario RetornaFuncPis(int idFuncionario, string pis);

        List<Modelo.Funcionario> GetAllList(bool pegaInativos, bool pegaExcluidos);
        List<Modelo.Funcionario> GetAllListLike(bool pegaInativos, bool pegaExcluidos, string nome);
        List<Modelo.Funcionario> GetAllListByIds(string funcionarios);
        List<Modelo.Funcionario> getLista(int pempresa);
        List<Modelo.Funcionario> getLista(int pempresa, int pdepartamento);
        List<Modelo.Funcionario> GetPorDepartamentoList(int pIDDepartamento);
        List<Modelo.Funcionario> GetPorFuncaoList(int pIdFuncao);
        List<Modelo.Funcionario> GetPorHorario(int pIdHorario);
        List<Modelo.Funcionario> GetTabelaMarcacao(int tipo, int identificacao, string consultaNomeFuncionario);

        List<int> GetIds();
        Hashtable GetHashCodigoId();
        Hashtable GetHashCodigoFunc();

        DataTable GetPisCodigo(List<string> pis);
        DataTable GetPisCodigo(bool webApi);
        DataTable GetPisCodigo();
        Hashtable GetHashIdFunc();

        string GetDsCodigo(string pPis);
        int GetIdDsCodigo(string pDsCodigo);
        int GetIdDsCodigoProximidade(string pDsCodigo);
        Modelo.Funcionario LoadObjectByCodigo(int codigo);

        List<PxyFuncionarioExcluidoGrid> GetExcluidosList();
        List<Modelo.Funcionario> GetAllListPorContrato(int idContrato);
        List<Modelo.Funcionario> GetAllListContratos();
        IList<Modelo.Proxy.pxyFuncionarioRelatorio> GetRelFuncionariosRelatorios(string filtro);
        IList<Modelo.Funcionario> GetFuncionariosPorIds(string pIDs);
        int GetIdporIdIntegracao(int? IdIntegracao);
        List<Modelo.FechamentoPonto> FechamentoPontoFuncionario(List<int> ids);
        void AtualizaHorariosFuncionariosMudanca(List<int> idsFuncionarios);
        void AtualizaIdIntegracaoPainel(Modelo.Funcionario funcionarioIdIntegracaoPnl);
        List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos);
        List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos, IList<int> idsFuncs);
        List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos);
        List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos, IList<int> idsFuncs);

        Funcionario GetFuncionarioPorCpfeMatricula(Int64 cpf, string matricula);
        Funcionario GetFuncionarioPorMatricula(string matricula);

        List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(int idDep, int idCont, int idEmp);
        List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp);
        List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido);
        IList<Modelo.Proxy.PxyFuncionarioCabecalhoRel> GetFuncionariosCabecalhoRel(IList<int> IdFuncs);
        List<Modelo.Funcionario> GetAllListPorPis(List<string> lPis);
        List<int> GetAllListPorCPF(List<string> lCPF);
        List<Modelo.Proxy.pxyFuncionarioGrid> GetAllGrid();
        List<Modelo.Proxy.pxyFuncionarioGrid> GetAllGrid(int flag);
        DataTable GetOrdenadoPorNomeRel(List<int> idsFuncs);
        DataTable GetRelogioPorNomeRel(List<int> idsFuncs);
        List<Modelo.Funcionario> GetPorHorarioVigencia(int idHorario);
        List<int> GetIdsFuncsAtivos(string condicao);
        Modelo.Funcionario LoadObjectByPis(string PIS);
        List<Modelo.Funcionario> RetornaFuncDsCodigos(List<String> pCodigo);
        List<Modelo.Funcionario> GetAllPisDuplicados(List<string> lPis);
        List<Modelo.Proxy.PxyFuncionarioDiaUtil> GetDiaUtilFuncionario(List<int> idsFuncs, DateTime dataIni, DateTime dataFin);
        DataTable CarregarTodosParaAPI();
        DataTable CarregarTodosParaAPI(Int16 Ativo, Int16 Excluido);
        DataTable CarregarHorarioMarcacao(int idMarcacao, int diaSemana);
        List<Modelo.Funcionario> GetAllFuncsListPorCPF(List<string> lCPF);
        List<Modelo.Funcionario> GetProximoOuAnterior(int tipo, int identificacao, int qtdRegistros, string nomeFuncionario, int tipoOrdenacao, int proximoOuAnterior);
        /// <summary>
        /// Retorna uma lista de ids de acordo com uma lista de dscodigos
        /// </summary>
        /// <param name="lDsCodigos">Lista de DsCodigos</param>
        /// <returns>Retorna lista de IDs</returns>
        List<int> GetIDsByDsCodigos(List<string> lDsCodigos);

        /// <summary>
        /// Retorna os ids dos funcionário relacionados a o tipo passado como parâmetro
        /// </summary>
        /// <param name="pTipo">0:Empresa, 1:Departamento, 2:Funcionário,3:Função,4:Horário</param>
        /// <param name="pIdentificacao">Id do registro passado no tipo</param>
        /// <param name="pegaExcluidos">Indica se deseja retornar os funcionário excluídos</param>
        /// <param name="pegaInativos">Indica se deseja retornar os funcionários inativos</param>
        /// <returns>Retorna a lista dos ids dos funcionários</returns>
        List<int> GetIDsByTipo(int? pTipo, List<int> pIdentificacao, bool pegaExcluidos, bool pegaInativos);
        DataTable GetPeriodoFechamentoPonto(List<int> idsFuncs);

        List<int> GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(int idFuncao, int idDep, int idCont, int idEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido);
        List<PxyUltimoFechamentoPonto> GetUltimoFechamentoPontoFuncionarios(List<int> idsFuncs);
        List<int> IdsFuncPeriodoContratado(TipoFiltroFuncionario tipo, List<int> idsReg, DateTime dtIni, DateTime dtFin);
        void setFuncionariosEmpresa(int idEmpresa, bool FuncionarioAtivo);
        List<string> GetDsCodigosByIDs(List<int> lIds);
        List<PxyFuncionarioFechamentosPontoEBH> GetFuncionariosComUltimoFechamentosPontoEBH(bool pegaTodos, IList<int> idsFuncs, DateTime dataInicio);

        void DeleteLogicoFuncionariosInativos(int qtdMeses);

    }
}

