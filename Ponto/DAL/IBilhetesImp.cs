using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;

namespace DAL
{
    public interface IBilheteSimp : DAL.IDAL
    {
        Modelo.BilhetesImp LoadObject(int id);
        bool PossuiRegistro(DateTime pData, string pHora, string pFunc, string pRelogio);
                
        List<Modelo.BilhetesImp> GetAllList();
        List<Modelo.BilhetesImp> GetBilhetesEspelho(DateTime dataInicial, DateTime dataFinal, string ids, int tipo);
        List<Modelo.BilhetesImp> GetListaNaoImportados();
        List<Modelo.BilhetesImp> GetListaNaoImportadosFunc(string pDsCodigo);
        List<Modelo.BilhetesImp> GetImportadosFunc(int idfuncionario);
        /// <summary>
        /// Retorna uma lista de bilhetes de um período
        /// </summary>
        /// <param name="tipo">Tipo: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função, 4 - Horário, 5 - Todos </param>
        /// <param name="idTipo"></param>
        /// <param name="dataI"></param>
        /// <param name="dataF"></param>
        /// <returns></returns>
        List<Modelo.BilhetesImp> GetImportadosPeriodo(int tipo, int idTipo, DateTime dataI, DateTime dataF);
        List<Modelo.BilhetesImp> GetImportadosPeriodo(List<int> idsFuncionarios, DateTime dataI, DateTime dataF, bool DesconsiderarFechamento);
        List<Modelo.BilhetesImp> GetListaImportar(string pDsCodigo, DateTime? pDataI, DateTime? pDataF);
        List<Modelo.BilhetesImp> GetBilhetesFuncPeriodo(string pDsCodigo, DateTime pDataI, DateTime pDataF);
        List<Modelo.BilhetesImp> LoadManutencaoBilhetes(string pDsCodigoFunc, DateTime data, bool pegaPA);

        int IncluirbilhetesEmLote(List<Modelo.BilhetesImp> pBilhetes);
        int IncluirbilhetesEmLoteWebApi(List<Modelo.BilhetesImp> pBilhetes, string login, string conection, out List<string> dsCodigoFuncsProcessados);
        int Incluir(List<Modelo.BilhetesImp> listaObjeto);
        int Alterar(List<Modelo.BilhetesImp> listaObjeto);
        int Alterar(List<Modelo.BilhetesImp> listaObjeto, string login);
        int Excluir(List<Modelo.BilhetesImp> listaObjeto);

        /// <summary>
        /// Método responsável em retornar um DataTable com todos os bilhetes que não foram importados
        /// </summary>
        /// <param name="pDsCodigo"></param>
        /// <param name="pManutBilhete"></param>
        /// <param name="pDataBilI"></param>
        /// <param name="pDataBilF"></param>
        /// <returns></returns>
        DataTable GetBilhetesImportar(string pDsCodigo, bool pManutBilhete, DateTime? pDataBilI, DateTime? pDataBilF);
        void GetDataBilhetesImportar(string pDsCodigo, bool pManutBilhete, out DateTime? pdatai, out DateTime? pdataf);
        Int64 GetUltimoNSRRep(string pRelogio);
        List<Modelo.Proxy.PxyBilhetesFuncsDoisRegistros> FuncsDoisRegistrosRegistribuirBilhetes(bool importado, List<string> lPis, DateTime datai, DateTime dataf);
        DataTable GetBilhetesPorPIS(List<string> lPIS, DateTime? pDataBilI, DateTime? pDataBilF);
        List<Modelo.BilhetesImp> GetBilhetesFuncPis(List<string> lPIS, DateTime pDataI, DateTime pDataF);
        Modelo.BilhetesImp UltimoBilhetePorRep(string pRelogio);

        DataTable GetIdsBilhetesByIdRegistroPonto(IList<int> IdsRegistrosPonto);

        DataTable GetBilhetesImportarByIDs(List<int> idsBilhetes);

        List<Modelo.BilhetesImp> LoadPorRegistroPonto(List<int> IdsRegistrosPonto);

        Hashtable GetHashPorPISPeriodo(SqlTransaction trans, DateTime pDataI, DateTime pDataF, List<string> lPis);
        List<Modelo.BilhetesImp> LoadObject(List<int> Ids);
        List<Modelo.Proxy.PxyRegistrosValidarPontoExcecao> RegistrosValidarPontoExcecao(List<int> idsFuncs);
    }
}
