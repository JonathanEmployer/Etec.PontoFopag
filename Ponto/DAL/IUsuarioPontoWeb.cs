using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modelo;

namespace DAL
{
    public interface IUsuarioPontoWeb : DAL.IcwkDAL
    {
        List<UsuarioPontoWeb> GetAllList();
        
        List<UsuarioPontoWeb> GetAllListWeb();

        int GetIdAdmin();
        UsuarioPontoWeb LoadObject(int id);
        UsuarioPontoWeb LoadObjectLogin(string pLogin);

        UsuarioPontoWeb LoadObjectByCodigo(int codigo);
    }
}
