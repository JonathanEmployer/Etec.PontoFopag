using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using LModel = cwkWebAPIPontoWeb.Models;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// Método que retorna a lista de classificacoes
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ClassificacaoController : ApiController
    {
        /// <summary>
        /// Método responsável por retornar a lista de Ocorrências
        /// </summary>
        /// <returns>Lista de Ocorrências
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage Classificacao()
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Classificacao bllClass = new BLL.Classificacao(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    List<Modelo.Classificacao> listaClassificacao = new List<Modelo.Classificacao>();

                    listaClassificacao = bllClass.GetAllPorExibePaineldoRH();
                    return Request.CreateResponse(HttpStatusCode.OK, listaClassificacao);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                }
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