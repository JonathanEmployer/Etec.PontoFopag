using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ILancamentoLoteMudancaHorario : DAL.IDAL
    {
        Modelo.LancamentoLoteMudancaHorario LoadObject(int id);
        List<Modelo.LancamentoLoteMudancaHorario> GetAllList();
        Modelo.LancamentoLoteMudancaHorario LoadByIdLote(int idLote);
    }
}
