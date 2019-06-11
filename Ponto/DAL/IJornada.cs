using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    
    public interface IJornada : DAL.IDAL
    {
        Modelo.Jornada LoadObject(int id);
        List<Modelo.Jornada> GetAllList();
        List<Modelo.Jornada> GetAllList(List<int> idsJornadas);
        bool JornadaExiste(Modelo.Jornada objJornada);
        List<Modelo.Jornada> getTodosHorariosDaEmpresa(int pIdEmpresa);
        Modelo.Jornada LoadObjectCodigo(int codigo);
        List<Modelo.FechamentoPonto> FechamentoPontoJornada(int id);

        List<Modelo.Proxy.PxyIdPeriodo> GetFuncionariosRecalculo(int id);
    }
}
