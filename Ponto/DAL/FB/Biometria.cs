using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.FB
{
    public class Biometria : DAL.FB.DALBase, DAL.IBiometria
    {
        public Modelo.Biometria LoadObject(int id)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Biometria> GetAllList()
        {
            throw new NotImplementedException();
        }

        public Modelo.Biometria LoadObjectByCodigo(int idFuncao)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Biometria> LoadPorFuncionario(int codBiometria)
        {
            throw new NotImplementedException();
        }

        protected override bool SetInstance(FirebirdSql.Data.FirebirdClient.FbDataReader dr, Modelo.ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        protected override FirebirdSql.Data.FirebirdClient.FbParameter[] GetParameters()
        {
            throw new NotImplementedException();
        }

        protected override void SetParameters(FirebirdSql.Data.FirebirdClient.FbParameter[] parms, Modelo.ModeloBase obj)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void InserirRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans, SqlConnection con)
        {
            throw new NotImplementedException();
        }

        public static IBiometria GetInstancia { get; set; }
    }
}
