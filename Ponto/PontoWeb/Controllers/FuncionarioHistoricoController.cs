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
    public class FuncionarioHistoricoController : Controller
    {
        [PermissoesFiltro(Roles = "FuncionarioHistorico")]
        public ActionResult Grid(int id)
        {
            Modelo.FuncionarioHistorico funcHist = new FuncionarioHistorico() { Idfuncionario = id };
            return View(funcHist);
        }

        [Authorize]
        public JsonResult Dados(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.FuncionarioHistorico bllFuncHist = new BLL.FuncionarioHistorico(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.FuncionarioHistorico> dados = bllFuncHist.LoadPorFuncionario(id);
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

        [PermissoesFiltro(Roles = "FuncionarioHistoricoConsultar")]
        public ActionResult Consultar(int id = 0, int idFunc = 0)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id, idFunc);
        }

        [PermissoesFiltro(Roles = "FuncionarioHistoricoCadastrar")]
        public ActionResult Cadastrar(int id = 0, int idFunc = 0)
        {
            return GetPagina(id, idFunc);
        }

        [PermissoesFiltro(Roles = "FuncionarioHistoricoAlterar")]
        public ActionResult Alterar(int id = 0, int idFunc = 0)
        {
            return GetPagina(id, idFunc);
        }

        [PermissoesFiltro(Roles = "FuncionarioHistoricoCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(FuncionarioHistorico obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FuncionarioHistoricoAlterar")]
        [HttpPost]
        public ActionResult Alterar(FuncionarioHistorico obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FuncionarioHistoricoExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioHistorico bllFuncHist = new BLL.FuncionarioHistorico(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            FuncionarioHistorico funcHist = bllFuncHist.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllFuncHist.Salvar(Acao.Excluir, funcHist);
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

        private ActionResult Salvar(FuncionarioHistorico obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioHistorico bllFuncHist = new BLL.FuncionarioHistorico(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllFuncHist.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                        msg = erro;
                    }
                    else
                    {
                        return RedirectToAction("Grid", new { id = obj.Idfuncionario });
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

        private ActionResult GetPagina(int id = 0, int idFunc = 0)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FuncionarioHistorico bllFuncHist = new BLL.FuncionarioHistorico(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            FuncionarioHistorico funchist = new FuncionarioHistorico();
            funchist = bllFuncHist.LoadObject(id);
            if (id == 0)
            {
                funchist.Codigo = bllFuncHist.MaxCodigo(bllFuncHist.LoadPorFuncionario(idFunc));
                funchist.Idfuncionario = idFunc;
            }
            return View("Cadastrar",funchist);
        }
    }
}