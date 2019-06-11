using BLL_N.IntegracaoTerceiro;
using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class RelClassHorasExtrasController : ApiController
    {
        [HttpPost]
        [Authorize]
        [TratamentoDeErro]
        public HttpResponseMessage RelClassHorasExtras(RelClassHorasExtrasParam parametros)
        {

            RetornoErro retErro = new RetornoErro();
            try
            {
                Modelo.UsuarioPontoWeb userPW = MetodosAuxiliares.UsuarioPontoWeb();
                if (ModelState.IsValid)
                {
                    BLL_N.Relatorios.RelClassHorasExtras bllClassificacaoHorasExtras = new BLL_N.Relatorios.RelClassHorasExtras(userPW);
                    List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> listRel = bllClassificacaoHorasExtras.GetDadosRelatorio(parametros.CPFs, DateTime.Parse(parametros.InicioPeriodo), DateTime.Parse(parametros.FimPeriodo), parametros.TipoSelecao).ToList();
                    PxyFileResult arquivo = bllClassificacaoHorasExtras.GerarRelatorio(listRel, parametros.Formato);

                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new ByteArrayContent(arquivo.Arquivo);
                    response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = arquivo.FileName;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(arquivo.MimeType);

                    return response;
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