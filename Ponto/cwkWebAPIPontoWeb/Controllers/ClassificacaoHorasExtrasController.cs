using cwkWebAPIPontoWeb.Utils;
using Modelo;
using Newtonsoft.Json;
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
    public class ClassificacaoHorasExtrasController : ApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Classificação de Horas extras.
        /// </summary>
        /// <param name="IdMarcacao">Id da Marcação</param>
        /// <param name="IdClassificacao">Id de Classificação</param>
        /// <returns> Retorna json de função quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(int IdMarcacao, int IdClassificacaoHoraExtra, int IdClassificacao, string Observacao)
        {
            Utils.MetodosAuxiliares.EscreveLog("Requisicao Recebida Pelo Post ClassificacaoHorasExtrasController do usuário = " + User.Identity.Name + ", Parâmetros:  IdMarcacao = " + IdMarcacao + " IdClassificacaoHoraExtra = " + IdClassificacaoHoraExtra + " IdClassificacao = " + IdClassificacao + " Observacao =" + Observacao, "ClassificacaoHorasExtrasController");
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.ClassificacaoHorasExtras bllClassHE = new BLL.ClassificacaoHorasExtras(connectionStr);

                try
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    IList<Modelo.Proxy.pxyClassHorasExtrasMarcacao> classificacoes = bllClassHE.GetClassificacoesMarcacao(IdMarcacao);
                    Modelo.Proxy.pxyClassHorasExtrasMarcacao classhe = classificacoes.Where(x => x.IdClassificacaoHorasExtras == IdClassificacaoHoraExtra).FirstOrDefault();
                    if (classhe == null || classhe.IdClassificacaoHorasExtras == 0 )
                    {
                        classhe = classificacoes.Where(x => x.Integrado).FirstOrDefault();
                    }
                    BLL.Classificacao bllclass = new BLL.Classificacao(connectionStr);
                    Modelo.Classificacao Classificacao = new Classificacao();
                    Classificacao = bllclass.LoadObject(IdClassificacao);
                    if (Classificacao.Id != 0)
                    {
                        if (classhe != null)
                        {
                            Modelo.ClassificacaoHorasExtras classificahe = new ClassificacaoHorasExtras();
                            classificahe = bllClassHE.LoadObject(classhe.IdClassificacaoHorasExtras);
                            classificahe.IdClassificacao = IdClassificacao;
                            classificahe.Observacao = Observacao;
                            classificahe.Integrado = true;
                            erros = bllClassHE.Salvar(Acao.Alterar, classificahe);
                            if (erros.Count > 0)
                            {
                                retErro.erroGeral = "Erro ao Salvar Classificação de Horas Extras";
                                retErro.ErrosDetalhados = new List<ErroDetalhe>();
                                foreach (KeyValuePair<string,string> item in erros)
	                            {
		                            ErroDetalhe d = new ErroDetalhe();
                                    d.campo = item.Key;
                                    d.erro = item.Value;
                                    retErro.ErrosDetalhados.Add(d);
	                            }
                                Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController retornou " + HttpStatusCode.BadRequest.ToString() + ", Erro:  " + JsonConvert.SerializeObject(retErro), "ClassificacaoHorasExtrasController");
                                return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
                            }
                            else
                            {
                                Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController retornou " + HttpStatusCode.OK.ToString() + ", Classificacao:  " + JsonConvert.SerializeObject(classificahe), "ClassificacaoHorasExtrasController");
                                return Request.CreateResponse(HttpStatusCode.OK, classificahe);
                            }
                        }
                        else
                        {
                            Modelo.ClassificacaoHorasExtras classificacao = new ClassificacaoHorasExtras();
                            classificacao.Codigo = bllClassHE.MaxCodigo();
                            classificacao.IdClassificacao = IdClassificacao;
                            classificacao.IdMarcacao = IdMarcacao;
                            classificacao.Incdata = DateTime.Now;
                            classificacao.Inchora = DateTime.Now;
                            classificacao.Incusuario = "Painel do RH";
                            classificacao.Tipo = 1;
                            classificacao.Observacao = Observacao;
                            classificacao.Integrado = true;
                            erros = bllClassHE.Salvar(Acao.Incluir, classificacao);
                            if (erros.Count > 0)
                            {
                                retErro.erroGeral = "Erro ao Salvar Classificação de Horas Extras";
                                Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController retornou " + HttpStatusCode.BadRequest.ToString() + ", Erro:  " + JsonConvert.SerializeObject(retErro), "ClassificacaoHorasExtrasController");
                                return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
                            }
                            else
                            {
                                Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController retornou " + HttpStatusCode.OK.ToString() + ", Classificacao:  " + JsonConvert.SerializeObject(classificacao), "ClassificacaoHorasExtrasController");
                                return Request.CreateResponse(HttpStatusCode.OK, classificacao);
                            }
                        }
                    }
                    else
                    {
                        retErro.erroGeral = "Classificação Não Encontrada";
                        Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController retornou " + HttpStatusCode.BadRequest.ToString() + ", Erro:  " + JsonConvert.SerializeObject(retErro), "ClassificacaoHorasExtrasController");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
                    }
                    
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                    return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
                }       
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
            Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController delete retornou " + HttpStatusCode.BadRequest.ToString() + ", Erro:  " + JsonConvert.SerializeObject(retErro), "ClassificacaoHorasExtrasController");
            return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
        }

        /// <summary>
        /// Excluir Classificação de Horas.
        /// </summary>
        /// <param name="IdMarcacao">Id da marcação para buscar a classificação.</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(int IdMarcacao)
        {
            Utils.MetodosAuxiliares.EscreveLog("Requisicao Recebida Pelo Delete ClassificacaoHorasExtrasController do usuário = " + User.Identity.Name + ", Parâmetros:  IdMarcacao = " + IdMarcacao, "ClassificacaoHorasExtrasController");
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(connectionStr);
            try
            {
                if (ModelState.IsValid)
                {
                    BLL.ClassificacaoHorasExtras bllclasshe = new BLL.ClassificacaoHorasExtras(connectionStr);
                    Modelo.Proxy.pxyClassHorasExtrasMarcacao pxyclasshe = new Modelo.Proxy.pxyClassHorasExtrasMarcacao();
                    pxyclasshe = bllclasshe.GetClassificacoesMarcacao(IdMarcacao).Where(x => x.Integrado).FirstOrDefault();

                    if (pxyclasshe != null && pxyclasshe.IdClassificacaoHorasExtras > 0)
                    {
                        Modelo.ClassificacaoHorasExtras classhe = new ClassificacaoHorasExtras();
                        classhe = bllclasshe.LoadObject(pxyclasshe.IdClassificacaoHorasExtras);
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllclasshe.Salvar(Acao.Excluir, classhe);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController Delete retornou " + HttpStatusCode.OK.ToString(), "ClassificacaoHorasExtrasController");
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        retErro.erroGeral = "Classificação de horas extras não encontrada. Registro não foi excluído.";
                        Utils.MetodosAuxiliares.EscreveLog("Requisicao ClassificacaoHorasExtrasController delete retornou " + HttpStatusCode.NotFound.ToString() + ", Erro:  " + JsonConvert.SerializeObject(retErro), "ClassificacaoHorasExtrasController");
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
                ModelState.AddModelError("Descricao", string.Join(";", erroDescricao.Select(x => x.Value).ToArray()));
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