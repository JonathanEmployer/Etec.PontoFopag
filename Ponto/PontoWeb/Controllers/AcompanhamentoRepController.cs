using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class AcompanhamentoRepController : Controller
    {
        // GET: AcompanhamentoRep
        public ActionResult Index()
        {
            return RedirectToAction("Grid");
        }

        public ActionResult Grid()
        {
            //Lista de Acompanhamentos
            return View();
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.AcompanhamentoRep bllREP = new BLL.AcompanhamentoRep(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<AcompanhamentoRep> dados = bllREP.GetAll().ConvertDataTable<AcompanhamentoRep>();
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        public ActionResult Acompanhamento()
        {
            return View(new AcompanhamentoRep());
        }
    
        [HttpPost]
        public ActionResult Acompanhamento(Modelo.AcompanhamentoRep acompanhamento)
        {
            return RedirectToAction("Grid", "Rep");
        }
    }
}