using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
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
    public class RelatorioInconsistenciasController : Controller
    {
        #region Métodos Get
        // GET: RelatorioAfastamento
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioInconsistenciasConsultar")]
        public ActionResult Inconsistencias()
        {
            RelatorioInconsistenciasModel viewModel = new RelatorioInconsistenciasModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 2;
            return View(viewModel);
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt));
                Modelo.UsuarioPontoWeb usuarioPW = Usuario.GetUsuarioPontoWebLogadoCache();
                List<Modelo.Proxy.Relatorios.PxyGridRelatorioInconsistencias> gridRelInconsistenciasl = bllRel.GetRelInconsistencias(usuarioPW);
                JsonResult jsonResult = Json(new { data = gridRelInconsistenciasl }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioInconsistenciasConsultar")]
        [HttpPost]
        public ActionResult Inconsistencias(RelatorioInconsistenciasModel imp)
        {
            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelInconsistencias(imp);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }

        /// <summary>
        /// Método para gerar o cartão ponto do Relatório de Cartão do Ponto Web (Necessário passar apenas uma lista de Funcionário)
        /// </summary>
        /// <returns></returns>
        private ActionResult GeraRelInconsistencias(RelatorioInconsistenciasModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioInconsistencias(parms);
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
            return ModelState.JsonErrorResult();
        }

        #endregion

        #region Métodos auxiliares
        private void ValidarRelatorio(RelatorioInconsistenciasModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
            if ((imp.FimPeriodo - imp.InicioPeriodo).Days > 31)
            {
                ModelState["FimPeriodo"].Errors.Add("O período não pode ser maior que 31 dias.");
            }
        }

        private static void GerarListagensRelatorio(
            ref pxyRelCartaoPonto imp
            , out List<Modelo.Empresa> empresas
            , out List<Modelo.Departamento> departamentos
            , out List<Modelo.Funcionario> funcionarios)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            var Empresas = imp.Empresas != null ? imp.Empresas.Where(w => w.Selecionado) : new List<pxyEmpresa>();
            var Deptos = imp.Departamentos != null ? imp.Departamentos.Where(w => w.Selecionado) : new List<pxyDepartamento>();
            var Funcs = imp.Funcionarios != null ? imp.Funcionarios.Where(w => w.Selecionado) : new List<pxyFuncionario>();
            BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usr);

            funcionarios = new List<Modelo.Funcionario>();
            departamentos = new List<Modelo.Departamento>();
            empresas = new List<Modelo.Empresa>();

            List<Modelo.Funcionario> listaFuncionarios = new BLL.Funcionario(conn, usr).GetAllList(true);
            List<Modelo.Departamento> listaDepartamentos = new BLL.Departamento(conn, usr).GetAllList();
            List<Modelo.Empresa> listaEmpresas = bllEmpresa.GetAllList();

            var idEmpresas = Empresas.Select(s => s.Id);
            var idDepartamentos = Deptos.Where(w => idEmpresas.Contains(w.IdEmpresa)).Select(s => s.Id);

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