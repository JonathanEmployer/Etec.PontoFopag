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
    public class RelatorioEspelhoController : Controller
    {
        #region Métodos Get
        // GET: RelatorioEspelho

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioEspelhoConsultar")]
        public ActionResult Espelho()
        {         
            RelatorioPadraoModel viewModel = new RelatorioPadraoModel();
			viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
			viewModel.FimPeriodo = DateTime.Now.Date;
			viewModel.TipoSelecao = 2;
			return View(viewModel);

		}
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioEspelhoConsultar")]
        [HttpPost]
        public ActionResult Espelho(RelatorioPadraoModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelEspelho(imp);
                }
				catch (Exception ex)
				{
					BLL.cwkFuncoes.LogarErro(ex);
					ModelState.AddModelError("CustomError", ex.Message);
				}
			}
            else
            {
                return View(pxyRelOcorrencias.Produce(new RelatoriosPontoWeb(new DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt)).GetListagemRelBase()));
            }
			return ModelState.JsonErrorResult();
		}

        private ActionResult GeraRelEspelho(RelatorioPadraoModel parms)
        {
            try
            {
				Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
				HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
				Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioEspelho(parms);
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