using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            BLL.JornadaSubstituirFuncionario bllJornadaSubstituirFuncionario = new BLL.JornadaSubstituirFuncionario(usr.ConnectionString, usr);
            jornadaSubstituir.JornadaSubstituirFuncionario = bllJornadaSubstituirFuncionario.GetByIdJornadaSubstituir(jornadaSubstituir.Id);
            jornadaSubstituir.JornadaSubstituirFuncionario.ForEach(f => f.Acao = Acao.Excluir);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllJornadaSubstituir.Salvar(Acao.Excluir, jornadaSubstituir);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + " = " + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                string parametrosExibicao = $"Jornada {jornadaSubstituir.DescricaoDe} para {jornadaSubstituir.DescricaoPara} no período {jornadaSubstituir.DataInicioStr} a {jornadaSubstituir.DataFimStr} (Funcionários: {jornadaSubstituir.JornadaSubstituirFuncionario.Count} excluído(s); )";
                HangfireManagerCalculos hfm = new HangfireManagerCalculos(usr.DataBase, "", "", "/JornadaSubstituir/Grid");
                PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por mudança de jornada", parametrosExibicao, 2, jornadaSubstituir.JornadaSubstituirFuncionario.Select(s => Convert.ToInt32(s.IdFuncionario)).ToList(), jornadaSubstituir.DataInicio.GetValueOrDefault(), jornadaSubstituir.DataFim.GetValueOrDefault());
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
            if (ModelState.IsValid)
            {
                ValidarErro(obj);
                if (ModelState.IsValid)
                {
                    try
                    {
                        Acao acao = obj.Id == 0 ? acao = Acao.Incluir : acao = Acao.Alterar;
                        BLL.JornadaSubstituir bllJornadaSubstituir = new BLL.JornadaSubstituir(usr.ConnectionString, usr);
                        BLL.JornadaSubstituirFuncionario bllJornadaSubstituirFuncionario = new BLL.JornadaSubstituirFuncionario(usr.ConnectionString, usr);
                        obj.JornadaSubstituirFuncionario = bllJornadaSubstituirFuncionario.GetByIdJornadaSubstituir(obj.Id);
                        List<int> idsFuncionarios = obj.IdFuncsSelecionados.Split(',').Where(w => !string.IsNullOrEmpty(w))
                                                                           .Select(s => Convert.ToInt32(s)).ToList();
                        //Os funcionários que não estão mais selecionados devem ser excluídos
                        obj.JornadaSubstituirFuncionario.Where(w => !idsFuncionarios.Contains(w.IdFuncionario)).ToList()
                                                        .ForEach(f => f.Acao = Acao.Excluir);
                        //Os funcionários que foram selecionados e nao estavam devem ser adicionados
                        obj.JornadaSubstituirFuncionario.AddRange(idsFuncionarios.Where(w => !obj.JornadaSubstituirFuncionario.Select(s => s.IdFuncionario).Contains(w)).ToList()
                                                                                 .Select(idFuncionarioNovo => new JornadaSubstituirFuncionario() { IdFuncionario = idFuncionarioNovo, IdJornadaSubstituir = obj.Id, Acao = Acao.Incluir }));
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        JornadaSubstituir jornadaAnterior = bllJornadaSubstituir.LoadObject(obj.Id);
                        erros = bllJornadaSubstituir.Salvar(acao, obj);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        else
                        {
                            DateTime? dtIni = jornadaAnterior != null && jornadaAnterior.Id > 0 && jornadaAnterior.DataInicio < obj.DataInicio ? jornadaAnterior.DataInicio : obj.DataInicio;
                            DateTime? dtFim = jornadaAnterior != null && jornadaAnterior.Id > 0 && jornadaAnterior.DataFim > obj.DataFim ? jornadaAnterior.DataFim : obj.DataFim;
                            int qtdInclusao = obj.JornadaSubstituirFuncionario.Where(w => w.Acao == Acao.Incluir).Count();
                            int qtdExclusao = obj.JornadaSubstituirFuncionario.Where(w => w.Acao == Acao.Excluir).Count();
                            if (qtdInclusao > 0 || qtdExclusao > 0 || jornadaAnterior.DataInicio != obj.DataInicio || jornadaAnterior.DataFim != obj.DataFim ||
                                jornadaAnterior.IdJornadaDe != obj.IdJornadaDe || jornadaAnterior.IdJornadaPara != obj.IdJornadaPara)
                            {
                                string alterados = "";
                                List<int> IdsFuncsRecalcular = obj.JornadaSubstituirFuncionario.Where(w => w.Acao == Acao.Incluir || w.Acao == Acao.Excluir).Select(s => s.IdFuncionario).ToList();
                                string incluidos = qtdInclusao == 0 ? "" : qtdInclusao + " incluído(s); ";
                                string excluidos = qtdExclusao == 0 ? "" : qtdExclusao + " excluído(s); ";

                                List<int> idsFuncsAlterados = obj.JornadaSubstituirFuncionario.Where(w => w.Acao != Acao.Incluir && w.Acao != Acao.Excluir).Select(s => s.IdFuncionario).ToList();
                                if (idsFuncsAlterados.Any() && 
                                    (jornadaAnterior.DataInicio != obj.DataInicio || jornadaAnterior.DataFim != obj.DataFim ||
                                     jornadaAnterior.IdJornadaDe != obj.IdJornadaDe || jornadaAnterior.IdJornadaPara != obj.IdJornadaPara))
                                {
                                    IdsFuncsRecalcular.AddRange(idsFuncsAlterados);
                                    alterados = idsFuncsAlterados.Count + " alterado(s); ";
                                }
                                string parametrosExibicao = $"Jornada {obj.DescricaoDe} para {obj.DescricaoPara} no período {dtIni.GetValueOrDefault().ToShortDateString()} a {dtFim.GetValueOrDefault().ToShortDateString()} (Funcionários: {incluidos}{excluidos}{alterados})";
                                HangfireManagerCalculos hfm = new HangfireManagerCalculos(usr.DataBase, "", "", "/JornadaSubstituir/Grid");
                                PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por mudança de jornada", parametrosExibicao, 2, IdsFuncsRecalcular, dtIni.GetValueOrDefault(), dtFim.GetValueOrDefault()); 
                            }
                            return RedirectToAction("Grid", "JornadaSubstituir");
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        ModelState.AddModelError("CustomError", ex.Message);
                    }
                }
            }
            return View("Cadastrar", obj);
        }

        private void ValidarErro(JornadaSubstituir obj)
        {
            string erro;
            ValidarJornada(obj.DescricaoDe, out int idJornadaDe, out erro);
            if (string.IsNullOrEmpty(erro))
                obj.IdJornadaDe = idJornadaDe;
            else
                ModelState["DescricaoDe"].Errors.Add("Selecione a jornada a ser substituída.");

            ValidarJornada(obj.DescricaoPara, out int idJornadaPara, out erro);
            if (string.IsNullOrEmpty(erro))
                obj.IdJornadaPara = idJornadaPara;
            else
                ModelState["DescricaoPara"].Errors.Add("Selecione a jornada substituta.");
        }

        private void ValidarJornada(string codigoDescricao, out int idJornada, out string erro)
        {
            idJornada = 0;
            erro = "";
            if (String.IsNullOrEmpty(codigoDescricao))
            {
                erro = "Selecione a jornada a ser substituída.";
            }
            else
            {
                BLL.Jornada bllJornada = new BLL.Jornada(usr.ConnectionString, usr);
                Jornada j = new Jornada();
                List<string> strs = codigoDescricao.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string jornada = strs[0];
                if (int.TryParse(jornada, out int codigoJornada))
                {
                    j = bllJornada.LoadObjectCodigo(codigoJornada);
                }
                if (j != null && j.Id > 0)
                {
                    idJornada = j.Id;
                }
                else
                {
                    erro = "Jornada " + codigoDescricao + " não cadastrada!";
                }
            }
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
                        string[] errosJC = Regex.Split(err.Value, Environment.NewLine);
                        ViewBag.JornadasConflitantes = errosJC;
                    }
                    else if (err.Key == "Fechamentos")
                    {
                        string[] errosJC = Regex.Split(err.Value, Environment.NewLine);
                        ViewBag.Fechamentos = errosJC;
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
            BLL.JornadaSubstituirFuncionario bllJornadaSubstituirFuncionario = new BLL.JornadaSubstituirFuncionario(usr.ConnectionString, usr);
            JornadaSubstituir jornadaSubstituir = bllJornadaSubstituir.LoadObject(id);
            var jornadaSubstituirFuncionario = bllJornadaSubstituirFuncionario.GetByIdJornadaSubstituir(jornadaSubstituir.Id);
            jornadaSubstituir.IdFuncsSelecionados = string.Join(",", jornadaSubstituirFuncionario.Select(s => s.IdFuncionario).ToList());
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