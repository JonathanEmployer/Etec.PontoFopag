using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LocalizacaoRegistroPontoController : IControllerPontoWeb<LocalizacaoRegistroPonto>
    {
        [PermissoesFiltro(Roles = "LocalizacaoRegistroPonto")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.LocalizacaoRegistroPonto bllLocalizacaoRegistroPonto = new BLL.LocalizacaoRegistroPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(bllLocalizacaoRegistroPonto.GetAllList());
        }

        [PermissoesFiltro(Roles = "LocalizacaoRegistroPontoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LocalizacaoRegistroPontoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LocalizacaoRegistroPontoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LocalizacaoRegistroPontoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LocalizacaoRegistroPonto obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LocalizacaoRegistroPontoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LocalizacaoRegistroPonto obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LocalizacaoRegistroPontoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.LocalizacaoRegistroPonto bllLocalizacaoRegistroPonto = new BLL.LocalizacaoRegistroPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            LocalizacaoRegistroPonto LocalizacaoRegistroPonto = bllLocalizacaoRegistroPonto.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllLocalizacaoRegistroPonto.Salvar(Acao.Excluir, LocalizacaoRegistroPonto);
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
            if (ex.Message.Contains("FK_Funcionario_LocalizacaoRegistroPonto"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(LocalizacaoRegistroPonto obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.LocalizacaoRegistroPonto bllLocalizacaoRegistroPonto = new BLL.LocalizacaoRegistroPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllLocalizacaoRegistroPonto.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "LocalizacaoRegistroPonto");
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
            BLL.LocalizacaoRegistroPonto bllLocalizacaoRegistroPonto = new BLL.LocalizacaoRegistroPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            LocalizacaoRegistroPonto LocalizacaoRegistroPonto = new LocalizacaoRegistroPonto();
            LocalizacaoRegistroPonto = bllLocalizacaoRegistroPonto.LoadObject(id);
            if (id == 0)
            {
                LocalizacaoRegistroPonto.Codigo = bllLocalizacaoRegistroPonto.MaxCodigo();
            }
            return View("Cadastrar", LocalizacaoRegistroPonto);
        }

        protected override void ValidarForm(LocalizacaoRegistroPonto obj)
        {
            throw new NotImplementedException();
        }
    }
}
