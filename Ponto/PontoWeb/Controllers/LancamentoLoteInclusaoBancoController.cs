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
    public class LancamentoLoteInclusaoBancoController : IControllerPontoWeb<LancamentoLote>
    {
        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBanco")]
        // GET: LancamentoLoteMudancaHorario
        public ActionResult Index()
        {
            return Grid();
        }

        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBanco")]
        public override ActionResult Grid()
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.InclusaoBanco };
            ViewBag.Title = "Lançamento de Crédito ou Débito de Banco de Horas em lote";
            ViewBag.Controller = "LancamentoLoteInclusaoBanco";
            return View("../LancamentoLote/Grid");
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.InclusaoBanco };
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

        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBancoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBancoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBancoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBancoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBancoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteInclusaoBancoExcluir")]
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
            ViewBag.Title = "Lançamento de Crédito ou Débito de Banco de Horas em Lote";
            ViewBag.Controller = "LancamentoLoteInclusaoBanco";

            return View("../LancamentoLote/Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            ViewBag.Title = "Lançamento de Crédito ou Débito de Banco de Horas em Lote";
            ViewBag.Controller = "LancamentoLoteInclusaoBanco";
            LancamentoLoteController llC = new LancamentoLoteController();
            LancamentoLote ll = llC.GetPagina(id, TipoLancamento.InclusaoBanco);
            return View("../LancamentoLote/Cadastrar", ll);
        }

        #region Validacoes
        protected override void ValidarForm(LancamentoLote obj)
        {
            if (obj.LancamentoLoteInclusaoBanco.Tipocreditodebito == 0)
            {
                obj.LancamentoLoteInclusaoBanco.Debito = "---:--";
            }
        }
        #endregion
    }
}