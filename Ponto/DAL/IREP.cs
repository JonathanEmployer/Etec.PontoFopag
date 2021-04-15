using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IREP : DAL.IDAL
    {
        Modelo.REP LoadObject(int id);
        List<Modelo.Proxy.pxyFuncionarioRep> LoadObjectListFuncionariosRep(int id);
        Modelo.REP LoadObjectPorNumRelogio(string numRelogio);
        string GetNumInner(string pNumSerie);
        bool GetCPFCNPJ(string pCPFCNPJ, string pTipo);
        List<Modelo.REP> GetAllList();

        List<Modelo.Proxy.pxyRep> PegaPxysRep();

        Modelo.REP LoadObjectByCodigo(int codigo);

        void SetUltimoNSR(Int32 idrep, Int32 ultimoNsr);

        void SetUltimoNSRComDataIntegracao(Int32 idrep, Int32 ultimoNsr);

        Modelo.REP LoadObjectByNumSerie(string NumSerie);

        void SetUltimaImportacao(string numRelogio, long NSR, DateTime dataUltimaImp);

        List<Modelo.REP> VerificarIpEntreRep(string ip, int id);

        List<Modelo.Proxy.RepSituacao> VerificarSituacaoReps(int TempoSemComunicacao);
        List<Modelo.REP> VerificarSituacaoReps(List<string> numsReps);
        List<Modelo.Proxy.PxyGridRepsPortaria373> GetGridRepsPortaria373();
    }
}
