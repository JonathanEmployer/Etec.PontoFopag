using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Web.Mvc;
using PontoWeb.Models.Helpers;
using BLL_N.JobManager.Hangfire;

namespace PontoWeb.Controllers
{
    public class RecalculaMarcacaoController : Controller
    {
        [PermissoesFiltro(Roles = "RecalculaMarcacaoAlterar")]
        public ActionResult Recalcular()
        {
            return View(new RecalculaMarcacaoViewModel());
        }

        [PermissoesFiltro(Roles = "RecalculaMarcacaoAlterar")]
        [HttpPost]
        public ActionResult Recalcular(RecalculaMarcacaoViewModel obj)
        {
            try
            {
                ValidaRecalculo(obj);
                if (ModelState.IsValid)
                {
                    UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(UserPW.DataBase);
                    string parametrosExibicao = obj.ParametroTipo + ", Período " + obj.DataInicial.GetValueOrDefault().ToShortDateString() + " a " + obj.DataFinal.GetValueOrDefault().ToShortDateString();
                    Modelo.Proxy.PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de Marcações", parametrosExibicao, obj.Tipo, obj.Identificacao, obj.DataInicial.GetValueOrDefault(), obj.DataFinal.GetValueOrDefault());
                    return new JsonResult
                    {
                        Data = new
                        {
                            success = true,
                            job = ret
                        }
                    };
                    //return Json(ret, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    return ModelState.JsonErrorResult();
                }
            }
            catch (Exception ex)
            {
                Employer.PlataformaLog.LogError.WriteLog(ex);
                throw ex;
            }
        }

        private void ValidaRecalculo(RecalculaMarcacaoViewModel objeto)
        {
            #region Validação Lookups
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            switch (objeto.Tipo)
            {
                case 0:
                    if (String.IsNullOrEmpty(objeto.Empresa))
                    {
                        ModelState["Empresa"].Errors.Add("Selecione uma empresa.");
                    }
                    else
                    {
                        BLL.Empresa bllEmp = new BLL.Empresa(conn, usr);
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
                        BLL.Departamento blldep = new BLL.Departamento(conn, usr);
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
                case 3:
                    if (String.IsNullOrEmpty(objeto.Funcao))
                    {
                        ModelState["Funcao"].Errors.Add("Selecione uma função.");
                    }
                    else
                    {
                        BLL.Funcao bllfuncao = new BLL.Funcao(conn, usr);
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
                case 2:
                    if (String.IsNullOrEmpty(objeto.Funcionario))
                    {
                        ModelState["Funcionario"].Errors.Add("Selecione um funcionário.");
                    }
                    else
                    {
                        BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
                        int idFuncionario = 0;
                        string func = objeto.Funcionario.Split('|')[0].Trim();
                        idFuncionario = bllFuncionario.GetIdDsCodigo(func);
                        if (idFuncionario > 0)
                        {
                            objeto.Identificacao = idFuncionario;
                        }
                        else
                        {
                            ModelState["Funcionario"].Errors.Add("Funcionário " + func + " não cadastrado!");
                        }
                    }
                    break;
                default:
                    break;

            }

            #endregion

            if (objeto.DataInicial.HasValue && objeto.DataFinal.HasValue)
            {
                if (objeto.DataInicial.Value > objeto.DataFinal.Value)
                {
                    ModelState["DataFinal"].Errors.Add("A Data Final não pode ser menor que a Data Inicial.");
                }
            }
        }
    }
}