using Modelo;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public interface IJustificativa : DAL.IDAL
    {
        int? GetIdPorCod(int Cod, bool validaPermissaoUser);
        Modelo.Justificativa LoadObject(int id);
        bool BuscaJustificativa(string pNomeDescricao);
        List<Modelo.Justificativa> GetAllList(bool validaPermissaoUser); 
        List<Modelo.Justificativa> GetAllListConsultaEvento(bool validaPermissaoUser);
        List<Modelo.Justificativa> GetAllPorExibePaineldoRH();
        Modelo.Justificativa LoadObjectByCodigo(int pCodigo);
        int GetIdPorIdIntegracao(int IdIntegracao);
        Modelo.Justificativa LoadObjectByDescricao(string descricao);
        Modelo.Justificativa LoadObjectParaColetor();
        List<Modelo.Justificativa> GetAllListPorIds(List<int> ids);
        List<Modelo.Justificativa> GetAllPorExibePainelRHPorFuncionario(int idFuncionario);
    }
}
