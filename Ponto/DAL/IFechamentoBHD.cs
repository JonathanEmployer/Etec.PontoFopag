using System;
using System.Data;
using System.Collections.Generic;
namespace DAL
{
    public interface IFechamentoBHD : DAL.IDAL
    {
        Modelo.FechamentoBHD LoadObject(int id);
        void Incluir(List<Modelo.FechamentoBHD> lista);
        DataTable GetFuncionariosFechamento(int pIdFechamentoBH);
        void SalvaLista(List<string> pLstStrFechamentoBHD);
        string MontaStringInsert(Modelo.FechamentoBHD pObjFechamentoBHD);
        string MontaStringUpdate(Modelo.FechamentoBHD pObjFechamentoBHD);

        List<Modelo.FechamentoBHD> getPorEmpresa(int pIdEmpresa);
        List<Modelo.FechamentoBHD> getPorDepartamento(int pIdDepartamento);
        List<Modelo.FechamentoBHD> getPorFuncionario(int pIdFuncionario);
        List<Modelo.FechamentoBHD> getPorListaFuncionario(List<int> pIdFuncionario);
        List<Modelo.FechamentoBHD> getPorFuncao(int pIdFuncao);
        List<Modelo.FechamentoBHD> getPorPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes);
        List<Modelo.FechamentoBHD> GetAllList();
        List<Modelo.Proxy.PxyFechamentoBHD> GetAllGrid(int idFechamentoBH);
        IList<Modelo.FechamentoBHD> GetFechamentoBHDPorIdFechamentoBH(int idFechamentoBH);
    }
}
