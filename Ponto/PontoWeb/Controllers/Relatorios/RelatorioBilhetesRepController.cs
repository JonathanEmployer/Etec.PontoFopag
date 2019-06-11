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
    public class RelatorioBilhetesRepController : Controller
    {
		private UsuarioPontoWeb _pw = Usuario.GetUsuarioPontoWebLogadoCache();
		// GET: RelatorioBilhetesRep
		[Authorize]
        [PermissoesFiltro(Roles = "RelatorioBilhetesRepConsultar")]
        public ActionResult RelatorioBilhetesRep(int id)
        {
            PxyIdPeriodo filtro = new PxyIdPeriodo() { Id = id, InicioPeriodo = DateTime.Now.AddMonths(-1).Date, FimPeriodo = DateTime.Now.Date };
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.REP bllRep = new BLL.REP(_pw.ConnectionString, _pw);
            ViewBag.Rep = bllRep.LoadObject(id);
            return View(filtro);
        }

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioBilhetesRepConsultar")]
        [HttpPost]
        public ActionResult RelatorioBilhetesRep(PxyIdPeriodo parm)
        {
			BLL.REP bllRep = new BLL.REP(_pw.ConnectionString, _pw);
			DateTime dataFinalB = parm.FimPeriodo;
			string dataFinalMax = dataFinalB.ToString("dd/MM/yyyy");
			if (ModelState.IsValid)
            {
				if (parm.InicioPeriodo > dataFinalB)
				{
					string erros = "Data de Início maior do que o período cadastrado para esse horário. Data Fim máxima permitida: " + dataFinalMax;
					return Json(new { Sucess = false, Erro = erros }, JsonRequestBehavior.AllowGet);
				}

				try
                {
					Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
					HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
					RelatorioPadraoModel parms = new RelatorioPadraoModel() { TipoArquivo = "Excel", IdSelecionados = parm.Id.ToString(), InicioPeriodo = parm.InicioPeriodo, FimPeriodo = dataFinalB };
					Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioBilhetesRep(parms);
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
    
    }
}