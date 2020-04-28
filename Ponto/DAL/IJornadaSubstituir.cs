using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IJornadaSubstituir : DAL.IDAL
    {
        Modelo.JornadaSubstituir LoadObject(int id);
        List<Modelo.JornadaSubstituir> GetAllList();
        List<Modelo.Proxy.PxyJornadaSubstituirFuncionarioPeriodo> GetPxyJornadaSubstituirFuncionarioPeriodo(DateTime dataIni, DateTime dataFim, List<int> idsFuncs);
        List<Modelo.Proxy.PxyJornadaSubstituirCalculo> GetPxyJornadaSubstituirCalculo(DateTime dataIni, DateTime dataFim, List<int> idsFuncs);
    }
}

