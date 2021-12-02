using BLL_N.IntegracaoTerceiro;
using BLL_N.JobManager.Hangfire;
using iTextSharp.text;
using Modelo;
using Modelo.Proxy;
using Modelo.Proxy.Relatorios;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioHorasInItinereController : Controller
    {
		#region Métodos Get
		// GET: RelatorioAfastamento
		public ActionResult RelatorioHtml()
		{
			string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
			UsuarioPontoWeb pw = Usuario.GetUsuarioPontoWebLogadoCache();
			BLL.HorasInItinere bllHorasInItinere = new BLL.HorasInItinere(conn, pw);
			DateTime di = Convert.ToDateTime("2016-12-01");
			DateTime df = Convert.ToDateTime("2016-12-30");
			IList<int> funcs = new List<int>() { 1281, 51, 107 };
			IList<PxyRelatorioHorasInItinere> cps = bllHorasInItinere.BuscaDadosRelatorio(funcs, di, df, null);
			return View(cps.ToList());
		}
        #endregion


        #region Métodos Get
        // GET: RelatorioAfastamento
        [PermissoesFiltro(Roles = "RelatorioHorasInItinereConsultar")]
        public ActionResult Index()
        {
            RelatorioHorasInItinereModel viewModel = new RelatorioHorasInItinereModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            viewModel.FimPeriodo = DateTime.Now.Date;
			viewModel.TipoSelecao = 2;
			return View(viewModel);
		}
        #endregion
        #region Métodos Post
        [HttpPost]
        [PermissoesFiltro(Roles = "RelatorioHorasInItinereConsultar")]
        public ActionResult Index(RelatorioHorasInItinereModel imp)
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

		private ActionResult GeraRelatorio(RelatorioHorasInItinereModel parms)
		{
			try
			{
				Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
				HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
				Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioInItinere(parms);
				JobController jc = new JobController();
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
		private void ValidarRelatorio(RelatorioHorasInItinereModel imp)
        {
			ModelState.Remove("FimPeriodo");
			if (String.IsNullOrEmpty(imp.IdSelecionados))
			{
				ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
			}
		}

		#endregion

	}

}