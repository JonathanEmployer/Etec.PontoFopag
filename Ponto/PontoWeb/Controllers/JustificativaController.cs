using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class JustificativaController : IControllerPontoWeb<Justificativa>
    {
        [PermissoesFiltro(Roles = "Justificativa")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.Justificativa());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Justificativa> dados = bllJustificativa.GetAllList();
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

        [PermissoesFiltro(Roles = "JustificativaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JustificativaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "JustificativaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JustificativaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Justificativa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JustificativaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Justificativa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JustificativaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Justificativa justificativa = bllJustificativa.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllJustificativa.Salvar(Acao.Excluir, justificativa);
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

        protected override ActionResult Salvar(Justificativa obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllJustificativa.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Justificativa");
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
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Justificativa justificativa = new Justificativa();
            if (id == 0)
            {
                justificativa.Codigo = bllJustificativa.MaxCodigo();
                justificativa.Ativo = true;
            }
            else
            {
                justificativa = bllJustificativa.LoadObject(id);
            }
            return View("Cadastrar", justificativa);
        }

        protected override void ValidarForm(Justificativa obj)
        {
            
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<Justificativa> ljust = new List<Justificativa>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllJustificativa.GetIdPorCod(codigo).GetValueOrDefault();
                Justificativa just = bllJustificativa.LoadObject(id);
                if (just != null && just.Id > 0)
                {
                    ljust.Add(just);
                }
            }

            if (ljust.Count == 0)
            {
                ljust = bllJustificativa.GetAllListConsultaEvento();
                if (!String.IsNullOrEmpty(consulta))
                {
                    ljust = ljust.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Justificativa";
            return View(ljust);
        }


        public ActionResult ValidaDescJustificativa(string consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            int codigo = -1;
            consulta = consulta.Split('|')[0];
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Justificativa just = bllJustificativa.LoadObjectByCodigo(codigo);
                if (just != null && just.Id > 0)
                {
                    return Json(new { Success = true, Erro = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Success = false, Erro = "Justificativa " + consulta + " não encontrada, Verifique!" }, JsonRequestBehavior.AllowGet);
        }

        public static int BuscaIdJustificativa(string consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            int id = 0;
            try
            {
                int codigo = -1;
                consulta = consulta.Split('|')[0];
                try { codigo = Int32.Parse(consulta); }
                catch (Exception) { codigo = -1; }
                if (codigo != -1)
                {
                    Justificativa just = bllJustificativa.LoadObjectByCodigo(codigo);
                    if (just != null && just.Id > 0)
                    {
                        return just.Id;
                    }
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}