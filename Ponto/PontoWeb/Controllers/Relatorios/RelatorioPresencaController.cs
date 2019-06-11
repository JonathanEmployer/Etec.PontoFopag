using BLL_N.JobManager.Hangfire;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using System;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
	public class RelatorioPresencaController : Controller
    {
        #region Métodos Get
        // GET: RelatorioAfastamento
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioPresencaConsultar")]
        public ActionResult Presenca()
        {
			RelatorioPadraoModel viewModel = new RelatorioPadraoModel();
			viewModel.InicioPeriodo = DateTime.Now;
			viewModel.FimPeriodo = DateTime.Now.Date;
			viewModel.TipoSelecao = 2;
			return View(viewModel);
		}
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioPresencaConsultar")]
        [HttpPost]
        public ActionResult Presenca(RelatorioPadraoModel imp)
        {
			ValidarRelatorio(imp);
			if (ModelState.IsValid)
			{
				try
				{
					return GeraRelPresenca(imp);
				}
				catch (Exception ex)
				{
					BLL.cwkFuncoes.LogarErro(ex);
					ModelState.AddModelError("CustomError", ex.Message);
				}
			}
			return ModelState.JsonErrorResult();
		}

        private ActionResult GeraRelPresenca(RelatorioPadraoModel parms)
        {
			try
			{
				Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
				HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
				Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioPresenca(parms);
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
			ModelState.Remove("FimPeriodo");
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

		#endregion
	}
}