using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IHorarioPHExtra : DAL.IDAL
    {
        Modelo.HorarioPHExtra LoadObject(int id);
        List<Modelo.HorarioPHExtra> LoadPorHorario(int idHorario);
        DataTable GetPercentualHoraExtra(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo);
        DataTable GetPercentualHoraExtraDepartamento(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo);
        List<Modelo.HorarioPHExtra> GetAllList();
    }
}
