using BLL.Util;
using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioFuncionariosRepController : Controller
    {
		private UsuarioPontoWeb _pw = Usuario.GetUsuarioPontoWebLogadoCache();
	

		[Authorize]
		[PermissoesFiltro(Roles = "RelatorioFuncionariosRepConsultar")]
		public ActionResult RelatorioFuncionariosRep(int id, bool? operacao)
		{
			PxyIdPeriodo filtro = new PxyIdPeriodo() { Id = id, InicioPeriodo = DateTime.Now.AddMonths(-1).Date, FimPeriodo = DateTime.Now.Date };
			var usr = Usuario.GetUsuarioPontoWebLogadoCache();
			BLL.REP bllRep = new BLL.REP(_pw.ConnectionString, _pw);
			ViewBag.Rep = bllRep.LoadObject(id);
			List<Modelo.Proxy.pxyFuncionarioRep> pxyFuncionarioReps = new List<pxyFuncionarioRep>();
			pxyFuncionarioReps = bllRep.LoadObjectListFuncionariosRep(id, operacao);
			ViewBag.Func = pxyFuncionarioReps;
			

			return View(filtro);
		}
		



		[Authorize]
		public JsonResult DadosFuncionario(int id, bool operacao)
		{
			try
			{
				BLL.REP bllRep = new BLL.REP(_pw.ConnectionString, _pw);
				List<pxyFuncionarioRep> dados = bllRep.LoadObjectListFuncionariosRep(id, operacao);
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

	}
}