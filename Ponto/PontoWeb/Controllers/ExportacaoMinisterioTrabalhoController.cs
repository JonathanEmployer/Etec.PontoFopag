using BLL.Util;
using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ExportacaoMinisterioTrabalhoController : Controller
    {
        UsuarioPontoWeb _user = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "ExportacaoMinisterioTrabalhoCadastrar")]
        public ActionResult Exportar()
        {
            ExportacaoMinisterioTrabalho exp = new ExportacaoMinisterioTrabalho();
            ViewBag.tipoArquivos = Conversores.ToSelectList(Modelo.Listas.TiposArquivosExportacao(), "indice", "nome");

            return View(exp);
        }

        [PermissoesFiltro(Roles = "ExportacaoMinisterioTrabalhoCadastrar")]
        [HttpPost]
        public ActionResult Exportar(ExportacaoMinisterioTrabalho exp)
        {
            Empresa objEmpresa = new Empresa();
            ValidaEmpresa(exp);
            if (ModelState.IsValid)
            {
                try
                {
                    HangfireManagerExportacoes hfm = new HangfireManagerExportacoes(_user.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.ExportaArquivosAFDTACJEF(exp.TipoArquivo, exp.ObjEmpresa, (DateTime)exp.DataInicial, (DateTime)exp.DataFinal);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
            ViewBag.tipoArquivos = Conversores.ToSelectList(Modelo.Listas.TiposArquivosExportacao(), "indice", "nome");
            return View();
        }

        private static int CarregarObjEmpresa(ExportacaoMinisterioTrabalho exp, BLL.Empresa bllEmpresa)
        {
            int codEmpresa;
            string codEmpresaStr = exp.NomeEmpresa.Split('|').FirstOrDefault().Trim();
            Int32.TryParse(codEmpresaStr, out codEmpresa);
            return bllEmpresa.LoadObjectByCodigo(codEmpresa).Id;
        }

        private void ValidaEmpresa(ExportacaoMinisterioTrabalho exp)
        {
            BLL.Empresa bllEmpresa = new BLL.Empresa(_user.ConnectionString, _user);
            try
            {
                if (exp.DataInicial < new DateTime(1760, 1, 1))
                {
                    ModelState["DataInicial"].Errors.Add("A Data Inicial deve ser maior que 01/01/1760");
                }
                int idEmpresa = CarregarObjEmpresa(exp, bllEmpresa);
                if (idEmpresa > 0)
                {
                    exp.ObjEmpresa = bllEmpresa.LoadObject(idEmpresa);
                }
                else
                {
                    ModelState["NomeEmpresa"].Errors.Add("Empresa " + exp.NomeEmpresa + " não cadastrado!");
                }
            }
            catch (Exception)
            {
                ModelState["NomeEmpresa"].Errors.Add("Empresa " + exp.NomeEmpresa + " não cadastrado!");
            }
        }
    }
}