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
    /// Métodos Referentes ao Cadastro de Ferias
    /// </summary>
    //[Authorize]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class FeriasController : ApiController
    {
        // POST: Ferias
        /// <summary>
        /// Cadastrar/Alterar Férias.
        /// </summary>
        /// <param name="Ferias">Json com os dados de férias</param>
        /// <returns> Retorna json de férias quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [ActionName("Ferias")]
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Ferias Ferias)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(connectionStr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(connectionStr);

            Models.Afastamento af = new Models.Afastamento();
            af.Codigo = Ferias.Codigo.GetValueOrDefault();
            af.DataFinal = Ferias.DataFinal;
            af.DataInicial = Ferias.DataInicial;
            af.IdIntegracao = "F" + Ferias.IdIntegracao;
            af.IdIntegracaoFuncionario = Ferias.IdIntegracaoFuncionario;


            if (ModelState.IsValid)
            {
                int? idFuncionario, idOcorrencia;
                ValidaDados(af, connectionStr, out idFuncionario, out idOcorrencia);

                if (ModelState.IsValid)
                {
                    try
                    {
                        if ((af.Id != null && af.Id > 0) || (af.IdFuncionario != null && af.IdOcorrencia != null))
                        {
                            Afastamento DadosAntAfastamento = bllAfastamento.LoadObject(af.Id.GetValueOrDefault());

                            if ((DadosAntAfastamento == null || DadosAntAfastamento.Id == 0) && af.IdMarcacao > 0)
                            {
                                int? idAfastamento = bllAfastamento.GetIdAfastamentoPorIdMarcacao(af.IdMarcacao.GetValueOrDefault());
                                if (idAfastamento != null && idAfastamento > 0)
                                {
                                    DadosAntAfastamento = bllAfastamento.LoadObject(idAfastamento.GetValueOrDefault());
                                }
                            }
                            Acao acao = new Acao();
                            if (DadosAntAfastamento.Id == 0)
                            {
                                acao = Acao.Incluir;
                                DadosAntAfastamento.Codigo = bllAfastamento.MaxCodigo();
                                DadosAntAfastamento.Datai = af.DataInicial;
                                DadosAntAfastamento.Dataf = af.DataFinal;
                                DadosAntAfastamento.Tipo = 0;
                                DadosAntAfastamento.IdFuncionario = idFuncionario.GetValueOrDefault();
                                BLL.Ocorrencia bllocorrencia = new BLL.Ocorrencia(connectionStr);
                                int? idocorrencia = bllocorrencia.getOcorrenciaNome("Férias");
                                DadosAntAfastamento.IdOcorrencia = idocorrencia.GetValueOrDefault();
                                DadosAntAfastamento.BAbonado = true;
                                DadosAntAfastamento.Observacao = af.Observacao;
                            }
                            else
                            {
                                acao = Acao.Alterar;
                                PreencheDadosAnt(DadosAntAfastamento);
                                DadosAntAfastamento.Datai = af.DataInicial;
                                DadosAntAfastamento.Dataf = af.DataFinal;
                                DadosAntAfastamento.IdFuncionario = idFuncionario.GetValueOrDefault();
                                BLL.Ocorrencia bllocorrencia = new BLL.Ocorrencia(connectionStr);
                                int? idocorrencia = bllocorrencia.getOcorrenciaNome("Férias");
                                DadosAntAfastamento.IdOcorrencia = idocorrencia.GetValueOrDefault();
                                DadosAntAfastamento.Observacao = af.Observacao;
                            }

                            Dictionary<string, string> erros = new Dictionary<string, string>();
                            DadosAntAfastamento.ForcarNovoCodigo = true;
                            erros = bllAfastamento.Salvar(acao, DadosAntAfastamento);
                            if (erros.Count > 0)
                            {
                                TrataErros(erros);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, af);
                            }
                        }
                        else if (af.IdIntegracao != null && af.IdIntegracao != "")
                        {
                            int? idAfastamento = bllAfastamento.GetIdPorIdIntegracao(af.IdIntegracao);
                            Modelo.Afastamento DadosAntAfastamento = new Modelo.Afastamento();
                            if (idAfastamento.GetValueOrDefault() > 0)
                            {
                                DadosAntAfastamento = bllAfastamento.LoadObject(idAfastamento.GetValueOrDefault());
                            }
                            else
                            {
                                DadosAntAfastamento = bllAfastamento.GetAfastamentoFuncionarioPeriodo(idFuncionario.GetValueOrDefault(), af.DataInicial, af.DataFinal).Where(w => w.Datai == af.DataInicial && w.Dataf == af.DataFinal && w.IdOcorrencia == idOcorrencia).FirstOrDefault();
                                if (DadosAntAfastamento != null && DadosAntAfastamento.Id > 0)
                                {
                                    DadosAntAfastamento.IdIntegracao = af.IdIntegracao;
                                }
                                else
                                {
                                    DadosAntAfastamento = new Afastamento();
                                }
                            }

                            Acao acao = new Acao();
                            if (DadosAntAfastamento.Id == 0)
                            {
                                acao = Acao.Incluir;
                                DadosAntAfastamento.IdIntegracao = af.IdIntegracao;
                                DadosAntAfastamento.Codigo = af.Codigo.GetValueOrDefault();
                            }
                            else
                            {
                                acao = Acao.Alterar;
                                PreencheDadosAnt(DadosAntAfastamento);
                            }

                            DadosAntAfastamento.Datai = af.DataInicial;
                            DadosAntAfastamento.Dataf = af.DataFinal;
                            DadosAntAfastamento.IdFuncionario = idFuncionario.GetValueOrDefault();
                            BLL.Ocorrencia bllocorrencia = new BLL.Ocorrencia(connectionStr);
                            int? idocorrencia = bllocorrencia.getOcorrenciaNome("Férias");
                            DadosAntAfastamento.IdOcorrencia = idocorrencia.GetValueOrDefault();
                            DadosAntAfastamento.Tipo = 0;
                            DadosAntAfastamento.BAbonado = false;
                            DadosAntAfastamento.SemAbono = true;

                            DadosAntAfastamento.Observacao = af.Observacao;

                            Dictionary<string, string> erros = new Dictionary<string, string>();
                            DadosAntAfastamento.ForcarNovoCodigo = true;
                            erros = bllAfastamento.Salvar(acao, DadosAntAfastamento);
                            if (erros.Count > 0)
                            {
                                TrataErros(erros);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, af);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        retErro.erroGeral += ex.Message;
                    }
                }
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

        private void ValidaDados(Models.Afastamento Afastamento, string connectionStr, out int? idFuncionario, out int? idOcorrencia)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(connectionStr);
            if (Afastamento.IdFuncionario != null && Afastamento.IdFuncionario > 0)
            {
                idFuncionario = bllFuncionario.LoadObject(Afastamento.IdFuncionario.GetValueOrDefault()).Id;
            }
            else
            {
                idFuncionario = bllFuncionario.GetIdporIdIntegracao(Afastamento.IdIntegracaoFuncionario);
            }
            if (idFuncionario == 0)
            {
                ModelState.AddModelError("IdIntegracaoFuncionario", "Funcionário não cadastrado no Pontofopag");
            }

            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(connectionStr);
            if (Afastamento.IdOcorrencia != null && Afastamento.IdOcorrencia > 0)
            {
                idOcorrencia = bllOcorrencia.LoadObject(Afastamento.IdOcorrencia.GetValueOrDefault()).Id;
            }
            else
            {
                idOcorrencia = bllOcorrencia.GetIdPorIdIntegracao(Afastamento.IdIntegracaoOcorrencia.GetValueOrDefault());
                if (idOcorrencia == 0)
                {
                    ModelState.AddModelError("IdIntegracaoOcorrencia", "Ocorrência não cadastrado no Pontofopag");
                }
            }
        }

        private static void PreencheDadosAnt(Modelo.Afastamento afastamento)
        {
            afastamento.Tipo_Ant = afastamento.Tipo;
            afastamento.IdEmpresa_Ant = afastamento.IdEmpresa;
            afastamento.IdDepartamento_Ant = afastamento.IdDepartamento;
            afastamento.IdContrato = afastamento.IdContrato_Ant;
            afastamento.IdFuncionario_Ant = afastamento.IdFuncionario;
            afastamento.Datai_Ant = afastamento.Datai;
            afastamento.Dataf_Ant = afastamento.Dataf;
            //afastamento.Observacao_Ant = afastamento.Observacao;
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

        [ActionName("idIntegracaoFerias")]
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(string idIntegracaoFerias)
        {
            RetornoErro retErro = new RetornoErro();
            AfastamentoController afastamentocontroller = new AfastamentoController();
            try
            {
                if (ModelState.IsValid)
                {
                    int? idAfastamento = afastamentocontroller.RetornaIdAfastamento("F" + idIntegracaoFerias, 0);

                    if (idAfastamento != null && idAfastamento > 0)
                    {
                        Dictionary<string, string> erros = afastamentocontroller.ExecutaAfastamento(idAfastamento);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        retErro.erroGeral = "Férias Não Encontrada";
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

    }
}