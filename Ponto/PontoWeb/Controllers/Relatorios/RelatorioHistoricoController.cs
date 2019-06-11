using BLL_N.JobManager.Hangfire;
using Modelo.Proxy;
using Modelo.Relatorios;
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
    public class RelatorioHistoricoController : Controller
    {
        #region Métodos Get
        // GET: RelatorioHistorico

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHistoricoConsultar")]
        public ActionResult Historico()
        {
            Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            RelatorioPadraoModel viewModel = new RelatorioPadraoModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 2;
            ViewBag.UtilizaControleContrato = userPW.UtilizaControleContratos;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHistoricoConsultar")]
        [HttpPost]
        public ActionResult Historico(RelatorioPadraoModel parms)
        {
            ValidarRelatorio(parms);
            if (ModelState.IsValid)
            {
                try
                {
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioHistorico(parms);

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

        private ActionResult GeraRelHistorico(pxyRelHistorico imp)
        {
            try
            {
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usr);

                RelatoriosController rc = new RelatoriosController();
                JobController jc = new JobController();
                DataTable Dt = new DataTable();
                string nomeEmpresa = String.Empty;

                Modelo.Empresa emp = bllEmpresa.GetEmpresaPrincipal();
                nomeEmpresa = emp.Nome;
                Job job = jc.GetRelatorioHistorico(imp, Dt, nomeEmpresa, conn, usr);
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
        #endregion

        #region Métodos auxiliares
        private void ValidarRelatorio(RelatorioPadraoModel parm)
        {
            if (String.IsNullOrEmpty(parm.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }
        #endregion
	}
}