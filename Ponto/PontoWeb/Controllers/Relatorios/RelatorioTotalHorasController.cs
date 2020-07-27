using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Modelo.Proxy.Relatorios;
using Newtonsoft.Json;
using PontoWeb.Controllers.JobManager;
using Modelo.Relatorios;
using PontoWeb.Models.Helpers;
using BLL_N.JobManager.Hangfire;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioTotalHorasController : Controller
    {
        #region Métodos Get
        // GET: RelatorioAfastamento
        [PermissoesFiltro(Roles = "RelatorioTotalHorasConsultar")]
        public ActionResult Index()
        {
            RelatorioTotalHoras parms = new RelatorioTotalHoras();
            parms.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
            parms.FimPeriodo = DateTime.Now.Date;
            parms.TipoSelecao = 2;
            return View(parms);
        }
        #endregion

        #region Métodos Post
        [HttpPost]
        [PermissoesFiltro(Roles = "RelatorioTotalHorasConsultar")]
        public ActionResult Index(RelatorioTotalHoras param)
        {
            ValidarRelatorio(param);
            if (ModelState.IsValid)
            {
                try
                {
                    return GeraRelTotalHoras(param);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return ModelState.JsonErrorResult();
        }
        #endregion

        private ActionResult GeraRelTotalHoras(RelatorioTotalHoras parms)
        {
            try
            {
                Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
                HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
                Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioTotalHoras(parms);
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

        #region Validações
        private void ValidarRelatorio(RelatorioTotalHoras param)
        {
            if (String.IsNullOrEmpty(param.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }
        #endregion

        #region Teste/Geração HTML Relatório
        public ActionResult RelatorioTotalHorasHtml()
        {
            RelatorioTotalHoras parms = new RelatorioTotalHoras()
            {
                InicioPeriodo = Convert.ToDateTime("2019-03-01"),
                FimPeriodo = Convert.ToDateTime("2019-03-30"),
                IdSelecionados = "160"
            };
            //JobController jc = new JobController();
            //IList<PxyRelTotalHoras> totais = jc.GetTotalizadoresFuncionarios(parms, Usuario.GetUsuarioPontoWebLogadoCache());
            //string json = JsonConvert.SerializeObject(totais);
            IList<PxyRelTotalHoras> totais = JsonConvert.DeserializeObject<IList<PxyRelTotalHoras>>("[{\"FuncionarioDsCodigo\":\"375556\",\"FuncionarioNome\":\"Diego Herrera Gremes Okabayashi\",\"FuncionarioMatricula\":\"51\",\"HorasTrabDiurna\":\"156:06\",\"HorasTrabNoturna\":\"00:00\",\"HorasAdNoturno\":\"00:00\",\"HorasExtraDiurna\":\"00:00\",\"HorasExtraNoturna\":\"00:00\",\"HorasExtraInterjornada\":\"00:00\",\"HorasFaltaDiurna\":\"00:00\",\"HorasFaltaNoturna\":\"00:00\",\"HorasDDSR\":\"000:00\",\"LRateioHorasExtras\":[],\"CreditoBHPeriodoMin\":407,\"DebitoBHPeriodoMin\":1194,\"CreditoBHPeriodo\":\"06:47\",\"DebitoBHPeriodo\":\"19:54\",\"SinalSaldoBHAtual\":'+',\"SaldoAnteriorBH\":\"13:28\",\"SinalSaldoAnteriorBH\":'+',\"SaldoBHPeriodo\":\"13:07\",\"SinalSaldoBHPeriodo\":'-',\"SaldoBHAtual\":\"00:21\"}]");
            return View(totais);
        } 
        #endregion
    }
}