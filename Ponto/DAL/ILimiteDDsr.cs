using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface ILimiteDDsr : IDAL
    {
        LimiteDDsr LoadObject(int id);
        List<LimiteDDsr> GetAllList();
        LimiteDDsr LoadPorCodigo(int codigo);
        List<LimiteDDsr> GetAllListPorHorario(int idHorario);
        List<Modelo.LimiteDDsr> GetAllListPorHorarios(List<int> idsHorario);
    }
}
