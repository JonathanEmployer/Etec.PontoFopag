using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IHorarioAItinere : DAL.IDAL
    {
        Modelo.HorarioAItinere LoadObject(int id);
        List<Modelo.HorarioAItinere> LoadPorHorario(int idHorario);
        //DataTable GetPercentualHoraExtra(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo);
        //DataTable GetPercentualHoraExtraDepartamento(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo);
        List<Modelo.HorarioAItinere> GetAllList();
    }
}