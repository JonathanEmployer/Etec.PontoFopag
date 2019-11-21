using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IEmpresa : DAL.IDAL
    {
        Modelo.Empresa LoadObject(int id);
        Modelo.Empresa LoadObjectByCodigo(int codigo);
        Modelo.Empresa LoadObjectByDocumento(Int64 documento);
        List<Modelo.Empresa> GetAllList();
        DataTable GetEmpresaAtestado(int pEmpresa);
        int GetQuantidadeMaximaDeFuncionarios();
        Modelo.Empresa GetEmpresaPrincipal();
        string GetPrimeiroCwk(out string mensagem);
        bool ConsultaBloqueiousuariosEmpresa();
        bool UtilizaControleContratos();
        bool RelatorioAbsenteismoLiberado();
        bool ModuloRefeitorioLiberado();
        List<Modelo.Proxy.pxyEmpresa> GetAllListPxyEmpresa(string filtro);
        int? GetIdporIdIntegracao(int? IdIntegracao);
        Modelo.PeriodoFechamento PeriodoFechamento(int idEmpresa);
        Modelo.PeriodoFechamento PeriodoFechamentoPorCodigo(int codigoEmp);
        List<Modelo.Empresa> GetEmpresaByIds(List<int> ids);
        List<int> GetIdsPorCodigos(List<int> codigos);
        List<int> GetAllIds();
        bool ConsultaUtilizaRegistradorAllEmp();
        List<Modelo.Empresa> GetAllListEmpresa();
    }
}
