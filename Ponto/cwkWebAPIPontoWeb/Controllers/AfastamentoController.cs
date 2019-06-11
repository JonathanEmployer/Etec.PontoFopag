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
    //[ApiExplorerSettings(IgnoreApi = true)]
    /// <summary>
    /// Métodos Referentes ao Cadastro de Afastamento
    /// </summary>
    public class AfastamentoController : ApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Afastamento.
        /// </summary>
        /// <param name="Afastamento">Json com os dados de função</param>
        /// <returns> Retorna json de função quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Afastamento Afastamento)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(connectionStr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(connectionStr);

            if (ModelState.IsValid)
            {
                int? idFuncionario, idOcorrencia;
                ValidaDados(Afastamento, connectionStr, out idFuncionario, out idOcorrencia);

                if (ModelState.IsValid)
                {
                    try
                    {
                        if ((Afastamento.Id != null && Afastamento.Id > 0) || (Afastamento.IdFuncionario != null && Afastamento.IdOcorrencia != null))
                        {
                            Afastamento DadosAntAfastamento = bllAfastamento.LoadObject(Afastamento.Id.GetValueOrDefault());

                            if ((DadosAntAfastamento == null || DadosAntAfastamento.Id == 0) && Afastamento.IdMarcacao > 0)
                            {
                                int? idAfastamento = bllAfastamento.GetIdAfastamentoPorIdMarcacao(Afastamento.IdMarcacao.GetValueOrDefault());
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
                                DadosAntAfastamento.Datai = Afastamento.DataInicial;
                                DadosAntAfastamento.Dataf = Afastamento.DataFinal;
                                DadosAntAfastamento.Tipo = 0;
                                DadosAntAfastamento.IdFuncionario = idFuncionario.GetValueOrDefault();
                                DadosAntAfastamento.IdOcorrencia = Afastamento.IdOcorrencia.GetValueOrDefault();
                                DadosAntAfastamento.BAbonado = true;
                                DadosAntAfastamento.Observacao = Afastamento.Observacao;
                                if (Afastamento.Parcial)
                                {
                                    DadosAntAfastamento.BParcial = true;
                                    DadosAntAfastamento.Horai = Afastamento.HorasAbonoDiurno;
                                    DadosAntAfastamento.Horaf = Afastamento.HorasAbonoNoturno;
                                }
                                else
                                {
                                    DadosAntAfastamento.BParcial = false;
                                }

                            }
                            else
                            {
                                acao = Acao.Alterar;
                                PreencheDadosAnt(DadosAntAfastamento);
                                DadosAntAfastamento.Datai = Afastamento.DataInicial;
                                DadosAntAfastamento.Dataf = Afastamento.DataFinal;
                                DadosAntAfastamento.IdFuncionario = idFuncionario.GetValueOrDefault();
                                DadosAntAfastamento.IdOcorrencia = Afastamento.IdOcorrencia.GetValueOrDefault();
                                DadosAntAfastamento.Observacao = Afastamento.Observacao;
                                if (Afastamento.Parcial)
                                {
                                    DadosAntAfastamento.BParcial = true;
                                    DadosAntAfastamento.Horai = Afastamento.HorasAbonoDiurno;
                                    DadosAntAfastamento.Horaf = Afastamento.HorasAbonoNoturno;
                                }
                                else
                                {
                                    DadosAntAfastamento.BParcial = false;
                                }
                                Modelo.Marcacao marc = bllMarcacao.getListaMarcacao(2, idFuncionario.GetValueOrDefault(), Afastamento.DataInicial, Afastamento.DataFinal).FirstOrDefault();
                                DadosAntAfastamento.Horai = marc.Horasfaltas;
                                DadosAntAfastamento.Horaf = marc.Horasfaltanoturna;
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
                                return Request.CreateResponse(HttpStatusCode.OK, Afastamento);
                            }
                        }
                        else if (Afastamento.IdIntegracao != null && Afastamento.IdIntegracao != "")
                        {
                            int? idAfastamento = bllAfastamento.GetIdPorIdIntegracao(Afastamento.IdIntegracao);
                            Modelo.Afastamento DadosAntAfastamento = new Modelo.Afastamento();
                            if (idAfastamento.GetValueOrDefault() > 0)
                            {
                                DadosAntAfastamento = bllAfastamento.LoadObject(idAfastamento.GetValueOrDefault());
                            }
                            else
                            {
                                DadosAntAfastamento = bllAfastamento.GetAfastamentoFuncionarioPeriodo(idFuncionario.GetValueOrDefault(), Afastamento.DataInicial, Afastamento.DataFinal).Where(w => w.Datai == Afastamento.DataInicial && w.Dataf == Afastamento.DataFinal && w.IdOcorrencia == idOcorrencia).FirstOrDefault();
                                if (DadosAntAfastamento != null && DadosAntAfastamento.Id > 0)
                                {
                                    DadosAntAfastamento.IdIntegracao = Afastamento.IdIntegracao;
                                }
                                else
                                {
                                    DadosAntAfastamento = new Modelo.Afastamento();
                                }
                            }
                           
                            Acao acao = new Acao();
                            if (DadosAntAfastamento.Id == 0)
                            {
                                acao = Acao.Incluir;
                                DadosAntAfastamento.IdIntegracao = Afastamento.IdIntegracao;
                                DadosAntAfastamento.Codigo = Afastamento.Codigo.GetValueOrDefault();
                            }
                            else
                            {
                                acao = Acao.Alterar;
                                PreencheDadosAnt(DadosAntAfastamento);
                            }

                            if (Afastamento.Parcial)
                            {
                                DadosAntAfastamento.BParcial = true;
                                DadosAntAfastamento.Horai = Afastamento.HorasAbonoDiurno;
                                DadosAntAfastamento.Horaf = Afastamento.HorasAbonoNoturno;
                            }
                            else
                            {
                                DadosAntAfastamento.BParcial = false;
                            }
                            DadosAntAfastamento.Datai = Afastamento.DataInicial;
                            DadosAntAfastamento.Dataf = Afastamento.DataFinal;
                            DadosAntAfastamento.IdFuncionario = idFuncionario.GetValueOrDefault();
                            DadosAntAfastamento.IdOcorrencia = idOcorrencia.GetValueOrDefault();
                            DadosAntAfastamento.Tipo = 0;
                            DadosAntAfastamento.BAbonado = true;
                            DadosAntAfastamento.Observacao = Afastamento.Observacao;

                            Dictionary<string, string> erros = new Dictionary<string, string>();
                            DadosAntAfastamento.ForcarNovoCodigo = true;
                            erros = bllAfastamento.Salvar(acao, DadosAntAfastamento);
                            if (erros.Count > 0)
                            {
                                TrataErros(erros);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, Afastamento);
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

        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(string id, int tipo)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(connectionStr);
            try
            {
                if (ModelState.IsValid)
                {
                    int? idAfastamento = RetornaIdAfastamento(id, tipo);

                    if (idAfastamento != null && idAfastamento > 0)
                    {
                        Dictionary<string, string> erros = ExecutaAfastamento(idAfastamento);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        retErro.erroGeral = "Afastamento Não Encontrado";
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

        public int? RetornaIdAfastamento(string id, int tipo)
        {
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(MetodosAuxiliares.Conexao());

            int? idAfastamento;
            if (tipo == 0)
            {
                idAfastamento = bllAfastamento.GetIdPorIdIntegracao(id.ToString());
            }
            else if (tipo == 1)
            {
                idAfastamento = Convert.ToInt32(id);
            }
            else if (tipo == 2)
            {
                BLL.Marcacao bllMarcacao = new BLL.Marcacao();
                idAfastamento = bllAfastamento.GetIdAfastamentoPorIdMarcacao(Convert.ToInt32(id));
            }
            else
            {
                idAfastamento = null;
            }

            return idAfastamento;
        }

        public Dictionary<string, string> ExecutaAfastamento(int? idAfastamento)
        {
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(MetodosAuxiliares.Conexao());
            Dictionary<string, string> erros = new Dictionary<string, string>();
            Modelo.Afastamento afastamento = bllAfastamento.LoadObject(idAfastamento.GetValueOrDefault());
            PreencheDadosAnt(afastamento);
            return bllAfastamento.Salvar(Acao.Excluir, afastamento);
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

