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
    /// <summary>
    /// Método responsável por retornar se o período solicitado está confirmado no Painel
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ConfirmacaoPainelController : ApiController
    {
        /// <summary>
        /// Método responsável por retornar se o período solicitado está confirmado no Painel
        /// </summary>
        /// <returns>Retorna objeto contendo se está confirmado ou não e erros caso identificados;
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage ConfirmacaoPainel(int Mes, int Ano, int idFuncionario)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.ConfirmacaoPainel bllConfirmacao = new BLL.ConfirmacaoPainel(connectionStr);
            BLL.Funcionario bllFunc = new BLL.Funcionario(connectionStr);
            try
            {
                Models.ConfirmacaoPainel Conf = new Models.ConfirmacaoPainel();
                if (bllFunc.LoadObject(idFuncionario) == null)
                {
                    Conf.Erro = true;
                    Conf.DescricaoErro = "Funcionário não encontrado.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Conf);
                }
                else
                {
                    Modelo.ConfirmacaoPainel Confirmacao = bllConfirmacao.GetPorMesAnoIdFunc(Mes, Ano, idFuncionario);
                    if (Confirmacao == null)
                    {
                        Conf.Confirmado = false;
                        Conf.Ano = Ano;
                        Conf.Mes = Mes;
                        Conf.idFuncionario = idFuncionario;
                        return Request.CreateResponse(HttpStatusCode.OK, Conf);
                    }
                    else if (Confirmacao.Id != 0)
                    {
                        Conf.Confirmado = true;
                        Conf.Ano = Confirmacao.Ano;
                        Conf.Mes = Confirmacao.Mes;
                        Conf.idFuncionario = Confirmacao.idFuncionario;
                        Conf.Data = Confirmacao.Inchora.GetValueOrDefault();
                        return Request.CreateResponse(HttpStatusCode.OK, Conf);
                    }
                    else
                    {
                        Conf.Confirmado = false;
                        Conf.Ano = Confirmacao.Ano;
                        Conf.Mes = Confirmacao.Mes;
                        Conf.idFuncionario = Confirmacao.idFuncionario;
                        return Request.CreateResponse(HttpStatusCode.OK, Conf);
                    }
                }

            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
            }
        }

        /// <summary>
        /// Incluir ConfirmacaoPonto
        /// </summary>
        /// <param name="ConfirmacaoPainel">Json com os dados de ConfirmacaoPainel</param>
        /// <returns> Retorna json de ConfirmacaoPainel quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.ConfirmacaoPainel ConfirmacaoPainel)
        {
            try
            {
                    string connectionStr = MetodosAuxiliares.Conexao();
                    BLL.ConfirmacaoPainel BllConfirmacao = new BLL.ConfirmacaoPainel(connectionStr);
                    BLL.Funcionario BllFuncionario = new BLL.Funcionario(connectionStr);
                    Modelo.ConfirmacaoPainel Conf = BllConfirmacao.GetPorMesAnoIdFunc(ConfirmacaoPainel.Mes, ConfirmacaoPainel.Ano, ConfirmacaoPainel.idFuncionario);
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    
                    if (Conf != null && Conf.Id > 0)
	                {
		                ConfirmacaoPainel.Erro = true;
                        ConfirmacaoPainel.DescricaoErro = "Período já está confirmado!";
                        return Request.CreateResponse(HttpStatusCode.BadRequest, ConfirmacaoPainel);
	                }
                    else
	                {
                        Modelo.ConfirmacaoPainel Confirmacao = new Modelo.ConfirmacaoPainel();
                        Confirmacao.idFuncionario = ConfirmacaoPainel.idFuncionario;
                        Confirmacao.Incdata = DateTime.Now;
                        Confirmacao.Codigo = BllConfirmacao.MaxCodigo();
                        Confirmacao.Inchora = DateTime.Now;
                        Confirmacao.Mes = ConfirmacaoPainel.Mes;
                        Confirmacao.Ano = ConfirmacaoPainel.Ano;
                        erros = BllConfirmacao.Salvar(Acao.Incluir, Confirmacao);
                        if (erros.Count() > 0)
	                    {
    		                ConfirmacaoPainel.Erro = true;
                            ConfirmacaoPainel.DescricaoErro = String.Join("; ", erros.Select(x=> x.Value));
                            return Request.CreateResponse(HttpStatusCode.BadRequest, ConfirmacaoPainel);
                    	}
                        else
	                    {
                             return Request.CreateResponse(HttpStatusCode.OK, ConfirmacaoPainel);
	                    }
	                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ConfirmacaoPainel);
            }
        }
    }
}