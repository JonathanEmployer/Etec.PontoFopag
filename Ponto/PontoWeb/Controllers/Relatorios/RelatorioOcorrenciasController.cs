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
    public class RelatorioOcorrenciasController : Controller
    {
        #region Métodos Get
        // GET: RelatorioOcorrencias
  
		[Authorize]
		[PermissoesFiltro(Roles = "RelatorioOcorrenciasConsultar")]
		public ActionResult Ocorrencias()
        {
            RelatorioOcorrenciasModel viewModel = new RelatorioOcorrenciasModel();
			viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
			viewModel.FimPeriodo = DateTime.Now.Date;
			viewModel.TipoSelecao = 2;
			return View(viewModel);
        }
		#endregion

		#region Métodos Post
		[Authorize]
		[PermissoesFiltro(Roles = "RelatorioCartaoPontoConsultar")]
		[HttpPost]
        public ActionResult Ocorrencias(RelatorioOcorrenciasModel parms)
        {
			ValidarRelatorio(parms);
			if (ModelState.IsValid)
			{
				try
				{
					return GeraRelOcorrencias(parms);
				}
				catch (Exception ex)
				{
					BLL.cwkFuncoes.LogarErro(ex);
					ModelState.AddModelError("CustomError", ex.Message);
				}
			}
			return ModelState.JsonErrorResult();
		}

		private ActionResult GeraRelOcorrencias(RelatorioOcorrenciasModel parms)
        {
            try
            {
				Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
				HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
				Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioOcorrencias(parms);
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
        private void ValidarRelatorio(RelatorioOcorrenciasModel parms)
        {
			ModelState.Remove("FimPeriodo");
			if (String.IsNullOrEmpty(parms.IdSelecionados))
			{
				ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
			}
		}

        #endregion
	}
}