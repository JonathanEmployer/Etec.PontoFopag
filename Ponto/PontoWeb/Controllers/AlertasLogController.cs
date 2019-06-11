using HtmlAgilityPack;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class AlertasLogController : Controller
    {
        [PermissoesFiltro(Roles = "AlertasLog")]
        public ActionResult Grid(int id)
        {
            AlertasLog aLog = new AlertasLog();
            BLL.Alertas bllAlertas = new BLL.Alertas(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            aLog.Alerta = bllAlertas.LoadObject(id);
            return View(aLog);
        }


        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.AlertasLog bllAlertasLog = new BLL.AlertasLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.AlertasLog> dados = bllAlertasLog.GetAllListByAlerta(id);
                string conteudoAlerta = @"<div>
                                            <button type='button' class='btn btn-primary btn-sm btnConteudoEmail' onclick='AddConteudoAlerta($(this))' data-toggle='modal' data-target='#modalEmail' conteudo-alerta='##Conteudo'><span class='glyphicon glyphicon-eye-open' aria-hidden='true'></span> Conteúdo e-mail</button>
                                        </div>";
                foreach (Modelo.AlertasLog log in dados.Where(w => w.Complemento != null && w.Complemento.Contains("Corpo =")))
                {
                    string html =  WebUtility.HtmlDecode(log.Complemento.Replace("Corpo =", ""));
                    string corpo = "";
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("/html/body");

                    if (bodyNode != null)
                    {
                        corpo = bodyNode.InnerHtml;
                    }
                    log.Complemento = conteudoAlerta.Replace("##Conteudo", corpo);
                    //log.Complemento = conteudoAlerta.Replace("##idReg", log.Id.ToString());
                }
                //dados.Where(w => w.Complemento.Contains("Corpo =")).ToList().ForEach(f => f.Complemento = conteudoAlerta.Replace("##Conteudo", f.Complemento.Replace("Corpo =", "")));
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

        [PermissoesFiltro(Roles = "AlertasLogConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AlertasLogCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "AlertasLogAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AlertasLogCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(AlertasLog obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AlertasLogAlterar")]
        [HttpPost]
        public ActionResult Alterar(AlertasLog obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AlertasLogExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.AlertasLog bllAlertasLog = new BLL.AlertasLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            AlertasLog AlertasLog = bllAlertasLog.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllAlertasLog.Salvar(Acao.Excluir, AlertasLog);
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
            if (ex.Message.Contains("FK_Funcionario_AlertasLog"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Salvar(AlertasLog obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.AlertasLog bllAlertasLog = new BLL.AlertasLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllAlertasLog.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "AlertasLog");
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

        protected ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.AlertasLog bllAlertasLog = new BLL.AlertasLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            AlertasLog AlertasLog = new AlertasLog();
            AlertasLog = bllAlertasLog.LoadObject(id);
            if (id == 0)
            {
                AlertasLog.Codigo = bllAlertasLog.MaxCodigo();
            }
            return View("Cadastrar", AlertasLog);
        }

        protected void ValidarForm(AlertasLog obj)
        {
            throw new NotImplementedException();
        }
    }
}
