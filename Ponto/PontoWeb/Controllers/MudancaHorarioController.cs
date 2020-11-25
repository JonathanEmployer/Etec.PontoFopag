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
    public class MudancaHorarioController : Controller
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        [PermissoesFiltro(Roles = "MudancaHorario")]
        public ActionResult GridPorFuncionario(int id)
        {
            return View(new Modelo.MudancaHorario() { Idfuncionario = id});
        }

        [PermissoesFiltro(Roles = "MudancaHorario")]
        public ActionResult GridPorMarcacao(int id)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            var mar = bllMarcacao.LoadObject(id); 

            return View("GridPorFuncionario", new Modelo.MudancaHorario() { Idfuncionario = mar.Idfuncionario });
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                BLL.MudancaHorario bllMudancaHorario = new BLL.MudancaHorario(_usr.ConnectionString, _usr);
                List<Modelo.MudancaHorario> dados = bllMudancaHorario.GetAllFuncionarioList(id);
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

        [PermissoesFiltro(Roles = "MudancaHorarioConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "MudancaHorarioCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "MudancaHorarioCadastrar")]
        public ActionResult CadastrarManutMarcacao(int Id)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            Marcacao marcacao = bllMarcacao.LoadObject(Id);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Funcionario func = bllFuncionario.LoadObject(marcacao.Idfuncionario);
            BLL.MudancaHorario bllMudancaHorario = new BLL.MudancaHorario(_usr.ConnectionString, _usr);
            MudancaHorario mudHorario = new MudancaHorario();
            mudHorario.Codigo = bllMudancaHorario.MaxCodigo();
            mudHorario.Tipohorario = 1;
            mudHorario.Tipo = 0;
            mudHorario.NomeFuncionario = func.Codigo + " | " + func.Nome;
            mudHorario.Data = marcacao.Data;

            ViewBag.ManutencaoMarcacao = 1;
            ViewBag.UtilizaControleContratos = _usr.UtilizaControleContratos;
            return View(mudHorario);
        }

        [PermissoesFiltro(Roles = "MudancaHorarioAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "MudancaHorarioCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(MudancaHorario obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "MudancaHorarioCadastrar")]
        [HttpPost]
        public ActionResult CadastrarManutMarcacao(MudancaHorario obj)
        {
            if (SalvarDados(obj, out Modelo.Proxy.PxyJobReturn retCalculo))
            {
                return Json(new { success = true, job = retCalculo }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(obj);
            }
        }

        [PermissoesFiltro(Roles = "MudancaHorarioAlterar")]
        [HttpPost]
        public ActionResult Alterar(MudancaHorario obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "MudancaHorarioExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            BLL.MudancaHorario bllMudancaHorario = new BLL.MudancaHorario(_usr.ConnectionString, _usr);
            MudancaHorario mudHorario = bllMudancaHorario.LoadObject(id);
            string erro = bllMudancaHorario.ValidaFechamentoPonto(mudHorario.Tipo, mudHorario.IdFuncao, mudHorario.IdEmpresa, mudHorario.IdDepartamento, mudHorario.Idfuncionario, mudHorario.Data);
            if (!String.IsNullOrEmpty(erro))
            {
                return new JsonResult { Data = new { success = false, Erro = erro }};
            }

            try
            {
                if (mudHorario.IdLancamentoLoteFuncionario != null && mudHorario.IdLancamentoLoteFuncionario.GetValueOrDefault() > 0)
                {
                    BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(_usr.ConnectionString, _usr);
                    BLL.LancamentoLoteFuncionario bllLancamentoLoteFuncionario = new BLL.LancamentoLoteFuncionario(_usr.ConnectionString, _usr);
                    LancamentoLoteFuncionario llf = bllLancamentoLoteFuncionario.LoadObject(mudHorario.IdLancamentoLoteFuncionario.GetValueOrDefault());
                    llf.Acao = Acao.Excluir;
                    LancamentoLote ll = bllLancamentoLote.LoadObject(llf.IdLancamentoLote);
                    ll.LancamentoLoteFuncionarios = new List<LancamentoLoteFuncionario>();
                    ll.LancamentoLoteFuncionarios.Add(llf);
                    Dictionary<string, string> erros = bllLancamentoLote.Salvar(Acao.Alterar, ll);
                    if (erros.Count > 0)
                    {
                        erro = string.Join(" <br/> ", erros.Select(x => x.Key + " <br/> " + x.Value).ToArray());
                        return new JsonResult { Data = new { success = false, Erro = erro } };
                    }
                    else
                    {
                        if (ll.LancamentoLoteFuncionarios.Where(x => x.Efetivado == false).Count() > 0)
                        {
                            return new JsonResult { Data = new { success = false, Erro = ll.LancamentoLoteFuncionarios.Where(x => x.Efetivado == false).FirstOrDefault().DescricaoErro } };
                        }
                        else
                        {
                            List<int> FuncRecalc = BLL.LancamentoLote.IdsFuncionariosRecalcularLote(ll);
                            DateTime dataIni = ((Modelo.LancamentoLote)ll).DataLancamento > ((Modelo.LancamentoLote)ll).DataLancamentoAnt ? ((Modelo.LancamentoLote)ll).DataLancamento : ((Modelo.LancamentoLote)ll).DataLancamentoAnt;
                            if (FuncRecalc.Count() > 0)
                            {
                                HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/Funcionario/Grid");
                                BLL.Horario bllHorario = new BLL.Horario(_usr.ConnectionString, _usr);
                                Modelo.Horario horario = bllHorario.LoadObject(mudHorario.Idhorario);
                                string parametrosExibicao = String.Format("Exclusão da mudança para o horário {0} | {1} no dia {2} removido", horario.Codigo, horario.Descricao, mudHorario.Data.GetValueOrDefault().ToShortDateString());
                                if (FuncRecalc.Count == 1)
                                {
                                    BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
                                    Modelo.Funcionario func = bllFuncionario.LoadObject(FuncRecalc.FirstOrDefault());
                                    parametrosExibicao += String.Format(" do funcionário {0} | {1}", func.Dscodigo, func.Nome);
                                }
                                else
                                {
                                    parametrosExibicao += String.Format(" de {0} funcionario(s)", FuncRecalc.Count());
                                }

                                Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Calculo de exclusão de mudança de horário", parametrosExibicao, FuncRecalc, dataIni, DateTime.Now.AddMonths(1));
                                return new JsonResult { Data = new { success = true, job = ret } };
                            }
                            return new JsonResult { Data = new { success = true } };
                        }
                    }
                }
                else
                {
                    bool excluiu = false;
                    excluiu = bllMudancaHorario.ExcluiMudancaWeb(mudHorario);
                    if (!excluiu)
                    {
                        erro = "Só é permitido excluir a última mudança de horário do funcionário.";
                        return new JsonResult { Data = new { success = false, Erro = erro } };
                    }
                   
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/Funcionario/Grid");
                    string parametrosExibicao =  String.Format("Recalculo da mudança para o horário {0} no dia {1} removido do funcionario {2}", mudHorario.descricaohorario, mudHorario.Data.GetValueOrDefault().ToShortDateString(), mudHorario.NomeFuncionario);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaExclusaoMudHorario("Recalculo de exclusão de mudança de horário", parametrosExibicao, mudHorario);
                    return new JsonResult { Data = new { success = true, job = ret } };

                }
            }
            catch (Exception ex)
            {
                Guid guid = BLL.cwkFuncoes.LogarErro(ex);
                return new JsonResult { Data = new { success = false, Erro = "Não foi possível executar a operação, código do erro: "+ guid.ToString() } };
            }
        }

        private ActionResult Salvar(MudancaHorario obj)
        {
            if (SalvarDados(obj, out Modelo.Proxy.PxyJobReturn retCalculo))
            {
                return RedirectToAction("Cadastrar", "MudancaHorario");
            }
            else
            {
                return View(obj);
            }
        }

        private bool SalvarDados(MudancaHorario obj, out Modelo.Proxy.PxyJobReturn retCalculo)
        {
            retCalculo = new Modelo.Proxy.PxyJobReturn();
            bool salvou = false;
            BLL.MudancaHorario bllMudancaHorario = new BLL.MudancaHorario(_usr.ConnectionString, _usr);
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    if (obj.Id == 0)
                        obj.Acao = Acao.Incluir;
                    else
                        obj.Acao = Acao.Alterar;


                    bllMudancaHorario.MudarHorarioWeb(obj.Tipo, obj.IdFuncao, obj.IdEmpresa, obj.IdDepartamento, obj.Idfuncionario, obj.Tipohorario, obj.Idhorario, obj.Data, obj.CicloSequenciaIndice);
                    retCalculo = Recalcular(obj);
                    salvou = true;
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", "<div class=\"comment alert alert-danger\">" + ex.Message + "</div>");
                }
            }
            ViewBag.UtilizaControleContratos = _usr.UtilizaControleContratos;
            return salvou;
        }

        private Modelo.Proxy.PxyJobReturn Recalcular(MudancaHorario obj)
        {
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/MudancaHorario/Cadastrar");
            string nomeAfastamento = "";
            switch (obj.Tipo)
            {
                case 0:
                    nomeAfastamento = "Funcionário: " + obj.NomeFuncionario;
                    break;
                case 1:
                    nomeAfastamento = "Departamento: " + obj.NomeDepartamento;
                    break;
                case 2:
                    nomeAfastamento = "Empresa: " + obj.NomeEmpresa;
                    break;
                default:
                    nomeAfastamento = "tipo: desconheciado";
                    break;
            }
            string parametrosExibicao = String.Format("Mudança de Horário {0}, data: {1} ", nomeAfastamento, obj.DataStr);
            string acao = obj.AcaoDescricao;
            Modelo.Proxy.PxyJobReturn ret = hfm.CalculaMudancaHorario(String.Format("Recalculo de marcações por {0} de mudança de horário", acao), parametrosExibicao, obj);
            return ret;
        }

        private ActionResult GetPagina(int id)
        {
            BLL.MudancaHorario bllMudancaHorario = new BLL.MudancaHorario(_usr.ConnectionString, _usr);
            MudancaHorario mudHorario = new MudancaHorario();
            mudHorario = bllMudancaHorario.LoadObject(id);
            if (id == 0)
            {
                mudHorario.Codigo = bllMudancaHorario.MaxCodigo();
                mudHorario.Tipohorario = 1;
            }

            ViewBag.UtilizaControleContratos = _usr.UtilizaControleContratos;
            return View("Cadastrar", mudHorario);
        }

        private void ValidarForm(MudancaHorario obj)
        {
            switch (obj.Tipo)
            {
                case 0:
                    VerificaFuncionario(obj);
                    break;
                case 1:
                    VerificaEmpresa(obj);
                    ValidaDepartamento(obj);
                    break;
                case 2:
                    VerificaEmpresa(obj);
                    break;
                case 3:
                    VerificaFuncao(obj);
                    break;
            }
            if (obj.Tipohorario == 1)
            {
                if (String.IsNullOrEmpty(obj.HorarioNormal))
                {
                    ModelState["HorarioNormal"].Errors.Add("O campo Horário Normal é Obrigatório.");
                }
                else
                {
                    ValidaHorarioNormal(obj);
                }
            }

            if (obj.Tipohorario == 2)
            {
                if (String.IsNullOrEmpty(obj.HorarioFlexivel))
                {
                    ModelState["HorarioFlexivel"].Errors.Add("O campo Horário Flexivel é Obrigatório.");
                }
                else
                {
                    ValidaHorarioFlexivel(obj);
                }
            }

            if (obj.Tipohorario == 3)
            {
                if (String.IsNullOrEmpty(obj.HorarioDinamico))
                {
                    ModelState["HorarioFlexivel"].Errors.Add("O campo Horário Flexivel é Obrigatório.");
                }
                else
                {
                    ValidaHorarioDinamico(obj);
                }
            }

            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(obj.Idfuncionario);
            if (objFuncionario.Idhorario == obj.Idhorario)
            {
                string erroFuncHor = "O funcionário já está utilizando o horário selecionado.";
                if (obj.Tipohorario == 1)
                {
                    ModelState["HorarioNormal"].Errors.Add(erroFuncHor);
                }
                else
                {
                    ModelState["HorarioFlexivel"].Errors.Add(erroFuncHor);
                }
            }
        }

        private void ValidaDepartamento(MudancaHorario mudHorario)
        {
            int idDepartamento = DepartamentoController.BuscaIdDepartamento(mudHorario.NomeDepartamento);
            if (idDepartamento > 0)
            {
                BLL.Departamento bllDepartamento = new BLL.Departamento(_usr.ConnectionString, _usr);
                Departamento dep = bllDepartamento.LoadObject(idDepartamento);
                if (dep.IdEmpresa == mudHorario.IdEmpresa)
                { mudHorario.IdDepartamento = idDepartamento; }
                else
                { ModelState["NomeDepartamento"].Errors.Add("Departamento " + mudHorario.NomeDepartamento + " não pertence a empresa selecionada!"); }
            }
            else
            {
                ModelState["NomeDepartamento"].Errors.Add("Departamento " + mudHorario.NomeDepartamento + " não cadastrado!");
            }
        }

        private void VerificaEmpresa(MudancaHorario mudHorario)
        {
            int idEmpresa = EmpresaController.BuscaIdEmpresa(mudHorario.NomeEmpresa);
            if (idEmpresa > 0)
            {
                mudHorario.IdEmpresa = idEmpresa;
            }
            else
            {
                ModelState["NomeEmpresa"].Errors.Add("Empresa " + mudHorario.NomeEmpresa + " não cadastrada!");
            }
        }

        private void VerificaFuncao(MudancaHorario mudHorario)
        {
            int idFuncao = FuncaoController.BuscaIdFuncao(mudHorario.NomeFuncao);
            if (idFuncao > 0)
            {
                mudHorario.IdFuncao = idFuncao;
            }
            else
            {
                ModelState["NomeFuncao"].Errors.Add("Função " + mudHorario.NomeFuncao + " não cadastrada!");
            }
        }

        private void VerificaFuncionario(MudancaHorario mudHorario)
        {
            int idFunc = FuncionarioController.BuscaIdFuncionario(mudHorario.NomeFuncionario);
            if (idFunc > 0)
            {
                mudHorario.Idfuncionario = idFunc;
            }
            else
            {
                ModelState["NomeFuncionario"].Errors.Add("Funcionário " + mudHorario.NomeFuncionario + " não cadastrado!");
            }
        }

        private void ValidaHorarioNormal(MudancaHorario mudHorario)
        {
            int idHorario = HorarioController.BuscaIdHorario(mudHorario.HorarioNormal);
            if (idHorario > 0)
            { mudHorario.Idhorario = idHorario; }
            else
            { ModelState["HorarioNormal"].Errors.Add("Horário Normal " + mudHorario.HorarioNormal + " não cadastrado!"); }
        }

        private void ValidaHorarioFlexivel(MudancaHorario mudHorario)
        {
            int idHorario = HorarioController.BuscaIdHorario(mudHorario.HorarioFlexivel);
            if (idHorario > 0)
            { mudHorario.Idhorario = idHorario; }
            else
            { ModelState["HorarioFlexivel"].Errors.Add("Horário Flexível" + mudHorario.HorarioFlexivel + " não cadastrado!"); }
        }

        private void ValidaHorarioDinamico(MudancaHorario mudHorario)
        {
            int idHorario = HorarioDinamicoController.BuscaIdHorario(mudHorario.HorarioDinamico);
            if (idHorario > 0)
            { mudHorario.Idhorario = idHorario; }
            else
            { ModelState["HorarioDinamico"].Errors.Add("Horário dinâmico" + mudHorario.HorarioFlexivel + " não cadastrado!"); }
        }
    }
}