using BLL.Util;
using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioHorarioMovelController : Controller
    {
        private UsuarioPontoWeb _pw = Usuario.GetUsuarioPontoWebLogadoCache();
        // GET: RelatorioAfastamento
        private ActionResult RelatorioHorarioMovelHtml(int id)
        {
            BLL.Horario bllHorario = new BLL.Horario(_pw.ConnectionString, _pw);
            BLL.HorarioDetalhe bllHorarioDetalhe = new BLL.HorarioDetalhe(_pw.ConnectionString, _pw);
            string htmlRecebeDados = Convert.ToString(bllHorario.GetHorarioMovel());

            IList<Modelo.Proxy.PxyHorarioMovel> listaPxyHorarioMovel = bllHorarioDetalhe.GetPxyGradeHorario(id);

            return View(listaPxyHorarioMovel);
        }
        #region Método Get
        // GET: RelatorioHorarioMovel
        [PermissoesFiltro(Roles = "RelatorioHorarioMovelConsultar")]
        public ActionResult Index(int id)
        {
            BLL.Horario bllHorario = new BLL.Horario(_pw.ConnectionString, _pw);
            BLL.HorarioDetalhe bllHorariODetalhe = new BLL.HorarioDetalhe(_pw.ConnectionString, _pw);
            string htmlRecebeDados = Convert.ToString(bllHorario.GetHorarioMovel());
            IList<PxyHorarioMovel> listaPxyHorarioMovel = bllHorariODetalhe.GetPxyGradeHorario(id);
            listaPxyHorarioMovel.FirstOrDefault().Id = id;
            listaPxyHorarioMovel.FirstOrDefault().DataInicial = listaPxyHorarioMovel.FirstOrDefault().Data;
            listaPxyHorarioMovel.FirstOrDefault().DataFinal = listaPxyHorarioMovel.LastOrDefault().Data;
            return PartialView(listaPxyHorarioMovel);
        }
        #endregion

        [HttpPost]
        [PermissoesFiltro(Roles = "RelatorioHorarioMovelConsultar")]
        public ActionResult Index(PxyHorarioMovel horDet)
        {
            BLL.HorarioDetalhe bllHorarioDetalhe = new BLL.HorarioDetalhe(_pw.ConnectionString, _pw);
            IList<PxyHorarioMovel> listaGradeHorario = bllHorarioDetalhe.GetPxyGradeHorario(horDet.Id);
            DateTime dataFinalB = listaGradeHorario.LastOrDefault().Data;
            string dataFinalMax = dataFinalB.ToString("dd/MM/yyyy");
            if (ModelState.IsValid)
            {
                if (horDet.DataInicial > dataFinalB)
                {
                    string erros = "Data de Início maior do que o período cadastrado para esse horário. Data Fim máxima permitida: " + dataFinalMax;
                    return Json(new { Sucess = false, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    RelatorioPadraoModel parms = new RelatorioPadraoModel() { TipoArquivo = "PDF", IdSelecionados = horDet.Id.ToString(), InicioPeriodo = horDet.DataInicial, FimPeriodo = dataFinalB };
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioGradeHorarioMovel(parms);
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
            }
            return ModelState.JsonErrorResult();
        }

        public ActionResult Impressao()
        {
            BLL.Horario bllHorario = new BLL.Horario(_pw.ConnectionString, _pw);
            BLL.HorarioDetalhe bllHorariODetalhe = new BLL.HorarioDetalhe(_pw.ConnectionString, _pw);
            string htmlRecebeDados = Convert.ToString(bllHorario.GetHorarioMovel());
            //IList<Modelo.HorarioDetalhe> listaHorarioDetalhe = bllHorariODetalhe.GetGradeHorarioMovel(id);
            IList<Modelo.Proxy.PxyHorarioMovel> listaPxyHorarioMovel = bllHorariODetalhe.GetPxyGradeHorario(89);


            string razorText = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Views/RelatorioHorarioMovel/RelatorioHorarioMovelHtml.cshtml"));
            //string htmlText = Engine.Razor.RunCompile(razorText, Guid.NewGuid().ToString(), null, listaPxyHorarioMovel, null);
            string htmlText = Razor.Parse(razorText, listaPxyHorarioMovel);


            HtmlReport htmlReport = new HtmlReport();
            byte[] buffer = htmlReport.RenderPDF(htmlText, true, true);
            return File(buffer, "application/PDF");
        }
    }
}