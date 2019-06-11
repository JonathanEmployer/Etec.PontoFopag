using RegistradorPontoWeb.Models.Ponto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public class ParametroPontoPainelRH
    {
        SqlConnectionStringBuilder Conexao = new SqlConnectionStringBuilder();
        public ParametroPontoPainelRH(SqlConnectionStringBuilder conn)
        {
            Conexao = conn;
        }

        public ParametroPainelRH GetParametrosPainelRH()
        {
            using (var db = new Models.Ponto.PontofopagEntities(Conexao.DataSource, Conexao.InitialCatalog, Conexao.UserID, Conexao.Password))
	        {
                return db.ParametroPainelRH.FirstOrDefault();
	        }
        }
    }
}