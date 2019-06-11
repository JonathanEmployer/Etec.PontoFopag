using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Modelo.Proxy;
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
	public class RelatorioAbsenteismoController : Controller
	{
		#region Métodos Get
		// GET: RelatorioAbsenteismo
		[Authorize]
		[PermissoesFiltro(Roles = "RelatorioAbsenteismoConsultar")]
		public ActionResult Absenteismo()
		{
			Modelo.Relatorios.RelatorioAbsenteismoModel viewModel = new Modelo.Relatorios.RelatorioAbsenteismoModel();
			viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
			viewModel.FimPeriodo = DateTime.Now.Date;
			viewModel.TipoSelecao = 2;
			return View(viewModel);
		}
		#endregion

		#region Métodos Post
		[Authorize]
		[PermissoesFiltro(Roles = "RelatorioAbsenteismoConsultar")]
		[HttpPost]
		public ActionResult Absenteismo(Modelo.Relatorios.RelatorioAbsenteismoModel parms)
		{
			string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;

			ValidarRelatorio(parms);
			if (ModelState.IsValid)
			{
				try
				{
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioAbsenteismo(parms);
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
            }
            return ModelState.JsonErrorResult();
        }
		#endregion

		#region Métodos auxiliares
		private void ValidarRelatorio(Modelo.Relatorios.RelatorioAbsenteismoModel parms)
		{
			if (String.IsNullOrEmpty(parms.IdSelecionados))
			{
				ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
			}
		}
		#endregion
	}
}