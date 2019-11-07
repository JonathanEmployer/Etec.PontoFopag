using System.Collections.Generic;

namespace DAL
{
    public interface IEmpresaTermoUso : DAL.IDAL
    {
        Modelo.EmpresaTermoUso LoadObject(int id);
        List<Modelo.EmpresaTermoUso> GetAllList();
        List<Modelo.EmpresaTermoUso> LoadObjectsByIdsEmpresa(List<int> idsEmpresa);
        void DeleteByIdsEmpresas(List<int> idsEmpresas);
    }
}

