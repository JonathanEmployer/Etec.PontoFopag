using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IParametrosInternos : IDAL
    {
        Modelo.ParametrosInternos LoadObject(int id);
        List<Modelo.ParametrosInternos> GetAllList();
        Modelo.ParametrosInternos LoadFirtObject();
    }
}
