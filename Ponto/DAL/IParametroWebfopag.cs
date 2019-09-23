using System.Collections.Generic;

namespace DAL
{
    public interface IParametroWebfopag : DAL.IDAL
    {
        Modelo.ParametroWebfopag LoadObject(int id);
        List<Modelo.ParametroWebfopag> GetAllList();
    }
}

