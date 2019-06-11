using Modelo;
using Modelo.Proxy;
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
    public class GrupoAcessoController : IControllerPontoWeb<pxyCwGrupoAcesso>
    {
        [PermissoesFiltro(Roles = "GrupoAcesso")]
        public override ActionResult Grid()
        {
            BLL.Cw_Grupo bllCw_Grupo = new BLL.Cw_Grupo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            return View(new Modelo.Proxy.PxyGridGrupodeUsuario());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.Cw_GrupoAcesso bllGrupo = new BLL.Cw_GrupoAcesso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                List<Modelo.Proxy.PxyGridGrupodeUsuario> funcs = bllGrupo.GetAllGrid();
                JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "GrupoAcessoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "GrupoAcessoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "GrupoAcessoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "GrupoAcessoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(pxyCwGrupoAcesso obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "GrupoAcessoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(pxyCwGrupoAcesso obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "GrupoAcessoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            return RedirectToAction("Grid", "GrupoAcesso");
        }

        protected override ActionResult Salvar(pxyCwGrupoAcesso obj)
        {
            BLL.Cw_GrupoAcesso bllGrupoAcesso = new BLL.Cw_GrupoAcesso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in obj.Permissoes)
                    {
                        if (item.Id != 0)
                        {
                            bllGrupoAcesso.Salvar(Acao.Alterar, item);
                        }
                    }
                    cw_usuario user = BLLWeb.Usuario.GetUsuarioLogadoCache();
                    BLLWeb.Usuario.LimpaAcessoCache();
                    BLLWeb.Usuario.AdicionaAcessoCache(user);
                    
                }
                catch (Exception e)
                {
                    BLL.cwkFuncoes.LogarErro(e);
                    ModelState.AddModelError("CustomError", "Houve um erro ao processar a operação: " + e.Message);
                    View("Cadastrar", obj);
                }
                return RedirectToAction("Grid", "GrupoAcesso");
            }
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.Cw_GrupoAcesso bllGrupoAcesso = new BLL.Cw_GrupoAcesso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            return View("Cadastrar", bllGrupoAcesso.GetPxyListPorGrupo(id));
        }

        protected override void ValidarForm(pxyCwGrupoAcesso obj)
        {
            
        }
    }
}