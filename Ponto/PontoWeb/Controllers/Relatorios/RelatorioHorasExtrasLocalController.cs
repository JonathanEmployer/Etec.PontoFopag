using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
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
    public class RelatorioHorasExtrasLocalController : Controller
    {
        #region Métodos Get
        // GET: RelatorioEspelho

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHorasExtrasLocalConsultar")]
        public ActionResult HorasExtras()
        {
            RelatorioHorasExtrasLocalModel viewModel = new RelatorioHorasExtrasLocalModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            viewModel.FimPeriodo = DateTime.Now.Date;
            return View(viewModel);
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt));
                Modelo.UsuarioPontoWeb usuarioPW = Usuario.GetUsuarioPontoWebLogadoCache();
                List<Modelo.Proxy.Relatorios.PxyGridRelHorasExtrasLocal> gridRelHorasExtrasLocal = bllRel.GetListagemRelHorasExtrasLocal(usuarioPW);
                JsonResult jsonResult = Json(new { data = gridRelHorasExtrasLocal }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        public JsonResult DadosLocalGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt));
                Modelo.UsuarioPontoWeb usuarioPW = Usuario.GetUsuarioPontoWebLogadoCache();
                List<Modelo.Proxy.Relatorios.PxyGridLocaisHorasExtrasLocal> gridRelLocaisHorasExtrasLocal = bllRel.GetListagemRelLocaisHorasExtrasLocal(usuarioPW);
                JsonResult jsonResult = Json(new { data = gridRelLocaisHorasExtrasLocal }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHorasExtrasLocalConsultar")]
        [HttpPost]
        public ActionResult HorasExtras(RelatorioHorasExtrasLocalModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelExcel(imp);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                return View(pxyRelOcorrencias.Produce(pxyRelEspelho.Produce(new RelatoriosPontoWeb(new DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt)).GetListagemFuncionariosRel(userPW))));
            }
        }

        private ActionResult GeraRelExcel(RelatorioHorasExtrasLocalModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioHorasExtrasLocal(parms);
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
        private void ValidarRelatorio(RelatorioHorasExtrasLocalModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }
        #endregion
    }
}