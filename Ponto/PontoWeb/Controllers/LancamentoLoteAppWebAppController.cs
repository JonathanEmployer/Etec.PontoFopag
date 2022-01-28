using Modelo;
using Modelo.Proxy;
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
    public class LancamentoLoteAppWebAppController : IControllerPontoWeb<LancamentoLote>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebApp")]
        // GET: LancamentoWebApp
        public ActionResult Index()
        {
            return Grid();
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebApp")]
        public override ActionResult Grid()
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
            List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.MudancaHorario };
            ViewBag.Title = "Lançamento de Permissão de Registro WebApp/App em Lote";
            ViewBag.Controller = "LancamentoLoteAppWebApp";
            return View("../LancamentoLote/Grid");
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
                List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.AppWebApp };
                List<Modelo.LancamentoLote> dados = bllLancamentoLote.GetAllListTipoLancamento(lstLancamento);
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebAppConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebAppCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebAppAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebAppCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebAppAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAppWebAppExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            LancamentoLoteController llC = new LancamentoLoteController();
            return llC.Excluir(id);
        }


        protected override ActionResult Salvar(LancamentoLote obj)
        {
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    string erro = String.Empty;
                    LancamentoLoteController llc = new LancamentoLoteController();
                    bool salvou = llc.SalvarLote(obj, out erro);
                    if (!salvou)
                    {
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            LancamentoLoteController.SetaDadosPadrao(obj, _usr.ConnectionString, _usr);
            ViewBag.Title = "Lançamento de Permissão de Registro WebApp/App em Lote";
            ViewBag.Controller = "LancamentoLoteAppWebApp";

            return View("../LancamentoLote/Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            ViewBag.Title = "Lançamento de Permissão de Registro WebApp/App em Lote";
            ViewBag.Controller = "LancamentoLoteAppWebApp";
            LancamentoLoteController llC = new LancamentoLoteController();
            LancamentoLote ll = llC.GetPagina(id, TipoLancamento.AppWebApp);
            return View("../LancamentoLote/Cadastrar", ll);
        }

        #region Validacoes
        protected override void ValidarForm(LancamentoLote obj) { 
            
        
        }
        #endregion
    }
}
