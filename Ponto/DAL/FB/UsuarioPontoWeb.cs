using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.FB
{
    public class UsuarioPontoWeb : DAL.SQL.cwkDALBase, DAL.ICw_Usuario
    {
        public static IUsuarioPontoWeb GetInstancia { get; set; }

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override bool SetInstance(System.Data.SqlClient.SqlDataReader dr, Modelo.cwkModeloBase obj)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters(System.Data.SqlClient.SqlParameter[] parms, Modelo.cwkModeloBase obj)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Cw_Usuario> GetAllList()
        {
            throw new NotImplementedException();
        }

        public int GetIdAdmin()
        {
            throw new NotImplementedException();
        }

        public Modelo.Cw_Usuario LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        public Modelo.Cw_Usuario LoadObjectLogin(string pLogin)
        {
            throw new NotImplementedException();
        }

        public Modelo.Cw_Usuario LoadObjectByCodigo(int codigo)
        {
            throw new NotImplementedException();
        }

    }
}
