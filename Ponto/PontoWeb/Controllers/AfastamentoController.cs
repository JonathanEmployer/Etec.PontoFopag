using BLL_N.JobManager.Hangfire;
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
    public class AfastamentoController : IControllerPontoWeb<Afastamento>
    {
        [PermissoesFiltro(Roles = "Afastamento")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Afastamento());
        }

        [Authorize]
        public JsonResult DadosGrid(string dataIni, string dataFim)
        {
            try
            {
                DateTime ini = new DateTime();
                if (!DateTime.TryParse(dataIni, out ini))
                {
                    new Exception("Data inicial inválida");
                }

                DateTime fin = new DateTime();
                if (!DateTime.TryParse(dataFim, out fin))
                {
                    new Exception("Data final inválida");
                }
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Afastamento bllAfastamento = new BLL.Afastamento(usr.ConnectionString, usr);
                List<Modelo.Afastamento> dados = bllAfastamento.GetPeriodo(ini, fin);
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

        public PartialViewResult GridAfastamentos(string dataIni, string dataFim)
        {
            ViewBag.dataIni = dataIni;
            ViewBag.dataFim = dataFim;
            return PartialView(new Modelo.Afastamento());
        }

        [PermissoesFiltro(Roles = "AfastamentoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(usr.ConnectionString, usr);
            try
            {
                Afastamento afastamento = bllAfastamento.LoadObject(id);
                string erro = "";
                #region Valida fechamento
                if (!validaFechamento(out erro, afastamento))
                {
                    throw new Exception(erro);
                }
                #endregion
                afastamento.Tipo_Ant = afastamento.Tipo;
                afastamento.IdEmpresa_Ant = afastamento.IdEmpresa;
                afastamento.IdDepartamento_Ant = afastamento.IdDepartamento;
                afastamento.IdContrato = afastamento.IdContrato_Ant;
                afastamento.IdFuncionario_Ant = afastamento.IdFuncionario;
                afastamento.Datai_Ant = afastamento.Datai;
                afastamento.Dataf_Ant = afastamento.Dataf;
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllAfastamento.Salvar(Acao.Excluir, afastamento);
                if (erros.Count > 0)
                {
                    erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    erro = trataErro(erro);
                    return new JsonResult { Data = new { success = false, Erro = erro } };
                }
                afastamento.Acao = Acao.Excluir;
                Modelo.Proxy.PxyJobReturn ret = Recalcular(usr, afastamento);
                return new JsonResult { Data = new { success = true, job = ret } };
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return new JsonResult { Data = new { success = false, Erro = ex.Message } };
            }
        }

        private Modelo.Proxy.PxyJobReturn Recalcular(UsuarioPontoWeb usuario, Afastamento afastamento)
        {
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuario.DataBase, "", "", "/Afastamento/Grid");
            string nomeAfastamento = afastamento.Nome;
                //Tipo do Afastamento: 0 = Funcionário, 1 = Departamento, 2 = Empresa, 3 = Contrato
            switch (afastamento.Tipo)
            {
                case 0:
                    nomeAfastamento = "Funcionário: " + afastamento.NomeFuncionario;
                    break;
                case 1:
                    nomeAfastamento = "Departamento: " +  afastamento.NomeDepartamento;
                    break;
                case 2:
                    nomeAfastamento = "Empresa: " + afastamento.NomeEmpresa;
                    break;
                case 3:
                    nomeAfastamento = "Contrato: " + afastamento.NomeContrato;
                    break;
                default:
                    nomeAfastamento = "tipo: desconheciado";
                    break;
            }
            string parametrosExibicao = String.Format("Afastamento código: {0}, {1}, tipo abono: {2}, período: {3} a {4}", afastamento.Codigo, nomeAfastamento, afastamento.TipoAbonoDesc, afastamento.DataInicialStr, afastamento.DataFinalStr);
            string acao = afastamento.AcaoDescricao;
            Modelo.Proxy.PxyJobReturn ret = hfm.CalculaAfastamento(String.Format("Recalculo de marcações por {0} de afastamento", acao), parametrosExibicao, afastamento);
            return ret;
        }


        [HttpGet]
        public ActionResult CarregaOcorrencia(string ocorrencia)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(usr.ConnectionString, usr);
            try
            {
                string[] ocorrencias = ocorrencia.Split('|');
                string Ocorrencia = ocorrencias[1].Remove(0, 1);
                int? idocorrencia = bllOcorrencia.getOcorrenciaNome(Ocorrencia).GetValueOrDefault();
                Modelo.Ocorrencia oc = bllOcorrencia.LoadObject(idocorrencia.GetValueOrDefault());
                string erro = "";

                Dictionary<string, string> erros = new Dictionary<string, string>();
                if (erros.Count > 0)
                {
                    erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    erro = trataErro(erro);
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, Erro = " ", DefaultTipoAfastamento = oc.DefaultTipoAfastamento, HorasAbonoPadrao = oc.HorasAbonoPadrao, HorasAbonoPadraoNoturno = oc.HorasAbonoPadraoNoturno }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = "Erro ao carregar ocorrência" }, JsonRequestBehavior.AllowGet);
            }
        }


        public bool validaFechamento(out string erro, Afastamento afastamento)
        {
            int idTipo = 0;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.FechamentoPonto bllFechamentoPonto = new BLL.FechamentoPonto(usr.ConnectionString, usr);
            switch (afastamento.Tipo)
            {
                case 0: idTipo = afastamento.IdFuncionario; break;
                case 1: idTipo = afastamento.IdDepartamento; break;
                case 2: idTipo = afastamento.IdEmpresa; break;
                case 3: idTipo = afastamento.IdContrato.GetValueOrDefault(); break;
                default:
                    break;
            }
            List<FechamentoPonto> fechamentos = bllFechamentoPonto.GetFechamentosPorTipoFiltro(afastamento.Datai.GetValueOrDefault(), afastamento.Tipo, new List<int>() { idTipo });
            if (fechamentos.Count() > 0)
            {
                erro = "Operação não permitida, existe fechamento de ponto nesse período. Fechamento: <br/> " + String.Join("<br/> ", fechamentos.Select(s => "Código: " + s.Codigo + " Data: " + s.DataFechamento.ToShortDateString()));
                return false;
            }
            erro = "";
            return true;
        }

        [PermissoesFiltro(Roles = "AfastamentoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AfastamentoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AfastamentoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Afastamento obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AfastamentoCadastrar")]
        public override ActionResult Cadastrar()
        {
            int id = 0;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AfastamentoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Afastamento afastamento)
        {
            return Salvar(afastamento);
        }


        #region Métodos Auxiliares
        protected override ActionResult Salvar(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(usr.ConnectionString, usr);

            if (afastamento.DataUltimoFechamentoPontoEBanco != null || (afastamento.Codigo == 0 && afastamento.Id > 0))
            {
                Afastamento afOriginal = bllAfastamento.LoadObject(afastamento.Id);
                if ((afastamento.DataUltimoFechamentoPontoEBanco.GetValueOrDefault() > afastamento.Datai || afastamento.Datai == null) && afastamento.DataUltimoFechamentoPontoEBanco.GetValueOrDefault() > afOriginal.Datai)
                {
                    SetTipo(afOriginal, out int tipoIdentificador, out int identificacao);
                    SetDadosAfastamento(afOriginal);
                    afOriginal.Dataf = afastamento.Dataf;
                    afastamento = afOriginal;
                    ModelState.Clear();
                    TryValidateModel(afastamento);
                }
            }

            Modelo.Parametros parm = new Parametros();
            BLL.Parametros bllparm = new BLL.Parametros(usr.ConnectionString, usr);
            BLL.Empresa bllEmp = new BLL.Empresa(usr.ConnectionString, usr);
            parm = bllparm.LoadPrimeiro();
            ValidarForm(afastamento);

            if (ModelState.IsValid)
            {
                try
                {
                    afastamento.NaoRecalcular = true;
                    Acao acao = new Acao();
                    if (afastamento.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllAfastamento.Salvar(acao, afastamento);

                    if (erros.Count > 0)
                    {
                        ValidaRetornoBLLSalvar(erros);
                    }
                    else
                    {
                        Recalcular(usr, afastamento);
                        return RedirectToAction("Grid", "Afastamento");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            ViewBag.UtilizaControleContratos = usr.UtilizaControleContratos;
            ViewBag.EmpresaControleContratos = bllEmp.GetEmpresaPrincipal().UtilizaControleContratos;
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            return View("Cadastrar", afastamento);
        }

        private static string trataErro(string erro)
        {
            erro = erro.Replace("cbIdentificacao=", "");
            erro = erro.Replace("rgTipo=", "");
            erro = erro.Replace("txtDataf=", "");
            return erro;
        }

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Afastamento bllAfastamento = new BLL.Afastamento(usr.ConnectionString, usr);
            BLL.Empresa bllEmp = new BLL.Empresa(usr.ConnectionString, usr);
            BLL.Parametros bllparm = new BLL.Parametros(usr.ConnectionString, usr);

            bool contcontrato = usr.UtilizaControleContratos;
            bool emprcontrato = bllEmp.GetEmpresaPrincipal().UtilizaControleContratos;
            ViewBag.UtilizaControleContratos = contcontrato;
            ViewBag.EmpresaControleContratos = emprcontrato;
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;

            Afastamento afastamento = new Afastamento();
            afastamento = bllAfastamento.LoadObject(id);
            if (id == 0)
            {
                afastamento.Codigo = bllAfastamento.MaxCodigo();
            }
            else
            {
                int tipoIdentificador, identificacao;
                SetTipo(afastamento, out tipoIdentificador, out identificacao);
                SetDadosAfastamento(afastamento);
                if ((afastamento.objFuncionario != null) && (afastamento.objFuncionario.Excluido == 1))
                {
                    string erro = "O funcionário " + afastamento.objFuncionario.Nome + " está excluído. Para alterar este afastamento é necessário restaurar o funcionário.";
                    ModelState.AddModelError("CustomError", erro);
                    ViewBag.DesabilitaCampos = true;
                }

                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    List<PxyUltimoFechamentoPonto> pxyUltimoFechamentos;
                    DateTime? maiorFechamento;
                    bllAfastamento.GetFechamentos(identificacao, tipoIdentificador, out pxyUltimoFechamentos, out maiorFechamento);
                    afastamento.DataUltimoFechamentoPontoEBanco = maiorFechamento;
                    string mensagemFechamentoBH = "";
                    if (pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Any())
                    {
                        mensagemFechamentoBH += "Fechamento de Banco <br/>";
                        mensagemFechamentoBH += String.Join("<br/>", pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Take(100).Select(fbh => " - Data: " + fbh.UltimoFechamentoBanco.GetValueOrDefault().ToShortDateString() + " código: " + fbh.Codigo + " descrição: " + fbh.Nome).ToList());
                        if (pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Count() > 100)
                        {
                            mensagemFechamentoBH += "<br/> - * Exibindo 100 registros de fechamento de Banco de " + pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Count();
                        }

                    }

                    string mensagemFechamentoPonto = "";
                    if (pxyUltimoFechamentos.Where(w => w.UltimoFechamentoPonto != null).Any())
                    {
                        mensagemFechamentoBH += "Fechamento de Ponto <br/>";
                        mensagemFechamentoBH += String.Join("<br/>", pxyUltimoFechamentos.Where(w => w.UltimoFechamentoPonto != null).Take(100).Select(fbh => " - Data: " + fbh.UltimoFechamentoBanco.GetValueOrDefault().ToShortDateString() + " código: " + fbh.Codigo + " descrição: " + fbh.Nome).ToList());
                        if (pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Count() > 100)
                        {
                            mensagemFechamentoBH += "<br/> - * Exibindo 100 registros de fechamento de ponto de " + pxyUltimoFechamentos.Where(w => w.UltimoFechamentoBanco != null).Count();
                        }

                    }

                    if (!string.IsNullOrEmpty(mensagemFechamentoPonto))
                        mensagemFechamentoPonto = "Fechamento de Ponto <br/>" + mensagemFechamentoPonto;


                    if (!String.IsNullOrEmpty(mensagemFechamentoPonto) || !String.IsNullOrEmpty(mensagemFechamentoBH))
                    {
                        string mensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento. Detalhes: <br/>";
                        mensagemFechamento += mensagemFechamentoPonto;
                        mensagemFechamento += mensagemFechamentoBH;
                        @ViewBag.MensagemFechamento = mensagemFechamento;
                    }
                }
                #endregion
            }
            return View("Cadastrar", afastamento);
        }

        private static void SetDadosAfastamento(Afastamento afastamento)
        {
            NomeOcorrencia(afastamento);
            afastamento.Tipo_Ant = afastamento.Tipo;
            afastamento.IdEmpresa_Ant = afastamento.IdEmpresa;
            afastamento.IdDepartamento_Ant = afastamento.IdDepartamento;
            afastamento.IdFuncionario_Ant = afastamento.IdFuncionario;
            afastamento.IdContrato_Ant = afastamento.IdContrato;
            afastamento.Datai_Ant = afastamento.Datai;
            afastamento.Dataf_Ant = afastamento.Dataf;
        }

        private void SetTipo(Afastamento afastamento, out int tipoIdentificador, out int identificacao)
        {
            tipoIdentificador = 0;
            identificacao = 0;
            switch (afastamento.Tipo)
            {
                case 0:
                    TipoFuncionario(afastamento);
                    tipoIdentificador = 2;
                    identificacao = afastamento.IdFuncionario;
                    break;
                case 1:
                    TipoDepartamento(afastamento);
                    identificacao = afastamento.IdDepartamento;
                    tipoIdentificador = 1;
                    break;
                case 2:
                    TipoEmpresa(afastamento);
                    identificacao = afastamento.IdEmpresa;
                    tipoIdentificador = 0;
                    break;
                case 3:
                    TipoContrato(afastamento);
                    identificacao = afastamento.IdContrato.GetValueOrDefault();
                    tipoIdentificador = 5;
                    break;
                default:
                    break;
            }
        }

        protected override void ValidarForm(Afastamento afastamento)
        {
            switch (afastamento.Tipo)
            {
                case 0:
                    VerificaFuncionario(afastamento);
                    break;
                case 1:
                    VerificaEmpresa(afastamento);
                    ValidaDepartamento(afastamento);
                    break;
                case 2:
                    VerificaEmpresa(afastamento);
                    break;
                case 3:
                    VerificaContrato(afastamento);
                    break;
            }
            string erro = "";

            VerificaOcorrencia(afastamento);
        }

        private void VerificaContrato(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Contrato bllContrato = new BLL.Contrato(usr.ConnectionString, usr);
            int codContrato = Convert.ToInt32(afastamento.NomeContrato.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]);
            Contrato ct = bllContrato.LoadPorCodigo(codContrato);
            if (ct != null)
            {
                afastamento.IdContrato = ct.Id;
            }
            else
            {
                ModelState["NomeContrato"].Errors.Add("O campo Contrato é obrigatório");
            }
        }

        private void VerificaOcorrencia(Afastamento afastamento)
        {
            int idOcorrencia = OcorrenciaController.BuscaIdOcorrencia(afastamento.ocorrencia);
            if (idOcorrencia > 0)
            {
                afastamento.IdOcorrencia = idOcorrencia;
            }
            else
            {
                ModelState["ocorrencia"].Errors.Add("Ocorrência " + afastamento.ocorrencia + " não cadastrada!");
            }
        }

        private void ValidaDepartamento(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Departamento bllDepartamento = new BLL.Departamento(usr.ConnectionString, usr);
            int idDepartamento = DepartamentoController.BuscaIdDepartamento(afastamento.NomeDepartamento);
            if (idDepartamento > 0)
            {

                Departamento dep = bllDepartamento.LoadObject(idDepartamento);
                if (dep.IdEmpresa == afastamento.IdEmpresa)
                { afastamento.IdDepartamento = idDepartamento; }
                else
                { ModelState["NomeDepartamento"].Errors.Add("Departamento " + afastamento.NomeDepartamento + " não pertence a empresa selecionada!"); }
            }
            else
            {
                ModelState["NomeDepartamento"].Errors.Add("Departamento " + afastamento.NomeDepartamento + " não cadastrado!");
            }
        }

        private void VerificaEmpresa(Afastamento afastamento)
        {
            int idEmpresa = EmpresaController.BuscaIdEmpresa(afastamento.NomeEmpresa);
            if (idEmpresa > 0)
            {
                afastamento.IdEmpresa = idEmpresa;
            }
            else
            {
                ModelState["NomeEmpresa"].Errors.Add("Empresa " + afastamento.NomeEmpresa + " não cadastrada!");
            }
        }

        private void VerificaFuncionario(Afastamento afastamento)
        {
            int idFunc = FuncionarioController.BuscaIdFuncionario(afastamento.NomeFuncionario);
            if (idFunc > 0)
            {
                afastamento.IdFuncionario = idFunc;
            }
            else
            {
                ModelState["NomeFuncionario"].Errors.Add("Funcionário " + afastamento.NomeFuncionario + " não cadastrado!");
            }
        }

        private static void NomeOcorrencia(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bll = new BLL.Ocorrencia(usr.ConnectionString, usr);
            Ocorrencia ret = bll.LoadObject(afastamento.IdOcorrencia);
            afastamento.ocorrencia = ret.Codigo + " | " + ret.Descricao;
        }

        private static void TipoEmpresa(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Empresa bll = new BLL.Empresa(usr.ConnectionString, usr);
            Empresa ret = bll.LoadObject(afastamento.IdEmpresa);
            afastamento.NomeEmpresa = ret.Codigo + " | " + ret.Nome;
        }

        private static void TipoDepartamento(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Departamento bllDepartamento = new BLL.Departamento(usr.ConnectionString, usr);
            Departamento ret = bllDepartamento.LoadObject(afastamento.IdDepartamento);
            afastamento.NomeDepartamento = ret.Codigo + " | " + ret.Descricao;
            afastamento.NomeEmpresa = ret.empresaNome;
        }

        private static void TipoFuncionario(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFunc = new BLL.Funcionario(usr.ConnectionString, usr);
            Funcionario func = bllFunc.LoadObject(afastamento.IdFuncionario);
            afastamento.NomeFuncionario = func.Dscodigo + " | " + func.Nome;
            afastamento.NomeDepartamento = func.Departamento;
            afastamento.NomeEmpresa = func.Empresa;
            afastamento.objFuncionario = func;
        }

        private void TipoContrato(Afastamento afastamento)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Contrato bllCont = new BLL.Contrato(usr.ConnectionString, usr);
            Contrato cont = bllCont.LoadObject(afastamento.IdContrato.GetValueOrDefault());
            afastamento.NomeContrato = cont.Codigo + " | " + cont.CodigoContrato + " - " + cont.NomeEmpresa;
        }
        #endregion


    }
}