using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// Métodos Referentes ao Cadastro de Contrato
    /// </summary>
    public class ContratoController : ExtendedApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Contratos.
        /// </summary>
        /// <param name="Contrato">Json com os dados do Contrato</param>
        /// <returns> Retorna json de Contrato quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Contrato contrato)
        {
            RetornoErro retErro = new RetornoErro();

            BLL.Contrato bllContrato = new BLL.Contrato(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            BLL.Empresa bllEmpresa = new BLL.Empresa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

            if (ModelState.IsValid)
            {
                try
                {
                    int? idcontrato = bllContrato.GetIdPorIdIntegracao(contrato.IdIntegracao);;
                    Modelo.Contrato DadosAntContr = bllContrato.LoadObject(idcontrato.GetValueOrDefault());
                    Acao acao = new Acao();
                    if (DadosAntContr.Id == 0)
                    {
                        acao = Acao.Incluir;
                        DadosAntContr.Codigo = bllContrato.MaxCodigo();
                    }
                    else
                    {
                        acao = Acao.Alterar;
                    }

                    DadosAntContr.CodigoContrato = contrato.CodigoContrato;
                    DadosAntContr.DescricaoContrato = contrato.DescricaoContrato;
                    DadosAntContr.idIntegracao = contrato.IdIntegracao;
                    DadosAntContr.IdEmpresa = bllEmpresa.LoadObjectByDocumento(contrato.DocumentoEmpresa).Id;
                    Modelo.Contrato cont = new Modelo.Contrato();
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllContrato.Salvar(acao, DadosAntContr);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, DadosAntContr);
                        }
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
        /// Excluir Contrato.
        /// </summary>
        /// <param name="IdIntegracao">ID de Integração do contrato cadastrado</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(int IdIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            BLL.Contrato bllContrato = new BLL.Contrato(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

            if (ModelState.IsValid)
            {
                try
                {
                    int? idContrato = bllContrato.GetIdPorIdIntegracao(IdIntegracao);
                    Modelo.Contrato DadosAntDepart = bllContrato.LoadObject(idContrato.GetValueOrDefault());
                    if (DadosAntDepart.Id > 0)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllContrato.Salvar(Acao.Excluir, DadosAntDepart);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, DadosAntDepart);
                        }
                    }
                    else
                    {
                        retErro.erroGeral = "Departamento Não Encontrado";
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
            ComponenteToModel.Add("cbIdEmpresa", "CodigoEmpresa");
            ComponenteToModel.Add("IdEmpresa", "DocumentoEmpresa");
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
    }
}
