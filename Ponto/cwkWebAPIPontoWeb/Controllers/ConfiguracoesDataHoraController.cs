using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using CentralCliente;
using cwkWebAPIPontoWeb.Utils;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// Controlador para envio de dados de Configurações de Data e Hora para o Rep.
    /// </summary>
    public class ConfiguracoesDataHoraController : ApiController
    {
        /// <summary>
        /// Retorna uma lista de Objetos do tipo EnvioConfiguracoesDataHora com os dados a serem importados pelos relógios.
        /// </summary>
        /// <param name="idRelogio">String com os id's dos relógios</param>
        /// <returns> Retorna uma lista de Objetos do Tipo EnvioConfiguracoesDataHora com a lista de Configurações de data e hora a ser importados.</returns>
        [HttpGet]
        [TratamentoDeErro]
        public List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora> RetornaConfiguracoesDataHora([FromUri]List<String> idRelogio)
        {
            List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora> LConfiguracoesDataHora = new List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>();
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            try
            {
                usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
                string connectionStr = CriptoString.Decrypt(usu.connectionString);
                CarregaConfiguracoesDataHora(idRelogio, connectionStr, ref LConfiguracoesDataHora);
                return LConfiguracoesDataHora;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (usu.ID == 0)
                    TratamentoDeErro.NaoEncontrado("Usuário não encontrado");
                throw ex;
            }
        }

        private static void CarregaConfiguracoesDataHora(List<String> idsRelogios, string connectionStr, ref List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora> dadosConfiguracoesDataHora)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idsRelogios", string.Join(",", idsRelogios));
                    conn.Open();
                    cmd.CommandText = @"select dt.* 
                                          from envioconfiguracoesdatahora dt
                                         inner join rep r on dt.idRelogio = r.id
                                        where r.id in (select * from [dbo].[F_RetornaTabelaLista] (@idsRelogios,','));";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora> dados = new List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>();
                            dados = MetodosAuxiliares.DataReaderMapToList<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>(reader);
                            dadosConfiguracoesDataHora = dados;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deleta uma lista de registros de Configuração de Data e Hora de acordo com os ids passados por parâmetro.
        /// </summary>
        /// <param name="idsDadosImportacao">Passar lista de id's dos registros a serem excluídos</param>
        /// <returns> Retorna um dicionário de chave e valor, 0 = Erro ao excluir, chave 1 = Ok - Registro Excluido, chave 2 = nenhum registro encontrado para exclusão, </returns>
        [HttpDelete]
        [TratamentoDeErro]
        public Dictionary<int, string> DeletaConfiguracoesDataHora([FromUri]List<int> idsConfigs)
        {
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            try
            {
                usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
                string connectionStr = CriptoString.Decrypt(usu.connectionString);
                using (var conn = new SqlConnection(connectionStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Parameters.AddWithValue("@idsConfigs", string.Join(",", idsConfigs));
                        conn.Open();
                        cmd.CommandText = @"delete from envioconfiguracoesdatahora where ID in (select * from [dbo].[F_RetornaTabelaLista] (@idsConfigs,','));";
                        int i = cmd.ExecuteNonQuery();
                        Dictionary<int, string> retorno = new Dictionary<int, string>();
                        if (i < 0)
                        {
                            return new Dictionary<int, string>() { { 0, "Erro ao excluir registro!" } };
                        }
                        else if (i == 0) { return new Dictionary<int, string>() { { 2, "Nenhum Registro Encontrado para Exclusão." } }; }
                    }
                }
                return new Dictionary<int, string>() { { 1, "Registro excluído com sucesso!" } };
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (usu.ID == 0)
                    TratamentoDeErro.NaoEncontrado("Usuário não encontrado");
                return new Dictionary<int, string>() { { 0, "Erro ao excluir registro: " + ex.Message } };
            }
        }
    }
}
