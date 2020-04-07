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
    public class BancoHorasController : IControllerPontoWeb<BancoHoras>
    {
        protected override ActionResult Salvar(BancoHoras obj)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(conn, usr);

            if(obj.Empresa == null)
            {

                var data = obj.DataFinal;
                obj = bllBancoHoras.LoadObject(obj.Id);
                obj.DataFinal = data;
                ModelState.Clear();
            }

            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();

                    erros = bllBancoHoras.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                        erro = erro.Replace("cbIdentificacao=", "");
                        ModelState.AddModelError("CustomError", "<div class=\"comment alert alert-danger\">" + erro + "</div>");
                        ViewBag.Cadastrar = 2;
                    }
                    else
                    {
                        Recalcular(usr, acao, obj);
                        return RedirectToAction("Grid", "BancoHoras");
                    }
                }
                catch (Exception ex)
                {
                    MostrarErro(ex, TratarErro(ex.Message));
                }
            }
            else
            {
                var errs = bllBancoHoras.ValidaObjeto(obj);
            }
            return View("Cadastrar", obj);
        }

        private Modelo.Proxy.PxyJobReturn Recalcular(UsuarioPontoWeb usuario, Modelo.Acao acao, BancoHoras bancoHoras)
        {
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuario.DataBase, "", "", "/BancoHoras/Grid");
            string nomeBanco = bancoHoras.Nome;
            switch (bancoHoras.Tipo)
            {
                case 0: //Empresa;
                    nomeBanco = "empresa: " + bancoHoras.Empresa;
                    break;
                case 1: // Departamento
                    nomeBanco = "departamento: " + bancoHoras.Departamento;
                    break;
                case 2: //Funcionário
                    nomeBanco = "funcionário: " + bancoHoras.Funcionario;
                    break;
                case 3: //Função
                    nomeBanco = "função: " + bancoHoras.Funcao;
                    break;
                default:
                    nomeBanco = "tipo: desconheciado";
                    break;
            }
            string parametrosExibicao = String.Format("Banco de horas código: {0}, {1}, Período: {2} a {3}", bancoHoras.Codigo, nomeBanco, bancoHoras.DataInicialStr, bancoHoras.DataFinalStr);
            string acaoDesc = "";
            switch (acao)
            {
                case Acao.Incluir:
                    acaoDesc = "inclusão";
                    break;
                case Acao.Alterar:
                    acaoDesc = "alteração";
                    break;
                case Acao.Excluir:
                    acaoDesc = "exclusão";
                    break;
                default:
                    acaoDesc = "acão desconhecia";
                    break;
            }
            Modelo.Proxy.PxyJobReturn ret = hfm.CalculaBancoHoras(String.Format("Recalculo de marcações por {0} de banco de horas", acaoDesc), parametrosExibicao, acao, bancoHoras);
            return ret;
        }

        protected override ActionResult GetPagina(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(conn, userPW);
            BancoHoras bancoHoras = new BancoHoras();
            bancoHoras = bllBancoHoras.LoadObject(id);
            if (id == 0)
            {
                bancoHoras.Codigo = bllBancoHoras.MaxCodigo();
                bancoHoras.BancoprimeiroBool = true;
            }
            else
            {
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(conn, userPW);
                    var mensagensFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionariosCollection(bancoHoras.Tipo, new List<int>() { bancoHoras.Identificacao }, bancoHoras.DataInicial.GetValueOrDefault());
                    if (mensagensFechamento.Count != 0)
                    {
                        ViewBag.Consultar = 2;
                        

                        @ViewBag.MensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento de ponto vinculado. Detalhes: <br/>";
                            //$"{string.Join(",", mensagensFechamentoKeys.Select(key => mensagensFechamento[key]))}";


                        foreach (var item in mensagensFechamento.AllKeys.Where(x => !string.IsNullOrEmpty(x)))
                        {
                            @ViewBag.MensagemFechamento += mensagensFechamento[item];

                            if (bancoHoras.DataFechamentoAcerto.HasValue && bancoHoras.DataFechamentoAcerto.Value < Convert.ToDateTime(item) || !bancoHoras.DataFechamentoAcerto.HasValue)
                                bancoHoras.DataFechamentoAcerto = Convert.ToDateTime(item);
                        }
                    }
                }
                #endregion
            }
            return View("Cadastrar", bancoHoras);
        }

        protected override void ValidarForm(BancoHoras obj)
        {
            switch (obj.Tipo)
            {
                case 0:
                    VerificaEmpresa(obj);
                    break;
                case 1:
                    VerificaDepartamento(obj);
                    break;
                case 2:
                    VerificaFuncionario(obj);
                    break;
                default:
                    VerificaFuncao(obj);
                    break;
            }

            obj.LimiteQtdHoras_1 = obj.LimiteQtdHoras_1 == null ? "--:--" : obj.LimiteQtdHoras_1;
            obj.LimiteQtdHoras_2 = obj.LimiteQtdHoras_2 == null ? "--:--" : obj.LimiteQtdHoras_2;
            obj.LimiteQtdHoras_3 = obj.LimiteQtdHoras_3 == null ? "--:--" : obj.LimiteQtdHoras_3;
            obj.LimiteQtdHoras_4 = obj.LimiteQtdHoras_4 == null ? "--:--" : obj.LimiteQtdHoras_4;
            obj.LimiteQtdHoras_5 = obj.LimiteQtdHoras_5 == null ? "--:--" : obj.LimiteQtdHoras_5;
            obj.LimiteQtdHoras_6 = obj.LimiteQtdHoras_6 == null ? "--:--" : obj.LimiteQtdHoras_6;
        }

        [PermissoesFiltro(Roles = "BancoHorasAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "BancoHorasAlterar")]
        [HttpPost]
        public override ActionResult Alterar(BancoHoras obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "BancoHorasConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "BancoHorasExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(usr.ConnectionString, usr);
            BancoHoras bancoHoras = bllBancoHoras.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllBancoHoras.Salvar(Acao.Excluir, bancoHoras);
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
                Modelo.Proxy.PxyJobReturn ret = Recalcular(usr, Acao.Excluir, bancoHoras);
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

        [PermissoesFiltro(Roles = "BancoHorasCadastrar")]
        public override ActionResult Cadastrar()
        {
            int id = 0;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "BancoHorasCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(BancoHoras obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "BancoHoras")]
        public override ActionResult Grid()
        {
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            return View(new Modelo.BancoHoras());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.BancoHoras> dados = bllBancoHoras.GetAllList(true);
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

        private string TratarErro(string erro)
        {
            if(erro.Contains("Data inválida"))
            {
                return "A data do fim do contrato não pode ser inferior a data de fechamento";
            }

            if (erro.Contains("Estouro de SqlDateTime"))
            {
                return "O período deve estar entre 01/01/1753 e 31/12/9999";
            }
            else
            {
                return erro;
            }
        }

        private void VerificaDepartamento(BancoHoras bancoHoras)
        {
            int idDepartamento = DepartamentoController.BuscaIdDepartamento(bancoHoras.Departamento);
            if (idDepartamento > 0)
            {
                bancoHoras.Identificacao = idDepartamento;
            }
            else
            {
                ModelState["Departamento"].Errors.Add("Departamento " + bancoHoras.Departamento + " não cadastrado!");
            }
        }

        private void VerificaEmpresa(BancoHoras bancoHoras)
        {
            int idEmpresa = EmpresaController.BuscaIdEmpresa(bancoHoras.Empresa);
            if (idEmpresa > 0)
            {
                bancoHoras.Identificacao = idEmpresa;
            }
            else
            {
                ModelState["Empresa"].Errors.Add("Empresa " + bancoHoras.Empresa + " não cadastrada!");
            }
        }

        private void VerificaFuncao(BancoHoras bancoHoras)
        {
            int idFuncao = FuncaoController.BuscaIdFuncao(bancoHoras.Funcao);
            if (idFuncao > 0)
            {
                bancoHoras.Identificacao = idFuncao;
            }
            else
            {
                ModelState["Funcao"].Errors.Add("Função " + bancoHoras.Funcao + " não cadastrada!");
            }
        }

        private void VerificaFuncionario(BancoHoras bancoHoras)
        {
            int idFunc = FuncionarioController.BuscaIdFuncionario(bancoHoras.Funcionario);
            if (idFunc > 0)
            {
                bancoHoras.Identificacao = idFunc;
            }
            else
            {
                ModelState["Funcionario"].Errors.Add("Funcionário " + bancoHoras.Funcionario + " não cadastrado!");
            }
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            IList<BancoHoras> lBancoHoras = new List<BancoHoras>();
            BancoHoras bancoHoras = new BancoHoras();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }

            bancoHoras = bllBancoHoras.LoadObjectByCodigo(codigo);
            if (codigo != -1)
            {
                if (bancoHoras != null && bancoHoras.Id > 0)
                {
                    lBancoHoras.Add(bancoHoras);
                }
            }

            if (lBancoHoras.Count == 0)
            {
                lBancoHoras = bllBancoHoras.GetAllList(true);
            }
            ViewBag.Title = "Pesquisar Banco de Horas";
            return View(lBancoHoras);
        }
    }
}