using System;
using System.Collections;
using System.Collections.Generic;

namespace DAL
{
    public interface IRegistroPonto : DAL.IDAL
    {
        Modelo.RegistroPonto LoadObject(int id);
        List<Modelo.RegistroPonto> GetAllList();
        List<Modelo.RegistroPonto> GetAllListByIds(List<int> ids, List<Modelo.Enumeradores.SituacaoRegistroPonto> situacoes);
        List<Modelo.RegistroPonto> GetAllListBySituacoes(List<Modelo.Enumeradores.SituacaoRegistroPonto> situacoes);

        void SetarSituacaoRegistros(List<int> idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto situacao);
        void SetarJobId(List<int> idsRegistros, string jobId);
        void SetarSituacaoJobIDRegistros(List<int> idsRegistros, Modelo.Enumeradores.SituacaoRegistroPonto situacao, string jobId);
        List<Modelo.RegistroPonto> GetAllListByFuncsData(List<int> idsFuncs, DateTime dataI, DateTime dataF);

        List<Modelo.RegistroPonto> GetAllListByIdsIntegracao(List<string> idsIntegracao);

        void SetarSituacaoRegistrosByLote(List<string> lote, Modelo.Enumeradores.SituacaoRegistroPonto situacao);

        Hashtable GetHashPorPISPeriodo(DateTime pDataI, DateTime pDataF, List<string> lPis);

        Modelo.RegistroPonto GetUltimoRegistroByOrigem(string origemRegistro);

        Dictionary<int, string> GetSituacao(List<int> idsRegistros);

        Dictionary<int, string> GetSituacaoByLote(string lote);
    }
}

