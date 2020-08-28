using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class EventosController : IControllerPontoWeb<Eventos>
    {

        [PermissoesFiltro(Roles = "Eventos")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Eventos bllEventos = new BLL.Eventos(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.Eventos());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Eventos bllEventos = new BLL.Eventos(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Eventos> dados = bllEventos.GetAllList();
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

        [PermissoesFiltro(Roles = "EventosConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "EventosCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "EventosAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "EventosCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Eventos obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EventosAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Eventos obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EventosExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Eventos bllEventos = new BLL.Eventos(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Eventos evt = bllEventos.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();

                erros = bllEventos.Salvar(Acao.Excluir, evt);
                string erro = String.Empty;
                if (erros.Count > 0)
                {
                    erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
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

        protected override ActionResult Salvar(Eventos obj)
        {
            obj.NaoValidaCodigo = true;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Eventos bllEventos = new BLL.Eventos(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            ValidarForm(obj);
            ValidarPercentualExtra(obj);
            if (!obj.bOcorrenciasSelecionadas)
            {
                obj.IdsOcorrencias = null;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Acao act;
                    if (obj.Id > 0)
                    {
                        act = Acao.Alterar;
                    }
                    else
                    {
                        act = Acao.Incluir;
                    }
                    
                    Dictionary<string, string> err = bllEventos.Salvar(act, obj);
                    if (err.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        string erro = string.Join(";", err.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                        return View("Cadastrar", obj);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                    return View("Cadastrar", obj);
                }
                return RedirectToAction("Grid", "Eventos");
            }
            else
            {
                return View("Cadastrar",obj);
            }
        }

        private void ValidarPercentualExtra(Eventos obj)
        {
            Type tipoObj = obj.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(tipoObj.GetProperties());
            List<object> listPercentualHE = new List<object>();
            Dictionary<string, string> dictioValue = new Dictionary<string, string>();
            foreach (PropertyInfo prop in props)
            {
                string propName = prop.Name;
                object propValue = prop.GetValue(obj, null);
                if (propName.StartsWith("PercentualExtra") && Convert.ToDecimal(propValue.ToString()) != 0)
                {
                    dictioValue.Add(propName, propValue.ToString());
                    listPercentualHE.Add(propValue);
                }
            }
            bool verificaValorDuplicado = listPercentualHE.GroupBy(x => x).Any(c => c.Count() > 1);
            if (verificaValorDuplicado == true)
            {
                ModelState[dictioValue.Keys.LastOrDefault().ToString()].Errors.Add("Registro duplicado, por favor, verifique o valor inserido!");
            }
        }

        protected override ActionResult GetPagina(int id)
        {
            Eventos ev = new Eventos();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Eventos bllEventos = new BLL.Eventos(conn, usr);
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(conn, usr);
            if (id == 0)
            {
                ev.Codigo = bllEventos.MaxCodigo();
                ev.Tipohoras = 0;
                ev.Ocorrencias = bllOcorrencia.GetAllOcorrenciaEventoList();
            }
            else
            {
                ev = bllEventos.LoadObject(id);
                ev.Ocorrencias = bllOcorrencia.GetAllOcorrenciaEventoList();
                List<int> idsOcorrencias = ev.GetIdsOcorrencias().ToList();
                ev.Ocorrencias.ForEach((f) =>
                {
                    if (idsOcorrencias.Contains(f.Id))
                    {
                        f.Selecionado = true;
                    }
                });

                BLL.EventosClassHorasExtras bllEventosClassHorasExtras = new BLL.EventosClassHorasExtras(conn, usr);
                ev.IdsClassificadas = bllEventosClassHorasExtras.GetIdsClassificacaoPorEvento(ev.Id);
            }
            BLL.Classificacao bllClassificacao = new BLL.Classificacao(conn, usr);
            ev.ClassificacaoHorasExtras = bllClassificacao.GetAllList();

            return View("Cadastrar",ev);
        }

        protected override void ValidarForm(Eventos obj)
        {
            if (!obj.ClassificarHorasExtras)
            {
                string msg = "Campo com horas preenchidas, devem ser obrigatóriamente marcados com pelo menos uma das opções (Diurna/Noturna)";
                if (obj.PercentualExtra1 > 0 && (!obj.bHe50 && !obj.bHe50N))
                {
                    ModelState["PercentualExtra1"].Errors.Add(msg);
                }
                if (obj.PercentualExtra2 > 0 && (!obj.bHe60 && !obj.bHe60N))
                {
                    ModelState["PercentualExtra2"].Errors.Add(msg);
                }
                if (obj.PercentualExtra3 > 0 && (!obj.bHe70 && !obj.bHe70N))
                {
                    ModelState["PercentualExtra3"].Errors.Add(msg);
                }
                if (obj.PercentualExtra4 > 0 && (!obj.bHe80 && !obj.bHe80N))
                {
                    ModelState["PercentualExtra4"].Errors.Add(msg);
                }
                if (obj.PercentualExtra5 > 0 && (!obj.bHe90 && !obj.bHe90N))
                {
                    ModelState["PercentualExtra5"].Errors.Add(msg);
                }
                if (obj.PercentualExtra6 > 0 && (!obj.bHe100 && !obj.bHe100N))
                {
                    ModelState["PercentualExtra6"].Errors.Add(msg);
                }
                if (obj.PercentualExtra7 > 0 && (!obj.bHesab && !obj.bHesabN))
                {
                    ModelState["PercentualExtra7"].Errors.Add(msg);
                }
                if (obj.PercentualExtra8 > 0 && (!obj.bHedom && !obj.bHedomN))
                {
                    ModelState["PercentualExtra8"].Errors.Add(msg);
                }
                if (obj.PercentualExtra9 > 0 && (!obj.bHefer && !obj.bHeferN))
                {
                    ModelState["PercentualExtra9"].Errors.Add(msg);
                }
                if (obj.PercentualExtra10 > 0 && (!obj.bFolga && !obj.bFolgaN))
                {
                    ModelState["PercentualExtra10"].Errors.Add(msg);
                } 
            }
        }
    }
}