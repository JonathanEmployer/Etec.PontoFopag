using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public interface ICw_GrupoAcesso : DAL.IDAL
    {

        Modelo.Cw_GrupoAcesso LoadObject(int id);
        List<Modelo.Cw_GrupoAcesso> GetAllList();
        Modelo.Cw_GrupoAcesso LoadObjectIDCw_Grupo(int id);
    }
}
