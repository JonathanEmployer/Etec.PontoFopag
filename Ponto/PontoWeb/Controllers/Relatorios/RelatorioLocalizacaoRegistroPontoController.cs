using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioLocalizacaoRegistroPontoController : Controller
    {
        //#region Métodos Get
        // GET: RelatorioAfastamento
        private ActionResult RelatorioLocalizacaoRegistroPontoHtml()
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb pw = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(conn, pw);
            DateTime di = Convert.ToDateTime("2016-08-01");
            DateTime df = Convert.ToDateTime("2016-08-30");
            IList<int> funcs = new List<int>() { 70, 88, 123 };
            BLL.LocalizacaoRegistroPonto bllLocalizacaoRegistroPonto = new BLL.LocalizacaoRegistroPonto(conn, pw);

            IList<Modelo.Proxy.Relatorios.PxyRelLocalizacaoRegistroPonto> LocRegPonto = bllLocalizacaoRegistroPonto.RelLocalizacaoRegistroPonto(funcs.ToList(), di, df);

            return View(LocRegPonto);
        }
        //#endregion


        #region Métodos Get
        // GET: RelatorioAfastamento
        [PermissoesFiltro(Roles = "RelatorioLocalizacaoRegistroPontoConsultar")]
        public ActionResult Index()
        {
            RelatorioPadraoModel rel = new RelatorioPadraoModel();
            rel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            rel.FimPeriodo = DateTime.Now.Date;
            rel.TipoSelecao = 2;
            return View(rel);
        }
        #endregion
        #region Métodos Post
        [HttpPost]
        [PermissoesFiltro(Roles = "RelatorioLocalizacaoRegistroPontoConsultar")]
        public ActionResult Index(RelatorioPadraoModel param)
        {
            ValidarRelatorio(param);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelLocalizacaoRegistroPonto(param);
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

        private void ValidarRelatorio(RelatorioPadraoModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        private ActionResult GeraRelLocalizacaoRegistroPonto(RelatorioPadraoModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioLocalizacaoRegistroPonto(parms);
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