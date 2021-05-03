using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LancamentoCreditoDebitoController : IControllerPontoWeb<InclusaoBanco>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        [PermissoesFiltro(Roles = "LancamentoCreditoDebito")]
        public override ActionResult Grid()
        {
            return View(new Modelo.InclusaoBanco());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {                
                BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(_usr.ConnectionString, _usr);
                List<Modelo.InclusaoBanco> dados = bllInclusaoBanco.GetAllList();
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "LancamentoCreditoDebitoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoCreditoDebitoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoCreditoDebitoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoCreditoDebitoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(InclusaoBanco obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoCreditoDebitoAlterar")]
        [HttpPost]

        public override ActionResult Alterar(InclusaoBanco obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoCreditoDebitoExcluir")]
        public override ActionResult Excluir(int id)
        {
            BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(_usr.ConnectionString, _usr);
            InclusaoBanco inclusaoBanco = bllInclusaoBanco.LoadObject(id);
            inclusaoBanco.Tipo_Ant = inclusaoBanco.Tipo;
            inclusaoBanco.Data_Ant = inclusaoBanco.Data;
            inclusaoBanco.Identificacao_Ant = inclusaoBanco.Identificacao;
            inclusaoBanco.NaoRecalcular = true;
            try
            {
                if (inclusaoBanco.IdLancamentoLoteFuncionario.GetValueOrDefault() > 0)
                {
                    BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
                    BLL.LancamentoLoteFuncionario bllLancamentoLoteFuncionario = new BLL.LancamentoLoteFuncionario(_usr.ConnectionString, _usr);
                    LancamentoLoteFuncionario llf = bllLancamentoLoteFuncionario.LoadObject(inclusaoBanco.IdLancamentoLoteFuncionario.GetValueOrDefault());
                    llf.Acao = Acao.Excluir;
                    LancamentoLote ll = bllLancamentoLote.LoadObject(llf.IdLancamentoLote);
                    ll.LancamentoLoteFuncionarios = new List<LancamentoLoteFuncionario>();
                    ll.LancamentoLoteFuncionarios.Add(llf);
                    ll.Acao = Acao.Excluir;
                    Dictionary<string, string> erros = bllLancamentoLote.Salvar(Acao.Alterar, ll);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(" <br/> ", erros.Select(x => x.Key + " <br/> " + x.Value).ToArray());
                        return new JsonResult
                        {
                            Data = new
                            {
                                success = false,
                                Erro = erro
                            }
                        };
                    }
                    else
                    {
                        if (ll.LancamentoLoteFuncionarios.Where(x => x.Efetivado == false).Count() > 0)
                        {
                            return new JsonResult
                            {
                                Data = new
                                {
                                    success = false,
                                    Erro = ll.LancamentoLoteFuncionarios.Where(x => x.Efetivado == false).FirstOrDefault().DescricaoErro
                                }
                            };
                        }
                        else
                        {
                            inclusaoBanco.Acao = Acao.Excluir;
                            Modelo.Proxy.PxyJobReturn ret = Recalcular(ll, inclusaoBanco);
                            return new JsonResult
                            {
                                Data = new
                                {
                                    success = true,
                                    job = ret
                                }
                            };
                        }
                    }
                }
                else
                {
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllInclusaoBanco.Salvar(Acao.Excluir, inclusaoBanco);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                        return new JsonResult
                        {
                            Data = new
                            {
                                success = false,
                                Erro = erro
                            }
                        };
                    }
                    else
                    {
                        inclusaoBanco.Acao = Acao.Excluir;
                        Modelo.Proxy.PxyJobReturn ret = Recalcular(null, inclusaoBanco);
                        return new JsonResult
                        {
                            Data = new
                            {
                                success = true,
                                job = ret
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        Erro = ex.Message
                    }
                };
            }
        }

        private Modelo.Proxy.PxyJobReturn Recalcular(LancamentoLote lote, InclusaoBanco inclusaoBanco)
        {
            UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW, "", "/LancamentoCreditoDebito/Grid");
            string parametrosExibicao = String.Format("Código: {0}, Data: {1}, tipo: {2} - {3}, tipo Crédito/Débito: {4}, valor: {5}", inclusaoBanco.Codigo, inclusaoBanco.Data.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm"), inclusaoBanco.TipoDescricao, inclusaoBanco.Nome, inclusaoBanco.TipocreditodebitoDescricao, (inclusaoBanco.Credito != "---:--" ? inclusaoBanco.Credito : inclusaoBanco.Debito));
            string acao = inclusaoBanco.AcaoDescricao;
            string nomeProcesso = String.Format("Recalculo de marcações por {0} de lançamento lote", acao);
            Modelo.Proxy.PxyJobReturn ret = new Modelo.Proxy.PxyJobReturn();
            if (lote != null)
            {
                List<int> FuncRecalc = BLL.LancamentoLote.IdsFuncionariosRecalcularLote(lote);
                if (FuncRecalc.Count() > 0)
                {
                    ret = hfm.RecalculaMarcacao(nomeProcesso, parametrosExibicao, inclusaoBanco.Tipo, inclusaoBanco.Identificacao, inclusaoBanco.Data.Value, inclusaoBanco.Data.Value);
                }
            }
            else
            {
                ret = hfm.CalculaLancamentoCreditoDebito(nomeProcesso, parametrosExibicao, inclusaoBanco);
            }
            return ret;
        }

        protected override ActionResult Salvar(InclusaoBanco obj)
        {
            BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(_usr.ConnectionString, _usr);
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    obj.NaoRecalcular = true;
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    obj.Acao = acao;
                    erros = bllInclusaoBanco.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", "<div class=\"comment alert alert-danger\">" + erro + "</div>");
                    }
                    else
                    {
                        Recalcular(null, obj);
                        return RedirectToAction("Grid", "LancamentoCreditoDebito");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            ViewBag.UtilizaControleContratos = _usr.UtilizaControleContratos;
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(_usr.ConnectionString, _usr);
            InclusaoBanco inclusaoBanco = new InclusaoBanco();
            inclusaoBanco = bllInclusaoBanco.LoadObject(id);
            if (id == 0)
            {
                inclusaoBanco.Codigo = bllInclusaoBanco.MaxCodigo();
            }
            else
            {
                inclusaoBanco.Tipo_Ant = inclusaoBanco.Tipo;
                inclusaoBanco.Data_Ant = inclusaoBanco.Data;
                inclusaoBanco.Identificacao_Ant = inclusaoBanco.Identificacao;
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);
                    string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(inclusaoBanco.Tipo, new List<int>() { inclusaoBanco.Identificacao }, inclusaoBanco.Data.GetValueOrDefault());
                    if (!String.IsNullOrEmpty(mensagemFechamento))
                    {
                        ViewBag.Consultar = 1;
                        @ViewBag.MensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento de ponto vinculado. Detalhes: <br/>" + mensagemFechamento;
                    }
                }
                #endregion
            }

            ViewBag.UtilizaControleContratos = _usr.UtilizaControleContratos;
            return View("Cadastrar", inclusaoBanco);
        }

        protected override void ValidarForm(InclusaoBanco obj)
        {
            ValidaJustificativa(obj);
            ValidaCreditoDebito(obj);
            switch (obj.Tipo)
            {
                case 0:
                    VerificaEmpresa(obj);
                    break;
                case 1:
                    VerificaDepartamento(obj);
                    break;
                case 2:
                    VerificaFuncionario(obj);
                    break;
                default:
                    VerificaFuncao(obj);
                    break;
            }
        }

        private void ValidaCreditoDebito(InclusaoBanco inclusaoBanco)
        {
            if (inclusaoBanco.Tipocreditodebito == 0)
            {
                if (inclusaoBanco.Credito == null || inclusaoBanco.Credito == "---:--")
                {
                    ModelState["Credito"].Errors.Add("O campo Crédito é obrigatório.");
                }
                inclusaoBanco.Debito = "---:--";
            }
            else
            {
                if (inclusaoBanco.Debito == null || inclusaoBanco.Debito == "---:--")
                {
                    ModelState["Debito"].Errors.Add("O campo Débito é obrigatório.");
                }
                inclusaoBanco.Credito = "---:--";
            }
        }
        private void ValidaJustificativa(InclusaoBanco inclusaoBanco)
        {
            if (!String.IsNullOrEmpty(inclusaoBanco.Justificativa))
            {
                int idJustificativa = JustificativaController.BuscaIdJustificativa(inclusaoBanco.Justificativa);
                if (idJustificativa > 0)
                { inclusaoBanco.IdJustificativa = idJustificativa; }
                else
                { ModelState["Justificativa"].Errors.Add("Justificativa " + inclusaoBanco.Justificativa + " não cadastrada!"); }
            }
            else
            {
                inclusaoBanco.IdJustificativa = null;
            }

        }
        private void VerificaDepartamento(InclusaoBanco inclusaoBanco)
        {
            int idDepartamento = DepartamentoController.BuscaIdDepartamento(inclusaoBanco.Departamento);
            if (idDepartamento > 0)
            {
                inclusaoBanco.Identificacao = idDepartamento;
                inclusaoBanco.Nome = inclusaoBanco.Departamento;
            }
            else
            {
                ModelState["Departamento"].Errors.Add("Departamento " + inclusaoBanco.Departamento + " não cadastrado!");
            }
        }

        private void VerificaEmpresa(InclusaoBanco inclusaoBanco)
        {
            int idEmpresa = EmpresaController.BuscaIdEmpresa(inclusaoBanco.Empresa);
            if (idEmpresa > 0)
            {
                inclusaoBanco.Identificacao = idEmpresa;
                inclusaoBanco.Nome = inclusaoBanco.Empresa;
            }
            else
            {
                ModelState["Empresa"].Errors.Add("Empresa " + inclusaoBanco.Empresa + " não cadastrada!");
            }
        }

        private void VerificaFuncao(InclusaoBanco inclusaoBanco)
        {
            int idFuncao = FuncaoController.BuscaIdFuncao(inclusaoBanco.Funcao);
            if (idFuncao > 0)
            {
                inclusaoBanco.Identificacao = idFuncao;
                inclusaoBanco.Nome = inclusaoBanco.Funcao;
            }
            else
            {
                ModelState["Funcao"].Errors.Add("Função " + inclusaoBanco.Funcao + " não cadastrada!");
            }
        }

        private void VerificaFuncionario(InclusaoBanco inclusaoBanco)
        {
            int idFunc = FuncionarioController.BuscaIdFuncionario(inclusaoBanco.Funcionario);
            if (idFunc > 0)
            {
                inclusaoBanco.Identificacao = idFunc;
                inclusaoBanco.Nome = inclusaoBanco.Funcionario;
            }
            else
            {
                ModelState["Funcionario"].Errors.Add("Função " + inclusaoBanco.Funcionario + " não cadastrada!");
            }
        }
    }
}