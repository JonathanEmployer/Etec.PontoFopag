using BLL_N.IntegracaoTerceiro;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class RepLogAFDController : Controller
    {
        [PermissoesFiltro(Roles = "RepLog")]
        public ActionResult Grid(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.REP bllRep = new BLL.REP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Modelo.REP rep = bllRep.LoadObject(id);
            Modelo.Proxy.PxyResumoRepLogImportacao resumosLog = new Modelo.Proxy.PxyResumoRepLogImportacao();
            resumosLog.Rep = rep;
            return View(resumosLog);
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.REP bllRep = new BLL.REP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                Modelo.REP rep = bllRep.LoadObject(id);
                BLL.RepLog bllRepLog = new BLL.RepLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Proxy.PxyResumoRepLogImportacao> resumosLog = bllRepLog.GetRepLogAFDResumo(rep.NumRelogio);
                JsonResult jsonResult = Json(new { data = resumosLog }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                throw e;
            }
        }

        
    }
}
