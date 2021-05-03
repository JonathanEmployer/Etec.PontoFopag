using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LancamentoCartaoPontoController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "LancamentoCartaoPontoCadastrar")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        [HttpGet]
        public ActionResult Cadastro()
        {
            bool UtilizaControleContrato = _usr.UtilizaControleContratos;
            ViewBag.UtilizaControleContrato = UtilizaControleContrato;
            LancamentoCartaoPonto lancamentoCartaoPonto = new LancamentoCartaoPonto();
            lancamentoCartaoPonto.Regs = new List<LancamentoCartaoPontoRegistros>();

            if (TempData["lcp"] is null)
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                DateTime dataIni = new DateTime();
                DateTime dataFin = new DateTime();
                bool carregouPeriodoFechamento = false;
                bool mudadataautomaticamente;

                lancamentoCartaoPonto.Empresa = "0 | TODAS AS EMPRESAS";
                lancamentoCartaoPonto.Departamento = "0 | TODOS OS DEPARTAMENTOS";
                lancamentoCartaoPonto.Contrato = "0 | TODOS OS CONTRATOS";

                BLL.Empresa bllEmpresa = new BLL.Empresa(_usr.ConnectionString, _usr);
                List<Empresa> empresas = bllEmpresa.GetAllList();
                if (empresas.Count == 1)
                {
                    lancamentoCartaoPonto.Empresa = empresas.FirstOrDefault().Codigo + " | " + empresas.FirstOrDefault().Nome;
                    if (!_usr.UtilizaControleContratos)
                    {
                        BLL.Departamento bllDepartamento = new BLL.Departamento(_usr.ConnectionString, _usr);
                        List<Departamento> departs = bllDepartamento.LoadPEmpresa(empresas.FirstOrDefault().Id);
                        if (departs.Count() == 1)
                        {
                            lancamentoCartaoPonto.Departamento = departs.FirstOrDefault().Codigo + " | " + departs.FirstOrDefault().Descricao;
                        }
                    }
                    else
                    {
                        BLL.Contrato bllContrato = new BLL.Contrato(_usr.ConnectionString, _usr);
                        List<Contrato> contratos = bllContrato.GetAllListPorEmpresa(empresas.FirstOrDefault().Id);
                        if (contratos.Count() == 1)
                        {
                            lancamentoCartaoPonto.Contrato = contratos.FirstOrDefault().Codigo + " | " + contratos.FirstOrDefault().DescricaoContrato;
                            carregouPeriodoFechamento = BLL.cwkFuncoes.AtribuiPeriodoFechamentoPonto(out dataIni, out dataFin, contratos.FirstOrDefault().DiaFechamentoInicial, contratos.FirstOrDefault().DiaFechamentoFinal);
                        }
                    }

                    if (!carregouPeriodoFechamento)
                    {
                        carregouPeriodoFechamento = BLL.cwkFuncoes.AtribuiPeriodoFechamentoPonto(out dataIni, out dataFin, empresas.FirstOrDefault().DiaFechamentoInicial, empresas.FirstOrDefault().DiaFechamentoFinal);
                    }
                }

                if (!carregouPeriodoFechamento)
                {
                    BLL.ConfiguracoesGerais bllConfiguracoes = new BLL.ConfiguracoesGerais(_usr.ConnectionString, _usr);
                    bllConfiguracoes.AtribuiDatas(String.Empty, out dataIni, out dataFin, out mudadataautomaticamente);
                }

                if (dataIni != new DateTime())
                {
                    lancamentoCartaoPonto.DataInicial = dataIni.ToShortDateString();
                }
                if (dataFin != new DateTime())
                {
                    lancamentoCartaoPonto.DataFinal = dataFin.ToShortDateString();
                } 
            }
            else
            {
                var obj = (LancamentoCartaoPonto)TempData["lcp"];
                lancamentoCartaoPonto.Contrato = obj.Contrato;
                lancamentoCartaoPonto.DataFinal = obj.DataFinal;
                lancamentoCartaoPonto.DataInicial = obj.DataInicial;
                lancamentoCartaoPonto.Departamento = obj.Departamento;
                lancamentoCartaoPonto.DescJustificativa = obj.DescJustificativa;
                lancamentoCartaoPonto.Empresa = obj.Empresa;
                lancamentoCartaoPonto.Motivo = obj.Motivo;
                lancamentoCartaoPonto.QuantidadeRegistros = obj.QuantidadeRegistros;
            }
            return View(lancamentoCartaoPonto);
        }

        [PermissoesFiltro(Roles = "LancamentoCartaoPontoCadastrar")]
        [HttpPost]
        public ActionResult Cadastro(LancamentoCartaoPonto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BLL.LancamentoCartaoPonto bllLancamentoCartaoPonto = new BLL.LancamentoCartaoPonto(_usr.ConnectionString, _usr);
                    obj.IdJustificativa = JustificativaController.BuscaIdJustificativa(obj.DescJustificativa);
                    obj.IdFuncionario = FuncionarioController.BuscaIdFuncionario(obj.Funcionario);

                    Dictionary<string, string> erros = bllLancamentoCartaoPonto.SalvarCartaoPonto(obj, out List<BilhetesImp> bilhetes, out Funcionario funcionario);

                    if (erros.Count == 0)
                    {

                        UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                        HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW, "", "/LancamentoCartaoPonto/Cadastro");
                        Modelo.Proxy.PxyJobReturn job = hfm.RecalculaMarcacao("Processamento de Cartão Ponto Manual", $"Processando Funcionário { funcionario.Dscodigo } | { funcionario.Nome } para { bilhetes.Count() } registros lançados", new List<int>() { funcionario.Id }, bilhetes.Min(m => m.Data), bilhetes.Max(m => m.Data));
                        TempData["lcp"] = new LancamentoCartaoPonto()
                        {
                            Contrato = obj.Contrato,
                            DataFinal = obj.DataFinal,
                            DataInicial = obj.DataInicial,
                            Departamento = obj.Departamento,
                            DescJustificativa = obj.DescJustificativa,
                            Empresa = obj.Empresa,
                            Funcionario = obj.Funcionario,
                            Motivo = obj.Motivo,
                            QuantidadeRegistros = obj.QuantidadeRegistros
                        };
                        return RedirectToAction("Cadastro");
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> erro in erros)
                        {
                            ModelState.AddModelError(erro.Key, erro.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("CustomError", e.Message);
            }
            
            return View(obj);
        }

        public ActionResult GridMarcacoes(LancamentoCartaoPonto lcp)
        {
            try
            {
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                string erro = "";
                Modelo.Funcionario func = bllFuncionario.ValidaEmpDepFunc(lcp.Empresa, lcp.Departamento, lcp.Funcionario, ref erro);
                if (erro == "" && func != null && func.Id > 0)
                {
                    bool UtilizaControleContrato = _usr.UtilizaControleContratos;
                    ViewBag.UtilizaControleContrato = UtilizaControleContrato;
                    List<Funcionario> fechamento = bllFuncionario.GetAllListComUltimosFechamentos(true, new List<int>() { func.Id });
                    if (fechamento != null && fechamento.Count > 0 && fechamento.FirstOrDefault().DataUltimoFechamento.HasValue && fechamento.FirstOrDefault().DataUltimoFechamento.GetValueOrDefault() >= Convert.ToDateTime(lcp.DataInicial))
                    {
                        lcp.Regs = new List<LancamentoCartaoPontoRegistros>();
                        ViewBag.Fechamento = $"Funcionário possui fechamento de ponto em { fechamento.FirstOrDefault().DataUltimoFechamento.GetValueOrDefault().ToShortDateString() }, não é possível alterar o período informado.";
                    }
                    else
                    {
                        lcp.Regs = bllMarcacao.GetLancamentoCartaoPonto(func.Id, Convert.ToDateTime(lcp.DataInicial), Convert.ToDateTime(lcp.DataFinal));
                    }
                    return PartialView(lcp);
                }
                else
                {
                    return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception w)
            {
                BLL.cwkFuncoes.LogarErro(w);
                return Json(new { erro = true, mensagemErro = w.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}