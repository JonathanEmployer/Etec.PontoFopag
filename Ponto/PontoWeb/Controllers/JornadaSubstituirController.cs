using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
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
                        TrataErros(erros);
                    }
                    else
                    {
                        JornadaSubstituir jornadaAnterior = bllJornadaSubstituir.LoadObject(obj.Id);
                        DateTime? dtIni = jornadaAnterior != null && jornadaAnterior.Id > 0 && jornadaAnterior.DataInicio < obj.DataInicio ? jornadaAnterior.DataInicio : obj.DataInicio;
                        DateTime? dtFim = jornadaAnterior != null && jornadaAnterior.Id > 0 && jornadaAnterior.DataFim < obj.DataFim ? jornadaAnterior.DataFim : obj.DataFim;
                        HangfireManagerCalculos hfm = new HangfireManagerCalculos(usr.DataBase,"","","/JornadaSubstituir/Grid");
                        int qtdInclusao = obj.JornadaSubstituirFuncionario.Where(w => w.Acao == Acao.Incluir).Count();
                        int qtdExclusao = obj.JornadaSubstituirFuncionario.Where(w => w.Acao == Acao.Excluir).Count();
                        string parametrosExibicao = $"Jornada {obj.DescricaoDe} para {obj.DescricaoPara} de {qtdInclusao} funcionários incluídos e {qtdExclusao} removidos no período {obj.DataInicioStr} a {obj.DataFimStr}";
                        PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por mudança de jornada", parametrosExibicao, 2, obj.JornadaSubstituirFuncionario.Where(w => w.Acao == Acao.Incluir || w.Acao == Acao.Excluir).Select(s => s.IdFuncionario).ToList(), dtIni.GetValueOrDefault(), dtFim.GetValueOrDefault());
                        return RedirectToAction("Grid", "JornadaSubstituir");
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

        private void TrataErros(Dictionary<string, string> erros)
        {
            List<string> propriedades = typeof(Modelo.Funcionario).GetProperties().Select(s => s.Name).ToList();

            List<string> errosCustom = new List<string>();
            if (erros.Count() > 0)
            {
                foreach (var err in erros)
                {
                    string prop = propriedades.Where(w => w == err.Key).FirstOrDefault();
                    if (!String.IsNullOrEmpty(prop))
                    {
                        ModelState.AddModelError(prop, err.Value);
                    }
                    else if (err.Key == "JornadasConflitantes")
                    {
                        ModelState.AddModelError(prop, err.Value);
                    }
                    else if (err.Key == "Fechamentos")
                    {
                        ModelState.AddModelError(prop, err.Value);
                    }
                    else
                    {
                        errosCustom.Add(err.Key + " = " + err.Value);
                    }
                }
            }

            if (errosCustom.Count > 0)
            {
                string erro = string.Join(";", errosCustom);
                ModelState.AddModelError("CustomError", erro);
            }
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