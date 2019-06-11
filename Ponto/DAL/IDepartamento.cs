using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IDepartamento : DAL.IDAL
    {
        Modelo.Departamento LoadObject(int id);
        DataTable GetPorEmpresa(int IdEmpresa);
        DataTable GetPorEmpresa(string empresas);
        List<Modelo.Departamento> GetAllList();
        List<Modelo.Departamento> GetAllListByIds(List<int> ids);
        List<Modelo.Departamento> GetAllListLike(string desc);
        int? GetIdPorDesc(String Descricao);
        int? GetIdPorCodigo(int Cod);
        int? GetIdPorIdIntegracao(int Cod);
        bool PossuiFuncionarios(int id);
        Modelo.Departamento LoadObjectByCodigo(int codigo);
        Modelo.Departamento GetDepartamentoPadrao(string cnpj);
        List<Modelo.Departamento> LoadPEmpresa (int idEmpresa);
        List<int> GetIdsPorCodigos(List<int> codigos);
    }
}
