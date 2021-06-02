using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class FechamentoBHController : IControllerPontoWeb<FechamentoBH> 
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        [PermissoesFiltro(Roles = "FechamentoBH")]
        public override ActionResult Grid()
        {
            return View(new Modelo.FechamentoBH());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(_usr.ConnectionString, _usr);
                List<Modelo.FechamentoBH> dados = bllFechamentoBH.GetAllList();
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

        [PermissoesFiltro(Roles = "FechamentoBHConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Disabled = true;
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoBHCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "FechamentoBHAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FechamentoBHCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(FechamentoBH obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoBHAlterar")]
        [HttpPost]
        public override ActionResult Alterar(FechamentoBH obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FechamentoBHExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(_usr.ConnectionString, _usr);
            FechamentoBH fechamento = bllFechamentoBH.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                bllFechamentoBH.ValidaFechamento(fechamento, ref erros);
                if (erros.Count == 0)
                {
                    fechamento.Acao = Acao.Excluir;
                    Modelo.Proxy.PxyJobReturn ret = FecharPonto(fechamento, new BancoHoras());
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = true,
                            job = ret
                        }
                    };
                }
                else
                {
                    string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = false,
                            Erro = erro
                        }
                    };
                }
                
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

        private Modelo.Proxy.PxyJobReturn FecharPonto(FechamentoBH fechamento, BancoHoras objBancoHoras)
        {
            UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();

            HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW, "", "/FechamentoBH/Grid");
            string parametrosExibicao = String.Format("Código: {0}, Data: {1}, tipo: {2} - {3}", fechamento.Codigo, fechamento.Data.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm"), fechamento.TipoStr, fechamento.NomeTipoPessoa);
            string acao = fechamento.AcaoDescricao;
            Modelo.Proxy.PxyJobReturn ret = new Modelo.Proxy.PxyJobReturn();
            if (fechamento.Acao == Acao.Excluir)
            {
                ret = hfm.ExcluirFechamentoPonto(String.Format("Cálculo de {0} de fechamento de Banco de Horas", acao), parametrosExibicao, fechamento);
            }
            else
            {
                ret = hfm.FechamentoPonto(String.Format("Cálculo de {0} de fechamento de Banco de Horas", acao), parametrosExibicao, fechamento, objBancoHoras);
            }
            return ret;
        }

        protected override ActionResult Salvar(FechamentoBH obj)
        {
            ValidarForm(obj);
            Dictionary<string, string> erros = new Dictionary<string, string>();
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(_usr.ConnectionString, _usr);
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(_usr.ConnectionString, _usr);
            BLL.Empresa bllEmpresa = new BLL.Empresa(_usr.ConnectionString, _usr);
            BLL.Departamento bllDepartamento = new BLL.Departamento(_usr.ConnectionString, _usr);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            BLL.Funcao bllFuncao = new BLL.Funcao(_usr.ConnectionString, _usr);
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
            BLL.FechamentobhdHE bllFechamentobhdHE = new BLL.FechamentobhdHE(_usr.ConnectionString, _usr);

            Modelo.BancoHoras objBancoHoras;
            Modelo.FechamentoBH objFechamentoBH;

            Modelo.ProgressBar objProgress = new Modelo.ProgressBar();

            int idTipo = -1;
            int idBancoHoras = -1;
            idBancoHoras = BuscaIdBancoHoras(obj, bllBancoHoras, idBancoHoras, out objBancoHoras);
            idTipo = BuscaIdTipo(obj, bllEmpresa, bllDepartamento, bllFuncionario, bllFuncao, idTipo);

            if ((obj.Data < objBancoHoras.DataInicial) ||
                (obj.Data > objBancoHoras.DataFinal))
            {
                ModelState["Data"].Errors.Add("A Data de fechamento está fora do intervalo do Banco de Horas");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        IList<Modelo.FechamentoBHDPercentual> listaobjFechamentoBHDPercentual = new List<Modelo.FechamentoBHDPercentual>();
                        IList<Modelo.FechamentobhdHE> listaobjFechamentobhdHE = new List<Modelo.FechamentobhdHE>();
                        obj.Acao = Acao.Incluir;
                        erros = bllFechamentoBH.ChamaPreencheObjeto(out objBancoHoras, out objFechamentoBH, obj.Tipo, idTipo, idBancoHoras, obj.Data.Value, obj.MotivoFechamento, obj.PagamentoHoraCreAuto, obj.LimiteHorasPagamentoCredito, obj.PagamentoHoraDebAuto, obj.LimiteHorasPagamentoDebito);

                        if (erros.Count != 0)
                        {
                            string erro = string.Join("<br/>", erros.Select(x => x.Key + "<br/>" + x.Value).ToArray());
                            ModelState.AddModelError("CustomError", "<div class=\"comment alert alert-danger\">" + erro + "</div>");
                        }
                        else
                        {
                            objFechamentoBH.Acao = Acao.Incluir;
                            objFechamentoBH.Tipo = obj.Tipo;
                            objFechamentoBH.NomeTipoPessoa = obj.NomeTipoPessoa;
                            FecharPonto(objFechamentoBH, objBancoHoras);
                            return RedirectToAction("Grid", "FechamentoBH");
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        ModelState.AddModelError("CustomError", ex.Message);
                    }
                }
            }
            ViewBag.UtilizaControleContratos = _usr.UtilizaControleContratos;
            return View();
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.FechamentoBH bllFechamentoBH = new BLL.FechamentoBH(_usr.ConnectionString, _usr);
            FechamentoBH f = new FechamentoBH();
            f = bllFechamentoBH.LoadObject(id);
            if (f.IdBancoHoras.HasValue && f.IdBancoHoras.GetValueOrDefault() > 0)
            {
                BLL.BancoHoras bllBH = new BLL.BancoHoras(_usr.ConnectionString, _usr);
                BancoHoras bancoHoras = bllBH.LoadObject(f.IdBancoHoras.GetValueOrDefault());
                f.BancoHoras = bancoHoras.Codigo + " - " + bancoHoras.Nome;
            }

            ViewBag.UtilizaControleContratos = _usr.UtilizaControleContratos;

            if (id == 0)
            {
                f.Codigo = bllFechamentoBH.MaxCodigo();
            }
            else
            {
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(_usr.ConnectionString, _usr);
                    string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(f.Tipo, new List<int>() { f.Identificacao }, f.Data.GetValueOrDefault());
                    if (!String.IsNullOrEmpty(mensagemFechamento))
                    {
                        ViewBag.Consultar = 1;
                        @ViewBag.MensagemFechamento = "Registro não pode mais ser alterado. Existe fechamento de ponto vinculado. Detalhes: <br/>" + mensagemFechamento;
                    }
                }
                #endregion 
            }
            return View("Cadastrar",f);
        }

        protected override void ValidarForm(FechamentoBH obj)
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
        }

        private void VerificaDepartamento(FechamentoBH fechamentoBH)
        {
            int idDepartamento = DepartamentoController.BuscaIdDepartamento(fechamentoBH.Departamento);
            if (idDepartamento > 0)
            {
                fechamentoBH.NomeTipoPessoa = fechamentoBH.Departamento;
            }
            else
            {
                ModelState["Departamento"].Errors.Add("Departamento " + fechamentoBH.Departamento + " não cadastrado!");
            }
        }

        private void VerificaEmpresa(FechamentoBH fechamentoBH)
        {
            int idEmpresa = EmpresaController.BuscaIdEmpresa(fechamentoBH.Empresa);
            if (idEmpresa > 0)
            {
                fechamentoBH.NomeTipoPessoa = fechamentoBH.Empresa;
            }
            else
            {
                ModelState["Empresa"].Errors.Add("Empresa " + fechamentoBH.Empresa + " não cadastrada!");
            }
        }

        private void VerificaFuncao(FechamentoBH fechamentoBH)
        {
            int idFuncao = FuncaoController.BuscaIdFuncao(fechamentoBH.Funcao);
            if (idFuncao > 0)
            {
                fechamentoBH.NomeTipoPessoa = fechamentoBH.Funcao;
            }
            else
            {
                ModelState["Funcao"].Errors.Add("Função " + fechamentoBH.Funcao + " não cadastrada!");
            }
        }

        private void VerificaFuncionario(FechamentoBH fechamentoBH)
        {
            int idFunc = FuncionarioController.BuscaIdFuncionario(fechamentoBH.Funcionario);
            if (idFunc > 0)
            {
                fechamentoBH.NomeTipoPessoa = fechamentoBH.Funcionario;               
            }
            else
            {
                ModelState["Funcionario"].Errors.Add("Função " + fechamentoBH.Funcionario + " não cadastrada!");
            }
        }


        private static int BuscaIdBancoHoras(FechamentoBH fechamentoBH, BLL.BancoHoras bllBancoHoras, int idBancoHoras, out Modelo.BancoHoras bancoHoras)
        {
            bancoHoras = new BancoHoras();
            if (fechamentoBH.BancoHoras != null)
            {
                bancoHoras = bllBancoHoras.LoadObjectByCodigo(fechamentoBH.CodigoBancoHoras);
                idBancoHoras = bancoHoras.Id;
            }
            return idBancoHoras;
        }

        private static int BuscaIdTipo(FechamentoBH fechamentoBH, BLL.Empresa bllEmpresa, BLL.Departamento bllDepartamento, BLL.Funcionario bllFuncionario, BLL.Funcao bllFuncao, int idTipo)
        {
            //Tipo do FechamentoBH:  0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função
            switch (fechamentoBH.Tipo)
            {
                case 0:
                    idTipo = bllEmpresa.LoadObjectByCodigo(fechamentoBH.CodigoEmpresa).Id;
                    break;
                case 1:
                    idTipo = bllDepartamento.LoadObjectByCodigo(fechamentoBH.CodigoDepartamento).Id;
                    break;
                case 2:
                    idTipo = bllFuncionario.GetIdDsCodigo(fechamentoBH.DsCodigoFuncionario);
                    break;
                case 3:
                    idTipo = bllFuncao.LoadObjectByCodigo(fechamentoBH.CodigoFuncao).Id;
                    break;
            }
            return idTipo;
        }
    }
}