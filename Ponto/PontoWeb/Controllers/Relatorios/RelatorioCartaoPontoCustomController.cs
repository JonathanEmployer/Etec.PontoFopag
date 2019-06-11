using BLL.Util;
using BLL_N.IntegracaoTerceiro;
using BLL_N.JobManager.Hangfire;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Modelo;
using Modelo.Proxy;
using Modelo.Proxy.Relatorios;
using Modelo.Relatorios;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Controllers.JobManager;
using PontoWeb.Models;
using PontoWeb.Models.Helpers;
using PontoWeb.Security;
using PontoWeb.Utils;
using ProgressReporting.Controllers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers.Relatorios
{
    public class RelatorioCartaoPontoCustomController : Controller
    {
        #region Métodos Get
		public ActionResult CartaoPontoHtmlCustom()
		{
			string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
			UsuarioPontoWeb pw = Usuario.GetUsuarioPontoWebLogadoCache();
			BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(conn, pw);
			DateTime di = Convert.ToDateTime("2019-02-09");
			DateTime df = Convert.ToDateTime("2019-03-11");
			IList<int> funcs = new List<int>() { 40498 };
			IList<pxyCartaoPontoEmployer> cps = bllCartaoPonto.BuscaDadosRelatorio(funcs, di, df, null, 0);

			PxyCPEMarcacao marc = new PxyCPEMarcacao();
			List<Modelo.Utils.CartaoPontoCamposParaCustomizacao> campos =  GetPropertiesCartaoPontoCustom.GetProperties(marc.GetType());
			BLL.CamposSelecionadosRelCartaoPonto bllCamposSelecionadosRelCartaoPonto = new BLL.CamposSelecionadosRelCartaoPonto(conn, pw);
			List<Modelo.CamposSelecionadosRelCartaoPonto> camposSelionadosCartao = bllCamposSelecionadosRelCartaoPonto.GetAllList();
			foreach (Modelo.CamposSelecionadosRelCartaoPonto item in camposSelionadosCartao)
			{
				item.PropriedadesCampo = campos.Where(c => c.NomePropriedade == item.PropriedadeModelo).FirstOrDefault();
			}
			cps.ToList().ForEach(f => f.CamposSelecionados = camposSelionadosCartao);

			return View(cps.ToList());
		}
		#endregion


		#region Métodos Get
		// GET: RelatorioAfastamento
		[PermissoesFiltro(Roles = "RelatorioCartaoPontoCustomConsultar")]
        public ActionResult Index()
        {
			RelatorioCartaoPontoHTMLModel viewModel = new RelatorioCartaoPontoHTMLModel();
			viewModel.InicioPeriodo = DateTime.Now.Date.AddDays(-30);
			viewModel.FimPeriodo = DateTime.Now.Date;
			viewModel.TipoSelecao = 2;
			return View(viewModel);
		}
        #endregion
        #region Métodos Post
        [HttpPost]
        [PermissoesFiltro(Roles = "RelatorioCartaoPontoCustomConsultar")]
        public ActionResult Index(RelatorioCartaoPontoHTMLModel imp)
        {
			ValidarRelatorio(imp);
			if (ModelState.IsValid)
			{
				try
				{
					return GeraRelCartaoPonto(imp);
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

        private byte[] Impressao(DateTime dataIni, DateTime dataFin, IList<int> funcs)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb pw = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(conn, pw);
            IList<pxyCartaoPontoEmployer> cps = bllCartaoPonto.BuscaDadosRelatorio(funcs, dataIni, dataFin, null, 0);


            ConcurrentBag<RelatorioParts> cartoes = new ConcurrentBag<RelatorioParts>();

            int partes = cps.Count();
            if (partes >= 3)
            {
                partes = cps.Count() / 3;
            }
            IList<List<Modelo.Proxy.pxyCartaoPontoEmployer>> cpParciais = BLL.cwkFuncoes.SplitList(cps, partes);
            HtmlReport htmlReport = new HtmlReport();

            IList<string> htmls = new List<string>();
            foreach (List<Modelo.Proxy.pxyCartaoPontoEmployer> cpi in cpParciais)
            {
                var controllerContext=Request.RequestContext;
                string htmlText = htmlReport.RenderViewToString(this.ControllerContext, "Index", cpi);
                htmls.Add(htmlText);
            }

            int part = 0;
            Parallel.ForEach(htmls, (ht) =>
            {
                part++;
                RelatorioParts cpb = new RelatorioParts();
                cpb.Parte = part;
                byte[] buffer = htmlReport.RenderPDF(ht, false, false);
                cpb.Arquivo = buffer;
                cartoes.Add(cpb);
            });

            byte[] buffer1 = htmlReport.MergeFiles(cartoes.OrderBy(o => o.Parte).Select(s => s.Arquivo).ToList(), true, false);
            return buffer1;
        }

        private void ValidarRelatorio(RelatorioCartaoPontoHTMLModel imp)
        {
            if (String.IsNullOrEmpty(imp.IdSelecionados))
            {
                ModelState.AddModelError("CustomError", "Você deve selecionar pelo menos um Funcionário");
            }
        }

        private ActionResult GeraRelCartaoPonto(RelatorioCartaoPontoHTMLModel parms)
        {
            try
            {
				Modelo.UsuarioPontoWeb UserPW = Usuario.GetUsuarioPontoWebLogadoCache();
				HangfireManagerRelatorios hfm = new HangfireManagerRelatorios(UserPW.DataBase);
				Modelo.Proxy.PxyJobReturn ret = hfm.RelatorioCartaoPontoCustom(parms);
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
    }

}