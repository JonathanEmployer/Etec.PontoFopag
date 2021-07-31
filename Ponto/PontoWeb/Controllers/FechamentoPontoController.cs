using BLL_N.JobManager.Hangfire;
using Hangfire.States;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using PontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class FechamentoPontoController : IControllerPontoWeb<FechamentoPonto>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "FechamentoPonto")]
        public override ActionResult Grid()
        {
            return View(new Modelo.FechamentoPonto());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {

                BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
                List<Modelo.FechamentoPonto> dados = bllFechamentoPonto.GetAllList();
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

        [PermissoesFiltro(Roles = "FechamentoPontoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(FechamentoPonto obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(FechamentoPonto obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoPontoExcluir")]
        public override ActionResult Excluir(int id)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFunc = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);
            FechamentoPonto obj = bllFechamentoPonto.LoadObject(id);

            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                
                GetChanges(obj, bllFechamentoPonto, bllFechamentoPontoFunc);

                erros = bllFechamentoPonto.Salvar(Acao.Excluir, obj);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var hasDeleted = obj.FechamentoPontoFuncionarios.Where(f => f.Acao == Acao.Excluir).Select(f => f.IdFuncionario);
                    if (hasDeleted.Any())
                        HangfireDeleteEpays(obj.Id, hasDeleted);
                }

                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
         
        protected override ActionResult Salvar(FechamentoPonto obj)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFunc = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);
            BLL.Epays.FechamentoPontoEpaysBLL fechamentoPontoEpaysBLL = new BLL.Epays.FechamentoPontoEpaysBLL(_usr.ConnectionString);

            //ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    List<int> idsFuncsSel = new List<int>();
                    try
                    {
                        idsFuncsSel = obj.PxyRelPontoWeb.idSelecionados.Split(',').ToList().Select(s => int.Parse(s)).ToList();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Erro ao carregar a lista de funcionários selecionados");
                    }

                    Acao acao = GetChanges(obj, bllFechamentoPonto, bllFechamentoPontoFunc, idsFuncsSel);

                    //Verifica se Existe empresa com integração ativa c/ Epays sem configuração de Periodos
                    var lsFuncs = VerificaConfEmpresaEpays(idsFuncsSel);

                    //Adiciona os novos funcionarios selecionados
                    AddFuncionarios(obj, bllFechamentoPontoFunc, idsFuncsSel);

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllFechamentoPonto.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        ExportToEpays(obj, lsFuncs);

                        var hasDeleted = obj.FechamentoPontoFuncionarios.Where(f => f.Acao == Acao.Excluir).Select(f => f.IdFuncionario);
                        if (acao != Acao.Incluir && hasDeleted.Any())
                            HangfireDeleteEpays(obj.Id, hasDeleted);

                        return RedirectToAction("Grid", "FechamentoPonto");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            pxyRelPontoWeb RelPadrao = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(_usr.ConnectionString)).GetListagemFuncionariosRel(_usr);

            RelPadrao.idSelecionados = obj.PxyRelPontoWeb.idSelecionados;
            RelPadrao.UtilizaControleContrato = _usr.UtilizaControleContratos;
            RelPadrao.InicioPeriodo = DateTime.Now;
            RelPadrao.FimPeriodo = DateTime.Now;
            obj.PxyRelPontoWeb = RelPadrao;

            return View("Cadastrar", obj);
        }

        private void HangfireDeleteEpays(int idFechamento, IEnumerable<int> idsExcluidos)
        {
            Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
            HangfireIntegrationEpays hie = new HangfireIntegrationEpays(UserPW.DataBase);
            hie.SendToEpaysDeleted(idFechamento, idsExcluidos);
        }

        private void AddFuncionarios(FechamentoPonto obj, BLL.FechamentoPontoFuncionario bllFechamentoPontoFunc, List<int> idsFuncsSel)
        {
            foreach (int idFunc in idsFuncsSel)
            {
                FechamentoPontoFuncionario fpf = new FechamentoPontoFuncionario();
                fpf.IdFechamentoPonto = obj.Id;
                fpf.IdFuncionario = idFunc;
                fpf.Acao = Acao.Incluir;
                fpf.Codigo = bllFechamentoPontoFunc.MaxCodigo();
                if (obj.FechamentoPontoFuncionarios == null)
                {
                    obj.FechamentoPontoFuncionarios = new List<FechamentoPontoFuncionario>();
                }
                obj.FechamentoPontoFuncionarios.Add(fpf);
            }
        }

        private Acao GetChanges(FechamentoPonto obj, BLL.FechamentoPonto bllFechamentoPonto, BLL.FechamentoPontoFuncionario bllFechamentoPontoFunc, List<int> idsFuncsSel = null)
        {
            idsFuncsSel = idsFuncsSel ?? new List<int>();
            Acao acao = new Acao();
            if (obj.Id == 0)
            {
                acao = Acao.Incluir;
                obj.FechamentoPontoFuncionarios = new List<FechamentoPontoFuncionario>();
                obj.Codigo = bllFechamentoPonto.MaxCodigo();
            }
            else
            {
                acao = Acao.Alterar;
                obj.FechamentoPontoFuncionarios = bllFechamentoPontoFunc.GetListWhere("and idFechamentoPonto = " + obj.Id.ToString());

                //Seta todo mundo para ser excluido
                obj.FechamentoPontoFuncionarios.ToList().ForEach(i => i.Acao = Acao.Excluir);
                //os que foram selecionados seta para alterar
                obj.FechamentoPontoFuncionarios.Where(w => idsFuncsSel.Contains(w.IdFuncionario)).ToList().ForEach(f => { f.Acao = Acao.Alterar; });
                //Retiro dos selecionados os que já existiam
                idsFuncsSel.RemoveAll(x => obj.FechamentoPontoFuncionarios.Select(s => s.IdFuncionario).Contains(x));
            }

            return acao;
        }

        private List<(int idFuncionario, string userEpays, string passwordEpays)> VerificaConfEmpresaEpays(List<int> idsFuncsSel)
        {
            var lsFuncs = GetIdsEnabledEpays(idsFuncsSel);
            if (lsFuncs.Any())
            {
                var funcionarioBll = new BLL.Funcionario(_usr.ConnectionString, _usr);
                var res = funcionarioBll.GetEmpresaPeriodoFechamentoPonto(lsFuncs.Select(f => f.idFuncionario).ToArray());

                if (res.Rows.Count == 0)
                    throw new Exception("Para Integração com Epays deverá ter configuração de Periodo de fechamento!");
            }

            return lsFuncs;
        }

        private void ExportToEpays(FechamentoPonto obj, List<(int idFuncionario, string userEpays, string passwordEpays)> lsFuncs)
        {
            if (lsFuncs.Count > 0)
            {
                ExportacaoFechamentoPontoModel imp = new ExportacaoFechamentoPontoModel()
                {
                    IdSelecionados = string.Join(",", lsFuncs.Select(f => f.idFuncionario)),
                    LstFuncs = lsFuncs,
                    IdFechamentoPonto = obj.Id,
                    TipoArquivo = "PDF"
                };
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireIntegrationEpays hie = new HangfireIntegrationEpays(UserPW.DataBase);
                hie.RelatorioExportacaoPontoFechamento(obj.Id, imp);
            }
        }

        private List<(int idFuncionario, string userEpays, string passwordEpays)> GetIdsEnabledEpays(List<int> idsFuncionariosSelecionados)
        {
            if (idsFuncionariosSelecionados.Count == 0)
                return new List<(int idFuncionario, string userEpays, string passwordEpays)>();

            BLL.Empresa empresa = new BLL.Empresa(_usr.ConnectionString, _usr);
            var empFuncs = empresa.GetCnpjsByFuncIds(idsFuncionariosSelecionados.ToArray());
            var cnpjs = empFuncs.Select(e => e.cnpj).Distinct().ToList();

            var lstCnpjs = cnpjs.Select(c =>
            {
                if (HasConfigEPays(c, out var userEpays, out var passwordEpays))
                    return new { cnpj = c, userEpays, passwordEpays };

                return null;
            })
            .Where(c => c != null)
            .ToDictionary(c => c.cnpj, c => c);

            return empFuncs
                    .Where(e => lstCnpjs.ContainsKey(e.cnpj))
                    .Select(e => (e.idFuncionario, lstCnpjs[e.cnpj].userEpays, lstCnpjs[e.cnpj].passwordEpays))
                    .ToList();
        }

        private string GetDataBaseName()
        {
            return new Regex(@"(Catalog=[A-Z]*_[A-Z]*)").Match(_usr.ConnectionString).Value.Replace("Catalog=", "");
        }

        private bool HasConfigEPays(string cnpj, out string userEpays, out string passwordEpays)
        {
            var ePaysConfig = new EPaysConfig();
            var result = ePaysConfig.GetToken(GetDataBaseName(), cnpj);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                userEpays = result.Data.UserEPays;
                passwordEpays = result.Data.PasswordEPays;

                return !string.IsNullOrEmpty(result.Data.UserEPays)
                        && !string.IsNullOrEmpty(result.Data.PasswordEPays)
                            && !string.IsNullOrEmpty(result.Data.TokenPontofopag)
                                && result.Data.EnableEPays;
            }

            userEpays = null;
            passwordEpays = null;

            return false;
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            FechamentoPonto fp = new FechamentoPonto();

            if (id == 0)
            {
                fp.Codigo = bllFechamentoPonto.MaxCodigo();
                fp.DataFechamento = DateTime.Now;
            }
            else
            {
                fp = bllFechamentoPonto.LoadObject(id);
                BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);
                fp.FechamentoPontoFuncionarios = bllFechamentoPontoFuncionario.GetListWhere("and idfechamentoponto = " + fp.Id.ToString());
            }


            pxyRelPontoWeb RelPadrao = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(_usr.ConnectionString)).GetListagemFuncionariosRel(_usr);

            RelPadrao.idSelecionados = "";
            if (fp.FechamentoPontoFuncionarios != null && fp.FechamentoPontoFuncionarios.Count() > 0)
            {
                RelPadrao.idSelecionados = String.Join(",", fp.FechamentoPontoFuncionarios.Select(x => x.IdFuncionario).ToArray());
            }

            RelPadrao.UtilizaControleContrato = _usr.UtilizaControleContratos;
            RelPadrao.InicioPeriodo = DateTime.Now;
            RelPadrao.FimPeriodo = DateTime.Now;
            fp.PxyRelPontoWeb = RelPadrao;
            return View("Cadastrar", fp);
        }

        [HttpPost]
        public JsonResult ValidaFechamento(FechamentoPonto obj, string idsSelecionados = null)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFunc = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);

            List<int> idsFuncsSel = idsSelecionados?.Split(',').ToList().Select(s => int.Parse(s)).ToList();
            Acao acao = GetChanges(obj, bllFechamentoPonto, bllFechamentoPontoFunc, idsFuncsSel);

            var hasChanges = obj.FechamentoPontoFuncionarios.Where(f => f.Acao == Acao.Excluir).Select(f => f.IdFuncionario);
            if (hasChanges.Any())
            {
                var lstDocAssinado = bllFechamentoPontoFunc.GetListWhere($"and idFechamentoPonto={obj.Id} and bAssinado=1");
                var assFunc = lstDocAssinado.Where(d => hasChanges.Contains(d.IdFuncionario));

                if (assFunc.Any())
                    return Json(new
                    {
                        isValid = false,
                        title = "Deseja realmente continuar?",
                        message = $"Foram encontrado(s) {assFunc.Count()} documento(s) assinado(s), esta operação excluirá as assinaturas."
                    }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isValid = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ValidaJob(int id)
        {
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(_usr.ConnectionString, _usr);
            var idJob = bllFechamentoPonto.GetIdJob(id);
            if (!string.IsNullOrEmpty(idJob))
            {
                var job = JobControlManager.GetJobControl(idJob);
                if (job.StatusNovo == EnqueuedState.StateName
                        || job.StatusNovo == ProcessingState.StateName)
                    return Json(new { isValid = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { isValid = true }, JsonRequestBehavior.AllowGet);
        }

        protected override void ValidarForm(FechamentoPonto obj)
        {
            throw new NotImplementedException();
        }
    }
}