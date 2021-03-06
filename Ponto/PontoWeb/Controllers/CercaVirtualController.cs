using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    [Authorize]
    public class CercaVirtualController : Controller //IControllerPontoWeb<CercaVirtual>
    {
        public ActionResult Grid()
        {
            return View(new Modelo.CercaVirtual());
        }
        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                var cercaVirtualBLL = new BLL.CercaVirtual(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                var Codigo = 0;
                if (HttpContext.Request.UrlReferrer.Segments.Length > 3)
                {
                    Codigo = int.Parse(HttpContext.Request.UrlReferrer.Segments[3]);
                }
                var dados = cercaVirtualBLL.GetAllList(Codigo);

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

        [PermissoesFiltro(Roles = "CercaVirtualCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }
        [Authorize]
        public JsonResult FuncsGrid()
        {
            return DadosGridFunc();
        }
        [Authorize]
        public JsonResult DadosGridFunc()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                var funcionarioBLL = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                var dados = funcionarioBLL.GetAllGrid();

                var localPath = HttpContext.Request.Url.LocalPath.Split('/');
                if (localPath.Length > 3)
                {
                    var funcionarios = localPath[3].Split(',').ToList().Select(x => int.Parse(x));
                    dados = dados.Where(x => funcionarios.Contains(x.Id)).ToList();
                }
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

        protected ActionResult GetPagina(int id)
        {
            var cercaVirtual = new CercaVirtual();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();

            cercaVirtual.Funcionario = new List<pxyCercaVirtualFuncionarioGrid>();

            if (id != 0)
            {
                Modelo.CercaVirtual cerca = new BLL.CercaVirtual(conn, usr).LoadObject(id);
                cercaVirtual = cerca;
            }
            else
            {
                cercaVirtual.Codigo = new BLL.CercaVirtual(conn, usr).MaxCodigo();
                cercaVirtual.Ativo = true;
            }
            cercaVirtual.Raio = cercaVirtual.Raio < 100 ? 100 : cercaVirtual.Raio;

            return View("Cadastrar", cercaVirtual);
        }


        [PermissoesFiltro(Roles = "CercaVirtualCadastrar")]
        public ActionResult Index()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "CercaVirtualCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(CercaVirtual CercaVirtual)
        {
            CercaVirtual.Acao = Acao.Incluir;
            return Salvar(CercaVirtual);
        }
        [PermissoesFiltro(Roles = "CercaVirtualAlterar")]
        [HttpPost]
        public ActionResult Alterar(CercaVirtual CercaVirtual)
        {
            CercaVirtual.Acao = Acao.Alterar;
            return Salvar(CercaVirtual);
        }
        protected ActionResult Salvar(CercaVirtual obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                    var usr = Usuario.GetUsuarioPontoWebLogadoCache();

                    BLL.CercaVirtual objCercaVirtual = new BLL.CercaVirtual(conn, usr);
                    obj.NaoValidaCodigo = false;
                    objCercaVirtual.Salvar(obj.Acao, obj);

                    return RedirectToAction("Grid", "CercaVirtual");
                }
                var err = ModelState.Values.Where(w => w.Errors.Count > 0);
                var error = string.Join("; ", err
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));
                ModelState.AddModelError("CustomError", error);
                return View("Cadastrar", obj);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                ModelState.AddModelError("CustomError", ex.Message);
                return View("Cadastrar", obj);
            }
        }
        [PermissoesFiltro(Roles = "CercaVirtualExcluir")]
        public ActionResult Excluir(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.CercaVirtual objCercaVirtual = new BLL.CercaVirtual(conn, usr);

            var CodigoFuncionario = 0;
            if (HttpContext.Request.UrlReferrer.Segments.Length > 3)
            {
                CodigoFuncionario = int.Parse(HttpContext.Request.UrlReferrer.Segments[3]);
            }

            objCercaVirtual.Excluir(id);

            var cercaVirtualBLL = new BLL.CercaVirtual(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

            var dados = cercaVirtualBLL.GetAllList(CodigoFuncionario);
            ViewBag.Btn = false;
            if (dados.Count() > 0)
                ViewBag.Btn = true;

            return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
        }



        [PermissoesFiltro(Roles = "CercaVirtualConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }
        [PermissoesFiltro(Roles = "CercaVirtualAlterar")]
        public  ActionResult Alterar(int id)
        {
            ViewBag.Consultar = 0;
            return GetPagina(id);
        }


        protected  void ValidarForm(CercaVirtual obj)
        {
        }
    }
}