using BLL_N.IntegracaoTerceiro;
using Modelo.Proxy;
using Modelo.Proxy.IntegracaoTerceiro.DB1;
using Modelo.Proxy.Relatorios;
using Newtonsoft.Json;
using PontoWeb.Controllers.BLLWeb;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class RelatoriosCustomizadosController : Controller
    {
        public ActionResult RelDB1HorasTrabalhadasTaskHTML()
        {
            Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(userPW);

            RecuperarTriagens rec = new RecuperarTriagens();
            rec.DataInicio = "2020-01-01";
            rec.DataFinal = "2020-01-30";
            rec.TipoRelatorio = 1;
            rec.Cpfs = new List<string>() { "37018907810", "08614615922", "22055689819", "03192770970", "05826146940", "34647378888", "30892611898", "29938766862", "08945566902", "61241717915", "06600621940", "05874034978", "01341501124", "35758312894", "04452491960", "39536539896", "04552585118", "08493840955", "07508551931", "05333282927", "33214080822", "08142110970", "40049943812", "06605662903", "00941267989", "06450831197" };

            List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> triagens = bllDB1.GetTriagens(rec, @"http://pontofopag.db1group.com/");

            triagens = triagens.OrderBy(o => o.EmpresaNome).ThenBy(t => t.FuncionarioNome).ToList();
            return View(triagens);
        }


        public ActionResult RelDB1HorasTrabalhadasTaskSinteticoHTML()
        {
            Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(userPW);

            RecuperarTriagens rec = new RecuperarTriagens();
            rec.DataInicio = "2016-08-01";
            rec.DataFinal = "2016-08-31";
            rec.TipoRelatorio = 1;
            rec.Cpfs = new List<string>() { "38717936802", "02643770951", "08753155939", "36657898851", "32310789895", "15826427884", "09343280998", "36153120822", "05816274902", "07505774956" };

            List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> triagens = bllDB1.GetTriagens(rec, @"http://pontofopag.db1group.com/");

            triagens = triagens.OrderBy(o => o.EmpresaNome).ThenBy(t => t.FuncionarioNome).ToList();
            return View(triagens);
        }



        public ActionResult Index()
        {
            try
            {
                RecuperarTriagens rec = new RecuperarTriagens();
                rec.DataInicio = "2016-09-01";
                rec.DataFinal = "2016-09-30";
                rec.TipoRelatorio = 0;
                rec.Sintetico = false;
                rec.Cpfs = new List<string>() { "38717936802", "02643770951", "08753155939", "36657898851", "32310789895", "15826427884", "09343280998", "36153120822", "05816274902", "07505774956", "40843177802"};
                Modelo.UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL_N.IntegracaoTerceiro.DB1 bllDB1 = new BLL_N.IntegracaoTerceiro.DB1(userPW);
             
                
                if (ModelState.IsValid)
                {
                    PxyFileResult arquivo = bllDB1.GetRelatorio(rec, ConfigurationManager.AppSettings["ApiDB1"]);

                    if (arquivo.Erros != null && arquivo.Erros.Count > 0)
                    {
                        return Json(new { Success = false, Erro = string.Join(";", arquivo.Erros.Select(x => x.Key + " = " + x.Value).ToArray()) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return File(arquivo.Arquivo, arquivo.MimeType, arquivo.FileName);
                    }
                }
                else
                {
                    string erros = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    return Json(new { Success = false, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}