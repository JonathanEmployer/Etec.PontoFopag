using BLL;
using DAL.SQL;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using PontoWeb.Utils;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
using System.Web.Script.Serialization;
using PontoWeb.Controllers.JobManager;
using BLL_N.JobManager.Hangfire;
using PontoWeb.Models.Helpers;
using Modelo.Relatorios;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioHomemHoraController : Controller
    {
        #region Métodos Get
        // GET: RelatorioEspelho

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHomemHoraConsultar")]
        public ActionResult HomemHora()
        {
            RelatorioHomemHoraModel RelPadrao = RelPadrao = new RelatorioHomemHoraModel();
            RelPadrao.InicioPeriodo = DateTime.Now.AddMonths(-1);
            RelPadrao.FimPeriodo = DateTime.Now;
            RelPadrao = RecuperaFiltro(RelPadrao);
            return View(RelPadrao);
        }

        private RelatorioHomemHoraModel RecuperaFiltro(RelatorioHomemHoraModel RelPadrao)
        {
            HttpCookie cookie = Request.Cookies.Get(CriptoString.Encrypt(Request.Path + "_" + User.Identity.Name));
            if (cookie != null)
            {
                RelPadrao = new JavaScriptSerializer().Deserialize<RelatorioHomemHoraModel>(CriptoString.Decrypt(cookie.Value));
            }

            return RelPadrao;
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHomemHoraConsultar")]
        [HttpPost]
        public ActionResult HomemHora(RelatorioHomemHoraModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                GuardaFiltro(imp);
                try
                {
                    return GeraRelExcel(imp);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }

        private void GuardaFiltro(RelatorioHomemHoraModel imp)
        {
            string filtroJson = new JavaScriptSerializer().Serialize(imp);
            var cookie = new HttpCookie(CriptoString.Encrypt(Request.Path + "_" + User.Identity.Name), CriptoString.Encrypt(filtroJson))
            {
                Expires = DateTime.Now.AddMonths(1)
            };
            HttpContext.Response.Cookies.Add(cookie);
        }

        private ActionResult GeraRelExcel(RelatorioHomemHoraModel parms)
        {
            try
            {

                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioHomemHora(parms);

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
        private void ValidarRelatorio(RelatorioHomemHoraModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Nenhum funcionário selecionado");
            }
        }
        #endregion
    }
}