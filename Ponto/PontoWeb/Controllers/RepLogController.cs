using BLL_N.IntegracaoTerceiro;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class RepLogController : Controller
    {
        [PermissoesFiltro(Roles = "RepLog")]
        public ActionResult Grid(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.REP bllRep = new BLL.REP(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            RepLog replog = new RepLog();
            replog.Rep = bllRep.LoadObject(id);
            return View(replog);
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.RepLog bllRepLog = new BLL.RepLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.RepLog> dados = bllRepLog.GetAllListByRep(id);
                //dados.Where(w => w.Complemento.Contains("Corpo =")).ToList().ForEach(f => f.Complemento = conteudoAlerta.Replace("##Conteudo", f.Complemento.Replace("Corpo =", "")));
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                throw e;
            }
        }

        [PermissoesFiltro(Roles = "RepLogConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "RepLogCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "RepLogAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "RepLogCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(RepLog obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "RepLogAlterar")]
        [HttpPost]
        public ActionResult Alterar(RepLog obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "RepLogExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.RepLog bllRepLog = new BLL.RepLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            RepLog RepLog = bllRepLog.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllRepLog.Salvar(Acao.Excluir, RepLog);
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
            if (ex.Message.Contains("FK_Funcionario_RepLog"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult Salvar(RepLog obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.RepLog bllRepLog = new BLL.RepLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllRepLog.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "RepLog");
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
            BLL.RepLog bllRepLog = new BLL.RepLog(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            RepLog RepLog = new RepLog();
            RepLog = bllRepLog.LoadObject(id);
            if (id == 0)
            {
                RepLog.Codigo = bllRepLog.MaxCodigo();
            }
            return View("Cadastrar", RepLog);
        }

        protected void ValidarForm(RepLog obj)
        {
            throw new NotImplementedException();
        }

        public ActionResult ResultadoImportacao(string chave)
        {
            try
            {
                UsuarioPontoWeb usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL_N.Exportacao.RepLogAFD bllRepLog = new BLL_N.Exportacao.RepLogAFD(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                byte[] arq = bllRepLog.GeraXLSLogAFD(chave);
                return new FileContentResult(arq, "application/vnd.ms-excel")
                {
                    FileDownloadName = "RepLogImportacao = " + chave + ".xls"
                };
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
