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
    [Authorize]
    /// <summary>
    /// Métodos Referentes ao Cadastro de Departamento
    /// </summary>
    public class DepartamentoController : ApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Departamento.
        /// </summary>
        /// <param name="departamento">Json com os dados do departamento</param>
        /// <returns> Retorna json de departamento quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Departamento departamento)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Departamento bllDepart = new BLL.Departamento(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    int? idDepartamento = bllDepart.GetIdPoridIntegracao(departamento.IdIntegracao);
                    Modelo.Departamento DadosAntDepart = bllDepart.LoadObject(idDepartamento.GetValueOrDefault());
                    Acao acao = new Acao();
                    if (DadosAntDepart.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                    {
                        acao = Acao.Alterar;
                    }

                    DadosAntDepart.Codigo = departamento.Codigo;
                    DadosAntDepart.Descricao = departamento.Descricao;
                    DadosAntDepart.idIntegracao = departamento.IdIntegracao;
                    Modelo.Empresa emp = new Modelo.Empresa();
                    if (departamento.DocumentoEmpresa > 0)
                    {
                        BLL.Empresa bllEmp = new BLL.Empresa(connectionStr);
                        emp = bllEmp.LoadObjectByDocumento(departamento.DocumentoEmpresa);
                    }
                    if (emp.Id == 0)
                    {
                        ModelState.AddModelError("CodigoEmpresa", "Empresa Não Encontrada");
                    }
                    else
                    {
                        DadosAntDepart.IdEmpresa = emp.Id;
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        DadosAntDepart.ForcarNovoCodigo = true;
                        erros = bllDepart.Salvar(acao, DadosAntDepart);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, DadosAntDepart);
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
        /// Excluir Função.
        /// </summary>
        /// <param name="Codigo">Código da Função Cadastrada</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(int idIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Departamento bllDepart = new BLL.Departamento(connectionStr);

            if (ModelState.IsValid)
            {
                try
                {
                    int? idDepartamento = bllDepart.GetIdPoridIntegracao(idIntegracao);
                    Modelo.Departamento DadosAntDepart = bllDepart.LoadObject(idDepartamento.GetValueOrDefault());
                    if (DadosAntDepart.Id > 0)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllDepart.Salvar(Acao.Excluir, DadosAntDepart);
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
