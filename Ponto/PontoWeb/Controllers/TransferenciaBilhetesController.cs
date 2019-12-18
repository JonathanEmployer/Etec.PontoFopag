using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    [Authorize]
    public class TransferenciaBilhetesController : Controller
    {
        
        [HttpGet]
        // GET: TransferenciaBilhetes
        public ActionResult Index()
        {
            return View(new PxyTransferenciaBilhetes());
        }

        [HttpGet]
        public ActionResult GridOutrosRegistrosFuncionario(PxyTransferenciaBilhetes transferenciaBilhetes)
        {
            int id = FuncionarioController.BuscaIdFuncionario(transferenciaBilhetes.FuncionarioOrigem);
            transferenciaBilhetes.IdFuncionarioOrigem = id;
            return View(transferenciaBilhetes);
        }

        [HttpGet]
        public JsonResult GetOutrosRegistrosFuncionario(int id)
        {
            var userPW = Usuario.GetUsuarioPontoWebLogadoCache();

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(userPW.ConnectionString, userPW);
            List<pxyFuncionarioGrid> funcs = bllFuncionario.GetRegistrosEmpregoFuncionario(id);

            JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}