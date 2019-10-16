using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IContrato : IDAL
    {
        Contrato LoadObject(int id);
        List<Contrato> GetAllList();
        Contrato LoadPorCodigo(int codigo);
        List<Modelo.Contrato> GetAllListPorEmpresa(int idEmpresa);
        List<Modelo.Contrato> GetAllPorUsuario(int idCw_Usuario);
        int? GetIdPorIdIntegracao(int idIntegracao);
        Modelo.PeriodoFechamento PeriodoFechamento(int idContrato);
        Modelo.PeriodoFechamento PeriodoFechamentoPorCodigo(int codigoContrato);
        List<Modelo.Contrato> ContratosPorFuncionario(int idFuncionario);
        bool ValidaContratoCodigo(int codcontrato, int idempresa);
    }
}
