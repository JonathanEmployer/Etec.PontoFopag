using BLL;
using BLL_N.JobManager.Hangfire;
using DAL.SQL;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioHoraExtraController : Controller
    {
        #region Métodos Get

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHoraExtraConsultar")]
        public ActionResult HoraExtra()
        {
            RelatorioHoraExtraModel viewModel = new RelatorioHoraExtraModel();
            viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-31);
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 2;
            return View(viewModel);
        }
        #endregion

        #region Métodos Post
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioHoraExtraConsultar")]
        [HttpPost]
        public ActionResult HoraExtra(RelatorioHoraExtraModel imp)
        {
            List<Modelo.Empresa> empresas = new List<Modelo.Empresa>();
            List<Modelo.Departamento> departamentos = new List<Modelo.Departamento>();
            List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();

            GerarListagensRelatorio(ref imp, out empresas);

            ValidarRelatorio(imp);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelHoraExtra(imp);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }

        private ActionResult GeraRelHoraExtra(RelatorioHoraExtraModel parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioHoraExtra(parms);
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
        private void ValidarRelatorio(RelatorioHoraExtraModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        private static void GerarListagensRelatorio(
            ref RelatorioHoraExtraModel imp
            , out List<Modelo.Empresa> empresas)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Empresa bllEmpresa = new BLL.Empresa(conn, usr);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);

            empresas = new List<Modelo.Empresa>();

            List<Modelo.Empresa> listaEmpresas = bllEmpresa.GetAllList();
            List<Modelo.Funcionario> listaFuncionarios = bllFuncionario.GetAllList(false);

            IList<string> idFuncionariosSelecionados = imp.IdSelecionados.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            IList<Modelo.Funcionario> funcionariosSelecionados = listaFuncionarios.Where(f => idFuncionariosSelecionados.Contains(f.Id.ToString())).ToList();

            IList<int> idEmpresaSelecionados = new List<int>();

            if (funcionariosSelecionados != null && funcionariosSelecionados.Count > 0)
            {
                idEmpresaSelecionados = funcionariosSelecionados.GroupBy(s => s.Idempresa).Select(p => p.First().Idempresa).ToList();

                empresas = listaEmpresas.Where(f => idEmpresaSelecionados.Contains(f.Id)).ToList();
            }
        }

        #endregion
	}
}