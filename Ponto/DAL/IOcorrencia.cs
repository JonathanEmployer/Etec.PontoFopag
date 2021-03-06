using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace DAL
{
    public interface IOcorrencia : DAL.IDAL
    {
        Modelo.Ocorrencia LoadObject(int id);
        List<Modelo.Ocorrencia> GetAllList(bool validaPermissaoUser);
        List<Modelo.Ocorrencia> GetAllListConsultaEvento(bool validaPermissaoUser);
        List<Modelo.Ocorrencia> GetAllPorExibePaineldoRH();
        List<Modelo.Ocorrencia> GetAllPorExibePainelRHPorEmpresa(int idEmpresa);
        List<Modelo.Ocorrencia> GetAllListPorIds(List<int> ids);
        int? getOcorrenciaNome(string pDescricao);
        Hashtable GetHashIdDescricao();
        Modelo.Ocorrencia LoadObjectByCodigo(int pCodigo, bool validaPermissaoUser);
        List<Modelo.Proxy.pxyOcorrenciaEvento> GetAllOcorrenciaEventoList();
        int? GetIdPorIdIntegracao(int idIntegracao);
        List<Modelo.Ocorrencia> GetAllPorExibePainelRHPorFuncionario(int idFuncionario);
    }
}
