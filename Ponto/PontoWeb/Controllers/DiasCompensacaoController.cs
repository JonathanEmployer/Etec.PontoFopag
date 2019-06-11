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
    public class DiasCompensacaoController : IControllerPontoWeb<DiasCompensacao>
    {
        [PermissoesFiltro(Roles = "Compensacao")]
        public override ActionResult Grid()
        {
            BLL.DiasCompensacao bllDiasComp = new BLL.DiasCompensacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            return View(bllDiasComp.LoadPCompensacao(0));
        }

        public override ActionResult Consultar(int id)
        {
            return RedirectToAction("Index", "Home"); 
        }

        [PermissoesFiltro(Roles = "CompensacaoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return RedirectToAction("Index", "Home");
        }

        [PermissoesFiltro(Roles = "CompensacaoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return RedirectToAction("Index", "Home");
        }

        [PermissoesFiltro(Roles = "CompensacaoCadastrar")]
        public override ActionResult Cadastrar(DiasCompensacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "CompensacaoAlterar")]
        public override ActionResult Alterar(DiasCompensacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "CompensacaoExcluir")]
        public override ActionResult Excluir(int id)
        {
            BLL.DiasCompensacao bllDiasComp = new BLL.DiasCompensacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            DiasCompensacao DiaComp = bllDiasComp.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllDiasComp.Salvar(Acao.Excluir, DiaComp);
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

        protected override ActionResult Salvar(DiasCompensacao obj)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                try
                {
                    Compensacao comp = obj.Compensacao;
                    comp.DiasC.Add(obj);
                    return RedirectToAction("Cadastrar", "Compensacao", obj);

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

        protected override ActionResult GetPagina(int id)
        {
            throw new NotImplementedException();
        }

        protected override void ValidarForm(DiasCompensacao obj)
        {
            throw new NotImplementedException();
        }
    }
}