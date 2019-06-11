using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioConclusoesBloqueioPnlRhController : Controller
    {
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioConclusoesBloqueioPnlRhConsultar")]
        public ActionResult ConclusoesBloqueioPnlRh()
        {
            RelatorioConclusoesBloqueioPnlRhModel viewModel = new RelatorioConclusoesBloqueioPnlRhModel();
            Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
            viewModel.UtilizaControleContrato = UserPW.UtilizaControleContratos;
            viewModel.InicioPeriodo = DateTime.Now.AddMonths(-1);
            viewModel.FimPeriodo = DateTime.Now;
            return View(viewModel);
        }

        #region Métodos Post

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioConclusoesBloqueioPnlRhConsultar")]
        [HttpPost]
        public ActionResult ConclusoesBloqueioPnlRh(RelatorioConclusoesBloqueioPnlRhModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelExcel(imp);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }

        #endregion

        private ActionResult GeraRelExcel(RelatorioConclusoesBloqueioPnlRhModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioConclusoesBloqueioPnlRh(parms);

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

        #region Métodos auxiliares
        private void ValidarRelatorio(RelatorioConclusoesBloqueioPnlRhModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }
        #endregion
    }
}