using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class FechamentoBHDController : Controller
    {
        [PermissoesFiltro(Roles = "FechamentoBHD")]
        public ActionResult Grid(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.FechamentoBHD() { Idfechamentobh = id});
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Proxy.PxyFechamentoBHD> dados = bllFechamentoBHD.GetAllGrid(id);
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

        [PermissoesFiltro(Roles = "FechamentoBHDConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoBHDCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "FechamentoBHDAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoBHDCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(FechamentoBHD obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoBHDAlterar")]
        [HttpPost]
        public ActionResult Alterar(FechamentoBHD obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoBHDExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            FechamentoBHD FechamentoBHD = bllFechamentoBHD.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllFechamentoBHD.Salvar(Acao.Excluir, FechamentoBHD);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult Salvar(FechamentoBHD obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        throw new Exception("Não é permitida inclusão de Fechamento por Funcionário, apenas edição!");
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllFechamentoBHD.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "FechamentoBHD", new { id = obj.Idfechamentobh });
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Cadastrar", obj);
        }

        private ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(conn, usr);
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(conn, usr);
            BLL.FechamentobhdHE bllFechamentobhdHE = new BLL.FechamentobhdHE(conn, usr);

            FechamentoBHD fechamentoBHD = new FechamentoBHD();
            fechamentoBHD = bllFechamentoBHD.LoadObject(id);
            
            FechamentoBH fechamentoBH = bllFechamentoBH.LoadObject(fechamentoBHD.Idfechamentobh);
            fechamentoBHD.fechamentoBH = fechamentoBH;

            IList<Modelo.FechamentobhdHE> fechamentosbhdHE = bllFechamentobhdHE.GetFechamentobhdHEPorIdFechamentoBH(fechamentoBHD.Idfechamentobh, fechamentoBHD.Identificacao);
            fechamentoBHD.fechamentosbhdHE = fechamentosbhdHE;

            if (id == 0)
            {
                fechamentoBHD.Codigo = bllFechamentoBHD.MaxCodigo();
            }
            else
            {
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(conn, usr);
                    string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(2, new List<int>() { fechamentoBHD.Identificacao }, fechamentoBH.Data.GetValueOrDefault());
                    if (!String.IsNullOrEmpty(mensagemFechamento))
                    {
                        ViewBag.Consultar = 1;
                        @ViewBag.MensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento de ponto vinculado. Detalhes: <br/>" + mensagemFechamento;
                    }
                }
                #endregion 
            }
            return View("Cadastrar", fechamentoBHD);
        }
    }
}