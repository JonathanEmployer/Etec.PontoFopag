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
    /// Métodos Referentes ao Vínculo de Contratos e Funcionários
    /// </summary>
    public class ContratoFuncionarioController : ApiController
    {
        /// <summary>
        /// Vincular Contrato a Funcionário
        /// </summary>
        /// <param name="listacontratofuncionario">Json da Lista de ContratosFuncionarios</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando incluído com sucesso, quando apresentar erro retorna o json da lista de contrato funcionário com o atributo erro = true e detalheerro com a descrição do erro nos registros que geraram erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(List<Models.ContratoFuncionario> listacontratofuncionario)
        {
            Acao acao = Acao.Incluir;
            return IncluiExcluiContratoFuncionario(listacontratofuncionario, acao);
        }

        private HttpResponseMessage IncluiExcluiContratoFuncionario(List<Models.ContratoFuncionario> listacontratofuncionario, Acao acao)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(connectionStr);
            BLL.Contrato bllContrato = new BLL.Contrato(connectionStr);
            BLL.ContratoFuncionario bllContratoFuncionario = new BLL.ContratoFuncionario(connectionStr);
            Dictionary<string, string> erros = new Dictionary<string, string>();
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in listacontratofuncionario)
                    {
                        int? funcid = bllFuncionario.GetIdporIdIntegracao(item.IdIntegracaoFuncionario);
                        int? contid = bllContrato.GetIdPorIdIntegracao(item.IdIntegracaoContrato);

                        if (funcid > 0 && contid > 0)
                        {
                            Modelo.Contrato Contrato = bllContrato.LoadObject(contid.GetValueOrDefault());
                            Modelo.ContratoFuncionario ContratoFunc = new Modelo.ContratoFuncionario();
                            BLL.ContratoFuncionario bllContratoFun = new BLL.ContratoFuncionario(connectionStr);                          

                            ContratoFunc.IdContrato = contid.GetValueOrDefault();
                            ContratoFunc.IdFuncionario = funcid.GetValueOrDefault();

                            int? idcontratofuncionario = bllContratoFuncionario.GetIdPorIdContratoeIdFuncionario(contid.GetValueOrDefault(), funcid.GetValueOrDefault());
                            int? idContratoAnt = bllContratoFun.getContratoId(ContratoFunc.IdFuncionario);
                            
                            Modelo.ContratoFuncionario ContFunc = new Modelo.ContratoFuncionario();                            
                            if (idcontratofuncionario.GetValueOrDefault() == 0 && acao == Acao.Incluir)
                            {
                                if (idContratoAnt != ContratoFunc.IdContrato && idContratoAnt != 0)
                                {
                                    int CodigoContrato = bllContratoFun.getContratoCodigo(idContratoAnt.GetValueOrDefault(), ContratoFunc.IdFuncionario);
                                    int IdAnt = CodigoContrato != 0 ? bllContratoFun.getId(CodigoContrato, null, null) : 0;
                                    Modelo.ContratoFuncionario ContFuncAnt = new Modelo.ContratoFuncionario();
                                    ContFuncAnt = bllContratoFuncionario.LoadObject(IdAnt);
                                    acao = Acao.Alterar;                                                                      
                                    ContFuncAnt.excluido = 1;                                    
                                    erros = SalvarContratoFuncionario(acao, bllContratoFuncionario, erros, item, ContFuncAnt);
                                }
                                ContFunc.IdContrato = contid.GetValueOrDefault();
                                ContFunc.IdFuncionario = funcid.GetValueOrDefault();
                                ContFunc.Codigo = bllContratoFuncionario.MaxCodigo();                            
                                acao = Acao.Incluir;
                                erros = SalvarContratoFuncionario(acao, bllContratoFuncionario, erros, item, ContFunc);
                            }
                            else if(idcontratofuncionario != 0 && acao == Acao.Excluir)
                            {
                                ContFunc = bllContratoFuncionario.LoadObject(idcontratofuncionario.GetValueOrDefault());
                                acao = Acao.Alterar;
                                ContFunc.Acao = Acao.Alterar;
                                ContFunc.excluido = 1;
                                erros = SalvarContratoFuncionario(acao, bllContratoFuncionario, erros, item, ContFunc);
                            }
                            
                        }
                        else
                        {
                            if (funcid == 0)
                            {
                                item.erro = true;
                                item.descricaoerro = "IdIntegracao: " + item.IdIntegracaoFuncionario + " Erro: " + "Funcionário não encontrado";

                            }
                            else if (contid == 0)
                            {
                                item.erro = true;
                                item.descricaoerro = "IdIntegracao: " + item.IdIntegracaoContrato + " Erro: " + "Contrato não encontrado";
                            }
                        }
                    }
                    if (listacontratofuncionario.Where(x => x.erro == true).Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, listacontratofuncionario);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    listacontratofuncionario.ForEach(c => { c.erro = true; c.descricaoerro = ex.Message; });
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);

                }
            }
            return TrataErroModelState(retErro);
        }

        private static Dictionary<string, string> SalvarContratoFuncionario(Acao acao, BLL.ContratoFuncionario bllContratoFuncionario, Dictionary<string, string> erros, Models.ContratoFuncionario item, Modelo.ContratoFuncionario ContFunc)
        {
            erros = bllContratoFuncionario.Salvar(acao, ContFunc);
            if (erros.Count > 0)
            {
                item.erro = true;
                item.descricaoerro = string.Join(";", erros.Select(x => x.Value).ToArray());
            }
            return erros;
        }

        /// <summary>
        /// Excluir o vínculo de Contrato e Funcionário.
        /// </summary>
        /// <param name="listacontratofuncionario">Lista de Contrato Funcionários</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorna o json da lista de contrato funcionário com o atributo erro = true e detalheerro com a descrição do erro nos registros que geraram erro.</returns>
        [HttpPut]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(List<Models.ContratoFuncionario> listacontratofuncionario)
        {
            Acao acao = Acao.Excluir;
            return IncluiExcluiContratoFuncionario(listacontratofuncionario, acao);
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
