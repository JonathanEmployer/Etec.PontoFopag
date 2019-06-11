using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class JornadaController : IControllerPontoWeb<Jornada>
    {
        [PermissoesFiltro(Roles = "Jornada")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Jornada());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Jornada bllJornada = new BLL.Jornada(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Jornada> dados = bllJornada.GetAllList();
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

        [PermissoesFiltro(Roles = "JornadaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JornadaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "JornadaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JornadaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Jornada obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JornadaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Jornada obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JornadaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Jornada bllJornada = new BLL.Jornada(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Jornada jornada = bllJornada.LoadObject(id);
            try
            {
                string erro = "";
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllJornada.Salvar(Acao.Excluir, jornada);
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

        protected override ActionResult Salvar(Jornada obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Jornada bllJornada = new BLL.Jornada(usr.ConnectionString, usr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(usr.ConnectionString, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    PadraoCampos(obj);

                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllJornada.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        if ((obj.Entrada_1 != obj.Entrada_1Ant || obj.Saida_1 != obj.Saida_1Ant
                            || obj.Entrada_2 != obj.Entrada_2Ant || obj.Saida_2 != obj.Saida_2Ant
                            || obj.Entrada_3 != obj.Entrada_3Ant || obj.Saida_3 != obj.Saida_3Ant
                            || obj.Entrada_4 != obj.Entrada_4Ant || obj.Saida_4 != obj.Saida_4Ant)
                        && erros.Count == 0)
                        {
                            bllJornada.AtualizaHorarios(obj);
                            if (acao != Modelo.Acao.Incluir)
                            {
                                List<Modelo.Proxy.PxyIdPeriodo> recalcular = bllJornada.GetFuncionariosRecalculo(obj.Id);
                                HangfireManagerCalculos hfm = new HangfireManagerCalculos(usr.DataBase, "", "", "/Jornada/Grid");
                                string parametrosExibicao = String.Format("Jornada {0} | {1}, {2} funcionários vinculados", obj.Codigo, obj.horarios, recalcular.Count());
                                Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por jornada", parametrosExibicao, recalcular);
                            }
                        }
                        return RedirectToAction("Grid", "Jornada");
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
            BLL.Jornada bllJornada = new BLL.Jornada(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Jornada jornada = new Jornada();
            jornada = bllJornada.LoadObject(id);
            if (id == 0)
            {
                jornada.Codigo = bllJornada.MaxCodigo();
            }
            else
            {
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    string erro = "";
                    if (!validaFechamento(bllJornada, jornada.Id, out erro))
                    {
                        @ViewBag.MensagemBloqueio = erro;
                    }
                }
                #endregion 
            }
            
            return View("Cadastrar",jornada);
        }

        private bool validaFechamento(BLL.Jornada bllJornada, int idJornada, out string erro)
        {
            List<FechamentoPonto> lFechamentoPonto = bllJornada.FechamentoPontoJornada(idJornada);
            if (lFechamentoPonto.Count > 0)
            {
                erro = "Operação não permitida, existe fechamento de ponto nesse período. Fechamento: <br/> " + String.Join("<br/> ", lFechamentoPonto.Select(s => "Código: " + s.Codigo + " Data: " + s.DataFechamento.ToShortDateString()));
                return false;
            }
            erro = "";
            return true;
        }

        protected override void ValidarForm(Jornada obj)
        {
            
        }

        private static void PadraoCampos(Jornada jornada)
        {
            string padrao = "--:--";
            if (String.IsNullOrEmpty(jornada.Entrada_2))
            {
                jornada.Entrada_2 = padrao;
            }
            if (String.IsNullOrEmpty(jornada.Entrada_3))
            {
                jornada.Entrada_3 = padrao;
            }
            if (String.IsNullOrEmpty(jornada.Entrada_4))
            {
                jornada.Entrada_4 = padrao;
            }
            if (String.IsNullOrEmpty(jornada.Saida_2))
            {
                jornada.Saida_2 = padrao;
            }
            if (String.IsNullOrEmpty(jornada.Saida_3))
            {
                jornada.Saida_3 = padrao;
            }
            if (String.IsNullOrEmpty(jornada.Saida_4))
            {
                jornada.Saida_4 = padrao;
            }
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            BLL.Jornada bllJornada = new BLL.Jornada(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            IList<Jornada> lJornada = new List<Jornada>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Jornada jornada = bllJornada.LoadObjectCodigo(codigo);
                if (jornada != null && jornada.Id > 0)
                {
                    lJornada.Add(jornada);
                }
            }

            if (lJornada.Count == 0)
            {
                lJornada = bllJornada.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lJornada = lJornada.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Jornada";
            return View(lJornada);
        }

        public static int BuscaIdJornada(string jornada)
        {
            Jornada objJornada = new Jornada();
            BLL.Jornada bllJornada = new BLL.Jornada(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            string codigo = jornada.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            objJornada = bllJornada.LoadObjectCodigo(cod);
            return objJornada.Id;
        }
    }
}