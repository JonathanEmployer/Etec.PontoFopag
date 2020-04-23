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
    public class JornadaSubstituirController : IControllerPontoWeb<JornadaSubstituir>
    {
        private UsuarioPontoWeb usr = Usuario.GetUsuarioPontoWebLogadoCache();
        // GET: JornadaSubstituir
        public override ActionResult Grid()
        {
            return View(new JornadaSubstituir());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.JornadaSubstituir bllJornadaSubstituir = new BLL.JornadaSubstituir(usr.ConnectionString, usr);
                List<JornadaSubstituir> dados = bllJornadaSubstituir.GetAllList();
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

        [PermissoesFiltro(Roles = "JornadaSubstituirConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JornadaSubstituirCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "JornadaSubstituirAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JornadaSubstituirCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(JornadaSubstituir obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JornadaSubstituirAlterar")]
        [HttpPost]
        public override ActionResult Alterar(JornadaSubstituir obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JornadaSubstituirExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.JornadaSubstituir bllJornadaSubstituir = new BLL.JornadaSubstituir(usr.ConnectionString, usr);
            JornadaSubstituir jornadaSubstituir = bllJornadaSubstituir.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllJornadaSubstituir.Salvar(Acao.Excluir, jornadaSubstituir);
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
            return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(JornadaSubstituir obj)
        {
            BLL.JornadaSubstituir bllJornadaSubstituir = new BLL.JornadaSubstituir(usr.ConnectionString, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = obj.Id == 0 ? acao = Acao.Incluir : acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllJornadaSubstituir.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Funcao");
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
            BLL.JornadaSubstituir bllJornadaSubstituir = new BLL.JornadaSubstituir(usr.ConnectionString, usr);
            JornadaSubstituir jornadaSubstituir = bllJornadaSubstituir.LoadObject(id);
            if (id == 0)
            {
                jornadaSubstituir.Codigo = bllJornadaSubstituir.MaxCodigo();
            }
            return View("Cadastrar", jornadaSubstituir);
        }

        protected override void ValidarForm(JornadaSubstituir obj)
        {
            throw new NotImplementedException();
        }
    }
}