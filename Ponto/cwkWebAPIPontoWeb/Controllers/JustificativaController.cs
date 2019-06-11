using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;


namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// Métodos Referentes ao Cadastro de Justificativas
    /// </summary>
    [Authorize]
    public class JustificativaController : ApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Justificativa.
        /// </summary>
        /// <param name="justificativa">Json com os dados de justificativa</param>
        /// <returns> Retorna json de justificativa quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Justificativa justificativa)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.ConexaoContexto(this.ActionContext);
            BLL.Justificativa bllJust = new BLL.Justificativa(connectionStr);
            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    int? idjustificativa = bllJust.GetIdPorIdIntegracao(justificativa.IdIntegracao);
                    Modelo.Justificativa DadosAntJust = bllJust.LoadObject(idjustificativa.GetValueOrDefault());
                    Acao acao = new Acao();
                    if (DadosAntJust.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                    {
                        acao = Acao.Alterar;
                    }

                    DadosAntJust.Codigo = justificativa.Codigo;
                    DadosAntJust.Descricao = justificativa.Descricao;
                    DadosAntJust.IdIntegracao = justificativa.IdIntegracao;
                    if (idjustificativa != null)
                    {
                        erros = bllJust.Salvar(acao, DadosAntJust);
                    }

                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, DadosAntJust);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                }
            }
            return TrataErroModelState(retErro);
        }

        /// <summary>
        /// Excluir Justificativa.
        /// </summary>
        /// <param name="IdIntegracao">Id de Integração da Justificativa Cadastrada</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(int IdIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Justificativa bllJust = new BLL.Justificativa(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    int? idJustificativa = bllJust.GetIdPorIdIntegracao(IdIntegracao);
                    Modelo.Justificativa DadosAntJust = bllJust.LoadObject(idJustificativa.GetValueOrDefault());
                    if (DadosAntJust.Id > 0)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllJust.Salvar(Acao.Excluir, DadosAntJust);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, DadosAntJust);
                        }
                    }
                    else
                    {
                        retErro.erroGeral = "Justificativa Não Encontrada";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
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

        private void TrataErros(Dictionary<string, string> erros)
        {
            //Componente Ex:txtCodigo, Nome no modelo onde o erro será adicionado Ex: Codigo
            Dictionary<string, string> ComponenteToModel = new Dictionary<string, string>();
            ComponenteToModel.Add("txtCodigo", "Codigo");
            ComponenteToModel.Add("txtDescricao", "Descricao");
            foreach (var item in ComponenteToModel)
            {
                ErroToModelState(erros, item);
                erros = erros.Where(x => !x.Key.Equals(item.Key)).ToDictionary(x => x.Key, x => x.Value);
            }

            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }

        private void ErroToModelState(Dictionary<string, string> erros, KeyValuePair<string, string> item)
        {
            Dictionary<string, string> erro = erros.Where(x => x.Key.Equals(item.Key)).ToDictionary(x => x.Key, x => x.Value);
            if (erro.Count > 0)
            {
                ModelState.AddModelError(item.Value, string.Join(";", erro.Select(x => x.Value).ToArray()));
            }
        }

        /// <summary>
        /// Método responsável por retornar a lista de justificativas
        /// </summary>
        /// <returns>Lista de justificativas
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage Justificativas()
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Justificativa bllJust = new BLL.Justificativa(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    List<Models.Justificativa> listajustificativas = new List<Models.Justificativa>();
                    List<Modelo.Justificativa> justificativas = new List<Modelo.Justificativa>();

                    justificativas = bllJust.GetAllPorExibePaineldoRH();

                    foreach (var item in justificativas)
                    {
                        Models.Justificativa justificativa = new Models.Justificativa();
                        justificativa.Id = item.Id;
                        justificativa.Codigo = item.Codigo;
                        justificativa.Descricao = item.Descricao;
                        justificativa.IdIntegracao = item.IdIntegracao.GetValueOrDefault();
                        listajustificativas.Add(justificativa);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listajustificativas);
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                }
            }
            return TrataErroModelState(retErro);
        }
    }
}