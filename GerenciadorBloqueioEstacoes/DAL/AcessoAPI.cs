using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAL.Crypto;
using Modelo.Acesso;

namespace DAL
{
    public class AcessoAPI : BaseDAL
    {

        private const string ComandoSelect = "select * from dbo.AcessoAPI;";
        private const string ComandoSelectID = "select * from dbo.AcessoAPI where id = @id;";
        private const string ComandoUpdateToken = "update dbo.AcessoAPI set token = @token where id = @id;";

        public Modelo.Acesso.AcessoAPI Carregar()
        {
            using (IDataReader reader = Conexao.ExecutarDataReader(ComandoSelect))
            {
                return reader.Mapear<Modelo.Acesso.AcessoAPI>(Criar);
            }
        }

        public Modelo.Acesso.AcessoAPI Carregar(int idAcesso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = idAcesso },
            };
            using (IDataReader reader = Conexao.ExecutarDataReader(ComandoSelectID, parametros))
            {
                return reader.Mapear<Modelo.Acesso.AcessoAPI>(Criar);
            }
        }

        private Modelo.Acesso.AcessoAPI Criar(IDataReader reader)
        {
            Modelo.Acesso.AcessoAPI saida = new Modelo.Acesso.AcessoAPI();
            saida.ID = Convert.ToInt32(reader["ID"]);
            saida.Senha = new AES().Decrypt(Convert.ToString(reader["Senha"]));
            saida.Timer = Convert.ToInt32(reader["Timer"]);
            saida.Token = Convert.ToString(reader["Token"]);
            saida.Url = Convert.ToString(reader["Url"]);
            saida.Usuario = Convert.ToString(reader["Usuario"]);
            return saida;
        }

        public void AtualizarToken(Modelo.Acesso.AcessoAPI acesso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = acesso.ID },
                new SqlParameter() { ParameterName = "@token", Value = acesso.Token },
            };
            this.Conexao.ExecutarComando(ComandoUpdateToken, parametros);
        }

    }
}
