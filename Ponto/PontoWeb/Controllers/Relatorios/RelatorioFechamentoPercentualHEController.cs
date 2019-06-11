using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioFechamentoPercentualHEController : Controller
    {
        #region Métodos Get

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioFechamentoPercentualHEConsultar")]
        public ActionResult FechamentoPercentualHE()
        {
            Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
            RelatorioFechamentoPercentualHEModel viewModel = new RelatorioFechamentoPercentualHEModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-31);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 2;
            ViewBag.UtilizaControleContrato = UserPW.UtilizaControleContratos;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioFechamentoPercentualHEConsultar")]
        [HttpPost]
        public ActionResult FechamentoPercentualHE(RelatorioFechamentoPercentualHEModel imp)
        {

            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelFechamentoPercentualHE(imp);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string erros = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return Json(new { Success = false, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult GeraRelFechamentoPercentualHE(RelatorioFechamentoPercentualHEModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioFechamentoPercentualHE(parms);
                return new JsonResult
                {
                    Data = new
                    {
                        success = true,
                        job = ret
                    }
                };
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                ModelState.AddModelError("CustomError", ex.Message);
            }
            return ModelState.JsonErrorResult();
        }
        #endregion

        #region Métodos auxiliares
        private void ValidarRelatorio(RelatorioFechamentoPercentualHEModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        #endregion
    }
}