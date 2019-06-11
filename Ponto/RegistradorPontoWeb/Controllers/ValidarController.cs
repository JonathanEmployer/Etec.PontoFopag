using Newtonsoft.Json;
using RegistradorPontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistradorPontoWeb.Controllers
{
    public class ValidarController : Controller
    {
        public ActionResult Index()
        {
            Comprovante comp = new Comprovante();
            return View(comp);
        }

        [HttpPost]
        public ActionResult Index(Comprovante comprovante)
        {
            try
            {
                List<string> pedadosChave = comprovante.ChaveSeguranca.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                string chavecorreta = string.Join("",pedadosChave).Replace(" ","");
                if (comprovante != null && !String.IsNullOrEmpty(chavecorreta))
                {
                
                        string chaveSeguranca = BLL.ClSeguranca.Descriptografar(chavecorreta);
                        comprovante = JsonConvert.DeserializeObject<Comprovante>(chaveSeguranca);
                        if (Convert.ToInt64(comprovante.NS) > 0)
                        {
                            TempData["compt"] = new List<Comprovante>() { comprovante };
                            return RedirectToAction("Index", "Comprovante", null);
                        }
                }
                else
                {
                    ModelState.AddModelError("ChaveSeguranca", "Informe a chave de segurança");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("ChaveSeguranca", "Chave inválida");
            }

            return View(comprovante);
        }
    }
}