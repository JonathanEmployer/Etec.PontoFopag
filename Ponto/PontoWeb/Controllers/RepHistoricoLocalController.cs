using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class RepHistoricoLocalController : Controller
    {
        [PermissoesFiltro(Roles = "RepHistoricoLocal")]
        public ActionResult Grid(int id)
        {
            Modelo.RepHistoricoLocal repHist = new RepHistoricoLocal() { IdRep = id };
            return View(repHist);
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.RepHistoricoLocal bllREPHisLocal = new BLL.RepHistoricoLocal(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.RepHistoricoLocal> dados = bllREPHisLocal.GetAllGrid(id);
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

        [PermissoesFiltro(Roles = "RepHistoricoLocalConsultar")]
        public ActionResult Consultar(int id = 0, int idRep = 0)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id, idRep);
        }

        [PermissoesFiltro(Roles = "RepHistoricoLocalCadastrar")]
        public ActionResult Cadastrar(int id = 0, int idRep = 0)
        {
            return GetPagina(id, idRep);
        }

        [PermissoesFiltro(Roles = "RepHistoricoLocalAlterar")]
        public ActionResult Alterar(int id = 0, int idRep = 0)
        {
            return GetPagina(id, idRep);
        }

        [PermissoesFiltro(Roles = "RepHistoricoLocalCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(RepHistoricoLocal obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "RepHistoricoLocalAlterar")]
        [HttpPost]
        public ActionResult Alterar(RepHistoricoLocal obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "RepHistoricoLocalExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.RepHistoricoLocal bllRepHistoricoLocal = new BLL.RepHistoricoLocal(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            RepHistoricoLocal repHistLocal = bllRepHistoricoLocal.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllRepHistoricoLocal.Salvar(Acao.Excluir, repHistLocal);
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

        private ActionResult Salvar(RepHistoricoLocal obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.RepHistoricoLocal bllRepHistoricoLocal = new BLL.RepHistoricoLocal(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            string msg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllRepHistoricoLocal.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                        msg = erro;
                    }
                    else
                    {
                        return RedirectToAction("Grid", new { id = obj.IdRep });
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                    msg = ex.Message;
                }
            }
            return PartialView();
        }

        private ActionResult GetPagina(int id = 0, int idRep = 0)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.RepHistoricoLocal bllRepHistoricoLocal = new BLL.RepHistoricoLocal(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            RepHistoricoLocal repHistoricoLocal = new RepHistoricoLocal();
            repHistoricoLocal = bllRepHistoricoLocal.LoadObject(id);
            if (id == 0)
            {
                repHistoricoLocal.Codigo = bllRepHistoricoLocal.MaxCodigo(bllRepHistoricoLocal.LoadPorRep(idRep));
                repHistoricoLocal.IdRep = idRep;
                repHistoricoLocal.Data = DateTime.Now;
            }
            return View("Cadastrar", repHistoricoLocal);
        }
    }
}