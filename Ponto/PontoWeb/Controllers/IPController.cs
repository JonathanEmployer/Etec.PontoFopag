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
    public class IPController : Controller
    {
        [PermissoesFiltro(Roles = "IP")]
        public ActionResult Grid(int id)
        {
            ViewBag.IdEmpresa = id;
            return View(new IP());
        }

        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.IP bllIP = new BLL.IP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

                IEnumerable<IP> dados = bllIP.GetAllListPorEmpresa(id);
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

        [PermissoesFiltro(Roles = "IPConsultar")]
        public ActionResult Consultar(int id = 0, int idEmp = 0)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id, idEmp);
        }

        [PermissoesFiltro(Roles = "IPCadastrar")]
        public ActionResult Cadastrar(int id = 0, int idEmp = 0)
        {
            return GetPagina(id, idEmp);
        }

        [PermissoesFiltro(Roles = "IPAlterar")]
        public ActionResult Alterar(int id = 0, int idEmp = 0)
        {
            return GetPagina(id, idEmp);
        }

        [PermissoesFiltro(Roles = "IPCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(IP obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "IPAlterar")]
        [HttpPost]
        public ActionResult Alterar(IP obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "IPExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.IP bllIP = new BLL.IP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IP IP = bllIP.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllIP.Salvar(Acao.Excluir, IP);
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

        protected ActionResult Salvar(IP obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.IP bllIP = new BLL.IP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            ValidarForm(obj);
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
                    erros = bllIP.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", new { id = obj.IdEmpresa });
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return PartialView("Cadastrar", obj);
        }

        protected ActionResult GetPagina(int id, int idEmp)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.IP bllIP = new BLL.IP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IP IP = new IP();
            IP = bllIP.LoadObject(id);
            if (id == 0)
            {
                IP.Codigo = bllIP.MaxCodigo();
                IP.Tipo = 0;
                IP.IdEmpresa = idEmp;
                IP.BloqueiaRegistrador = true;
            }
            return View("Cadastrar", IP);
        }

        protected void ValidarForm(IP obj)
        {
            if ((obj.Tipo == 0 || obj.Tipo == 2) && !BLL.cwkFuncoes.ValidaIP(obj.IPDNS))
            {
                ModelState["IPDNS"].Errors.Add("Ip inválido!");
            }

        }
    }
}