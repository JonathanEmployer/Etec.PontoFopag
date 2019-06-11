using BLL.Util;
using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ExportarAFDController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "ExportarAFDCadastrar")]
        public ActionResult Exportar(int id)
        {
            ExportarAFD afd = new ExportarAFD();
            
            BLL.REP BLLRep = new BLL.REP(_usr.CentroServico, _usr);
            Modelo.ExportarAFD exportarAFD = new Modelo.ExportarAFD();

            var buscaDadosRelogio = BLLRep.LoadObject(id);
            exportarAFD.NumSerie = buscaDadosRelogio.NumSerie;
            exportarAFD.NomeRelogio = buscaDadosRelogio.modeloNome;
            exportarAFD.CodLocal = buscaDadosRelogio.CodigoLocal;
            exportarAFD.NumRelogio = buscaDadosRelogio.NumRelogio;
            exportarAFD.IdRep = buscaDadosRelogio.Id;

            

            ViewBag.tipoArquivos = Conversores.ToSelectList(Modelo.Listas.TiposArquivosExportacao(), "indice", "nome");

            return View(exportarAFD);
        }

        [PermissoesFiltro(Roles = "ExportarAFDCadastrar")]
        [HttpPost]
        public ActionResult Exportar(ExportarAFD afd)
        {
            Empresa objEmpresa = new Empresa();
            BLL.REP BLLRep = new BLL.REP(_usr.ConnectionString, _usr);
            var buscaDadosRelogio = BLLRep.LoadObject(afd.IdRep);

            afd.NumSerie = buscaDadosRelogio.NumSerie;
            afd.NomeRelogio= buscaDadosRelogio.modeloNome;
            afd.CodLocal = buscaDadosRelogio.CodigoLocal;
            afd.NumRelogio = buscaDadosRelogio.NumRelogio;

            if (ModelState.IsValid)
            {
                try
                {
                    HangfireManagerExportacoes hfm = new HangfireManagerExportacoes(_usr.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.ExportarAFD(1, 1, (DateTime)afd.DataInicial, (DateTime)afd.DataFinal);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
            ViewBag.tipoArquivos = Conversores.ToSelectList(Modelo.Listas.TiposArquivosExportacao(), "indice", "nome");
            return View(afd);
        }

        
    }
}