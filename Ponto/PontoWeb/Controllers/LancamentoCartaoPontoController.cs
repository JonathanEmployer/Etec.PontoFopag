using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LancamentoCartaoPontoController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        //[PermissoesFiltro(Roles = "LancamentoCartaoPontoCadastrar")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult Index()
        {
            bool UtilizaControleContrato = _usr.UtilizaControleContratos;
            ViewBag.UtilizaControleContrato = UtilizaControleContrato;
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            DateTime dataIni = new DateTime();
            DateTime dataFin = new DateTime();
            bool carregouPeriodoFechamento = false;
            bool mudadataautomaticamente;

            LancamentoCartaoPonto lancamentoCartaoPonto = new LancamentoCartaoPonto();

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
            return View(lancamentoCartaoPonto);
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
                    List<Modelo.LancamentoCartaoPontoRegistros> regs = bllMarcacao.GetLancamentoCartaoPonto(func.Id, Convert.ToDateTime(lcp.DataInicial), Convert.ToDateTime(lcp.DataFinal));
                    lcp.Regs = regs;
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