using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;


namespace DAL
{
    public interface IHorarioDinamicoCicloSequencia : DAL.IDAL
    {
        Modelo.HorarioDinamicoCicloSequencia LoadObject(int id);
        List<Modelo.HorarioDinamicoCicloSequencia> GetAllListByHorarioDinamicoCiclo(int idHorarioDinamicoCiclo);
        List<Modelo.HorarioDinamicoCicloSequencia> GetAllListByHorarioDinamicoCiclo(List<int> idHorarioDinamicoCiclo);
    }
}
