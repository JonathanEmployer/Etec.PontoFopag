using BLL_N.JobManager.Hangfire;
using Modelo.Proxy;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioManutencaoDiariaController : Controller
    {
        private Modelo.UsuarioPontoWeb _userPW = Usuario.GetUsuarioPontoWebLogadoCache();
        #region Métodos Get
        // GET: RelatorioAfastamento
        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioManutencaoDiariaConsultar")]
        public ActionResult ManutencaoDiaria()
        {
            RelatorioManutencaoDiariaModel viewModel = new RelatorioManutencaoDiariaModel();
            viewModel.InicioPeriodo = DateTime.Now.Date;
            viewModel.FimPeriodo = DateTime.Now.Date;
            viewModel.TipoSelecao = 1;
            return View(viewModel);
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.RelatoriosPontoWeb bllRel = new BLL.RelatoriosPontoWeb(new DAL.SQL.DataBase(_userPW.ConnectionString));
                List<PxyGridRelatorioManutencaoDiaria> gridRelHorario = bllRel.GetListagemManutencaoDiariaRel(_userPW);
                JsonResult jsonResult = Json(new { data = gridRelHorario }, JsonRequestBehavior.AllowGet);
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
        [PermissoesFiltro(Roles = "RelatorioManutencaoDiariaConsultar")]
        [HttpPost]
        public ActionResult ManutencaoDiaria(RelatorioManutencaoDiariaModel parms)
        {
            parms.FimPeriodo = parms.InicioPeriodo;
            if (ModelState.IsValid)
            {
                try
                {
                    HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(_userPW.DataBase);
                    Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioManutencaoDiaria(parms);
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

        [Authorize]
        [PermissoesFiltro(Roles = "RelatorioManutencaoDiariaConsultar")]
        [HttpPost]
        public ActionResult ManutencaoDiariaParms(string empresa, string departamento, string contrato, string dataInicial, string dataFinal)
        {
            DateTime dtInicial, dtFinal;
            RelatorioManutencaoDiariaModel parms = new RelatorioManutencaoDiariaModel() { TipoArquivo = "PDF"};

            if ((DateTime.TryParse(dataInicial, out dtInicial)) && (DateTime.TryParse(dataFinal, out dtFinal)))
            {
                parms.InicioPeriodo = dtInicial.Date;
                parms.FimPeriodo = dtFinal.Date;
            }

            List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
            GerarListagensRelatorio(ref parms, empresa, departamento, contrato);

            return ManutencaoDiaria(parms);
        }

        #endregion

        #region Métodos auxiliares

        private void GerarListagensRelatorio(
            ref RelatorioManutencaoDiariaModel imp
            , string empresa
            , string departamento
            , string contrato)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(_userPW.ConnectionString, _userPW);
            int idEmpresa = 0, idDepartamento = 0, idContrato = 0;
            string erro = "";
            List<int> idsFuncs = new List<int>();
            if (bllFuncionario.ValidaEmpDepCont(empresa, departamento, contrato, ref idEmpresa, ref idDepartamento, ref idContrato, ref erro))
            {
                if (idDepartamento > 0)
                {
                    imp.TipoSelecao = 1;
                    imp.IdSelecionados = idDepartamento.ToString();
                } else if (idEmpresa > 0)
                {
                    imp.TipoSelecao = 0;
                    imp.IdSelecionados = idEmpresa.ToString();
                }
                else
                {
                    imp.TipoSelecao = 0;
                    BLL.Empresa bllEmpresa = new BLL.Empresa(_userPW.ConnectionString, _userPW);
                    List<int> idsEmpresas = bllEmpresa.GetAllIds();
                    imp.IdSelecionados = String.Join(",", idsEmpresas);
                }
            }
            else
            {
                ModelState.AddModelError("CustomError", erro);
            }
        }

        #endregion
    }
}