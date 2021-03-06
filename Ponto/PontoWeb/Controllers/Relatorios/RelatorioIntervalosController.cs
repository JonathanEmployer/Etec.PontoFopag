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
    public class RelatorioIntervalosController : Controller
    {
        #region Métodos Get
        // GET: RelatorioIntervalos
        private IList<bool> pegaOcorrencias;

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioOcorrenciasConsultar")]
        public ActionResult Intervalos()
        {
			RelatorioPadraoModel viewModel = new RelatorioPadraoModel();
			viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            viewModel.FimPeriodo = DateTime.Now.Date;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioOcorrenciasConsultar")]
        [HttpPost]
        public ActionResult Intervalos(RelatorioPadraoModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelIntervalos(imp);
                }
                   catch (Exception ex)
				 {
                BLL.cwkFuncoes.LogarErro(ex);
                ModelState.AddModelError("CustomError", ex.Message);
				}
            }
			return ModelState.JsonErrorResult();

		}

        private ActionResult GeraRelIntervalos(RelatorioPadraoModel parms)
        {
            try
            {
				Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
				HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
				Modelo.Proxy.PxyJobReturn ret = hfm.RelIntervalos(parms);
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
        private void ValidarRelatorio(RelatorioPadraoModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        #endregion
	}
}