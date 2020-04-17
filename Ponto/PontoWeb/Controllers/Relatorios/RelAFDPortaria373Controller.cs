using BLL_N.JobManager.Hangfire;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelAFDPortaria373Controller : Controller
    {
        // GET: RelAFDPortaria373
        public ActionResult Index()
        {
            RelatorioAfdPortaria373Model rel = new RelatorioAfdPortaria373Model();
            rel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            rel.FimPeriodo = DateTime.Now.Date;
            return View(rel);
        }

        [HttpPost]
        public ActionResult Index(RelatorioAfdPortaria373Model rel)
        {
            ValidarRelatorio(rel);
            if (ModelState.IsValid)
            {
                try
                {
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioAFDPortaria373(rel);
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

        [Authorize]
        public JsonResult GetReps()
        {
            try
            {
                Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.REP bllREP = new BLL.REP(userPW.ConnectionString, userPW);

                List<Modelo.Proxy.PxyGridRepsPortaria373> reps = bllREP.GetGridRepsPortaria373();

                JsonResult jsonResult = Json(new { data = reps }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Métodos auxiliares
        private void ValidarRelatorio(RelatorioAfdPortaria373Model imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Nenhum funcionário selecionado");
            }
        }
        #endregion
    }
}