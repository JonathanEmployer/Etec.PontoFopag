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
    public class LancamentoLoteAfastamentoController : IControllerPontoWeb<LancamentoLote>
    {
        string titulo = "Lançamento de Afastamento em Lote";
        string controller = "LancamentoLoteAfastamento";
        [PermissoesFiltro(Roles = "LancamentoLoteAfastamento")]
        public ActionResult Index()
        {
            return Grid();
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAfastamento")]
        public override ActionResult Grid()
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.Afastamento };
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
                List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.Afastamento };
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

        [PermissoesFiltro(Roles = "LancamentoLoteAfastamentoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAfastamentoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAfastamentoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAfastamentoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAfastamentoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteAfastamentoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            LancamentoLoteController llC = new LancamentoLoteController();
            return llC.Excluir(id);
        }

        protected override ActionResult Salvar(LancamentoLote obj)
        {
            obj.DataLancamento = obj.LancamentoLoteAfastamento.DataI.GetValueOrDefault();
            obj.DataLancamentoAnt = obj.LancamentoLoteAfastamento.DataI_Ant.GetValueOrDefault();
            ModelState["DataLancamento"].Errors.Clear();
            string conexao = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb userpw = Usuario.GetUsuarioPontoWebLogadoCache();
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    string erro = String.Empty;
                    LancamentoLoteController llc = new LancamentoLoteController();
                    if (obj.Id > 0)
                    {
                        obj.Acao = Acao.Alterar;
                    }
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
            LancamentoLote ll = llC.GetPagina(id, TipoLancamento.Afastamento);
            return View("../LancamentoLote/Cadastrar", ll);
        }

        #region Validacoes
        protected override void ValidarForm(LancamentoLote obj)
        {
            obj.LancamentoLoteAfastamento.IdOcorrencia = OcorrenciaController.BuscaIdOcorrencia(obj.LancamentoLoteAfastamento.Ocorrencia);
            if (obj.LancamentoLoteAfastamento.IdOcorrencia == 0)
            {
                ModelState["LancamentoLoteAfastamento.Ocorrencia"].Errors.Add("Ocorrência " + obj.LancamentoLoteAfastamento.Ocorrencia + " não cadastrada!");
            }

            if (obj.LancamentoLoteAfastamento.DataI > obj.LancamentoLoteAfastamento.DataF)
            {
                ModelState["LancamentoLoteAfastamento.DataF"].Errors.Add("Data deve ser maior que inicial!");
            }
        }
        #endregion
    }
}