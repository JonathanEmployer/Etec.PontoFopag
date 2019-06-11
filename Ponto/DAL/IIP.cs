using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IIP : DAL.IDAL
    {
        Modelo.IP LoadObject(int id);
        List<Modelo.IP> GetAllList();
        List<Modelo.IP> GetAllListPorEmpresa(int IDEmpresa);
    }
}
