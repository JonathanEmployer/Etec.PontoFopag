using BLL_N.JobManager.Hangfire;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Data;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioAbonoController : Controller
    {
        #region Métodos Get
        // GET: RelatorioAbono
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioAbonoConsultar")]
        public ActionResult Abono()
        {
            Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            Modelo.Relatorios.RelatorioAbonoModel viewModel = new Modelo.Relatorios.RelatorioAbonoModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddMonths(-1);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.bOcorrencia = true;
            ViewBag.UtilizaControleContrato = userPW.UtilizaControleContratos;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioAbonoConsultar")]
        [HttpPost]
        public ActionResult Abono(Modelo.Relatorios.RelatorioAbonoModel parms)
        {
            ValidarRelatorio(parms);

            if (ModelState.IsValid)
            {
                try
                {
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioAbono(parms);
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

        private ActionResult GeraRelAbono(pxyRelOcorrencias imp)
        {
            try
            {
                RelatoriosController rc = new RelatoriosController();
                JobController jc = new JobController();

                DataTable dt = new DataTable();
                Job job = jc.GetRelatorioAbono(imp, dt, Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                return Json(new
                {
                    JobId = job.Id,
                    Progress = job.Progress,
                    Erro = "",
                    AbrirNovaAba = true
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void ValidarRelatorio(Modelo.Relatorios.RelatorioAbonoModel parms)
        {
            if (String.IsNullOrEmpty(parms.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        #endregion
    }
}