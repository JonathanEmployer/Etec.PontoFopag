using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using System;
using System.Web.Mvc;
using BLL_N.JobManager.Hangfire;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioCartaoPontoController : Controller
    {
        #region Métodos Get
        // GET: RelatorioAfastamento
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioCartaoPontoConsultar")]
        public ActionResult CartaoPonto()
        {
            Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            RelatorioCartaoPontoModel viewModel = new Modelo.Relatorios.RelatorioCartaoPontoModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 2;
            ViewBag.UtilizaControleContrato = userPW.EmpresaPrincipal.UtilizaControleContratos;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioCartaoPontoConsultar")]
        [HttpPost]
        public ActionResult CartaoPonto(RelatorioCartaoPontoModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioCartaoPonto(imp);
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
        #endregion

        #region Métodos auxiliares
        private void ValidarRelatorio(RelatorioCartaoPontoModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }
        #endregion
    }
}