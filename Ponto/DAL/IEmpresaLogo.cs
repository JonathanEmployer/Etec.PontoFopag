using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IEmpresaLogo : DAL.IDAL
    {
        Modelo.EmpresaLogo LoadObject(int id);
        List<Modelo.EmpresaLogo> GetAllListPorEmpresa(int IDEmpresa);

       
    }
}
