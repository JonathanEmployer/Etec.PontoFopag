using BLL_N.JobManager.Hangfire;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class MudCodigoFuncController : IControllerPontoWeb<MudCodigoFunc>
    {
        UsuarioPontoWeb _usr = Usuario.GetUsuarioPontoWebLogadoCache();

        [PermissoesFiltro(Roles = "MudCodigoFunc")]
        public override ActionResult Grid()
        {
            return View(new Modelo.MudCodigoFunc());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.MudCodigoFunc bllMudCodigoFunc = new BLL.MudCodigoFunc(_usr.ConnectionString, _usr);
                List<Modelo.MudCodigoFunc> dados = bllMudCodigoFunc.GetAllList();
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

        [PermissoesFiltro(Roles = "MudCodigoFuncConsultar")]
        [HttpPost]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "MudCodigoFuncCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "MudCodigoFuncAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "MudCodigoFuncCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(MudCodigoFunc obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "MudCodigoFuncAlterar")]
        [HttpPost]
        public override ActionResult Alterar(MudCodigoFunc obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "MudCodigoFuncExcluir")]
        public override ActionResult Excluir(int id)
        {
            return RedirectToAction("Index", "Home");
        }

        protected override ActionResult Salvar(MudCodigoFunc mudCodigoFunc)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Dictionary<string, string> erros = new Dictionary<string, string>();
            ValidarForm(mudCodigoFunc);
            if (ModelState.IsValid)
            {
                try
                {
                    BLL.MudCodigoFunc bllMudCodigoFunc = new BLL.MudCodigoFunc(_usr.ConnectionString, _usr);
                    BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usr.ConnectionString, _usr);
                    BLL.Provisorio bllProvisorio = new BLL.Provisorio(_usr.ConnectionString, _usr);

                    DateTime data = mudCodigoFunc.Datainicial.Value.Date;
                    Dictionary<string, string> ret = new Dictionary<string, string>();
                    Int64 codigo;
                    Int64.TryParse(mudCodigoFunc.DSCodigoNovo, out codigo);
                    //Verifica se existe algum bilhete importado depois da data da mudança

                    if (bllMudCodigoFunc.VerificaMarcacao(mudCodigoFunc.IdFuncionario, data))
                    {
                        
                        //Verifica se existe algum provisório no período
                        if (!bllProvisorio.ExisteProvisorio(mudCodigoFunc.DSCodigoNovo, data))
                        {
                            Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(mudCodigoFunc.IdFuncionario);
                            DateTime? ultimaData = bllMarcacao.GetUltimaDataFuncionario(mudCodigoFunc.IdFuncionario);
                            if (ultimaData == null || ultimaData == new DateTime())
                            {
                                //Cria as marcações que não existem desde a data de admissao do funcionario até a data da mudanca
                                bllMarcacao.AtualizaData(objFuncionario.Dataadmissao.Value, data.AddDays(-1), objFuncionario);
                            }
                            else
                            {
                                if (ultimaData < data)
                                {
                                    //Cria as marcações que não existem no período anterior à mudança de código
                                    bllMarcacao.AtualizaData(ultimaData.Value, data.AddDays(-1), objFuncionario);
                                }
                            }
                            if (bllFuncionario.MudaCodigoFuncionario(objFuncionario.Id, codigo.ToString(), data))
                            {
                                HangfireManagerCalculos hfm = new HangfireManagerCalculos(_usr.DataBase, "", "", "/MudCodigoFunc/Grid");
                                string parametrosExibicao = String.Format("Mudança de código do funcionário: {0} | {1}, do código: {2} para {3}", objFuncionario.Codigo, objFuncionario.Nome, mudCodigoFunc.DSCodigoAntigo, mudCodigoFunc.DSCodigoNovo);
                                hfm.RecalculaMarcacao("Mudança de código de funcionário", parametrosExibicao, new List<int>() { objFuncionario.Id}, data, DateTime.Today);
                            }
                        }
                        else
                        {
                            erros.Add("txtCodigo", "Existe um código provisório cadastrado no mesmo período da mudança de código. Escolha outro código.");
                        }
                    }
                    else
                    {
                        ModelState["Datainicial"].Errors.Add("Existe bilhete importado após a data selecionada, só é possível alterar a partir de uma data sem movimentação.");
                    }

                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                    }
                    if (ModelState.IsValid)
                    {
                        return RedirectToAction("Grid", "MudCodigoFunc");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Cadastrar", mudCodigoFunc);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.MudCodigoFunc bllMudCodigoFunc = new BLL.MudCodigoFunc(_usr.ConnectionString, _usr);
            MudCodigoFunc mudCodigoFunc = new MudCodigoFunc();
            mudCodigoFunc = bllMudCodigoFunc.LoadObject(id);
            if (id == 0)
            {
                mudCodigoFunc.Codigo = bllMudCodigoFunc.MaxCodigo();
            }
            return View("Cadastrar", mudCodigoFunc);
        }

        protected override void ValidarForm(MudCodigoFunc mudCodigoFunc)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_usr.ConnectionString, _usr);
            Funcionario objFuncionario = new Funcionario();
            int idFunc = FuncionarioController.BuscaIdFuncionario(mudCodigoFunc.NomeFuncionario);
            objFuncionario = bllFuncionario.LoadObject(idFunc);
            DateTime iniDataLim = new DateTime(1753, 01, 01);
            DateTime fimDataLim = new DateTime(9999, 12, 31);
            if ((mudCodigoFunc.Datainicial <= iniDataLim) ||
                (mudCodigoFunc.Datainicial >= fimDataLim))
            {
                ModelState["Datainicial"].Errors.Add("A Data informada deve estar entre 01/01/1753 e 31/12/999.");
            }
            else
            {
                if (idFunc > 0)
                {
                    mudCodigoFunc.DSCodigoAntigo = objFuncionario.Dscodigo;
                    mudCodigoFunc.IdFuncionario = idFunc;
                }
                else
                {
                    ModelState["NomeFuncionario"].Errors.Add("Funcionário " + mudCodigoFunc.NomeFuncionario + " não cadastrado!");
                }
            }
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            Dictionary<string, string> erroLocais = new Dictionary<string, string>();

            erroLocais = erros.Where(x => x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);
            if (erroLocais.Count > 0)
            {
                ModelState["DSCodigoNovo"].Errors.Add(string.Join(";", erroLocais.Select(x => x.Value).ToArray()));
            }

            erros = erros.Where(x => !x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);

            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }
    }
}