using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IEquipamentoHomologado : DAL.IDAL
    {
        Modelo.EquipamentoHomologado LoadObject(int id);
        List<Modelo.EquipamentoHomologado> GetAllList();
        Modelo.EquipamentoHomologado LoadByCodigoModelo(string codModelo);
        List<Modelo.EquipamentoHomologado> GetAllListPortaria373();
    }
}
