using BLL_N.JobManager.Hangfire;
using Hangfire.Annotations;
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
        UsuarioPontoWeb userPW = Usuario.GetUsuarioPontoWebLogadoCache();
        protected override ActionResult Salvar(BancoHoras obj)
        {
            bool naoValidarFechamento = false;
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(userPW.ConnectionString, userPW);
            if (obj.DataUltimoFechamentoPontoEBanco != null || (obj.Codigo == 0 && obj.Id > 0))
            {
                BancoHoras bhOriginal = bllBancoHoras.LoadObject(obj.Id);
                GetFechamento(bhOriginal);
                if (bhOriginal.DataUltimoFechamentoPontoEBanco != null)
                {
                    bhOriginal.DataFinal = obj.DataFinal;
                    obj = bhOriginal;
                    ModelState.Clear();
                    TryValidateModel(obj);
                    naoValidarFechamento = true;
                }
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
                    erros = bllBancoHoras.Salvar(acao, obj, naoValidarFechamento);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                        erro = erro.Replace("cbIdentificacao=", "");
                        ModelState.AddModelError("CustomError", "<div class=\"comment alert alert-danger\">" + erro + "</div>");
                    }
                    else
                    {
                        Recalcular(userPW, acao, obj);
                        return RedirectToAction("Grid", "BancoHoras");
                    }
                }
                catch (Exception ex)
                {
                    MostrarErro(ex, TratarErro(ex.Message));
                }
            }
            return View("Cadastrar", obj);
        }

        private Modelo.Proxy.PxyJobReturn Recalcular(UsuarioPontoWeb usuario, Modelo.Acao acao, BancoHoras bancoHoras)
        {
            HangfireManagerCalculos hfm = new HangfireManagerCalculos(usuario.DataBase, "", "", "/BancoHoras/Grid");
            Modelo.Proxy.PxyJobReturn ret = hfm.CalculaBancoHoras(acao, bancoHoras);
            return ret;
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(userPW.ConnectionString, userPW);
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
                    GetFechamento(bancoHoras);
                }
                #endregion
            }
            return View("Cadastrar", bancoHoras);
        }

        private void GetFechamento(BancoHoras bancoHoras)
        {
            string mensagemFechamentoBH = "";
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(userPW.ConnectionString, userPW);
            List<FechamentoBH> fechamentosbh = bllFechamentoBH.GetByIdBancoHoras(bancoHoras.Id);
            DateTime? dataUltimoFechamentoBH = null;
            if (fechamentosbh.Any())
            {
                mensagemFechamentoBH += "Fechamento de Banco <br/>";
                dataUltimoFechamentoBH = fechamentosbh.Max(m => m.Data);
                mensagemFechamentoBH += String.Join("<br/>", fechamentosbh.Take(100).Select(fbh => " - Data: " + fbh.Data.GetValueOrDefault().ToShortDateString() + " código: " + fbh.Codigo + " descrição: " + fbh.NomeTipoPessoa ).ToList());
                if (fechamentosbh.Count > 100)
                {
                    mensagemFechamentoBH += "<br/> - * Exibindo 100 registros de fechamento de Banco de " + fechamentosbh.Count;
                }

            }

            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(userPW.ConnectionString, userPW);
            string mensagemFechamentoPonto = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(bancoHoras.Tipo, new List<int>() { bancoHoras.Identificacao }, bancoHoras.DataInicial.GetValueOrDefault(), out DateTime? dataUltimoFechamentoPonto);

            if (!string.IsNullOrEmpty(mensagemFechamentoPonto))
                mensagemFechamentoPonto = "Fechamento de Ponto <br/>" + mensagemFechamentoPonto;
            

            if (!String.IsNullOrEmpty(mensagemFechamentoPonto) || !String.IsNullOrEmpty(mensagemFechamentoBH))
            {
                string mensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento. Detalhes: <br/>";
                mensagemFechamento += mensagemFechamentoPonto;
                mensagemFechamento += mensagemFechamentoBH;
                @ViewBag.MensagemFechamento = mensagemFechamento;
                bancoHoras.DataUltimoFechamentoPontoEBanco = dataUltimoFechamentoPonto.GetValueOrDefault() > dataUltimoFechamentoBH.GetValueOrDefault() ? dataUltimoFechamentoPonto : dataUltimoFechamentoBH;
            }
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
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(userPW.ConnectionString, userPW);
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
                Modelo.Proxy.PxyJobReturn ret = Recalcular(userPW, Acao.Excluir, bancoHoras);
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
            return View(new Modelo.BancoHoras());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(userPW.ConnectionString, userPW);
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
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(userPW.ConnectionString, userPW);
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