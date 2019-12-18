using BLL_N.JobManager.Hangfire;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioFuncionarioController : Controller
    {
        #region Métodos Get
        // GET: RelatorioAfastamento
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioFuncionarioConsultar")]
        public ActionResult Funcionario()
        {
           RelatorioFuncionarioModel viewModel = new RelatorioFuncionarioModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            viewModel.FimPeriodo = DateTime.Now.Date;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioFuncionarioConsultar")]
        [HttpPost]
        public ActionResult Funcionario(Modelo.Relatorios.RelatorioFuncionarioModel parms)
        {
            List<Modelo.Empresa> empresas = new List<Modelo.Empresa>();

            if (ModelState.ContainsKey("Relatorios"))
            {
                ModelState.Remove("Relatorios");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //return GeraRelFuncionario(obj);
                    Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioFuncionario(parms);

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
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
           
            return ModelState.JsonErrorResult();
        }

        private ActionResult GeraRelFuncionario(pxyRelFuncionario imp)
        {
            try
            {
                RelatoriosController rc = new RelatoriosController();
                JobController jc = new JobController();
                Job job = jc.GetRelatorioFuncionario(imp, Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                return Json(new
                {
                    JobId = job.Id,
                    Progress = job.Progress,
                    Erro = "",
                    AbrirNovaAba = true
                });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Métodos auxiliares
        private void ValidarRelatorio(pxyRelFuncionario imp, List<Modelo.Empresa> empresas, List<Modelo.Departamento> departamentos, List<Modelo.Funcionario> funcionarios)
        {
            switch (imp.TipoSelecao)
            {
                case 0:
                    if (empresas.Count == 0)
                    {
                        ModelState["TipoSelecao"].Errors.Add("Você deve selecionar pelo menos uma Empresa");
                    }
                    break;
                case 1:
                    if (departamentos.Count == 0)
                    {
                        ModelState["TipoSelecao"].Errors.Add("Você deve selecionar pelo menos um Departamento");
                    }
                    break;
                case 2:
                    if (funcionarios.Count == 0)
                    {
                        ModelState["TipoSelecao"].Errors.Add("Você deve selecionar pelo menos um Funcionário");
                    }
                    break;
                default:
                    break;
            }
        }

        private static void GerarListagensRelatorio(
            ref pxyRelFuncionario imp
            , out List<Modelo.Empresa> empresas
            , out List<Modelo.Departamento> departamentos
            , out List<Modelo.Funcionario> funcionarios
            , out List<Modelo.Horario> horarios)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            var Empresas = imp.Empresas != null ? imp.Empresas.Where(w => w.Selecionado) : new List<pxyEmpresa>();
            var Deptos = imp.Departamentos != null ? imp.Departamentos.Where(w => w.Selecionado) : new List<pxyDepartamento>();
            var Funcs = imp.Funcionarios != null ? imp.Funcionarios.Where(w => w.Selecionado) : new List<pxyFuncionario>();
            var Horarios = imp.Horarios != null ? imp.Horarios.Where(w => w.Selecionado) : new List<pxyHorario>();
            BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usr);

            funcionarios = new List<Modelo.Funcionario>();
            departamentos = new List<Modelo.Departamento>();
            empresas = new List<Modelo.Empresa>();
            horarios = new List<Modelo.Horario>();

            List<Modelo.Funcionario> listaFuncionarios = new BLL.Funcionario(conn, usr).GetAllList(true, true);
            List<Modelo.Departamento> listaDepartamentos = new BLL.Departamento(conn, usr).GetAllList();
            List<Modelo.Empresa> listaEmpresas = bllEmpresa.GetAllList();
            List<Modelo.Horario> listaHorarios = new BLL.Horario(conn, usr).GetAllList(false, false);

            var idEmpresas = Empresas.Select(s => s.Id);
            var idDepartamentos = Deptos.Where(w => idEmpresas.Contains(w.IdEmpresa)).Select(s => s.Id);
            var idHorarios = Horarios.Select(s => s.Id);

            horarios = listaHorarios.Where(w => idHorarios.Contains(w.Id)).ToList();
            switch (imp.TipoSelecao)
            {
                case 0:
                    if (Empresas != null)
                    {
                        foreach (var item in Empresas)
                        {
                            empresas.Add(listaEmpresas.FirstOrDefault(f => f.Id == item.Id));
                        }
                    }
                    break;
                case 1:
                    if (Empresas != null)
                    {
                        foreach (var item in Empresas)
                        {
                            empresas.Add(listaEmpresas.FirstOrDefault(f => f.Id == item.Id));
                        }
                    }
                    if (Deptos != null)
                    {
                        foreach (var item in Deptos)
                        {
                            Modelo.Departamento dep = listaDepartamentos.FirstOrDefault(f => f.Id == item.Id && idEmpresas.Contains(item.IdEmpresa));
                            if (dep != null)
                            {
                                departamentos.Add(dep);
                            }
                        }
                    }
                    break;
                case 2:
                    if (Empresas != null)
                    {
                        foreach (var item in Empresas)
                        {
                            empresas.Add(listaEmpresas.FirstOrDefault(f => f.Id == item.Id));
                        }
                    }
                    if (Deptos != null)
                    {
                        foreach (var item in Deptos)
                        {
                            Modelo.Departamento dep = listaDepartamentos.FirstOrDefault(f => f.Id == item.Id && idEmpresas.Contains(item.IdEmpresa));
                            if (dep != null)
                            {
                                departamentos.Add(dep); 
                            }
                        }
                    }
                    if (Funcs != null)
                    {
                        foreach (var item in Funcs)
                        {
                            Modelo.Funcionario fun = listaFuncionarios.FirstOrDefault(w => w.Id == item.Id && idDepartamentos.Contains(item.IdDepartamento));
                            if (fun != null)
                            {
                                funcionarios.Add(fun);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}