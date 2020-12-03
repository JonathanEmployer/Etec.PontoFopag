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
    public class AlertasComunicacaoRepController : IControllerPontoWeb<Alertas>
    {
        [PermissoesFiltro(Roles = "AcompanhamentoRep")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Proxy.PxyGridAlertasComunicacaoRep());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                BLL.Alertas bllAlertas = new BLL.Alertas(conn, usr);
                List<PxyGridAlertasComunicacaoRep> dados = bllAlertas.GetAllListAcompanhamentoRep();
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

        [PermissoesFiltro(Roles = "AcompanhamentoRepConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AcompanhamentoRepCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "AcompanhamentoRepAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AcompanhamentoRepCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Alertas obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AcompanhamentoRepAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Alertas obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AcompanhamentoRepExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Alertas bllAlertas = new BLL.Alertas(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Alertas Alertas = bllAlertas.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllAlertas.Salvar(Acao.Excluir, Alertas);
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
            if (ex.Message.Contains("FK_Funcionario_Alertas"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(Alertas obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            var conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.Alertas bllAlertas = new BLL.Alertas(conn, usr);
            ValidarForm(obj);
            ModelState.Remove("FimVerificacao");
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
                    erros = bllAlertas.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> erro in erros)
                        {
                            ModelState.AddModelError(erro.Key, erro.Value);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Grid");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            AlertasDispononiveis(usr, conn);
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.Alertas bllAlertas = new BLL.Alertas(conn, usr);
            Alertas alertas = new Alertas();
            alertas = bllAlertas.LoadObject(id);
            if (id == 0)
            {
                alertas.Codigo = bllAlertas.MaxCodigo();
                alertas.DiasSemanaEnvio = "1,2,3,4,5,6,7";
                alertas.Ativo = true;
                alertas.Tipo = "";
                alertas.IdFuncsSelecionados = "";
                alertas.IdFuncsSelecionados_Ant = "";
                alertas.UltimaExecucao = DateTime.Now;
                alertas.EmailIndividual = true;
                alertas.ProcedureAlerta = "p_enviaAlertasAcompanhamentoRep";
                alertas.IntervaloVerificacao = new TimeSpan(0, 5, 0);
                alertas.Tolerancia = new TimeSpan(0,0,0);
                alertas.InicioVerificacao = new TimeSpan(0, 0, 0);
                alertas.FimVerificacao = new TimeSpan(23, 59, 59);
            }
            else
            {
                BLL.AlertasRepAcompanhamento bllAlertasRepAcompanhamento = new BLL.AlertasRepAcompanhamento(conn, usr);
                List<AlertasRepAcompanhamento> alertasRepAcompanhamento = bllAlertasRepAcompanhamento.GetAllPorAlerta(alertas.Id);
                alertas.IdRepsSelecionados = String.Join(",", alertasRepAcompanhamento.Select(s => s.IdRep).ToList());
                alertas.IdRepsSelecionados_Ant = alertas.IdRepsSelecionados;
            }

            AlertasDispononiveis(usr, conn);
            return View("Cadastrar", alertas);
        }

        private void AlertasDispononiveis(UsuarioPontoWeb usr, string conn)
        {
            BLL.AlertasDisponiveis bllAlertasDisponiveis = new BLL.AlertasDisponiveis(conn, usr);
            IList<AlertasDisponiveis> alertasDisponiveis = bllAlertasDisponiveis.GetAllList();
            ViewBag.AlertasDisponiveis = alertasDisponiveis.Select(x => new SelectListItem() { Text = x.Nome.Trim(), Value = x.NomeProcedure.Trim() });
        }

        protected override void ValidarForm(Alertas obj)
        {
            if (string.IsNullOrEmpty(obj.IntervaloVerificacaoLivre))
            {
                ModelState.AddModelError("IntervaloVerificacaoLivre", "Intervalo deve ser informado");
            }
        }

        #region Métodos auxiliares

        #endregion
    }
}
