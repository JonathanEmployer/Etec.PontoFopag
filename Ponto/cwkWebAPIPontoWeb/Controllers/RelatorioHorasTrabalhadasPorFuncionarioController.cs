using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using Modelo.Proxy;
using Modelo.Proxy.IntegracaoTerceiro.DB1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class RelatorioHorasTrabalhadasPorFuncionarioController : ExtendedApiController
    {
        /// <summary>
        /// Retorna o total de horas trabalhadas por dia por funcionário
        /// </summary>
        /// <param name="rec"></param>
        /// <returns>
        /// O Relatório é obtido no padrão Json
        /// </returns>
        [HttpPost]
        [Authorize]
        [TratamentoDeErro]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage RelatorioHorasTrabalhadasPorFuncionario(GetHorasTrabalhadas get)
        {
            RetornoErro retErro = new RetornoErro();
            try
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(userPW);

                if (ModelState.IsValid)
                {
                    RecuperarTriagens rec = new RecuperarTriagens() { Cpfs = get.Cpfs, DataInicio = get.DataInicio, DataFinal = get.DataFinal, Sintetico = false, TipoRelatorio = 3};
                    PxyFileResult arquivo = bllDB1.GetRelatorioHorasTrabalhadasPorFuncionario(rec, ConfigurationManager.AppSettings["ApiDB1"]);

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