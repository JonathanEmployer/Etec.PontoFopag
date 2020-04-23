using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace DAL
{
    public interface IFechamentoBH : DAL.IDAL
    {
        Modelo.FechamentoBH LoadObject(int id);
        List<Modelo.FechamentoBH> GetAllList();
        List<int> GetIds();
        Hashtable GetHashCodigoId();

        void ClearFechamentoBH(int pIdFechamento);

        DataTable getTotaisFuncionarios(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF);
        Hashtable getSaldoAnterior(int? pTipo, int pIdentificacao);
        bool VerificaSeExisteFechamento(int pCodigo);
        List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs, bool ValidaPermissao);
        List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs);
        List<Modelo.FechamentoBH> GetByIdBancoHoras(int idBancoHoras);
    }
}
