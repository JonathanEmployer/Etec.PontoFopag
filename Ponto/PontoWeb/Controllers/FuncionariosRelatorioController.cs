using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class FuncionariosRelatorioController : Controller
    {
        // GET: FuncionariosRelatorio
        public ActionResult Index()
        {
            return View();
        }

        // GET: FuncionariosRelatorio
        [HttpGet]
        public ActionResult GridFuncionarios(string idsSelecionados)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            pxyRelPontoWeb RelPadrao = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(conn)).GetListagemFuncionariosRel(userPW);
            RelPadrao.idSelecionados = idsSelecionados;
            RelPadrao.UtilizaControleContrato = userPW.UtilizaControleContratos;
            RelPadrao.InicioPeriodo = DateTime.Now;
            RelPadrao.FimPeriodo = DateTime.Now;
            return View(RelPadrao);
        }

        [HttpPost]
        public ActionResult GridFuncionariosPost(string idsSelecionados)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            pxyRelPontoWeb RelPadrao = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(conn)).GetListagemFuncionariosRel(userPW);
            RelPadrao.idSelecionados = idsSelecionados;
            RelPadrao.UtilizaControleContrato = userPW.UtilizaControleContratos;
            RelPadrao.InicioPeriodo = DateTime.Now;
            RelPadrao.FimPeriodo = DateTime.Now;
            return View("GridFuncionarios", RelPadrao);
        }
    }
}