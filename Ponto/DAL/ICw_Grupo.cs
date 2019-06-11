using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface ICw_Grupo : DAL.IDAL
    {
        Modelo.Cw_Grupo LoadObject(int id);
        List<Modelo.Cw_Grupo> getListaGrupo();
    }
}
