using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IEnvioConfiguracoesDataHora : DAL.IDAL
    {
        Modelo.EnvioConfiguracoesDataHora LoadObject(int id);

        Modelo.EnvioConfiguracoesDataHora LoadObjectByID(int id);

        IList<Modelo.EnvioConfiguracoesDataHora> LoadListByIDRep(int idRep);
    }
}
