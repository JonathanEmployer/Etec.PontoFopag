using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using cwkWebAPIPontoWeb.Utils;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    /// <summary>
    /// Métodos Referentes ao Cadastro de Função
    /// </summary>
    public class FuncaoController : ExtendedApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Função.
        /// </summary>
        /// <param name="funcao">Json com os dados de função</param>
        /// <returns> Retorna json de função quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [ActionName("Cadastrar")]
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Funcao funcao)
        {
            RetornoErro retErro = new RetornoErro();
            BLL.Funcao bllFuncao = new BLL.Funcao(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            
            if (ModelState.IsValid)
            {
                try
                {
                    int? idfuncao = bllFuncao.GetIdPorIdIntegracao(funcao.idIntegracao);
                    Modelo.Funcao DadosAntFunc = bllFuncao.LoadObject(idfuncao.GetValueOrDefault());
                    Acao acao = new Acao();
                    if (DadosAntFunc.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                    {
                        acao = Acao.Alterar;
                    }

                    DadosAntFunc.Codigo = funcao.Codigo;
                    DadosAntFunc.Descricao = funcao.Descricao;
                    DadosAntFunc.idIntegracao = funcao.idIntegracao;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllFuncao.Salvar(acao, DadosAntFunc);
                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, funcao);
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
        /// Cadastrar/Alterar Função apenas pela descrição/nome.
        /// </summary>
        /// <param name="DescricaoFuncao">Descrição/Nome da Função</param>
        /// <returns> Retorna json de função quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [ActionName("CadastrarPorDescricao")]
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage CadastrarPorDescricao(String DescricaoFuncao)
        {
            if (!String.IsNullOrEmpty(DescricaoFuncao))
            {
                RetornoErro retErro = new RetornoErro();
                BLL.Funcao bllFuncao = new BLL.Funcao(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                try
                {
                    Modelo.Funcao DadosAntFunc;
                    Dictionary<string, string> erros;
                    BLLAPI.Funcao.SalvarFuncaoPorDescricao(DescricaoFuncao, bllFuncao, out DadosAntFunc, out erros);
                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                        return TrataErroModelState(retErro);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, DadosAntFunc);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                    return TrataErroModelState(retErro);
                }
            }
            RetornoErro retornaErro = new RetornoErro();
            retornaErro.erroGeral = "Descrição deve ser informada";
            return Request.CreateResponse(HttpStatusCode.BadRequest, retornaErro);
        }

        /// <summary>
        /// Excluir Função.
        /// </summary>
        /// <param name="idIntegracao">Id da integração da Função Cadastrada</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [ActionName("Excluir")]
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(int? idIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            BLL.Funcao bllFuncao = new BLL.Funcao(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            try
            {
                if (ModelState.IsValid)
                {
                    int? idfuncao = bllFuncao.GetIdPorIdIntegracao(idIntegracao);
                    if (idfuncao != null && idfuncao > 0)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        Modelo.Funcao funcao = bllFuncao.LoadObject(idfuncao.GetValueOrDefault());
                        erros = bllFuncao.Salvar(Acao.Excluir, funcao);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        retErro.erroGeral = "Funçao Não Encontrada";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }

                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                retErro.erroGeral += ex.Message;
            }
            return TrataErroModelState(retErro);
        }

        /// <summary>
        /// Excluir Função por Descrição/Nome, quando possuir mais de uma descrição com o mesmo nome cadastrada no pontofopag, o método tentará excluir a primeira descrição encontrada
        /// </summary>
        /// <param name="DescricaoFuncao">Descrição/Nome da Função Cadastrada</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [ActionName("ExcluirPorDescricao")]
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage ExcluirPorDescricao(string DescricaoFuncao)
        {
            if (!String.IsNullOrEmpty(DescricaoFuncao))
            {
                RetornoErro retErro = new RetornoErro();
                BLL.Funcao bllFuncao = new BLL.Funcao(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                try
                {
                    int? idfuncao = bllFuncao.getFuncaoNome(DescricaoFuncao);
                    if (idfuncao != null && idfuncao > 0)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        Modelo.Funcao funcao = bllFuncao.LoadObject(idfuncao.GetValueOrDefault());
                        erros = bllFuncao.Salvar(Acao.Excluir, funcao);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        retErro.erroGeral = "Funçao Não Encontrada";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
                }
            }

            RetornoErro retornaErro = new RetornoErro();
            retornaErro.erroGeral = "Descrição deve ser informada";
            return Request.CreateResponse(HttpStatusCode.BadRequest, retornaErro);
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
            Dictionary<string, string> erroCodigo = new Dictionary<string, string>();
            erroCodigo = erros.Where(x => x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);
            if (erroCodigo.Count > 0)
            {
                ModelState.AddModelError("Codigo", string.Join(";", erroCodigo.Select(x => x.Value).ToArray()));
            }

            Dictionary<string, string> erroDescricao = new Dictionary<string, string>();
            erroDescricao = erros.Where(x => x.Key.Equals("txtDescricao")).ToDictionary(x => x.Key, x => x.Value);
            if (erroDescricao.Count > 0)
            {
                ModelState.AddModelError("Descricao",string.Join(";", erroDescricao.Select(x => x.Value).ToArray()));
            }

            erros = erros.Where(x => !x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtDescricao")).ToDictionary(x => x.Key, x => x.Value);
            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }
    }
}
