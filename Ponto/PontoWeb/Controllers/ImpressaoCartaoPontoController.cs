using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using PontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ImpressaoCartaoPontoController : Controller
    {
        [Authorize]
        [PermissoesFiltro(Roles = "ImpressaoCartaoPonto")]
        public ActionResult FCartaoPontoIndividual(int? id, string empresa, string departamento, string funcionario, string dataInicial, string dataFinal)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
            ImpressaoCartaoPontoIndividual imp = new ImpressaoCartaoPontoIndividual();
            DateTime dataIni;
            DateTime dataFin;
            bool mudadataautomaticamente;
            BLL.ConfiguracoesGerais bllConfiguracoes = new BLL.ConfiguracoesGerais(conn, usr);
            bllConfiguracoes.AtribuiDatas(String.Empty, out dataIni, out dataFin, out mudadataautomaticamente);
            if (String.IsNullOrEmpty(dataInicial) && String.IsNullOrEmpty(dataFinal))
            {
                imp.DataInicial = dataIni == new DateTime() ? DateTime.Now : dataIni;
                imp.DataFinal = dataFin == new DateTime() ? DateTime.Now : dataFin;
            }
            else
            {
                if (DateTime.TryParse(dataInicial, out dataIni) && DateTime.TryParse(dataFinal, out dataFin))
                {
                    imp.DataInicial = dataIni;
                    imp.DataFinal = dataFin;
                }
                else
                {
                    imp.DataInicial = DateTime.Now.Date;
                    imp.DataFinal = DateTime.Now.Date.AddDays(-30);
                }
            }

            Modelo.Funcionario objFuncionario;
            if (id.HasValue)
            {
                objFuncionario = bllFuncionario.LoadObject(id.Value);
                imp.IdFuncionario = objFuncionario.Id;
            }
            else if (!String.IsNullOrEmpty(empresa) && !String.IsNullOrEmpty(departamento) && !String.IsNullOrEmpty(funcionario))
            {
                string erro = String.Empty;
                objFuncionario = bllFuncionario.ValidaEmpDepFunc(empresa, departamento, funcionario, ref erro);
                imp.IdFuncionario = objFuncionario.Id;
                if (!String.IsNullOrEmpty(erro))
                {
                    return Json(new { erro = true, mensagemErro = erro }, JsonRequestBehavior.AllowGet);
                }
            }

            return PartialView(imp);
        }

        [Authorize]
        [PermissoesFiltro(Roles = "ImpressaoCartaoPontoConsultar")]
        [HttpPost]
        public ActionResult FCartaoPontoIndividual(ImpressaoCartaoPontoIndividual imp)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Funcionario func = bllFuncionario.LoadObject(imp.IdFuncionario);
                    if (String.IsNullOrEmpty(imp.TipoArquivo))
                    {
                        imp.TipoArquivo = "PDF";
                    }
                    RelatoriosController rc = new RelatoriosController();
                    ActionResult resultado = rc.ImprimirCartaoPonto(imp.TipoArquivo, imp.DataInicial.GetValueOrDefault(), imp.DataFinal.GetValueOrDefault(), func, func.Idhorario, false, conn, usr);
                    if (resultado != null)
                    {
                        return resultado;
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    throw ex;
                }
            }
            return View();
        }
    }
}