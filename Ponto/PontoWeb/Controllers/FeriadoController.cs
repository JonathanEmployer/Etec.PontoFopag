using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class FeriadoController : IControllerPontoWeb<Feriado>
    {
        [PermissoesFiltro(Roles = "Feriado")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Feriado());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Feriado bllFeriado = new BLL.Feriado(usr.ConnectionString, usr);
                List<Modelo.Feriado> dados = bllFeriado.GetAllList();
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

        [PermissoesFiltro(Roles = "FeriadoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FeriadoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "FeriadoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FeriadoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Feriado obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FeriadoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Feriado obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FeriadoExcluir")]
        public override ActionResult Excluir(int id)
        {
            UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Feriado bllFeriado = new BLL.Feriado(userPW.ConnectionString, userPW);
            Feriado feriado = bllFeriado.LoadObject(id);
            feriado.NaoRecalcular = true;
            try
            {
                SelecaoFuncionarios(userPW.ConnectionString, userPW, feriado);
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllFeriado.Salvar(Acao.Excluir, feriado);
                if (erros.Count > 0)
                {
                    string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Recalcular(userPW, Acao.Excluir, feriado);
                    return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override ActionResult Salvar(Feriado obj)
        {
            UsuarioPontoWeb user = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Feriado bllFeriado = new BLL.Feriado(user.ConnectionString, user);
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    obj.NaoRecalcular = true;
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllFeriado.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                    }
                    else
                    {
                        Recalcular(user, acao, obj);
                        return RedirectToAction("Grid", "Feriado");
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
            Dictionary<string, string> erroCodigo = new Dictionary<string, string>();
            erroCodigo = erros.Where(x => x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);
            if (erroCodigo.Count > 0)
            {
                ModelState["Codigo"].Errors.Add(string.Join(";", erroCodigo.Select(x => x.Value).ToArray()));
            }

            Dictionary<string, string> erroDescricao = new Dictionary<string, string>();
            erroDescricao = erros.Where(x => x.Key.Equals("txtDescricao")).ToDictionary(x => x.Key, x => x.Value);
            if (erroDescricao.Count > 0)
            {
                ModelState["Descricao"].Errors.Add(string.Join(";", erroDescricao.Select(x => x.Value).ToArray()));
            }

            Dictionary<string, string> erroData = new Dictionary<string, string>();
            erroData = erros.Where(x => x.Key.Equals("txtData")).ToDictionary(x => x.Key, x => x.Value);
            if (erroData.Count > 0)
            {
                ModelState["Data"].Errors.Add(string.Join(";", erroData.Select(x => x.Value).ToArray()));
            }

            Dictionary<string, string> erroTipoFeriado = new Dictionary<string, string>();
            erroTipoFeriado = erros.Where(x => x.Key.Equals("rgTipoFeriado")).ToDictionary(x => x.Key, x => x.Value);
            if (erroTipoFeriado.Count > 0)
            {
                ModelState["TipoFeriado"].Errors.Add(string.Join(";", erroTipoFeriado.Select(x => x.Value).ToArray()));
            }

            erros = erros.Where(x => !x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtDescricao")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtData")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("rgTipoFeriado")).ToDictionary(x => x.Key, x => x.Value);
            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }

        protected override ActionResult GetPagina(int id)
        {
            UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Feriado bllFeriado = new BLL.Feriado(userPW.ConnectionString, userPW);
            Feriado feriado = new Feriado();
            BLL.Parametros bllparm = new BLL.Parametros(userPW.ConnectionString, userPW);
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            ViewBag.RetricaoDeAcesso = userPW.UtilizaControleContratos || userPW.UtilizaControleEmpresa || userPW.UtilizaControleSupervisor;
            feriado = bllFeriado.LoadObject(id);
            if (!feriado.Data_Ant.HasValue)
            {
                feriado.Data_Ant = feriado.Data;
            }
            feriado.ParcialAnt = feriado.Parcial;
            feriado.HoraInicioAnt = feriado.HoraInicio;
            feriado.HoraFimAnt = feriado.HoraFim;
            if (id == 0)
            {
                feriado.Codigo = bllFeriado.MaxCodigo();
            }
            else
            {
                SelecaoFuncionarios(userPW.ConnectionString, userPW, feriado);
                feriado.TipoFeriado_Ant = feriado.TipoFeriado;
                if (ViewBag.RetricaoDeAcesso && feriado.TipoFeriado != 3)
                {
                    ViewBag.RetricaoDeAcesso = false;
                    ViewBag.Consultar = 1;
                    ViewBag.MensagemRestricao = "Você não tem permissão para alterar esse tipo de lançamento de feriado, permitido apenas alterar feriados lançados por funcionário.";
                }
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(userPW.ConnectionString, userPW);
                    int tipoFechamento = 0;
                    List<int> idTipos = new List<int>();
                    switch (feriado.TipoFeriado)
                    {
                        case 1: // Empresa
                            tipoFechamento = 0;
                            idTipos.Add(feriado.IdEmpresa);
                            break;
                        case 2: // Departamento
                            tipoFechamento = 1;
                            idTipos.Add(feriado.IdDepartamento);
                            break;
                        case 3: // Funcionario
                            tipoFechamento = 2;
                            idTipos = feriado.IdsFeriadosFuncionariosSelecionados.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();
                            break;
                        default: // Todos
                            tipoFechamento = 4;
                            break;
                    }
                    string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(tipoFechamento, idTipos, feriado.Data.GetValueOrDefault());
                    if (!String.IsNullOrEmpty(mensagemFechamento))
                    {
                        ViewBag.Consultar = 1;
                        @ViewBag.MensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento de ponto vinculado. Detalhes: <br/>" + mensagemFechamento;
                    }
                }
                #endregion 
            }
            return View("Cadastrar",feriado);
        }

        private static void SelecaoFuncionarios(string conn, UsuarioPontoWeb userPW, Feriado feriado)
        {
            BLL.FeriadoFuncionario bllFeriadoFunc = new BLL.FeriadoFuncionario(conn, userPW);
            feriado.FeriadoFuncionarios = bllFeriadoFunc.GetListWhere(" and idFeriado = "+feriado.Id);
            if (feriado.FeriadoFuncionarios != null && feriado.FeriadoFuncionarios.Count() > 0)
            {
                feriado.IdsFeriadosFuncionariosSelecionados = String.Join(",", feriado.FeriadoFuncionarios.Select(x => x.IdFuncionario).ToArray());
                feriado.IdsFeriadosFuncionariosSelecionados_Ant = feriado.IdsFeriadosFuncionariosSelecionados;
            }
        }

        protected override void ValidarForm(Feriado obj)
        {
            Modelo.UsuarioPontoWeb usu = Usuario.GetUsuarioPontoWebLogadoCache();
            if (!String.IsNullOrEmpty(obj.Empresa))
            {
                BLL.Empresa bllEmp = new BLL.Empresa(usu.ConnectionString, usu);
                Empresa e = new Empresa();
                int idEmpresa;
                string empresa = obj.Empresa.Split('|')[0].Trim();
                if (int.TryParse(empresa, out idEmpresa))
                {
                    e = bllEmp.LoadObjectByCodigo(idEmpresa);
                }
                if (e != null && e.Id > 0)
                {
                    obj.IdEmpresa = e.Id;
                }
                else
                {
                    ModelState["Empresa"].Errors.Add("Empresa " + empresa + " não cadastrada!");
                }
            }

            if (!String.IsNullOrEmpty(obj.Departamento))
            {
                BLL.Departamento blldep = new BLL.Departamento(usu.ConnectionString, usu);
                Departamento d = new Departamento();
                int idDepartamento;
                string depto = obj.Departamento.Split('|')[0].Trim();
                if (int.TryParse(depto, out idDepartamento))
                {
                    d = blldep.LoadObjectByCodigo(idDepartamento);
                }
                if (d != null && d.Id > 0)
                {
                    obj.IdDepartamento = d.Id;
                }
                else
                {
                    ModelState["Departamento"].Errors.Add("Departamento " + depto + " não cadastrado!");
                }
                ValidaDepartamento(obj);
            }

            switch (obj.TipoFeriado)
            {
                case 1:
                    if (String.IsNullOrEmpty(obj.Empresa))
                    {
                        ModelState["Empresa"].Errors.Add("Selecione uma empresa.");
                    }
                    break;
                case 2:
                    if (String.IsNullOrEmpty(obj.Empresa))
                    {
                        ModelState["Empresa"].Errors.Add("Selecione uma empresa.");
                    }
                    if (String.IsNullOrEmpty(obj.Departamento))
                    {
                        ModelState["Departamento"].Errors.Add("Selecione um departamento.");
                    }
                    break;
                default:
                    break;
            }

            if (obj.TipoFeriado != 3)
            {
                obj.IdsFeriadosFuncionariosSelecionados = "";
            }
        }

        private void ValidaDepartamento(Feriado feriado)
        {
            Modelo.UsuarioPontoWeb usu = Usuario.GetUsuarioPontoWebLogadoCache();
            int idDepartamento = DepartamentoController.BuscaIdDepartamento(feriado.Departamento);
            if (idDepartamento > 0)
            {
                BLL.Departamento bllDepartamento = new BLL.Departamento(usu.ConnectionString, usu);
                Departamento dep = bllDepartamento.LoadObject(idDepartamento);
                if (dep.IdEmpresa == feriado.IdEmpresa)
                { feriado.IdDepartamento = idDepartamento; }
                else
                { ModelState["Departamento"].Errors.Add("Departamento " + feriado.Departamento + " não pertence a empresa selecionada!"); }
            }
            else
            {
                ModelState["Departamento"].Errors.Add("Departamento " + feriado.Departamento + " não cadastrado!");
            }
        }

        public void Recalcular(UsuarioPontoWeb usuario, Acao acao, Feriado feriado)
        {
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuario, "", "/Feriado/Grid");
            string parametrosExibicao = String.Format("Feriado: {0} | {1}, Data: {2}, tipo {3} | {4}", feriado.Codigo, feriado.Descricao, feriado.DataInicialStr, feriado.TipoFeriado, feriado.TipoDesc);
            Modelo.Proxy.PxyJobReturn ret = hfm.AtualizarMarcacoesFeriado("Recalculo de marcações por Feriado", parametrosExibicao, acao, feriado);
        }
    }
}