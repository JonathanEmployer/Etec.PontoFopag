using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public interface IFuncao : DAL.IDAL
    {
        Modelo.Funcao LoadObject(int id);
        bool BuscaFuncao(string pNomeDescricao);
        int? getFuncaoNome(string pNomeDescricao);
        int? GetIdPorCod(int Cod);
        int? GetIdPorIdIntegracao(int? idIntegracao);

        List<Modelo.Funcao> GetAllList();

        Modelo.Funcao LoadObjectByCodigo(int idFuncao);
    }
}
