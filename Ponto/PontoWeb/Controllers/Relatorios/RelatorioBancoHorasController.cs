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
    public class RelatorioBancoHorasController : Controller
    {
        #region Métodos Get

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioBancoHorasConsultar")]
        public ActionResult BancoHoras()
        {
            RelatorioBancoHorasModel viewModel = new RelatorioBancoHorasModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-31);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 2;
            return View(viewModel);
        }
        #endregion

        public JsonResult GetFuns()
        {
            try
            {
                Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                pxyRelCartaoPonto viewModel = pxyRelCartaoPonto.Produce(new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt)).GetListagemRelBancoHoras(userPW));
                JsonResult jsonResult = Json(new { data = viewModel.FuncionariosRelatorio }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioBancoHorasConsultar")]
        [HttpPost]
        public ActionResult BancoHoras(RelatorioBancoHorasModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelBancoHoras(imp);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }

        private ActionResult GeraRelBancoHoras(RelatorioBancoHorasModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioBancoHoras(parms);
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
        private void ValidarRelatorio(RelatorioBancoHorasModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        #endregion
	}
}