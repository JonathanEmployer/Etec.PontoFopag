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
    public class CercaVirtualController : IControllerPontoWeb<CercaVirtual>
    {
        [PermissoesFiltro(Roles = "CercaVirtual")]
        public override ActionResult Grid()
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

        [PermissoesFiltro(Roles = "CercaVirtual")]
        public override ActionResult Cadastrar()
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

        protected override ActionResult GetPagina(int id)
        {
            var cercaVirtual = new CercaVirtual();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();

            cercaVirtual.Funcionario = new List<pxyFuncionarioGrid>();

            if (id != 0)
            {
                Modelo.CercaVirtual cerca = new BLL.CercaVirtual(conn, usr).LoadObject(id);
                cercaVirtual = cerca;
            }

            return View("Cadastrar", cercaVirtual);
        }


        [PermissoesFiltro(Roles = "CercaVirtual")]
        public ActionResult Index()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "CercaVirtual")]
        [HttpPost]
        public override ActionResult Cadastrar(CercaVirtual CercaVirtual)
        {
            CercaVirtual.Acao = Acao.Incluir;
            return Salvar(CercaVirtual);
        }
        [PermissoesFiltro(Roles = "CercaVirtual")]
        [HttpPost]
        public override ActionResult Alterar(CercaVirtual CercaVirtual)
        {
            CercaVirtual.Acao = Acao.Alterar;
            return Salvar(CercaVirtual);
        }
        protected override ActionResult Salvar(CercaVirtual obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                    var usr = Usuario.GetUsuarioPontoWebLogadoCache();


                    BLL.CercaVirtual objCercaVirtual = new BLL.CercaVirtual(conn, usr);

                    if (obj.Acao == Acao.Incluir)
                    {

                    }
                    obj.NaoValidaCodigo = false;
                    objCercaVirtual.Salvar(obj.Acao, obj);


                    //ValidaFuncionariosEmpresaSelecionados(obj, conn, usr);

                    //Dictionary<string, string> erros = new Dictionary<string, string>();

                    //ValidarForm(ref obj);

                    //PreencheObjFilhosParaSalvar(ref obj, conn, usr);
                    //var envioDadosRep = new EnvioDadosRep()
                    //{
                    //    Codigo = obj.Codigo == 0 ? 1 : obj.Codigo,
                    //    relogioSelecionado = obj.relogioSelecionado,
                    //    listEnvioDadosRepDet = obj.ListEnvioDadosRepDet,
                    //    bEnviarEmpresa = false,
                    //    TipoComunicacao = obj.Enviar ? "E" : "R",
                    //    idRelogioSelecionado = obj.relogioSelecionado.Id
                    //};
                    //bllEnvioEmpresaFuncionariosRep.Salvar(envioDadosRep);
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
        [PermissoesFiltro(Roles = "CercaVirtual")]
        public override ActionResult Excluir(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.CercaVirtual objCercaVirtual = new BLL.CercaVirtual(conn, usr);

            var CodigoFuncionario = 0;
            if (HttpContext.Request.UrlReferrer.Segments.Length > 3)
            {
                CodigoFuncionario = int.Parse(HttpContext.Request.UrlReferrer.Segments[3]);
            }

            objCercaVirtual.Excluir(id, CodigoFuncionario);

            var cercaVirtualBLL = new BLL.CercaVirtual(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

            var dados = cercaVirtualBLL.GetAllList(CodigoFuncionario);
            ViewBag.Btn = false;
            if (dados.Count() > 0)
                ViewBag.Btn = true;

            return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
        }



        [PermissoesFiltro(Roles = "CercaVirtual")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }
        [PermissoesFiltro(Roles = "CercaVirtual")]
        public override ActionResult Alterar(int id)
        {
            ViewBag.Consultar = 0;
            return GetPagina(id);
        }


        protected override void ValidarForm(CercaVirtual obj)
        {
        }
    }
}