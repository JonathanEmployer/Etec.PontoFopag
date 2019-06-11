using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IEnvioDadosRepDet : DAL.IDAL
    {
        Modelo.EnvioDadosRepDet LoadObject(int id);
        List<Modelo.EnvioDadosRepDet> getListByRep(int idRep);
        List<Modelo.EnvioDadosRepDet> getByIdEnvioDadosRep(int idEnvioDadosRep);
    }
}
