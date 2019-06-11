using BLL;
using CentralCliente;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace cwkWebAPIPontoWeb
{
    public class LogAPI : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var bkpActionContext = actionContext;
            try
            {
                string chaveIntegracao = RetornaChaveIntegracao(actionContext);
                string jsonDoRequestParameters = string.Empty;
                dynamic dadosIniciais = Utils.MetodosAuxiliares.RetornaDadosIniciais();
                string conexao = dadosIniciais.ConnectionString;
                string usuario = dadosIniciais.Usuario;
                string centroServico = dadosIniciais.NomeDaBase;

                // Verificar existência da integração na central cliente e caso encontre retorna o id.
                int idIntegracaoCentralCliente = CentralCliente.Integracoes.getIdIntegracaoPelaChave(chaveIntegracao);

                if (idIntegracaoCentralCliente == 0)// Integração Não localizada na central cliente.
                {
                    // Buscara a Integracao pela descrição na api de integrações
                    // Obs: Buscar Também Pelo id da Aplicação
                    RetornoIntegracao objIntegracaoLocalizada = BLL.IntegracoesApi.VerificaExistenciaIntegracao(new ObterIntegracao(chaveIntegracao));

                    idIntegracaoCentralCliente = !string.IsNullOrWhiteSpace(objIntegracaoLocalizada.Id) ? int.Parse(objIntegracaoLocalizada.Id) : 0;

                    if (idIntegracaoCentralCliente != 0) //      1.1 Localizado na API de integrações: Insere no central cliente
                    {
                        Integracao objNovaIntegracao = new Integracao { Chave = chaveIntegracao, idIntegracao = idIntegracaoCentralCliente };
                        CentralCliente.Integracoes.cadastrarIntegracao(objNovaIntegracao);
                    }
                    else //      1.2 Não Localizado na API de integrações: Cadastrar na api de integrações e na Central Cliente
                    {
                        idIntegracaoCentralCliente = BLL.IntegracoesApi.CriarIntegracao(new CadastrarIntegracao(chaveIntegracao));
                        if (idIntegracaoCentralCliente != 0)
                        {
                            Integracao objNovaIntegracao = new Integracao { Chave = chaveIntegracao, idIntegracao = idIntegracaoCentralCliente };
                            CentralCliente.Integracoes.cadastrarIntegracao(objNovaIntegracao);
                        }
                    }
                }

                // Captura os argumentos do request
                if (actionContext.ActionArguments != null)
                {
                    CapturarRequestParameters(actionContext.ActionArguments, ref jsonDoRequestParameters);
                }

                // Envia a solicitação de inicio da Integração para a api de integrações.
                string retIntegracaoExecucao = BLL.IntegracoesApi.IniciarIntegracaoExecucao(new IniciarIntegracaoExecucao(idIntegracaoCentralCliente, usuario, centroServico, jsonDoRequestParameters));

                // Adiciona os argumentos no contexto da action http, 
                // permitindo acessar esses dados depois dentro da action ou após a finalização de execução da action
                AdicionaItemsNoActionArguments(ref actionContext, retIntegracaoExecucao);
                if (!actionContext.ActionArguments.ContainsKey("conexao"))
                    actionContext.ActionArguments.Add("conexao", conexao);
                if (!actionContext.ActionArguments.ContainsKey("usuario"))
                    actionContext.ActionArguments.Add("usuario", usuario);
                if (!actionContext.ActionArguments.ContainsKey("centroservico"))
                    actionContext.ActionArguments.Add("centroservico", centroServico);
            }
            catch (Exception ex)
            {
                actionContext = bkpActionContext;
                BLL.cwkFuncoes.LogarErro(ex);
            }
        }

        private void CapturarRequestParameters(Dictionary<string, object> ActionArguments, ref string jsonDoRequestParameters)
        {
            foreach (var param in ActionArguments)
            {
                jsonDoRequestParameters += (JsonConvert.SerializeObject(param) + "\n");
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var bkpActionContext = actionExecutedContext;
            try
            {
                Dictionary<string, object> retorno = new Dictionary<string, object>();
                if (actionExecutedContext != null && actionExecutedContext.Response != null)
                {
                    var objectContent = actionExecutedContext.Response.Content as ObjectContent;
                    string type = string.Empty;
                    object value = new object();
                    if (objectContent != null)
                    {
                        type = !string.IsNullOrWhiteSpace(objectContent.ObjectType.FullName) ? objectContent.ObjectType.FullName : string.Empty;
                        type = string.IsNullOrEmpty(type) && !string.IsNullOrWhiteSpace(objectContent.ObjectType.Name) ? objectContent.ObjectType.Name : type;
                        value = objectContent.Value;

                        if (!retorno.ContainsKey(type))
                            retorno.Add(type, value);
                    }
                }
                else
                {
                    retorno.Add("Erro Geral", actionExecutedContext.Exception);
                }

                string valorDevolvido = JsonConvert.SerializeObject(retorno);

                string idIntegracaoTransacao = (string)actionExecutedContext.ActionContext.ActionArguments
                                                        .Where(x => x.Key.Equals("idIntegracaoExecucao"))
                                                        .Select(x => x.Value).FirstOrDefault();

                string usuario = (string)actionExecutedContext.ActionContext.ActionArguments
                                        .Where(x => x.Key.Equals("usuario"))
                                        .Select(x => x.Value).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(idIntegracaoTransacao))
                {
                    if (actionExecutedContext != null && actionExecutedContext.Response != null && actionExecutedContext.Response.IsSuccessStatusCode)
                    {
                        BLL.IntegracoesApi.FinalizarIntegracaoExecucao(new FinalizarIntegracaoExecucao(int.Parse(idIntegracaoTransacao), usuario, valorDevolvido));
                    }
                    else
                    {
                        BLL.IntegracoesApi.CancelarIntegracaoExecucao(new CancelarIntegracaoExecucao(int.Parse(idIntegracaoTransacao), usuario, valorDevolvido));
                    }
                }
            }
            catch (Exception ex)
            {
                actionExecutedContext = bkpActionContext;
                BLL.cwkFuncoes.LogarErro(ex);
            }
        }

        private void AdicionaItemsNoActionArguments(ref HttpActionContext contexto, string dadosIntegracaoExecucao)
        {
            var retornoDictionary = (Dictionary<string, string>)JsonConvert.DeserializeObject(dadosIntegracaoExecucao, typeof(Dictionary<string, string>));

            foreach (var item in retornoDictionary)
            {
                if (!contexto.ActionArguments.ContainsKey(item.Key))
                    contexto.ActionArguments.Add(item.Key, item.Value);
            }
        }

        private string RetornaChaveIntegracao(HttpActionContext actionContext)
        {
            var descriptor = actionContext.ActionDescriptor;

            return string.Format("{0}.{1}.{2}",
                !string.IsNullOrEmpty(descriptor.ControllerDescriptor.ControllerName) ? descriptor.ControllerDescriptor.ControllerName : string.Empty,
                !string.IsNullOrEmpty(descriptor.ActionName) ? descriptor.ActionName : string.Empty,
                !string.IsNullOrEmpty(actionContext.Request.Method.Method) ? actionContext.Request.Method.Method : string.Empty).ToLower();
        }
    }
}