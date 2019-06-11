using BLL_N.IntegracaoTerceiro;
using BLL_N.JobManager.Hangfire;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Modelo;
using Modelo.Proxy;
using Modelo.Proxy.Relatorios;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using PontoWeb.Utils;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioHorasTrabalhadasNoturnasController : Controller
    {        
        #region Métodos Get
        // GET: RelatorioAfastamento
        [PermissoesFiltro(Roles = "RelatorioCartaoPontoCustomConsultar")]
        public ActionResult Index()
        {
            RelatorioPadraoModel viewModel = new RelatorioPadraoModel();
			viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
			viewModel.FimPeriodo = DateTime.Now.Date;
			return View(viewModel);
        }
        #endregion
        #region Métodos Post
        [HttpPost]
        [PermissoesFiltro(Roles = "RelatorioCartaoPontoCustomConsultar")]
        public ActionResult Index(RelatorioPadraoModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelatorio(imp);
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

        private void ValidarRelatorio(RelatorioPadraoModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        private ActionResult GeraRelatorio(RelatorioPadraoModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
				HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
				Modelo.Proxy.PxyJobReturn ret = hfm.RelHorasTrabalhadasNoturnas(parms);
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
    }

}