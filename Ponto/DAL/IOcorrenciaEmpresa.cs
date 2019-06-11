using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace DAL
{
    public interface IOcorrenciaEmpresa : DAL.IDAL
    {
        Modelo.OcorrenciaEmpresa LoadObject(int id);
        List<Modelo.OcorrenciaEmpresa> GetAllList();
        Modelo.OcorrenciaEmpresa LoadObjectByCodigo(int pCodigo);
        List<Modelo.OcorrenciaEmpresa> GetAllPorExibePainelRHPorEmpresa(int idEmpresa);
        void DeleteAllByIdEmpresa(int idEmpresa);
        void IncluirOcorrenciasEmpresa(List<Modelo.OcorrenciaEmpresa> ocorrenciasEmpresa);
    }
}
