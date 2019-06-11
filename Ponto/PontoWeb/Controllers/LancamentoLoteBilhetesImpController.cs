using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LancamentoLoteBilhetesImpController : IControllerPontoWeb<LancamentoLote>
    {
        string titulo = "Lançamento de Marcação Manual em Lote";
        string controller = "LancamentoLoteBilhetesImp";
        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImp")]
        public ActionResult Index()
        {
            return Grid();
        }

        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImp")]
        public override ActionResult Grid()
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.BilhetesImp };
            ViewBag.Title = titulo;
            ViewBag.Controller = controller;
            return View("../LancamentoLote/Grid");
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.BilhetesImp };
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

        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImpConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImpCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImpAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImpCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImpAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteBilhetesImpExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            LancamentoLoteController llC = new LancamentoLoteController();
            return llC.Excluir(id);
        }

        protected override ActionResult Salvar(LancamentoLote obj)
        {
            string conexao = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb userpw = Usuario.GetUsuarioPontoWebLogadoCache();
            obj.LancamentoLoteBilhetesImp.Ocorrencia = "I";
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

            LancamentoLoteController.SetaDadosPadrao(obj, conexao, userpw);
            ViewBag.Title = titulo;
            ViewBag.Controller = controller;

            return View("../LancamentoLote/Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            ViewBag.Title = titulo;
            ViewBag.Controller = controller;
            LancamentoLoteController llC = new LancamentoLoteController();
            LancamentoLote ll = llC.GetPagina(id, TipoLancamento.BilhetesImp);
            return View("../LancamentoLote/Cadastrar", ll);
        }

        #region Validacoes
        protected override void ValidarForm(LancamentoLote obj)
        {
            obj.LancamentoLoteBilhetesImp.Idjustificativa = JustificativaController.BuscaIdJustificativa(obj.LancamentoLoteBilhetesImp.DescJustificativa);
            if (obj.LancamentoLoteBilhetesImp.Idjustificativa == 0)
            {
                ModelState["LancamentoLoteBilhetesImp.DescJustificativa"].Errors.Add("Justificativa " + obj.LancamentoLoteBilhetesImp.DescJustificativa + " não cadastrada!");
            }
        }
        #endregion
    }
}