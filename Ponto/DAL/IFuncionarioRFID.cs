using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Modelo;

namespace DAL
{
    public interface IFuncionarioRFID : DAL.IDAL
    {
        Modelo.FuncionarioRFID LoadObject(int id);
        List<FuncionarioRFID> GetAllList();
        int? GetIdPorCod(int Cod);
        List<Modelo.FuncionarioRFID> GetAllListByFuncionario(int idFuncionario);
    }
}
