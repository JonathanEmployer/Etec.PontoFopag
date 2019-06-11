using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;


namespace DAL
{
    public interface IHorarioDinamicoCiclo : DAL.IDAL
    {
        List<Modelo.HorarioDinamicoCiclo> GetAllList();
        List<Modelo.HorarioDinamicoCiclo> GetHorarioDinamicoCiclo(int idhorariodinamico);
        List<Modelo.HorarioDinamicoCiclo> GetHorarioDinamicoCiclo(List<int> idshorariodinamico);
        Modelo.HorarioDinamicoCiclo LoadObject(int id);
    }
}
