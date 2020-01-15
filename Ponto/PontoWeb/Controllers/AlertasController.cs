using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class AlertasController : IControllerPontoWeb<Alertas>
    {
        [PermissoesFiltro(Roles = "Alertas")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Alertas());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                BLL.Alertas bllAlertas = new BLL.Alertas(conn, usr);
                List<Modelo.Alertas> dados = bllAlertas.GetAllList();
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

        [PermissoesFiltro(Roles = "AlertasConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AlertasCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "AlertasAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AlertasCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Alertas obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AlertasAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Alertas obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AlertasExcluir")]
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
                        return RedirectToAction("Grid", "Alertas");
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
                alertas.UltimaExecucao = DateTime.Now.AddDays(-1);
            }
            else
            {
                BLL.AlertasFuncionario bllAlertasFunc = new BLL.AlertasFuncionario(conn, usr);
                List<AlertasFuncionario> alertasFuncs = bllAlertasFunc.GetAllPorAlerta(alertas.Id);
                alertas.IdFuncsSelecionados = String.Join(",", alertasFuncs.Select(s => s.IDFuncionario).ToList());
                alertas.IdFuncsSelecionados_Ant = alertas.IdFuncsSelecionados;
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
            TimeSpan iniVeri = obj.InicioVerificacao.Add(new TimeSpan(0, 5, 0));
            if (iniVeri > obj.FimVerificacao)
                ModelState.AddModelError("FimVerificacao", "Fim deve ser maior que início + 5 min");

            var dataLimite = DateTime.Now.AddDays(-8);
            if (obj.UltimaExecucao.HasValue && obj.UltimaExecucao < dataLimite)
                ModelState.AddModelError("UltimaExecucao", $"A data limite para ultima execução é {dataLimite.ToString("dd/MM/yyyy")}");

            if(obj.Descricao == null || obj.Descricao == string.Empty)
                ModelState.AddModelError("Descricao", "O campo descrição é obrigatório");
        }

        #region Métodos auxiliares

        #endregion
    }
}
