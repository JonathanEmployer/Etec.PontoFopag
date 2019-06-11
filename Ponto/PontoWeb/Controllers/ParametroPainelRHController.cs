using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ParametroPainelRHController : IControllerPontoWeb<ParametroPainelRH>
    {
        [PermissoesFiltro(Roles = "ParametroPainelRH")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ParametroPainelRH bllParametroPainelRH = new BLL.ParametroPainelRH(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(bllParametroPainelRH.GetAllList());
        }

        [PermissoesFiltro(Roles = "ParametroPainelRHConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ParametroPainelRHCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "ParametroPainelRHAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ParametroPainelRHCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(ParametroPainelRH obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ParametroPainelRHAlterar")]
        [HttpPost]
        public override ActionResult Alterar(ParametroPainelRH obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ParametroPainelRHExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ParametroPainelRH bllParametroPainelRH = new BLL.ParametroPainelRH(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            ParametroPainelRH ParametroPainelRH = bllParametroPainelRH.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllParametroPainelRH.Salvar(Acao.Excluir, ParametroPainelRH);
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
                return TratarErros(ex);
            }
        }

        private ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_Funcionario_ParametroPainelRH"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(ParametroPainelRH obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ParametroPainelRH bllParametroPainelRH = new BLL.ParametroPainelRH(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllParametroPainelRH.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "ParametroPainelRH");
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

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ParametroPainelRH bllParametroPainelRH = new BLL.ParametroPainelRH(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            ParametroPainelRH ParametroPainelRH = new ParametroPainelRH();
            ParametroPainelRH = bllParametroPainelRH.LoadObject(id);
            if (id == 0)
            {
                ParametroPainelRH.Codigo = bllParametroPainelRH.MaxCodigo();
            }
            return View("Cadastrar", ParametroPainelRH);
        }

        protected override void ValidarForm(ParametroPainelRH obj)
        {
            throw new NotImplementedException();
        }
    }
}
