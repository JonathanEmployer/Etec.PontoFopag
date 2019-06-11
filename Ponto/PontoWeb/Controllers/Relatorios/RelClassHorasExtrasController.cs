using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelClassHorasExtrasController : Controller
    {
        //#region Métodos Get
        // GET: RelatorioAfastamento
        public ActionResult RelClassHorasExtrasHtml()
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb pw = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(conn, pw);
            DateTime di = Convert.ToDateTime("2016-06-01");
            DateTime df = Convert.ToDateTime("2016-08-30");
            IList<int> funcs = new List<int>() { 179, 107, 10, 11, 1230, 1, 9};
            BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(conn, pw);

            IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> classHorasExtras = bllClassificacaoHorasExtras.RelatorioClassificacaoHorasExtras(funcs.ToList(), di, df, 0);

            return View(classHorasExtras);
        }
        //#endregion


        #region Métodos Get
        // GET: RelatorioAfastamento
        [PermissoesFiltro(Roles = "RelClassHorasExtrasConsultar")]
        public ActionResult Index()
        {
            RelatorioClassificacaoHorasExtrasModel viewModel = new RelatorioClassificacaoHorasExtrasModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 0;
            return View(viewModel);
        }

        #endregion
        #region Métodos Post
        [HttpPost]
        [PermissoesFiltro(Roles = "RelClassHorasExtrasConsultar")]
        public ActionResult Index(RelatorioClassificacaoHorasExtrasModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelClassificacaoHorasExtras(imp);
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

        private void ValidarRelatorio(RelatorioClassificacaoHorasExtrasModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
            if (imp.TipoSelecao < 0 || imp.TipoSelecao > 2)
            {
                ModelState.AddModelError("TipoSelecao", "Tipo inválido");
            }
        }

        private ActionResult GeraRelClassificacaoHorasExtras(RelatorioClassificacaoHorasExtrasModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioClassificacaoHorasExtras(parms);
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
    }

}