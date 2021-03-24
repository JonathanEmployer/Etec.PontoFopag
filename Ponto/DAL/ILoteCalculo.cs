using System.Collections.Generic;

namespace DAL
{
    public interface ILoteCalculo : DAL.IDAL
    {
        Modelo.LoteCalculo LoadObject(int id);
        List<Modelo.LoteCalculo> GetAllList();
    }
}

