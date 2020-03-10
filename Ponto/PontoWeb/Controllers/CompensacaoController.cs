using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class CompensacaoController : IControllerPontoWeb<Compensacao>
    {
        [PermissoesFiltro(Roles = "Compensacao")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Compensacao());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Compensacao bllCompensacao = new BLL.Compensacao(usr.ConnectionString, usr);
                List<Modelo.Compensacao> dados = bllCompensacao.GetAllList();
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

        [PermissoesFiltro(Roles = "CompensacaoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "CompensacaoCadastrar")]
        public override ActionResult Cadastrar()
        {
            int id = 0;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "CompensacaoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "CompensacaoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Compensacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "CompensacaoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Compensacao obj)
        {
            return Salvar(obj);
        }
        [PermissoesFiltro(Roles = "CompensacaoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(UsuPW.ConnectionString, UsuPW);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(UsuPW.ConnectionString, UsuPW);
            Compensacao compensacao = bllCompensacao.LoadObject(id);
            try
            {
                compensacao.NaoRecalcular = true;
                Dictionary<string, string> erros = new Dictionary<string, string>();

                if (bllMarcacao.QuantidadeCompensada(compensacao.Id) > 0)
                {
                    string erro = "A Compensação deve ser desfeita antes de ser alterada ou excluída";
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = false,
                            Erro = erro
                        }
                    };
                }

                erros = bllCompensacao.Salvar(Acao.Excluir, compensacao);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = false,
                            Erro = erro
                        }
                    };
                }
                
                Modelo.Proxy.PxyJobReturn ret = Recalcular(UsuPW, Acao.Excluir, compensacao);
                return new JsonResult
                {
                    Data = new
                    {
                        success = true,
                        job = ret
                    }
                };
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        Erro = ex.Message
                    }
                };
            }
        }

        protected override ActionResult Salvar(Compensacao obj)
        {
            obj.NaoValidaCodigo = true;
            obj.NaoRecalcular = true;
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            ViewBag.UtilizaControleContratos = UsuPW.UtilizaControleContratos;
            if (UsuPW.UtilizaControleContratos)
            {
                obj.Tipo = 2;
            }
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(UsuPW.ConnectionString, UsuPW);
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    if (obj.DiasC == null)
                    {
                        obj.DiasC = new List<DiasCompensacao>();
                    }
                    else
                    {
                        foreach (var item in obj.DiasC)
                        {
                            item.NaoValidaCodigo = true;                            
                            item.Compensacao = obj;
                            if (item.Delete)
                            {
                                item.Acao = Acao.Excluir;
                            }
                            else
                            {
                                if (item.Id > 0)
                                {
                                    item.Acao = Acao.Alterar;
                                }
                                else
                                {
                                    item.Acao = Acao.Incluir;
                                }
                            }
                        }
                    }
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>(); 
                    erros = bllCompensacao.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", "<div class=\"comment alert alert-danger\">" + erro + "</div>");
                    }
                    else
                    {
                        Recalcular(UsuPW, acao, obj);
                        return RedirectToAction("Grid", "Compensacao");
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
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(UsuPW.ConnectionString, UsuPW);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(UsuPW.ConnectionString, UsuPW);
            Compensacao compensacao = new Compensacao();
            compensacao = bllCompensacao.LoadObject(id);
            if (id == 0)
            {
                compensacao.Codigo = bllCompensacao.MaxCodigo();
            }
            else
            {
                compensacao.Periodofinal_Ant = compensacao.Periodofinal;
                compensacao.Periodoinicial_Ant = compensacao.Periodoinicial;
                compensacao.Tipo_Ant = compensacao.Tipo;
                compensacao.Identificacao_Ant = compensacao.Identificacao;
                if (bllMarcacao.QuantidadeCompensada(compensacao.Id) > 0)
                {
                    string erro = "A Compensação deve ser desfeita antes de ser alterada";
                    ModelState.AddModelError("CustomError", erro);
                    ViewBag.DesabilitaCampos = true;
                }

                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(UsuPW.ConnectionString, UsuPW);
                    DateTime menorData = DateTime.Now;
                    if (compensacao.DiasC != null && compensacao.DiasC.Count() > 0)
                    {
                        menorData = compensacao.DiasC.Min(x => x.Datacompensada.GetValueOrDefault());
                    }
                    if (compensacao.Periodoinicial.GetValueOrDefault() != default(DateTime) && compensacao.Periodoinicial.GetValueOrDefault() < menorData)
                        menorData = compensacao.Periodoinicial.GetValueOrDefault();
                    if (compensacao.Diacompensarfinal.GetValueOrDefault() != default(DateTime) && compensacao.Diacompensarfinal.GetValueOrDefault() < menorData)
                        menorData = compensacao.Diacompensarfinal.GetValueOrDefault();
                    string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(compensacao.Tipo, new List<int>() { compensacao.Identificacao }, menorData);
                    if (!String.IsNullOrEmpty(mensagemFechamento))
                    {
                        ViewBag.Consultar = 1;
                        @ViewBag.MensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento de ponto vinculado. Detalhes: <br/>" + mensagemFechamento;
                    }
                }
                #endregion
            }

            ViewBag.UtilizaControleContratos = UsuPW.UtilizaControleContratos;
            return View("Cadastrar", compensacao);
        }

        public Modelo.Proxy.PxyJobReturn Recalcular(UsuarioPontoWeb usuario, Acao acao, Compensacao compensacao)
        {
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuario.DataBase, "", "", "/Compensacao/Grid");
            string parametrosExibicao = String.Format("Compensação código: {0}, Tipo: {1} - {2}, Período: {3} a {4}, período a ser compensado: {5} a {6}", compensacao.Codigo, compensacao.Tipo, compensacao.Nome, compensacao.DataInicialStr, compensacao.DataFinalStr, compensacao.DiaCompensarInicialStr, compensacao.DiaCompensarFinalStr);
            Modelo.Proxy.PxyJobReturn ret = hfm.AtualizaMarcacoesCompensacao("Recalculo de marcações por Compensacao", parametrosExibicao, acao, compensacao);
            return ret;
        }

        protected override void ValidarForm(Compensacao obj)
        {
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(UsuPW.ConnectionString, UsuPW);
            BLL.Empresa bllEmp = new BLL.Empresa(UsuPW.ConnectionString, UsuPW);
            BLL.Departamento blldep = new BLL.Departamento(UsuPW.ConnectionString, UsuPW);
            BLL.Funcionario bllfuncionario = new BLL.Funcionario(UsuPW.ConnectionString, UsuPW);
            BLL.Funcao bllfuncao = new BLL.Funcao(UsuPW.ConnectionString, UsuPW);

            if (bllMarcacao.QuantidadeCompensada(obj.Id) > 0)
            {
                ModelState.AddModelError("CustomError", "A Compensação deve ser desfeita antes de ser alterada ou excluída");
            }
            #region Validação de Dias e quantidades de hora

            if (((!obj.Segunda) && (!obj.Terca) && (!obj.Quarta) && (!obj.Quinta) &&
                   (!obj.Sexta) && (!obj.Sabado) && (!obj.Domingo) && (!obj.Feriado)))
            {
                ModelState["Segunda"].Errors.Add("Você deve selecionar um dia da semana para ser compensado");
            }
            if (obj.Segunda && (obj.Totalhorassercompensadas_1 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_1)))
            {
                ModelState["Totalhorassercompensadas_1"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }
            if (obj.Terca && (obj.Totalhorassercompensadas_2 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_2)))
            {
                ModelState["Totalhorassercompensadas_2"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }
            if (obj.Quarta && (obj.Totalhorassercompensadas_3 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_3)))
            {
                ModelState["Totalhorassercompensadas_3"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }
            if (obj.Quinta && (obj.Totalhorassercompensadas_4 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_4)))
            {
                ModelState["Totalhorassercompensadas_4"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }
            if (obj.Sexta && (obj.Totalhorassercompensadas_5 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_5)))
            {
                ModelState["Totalhorassercompensadas_5"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }
            if (obj.Sabado && (obj.Totalhorassercompensadas_6 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_6)))
            {
                ModelState["Totalhorassercompensadas_6"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }
            if (obj.Domingo && (obj.Totalhorassercompensadas_7 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_7)))
            {
                ModelState["Totalhorassercompensadas_7"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }
            if (obj.Feriado && (obj.Totalhorassercompensadas_8 == "--:--" || String.IsNullOrEmpty(obj.Totalhorassercompensadas_8)))
            {
                ModelState["Totalhorassercompensadas_8"].Errors.Add("Você deve definir uma quantidade de horas para este dia");
            }

            #endregion

            #region Validação Tipo Compensação
            switch (obj.Tipo)
            {
                case 0:
                    if (String.IsNullOrEmpty(obj.Empresa))
                    {
                        ModelState["Empresa"].Errors.Add("Selecione uma empresa.");
                    }
                    else
                    {

                        Empresa e = new Empresa();
                        int idEmpresa;
                        string empresa = obj.Empresa.Split('|')[0].Trim();
                        if (int.TryParse(empresa, out idEmpresa))
                        {
                            e = bllEmp.LoadObjectByCodigo(idEmpresa);
                        }
                        if (e != null && e.Id > 0)
                        {
                            obj.Identificacao = e.Id;
                        }
                        else
                        {
                            ModelState["Empresa"].Errors.Add("Empresa " + empresa + " não cadastrada!");
                        }
                    }
                    break;
                case 1:
                    if (String.IsNullOrEmpty(obj.Departamento))
                    {
                        ModelState["Departamento"].Errors.Add("Selecione um departamento.");
                    }
                    else
                    {

                        Departamento d = new Departamento();
                        int idDepartamento;
                        string depto = obj.Departamento.Split('|')[0].Trim();
                        if (int.TryParse(depto, out idDepartamento))
                        {
                            d = blldep.LoadObjectByCodigo(idDepartamento);
                        }
                        if (d != null && d.Id > 0)
                        {
                            obj.Identificacao = d.Id;
                        }
                        else
                        {
                            ModelState["Departamento"].Errors.Add("Departamento " + depto + " não cadastrado!");
                        }
                    }
                    break;
                case 2:
                    if (String.IsNullOrEmpty(obj.Funcionario))
                    {
                        ModelState["Funcionario"].Errors.Add("Selecione um funcionário.");
                    }
                    else
                    {
                        int idFuncionario = 0;
                        string func = obj.Funcionario.Split('|')[0].Trim();
                        idFuncionario = bllfuncionario.GetIdDsCodigo(func);
                        if (idFuncionario > 0)
                        {
                            obj.Identificacao = idFuncionario;
                        }
                        else
                        {
                            ModelState["Funcionario"].Errors.Add("Funcionário " + obj.Funcionario + " não cadastrado!");
                        }
                    }
                    break;
                case 3:
                    if (String.IsNullOrEmpty(obj.Funcao))
                    {
                        ModelState["Funcao"].Errors.Add("Selecione uma função.");
                    }
                    else
                    {
                        Funcao d = new Funcao();
                        int idFuncao;
                        string func = obj.Funcao.Split('|')[0].Trim();
                        if (int.TryParse(func, out idFuncao))
                        {
                            d = bllfuncao.LoadObjectByCodigo(idFuncao);
                        }
                        if (d != null && d.Id > 0)
                        {
                            obj.Identificacao = d.Id;
                        }
                        else
                        {
                            ModelState["Funcao"].Errors.Add("Função " + func + " não cadastrada!");
                        }
                    }
                    break;
                default:
                    break;
            }
            #endregion

            #region Validação Datas e Periodos

            if (obj.Periodoinicial == null)
            {
                ModelState["Periodoinicial"].Errors.Add("Campo Obrigatório.");
            }

            if (obj.Periodofinal == null)
            {
                ModelState["Periodofinal"].Errors.Add("Campo Obrigatório.");
            }
            else if (obj.Periodofinal < obj.Periodoinicial)
            {
                ModelState["Periodofinal"].Errors.Add("A data final da compensação deve ser maior do que a data inicial.");
            }



            if (!ModelState.ContainsKey("DiasC"))
            {
                ModelState.Add("DiasC", new ModelState());
            }
            if (obj.DiasC != null)
            {
                obj.DiasC = obj.DiasC.Where(w => !(w.Id == 0 && w.Delete)).ToList();
                if (obj.DiasC.Count == 0)
                {
                    if (obj.Diacompensarinicial == null && obj.Diacompensarfinal == null)
                    {
                        ModelState["DiasC"].Errors.Add("Obrigatório informar ao menos uma data ou período para compensação");
                    }
                    else
                    {
                        if (obj.Diacompensarinicial == null)
                        {
                            ModelState["Diacompensarinicial"].Errors.Add("Campo Obrigatório");
                        }

                        if (obj.Diacompensarfinal == null)
                        {
                            ModelState["Diacompensarfinal"].Errors.Add("Campo Obrigatório");
                        }

                        if (obj.Diacompensarinicial != null && obj.Diacompensarfinal != null)
                        {
                            if (obj.Diacompensarfinal > obj.Diacompensarinicial)
                            {
                                ModelState["Diacompensarinicial"].Errors.Add("A data de Início deve ser maior que a data de Fim");
                            }
                        }
                    }
                }
                else
                {
                    IList<DiasCompensacao> excluidos = new List<DiasCompensacao>();
                    IList<DiasCompensacao> persistidos = new List<DiasCompensacao>();
                    excluidos = obj.DiasC.Where(w => w.Delete && w.Id != 0).ToList();
                    persistidos = obj.DiasC.Where(w => w.Id != 0).ToList();
                    if (excluidos != null && persistidos != null)
                    {
                        if ((excluidos.Count == persistidos.Count && persistidos.Count > 0
                            && ((!obj.Diacompensarinicial.HasValue) && (!obj.Diacompensarinicial.HasValue)))
                            ^
                            (obj.DiasC.Count == 0 && ((!obj.Diacompensarinicial.HasValue) && (!obj.Diacompensarinicial.HasValue))))
                        {
                            ModelState["DiasC"].Errors.Add("Obrigatório informar ao menos uma data ou período para compensação");
                        }
                    }
                }
            }
            else
            {
                if (obj.Diacompensarinicial == null && obj.Diacompensarfinal == null)
                {
                    ModelState["DiasC"].Errors.Add("Obrigatório informar ao menos uma data ou período para compensação");
                }
                else
                {
                    if (obj.Diacompensarinicial == null)
                    {
                        ModelState["Diacompensarinicial"].Errors.Add("Campo Obrigatório");
                    }

                    if (obj.Diacompensarfinal == null)
                    {
                        ModelState["Diacompensarfinal"].Errors.Add("Campo Obrigatório");
                    }
                }
            }

            if (obj.Diacompensarfinal.HasValue && obj.Diacompensarinicial.HasValue)
            {
                if ((obj.Diacompensarfinal.Value < obj.Diacompensarinicial.Value))
                {
                    ModelState["Diacompensarinicial"].Errors.Add("A data final do período a ser compensado deve ser maior do que a data inicial.");
                    ModelState["Diacompensarfinal"].Errors.Add("A data final do período a ser compensado deve ser maior do que a data inicial.");
                }
            }

            #endregion

            if (obj.Tipo == -1)
            {
                ModelState["Tipo"].Errors.Add("Campo obrigatório.");
            }

            if (obj.Identificacao == 0)
            {
                ModelState["Identificacao"].Errors.Add("Campo obrigatório.");
            }
        }

        [PermissoesFiltro(Roles = "CompensacaoAlterar")]
        public ActionResult FecharComp(int id)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            UsuarioPontoWeb usuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(usuPW.ConnectionString, usuPW);
            Compensacao comp = bllCompensacao.LoadObject(id);
            if (comp.Id > 0)
            {
                bllCompensacao.ValidaFechamentoPonto(comp, ref ret);
                if (ret.Count > 0)
                {
                    string erro = string.Join("<br/>", ret.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuPW.DataBase);
                    string parametrosExibicao = String.Format("Compensação código: {0}, Tipo: {1} - {2}, Período: {3} a {4}, período a ser compensado: {5} a {6}", comp.Codigo, comp.Tipo, comp.Nome, comp.DataInicialStr, comp.DataFinalStr, comp.DiaCompensarInicialStr, comp.DiaCompensarFinalStr);
                    Modelo.Proxy.PxyJobReturn retJob = hfm.FechaCompensacao("Recalculo de marcações por compensação fechada", parametrosExibicao, comp.Id);
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = true,
                            job = retJob
                        }
                    };
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        errors = new Error("CustomError", "Compensação não encontrada!")
                    }
                };
            }
        }

        [PermissoesFiltro(Roles = "CompensacaoAlterar")]
        public ActionResult DesfazComp(int id)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            UsuarioPontoWeb usuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(usuPW.ConnectionString, usuPW);
            Compensacao comp = bllCompensacao.LoadObject(id);
            if (comp.Id > 0)
            {
                bllCompensacao.ValidaFechamentoPonto(comp, ref ret);
                if (ret.Count > 0)
                {
                    string erro = string.Join("<br/>", ret.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = false,
                            errors = new Error("CustomError", erro)
                        }
                    };
                }
                else
                {
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuPW.DataBase);
                    string parametrosExibicao = String.Format("Compensação código: {0}, Tipo: {1} - {2}, Período: {3} a {4}, período a ser compensado: {5} a {6}", comp.Codigo, comp.Tipo, comp.Nome, comp.DataInicialStr, comp.DataFinalStr, comp.DiaCompensarInicialStr, comp.DiaCompensarFinalStr);
                    Modelo.Proxy.PxyJobReturn retJob = hfm.DesfazCompensacao("Recalculo de marcações por compensação desfeita", parametrosExibicao, comp.Id);
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = true,
                            job = retJob
                        }
                    };
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        errors = new Error("CustomError", "Compensação não encontrada!")
                    }
                };
            }
        }
    }
}