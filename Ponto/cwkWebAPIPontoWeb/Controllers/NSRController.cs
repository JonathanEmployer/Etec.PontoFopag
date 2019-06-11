using cwkWebAPIPontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CentralCliente;
using cwkWebAPIPontoWeb.Utils;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NSRController : ApiController
    {
        /// <summary>
        /// Retorna Lista de Rep's com o número do último NSR cadastrado no sistema.
        /// </summary>
        [TratamentoDeErro]
        public IList<PxyNSR> Get(string usuario, [FromUri]List<String> numRelogios)
        {
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();

            IList<PxyNSR> retorno = new List<PxyNSR>();
            string numRelogiosStr = String.Empty;
            string connectionStr = String.Empty;
            Usuario usu = new Usuario();
            try
            {
                usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
                numRelogiosStr = String.Join(",", numRelogios.Select(s => "'" + s + "'"));
                connectionStr = CriptoString.Decrypt(usu.connectionString);
                using (var conn = new SqlConnection(connectionStr))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = @"
                            select r.numrelogio IdentificadorREP, coalesce(bi.nsr, 0) UltimoNSR, 
	                               Convert(datetime,CONVERT(VARCHAR(10), ISNULL(data,GETDATE()),110) + ' '+ isnull(hora,'00:00')) DataUltimoNsr
                              from rep r
                              left join bilhetesimp bi on r.numrelogio = bi.relogio and bi.nsr = (select max(nsr) from bilhetesimp t where r.numrelogio = t.relogio)";
                        cmd.CommandText += "where r.numrelogio in (" + numRelogiosStr + ")";
                        using (var reader = cmd.ExecuteReader())
                        {
                            List<PxyNSR> dados = new List<PxyNSR>();
                            dados = MetodosAuxiliares.DataReaderMapToList<PxyNSR>(reader);
                            retorno = dados;
                        }
                    }
                }
                return retorno;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                if (usu.ID == 0)
                    TratamentoDeErro.NaoEncontrado("Usuário não encontrado");
                else if (((numRelogios != null) && (numRelogios.Count == 0)) || 
                    (numRelogios == null))
                    TratamentoDeErro.NaoEncontrado("Relógios não encontrados");
                throw ex;
            }
        }
    }
}
