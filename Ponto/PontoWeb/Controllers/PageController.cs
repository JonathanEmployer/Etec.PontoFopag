using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class PageController : Controller
    {
        public ActionResult Erro(string mensagem)
        {
            ViewBag.Titulo = "Erro encontrado ao executar a operação.";
            ViewBag.Erro = mensagem;
            return View();
        }

        public ActionResult HttpError404(string mensagem)
        {
            ViewBag.Titulo = "Página não encontrada.";
            mensagem = mensagem.Replace("O controlador do", "O");
            mensagem = mensagem.Replace(" ou não implementa IController","");
            ViewBag.Erro = mensagem;
            return View("Erro");
        }
    }
}