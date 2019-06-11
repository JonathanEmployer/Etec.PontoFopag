using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using LModel = cwkWebAPIPontoWeb.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Modelo.Proxy.IntegracaoTerceiro.DB1;
using Modelo.Proxy;
using System.Configuration;


namespace cwkWebAPIPontoWeb.Controllers
{
    public class RelatorioConferenciaHorasController : ExtendedApiController
    {
        /// <summary>
        /// Retorna os dados referente a comparação de horas trabalhadas vs horas apontadas dos funcionários do cliente DB1
        /// </summary>
        /// <param name="rec">
        /// Dados para filtro do relatório
        /// O Relatório pode ser obtido nos seguintes padrões de acordo com o parâmetro TipoRelatorio, sendo: 0 - PDF; 1 - HTML; 2 - Excel; 3 - Json
        /// </param>
        /// <returns>Retorna Relatório ou dados para o relatório de acordo com o parâmetro.</returns>
        [HttpPost]
        [Authorize]
        [TratamentoDeErro]
        [ApiExplorerSettings(IgnoreApi = true)] 
        public HttpResponseMessage  RelatorioConferenciaHoras(RecuperarTriagens rec)
        {
            RetornoErro retErro = new RetornoErro();
            try
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(userPW);

                if (ModelState.IsValid)
                {
                    PxyFileResult arquivo = bllDB1.GetRelatorio(rec, ConfigurationManager.AppSettings["ApiDB1"]);

                    if (arquivo.Erros != null && arquivo.Erros.Count > 0)
                    {
                        retErro.erroGeral = "Erro encontrado, verifique os detalhes dos erro!";
                        retErro.ErrosDetalhados = new List<ErroDetalhe>();
                        foreach (KeyValuePair<string, string> Erro in arquivo.Erros)
                        {
                            retErro.ErrosDetalhados.Add(new ErroDetalhe() { campo = Erro.Key, erro = Erro.Value });
                        }
                    }
                    else
                    {
                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                        response.Content = new ByteArrayContent(arquivo.Arquivo);
                        response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = arquivo.FileName;
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue(arquivo.MimeType);

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                retErro.erroGeral = ex.Message;
            }

            return TrataErroModelState(retErro);
        }

        private HttpResponseMessage TrataErroModelState(RetornoErro retErro)
        {
            List<ErroDetalhe> lErroDet = new List<ErroDetalhe>();
            var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                        .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                        );
            foreach (var item in errorList)
            {
                ErroDetalhe ed = new ErroDetalhe();
                ed.campo = item.Key;
                ed.erro = String.Join(", ", item.Value);
                lErroDet.Add(ed);
            }
            if (retErro.erroGeral == "")
            {
                retErro.erroGeral = "Um ou mais erros encontrados, verifique os detalhes!";
            }
            retErro.ErrosDetalhados = lErroDet;
            return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
        }
     }          
}