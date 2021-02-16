using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
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
    public class JornadaAlternativaController : IControllerPontoWeb<JornadaAlternativa>
    {

        [PermissoesFiltro(Roles = "JornadaAlternativa")]
        public override ActionResult Grid()
        {
            BLL.JornadaAlternativa bllJornada = new BLL.JornadaAlternativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            return View(new Modelo.JornadaAlternativa());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.JornadaAlternativa> dados = bllJornadaAlternativa.GetAllList(false);
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

        [PermissoesFiltro(Roles = "JornadaAlternativaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JornadaAlternativaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "JornadaAlternativaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JornadaAlternativaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(JornadaAlternativa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JornadaAlternativaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(JornadaAlternativa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JornadaAlternativaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.JornadaAlternativa bllJornada = new BLL.JornadaAlternativa(usr.ConnectionString, usr);
            JornadaAlternativa jornada = bllJornada.LoadObject(id);
            jornada.NaoRecalcular = true;
            try
            {
                SelecaoFuncionarios(usr.ConnectionString, usr, jornada);
                Dictionary<string, string> erros = new Dictionary<string, string>();
                if (!jornada.DataInicial_Ant.HasValue)
                {
                    jornada.DataInicial_Ant = jornada.DataInicial;
                }
                if (!jornada.DataFinal_Ant.HasValue)
                {
                    jornada.DataFinal_Ant = jornada.DataFinal;
                }
                erros = bllJornada.Salvar(Acao.Excluir, jornada);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                jornada.Acao = Acao.Excluir;
                Recalcular(usr, jornada);
                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override ActionResult Salvar(JornadaAlternativa obj)
        {
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            ViewBag.UtilizaControleContratos = UsuPW.UtilizaControleContratos;
            if (UsuPW.UtilizaControleContratos)
            {
                obj.Tipo = 2;
            }
            BLL.JornadaAlternativa bllJornada = new BLL.JornadaAlternativa(UsuPW.ConnectionString, UsuPW);
            ValidarJornada(obj, true);
            if (ModelState.IsValid)
            {
                try
                {
                    obj.ConverteHoraStringToInt();
                    if (obj.DiasJA == null)
                    {
                        obj.DiasJA = new List<DiasJornadaAlternativa>();
                    }
                    else
                    {
                        foreach (var item in obj.DiasJA)
                        {
                            item.NaoValidaCodigo = true;
                            item.JornadaAlternativa = obj;
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
                    bool salvar = true;
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                        acao = Acao.Incluir;
                    else
                    {
                        acao = Acao.Alterar;
                        JornadaAlternativa jAnt = bllJornada.LoadObject(obj.Id);
                        var log = obj.GetChanges(jAnt);
                        if (log.Where(w => w.OldValue != w.NewValue).ToList().Count == 0 &&
                            obj.DiasJA.Where(w => w.Acao == Acao.Incluir).Count() != jAnt.DiasJA.Where(w => w.Acao == Acao.Incluir).Count() &&
                            obj.DiasJA.Where(w => w.Acao == Acao.Alterar).Count() != jAnt.DiasJA.Where(w => w.Acao == Acao.Alterar).Count() &&
                            obj.DiasJA.Where(w => w.Acao == Acao.Excluir).Count() != jAnt.DiasJA.Where(w => w.Acao == Acao.Excluir).Count())
                        {
                            salvar = false;
                        }
                        obj.DataFinal_Ant = jAnt.DataFinal;
                        obj.DataInicial_Ant = jAnt.DataInicial;
                        obj.Tipo_Ant = jAnt.Tipo;
                        obj.Identificacao_Ant = jAnt.Identificacao;
                    }

                    if (salvar)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        obj.NaoRecalcular = true;
                        erros = bllJornada.Salvar(acao, obj);
                        if (erros.Count > 0)
                        {
                            string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                            ModelState.AddModelError("CustomError", "<div class=\"comment alert alert-danger\">" + erro + "</div>");
                        }
                        else
                        {
                            obj.Acao = acao;
                            Recalcular(UsuPW, obj);
                            return RedirectToAction("Grid", "JornadaAlternativa");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Grid", "JornadaAlternativa");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
                int z = errors.Count;
                return View(obj);
            }
            return View("Cadastrar", obj);
        }

        public PxyJobReturn Recalcular(UsuarioPontoWeb usuario, JornadaAlternativa jornada)
        {
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuario.DataBase, "", "", "/JornadaAlternativa/Grid");
            string parametrosExibicao = String.Format("Jornada Alternativa código: {0}, jornada: {1}, período: {2} a {3}, tipo {4} | {5}", jornada.Codigo, jornada.DescJornada, jornada.DataInicialStr, jornada.DataFinalStr, jornada.TipoDesc, jornada.Funcionario);
            Modelo.Proxy.PxyJobReturn ret = hfm.AtualizaMarcacaoJornadaAlternativa("Recalculo de marcações por jornada alternativa", parametrosExibicao, jornada);
            return ret;
        }

        protected override ActionResult GetPagina(int id)
        {
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.JornadaAlternativa bllJornada = new BLL.JornadaAlternativa(conn, UsuPW);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(conn, UsuPW);
            BLL.Parametros bllParams = new BLL.Parametros(conn, UsuPW);
            JornadaAlternativa objJornada = new JornadaAlternativa();

            if (id == 0)
            {
                objJornada.Codigo = bllJornada.MaxCodigo();
                objJornada.DiasJA = new List<DiasJornadaAlternativa>();
                objJornada.LimiteMin = "03:00";
                objJornada.LimiteMax = "03:00";
                objJornada.Tipo = 0;
                objJornada.bHorasNormais = true;
                objJornada.DiasJA = new List<DiasJornadaAlternativa>();
            }
            else
            {
                objJornada = bllJornada.LoadObject(id);
                if (!objJornada.DataInicial_Ant.HasValue && !objJornada.DataFinal_Ant.HasValue)
                {
                    objJornada.DataInicial_Ant = objJornada.DataInicial;
                    objJornada.DataFinal_Ant = objJornada.DataFinal;
                }
                objJornada.Tipo_Ant = objJornada.Tipo;
                SelecaoFuncionarios(UsuPW.ConnectionString, UsuPW, objJornada);


                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    List<int> idTipos = (!string.IsNullOrEmpty(objJornada.IdsJornadaAlternativaFuncionariosSelecionados) && objJornada.Tipo == 2) ?
                                         objJornada.IdsJornadaAlternativaFuncionariosSelecionados.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList() :
                                         new List<int>() { objJornada.Identificacao };

                    BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(conn, UsuPW);
                    string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(objJornada.Tipo, idTipos, objJornada.DataInicial.GetValueOrDefault());
                    if (!String.IsNullOrEmpty(mensagemFechamento))
                    {
                        ViewBag.Consultar = 1;
                        @ViewBag.MensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento de ponto vinculado. Detalhes: <br/>" + mensagemFechamento;
                    }
                }
                #endregion 
            }
            objJornada.Parametros = new Parametros();

            objJornada.Parametros = bllParams.LoadPrimeiro();

            PreencheViewBagsAdNoturno(objJornada, bllParams);
            ViewBag.UtilizaControleContratos = UsuPW.UtilizaControleContratos;
            return View("Cadastrar", objJornada);
        }

        protected override void ValidarForm(JornadaAlternativa obj)
        {

        }

        #region Funções Vazia necessarias somente para o progress funcionar
        private void SetaValorProgressBar(int valor) { }
        private void SetaMinMaxProgressBar(int min, int max) { }
        private void SetaMensagem(string mensagem) { }
        private void IncrementaProgressBar(int incremento) { }
        #endregion

        [PermissoesFiltro(Roles = "JornadaAlternativaCadastrar")]
        public ActionResult CadastrarJornadaManutMarcacao(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(conn, UsuPW);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(conn, UsuPW);
            BLL.Parametros bllParams = new BLL.Parametros(conn, UsuPW);
            Marcacao objMarcacao = new Marcacao();
            objMarcacao = bllMarcacao.LoadObject(id);

            JornadaAlternativa objJornadaAlternativa = new JornadaAlternativa();
            objJornadaAlternativa = bllJornadaAlternativa.LoadParaUmaMarcacao(objMarcacao.Data, 2, objMarcacao.Idfuncionario);
            if (objJornadaAlternativa.Id == 0)
            {
                objJornadaAlternativa.Acao = Acao.Incluir;
                objJornadaAlternativa.DataInicial = objMarcacao.Data;
                objJornadaAlternativa.DataFinal = objMarcacao.Data;
                objJornadaAlternativa.DataInicial_Ant = objJornadaAlternativa.DataInicial;
                objJornadaAlternativa.DataFinal_Ant = objJornadaAlternativa.DataFinal;
                objJornadaAlternativa.Identificacao = objMarcacao.Idfuncionario;
                objJornadaAlternativa.Tipo = 2;
                objJornadaAlternativa.LimiteMin = "03:00";
                objJornadaAlternativa.LimiteMax = "03:00";
                objJornadaAlternativa.HorasNormais = 1;
                objJornadaAlternativa.HabilitaTolerancia = 0;
                objJornadaAlternativa.SomenteCargaHoraria = 0;
                objJornadaAlternativa.OrdenaBilheteSaida = 0;
                objJornadaAlternativa.Codigo = bllJornadaAlternativa.MaxCodigo();
                objJornadaAlternativa.Entrada_1 = "--:--";
                objJornadaAlternativa.Entrada_2 = "--:--";
                objJornadaAlternativa.Entrada_3 = "--:--";
                objJornadaAlternativa.Entrada_4 = "--:--";
                objJornadaAlternativa.Saida_1 = "--:--";
                objJornadaAlternativa.Saida_2 = "--:--";
                objJornadaAlternativa.Saida_3 = "--:--";
                objJornadaAlternativa.Saida_4 = "--:--";
                objJornadaAlternativa.TotalTrabalhadaDiurna = "--:--";
                objJornadaAlternativa.TotalTrabalhadaNoturna = "--:--";
                objJornadaAlternativa.bHorasNormais = true;
                objJornadaAlternativa.bCalculoAdicionalNoturno = false;
            }
            else
            {
                objJornadaAlternativa.Acao = Acao.Alterar;
                objJornadaAlternativa.Tipo_Ant = objJornadaAlternativa.Tipo;
                objJornadaAlternativa.DataInicial_Ant = objJornadaAlternativa.DataInicial;
                objJornadaAlternativa.DataFinal_Ant = objJornadaAlternativa.DataFinal;
                objJornadaAlternativa.Identificacao_Ant = objJornadaAlternativa.Identificacao;
            }

            PreencheViewBagsAdNoturno(objJornadaAlternativa, bllParams);
            return View(objJornadaAlternativa);
        }

        private void PreencheViewBagsAdNoturno(JornadaAlternativa objJornadaAlternativa, BLL.Parametros bllParams)
        {
            objJornadaAlternativa.Parametros = objJornadaAlternativa.Parametros == null ? bllParams.LoadPrimeiro() : objJornadaAlternativa.Parametros;

            if ((objJornadaAlternativa.Parametros.Id == 0) ||
                (String.IsNullOrEmpty(objJornadaAlternativa.Parametros.InicioAdNoturno) ||
                (String.IsNullOrEmpty(objJornadaAlternativa.Parametros.FimAdNoturno))))
            {
                ViewBag.InicioHNoturna = "--:--";
                ViewBag.FimHNotura = "--:--";
            }
            else
            {
                ViewBag.InicioHNoturna = objJornadaAlternativa.Parametros.InicioAdNoturno;
                ViewBag.FimHNotura = objJornadaAlternativa.Parametros.FimAdNoturno;
            }
        }

        [PermissoesFiltro(Roles = "JornadaAlternativaCadastrar")]
        [HttpPost]
        public ActionResult CadastrarJornadaManutMarcacao(JornadaAlternativa obj)
        {
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.JornadaAlternativa bllJornada = new BLL.JornadaAlternativa(UsuPW.ConnectionString, UsuPW);
            try
            {
                if (obj.Acao != Acao.Excluir)
                {
                    ValidarJornada(obj, false);
                }
                if (ModelState.IsValid)
                {
                    obj.ConverteHoraStringToInt();
                    if (obj.DiasJA == null)
                    {
                        obj.DiasJA = new List<DiasJornadaAlternativa>();
                    }
                    else
                    {
                        foreach (var item in obj.DiasJA)
                        {
                            item.JornadaAlternativa = obj;
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

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    obj.NaoRecalcular = true;
                    erros = bllJornada.Salvar(obj.Acao, obj);

                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        return new JsonResult { Data = new { success = false, Erro = erro } };
                    }
                    else
                    {
                        Modelo.Proxy.PxyJobReturn ret = Recalcular(UsuPW, obj);
                        return new JsonResult
                        {
                            Data = new
                            {
                                success = true,
                                job = ret
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                ModelState.AddModelError("CustomError", ex.Message);
            }
            return ModelState.JsonErrorResult();
        }


        public void ValidarJornada(JornadaAlternativa objeto, bool validarTipo)
        {
            UsuarioPontoWeb UsuPW = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.JornadaAlternativa bllJornada = new BLL.JornadaAlternativa(conn, UsuPW);
            #region Validação Tipo Jornada
            if (validarTipo)
            {
                switch (objeto.Tipo)
                {
                    case 0:
                        if (String.IsNullOrEmpty(objeto.Empresa))
                        {
                            ModelState["Empresa"].Errors.Add("Selecione uma empresa.");
                        }
                        else
                        {
                            BLL.Empresa bllEmp = new BLL.Empresa(conn, UsuPW);
                            Empresa e = new Empresa();
                            int idEmpresa;
                            string empresa = objeto.Empresa.Split('|')[0].Trim();
                            if (int.TryParse(empresa, out idEmpresa))
                            {
                                e = bllEmp.LoadObjectByCodigo(idEmpresa);
                            }
                            if (e != null && e.Id > 0)
                            {
                                objeto.Identificacao = e.Id;
                            }
                            else
                            {
                                ModelState["Empresa"].Errors.Add("Empresa " + empresa + " não cadastrada!");
                            }
                        }
                        break;
                    case 1:
                        if (String.IsNullOrEmpty(objeto.Departamento))
                        {
                            ModelState["Departamento"].Errors.Add("Selecione um departamento.");
                        }
                        else
                        {
                            BLL.Departamento blldep = new BLL.Departamento(conn, UsuPW);
                            Departamento d = new Departamento();
                            int idDepartamento;
                            string depto = objeto.Departamento.Split('|')[0].Trim();
                            if (int.TryParse(depto, out idDepartamento))
                            {
                                d = blldep.LoadObjectByCodigo(idDepartamento);
                            }
                            if (d != null && d.Id > 0)
                            {
                                objeto.Identificacao = d.Id;
                            }
                            else
                            {
                                ModelState["Departamento"].Errors.Add("Departamento " + depto + " não cadastrado!");
                            }
                        }
                        break;
                    case 2:

                        if (String.IsNullOrEmpty(objeto.IdsJornadaAlternativaFuncionariosSelecionados))
                        {
                            ModelState["IdsJornadaAlternativaFuncionariosSelecionados"].Errors.Add("Selecione um funcionário.");
                        }

                        break;

                    case 3:
                        if (String.IsNullOrEmpty(objeto.Funcao))
                        {
                            ModelState["Funcao"].Errors.Add("Selecione uma função.");
                        }
                        else
                        {
                            BLL.Funcao bllfuncao = new BLL.Funcao(conn, UsuPW);
                            Funcao d = new Funcao();
                            int idFuncao;
                            string func = objeto.Funcao.Split('|')[0].Trim();
                            if (int.TryParse(func, out idFuncao))
                            {
                                d = bllfuncao.LoadObjectByCodigo(idFuncao);
                            }
                            if (d != null && d.Id > 0)
                            {
                                objeto.Identificacao = d.Id;
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
            }
            #endregion
            //ModelState.AddModelError("CustomError", "A Compensação deve ser desfeita antes de ser alterada ou excluída");

            if (String.IsNullOrEmpty(objeto.DescJornada))
            {
                ModelState["DescJornada"].Errors.Add("Selecione uma Jornada.");
            }
            else
            {
                BLL.Jornada bllJornadaNormal = new BLL.Jornada(conn, UsuPW);
                Jornada j = new Jornada();
                int idJornada;
                List<string> strs = objeto.DescJornada.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string jornada = strs[0];
                if (int.TryParse(jornada, out idJornada))
                {
                    j = bllJornadaNormal.LoadObjectCodigo(idJornada);
                }
                if (j != null && j.Id > 0)
                {
                    objeto.Idjornada = j.Id;
                }
                else
                {
                    ModelState["DescJornada"].Errors.Add("Jornada " + objeto.DescJornada + " não cadastrada!");
                }
            }

            if (objeto.Codigo == 0)
            {
                ModelState["Codigo"].Errors.Add("Campo obrigatório.");
            }

            if (objeto.DataInicial != null && objeto.DataFinal != null)
            {
                if (objeto.DataInicial > objeto.DataFinal)
                {
                    ModelState["DataFinal"].Errors.Add("A data final deve ser maior ou igual à data inicial.");
                }
            }
            else
            {
                if (objeto.DataInicial == null)
                {
                    ModelState["DataInicial"].Errors.Add("Campo obrigatório.");
                }

                if (objeto.DataFinal == null)
                {
                    ModelState["DataFinal"].Errors.Add("Campo obrigatório.");
                }
            }

            if (objeto.Tipo <= -1)
            {
                ModelState["Tipo"].Errors.Add("Campo obrigatório.");
            }

            if (objeto.LimiteMin == "--:--")
            {
                ModelState["LimiteMin"].Errors.Add("Campo obrigatório");
            }
            if (objeto.LimiteMax == "--:--")
            {
                ModelState["LimiteMax"].Errors.Add("Campo obrigatório");
            }

            if ((objeto.DataInicial != null) && (objeto.DataFinal != null))
            {
                if (!string.IsNullOrEmpty(objeto.IdsJornadaAlternativaFuncionariosSelecionados) && objeto.Tipo == 2)
                {
                    List<int> idTipos = objeto.IdsJornadaAlternativaFuncionariosSelecionados.Split(',').ToList().Select(s => Convert.ToInt32(s)).ToList();

                    foreach (var item in idTipos)
                    {
                        if (bllJornada.VerificaExiste(objeto.Id, objeto.DataInicial.Value, objeto.DataFinal.Value, objeto.Tipo, item))
                        {
                            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, UsuPW);
                            var func = bllFuncionario.LoadObject(item);
                            ModelState["DataInicial"].Errors.Add("Já existe um registro gravado dentro deste período para o funcionario '" + func.Nome + "'.");
                            ModelState["DataFinal"].Errors.Add("Já existe um registro gravado dentro deste período para o funcionario '" + func.Nome + "'.");
                        }
                    }
                }
                else
                {
                    if (bllJornada.VerificaExiste(objeto.Id, objeto.DataInicial.Value, objeto.DataFinal.Value, objeto.Tipo, objeto.Identificacao))
                    {
                        ModelState["DataInicial"].Errors.Add("Já existe um registro gravado dentro deste período.");
                        ModelState["DataFinal"].Errors.Add("Já existe um registro gravado dentro deste período.");
                    }
                }
            }
            if (objeto.DiasJA != null)
            {
                if (objeto.DiasJA.Count > 0)
                {
                    IList<DiasJornadaAlternativa> excluidos = new List<DiasJornadaAlternativa>();
                    IList<DiasJornadaAlternativa> persistidos = new List<DiasJornadaAlternativa>();
                    excluidos = objeto.DiasJA.Where(w => w.Delete && w.Id != 0).ToList();
                    persistidos = objeto.DiasJA.Where(w => w.Id != 0).ToList();
                    if (excluidos != null && persistidos != null)
                    {
                        if ((excluidos.Count == persistidos.Count && persistidos.Count > 0
                            && ((!objeto.DataInicial.HasValue) && (!objeto.DataFinal.HasValue)))
                            ^
                            (objeto.DiasJA.Count == 0 && ((!objeto.DataInicial.HasValue) && (!objeto.DataFinal.HasValue))))
                        {
                            ModelState["DiasJA"].Errors.Add("Obrigatório informar ao menos uma data ou período para compensação");
                        }
                    }
                }
            }
        }

        private static void SelecaoFuncionarios(string conn, UsuarioPontoWeb userPW, JornadaAlternativa objJornada)
        {
            BLL.JornadaAlternativaFuncionario bllJornadaAlternativaFuncionario = new BLL.JornadaAlternativaFuncionario(conn, userPW);
            objJornada.JornadaAlternativaFuncionarios = bllJornadaAlternativaFuncionario.GetListWhere(" and idJornadaAlternativa = " + objJornada.Id);
            if (objJornada.JornadaAlternativaFuncionarios != null && objJornada.JornadaAlternativaFuncionarios.Count() > 0)
            {
                objJornada.IdsJornadaAlternativaFuncionariosSelecionados = String.Join(",", objJornada.JornadaAlternativaFuncionarios.Select(x => x.IdFuncionario).ToArray());
                objJornada.IdsJornadaAlternativaFuncionariosSelecionados_Ant = objJornada.IdsJornadaAlternativaFuncionariosSelecionados;
            }
        }

    }
}