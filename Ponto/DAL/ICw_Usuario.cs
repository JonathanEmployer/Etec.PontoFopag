using System;
using System.Data;

namespace DAL
{
    public interface ICw_Usuario : DAL.IDAL
    {
        Modelo.Cw_Usuario LoadObject(int id);
        Modelo.Cw_Usuario LoadObjectLogin(string pLogin);
    }
}
