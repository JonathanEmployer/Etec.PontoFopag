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
    public class LogScript : BaseDAL
    {

        private const string ComandoSelect = "select * from dbo.LogScript;";
        private const string ComandoSelectID = "select * from dbo.LogScript where id = @id;";
        #region Comando de Insert
        private const string ComandoInsert = @" INSERT INTO dbo.LogScript
                                                    (
                                                        Bloqueado,
                                                        Mensagem,
                                                        Liberacao,
                                                        Usuario,
                                                        RegraBloqueio,
                                                        FlagBloqueadoGestor,
                                                        ExpiracaoFlagGestor,
                                                        MensagemFlagGestor,
                                                        AlertaEnviado
                                                    )
                                                    VALUES
                                                    (   
                                                        @Bloqueado,
                                                        @Mensagem,
                                                        @Liberacao,
                                                        @Usuario,
                                                        @RegraBloqueio,
                                                        @FlagBloqueadoGestor,
                                                        @ExpiracaoFlagGestor,
                                                        @MensagemFlagGestor,
                                                        @AlertaEnviado
                                                    )";
        #endregion

        public void Inserir(Modelo.RegraBloqueio.LogScript logScript)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@bloqueado", Value = logScript.Bloqueado },
                new SqlParameter() { ParameterName = "@mensagem", Value = string.IsNullOrEmpty(logScript.Mensagem) ? (object)DBNull.Value : logScript.Mensagem },
                new SqlParameter() { ParameterName = "@liberacao", Value = !logScript.Liberacao.HasValue ? (object)DBNull.Value : logScript.Liberacao },
                new SqlParameter() { ParameterName = "@usuario", Value = string.IsNullOrEmpty(logScript.Usuario) ? (object)DBNull.Value : logScript.Usuario },
                new SqlParameter() { ParameterName = "@regrabloqueio", Value = !logScript.RegraBloqueio.HasValue ? (object)DBNull.Value : logScript.RegraBloqueio },
                new SqlParameter() { ParameterName = "@FlagBloqueadoGestor", Value = !logScript.FlagBloqueadoGestor.HasValue ? (object)DBNull.Value : logScript.FlagBloqueadoGestor},
                new SqlParameter() { ParameterName = "@ExpiracaoFlagGestor", Value = !logScript.ExpiracaoFlagGestor.HasValue ? (object)DBNull.Value : logScript.ExpiracaoFlagGestor },
                new SqlParameter() { ParameterName = "@MensagemFlagGestor", Value = string.IsNullOrEmpty(logScript.MensagemFlagGestor) ? (object)DBNull.Value : logScript.MensagemFlagGestor },
                new SqlParameter() { ParameterName = "@AlertaEnviado", Value = !logScript.AlertaEnviado.HasValue ? (object)DBNull.Value : logScript.AlertaEnviado },
            };
            Conexao.ExecutarComando(ComandoInsert, parametros);
        }

        public List<Modelo.RegraBloqueio.LogScript> Carregar()
        {
            using (IDataReader reader = Conexao.ExecutarDataReader(ComandoSelect))
            {
                return reader.MapearTodos<Modelo.RegraBloqueio.LogScript>(Criar).ToList();
            }
        }

        public Modelo.RegraBloqueio.LogScript Carregar(int idAcesso)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = idAcesso },
            };
            using (IDataReader reader = Conexao.ExecutarDataReader(ComandoSelectID, parametros))
            {
                return reader.Mapear<Modelo.RegraBloqueio.LogScript>(Criar);
            }
        }

        private Modelo.RegraBloqueio.LogScript Criar(IDataReader dr)
        {
            DateTime dateTime;
            int integer;
            bool flag;

            Modelo.RegraBloqueio.LogScript saida = new Modelo.RegraBloqueio.LogScript();
            saida.Id = Convert.ToInt32(dr["ID"]);
            saida.DataHora = Convert.ToDateTime(dr["DataHora"]);
            saida.Usuario = Convert.ToString(dr["Usuario"]);
            saida.Bloqueado = Convert.ToBoolean(dr["Bloqueado"]);
            saida.Mensagem = Convert.ToString(dr["Mensagem"]);
            saida.Liberacao = (DateTime.TryParse(Convert.ToString(dr["liberacao"]), out dateTime)) ? dateTime : (DateTime?)null;
            saida.RegraBloqueio = (int.TryParse(Convert.ToString(dr["RegraBloqueio"]), out integer)) ? integer : (int?)null;
            saida.FlagBloqueadoGestor = (bool.TryParse(Convert.ToString(dr["FlagBloqueadoGestor"]), out flag)) ? flag : (bool?)null;
            saida.ExpiracaoFlagGestor = (DateTime.TryParse(Convert.ToString(dr["ExpiracaoFlagGestor"]), out dateTime)) ? dateTime : (DateTime?)null;
            saida.MensagemFlagGestor = Convert.ToString(dr["MensagemFlagGestor"]);
            saida.DescricaoRegra = ConverterRegra(saida.RegraBloqueio);
            saida.AlertaEnviado = (DateTime.TryParse(Convert.ToString(dr["AlertaEnviado"]), out dateTime)) ? dateTime : (DateTime?)null;
            return saida;
        }
    }
}
