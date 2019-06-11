using PontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class CalculosMarcacaoController : Controller
    {
        // GET: /CalculosMarcacao/
        public ActionResult Index()
        {
            return View();
        }
    }
}