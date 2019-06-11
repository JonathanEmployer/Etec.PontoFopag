using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using BLL.Util;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;
using RazorEngine;
using RazorEngine.Templating;

namespace BLL.Relatorios.V2
{
	public class RelatorioCartaoPontoCustomBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioCartaoPontoCustomBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioCartaoPontoHTMLModel parms = ((RelatorioCartaoPontoHTMLModel)relatorioFiltro);
			parms.NomeArquivo = "Relatorio_Cartao_Ponto_Customizável" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}
		protected override string GetRelatorioExcel()
		{
			throw new NotImplementedException();
		}

		private IList<pxyCartaoPontoEmployer> GetDados()
		{
			_progressBar.setaMensagem("Carregando dados...");
			_progressBar.setaValorPB(-1);
			BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(_usuario.ConnectionString, _usuario);
			RelatorioCartaoPontoHTMLModel parms = (RelatorioCartaoPontoHTMLModel)_relatorioFiltro;
			IList<int> funcs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

			IList<pxyCartaoPontoEmployer> cps = bllCartaoPonto.BuscaDadosRelatorio(funcs, parms.InicioPeriodo, parms.FimPeriodo, _progressBar, parms.OrdemRelatorio);

			PxyCPEMarcacao marc = new PxyCPEMarcacao();
			List<Modelo.Utils.CartaoPontoCamposParaCustomizacao> campos = GetPropertiesCartaoPontoCustom.GetProperties(marc.GetType());
			BLL.CamposSelecionadosRelCartaoPonto bllCamposSelecionadosRelCartaoPonto = new BLL.CamposSelecionadosRelCartaoPonto(_usuario.ConnectionString, _usuario);
			List<Modelo.CamposSelecionadosRelCartaoPonto> camposSelionadosCartao = bllCamposSelecionadosRelCartaoPonto.GetAllList();
			foreach (Modelo.CamposSelecionadosRelCartaoPonto item in camposSelionadosCartao)
			{
				item.PropriedadesCampo = campos.Where(c => c.NomePropriedade == item.PropriedadeModelo).FirstOrDefault();
			}
			cps.ToList().ForEach(f => f.CamposSelecionados = camposSelionadosCartao);

			return cps;
		}
		protected override string GetRelatorioHTML()
		{
			return GerarArquivoHTML(ParametrosHTML());
		}

		protected override string GetRelatorioPDF()
		{
			return GerarArquivoPdfBaseHTML(ParametrosHTML());
		}

		protected ParametrosReportHTML ParametrosHTML()
		{
			return new ParametrosReportHTML()
			{
				Dados = GetDados(),
				NomeArquivo = ((RelatorioCartaoPontoHTMLModel)_relatorioFiltro).NomeArquivo,
				ResourceName = "BLL.Relatorios.V2.cshtml.CartaoPontoHtmlCustom.cshtml"
			};
		}
	}
}
