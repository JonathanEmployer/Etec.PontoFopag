using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using System;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioBilhetesImpController : Controller
    {
        #region Métodos Get
        // GET: RelatorioEspelho

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHomemHoraConsultar")]
        public ActionResult BilhetesImp()
        {
            RelatorioPadraoModel rel = new RelatorioPadraoModel();
            rel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            rel.FimPeriodo = DateTime.Now.Date;
            return View(rel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHomemHoraConsultar")]
        [HttpPost]
        public ActionResult BilhetesImp(RelatorioPadraoModel imp)
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
                return View(pxyRelOcorrencias.Produce(new RelatoriosPontoWeb(new DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt)).GetListagemRelBase()));
            }
        }

        private ActionResult GeraRelExcel(RelatorioPadraoModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioBilhetesImp(parms);
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
                ModelState.AddModelError("CustomError", "Nenhum funcionário selecionado");
            }
        }
        #endregion
    }
}