using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// Métodos Referentes ao Cadastro de Ocorrências
    /// </summary>
    [Authorize]
    public class OcorrenciaController : ApiController
    {
        /// <summary>
        /// Método responsável por retornar a lista de Ocorrências
        /// </summary>
        /// <returns>Lista de Ocorrências
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage Ocorrencias(int idFuncionario)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Ocorrencia bllOco = new BLL.Ocorrencia(connectionStr);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    List<Models.Ocorrencia> listaOcorrencias = new List<Models.Ocorrencia>();
                    List<Modelo.Ocorrencia> ocorrencias = new List<Modelo.Ocorrencia>();

                    Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(idFuncionario);
                    if (objFuncionario == null)
                        ocorrencias = bllOco.GetAllPorExibePaineldoRH();
                    else
                        ocorrencias = bllOco.GetAllPorExibePainelRHPorEmpresa(objFuncionario.Idempresa);

                    foreach (var item in ocorrencias)
                    {
                        Models.Ocorrencia ocorrencia = new Models.Ocorrencia();
                        ocorrencia.Id = item.Id;
                        ocorrencia.Descricao = item.Descricao;
                        ocorrencia.ObrigarAnexoPainel = item.ObrigarAnexoPainel;
                        ocorrencia.OcorrenciaFerias = item.OcorrenciaFerias;
                        ocorrencia.HorasAbonoPadrao = item.HorasAbonoPadrao;
                        ocorrencia.HorasAbonoPadraoNoturno = item.HorasAbonoPadraoNoturno;
                        listaOcorrencias.Add(ocorrencia);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listaOcorrencias);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                }
            }
            return TrataErroModelState(retErro);
        }

        /// <summary>
        /// Retorna todas as ocorrências cadastradas no pontofopag
        /// </summary>
        /// <returns>Objeto Ocorrências:
        /// public class Ocorrencia
        /// {
        ///     public int Id { get; set; }
        ///     public string Descricao { get; set; }
        ///     public bool ObrigarAnexoPainel { get; set; }
        ///     public bool OcorrenciaFerias { get; internal set; }
        ///     public string HorasAbonoPadrao { get; set; }
        ///     public string HorasAbonoPadraoNoturno { get; set; }
        /// }
        /// </returns>
        [Route("api/TodasOcorrencias")]
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage Ocorrencias()
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Ocorrencia bllOco = new BLL.Ocorrencia(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    List<Models.Ocorrencia> listaOcorrencias = new List<Models.Ocorrencia>();
                    List<Modelo.Ocorrencia> ocorrencias = new List<Modelo.Ocorrencia>();

                        ocorrencias = bllOco.GetAllList(false);

                    foreach (var item in ocorrencias)
                    {
                        Models.Ocorrencia ocorrencia = new Models.Ocorrencia();
                        ocorrencia.Id = item.Id;
                        ocorrencia.Descricao = item.Descricao;
                        ocorrencia.ObrigarAnexoPainel = item.ObrigarAnexoPainel;
                        ocorrencia.OcorrenciaFerias = item.OcorrenciaFerias;
                        ocorrencia.HorasAbonoPadrao = item.HorasAbonoPadrao;
                        ocorrencia.HorasAbonoPadraoNoturno = item.HorasAbonoPadraoNoturno;
                        listaOcorrencias.Add(ocorrencia);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listaOcorrencias);
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