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
    public class GrupoUsuarioController : Controller
    {
        [PermissoesFiltro(Roles = "GrupoUsuario")]
        [HttpGet]
        public ActionResult Grid()
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

        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "GrupoUsuarioCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "GrupoUsuarioAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "GrupoUsuarioCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(Cw_Grupo obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "GrupoUsuarioAlterar")]
        [HttpPost]
        public ActionResult Alterar(Cw_Grupo obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "GrupoUsuarioExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            BLL.Cw_Grupo bllCw_Grupo = new BLL.Cw_Grupo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Dictionary<string, string> erros = new Dictionary<string, string>();
            Modelo.Cw_Grupo grupoSelecionado = bllCw_Grupo.LoadObject(id);
            cwkAcao acao = cwkAcao.Excluir;

            try
            {
                erros = bllCw_Grupo.Salvar(acao, grupoSelecionado);
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

        private ActionResult Salvar(Cw_Grupo obj)
        {
            BLL.Cw_Grupo bllCw_Grupo = new BLL.Cw_Grupo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            if (ModelState.IsValid)
            {
                try
                {
                    cwkAcao acao = new cwkAcao();
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    if (obj.Id == 0)
                        acao = cwkAcao.Incluir;
                    else
                        acao = cwkAcao.Alterar;

                    erros = bllCw_Grupo.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "GrupoUsuario");
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

        private ActionResult GetPagina(int id)
        {
            BLL.Cw_Grupo bllCw_Grupo = new BLL.Cw_Grupo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Cw_Grupo grupoSelecionado = new Cw_Grupo();
            grupoSelecionado = bllCw_Grupo.LoadObject(id);

            if (id == 0)
            {
                grupoSelecionado.Codigo = bllCw_Grupo.MaxCodigo();
            }

            return View("Cadastrar", grupoSelecionado);
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            IList<Cw_Grupo> lGrupo = PesquisaGrupo(consulta, true);
            ViewBag.Title = "Pesquisar Grupo";
            return View(lGrupo);
        }

        private IList<Cw_Grupo> PesquisaGrupo(string consulta, bool opcaotodas)
        {
            BLL.Cw_Grupo bllCw_Grupo = new BLL.Cw_Grupo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            IList<Cw_Grupo> lGrupo = new List<Cw_Grupo>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Cw_Grupo e = bllCw_Grupo.LoadObjectbyCodigo(codigo);
                if (e != null && e.Id > 0)
                {
                    lGrupo.Add(e);
                }
            }

            if (lGrupo.Count == 0)
            {
                lGrupo = bllCw_Grupo.getAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    var t = lGrupo.Where(p => p.Nome.ToUpper().Contains(consulta.ToUpper()));
                    lGrupo = lGrupo.Where(p => p.Nome.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            return lGrupo;
        }

        public static int BuscaIdgrupo(string grupo)
        {
            BLL.Cw_Grupo bllCw_Grupo = new BLL.Cw_Grupo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            int id = 0;
            try
            {
                if (!String.IsNullOrEmpty(grupo))
                {
                    string cod = grupo.Split('|')[0].Trim();
                    int codigo = Convert.ToInt32(cod);
                    id = bllCw_Grupo.LoadObjectbyCodigo(codigo) == null ? 0 : bllCw_Grupo.LoadObjectbyCodigo(codigo).Id;
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